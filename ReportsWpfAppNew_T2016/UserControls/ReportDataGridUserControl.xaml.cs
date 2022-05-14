using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeklaReportsApp.UserControls
{
  /// <summary>
  /// Interaction logic for ReportDataGridUserControl.xaml
  /// </summary>
  public partial class ReportDataGridUserControl : UserControl
  {
    public ReportDataGridUserControl()
    {
      InitializeComponent();
    }

    private void ReportDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //if (ReportDataGrid != null && CheckBoxLockTeklaEvents.IsChecked == true)
      //{
      //  var ids = GetSelectedRows();
      //  SelectModelObjectsInUi(ids);
      //}
    }
  }
}
