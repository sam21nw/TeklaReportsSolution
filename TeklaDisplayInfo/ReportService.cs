using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

using Tekla.Structures.Model;

namespace TeklaDisplayInfo
{
  public static class ReportService
  {
    public static void GetReport(this ModelObject obj)
    {
      var combinedObjectProperties = new List<ModelObjectProperties>();

      if (obj.IsGrating())
      {
        ModelObjectsSchedule.GetGratingReportProperties(obj, combinedObjectProperties);
      }
    }

    private static bool IsGrating(this ModelObject mo)
    {
      bool isGrating = false;

      var name = string.Empty;
      var assPos = string.Empty;
      var partPos = string.Empty;

      mo.GetReportProperty("NAME", ref name);
      mo.GetReportProperty("ASSEMBLY_POS", ref assPos);
      mo.GetReportProperty("PART_POS", ref partPos);

      if (name.ToUpper().Contains("GRAT") || assPos.ToUpper().Contains("GR") || partPos.ToUpper().Contains("GR") || !name.ToUpper().Contains("DUM"))
      {
        isGrating = true;
      }
      return isGrating;
    }
  }
}
