/* Ree7 WPNativeRTSP
 * Copyright (C) 2013 Pierre BELIN <pierre@ree7.fr>, and project contributors.
 *
 * Distributed under the Microsoft Public License (Ms-PL).
 */

#include "WPUsageEnvironment.h"
#include <string>

WPUsageEnvironment::WPUsageEnvironment(TaskScheduler& taskScheduler, std::function<void (const char*)> textOutputFunc)
	: BasicUsageEnvironment(taskScheduler) {
		this->textOutputFunc = textOutputFunc;
}

WPUsageEnvironment::~WPUsageEnvironment() {
}

WPUsageEnvironment* WPUsageEnvironment::createNew(TaskScheduler& taskScheduler, std::function<void (const char*)> textOutputFunc) {
	return new WPUsageEnvironment(taskScheduler, textOutputFunc);
}

UsageEnvironment& WPUsageEnvironment::operator<<(char const* str) {
	if (str == NULL) str = "(NULL)"; // sanity check
	textOutputFunc(str);
	return *this;
}

UsageEnvironment& WPUsageEnvironment::operator<<(int i) {
	textOutputFunc(std::to_string(i).c_str());
	return *this;
}

UsageEnvironment& WPUsageEnvironment::operator<<(unsigned u) {
	textOutputFunc(std::to_string(u).c_str());
	return *this;
}

UsageEnvironment& WPUsageEnvironment::operator<<(double d) {
	textOutputFunc(std::to_string(d).c_str());
	return *this;
}

UsageEnvironment& WPUsageEnvironment::operator<<(void* p) {
	textOutputFunc(std::to_string((int)p).c_str()); // TODO : rewrite as hex
	return *this;
}