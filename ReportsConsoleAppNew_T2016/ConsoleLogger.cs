using System;

namespace TeklaReportsApp
{
  public sealed class ConsoleLogger
  {
    private ConsoleLogger()
    {
    }
    public static ConsoleLogger Instance
    {
      get
      {
        return Nested.instance;
      }
    }
    private class Nested
    {
      // Explicit static constructor to tell C# compiler
      // not to mark type as beforefieldinit
      static Nested()
      {
      }
      internal static readonly ConsoleLogger instance = new ConsoleLogger();
    }
    public static void ShowReport(string message)
    {
      Console.WriteLine(message);
    }
  }
}
