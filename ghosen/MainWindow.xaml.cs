using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace ghosen
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    ViewModels.MainWindowViewModel vm = null;

    public MainWindow()
    {
      InitializeComponent();

      var pl = new PluginLoader(".");
      pl.Reload();

      vm = new ViewModels.MainWindowViewModel();
      this.DataContext = vm;
    }

    private void DropHandler(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        var filter = new Plugins.ArbIdFilter(new List<uint>() { 0x7E0, 0x7E8 });
        // Assuming you have one file that you care about, pass it off to whatever
        // handling code you have defined.
        var candumpLines = File.ReadAllLines(files[0]);
        var msg = (selectedParserPlugin.SelectedItem as IPluginV1).ParseLines(candumpLines, filter);
        var messages = ISO_TP.ISO_TP_Session.ProcessFrames(msg);
        var commands = ISO14229.MessageParser.ProcessMessages(messages);
        var fileChunks = FileExtractor.FileExtractor.ProcessMessages(commands).ToList();
        vm.Strings.Clear();
        var count = 0;
        foreach (var fc in fileChunks)
        {
          File.WriteAllBytes($"{files[0]}.chunks_{count}_0x{fc.MemoryAddress:X}.bin", fc.PayLoad);
          count++;
        }
        foreach (var item in commands)
        {
          vm.Strings.Add(item.ToString());
        }
      }
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      return;
      /*
      var a = CandumpParser.ParseLines(candumpLines, filter);

/*
var a = CandumpParser.ParseLines(candumpLines, filter);

var messageList = a.Select((va) => va.Message).ToList();
var messages = ISO_TP.ISO_TP_Session.ProcessFrames(messageList);
var commands = ISO14229.MessageParser.ProcessMessages(messages);

var sw = Stopwatch.StartNew();
a = CandumpParser.ParseLines(File.ReadAllLines("../../candump-2017-01-06_135802.log"), new CandumpParserArbIdFilter(new List<uint>() { 0x7E0, 0x7E8 }));
sw.Stop();
//Debug.WriteLine($"Parsed {a.Count} lines in {sw.ElapsedMilliseconds/1000.0d} seconds");
messageList = a.Select((va) => va.Message).ToList();
messages = ISO_TP.ISO_TP_Session.ProcessFrames(messageList);
commands = ISO14229.MessageParser.ProcessMessages(messages);
//var p = CandumpParser.ParseStream(File.OpenRead("../../candump-2017-01-06_135802.log"));
*/

    }
  }
}
