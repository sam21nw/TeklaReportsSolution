using System;

using Tekla.Structures.Model;
using Tekla.Structures;
using System.Data;
using TeklaReportsApp.Extensions;

namespace TeklaReportsApp
{
  static class ConsoleSettings
  {
    public static void SetConsoleWindow()
    {
      Console.WindowWidth = 174;
      Console.WindowHeight = 36;
      Console.Clear();
      SetConsole();
      SetAdvancedOptions();

      Console.WriteLine();
      ColorConsole.WriteLine("Select objects in Model. Creates Grating or stiffener angles or chequered plates report...", ConsoleColor.Green);
    }

    public static void SetConsole()
    {
      Model _model = new Model();
      string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
      ModelInfo modelInfo = _model.GetInfo();
      ProjectInfo projectInfo = _model.GetProjectInfo();

      var modelName = modelInfo.ModelName;
      var projectNumber = projectInfo.ProjectNumber;
      var projectName = projectInfo.Name;
      var projectObject = projectInfo.Object;
      var projectBuilder = projectInfo.Builder;
      var projectAddress = projectInfo.Address;

      Console.ResetColor();
      Console.WriteLine("Displays Grating + TP + BB + NS (combined) or StiffAngles or CPls report. Created by sam.");
      Console.Write("App version:- ");
      ColorConsole.WriteLine($"{version}", ConsoleColor.Green);
      Console.Write("Date Time:- ");
      ColorConsole.WriteLine($"{DateTime.Now:dddd, dd MMMM yyyy H:mm:ss}", ConsoleColor.Green);
      Console.Write("Model Name:- ");
      ColorConsole.WriteLine(modelName, ConsoleColor.Yellow);
      Console.Write("Project Name:- ");
      ColorConsole.WriteLine(projectName, ConsoleColor.Yellow);
      Console.Write("Structure:- ");
      ColorConsole.WriteLine(projectObject, ConsoleColor.Yellow);
      Console.Write("Builder:- ");
      ColorConsole.WriteLine(projectBuilder + ", " + projectAddress, ConsoleColor.Yellow);
      Console.Write("Project Number:- ");
      ColorConsole.WriteLine(projectNumber, ConsoleColor.Yellow);
      Console.WriteLine();
    }
    public static void SetAdvancedOptions()
    {
      var fllen = true;
      var fldes = true;
      var npdes = string.Empty;
      var uan = true;
      var un = true;
      var uanf = string.Empty;
      var cpblar = true;
      var opblc = true;
      //TeklaStructuresSettings.GetAdvancedOption("XS_ASSEMBLY_POSITION_NUMBER_FORMAT_STRING", "%ASSEMBLY_PREFIX%%ASSEMBLY_POS.3%");
      TeklaStructuresSettings.GetAdvancedOption("XS_CHECK_FLAT_LENGTH_ALSO", ref fllen);
      TeklaStructuresSettings.GetAdvancedOption("XS_USE_FLAT_DESIGNATION", ref fldes);
      TeklaStructuresSettings.GetAdvancedOption("XS_USE_NEW_PLATE_DESIGNATION", ref npdes);
      TeklaStructuresSettings.GetAdvancedOption("XS_UNIQUE_ASSEMBLY_NUMBERS", ref uan);
      TeklaStructuresSettings.GetAdvancedOption("XS_UNIQUE_NUMBERS", ref un);
      TeklaStructuresSettings.GetAdvancedOption("XS_CALCULATE_POLYBEAM_LENGTH_ALONG_REFERENCE_LINE", ref cpblar);
      TeklaStructuresSettings.GetAdvancedOption("XS_USE_OLD_POLYBEAM_LENGTH_CALCULATION", ref opblc);

      TeklaStructuresSettings.GetAdvancedOption("XS_USE_ASSEMBLY_NUMBER_FOR", ref uanf);

      ColorConsole.WriteLine($"Advanced options from Model", ConsoleColor.Cyan);

      Console.Write($"XS_CHECK_FLAT_LENGTH_ALSO".PadRight(52));
      Console.Write($": {fllen}".PadRight(24));
      if (fllen == false)
      {
        ColorConsole.WriteLine("<<< ok", ConsoleColor.Green);
      }
      else
      {
        ColorConsole.WriteLine("<<< check", ConsoleColor.Red);
      }

      Console.Write($"XS_USE_FLAT_DESIGNATION".PadRight(52));
      Console.Write($": {fldes}".PadRight(24));
      if (fldes == false)
      {
        ColorConsole.WriteLine("<<< ok", ConsoleColor.Green);
      }
      else
      {
        ColorConsole.WriteLine("<<< check", ConsoleColor.Red);
      }

      Console.Write($"XS_USE_NEW_PLATE_DESIGNATION".PadRight(52));
      Console.Write($": {npdes}".PadRight(24));
      if (npdes == string.Empty || npdes.ToUpper() == "FALSE")
      {
        ColorConsole.WriteLine("<<< ok", ConsoleColor.Green);
      }
      else
      {
        ColorConsole.WriteLine("<<< check", ConsoleColor.Red);
      }

      Console.Write($"XS_UNIQUE_ASSEMBLY_NUMBERS".PadRight(52));
      Console.Write($": {uan}".PadRight(24));
      if (uan == false)
      {
        ColorConsole.WriteLine("<<< ok", ConsoleColor.Green);
      }
      else
      {
        ColorConsole.WriteLine("<<< check", ConsoleColor.Red);
      }

      Console.Write($"XS_UNIQUE_NUMBERS".PadRight(52));
      Console.Write($": {un}".PadRight(24));
      if (un == false)
      {
        ColorConsole.WriteLine("<<< ok", ConsoleColor.Green);
      }
      else
      {
        ColorConsole.WriteLine("<<< check", ConsoleColor.Red);
      }

      Console.Write($"XS_USE_ASSEMBLY_NUMBER_FOR".PadRight(52));
      Console.Write($": {uanf}".PadRight(24));
      if (uanf == "MAIN_PART")
      {
        ColorConsole.WriteLine("<<< ok", ConsoleColor.Green);
      }
      else
      {
        ColorConsole.WriteLine("<<< check", ConsoleColor.Red);
      }

      Console.Write($"XS_CALCULATE_POLYBEAM_LENGTH_ALONG_REFERENCE_LINE".PadRight(52));
      Console.Write($": {cpblar}".PadRight(24));
      if (cpblar == false)
      {
        ColorConsole.WriteLine("<<< ok", ConsoleColor.Green);
      }
      else
      {
        ColorConsole.WriteLine("<<< check", ConsoleColor.Red);
      }

      Console.Write($"XS_USE_OLD_POLYBEAM_LENGTH_CALCULATION".PadRight(52));
      Console.Write($": {opblc}".PadRight(24));
      if (opblc == false)
      {
        ColorConsole.WriteLine("<<< ok", ConsoleColor.Green);
      }
      else
      {
        ColorConsole.WriteLine("<<< check", ConsoleColor.Red);
      }

      //Operation.dotSetAdvancedOption("XS_CHECK_FLAT_LENGTH_ALSO", false);
      //Operation.dotSetAdvancedOption("XS_USE_FLAT_DESIGNATION", false);
      //Operation.dotSetAdvancedOption("XS_USE_NEW_PLATE_DESIGNATION", "");
      //Operation.dotSetAdvancedOption("XS_UNIQUE_ASSEMBLY_NUMBERS", false);
      //Operation.dotSetAdvancedOption("XS_UNIQUE_NUMBERS", false);
      //Operation.dotSetAdvancedOption("XS_USE_ASSEMBLY_NUMBER_FOR", "MAIN_PART");
      //Operation.dotSetAdvancedOption("XS_ASSEMBLY_POSITION_NUMBER_FORMAT_STRING", "%ASSEMBLY_PREFIX%%ASSEMBLY_POS.3%");
      //Operation.dotSetAdvancedOption("XS_ALLOW_INCH_MARK_IN_DIMENSIONS", true);
      //Operation.dotSetAdvancedOption("XS_INCH_SIGN_ALWAYS", true);
    }
    public static void PrintToConsole(this DataTable dt)
    {
      int columnWidth = 36; //must be >5
      int tableWidth = (columnWidth * dt.Columns.Count) + dt.Columns.Count;

      ResizeTheWindow(tableWidth);

      Console.WriteLine("");
      Console.WriteLine("Table name : " + dt.TableName + "\n");

      #region PRINT THE TABLE HEADER

      DrawHorizontalSeperator(tableWidth, '=');
      Console.Write("|");

      foreach (DataColumn column in dt.Columns)
      {
        string name = (" " + column.ColumnName + " ").PadRight(columnWidth);
        Console.Write(name + "|");
      }
      Console.WriteLine("");
      DrawHorizontalSeperator(tableWidth, '=');

      #endregion

      #region PRINTING DATA ROWS

      foreach (DataRow row in dt.Rows)
      {
        Console.Write("|");
        foreach (DataColumn column in dt.Columns)
        {
          string value = (" " + GetShortString(row[column.ColumnName].ToString(), columnWidth) + " ").PadRight(columnWidth);
          Console.Write(value + "|");
        }
        Console.WriteLine("");
        DrawHorizontalSeperator(tableWidth, '-');
      }

      #endregion

      Console.WriteLine("");
    }

    private static void ResizeTheWindow(int tableWidth)
    {
      if (tableWidth > Console.LargestWindowWidth)
      {
        Console.WindowWidth = Console.LargestWindowWidth;
        Console.SetWindowPosition(0, 0);
      }
      else if (tableWidth > Console.WindowWidth)
      {
        Console.WindowWidth = tableWidth;
      }
      else
      {
        // let it be as it is.
      }
    }

    private static void DrawHorizontalSeperator(int width, char seperator)
    {
      for (int counter = 0; counter <= width; counter++)
      {
        Console.Write(seperator);
      }
      Console.WriteLine("");
    }

    private static string GetShortString(string text, int length)
    {
      if (text.Length >= length - 1)
      {
        string shortText = text.Substring(0, length - 5) + "...";
        return shortText;
      }
      else
      {
        return text;
      }
    }
  }
}
