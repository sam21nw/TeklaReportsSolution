using AdonisUI;
using AdonisUI.Controls;

using AppExtensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Tekla.Structures;
using Tekla.Structures.Model;
using Tekla.Structures.ModelInternal;

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
      try
      {
        string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        TextBlockVersion.Text = version;
        if (!_model.GetConnectionStatus())
        {
          reportStatus.Text = "Tekla 2016 model is not connected...";
          return;
        }
        else
        {
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
      lock (_selectionEventHandlerLock)
      {
        Dispatcher.Invoke(() =>
        {
          reportStatus.Text = string.Empty;
          MainPartSchedule.WHAreaCut = 100;
          MainPartSchedule.UnitWeight = 0;

          var timer = new Stopwatch();
          timer.Start();

          if (TextBoxWHAreaFactor.Text == string.Empty)
          {
            MainPartSchedule.WHAreaCut = 100;
          }
          else
          {
            try
            {
              double txtWHFactor = double.Parse(TextBoxWHAreaFactor.Text);
              if (0.01 <= txtWHFactor && txtWHFactor <= 0.5)
              {
                MainPartSchedule.WHAreaCut = txtWHFactor;
              }
              else
              {
                reportStatus.Text = "W/H cut value must be between 0.01 and 0.5";
              }
            }
            catch (Exception ex)
            {
              reportStatus.Text = "W/H cut " + ex.Message.ToString() + ", default value is used.";
            }
          }
          if (TextBoxWeightFactor.Text == string.Empty)
          {
            MainPartSchedule.UnitWeight = 0;
          }
          else
          {
            try
            {
              double txtUnitWt = double.Parse(TextBoxWeightFactor.Text);
              if (1 <= txtUnitWt && txtUnitWt <= 500)
              {
                MainPartSchedule.UnitWeight = txtUnitWt;
              }
              else
              {
                reportStatus.Text = "Unit weight value must be between 1 and 500, else weight taken from model";
              }
            }
            catch (Exception ex)
            {

              reportStatus.Text = "Unit weight " + ex.Message.ToString() + ", default value is used.";
            }
          }


          ListtoDataTableConverter converter = new ListtoDataTableConverter();

          List<Part> parts = GetListedModelObjects();
          List<MainPartProperties> mpPropsList = new List<MainPartProperties>();
          List<PartsSummaryModel> psmList = new List<PartsSummaryModel>();

          if (CheckBoxLockTeklaEvents.IsChecked == false)
          {
            if (ComboBoxSearchReport.Text == "Combined")
            {
              mpPropsList = parts.GetPartsCombinedReport();
            }
            if (ComboBoxSearchReport.Text == "Unique")
            {
              mpPropsList = parts.GetPartsUniqueReport();
            }
            DataTable dtProps = converter.ToDataTable(mpPropsList);
            //reportDataGrid.ItemsSource = grpList;
            ReportDataGrid.DataContext = dtProps;
            timer.Stop();

            List<string> listStr = GetPartsStringList();

            psmList = parts.GetPartsSummaryList();
            DataTable dtSummary = converter.ToDataTable(psmList);
            SummaryDataGrid.DataContext = dtSummary;
          }

          TimeSpan timeTaken = timer.Elapsed;
          string timeStr = timeTaken.ToString(@"m\:ss\.fff");
          //reportStatus.Text = "Time elapsed: " + timeStr;

          if (CheckBoxLockTeklaEvents.IsChecked == true)
          {
            reportStatus.Text = "Report Locked. Select objects in Model by selecting table rows";
          }
        });
      }
    }

    private List<Part> GetListedModelObjects()
    {
      var partList = new List<Part>();
      List<string> listStr = GetPartsStringList();

      List<ModelObject> modelObjects = _model.GetSelectedObjectsinModel().ToList();

      var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

      Parallel.ForEach(modelObjects, options, obj =>
      {
        string name = string.Empty;
        string partPrefix = string.Empty;
        string assPrefix = string.Empty;

        obj.GetReportProperty("NAME", ref name);
        obj.GetReportProperty("PART_PREFIX", ref partPrefix);
        obj.GetReportProperty("ASS_PREFIX", ref assPrefix);

        listStr.ForEach((item) =>
        {
          if (name.ToUpper().Contains(item))
          {
            partList.Add((Part)obj);
          }
        });

        //if (listStr.Any(name.ToUpper().Contains) || listStr.Any(partPrefix.ToUpper().Contains) || listStr.Any(assPrefix.ToUpper().Contains))
        //{
        //  partList.Add((Part)obj);
        //}
      });
      return partList;
    }

    private List<string> GetPartsStringList()
    {
      string[] listStr = TextBoxMainParts.Text.Split(' ');

      return listStr.ToList();
    }

    private void ReportDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (ReportDataGrid != null && CheckBoxLockTeklaEvents.IsChecked == true)
      {
        var ids = GetSelectedRows();
        SelectModelObjectsInUi(ids);
      }
    }
    private void SelectModelObjectsInUi(List<int> ids)
    {
      var modelObjects = new ArrayList();

      ids.ForEach(id =>
      {
        var modelObject = _model.SelectModelObject(new Identifier(id));
        if (modelObject == null) return;
        modelObjects.Add(modelObject);
      });

      var selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
      selector.Select(modelObjects);
      Operation.dotStartAction("ZoomToSelected", "");
    }
    private List<int> GetSelectedRows()
    {
      List<int> ids = new List<int>();
      foreach (DataRowView row in ReportDataGrid.SelectedItems)
      {
        DataRow myRow = row.Row;
        var id = myRow["ID"].ToString();
        ids.Add(int.Parse(id));
      }
      return ids;
    }

    private void Events_TeklaExitEvent()
    {
      lock (_tsExitEventHandlerLock)
      {
        UnRegisterEventHandler();
      }
    }

    private void CheckBoxLockTeklaEvents_Checked(object sender, RoutedEventArgs e)
    {
      reportStatus.Text = "Report Locked. Select objects in Model by selecting table rows";
    }

    private void CheckBoxLockTeklaEvents_Unchecked(object sender, RoutedEventArgs e)
    {
      reportStatus.Text = "";
    }
  }
}
