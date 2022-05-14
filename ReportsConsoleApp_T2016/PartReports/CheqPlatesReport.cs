using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Tekla.Structures.Model;
using TeklaReportsApp.PartProperties;
using TeklaReportsApp;
using TeklaReportsApp.Extensions;

namespace TeklaReportsApp.PartReports
{
  internal static class CheqPlatesReport
  {
    public static void GetCheqPlatesReport(this List<Part> cpls)
    {
      var mainPartsSchedule = new MainPartsSchedule();
      var cplProperties = new List<ChequeredPlateProperties>();

      var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

      if (cpls.Count == 0)
      {
        return;
      }
      Parallel.ForEach(cpls, options, cpl =>
      {
        mainPartsSchedule.GetCplReportProperties(cpl, cplProperties);
      });

      var orderedCPLProps = cplProperties.GroupBy(x => x.AssemblyPos).Select(x => new ChequeredPlateProperties
      {
        AssemblyPos = x.FirstOrDefault().AssemblyPos,
        Quantity = x.Count(),
        Area = x.Sum(s => s.Area),
        TopLevel = x.FirstOrDefault().TopLevel,
        Position = x.FirstOrDefault().Position,
        UserPhase = x.FirstOrDefault().UserPhase,
        UserField4 = x.FirstOrDefault().UserField4,
        UserField3 = x.FirstOrDefault().UserField3,
      }).OrderBy(p => p.UserField4).ThenBy(p => p.UserField3).ThenBy(p => p.AssemblyPos, new AlphanumComparator()).ToList();

      Console.WriteLine();

      ColorConsole.WriteLine($"CPL_Ass_Pos\tNo:s\tArea\tU/Ph\tU/F4   \tU/F3   ", ConsoleColor.Green);
      foreach (var cplProp in orderedCPLProps)
      {
        Console.WriteLine($"{cplProp.AssemblyPos,-10}\t{cplProp.Quantity}\t{cplProp.Area}\t{cplProp.TopLevel,-12}\t{cplProp.Position,-8}\t{cplProp.UserPhase}\t{cplProp.UserField4,-5}\t{cplProp.UserField3,-5}", ConsoleColor.White);
      }
    }
  }
}