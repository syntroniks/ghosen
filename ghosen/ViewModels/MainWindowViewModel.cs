using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ghosen.ViewModels
{
  public class MainWindowViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    public ObservableCollection<string> Strings { get; set; }

    public MainWindowViewModel()
    {
      Strings = new ObservableCollection<string>();
    }
  }
}
