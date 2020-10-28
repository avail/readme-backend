using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:7477")
                .Build()
                .Run();
        }            
    }
}
