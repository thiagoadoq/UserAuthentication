using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace RegistrationUser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseKestrel()
                .UseUrls("http://localhost:5004/")
                .UseStartup<Startup>();
    }
}
