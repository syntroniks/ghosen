using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var sw = Stopwatch.StartNew();
			var a = await CandumpParser.ParseLines(File.ReadAllLines("../../candump-2017-01-06_135802.log"), new ghosen.CandumpParserArbIdFilter(new List<uint>() { 0x7E0, 0x7E8 }));
			sw.Stop();
			Debug.WriteLine($"Parsed {a.Count} lines in {sw.ElapsedMilliseconds/1000.0d} seconds");
			var messageList = a.Select((va) => va.Message).ToList();
			var messages = ISO_TP.ISO_TP_Session.ProcessFrames(messageList);
			var commands = ISO14229.Messages.ProcessMessages(messages);
			//var p = CandumpParser.ParseStream(File.OpenRead("../../candump-2017-01-06_135802.log"));
		}
	}
}
