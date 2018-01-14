using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace PlexLander
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all 
            var logger = NLogBuilder.ConfigureNLog("NLog.debug.config").GetCurrentClassLogger();
            logger.Log(NLog.LogLevel.Info, "Building server");

            try
            {
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseNLog()
                    .UseApplicationInsights()
                    .Build();

                logger.Log(NLog.LogLevel.Info, "Starting server");
                host.Run();
            } catch (Exception e)
            {
                logger.Log(NLog.LogLevel.Fatal, e, "An exception has occured while building or starting the server.");
            }
            logger.Log(NLog.LogLevel.Info, "Server was shut down.");
        }
    }
}
