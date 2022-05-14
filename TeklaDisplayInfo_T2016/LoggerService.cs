using System;
using System.Collections.Generic;
using System.Linq;

using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;

using AppExtensions;

namespace TeklaDisplayInfo
{
  internal class LoggerService
  {
    private LoggerService()
    {
    }
    public static LoggerService Instance
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
      internal static readonly LoggerService instance = new LoggerService();
    }

    internal static void DisplayInfo()
    {
      string result = string.Empty;
      Operation.DisplayPrompt(result);
    }
  }
}
