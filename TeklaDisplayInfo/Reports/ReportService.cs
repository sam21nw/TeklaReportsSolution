using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model.Operations;

using Tekla.Structures.Model;

namespace TeklaDisplayInfo
{
  public static class ReportService
  {
    public static string GetSelObjectsReport(this List<ModelObject> selObjects, List<ModelObjectProperties> MainPartList)
    {
      selObjects = SelectedModelParts.GetSelectedObjectsinModel();

      var ModelObjectProperties = new List<ModelObjectProperties>();

      var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

      Parallel.ForEach(selObjects, options, selObj =>
      {
        if (selObj != null)
          ModelObjectsSchedule.GetObjectReportProperties(selObj, ModelObjectProperties);
        return;
      });
    }

    public static List<ModelObjectProperties> GetPartsCombinedReport(this List<Part> mainParts)
    {
      var ModelObjectProperties = new List<ModelObjectProperties>();

      var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

      Parallel.ForEach(mainParts, options, mainPart =>
      {
        if (mainPart != null)
          ModelObjectsSchedule.GetObjectReportProperties(mainPart, ModelObjectProperties);
        return;
      });

      List<ModelObjectProperties> mainPartReportList = ModelObjectProperties.Where(x => x != null).GroupBy(x => x.AssemblyPos).Select(x => new ModelObjectProperties
      {
        AssemblyPos = x.FirstOrDefault().AssemblyPos,
        PartPos = x.FirstOrDefault().PartPos,
        Quantity = x.Count(),
        Length_Imperial = x.FirstOrDefault().Length_Imperial,
        Width_Imperial = x.FirstOrDefault().Width_Imperial,
        Length = x.FirstOrDefault().Length,
        Width = x.FirstOrDefault().Width,
        CutArea = x.FirstOrDefault().CutArea,
        Area = x.FirstOrDefault().Area,
        Weight = x.FirstOrDefault().Weight,
        TP_Length = x.Sum(t => t.TP_Length),
        BB_Length = x.Sum(t => t.BB_Length),
        NS_Length = x.Sum(t => t.NS_Length),
        CHQ_Area = x.Sum(c => c.CHQ_Area),
        UserPhase = x.FirstOrDefault().UserPhase,
        UserField4 = x.FirstOrDefault().UserField4,
        UserField3 = x.FirstOrDefault().UserField3,
        UserField2 = x.FirstOrDefault().UserField2,
        UserField1 = x.FirstOrDefault().UserField1,
        Top_Level = x.FirstOrDefault().Top_Level,
        Position = x.FirstOrDefault().Position,
        ID = x.FirstOrDefault().ID,
      }).OrderBy(p => p.UserField4)
      .ThenBy(p => p.UserField3)
      .ThenBy(p => p.UserField2)
      .ThenBy(p => p.UserField1)
      .ThenBy(p => p.AssemblyPos, new AlphanumComparator())
      .ToList();

      return mainPartReportList;
    }
  }
}
