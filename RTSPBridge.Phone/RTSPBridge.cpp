/* Ree7 WPNativeRTSP
 * Copyright (C) 2013 Pierre BELIN <pierre@ree7.fr>, and project contributors.
 *
 * Distributed under the Microsoft Public License (Ms-PL).
 */

// RTSPBridge.cpp
#include "pch.h"
#include "RTSPBridge.h"
#include "libLive555.h"

#include <iostream>
#include <string>
#include <collection.h>

using namespace RTSPBridge;
using namespace Platform;
using namespace Platform::Collections;

WindowsPhoneRuntimeComponent::WindowsPhoneRuntimeComponent()
{
	LibLive555Config cfg;
	
	// Setup the logging func to channel log messages back to the C# part
	cfg.LoggingFunc = [&] (const char* message) { 
		// Convert the const char* to a Platform::String^
		int wchars_num =  MultiByteToWideChar( CP_UTF8 , 0 , message , -1, NULL , 0 );
		wchar_t* wstr = new wchar_t[wchars_num];
		MultiByteToWideChar( CP_UTF8 , 0 , message, -1, wstr , wchars_num );

		LogEvent(ref new String(wstr));
	};

	cfg.DataChunkFunc = [&] (const uint8* data, int count) {
		Vector<uint8>^ vec = ref new Vector<uint8>(data, count);
		DataChunkEvent(vec);
	};

	ConfigureRtspLib(cfg);
}

void WindowsPhoneRuntimeComponent::OpenStream(String^ rtspStreamUri)
{
	LogEvent(L"RTSPBridge::OpenStream");

	// Convert the Platform::String^ to an ANSI const char*
	int strLength = WideCharToMultiByte(CP_ACP, WC_COMPOSITECHECK, rtspStreamUri->Data(), -1, NULL, 0,  NULL, NULL);
	std::string rtspStreamUri2(strLength + 1, 0);
	strLength = WideCharToMultiByte(CP_ACP, WC_COMPOSITECHECK, rtspStreamUri->Data(), -1, &rtspStreamUri2[0], strLength+1, NULL, NULL);

	currentStreamUri = rtspStreamUri2;

	OpenRtspStream(currentStreamUri.c_str());
}

void WindowsPhoneRuntimeComponent::CloseStream()
{
	CloseRtspStream(currentStreamUri.c_str());
}