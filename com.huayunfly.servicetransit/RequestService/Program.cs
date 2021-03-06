﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;
using MassTransit.Log4NetIntegration.Logging;
using Topshelf;
using Topshelf.Logging;

namespace com.huayunfly.servicetransit
{
    class Program
    {
        static int Main(string[] args)
        {
            ConfigureLogger();
            // Masstransit to use log4net
            Log4NetLogger.Use();
            // Topshelf to use log4net
            Log4NetLogWriterFactory.Use();
            // Topshelf HostFactory
            return (int)HostFactory.Run(x => x.Service<RequestService>());
        }

        static void ConfigureLogger()
        {
            const string logConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<log4net>
  <root>
    <level value=""INFO"" />
    <appender-ref ref=""console"" />
  </root>
  <appender name=""console"" type=""log4net.Appender.ColoredConsoleAppender"">
    <layout type=""log4net.Layout.PatternLayout"">
      <conversionPattern value=""%m%n"" />
    </layout>
  </appender>
</log4net>";
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(logConfig)))
            {
                XmlConfigurator.Configure(stream);
            }
        }
    }
}
