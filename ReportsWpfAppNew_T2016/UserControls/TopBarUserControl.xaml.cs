using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace TeklaReportsApp.UserControls
{
  /// <summary>
  /// Interaction logic for TopBarUserControl.xaml
  /// </summary>
  public partial class TopBarUserControl : UserControl
  {
    public TopBarUserControl()
    {
      InitializeComponent();
    }

    private void CheckBoxLockTeklaEvents_Checked(object sender, RoutedEventArgs e)
    {
      //reportStatus.Text = "Report Locked. Select objects in Model by selecting table rows";
    }

    private void CheckBoxLockTeklaEvents_Unchecked(object sender, RoutedEventArgs e)
    {
      //reportStatus.Text = "";
    }
  }
}
