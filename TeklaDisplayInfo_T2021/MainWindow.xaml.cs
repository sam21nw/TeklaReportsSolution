using System.Windows;

using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;


namespace TeklaDisplayInfo_T2021
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      Model model = new Model();

      if (model.GetConnectionStatus())
      {
        MessageBox.Show("Tekla Structures 2021 Model Connected");
        Operation.DisplayPrompt("Tekla Structures 2021 Model Connected");
      }
      else
      {
        MessageBox.Show("Not Connected");
      }
    }
  }
}
