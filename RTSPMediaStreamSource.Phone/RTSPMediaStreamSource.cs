/* Ree7 WPNativeRTSP
 * Copyright (C) 2013 Pierre BELIN <pierre@ree7.fr>, and project contributors.
 *
 * Distributed under the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace Media
{
	public class RTSPMediaStreamSource : MediaStreamSource
	{
		private RTSPBridge.WindowsPhoneRuntimeComponent bridge;
		private byte[] buffer;
		private object sync = new object();

		public string Source { get; set; }

		public RTSPMediaStreamSource(string rtspSourceUri)
		{
			Source = rtspSourceUri;

			bridge = new RTSPBridge.WindowsPhoneRuntimeComponent();
			bridge.DataChunkEvent += OnBridge_DataChunkEvent;
			bridge.LogEvent += OnBridge_LogEvent;
		}

		private void OnBridge_LogEvent(string __param0)
		{
#if DEBUG
			System.Diagnostics.Debug.WriteLine(__param0);
#endif
		}

		private void OnBridge_DataChunkEvent(System.Collections.Generic.IList<byte> __param0)
		{
			lock (sync)
			{
				buffer = __param0.ToArray();
			}

			// TODO DEMUX
		}

		protected override void CloseMedia()
		{
			bridge.CloseStream();
		}

		protected override void GetDiagnosticAsync(MediaStreamSourceDiagnosticKind diagnosticKind)
		{
			throw new NotImplementedException();
		}

		protected override void GetSampleAsync(MediaStreamType mediaStreamType)
		{
			throw new NotImplementedException();

			// Gotta demux first to separate the Video/Audio frames in different buffers

		}

		protected override void OpenMediaAsync()
		{
			if (String.IsNullOrEmpty(Source))
				throw new ArgumentException("Must set Source before opening the media");

			// Initialize data structures to pass to the Media pipeline via the MediaStreamSource
            Dictionary<MediaSourceAttributesKeys, string> mediaSourceAttributes = new Dictionary<MediaSourceAttributesKeys, string>();
            Dictionary<MediaStreamAttributeKeys, string> mediaStreamAttributes = new Dictionary<MediaStreamAttributeKeys, string>();
            List<MediaStreamDescription> mediaStreamDescriptions = new List<MediaStreamDescription>();

			// Configure the specific handler for the first data chunk received
			RTSPBridge.DataChunkEventHandler firstPacketHandler = null;
			firstPacketHandler = (data) =>
			{
				bridge.DataChunkEvent -= firstPacketHandler;

				// Set the media stream descriptors
				// TODO : Extract from stream and replace hardcoded values
				mediaSourceAttributes[MediaSourceAttributesKeys.CanSeek] = "0";

				/* FBX France 2 : 
				 * Container M2TS
				 * - Flux 0 : Video ID 68, Codec H264 - MPEG4-AVC(10), 720x576, 50fpsn 4:2:0 YUV
				 * - Flux 1 : Audio ID 69, Codec AAC (mp4a), FR, Stéréo, 48kHz
				 * - Flux 2 : Text ID 70, Codec telx, FR
				 * - Flux 3 : Text, Codec telx
				 * - Flux 4 : Audio ID 71, Codec AAC (mp4a), qad
				 * - Flux 5 : Audio ID 72, Codec AAC (mp4a), original audio
				 */
				Dictionary<MediaStreamAttributeKeys, string> stream0 = new Dictionary<MediaStreamAttributeKeys, string>();
				stream0[MediaStreamAttributeKeys.Height] = "576";
				stream0[MediaStreamAttributeKeys.Width] = "720";

				mediaStreamDescriptions.Add(new MediaStreamDescription(MediaStreamType.Video, stream0));

				this.ReportOpenMediaCompleted(mediaSourceAttributes, mediaStreamDescriptions);
			};

			bridge.DataChunkEvent += firstPacketHandler;

			// Start the streaming
			bridge.OpenStream(Source);
		}

		protected override void SeekAsync(long seekToTime)
		{
			throw new NotImplementedException();
		}

		protected override void SwitchMediaStreamAsync(MediaStreamDescription mediaStreamDescription)
		{
			throw new NotImplementedException();
		}
	}
}
