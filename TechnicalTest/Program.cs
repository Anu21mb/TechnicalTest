using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TechnicalTest.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder<Startup>(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args, Action<IWebHostBuilder> builder = null) where TStartup : class
        {
            IHostBuilder webHostBuilder = Host.CreateDefaultBuilder(args)
                
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                    if (builder != null)
                        builder(webBuilder);

                });
            return webHostBuilder;
        }
    }
}
