
using System.Collections.Generic;
using System.Linq;

using Tekla.Structures.Model;

using TeklaInfoDisplay;

using TSMUI = Tekla.Structures.Model.UI;

namespace TeklaInfoDisplay.Models
{
  public static class ModelParts
  {
    public static List<ModelObject> GetSelectedObjectsinModel()
    {
      ModelObjectEnumerator.AutoFetch = true;

      TSMUI.ModelObjectSelector selector = new TSMUI.ModelObjectSelector();
      return selector.GetSelectedObjects().ToList();
    }
    public static List<Beam> GetGratingParts(this List<ModelObject> modelObjects)
    {
      var gratings = modelObjects.AsParallel().OfType<Beam>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("GRAT") || partPrefix.ToUpper().Contains("GR") && !name.ToUpper().Contains("DUM");

      }).ToList();
      return gratings;
    }
    public static List<Beam> GetCPLParts(this List<ModelObject> modelObjects)
    {
      var chqpls = modelObjects.AsParallel().OfType<Beam>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("CHEQ") || partPrefix.ToUpper().Contains("CHQ") || partPrefix.ToUpper().Contains("CPL") && !name.ToUpper().Contains("DUM");

      }).ToList();
      return chqpls;
    }
    public static List<Assembly> GetGratingAssemblies(this List<ModelObject> modelObjects)
    {
      var gratingAss = modelObjects.AsParallel().OfType<Assembly>().Where(p =>
      {
        string name = string.Empty;
        string assPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("ASSEMBLY_PREFIX", ref assPrefix);

        return name.ToUpper().Contains("GRAT") || assPrefix.ToUpper().Contains("GR") || assPrefix.ToUpper().Contains("CG") && !name.ToUpper().Contains("DUM");

      }).ToList();
      return gratingAss;
    }

    public static List<ModelObject> GetBeams(this List<ModelObject> modelObjects)
    {
      var beams = modelObjects.AsParallel().OfType<ModelObject>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("BE") || partPrefix.ToUpper().Contains("UP") || partPrefix.ToUpper().Contains("JO") || name.ToUpper().Contains("STR") || name.ToUpper().Contains("BRACE") || name.ToUpper().Contains("FRAME") || name.ToUpper().Contains("CH") || name.ToUpper().Contains("ANG") || name.ToUpper().Contains("FITT");
      }).ToList();

      return beams;
    }
    public static List<ModelObject> GetColumns(this List<ModelObject> modelObjects)
    {
      var columns = modelObjects.AsParallel().OfType<ModelObject>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("COL") || partPrefix.ToUpper().Contains("CO");
      }).ToList();

      return columns;
    }
    public static List<Beam> GetOpenings(this List<ModelObject> modelObjects)
    {
      var openings = modelObjects.AsParallel().OfType<Beam>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("OP") || partPrefix.ToUpper().Contains("OP") && !name.ToUpper().Contains("DUM");
      }).ToList();

      return openings;
    }

    public static List<ModelObject> GetToePlates(this List<ModelObject> modelObjects)
    {
      var toePlates = modelObjects.AsParallel().OfType<ModelObject>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("TOE") || partPrefix.ToUpper().Contains("TP") && !name.ToUpper().Contains("DUM");
      }).ToList();

      return toePlates;
    }

    public static List<ModelObject> GetBindingBars(this List<ModelObject> modelObjects)
    {
      var bindingBars = modelObjects.AsParallel().OfType<ModelObject>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("BIND") || partPrefix.ToUpper().Contains("BB") && !name.ToUpper().Contains("DUM");
      }).ToList();

      return bindingBars;
    }

    public static List<ModelObject> GetNosingPlates(this List<ModelObject> modelObjects)
    {
      var nosingPlates = modelObjects.AsParallel().OfType<ModelObject>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("NOSING") || partPrefix.ToUpper().Contains("NS") && !name.ToUpper().Contains("DUM");
      }).ToList();

      return nosingPlates;
    }

    public static List<ModelObject> GetStiffenerAngles(this List<ModelObject> modelObjects)
    {
      var stiffenerAngles = modelObjects.AsParallel().OfType<ModelObject>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("STIFFENER") || partPrefix.ToUpper().Contains("SA") && !name.ToUpper().Contains("DUM");
      }).ToList();

      return stiffenerAngles;
    }
    public static List<ModelObject> GetStairTreads(this List<ModelObject> modelObjects)
    {
      var treads = modelObjects.AsParallel().OfType<ModelObject>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("TREAD") || partPrefix.ToUpper().Contains("TR") && !name.ToUpper().Contains("DUM");
      }).ToList();

      return treads;
    }
  }
}
