using System;
using Serilog;

namespace ProjectServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            Server server = new Server();
            server.Start();
        }
    }
}
