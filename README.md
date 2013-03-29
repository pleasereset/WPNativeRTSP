WPNativeRTSP
============

Disclaimer : This is a work in progress / unfinished work.

Port of Live555 Streaming Media (http://www.live555.com/) on Windows Phone - WinPRT.

- libLive555 is the original Live555 as a static lib compiled with Windows Phone 8 C++ compiler
- MPEGTransportDecoder.Phone is a WinRT Component to demux an MPEG TS (TODO)
- RTSPBridge.Phone is a WinRT Component that will open and handle a RTSP/RTP stream and report content buffers

- RTSPMediaStreamSource.Phone is an attempt to make a MediaStreamSource based on the previous project

- RTSPVideoApp is the test application