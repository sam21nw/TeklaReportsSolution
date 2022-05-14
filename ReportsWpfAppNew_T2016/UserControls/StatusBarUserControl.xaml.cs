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
      string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

      TextBlockVersion.Text = version;

      Model model = new Model();

      if (!model.GetConnectionStatus())
      {
        return;
      }

      PersonModel personModel = new PersonModel();

      TextBlockReportStatus.Text = personModel.Name;
    }
  }
}
