
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;

using TeklaInfoDisplay.Models;

namespace TeklaInfoDisplay
{
  public static class DisplayInfoService
  {
    public static void DisplayInfo()
    {
      try
      {
        string result = string.Empty;

        //var timer = new Stopwatch();
        //timer.Start();

        var selectedObjects = ModelParts.GetSelectedObjectsinModel();

        if (selectedObjects.Count == 1)
        {
          var selObj = selectedObjects.First();
          if (selObj.GetType() == typeof(ControlPoint))
          {
            result = ((ControlPoint)selObj).GetPointCoords();
          }
          else
          {
            result = ModelPartReports.GetSelectedObjectReport(selObj);
          }
        }

        if (selectedObjects.Count > 1)
        {
          var gratings = selectedObjects.GetGratingParts();
          result = GratingReport.GetGratingGroupReport(gratings);

          if (gratings.Count == 0)
          {
            var gratingAssemblies = selectedObjects.GetGratingAssemblies();

            if (gratingAssemblies.Count != 0)
            {
              result = GratingReport.GetGratingAssGroupReport(gratingAssemblies);
            }
            else
            {
              var beams = selectedObjects.GetBeams();
              var columns = selectedObjects.GetColumns();
              var openings = selectedObjects.GetOpenings();
              var toePlates = selectedObjects.GetToePlates();
              var bindingBars = selectedObjects.GetBindingBars();
              var nosingPlates = selectedObjects.GetNosingPlates();
              var stiffenerAngles = selectedObjects.GetStiffenerAngles();
              var treads = selectedObjects.GetStairTreads();
              var chqpls = selectedObjects.GetCPLParts();

              if (stiffenerAngles.Count > 1)
              {
                result = GratingReport.GetGratingPartsGroupReport(stiffenerAngles);
              }
              else if (chqpls.Count > 1)
              {
                result = GratingReport.GetCheqPLGroupReport(chqpls);
              }
              else if (nosingPlates.Count > 1)
              {
                result = GratingReport.GetGratingPartsGroupReport(nosingPlates);
              }
              else if (toePlates.Count > 1)
              {
                result = GratingReport.GetGratingPartsGroupReport(toePlates);
              }
              else if (bindingBars.Count > 1)
              {
                result = GratingReport.GetGratingPartsGroupReport(bindingBars);
              }
              else if (columns.Count > 1)
              {
                result = ModelPartReports.GetPartsGroupReport(columns);
              }
              else if (beams.Count > 1)
              {
                result = ModelPartReports.GetPartsGroupReport(beams);
              }
              else if (treads.Count > 1)
              {
                result = ModelPartReports.GetPartsGroupReport(treads);
              }
              else if (openings.Count == 2)
              {
                result = ModelPartReports.GetDistanceBtwParts(openings);
              }
            }
          }
        }
        Operation.DisplayPrompt(result);

        //timer.Stop();
        //TimeSpan timeTaken = timer.Elapsed;
        //string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
        //MessageBox.Show(foo);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }
  }
}
