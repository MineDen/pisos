using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace pisoscontrol
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		TcpListener listener = new TcpListener(IPAddress.Any, 8082);
		bool listening = false;

		public MainWindow()
		{
			InitializeComponent();
		}

		private static void StartListeningThread()
		{
			new Thread(new ThreadStart(() => {
				try
				{

				}
			}));
		}

		private void ToggleListening(object sender, RoutedEventArgs e)
		{
			if (listening == false)
			{
				listening = true;
				StartListeningThread();
			} else
			{
				listening = false;
			}
		}
	}
}
