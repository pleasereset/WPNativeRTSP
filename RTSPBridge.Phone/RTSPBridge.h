/* Ree7 WPNativeRTSP
 * Copyright (C) 2013 Pierre BELIN <pierre@ree7.fr>, and project contributors.
 *
 * Distributed under the Microsoft Public License (Ms-PL).
 */

#pragma once
#include <string>

namespace RTSPBridge
{
	public delegate void LogEventHandler(Platform::String^ str);
	public delegate void DataChunkEventHandler(Windows::Foundation::Collections::IVector<uint8>^ data);

    public ref class WindowsPhoneRuntimeComponent sealed
    {
	private:
		std::string currentStreamUri;

    public:
		event LogEventHandler^ LogEvent;
		event DataChunkEventHandler^ DataChunkEvent;

		WindowsPhoneRuntimeComponent();
		
		void OpenStream(Platform::String^ rtspStreamUri);
		void CloseStream();
    };
}