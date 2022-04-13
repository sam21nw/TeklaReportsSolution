using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Tekla.Structures.Model;

using AppExtensions;

namespace TeklaReportsApp
{
  internal static class StiffenerAnglesCombinedReport
  {
    public static void GetSACombinedReport(this List<Part> stiffAngles)
    {
      var mainPartsSchedule = new MainPartsSchedule();
      var combinedSAProperties = new List<StiffenerAngleProperties>();

      var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

      if (stiffAngles.Count == 0)
      {
        return;
      }

      Parallel.ForEach(stiffAngles, options, sa =>
      {
        mainPartsSchedule.GetSAReportProperties(sa, combinedSAProperties);
      });

      var summary = combinedSAProperties.GroupBy(x => x.ID).Select(x => new StiffenerAngleProperties
      {
        Quantity = x.Count(),
        Span = x.Sum(t => t.Span),
      }).ToList();

      var combLength = (summary.SingleOrDefault().Span / 1000).ToString("0.### m");
      var combLengthImp = (summary.SingleOrDefault().Span).MMtoFeetInches();

      ColorConsole.WriteLine("Selected Stiffener Angles Summary", ConsoleColor.Cyan);
      Console.Write("Quantity:".PadRight(36));
      ColorConsole.WriteLine($" {summary.First().Quantity} No:s".PadRight(32), ConsoleColor.Yellow);
      Console.Write("Total Length:".PadRight(36));
      ColorConsole.WriteLine($" {combLength} ({combLengthImp})".PadRight(32), ConsoleColor.Yellow);
      Console.WriteLine();
    }
  }
}