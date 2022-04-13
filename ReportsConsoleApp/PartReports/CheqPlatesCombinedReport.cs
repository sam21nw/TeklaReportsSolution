using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Tekla.Structures.Model;

using AppExtensions;

namespace TeklaReportsApp
{
  internal static class CheqPlatesCombinedReport
  {
    public static void GetCplCombinedReport(this List<Part> cpls)
    {
      var mainPartsSchedule = new MainPartsSchedule();
      var combinedCplProperties = new List<ChequeredPlateProperties>();

      var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

      if (cpls.Count == 0)
      {
        return;
      }
      Parallel.ForEach(cpls, options, cpl =>
      {
        mainPartsSchedule.GetCplReportProperties(cpl, combinedCplProperties);
      });

      var summary = combinedCplProperties.GroupBy(x => x.ID).Select(x => new ChequeredPlateProperties
      {
        Quantity = x.Count(),
        Area = x.Sum(t => t.Area),
      }).ToList();

      var combArea = (summary.SingleOrDefault().Area).ToString("0.### m2");
      var combAreaImp = (summary.SingleOrDefault().Area).M2toSqFt().ToString("0.### ft2"); ;

      ColorConsole.WriteLine("Selected Chequered Plates Summary", ConsoleColor.Cyan);
      Console.Write("Quantity:".PadRight(36));
      ColorConsole.WriteLine($" {summary.First().Quantity} No:s".PadRight(32), ConsoleColor.Yellow);
      Console.Write("Total Area:".PadRight(36));
      ColorConsole.WriteLine($" {combArea} ({combAreaImp})".PadRight(32), ConsoleColor.Yellow);
      Console.WriteLine();
    }

  }
}