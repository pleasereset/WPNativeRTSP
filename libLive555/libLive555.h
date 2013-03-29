/* Ree7 WPNativeRTSP
 * Copyright (C) 2013 Pierre BELIN <pierre@ree7.fr>, and project contributors.
 *
 * Distributed under the Microsoft Public License (Ms-PL).
 */

#pragma once
#include <functional>
#include <string>

// Configuration object
typedef struct libLive555_config
{
	std::function<void (const unsigned char*, int)> DataChunkFunc;
	std::function<void (const char*)> LoggingFunc;
} LibLive555Config;


void ConfigureRtspLib(const LibLive555Config& configuration);
void OpenRtspStream(const char* uri);
void CloseRtspStream(const char* uri);
