using BaseServices;
using EnvironmentService.Response;
using Microsoft.Extensions.Logging;
using Poikaparka.Extensions;
using Telegram.Bot.Types;

namespace Telegram.Services
{
	public class MessageHandlerService(
		ILoggerFactory loggerFactory,
		CommandHandlerService _commandHandler,
		ResponseService _responseService) : BaseService(loggerFactory)
	{
		public async Task HandleNew(Message message, string botName)
		{
			if (message.IsCommand("/"))
			{
				await _commandHandler.HandleCommand(message.Text.Replace("/", "").Replace($"@{botName}", "")
					, message.From.Id.ToString()
					, message.Chat.Id.ToString());
			}
			else
			{
				_responseService.UserResponsed(message.From.Id.ToString(), message.Text);
			}
		}
	}
}