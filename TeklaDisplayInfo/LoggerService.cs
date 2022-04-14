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
    private static Model _model = new Model();

    internal static void DisplayInfo()
    {
      var _selObjectsList = _model.GetSelectedObjectsinModel().ToList();
      var count = _selObjectsList.Count;

      if (_selObjectsList != null)
      {
        if (count == 1)
        {
          var selObj = _selObjectsList.FirstOrDefault();
          selObj.GetReport();
        }
      }
    }
  }
}