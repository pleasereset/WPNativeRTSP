/* Ree7 WPNativeRTSP
 * Copyright (C) 2013 Pierre BELIN <pierre@ree7.fr>, and project contributors.
 *
 * Distributed under the Microsoft Public License (Ms-PL).
 */

#pragma once
#include "BasicUsageEnvironment.hh"
#include <functional>

class WPUsageEnvironment: public BasicUsageEnvironment {
public:
  static WPUsageEnvironment* createNew(TaskScheduler& taskScheduler, std::function<void (const char*)> textOutputFunc);

  virtual UsageEnvironment& operator<<(char const* str);
  virtual UsageEnvironment& operator<<(int i);
  virtual UsageEnvironment& operator<<(unsigned u);
  virtual UsageEnvironment& operator<<(double d);
  virtual UsageEnvironment& operator<<(void* p);

protected:
  WPUsageEnvironment(TaskScheduler& taskScheduler, std::function<void (const char*)> textOutputFunc);
      // called only by "createNew()" (or subclass constructors)
  virtual ~WPUsageEnvironment();

  std::function<void (const char*)> textOutputFunc;
};