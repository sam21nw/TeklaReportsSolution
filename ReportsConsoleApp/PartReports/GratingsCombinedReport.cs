using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Tekla.Structures.Model;

using AppExtensions;

namespace TeklaReportsApp
{
  internal static class GratingsCombinedReport
  {
    public static void GetGratingsCombinedReport(this List<Part> gratings)
    {
      var mainPartsSchedule = new MainPartsSchedule();
      var combinedGratingProperties = new List<GratingProperties>();

      var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

      if (gratings.Count == 0)
      {
        return;
      }

      Parallel.ForEach(gratings, options, grating =>
      {
        mainPartsSchedule.GetGratingReportProperties(grating, combinedGratingProperties);
      });

      var summary = combinedGratingProperties.GroupBy(x => x.ID).Select(x => new GratingProperties
      {
        Quantity = x.Count(),
        Area = x.Sum(t => t.Area),
        WHArea = x.Sum(t => t.WHArea),
        TpLength = x.Sum(t => t.TpLength),
        BbLength = x.Sum(t => t.BbLength),
        NsLength = x.Sum(t => t.NsLength),
        ChqArea = x.Sum(c => c.ChqArea)
      }).ToList();

      var combArea = (summary.SingleOrDefault().Area).ToString("0.### m2");
      var combAreaImp = (summary.SingleOrDefault().Area).M2toSqFt().ToString("0.### ft2");
      var combWHArea = (summary.SingleOrDefault().WHArea).ToString("0.### m2");
      var combWHAreaImp = (summary.SingleOrDefault().WHArea).M2toSqFt().ToString("0.### ft2");
      var combTpLength = (summary.SingleOrDefault().TpLength / 1000).ToString("0.### m");
      var combTpLengthImp = summary.SingleOrDefault().TpLength.MMtoFeetInches();
      var combBbLength = (summary.SingleOrDefault().BbLength / 1000).ToString("0.### m");
      var combBbLengthImp = summary.SingleOrDefault().BbLength.MMtoFeetInches();
      var combNsLength = (summary.SingleOrDefault().NsLength / 1000).ToString("0.### m");
      var combNsLengthImp = summary.SingleOrDefault().NsLength.MMtoFeetInches();
      var combChqArea = (summary.SingleOrDefault().ChqArea).ToString("0.### m2");
      var combChqAreaImp = (summary.SingleOrDefault().ChqArea).M2toSqFt().ToString("0.### ft2");

      Console.WriteLine();
      ColorConsole.WriteLine("Selected Gratings Summary", ConsoleColor.Cyan);
      Console.Write("Quantity:".PadRight(36));
      ColorConsole.WriteLine($" {summary.First().Quantity} No:s".PadRight(32), ConsoleColor.Yellow);
      Console.Write("Gross Area:".PadRight(36));
      ColorConsole.WriteLine($" {combArea} ({combAreaImp})".PadRight(32), ConsoleColor.Yellow);
      Console.Write("W/H Area (cut limit:- 0.2 m2):".PadRight(36));
      ColorConsole.WriteLine($" {combWHArea} ({combWHAreaImp})".PadRight(32), ConsoleColor.Yellow);
      Console.Write("Total TP length:".PadRight(36));
      ColorConsole.WriteLine($" {combTpLength} ({combTpLengthImp})".PadRight(32), ConsoleColor.Yellow);
      Console.Write("Total BB length:".PadRight(36));
      ColorConsole.WriteLine($" {combBbLength} ({combBbLengthImp})".PadRight(32), ConsoleColor.Yellow);
      Console.Write("Total NS length:".PadRight(36));
      ColorConsole.WriteLine($" {combNsLength} ({combNsLengthImp})".PadRight(32), ConsoleColor.Yellow);
      if (summary.SingleOrDefault().ChqArea != 0)
      {
        Console.Write("Total CHQ Area (gross):".PadRight(36));
        ColorConsole.WriteLine($" {combChqArea} ({combChqAreaImp})".PadRight(32), ConsoleColor.Yellow);
      }
      Console.WriteLine();
    }
  }
}