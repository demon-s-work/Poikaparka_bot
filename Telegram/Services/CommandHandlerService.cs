using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using BaseServices;
using EnvironmentService.Response;
using Microsoft.Extensions.Logging;
using Telegram.Attributes;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Services
{
	public class CommandHandlerService(
		ILoggerFactory loggerFactory,
		ResponseService _responseService,
		TelegramClientService _telegramClient) : BaseService(loggerFactory)
	{
		public async Task HandleCommand(string command, string userId, string chatId)
		{
			_logger.LogInformation($"User id: {userId} command {command}");
			var methodInfo = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(m =>
				m.GetCustomAttributes(typeof(TelegramCommandAttribute), true)[0] is not null && m.Name.ToLower() == command);
			if (methodInfo is not null)
				if (methodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(AsyncStateMachineAttribute)))
					await (methodInfo.Invoke(this, [userId, chatId]) as Task);
				else
					methodInfo.Invoke(this, [userId, chatId]);
		}

		[TelegramCommand]
		private void Feed(string userId, string chatId)
		{
		}

		[TelegramCommand]
		private async Task NewPet(string userId, string chatId)
		{
			await _telegramClient.SendTextMessageAsync(chatId, text: "Enter name");
			_responseService.WaitForResponse(userId, async s => {
				await _telegramClient.SendTextMessageAsync(chatId, $"Name {s} free", new InlineKeyboardMarkup(
					InlineKeyboardButton.WithCallbackData("Accept?", )));
			});
		}
	}
}