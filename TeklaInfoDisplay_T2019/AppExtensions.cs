
using System.Collections;
using System.Collections.Generic;

using Tekla.Structures;
using Tekla.Structures.Model;

namespace TeklaInfoDisplay
{
  public static class AppExtensions
  {
    /// <summary>
    /// Returns if imperial units are being used
    /// </summary>
    public static bool IsImperial(this Model model)
    {
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

    /// <summary>
    /// Enumerate through objects in model
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public static List<ModelObject> ToList(this ModelObjectEnumerator enumerator, bool selectInstance = false)
    {
      enumerator.SelectInstances = selectInstance;
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
  }
}
