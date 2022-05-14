using System.Linq;
using System.Collections.Generic;

using Tekla.Structures.Model;

namespace TeklaReportsApp
{
  public static class ModelParts
  {
    public static List<Part> GetGratingParts(this List<ModelObject> modelObjects)
    {
      var gratings = modelObjects.AsParallel().OfType<Part>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;
        string assPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);
        p.GetReportProperty("ASS_PREFIX", ref assPrefix);

        return name.ToUpper().Contains("GRATING") || partPrefix.ToUpper().Contains("GR") || partPrefix.ToUpper().Contains("CG") || assPrefix.ToUpper().Contains("CG") && !name.ToUpper().Contains("DUM");
      }).ToList();
      return gratings;
    }
    public static List<Assembly> GetGratingAssParts(this List<ModelObject> modelObjects)
    {
      var gratingsAssembly = modelObjects.AsParallel().OfType<Assembly>().Where(p =>
      {
        string name = string.Empty;
        string assPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("ASSEMBLY_PREFIX", ref assPrefix);

        return name.ToUpper().Contains("GRAT") || assPrefix.ToUpper().Contains("GR") || assPrefix.ToUpper().Contains("CG") && !name.ToUpper().Contains("DUM");
      }).ToList();
      return gratingsAssembly;
    }
    public static List<Part> GetStiffenerAngleParts(this List<ModelObject> modelObjects)
    {
      var stiffenerAngles = modelObjects.AsParallel().OfType<Part>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;
        string assPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);
        p.GetReportProperty("ASS_PREFIX", ref assPrefix);

        return name.ToUpper().Contains("STIFFENER_ANGLE") || assPrefix.ToUpper().Contains("SA") && !name.ToUpper().Contains("DUM");
      }).ToList();
      return stiffenerAngles;
    }
    public static List<Part> GetCplParts(this List<ModelObject> modelObjects)
    {
      var cpls = modelObjects.AsParallel().OfType<Part>().Where(p =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;

        p.GetReportProperty("NAME", ref name);
        p.GetReportProperty("PART_PREFIX", ref partPrefix);

        return name.ToUpper().Contains("CHEQUERED_PLATE") || name.ToUpper().Contains("CPLT") || partPrefix.ToUpper().Contains("CHQ") || partPrefix.ToUpper().Contains("CPL") && !name.ToUpper().Contains("DUM");
      }).ToList();
      return cpls;
    }
  }
}
