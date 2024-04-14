using Microsoft.Extensions.Logging;

namespace BaseServices
{
	public abstract class BaseService
	{
		protected readonly ILogger _logger;

		public BaseService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger(GetType().Name);
		}
	}
}