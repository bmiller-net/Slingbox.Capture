using System;
using Microsoft.Owin.Hosting;

namespace Slingbox.Capture
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Starting web server... ");

            StartWebServer();
            
            Console.WriteLine("FINISHED. Press any key to quit.");

            Console.ReadKey();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            
        }

        private static void StartWebServer()
        {
            var baseAddress = "http://localhost:9090/";

            WebApp.Start<OwinStartup>(url: baseAddress);
        }
    }
}
