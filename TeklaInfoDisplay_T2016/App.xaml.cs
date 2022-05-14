using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;

using Tekla.Structures.Model;

using TeklaInfoDisplay;

namespace InfoDisplay_2016
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private readonly Model _model = new Model();
    private Events _events = null;
    private readonly object _selectionEventHandlerLock = new object();
    private readonly object _tsExitEventHandlerLock = new object();

    private System.Windows.Forms.NotifyIcon _notifyIcon;
    private bool _isExit;
    const string appName = "TeklaInfoDisplay_2016";

    protected override void OnStartup(StartupEventArgs e)
    {
      Mutex _mutex = new Mutex(true, appName, out bool createdNew);

      Process[] procByName = Process.GetProcessesByName(appName);
      if (procByName.Length > 1 || !createdNew)
      {
        App.Current.Shutdown();
      }

      MainWindow = new MainWindow();
      MainWindow.Closing += MainWindow_Closing;

      _notifyIcon = new System.Windows.Forms.NotifyIcon
      {
        Text = appName
      };
      _notifyIcon.DoubleClick += (s, args) => RestartApplication();

      _notifyIcon.Icon = TeklaInfoDisplay.Properties.Resources.Icon;
      _notifyIcon.Visible = true;

      CreateContextMenu();
      StartDisplayInfo();
      base.OnStartup(e);
    }
    private void CreateContextMenu()
    {
      _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
      _notifyIcon.ContextMenuStrip.Items.Add("Start Display Info").Click += (s, e) => StartDisplayInfo();
      _notifyIcon.ContextMenuStrip.Items.Add("Stop Display Info").Click += (s, e) => StopDisplayInfo();
      _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
    }

    private void StartDisplayInfo()
    {
      UnRegisterEventHandler();
      if (_model.GetConnectionStatus())
      {
        _events = new Events();
        _events.TeklaStructuresExit += this.Events_TeklaExitEvent;
        _events.SelectionChange += this.Events_SelectionChanged;
        _events.Register();
      }
      else
      {
        return;
      }
    }
    private void StopDisplayInfo()
    {
      if (!_model.GetConnectionStatus())
      {
        return;
      }
      UnRegisterEventHandler();
    }

    public void UnRegisterEventHandler()
    {
      if (_events != null) _events.UnRegister();
    }

    private void Events_TeklaExitEvent()
    {
      lock (_tsExitEventHandlerLock)
      {
        ExitApplication();
      }
    }

    private void ExitApplication()
    {
      _isExit = true;
      App.Current.Dispatcher.InvokeAsync(new Action(() =>
      {
        MainWindow = new MainWindow();
        MainWindow.Close();
        this.Shutdown();
        _notifyIcon.Dispose();
        _notifyIcon = null;
      }));
    }

    private void RestartApplication()
    {
      if (_model.GetConnectionStatus())
      {
        return;
      }
      _isExit = true;
      System.Windows.Forms.Application.Restart();
      Current.Shutdown();
    }
    private void Events_SelectionChanged()
    {
      lock (_selectionEventHandlerLock)
      {
        DisplayInfoService.DisplayInfo();
      }
    }

    private void MainWindow_Closing(object sender, CancelEventArgs e)
    {
      if (!_isExit)
      {
        e.Cancel = true;
        MainWindow.Hide(); // A hidden window can be shown again, a closed one not
      }
    }
  }

}
