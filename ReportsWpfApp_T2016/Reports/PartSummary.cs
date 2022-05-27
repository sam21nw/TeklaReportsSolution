using AppExtensions;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tekla.Structures.Model;

using AppExtensions;
using PartProperties;

namespace Reports
{
  internal static class PartSummary
  {
    public static List<PartsSummaryModel> GetPartsSummaryList(this List<Part> parts)
    {
      var combinedMainPartProperties = new List<MainPartProperties>();
      var partsSummary = new List<PartsSummaryModel>();
      PartsSummaryModel psm = new PartsSummaryModel();

      var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

      Parallel.ForEach(parts, options, part =>
      {
        MainPartSchedule.GetMainPartReportProperties(part, combinedMainPartProperties);
      });

      var summary = combinedMainPartProperties.Where(x => x != null).GroupBy(x => x.Name).Select(x => new MainPartProperties
      {
        Name = x.Key,
        Quantity = x.Count(),
        Length = x.Sum(t => t.Length),
        Area = x.Sum(t => t.Area),
        Weight = x.Sum(t => t.Weight),
        TP_Length = x.Sum(t => t.TP_Length),
        NS_Length = x.Sum(t => t.NS_Length),
        BB_Length = x.Sum(t => t.BB_Length),
        CHQ_Area = x.Sum(c => c.CHQ_Area)
      }).ToList();

      if (summary != null)
      {
        foreach (var item in summary)
        {
          var combQty = item.Quantity.ToString();
          var combLength = (item.Length / 1000).ToString("0.### m");
          var combLengthImp = item.Length.MMtoFeetInches();
          var combArea = item.Area.ToString("0.### m2");
          var combAreaImp = item.Area.M2toSqFt().ToString("0.### ft2");
          var combWeight = item.Weight.ToString("0.### kg");
          var combWeightImp = (item.Weight * 2.20462).ToString("0.### lbs");
          var combTpLength = (item.TP_Length / 1000).ToString("0.### m");
          var combTpLengthImp = item.TP_Length.MMtoFeetInches();
          var combBbLength = (item.BB_Length / 1000).ToString("0.### m");
          var combBbLengthImp = item.BB_Length.MMtoFeetInches();
          var combNsLength = (item.NS_Length / 1000).ToString("0.### m");
          var combNsLengthImp = item.NS_Length.MMtoFeetInches();
          var combChqArea = item.CHQ_Area.ToString("0.### m2");
          var combChqAreaImp = item.CHQ_Area.M2toSqFt().ToString("0.### ft2");

          if (item.Name.Contains("GR") || item.Name.Contains("CHEQ") || item.Name.Contains("PL") || item.Name.Contains("CPL"))
          {
            partsSummary.Add(new PartsSummaryModel
            {
              Name = item.Name,
              Quantity = combQty,
              Length = null,
              LengthImp = null,
              Area = combArea,
              AreaImp = combAreaImp,
              Weight = combWeight,
              WeightImp = combWeightImp,
              TPLength = combTpLength,
              TPLengthImp = combTpLengthImp,
              NSLength = combNsLength,
              NSLengthImp = combNsLengthImp,
              BBLength = combBbLength,
              BBLengthImp = combBbLengthImp,
              CHQArea = combChqArea,
              CHQAreaImp = combChqAreaImp,
            });
          }
          else
          {
            partsSummary.Add(new PartsSummaryModel
            {
              Name = item.Name,
              Quantity = combQty,
              Length = combLength,
              LengthImp = combLengthImp,
              Area = null,
              AreaImp = null,
              Weight = combWeight,
              WeightImp = combWeightImp,
              TPLength = null,
              TPLengthImp = null,
              NSLength = null,
              NSLengthImp = null,
              BBLength = null,
              BBLengthImp = null,
              CHQArea = null,
              CHQAreaImp = null,
            });
          }
        }
      }
      return partsSummary;
    }
  }
}
