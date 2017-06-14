using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace ghosen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var pl = new PluginLoader("Plugins");
            pl.Reload();
            var parser = pl.Plugins.ElementAt(1);

            var filter = new Plugins.ArbIdFilter(new List<uint>() { 0x7E0, 0x7E8 });

            var candumpLines = File.ReadAllLines(@"C:\Users\stefan.giroux\Documents\GolfR\ianGolfRAPR004.txt").ToArray();
            var msg = parser.ParseLines(candumpLines, filter).ToArray();
            var messages = ISO_TP.ISO_TP_Session.ProcessFrames(msg).ToArray();
            var commands = ISO14229.MessageParser.ProcessMessages(messages);
            var fileChunks = FileExtractor.FileExtractor.ProcessMessages(commands).ToList();
            foreach (var item in commands)
            {
                //vm.Strings.Add(item.ToString());
            }

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
