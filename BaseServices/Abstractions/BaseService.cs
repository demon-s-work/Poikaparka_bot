using Microsoft.Extensions.Logging;
using Poikaparka.Services;

namespace Poikaparka.Abstractions.BaseServices
{
	public abstract class BaseService : LoggedService
	{
		protected readonly OperationContext _operationContext;
		protected string BotName => _operationContext[ItemNames.BotName];
		protected string UserId => _operationContext[ItemNames.UserId];

		protected BaseService(OperationContext operationContext, ILoggerFactory loggerFactory) : base(loggerFactory)
		{
			_operationContext = operationContext;
		}
	}
}