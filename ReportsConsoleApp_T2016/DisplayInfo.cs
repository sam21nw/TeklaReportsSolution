using System;
using System.Diagnostics;

using Tekla.Structures.Model;

using Task = System.Threading.Tasks.Task;
using TeklaReportsApp.PartReports;
using TeklaReportsApp.Extensions;

namespace TeklaReportsApp
{
  internal static class DisplayInfo
  {
    private static readonly Model _model = new Model();
    public static async void DisplayReport()
    {
      var timer = new Stopwatch();
      timer.Start();
      ColorConsole.WriteLine("Generating Report...", ConsoleColor.Green);
      Console.WriteLine();

      var modelObjects = Task.Run(() => _model.GetSelectedObjectsinModel().ToList());

      var gratings = Task.Run(() => modelObjects.Result.GetGratingParts());
      var stiffenerAngles = Task.Run(() => modelObjects.Result.GetStiffenerAngleParts());
      var cpls = Task.Run(() => modelObjects.Result.GetCplParts());

      //var tasks = new List<Task<List<Part>>>
      //{
      //  Task.Run(() => modelObjects.GetGratingParts()),
      //  Task.Run(() => modelObjects.GetStiffenerAngleParts()),
      //  Task.Run(() => modelObjects.GetCplParts())
      //};

      //var results = await Task.WhenAll(tasks);

      Console.WriteLine("=======================================================================================================================================================================");

      await Task.WhenAll(gratings, stiffenerAngles, cpls);
      gratings.Result.GetGratingReport();
      stiffenerAngles.Result.GetStiffenerAnglesReport();
      cpls.Result.GetCheqPlatesReport();

      Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------");

      if (gratings.Result.Count != 0 || stiffenerAngles.Result.Count != 0 || cpls.Result.Count != 0)
      {
        Console.WriteLine();
        Console.WriteLine("Report created. Copy and paste in excel.");
        timer.Stop();
        TimeSpan timeTaken = timer.Elapsed;
        string timeStr = timeTaken.ToString(@"m\:ss\.fff");
        Console.Write("Time elapsed: ");
        ColorConsole.WriteLine(timeStr, ConsoleColor.Green);
        Console.WriteLine();
        Console.WriteLine();

        if (gratings.Result.Count != 0)
        {
          gratings.Result.GetGratingsCombinedReport();
        }
        if (stiffenerAngles.Result.Count != 0)
        {
          stiffenerAngles.Result.GetSACombinedReport();
        }
        if (cpls.Result.Count != 0)
        {
          cpls.Result.GetCplCombinedReport();
        }
      }
      else
      {
        ColorConsole.WriteLine("Oops... No Grating parts selected in model. Select Gratings or Stiffener Angles or Chequered plates to display report...", ConsoleColor.Yellow);
      }
    }
  }
}
