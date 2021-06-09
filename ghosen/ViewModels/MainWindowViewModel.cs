using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ghosen.ViewModels
{
  public class MainWindowViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    public ObservableCollection<string> Strings { get; set; }
    public ObservableCollection<IPluginV1> Plugins { get; set; }

    public MainWindowViewModel()
    {
      Strings = new ObservableCollection<string>();
      Plugins = new ObservableCollection<IPluginV1>();
      var pl = new PluginLoader(".");
      pl.Reload();
      foreach (var item in pl.Plugins)
      {
        Plugins.Add(item);
      }
    }
  }
}
