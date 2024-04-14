using System.Reflection;
using BaseServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DI
{
	public static class StartupExtensions
	{
		public static IServiceCollection ConfigureBaseServices(this IServiceCollection services, IHostApplicationBuilder builder)
		{
			foreach (var iType in Assembly
			                      .GetEntryAssembly()
			                      .GetTypes()
			                      .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(typeof(BaseService))))
			{
				services.AddScoped(iType);
			}

			return services;
		}
	}
}