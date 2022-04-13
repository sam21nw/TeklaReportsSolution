using System.Windows;
using System.Windows.Controls;

using Tekla.Structures.Model;

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
      DataContext = this;
      string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

      TextBlockVersion.Text = version;

      Model model = new Model();

      if (!model.GetConnectionStatus())
      {
        return;
      }
      ReportStatus = "Hello World";
    }

    public string ReportStatus { get; set; }
  }
}
