using EnvironmentService.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poikaparka.Dal;
using Telegram;
using Telegram.Services;

namespace Poikaparka_bot
{
	public static class Program
	{
		public static async Task Main()
		{
			var builder = Host.CreateApplicationBuilder();

			builder.Logging.AddConsole();

			builder.Configuration.AddEnvironmentVariables("POI_");

			builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection(nameof(TelegramSettings)));

			builder.Services.AddDbContext<ApplicationDbContext>(options => {
				options.UseMySQL(builder.Configuration.GetConnectionString("Default"));
			});

			builder.Services.AddSingleton<TelegramClientService>();
			builder.Services.AddHostedService<TelegramService>();
			builder.Services.AddScoped<CommandHandlerService>();
			builder.Services.AddScoped<MessageHandlerService>();

			builder.Services.AddSingleton<ResponseService>();
			await builder.Build().RunAsync();
		}
	}
}