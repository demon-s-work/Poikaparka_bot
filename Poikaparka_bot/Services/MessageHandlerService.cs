using Microsoft.Extensions.Logging;
using Poikaparka.Extensions;
using Poikaparka.Abstractions.BaseServices;
using Telegram.Bot.Types;

namespace Poikaparka.Services
{
	public class MessageHandlerService(ILoggerFactory loggerFactory,
	                                   OperationContext operationContext,
	                                   CommandHandlerService _commandHandler) : BaseService(operationContext, loggerFactory)
	{
		public void HandleNew(Message message)
		{
			if (message.IsCommand(Constants.CommandPrefix))
			{
				_operationContext[ItemNames.UserId] = message.From.Id.ToString();
				_commandHandler.HandleCommand(message.Text.Replace(Constants.CommandPrefix, "").Replace($"@{BotName}", ""));
			}
		}
	}
}