using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poikaparka.Attributes;
using Poikaparka.Dal;
using Poikaparka.Extensions;
using Poikaparka.HostedServices;
using Poikaparka.Services;
using Poikaparka.Settings;

namespace Poikaparka
{
	public static class Program
	{
		public static async Task Main()
		{
			var builder = Host.CreateApplicationBuilder();

			builder.Logging.AddConsole();

			builder.Configuration.AddEnvironmentVariables("POI_");

			builder.Services.AddDbContext<ApplicationDbContext>(options => {
				options.UseMySQL(builder.Configuration.GetConnectionString("Default"));
			});

			builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection(nameof(TelegramSettings)));

			builder.Services.ConfigureBaseServices();

			builder.Services.AddScoped<OperationContext>();

			builder.Services.AddHostedService<TelegramBotService>();

			await builder.Build().RunAsync();
		}
	}
}