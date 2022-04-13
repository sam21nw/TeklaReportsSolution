using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using Tekla.Structures.Model;

using AppExtensions;

namespace TeklaReportsApp
{
  class Program
  {
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        int uFlags);

    private const int HWND_TOPMOST = -1;
    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_NOSIZE = 0x0001;

    private static readonly object _selectionEventHandlerLock = new object();
    private static readonly object _tsExitEventHandlerLock = new object();
    static void Main()
    {
      IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;

      SetWindowPos(hWnd,
          new IntPtr(HWND_TOPMOST),
          0, 0, 0, 0,
          SWP_NOMOVE | SWP_NOSIZE);

      string mutex_id = "ReportsApp_T2016";
      using (Mutex mutex = new Mutex(false, mutex_id))
        if (!mutex.WaitOne(0, false))
        {
          return;
        }
        else
        {
          Console.Title = "Reports App for Tekla 2016";
          Console.WindowWidth = 138;
          Console.WindowHeight = 36;
          Console.BackgroundColor = ConsoleColor.Black;
          try
          {
            Model _model = new Model();

            if (!_model.GetConnectionStatus())
            {
              ColorConsole.WriteLine("Oops... Tekla 2016 model is not conneted. Open a model and restart the app.", ConsoleColor.Red);
              Console.ReadLine();
            }
            RegisterEventHandler();
            Console.ReadLine();
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.ToString());
            Console.ReadLine();
          }
          Console.ReadLine();
        }
    }

    static void Events_SelectionChangeEvent()
    {
      /* Make sure that the inner code block is running synchronously */
      lock (_selectionEventHandlerLock)
      {
        Console.Clear();
        ConsoleLogger.ShowReport("Hello dude");
        ConsoleLogger.ShowReport("Hello rock");
      }
    }

    private static void Events_TeklaExitEvent()
    {
      lock (_tsExitEventHandlerLock)
      {
        UnRegisterEventHandler();
      }
    }
    public static void RegisterEventHandler()
    {
      var _events = new Events();
      _events.SelectionChange += Events_SelectionChangeEvent;
      _events.TeklaStructuresExit += Events_TeklaExitEvent;
      _events.Register();
    }

    public static void UnRegisterEventHandler()
    {
      var _events = new Events();
      if (_events != null) _events.UnRegister();
    }
  }
}
