using AdonisUI;
using AdonisUI.Controls;

using System;
using System.Windows;
using System.Windows.Input;

using Tekla.Structures.Model;

using TeklaReportsApp.UserControls;
using TeklaReportsApp.AppExtensions;

namespace TeklaReportsApp
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : AdonisWindow
  {
    private static readonly Model _model = new Model();
    private readonly object _selectionEventHandlerLock = new object();
    private readonly object _tsExitEventHandlerLock = new object();
    public bool IsDark
    {
      get => (bool)GetValue(IsDarkProperty);
      set => SetValue(IsDarkProperty, value);
    }

    public static readonly DependencyProperty IsDarkProperty = DependencyProperty.Register("IsDark", typeof(bool), typeof(MainWindow), new PropertyMetadata(false, OnIsDarkChanged));

    private static void OnIsDarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((MainWindow)d).ChangeTheme((bool)e.OldValue);
    }
    private void ChangeTheme(bool oldValue)
    {
      ResourceLocator.SetColorScheme(Application.Current.Resources, oldValue ? ResourceLocator.LightColorScheme : ResourceLocator.DarkColorScheme);
    }

    public MainWindow()
    {
      InitializeComponent();
      Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      PersonModel p = new PersonModel();
      try
      {
        if (!_model.GetConnectionStatus())
        {
          p.Name = "Tekla Structures 2016 Model is not connected";
          return;
        }
        else
        {
          var modelName = _model.GetInfo().ModelName;
          p.Name = $"Connected Model: {modelName}";

          RegisterEventHandler();
          var window = e.Source as Window;
          System.Threading.Thread.Sleep(100);
          window.Dispatcher.Invoke(
          new Action(() =>
          {
            window.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
          }));
        }
      }
      catch (Exception ex)
      {
        AdonisUI.Controls.MessageBox.Show(ex.ToString());
      }
    }
    public void RegisterEventHandler()
    {
      Events _events = new Events();
      _events.SelectionChange += Events_SelectionChangeEvent;
      _events.TeklaStructuresExit += Events_TeklaExitEvent;
      _events.Register();
    }
    public void UnRegisterEventHandler()
    {
      Events _events = new Events();
      if (_events != null) _events.UnRegister();
    }

    private void Events_SelectionChangeEvent()
    {
      /* Make sure that the inner code block is running synchronously */
     StatusBarUserControl StatusBarUserControl = new StatusBarUserControl();
      PersonModel p = new PersonModel();
      lock (_selectionEventHandlerLock)
      {
        if (!_model.IsConnected())
        {
          p.Name = "Tekla Structures 2016 Model is not connected";
          return;
        }
        p.Name = "Tekla Structures 2016 Model is connected";
      }
    }
    private void Events_TeklaExitEvent()
    {
      lock (_tsExitEventHandlerLock)
      {
        UnRegisterEventHandler();
      }
    }
  }
}
