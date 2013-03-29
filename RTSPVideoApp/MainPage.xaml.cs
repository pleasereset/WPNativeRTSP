/* Ree7 WPNativeRTSP
 * Copyright (C) 2013 Pierre BELIN <pierre@ree7.fr>, and project contributors.
 *
 * Distributed under the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RTSPVideoApp.Resources;
using RTSPBridge;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace RTSPVideoApp
{
	public partial class MainPage : PhoneApplicationPage
	{
		RTSPBridge.WindowsPhoneRuntimeComponent myRTSPBridge;

		MemoryStream videoStream;
		StreamWriter writer;

		// Constructor
		public MainPage()
		{
			InitializeComponent();

			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();

			// Default RTSP feed uri
			TbRtspSource.Text = @"rtsp://mafreebox.freebox.fr/fbxtv_pub/stream?namespace=1&service=201&flavour=ld";

			myRTSPBridge = new WindowsPhoneRuntimeComponent();
			myRTSPBridge.LogEvent += myRTSPBridge_LogEvent;
			myRTSPBridge.DataChunkEvent += myRTSPBridge_DataChunkEvent;

			Player.MediaFailed += Player_MediaFailed;
			Player.MediaOpened += Player_MediaOpened;

			videoStream = new MemoryStream();
			writer = new StreamWriter(videoStream);
		}

		void Player_MediaOpened(object sender, RoutedEventArgs e)
		{
			return;
		}

		void Player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			return;
		}

		private void myRTSPBridge_DataChunkEvent(IList<byte> __param0)
		{
			System.Diagnostics.Debug.WriteLine("[RTSPBridge-chunk] Received data, size : " + __param0.Count);
			//myHttpRelay.SpitData(__param0.ToArray());
			byte[] array = __param0.ToArray();
			videoStream.Write(array, 0, array.Length);
		}

		private void myRTSPBridge_LogEvent(string __param0)
		{
			System.Diagnostics.Debug.WriteLine("[RTSPBridge-log] " + __param0);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			string rtspUri = TbRtspSource.Text;

			Thread rtspWorkerThread = new Thread(new ThreadStart(() =>
			{
				myRTSPBridge.OpenStream(rtspUri);
			}));

			rtspWorkerThread.Start();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			myRTSPBridge.CloseStream();
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			Player.SetSource(videoStream);
		}

		// Sample code for building a localized ApplicationBar
		//private void BuildLocalizedApplicationBar()
		//{
		//    // Set the page's ApplicationBar to a new instance of ApplicationBar.
		//    ApplicationBar = new ApplicationBar();

		//    // Create a new button and set the text value to the localized string from AppResources.
		//    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
		//    appBarButton.Text = AppResources.AppBarButtonText;
		//    ApplicationBar.Buttons.Add(appBarButton);

		//    // Create a new menu item with the localized string from AppResources.
		//    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
		//    ApplicationBar.MenuItems.Add(appBarMenuItem);
		//}
	}
}