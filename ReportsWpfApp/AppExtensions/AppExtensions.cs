using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using System.Data;

using Tekla.Structures;
using Tekla.Structures.Model;

using TSMUI = Tekla.Structures.Model.UI;

namespace AppExtensions
{
  public static class Extensions
  {
    /// <summary>
    /// Enumerate through objects in model
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public static List<ModelObject> ToList(this ModelObjectEnumerator enumerator)
    {
      enumerator.SelectInstances = false;
      var modelObjects = new List<ModelObject>();
      while (enumerator.MoveNext())
      {
        var modelObject = enumerator.Current;
        if (modelObject == null) continue;
        modelObjects.Add(modelObject);
      }

      return modelObjects;
    }

    /// <summary>
    /// Enumerate through objects in model
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public static List<T> ToAList<T>(this IEnumerator enumerator)
    {
      var list = new List<T>();
      while (enumerator.MoveNext())
      {
        var current = enumerator.Current;
        if (!(current is T)) continue;

        list.Add((T)current);
      }
      return list;
    }

    /// <summary>
    /// Returns if imperial units are being used
    /// </summary>
    public static bool IsImperial(this Model model)
    {
      if (model is null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      var stringTemp = string.Empty;
      TeklaStructuresSettings.GetAdvancedOption("XS_IMPERIAL", ref stringTemp);
      if (!string.IsNullOrEmpty(stringTemp)) return true;
      return string.CompareOrdinal(stringTemp, "1") == 0;
    }

    /// <summary>
    /// Returns if tekla model is connected
    /// </summary>
    public static bool IsConnected(this Model model)
    {
      if (!model.GetConnectionStatus()) return false;
      return true;
    }

    public static List<Part> GetPartsInAssembly(this Assembly assembly, bool includeMainPart = true)
    {
      if (assembly.GetMainPart() is Part mainPart)
      {
        var assemblyParts = new List<Part>();

        if (includeMainPart) assemblyParts.Add(mainPart);

        foreach (var secondary in assembly.GetSecondaries())
        {
          if (secondary is Part part)
          {
            assemblyParts.Add(part);
          }
        }

        return assemblyParts;
      }
      else
      {
        Debug.WriteLine($"WARNING: Assembly {assembly.Identifier.ID} has no main part");
        return new List<Part>();
      }
    }
    public static ModelObjectEnumerator GetSelectedObjectsinModel(this Model _model)
    {
      if (_model is null)
      {
        throw new ArgumentNullException(nameof(_model));
      }

      ModelObjectEnumerator.AutoFetch = true;

      TSMUI.ModelObjectSelector selector = new TSMUI.ModelObjectSelector();
      return selector.GetSelectedObjects();
    }

    public static List<Assembly> GetGratingAssembly(this List<ModelObject> modelObjects)
    {
      var gratings = modelObjects.AsParallel().OfType<Assembly>().Where(p =>
      {
        string name = string.Empty;
        string assPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("ASSEMBLY_PREFIX", ref assPrefix);

        return name.ToUpper().Contains("GRAT") || assPrefix.ToUpper().Contains("GR") && !name.ToUpper().Contains("DUM");

      }).ToList();
      return gratings;
    }
  }
}
