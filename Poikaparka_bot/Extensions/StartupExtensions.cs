using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Poikaparka.Abstractions.BaseServices;

namespace Poikaparka.Extensions
{
	public static class StartupExtensions
	{
		public static IServiceCollection ConfigureBaseServices(this IServiceCollection services)
		{
			foreach (var iType in Assembly
			                      .GetExecutingAssembly()
			                      .GetTypes()
			                      .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(typeof(BaseService))))
			{
				services.AddTransient(iType);
			}
			return services;
		}
	}
}