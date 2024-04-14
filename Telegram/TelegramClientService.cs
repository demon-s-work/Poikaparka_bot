using BaseServices;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram
{
	public class TelegramClientService : BaseService
	{
		public TelegramBotClient _telegramBot;

		public TelegramClientService(ILoggerFactory loggerFactory) : base(loggerFactory)
		{
		}

		public void SetClient(TelegramBotClient client)
		{
			_telegramBot = client;
		}

		public async Task<User> GetMeAsync(CancellationToken ct)
		{
			return await _telegramBot.GetMeAsync(ct);
		}

		public async Task StopAsync(CancellationToken ct)
		{
			await _telegramBot.CloseAsync(ct);
			_telegramBot = null;
		}

		public async Task SendTextMessageAsync(string chatId, string text, IReplyMarkup replyMarkup = null)
		{
			await _telegramBot.SendTextMessageAsync(chatId: chatId, text: text, replyMarkup: replyMarkup);
		}
	}
}