using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace pisos
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		private static BitmapImage LoadImage(byte[] imageData)
		{
			if (imageData == null || imageData.Length == 0) return null;
			var image = new BitmapImage();
			using (var mem = new MemoryStream(imageData))
			{
				mem.Position = 0;
				image.BeginInit();
				image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.UriSource = null;
				image.StreamSource = mem;
				image.EndInit();
			}
			image.Freeze();
			return image;
		}

		private static Random smprnd = new Random();

		private static void StartMoveThread(Window win, Image img)
		{
			int xd = 1;
			int yd = -1;
			int rot = 0;
			double smp = smprnd.NextDouble();
			Console.WriteLine(smp);
			Task.Factory.StartNew(() =>
			{
				Thread.Sleep((int)Math.Round(20 * smprnd.NextDouble()));
				/*Application.Current.Dispatcher.Invoke(() =>
				{
					Random rnd = new Random();
					win.Left = SystemParameters.WorkArea.Left + (rnd.Next(0, 2) * (SystemParameters.WorkArea.Right - SystemParameters.WorkArea.Left - win.Width));
					win.Top = SystemParameters.WorkArea.Top + (rnd.Next(0, 1) * (SystemParameters.WorkArea.Bottom - SystemParameters.WorkArea.Top - win.Height));
				});		*/
				while (win != null) {
					double cx = 0;
					double cy = 0;
					double cr = 0;
					double cb = 0;
					if (smprnd.NextDouble() > 0.8)
					{
						Thread.Sleep(10);
						smp = smprnd.NextDouble() / 5 - 0.1;
					}
					Current.Dispatcher.Invoke(() =>
					{
						cx = win.Left;
						cy = win.Top;
						cr = win.Left + win.Width;
						cb = win.Top + win.Height;
					});
					if (cx <= SystemParameters.WorkArea.Left)
						xd = 1;
					else if (cr >= SystemParameters.WorkArea.Right)
						xd = -1;
					if (cy <= SystemParameters.WorkArea.Top)
						yd = 1;
					else if (cb >= SystemParameters.WorkArea.Bottom)
						yd = -1;
					if (rot == 360)
						rot = 0;
					rot += 5;
					Current.Dispatcher.Invoke(() =>
					{
						win.Left += xd * 2 * smp + xd;
						win.Top += yd * 2 * smp + yd;
						if (rot % 90 == 0) img.RenderTransform = new RotateTransform(rot, win.Width / 2, win.Height / 2);
					});
					Thread.Sleep(10);
				}
			});
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetCursorPos(ref Win32Point pt);

		[StructLayout(LayoutKind.Sequential)]
		internal struct Win32Point
		{
			public Int32 X;
			public Int32 Y;
		};

		public static Point GetMousePosition()
		{
			Win32Point w32Mouse = new Win32Point();
			GetCursorPos(ref w32Mouse);
			return new Point(w32Mouse.X, w32Mouse.Y);
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			Mouse.OverrideCursor = Cursors.Arrow;
			for (int i = 0; i < 5; i++)
			{
				Window win1 = new Window();
				win1.Width = 268;
				win1.Height = 268;
				for (double r = smprnd.NextDouble() * 1.5; r < 2; r++)
				{
					win1.Left = SystemParameters.WorkArea.Left + (smprnd.NextDouble() * (SystemParameters.WorkArea.Right - SystemParameters.WorkArea.Left - win1.Width));
					win1.Top = SystemParameters.WorkArea.Top + (smprnd.NextDouble() * (SystemParameters.WorkArea.Bottom - SystemParameters.WorkArea.Top - win1.Height));
				}
				win1.WindowStyle = WindowStyle.None;
				win1.AllowsTransparency = true;
				win1.Topmost = true;
				win1.ShowInTaskbar = false;
				win1.Background = null;
				Image img = new Image();
				img.ToolTip = "ЛОХ";
				img.Width = win1.Width;
				img.Height = win1.Height;
				img.Source = LoadImage(trh.rawData);
				win1.Content = img;
				win1.Show();
				StartMoveThread(win1, img);
				Thread.Sleep(0);
				Thread.Sleep(550);
			}
			/*Window dick = new Window();
			dick.Width = SystemParameters.PrimaryScreenWidth;
			dick.Height = SystemParameters.PrimaryScreenHeight;
			dick.Left = 0;
			dick.Top = 0;
			dick.WindowStyle = WindowStyle.None;
			dick.AllowsTransparency = true;
			dick.Background = null;*/
		}
	}
}
