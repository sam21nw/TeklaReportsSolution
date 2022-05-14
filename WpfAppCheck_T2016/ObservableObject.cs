using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAppCheck_T2016
{
  internal class ObservableObject : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyname = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
    }
  }
}
