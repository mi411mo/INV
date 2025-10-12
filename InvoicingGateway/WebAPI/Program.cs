using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
                 webBuilder.UseSetting("detailedErrors", "true");
                 webBuilder.CaptureStartupErrors(true);
                 webBuilder.UseIIS();
                 webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config
                     .AddJsonFile($"ocelot.json", false, true)
                     .AddJsonFile($"appsettings.json", false, true)
                     .AddEnvironmentVariables();

                 });
                 webBuilder.ConfigureLogging(logging =>
                 {
                     logging.ClearProviders();
                     logging.AddLog4Net("log4net.config");
                 });

                 var configuration = new ConfigurationBuilder()
                 .AddEnvironmentVariables()
                 .AddCommandLine(args)
                 .AddJsonFile("appsettings.json")
                 .Build();
                 var HTTPSSupportedStr = configuration.GetSection("Certificate")["SupportHTTPS"];
                 bool.TryParse(HTTPSSupportedStr, out bool isHTTPSSupported);
                 if (isHTTPSSupported)
                 {
                     var certificatePath = configuration.GetSection("Certificate")["Path"];
                     var certificatePass = configuration.GetSection("Certificate")["Password"];

                     var apiGatewayPublishHost = Environment.GetEnvironmentVariable("OrganizationsCorePublishHost", EnvironmentVariableTarget.Machine).Replace("https://", "").Split(":");
                     string ip = apiGatewayPublishHost[0];
                      //string ip = "localhost";// "172.16.11.190";
                      int port = Int32.Parse(apiGatewayPublishHost[1]);//
                                                                       //int port = 54444;// Int32.Parse("8800");
                      webBuilder.ConfigureKestrel(serverOptions =>
                     {
                         if (ip.Equals("localhost"))
                             serverOptions.ListenLocalhost(port, listenOptions =>
                             {
                                 listenOptions.UseHttps(certificatePath, certificatePass);
                             });
                         else
                         {
                             IPAddress.TryParse(ip, out var ipadd);
                             serverOptions.Listen(ipadd, port, listenOptions =>
                             {
                                 listenOptions.UseHttps(certificatePath, certificatePass);
                             });
                         }
                     });
                 }
             });
    }
}
