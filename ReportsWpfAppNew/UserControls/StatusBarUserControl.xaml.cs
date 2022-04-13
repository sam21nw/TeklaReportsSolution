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
  /// Interaction logic for StatusBarUserControl.xaml
  /// </summary>
  public partial class StatusBarUserControl : UserControl
  {
    public StatusBarUserControl()
    {
      InitializeComponent();
      Loaded += StatusBarUserControl_Loaded;
    }

    private void StatusBarUserControl_Loaded(object sender, RoutedEventArgs e)
    {
      string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

      TextBlockVersion.Text = version;
    }
  }
}
