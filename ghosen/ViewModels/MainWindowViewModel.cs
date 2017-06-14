using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<string> Strings { get; set; }

        public MainWindowViewModel()
        {
            Strings = new ObservableCollection<string>();
        }
    }
}
