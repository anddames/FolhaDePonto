using Microsoft.AspNetCore.Builder;

namespace FolhaDePonto
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
                    webBuilder.UseDefaultServiceProvider(options =>
                        options.ValidateScopes = false); //opcional, dependendo da vers�o do .NET Core utilizada
                });
    }
}
