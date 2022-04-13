using System;

namespace ReportsApp
{
  class ConsoleLogger
  {
    private static int _counter;
    public static int Counter
    {
      get
      {
        return _counter;
      }
      set
      {
        _counter = value;
      }
    }
    public ConsoleLogger()
    {
      Counter++;
    }
    public void ShowReport()
    {
      Console.WriteLine($"Report displayed. Counter: {Counter}");
    }
  }
}
