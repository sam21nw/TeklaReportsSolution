using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Tekla.Structures.Model;
using TeklaReportsApp;
using TeklaReportsApp.PartProperties;
using TeklaReportsApp.Extensions;

namespace TeklaReportsApp.PartReports
{
  internal static class StiffenerAnglesReport
  {
    public static void GetStiffenerAnglesReport(this List<Part> stiffenerAngles)
    {
      var mainPartsSchedule = new MainPartsSchedule();
      var saProperties = new List<StiffenerAngleProperties>();

      var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

      if (stiffenerAngles.Count == 0)
      {
        return;
      }
      Parallel.ForEach(stiffenerAngles, options, stiffenerAngle =>
      {
        mainPartsSchedule.GetSAReportProperties(stiffenerAngle, saProperties);
      });

      var orderedSAProps = saProperties.GroupBy(x => x.AssemblyPos).Select(x => new StiffenerAngleProperties
      {
        AssemblyPos = x.FirstOrDefault().AssemblyPos,
        Quantity = x.Count(),
        Span = x.Sum(s => s.Span),
        TopLevel = x.FirstOrDefault().TopLevel,
        Position = x.FirstOrDefault().Position,
        UserPhase = x.FirstOrDefault().UserPhase,
        UserField4 = x.FirstOrDefault().UserField4,
        UserField3 = x.FirstOrDefault().UserField3,
      }).OrderBy(p => p.UserField4).ThenBy(p => p.UserField3).ThenBy(p => p.AssemblyPos, new AlphanumComparator()).ToList();

      Console.WriteLine();

      Console.ResetColor();
      ColorConsole.WriteLine($"SA_Ass_Pos\tSA_No:s\tLength\tTop_Level   \tPosition\tU/Ph\tU/F4   \tU/F3   ", ConsoleColor.Green);
      foreach (var saProp in orderedSAProps)
      {
        ColorConsole.WriteLine($"{saProp.AssemblyPos,-10}\t{saProp.Quantity}\t{saProp.Span}\t{saProp.TopLevel,-12}\t{saProp.Position,-8}\t{saProp.UserPhase}\t{saProp.UserField4,-5}\t{saProp.UserField3,-5}", ConsoleColor.White);
      }
    }
  }
}
