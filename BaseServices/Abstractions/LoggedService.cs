using Microsoft.Extensions.Logging;

namespace Poikaparka.Abstractions.BaseServices
{
	public class LoggedService
	{
		protected readonly ILogger _logger;

		public LoggedService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger(GetType().Name);
		}
	}
}