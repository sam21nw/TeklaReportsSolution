using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Tekla.Structures.Model;

using AppExtensions;

namespace TeklaReportsApp
{
  internal static class GratingPartReport
  {
    public static void GetGratingReport(this List<Part> gratings)
    {
      var mainPartsSchedule = new MainPartsSchedule();
      var gratingProperties = new List<GratingProperties>();

      var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

      if (gratings.Count == 0)
      {
        return;
      }

      Parallel.ForEach(gratings, options, grating =>
      {
        mainPartsSchedule.GetGratingReportProperties(grating, gratingProperties);
      });
      //foreach (var grating in gratings)
      //{
      //  mainPartsSchedule.GetGratingReportProperties(grating, gratingProperties);
      //}

      var gratingReport = gratingProperties.GroupBy(x => x.AssemblyPos).Select(x => new GratingProperties
      {
        AssemblyPos = x.FirstOrDefault().AssemblyPos,
        Quantity = x.Count(),
        SpanImperial = x.FirstOrDefault().SpanImperial,
        WidthImperial = x.FirstOrDefault().WidthImperial,
        Span = x.FirstOrDefault().Span,
        Width = x.FirstOrDefault().Width,
        CutArea = x.FirstOrDefault().CutArea,
        TpLength = x.Sum(t => t.TpLength),
        BbLength = x.Sum(t => t.BbLength),
        NsLength = x.Sum(t => t.NsLength),
        ChqArea = x.Sum(c => c.ChqArea),
        UserPhase = x.FirstOrDefault().UserPhase,
        UserField4 = x.FirstOrDefault().UserField4,
        UserField3 = x.FirstOrDefault().UserField3,
        TopLevel = x.FirstOrDefault().TopLevel,
        Position = x.FirstOrDefault().Position,
      }).OrderBy(p => p.UserField4).ThenBy(p => p.UserField3).ThenBy(p => p.AssemblyPos, new AlphanumComparator()).ToList();

      using (DataTable dt = new DataTable("GratingReport"))
      {
        dt.Columns.Add("Ass_Pos".PadRight(12), typeof(string));
        dt.Columns.Add("No:s", typeof(int));
        dt.Columns.Add("Span_Imp".PadRight(12), typeof(string));
        dt.Columns.Add("Width_Imp".PadRight(12), typeof(string));
        dt.Columns.Add("Span", typeof(double));
        dt.Columns.Add("Width", typeof(double));
        dt.Columns.Add("CutArea", typeof(double));
        dt.Columns.Add("TP_mm", typeof(double));
        dt.Columns.Add("NS_mm", typeof(double));
        dt.Columns.Add("BB_mm", typeof(double));
        dt.Columns.Add("CHQ_m2", typeof(double));
        dt.Columns.Add("Top_Level".PadRight(12), typeof(string));
        dt.Columns.Add("Position".PadRight(10), typeof(string));
        dt.Columns.Add("U/Ph".PadRight(5), typeof(string));
        dt.Columns.Add("U/F3".PadRight(5), typeof(string));
        dt.Columns.Add("U/F4".PadRight(5), typeof(string));

        foreach (var gratingProp in gratingReport)
        {
          dt.Rows.Add(gratingProp.AssemblyPos.PadRight(12), gratingProp.Quantity, gratingProp.SpanImperial.PadRight(12), gratingProp.WidthImperial.PadRight(12), gratingProp.Span, gratingProp.Width, gratingProp.CutArea, gratingProp.TpLength, gratingProp.NsLength, gratingProp.BbLength, gratingProp.ChqArea, gratingProp.TopLevel.PadRight(12), gratingProp.Position.PadRight(8), gratingProp.UserPhase.PadRight(5), gratingProp.UserField3.PadRight(5), gratingProp.UserField4.PadRight(5));
        }
        foreach (DataColumn column in dt.Columns)
        {
          ColorConsole.Write(column.ColumnName + "\t", ConsoleColor.Green);
        }
        Console.WriteLine();
        foreach (DataRow dr in dt.Rows)
        {
          foreach (var item in dr.ItemArray)
          {
            ColorConsole.Write(item + "\t", ConsoleColor.White);
          }
          Console.WriteLine();
        }
      }
    }
  }
}