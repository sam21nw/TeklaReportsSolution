﻿
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AppExtensions
{
  public sealed class SingleInstance
  {
    public static bool AlreadyRunning()
    {
      bool running = false;
      try
      {
        // Getting collection of process  
        Process currentProcess = Process.GetCurrentProcess();

        // Check with other process already running   
        foreach (var p in Process.GetProcesses())
        {
          if (p.Id != currentProcess.Id) // Check running process   
          {
            if (p.ProcessName.Equals(currentProcess.ProcessName) == true)
            {
              running = true;
              IntPtr hFound = p.MainWindowHandle;
              if (User32API.IsIconic(hFound)) // If application is in ICONIC mode then  
                User32API.ShowWindow(hFound, User32API.SW_RESTORE);
              User32API.SetForegroundWindow(hFound); // Activate the window, if process is already running  
              break;
            }
          }
        }
      }
      catch { }
      return running;
    }
  }
  public class User32API
  {
    [DllImport("User32.dll")]
    public static extern bool IsIconic(IntPtr hWnd);

    [DllImport("User32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("User32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public const int SW_RESTORE = 9;
  }
}