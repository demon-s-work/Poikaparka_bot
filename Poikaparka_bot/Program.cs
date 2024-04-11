using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Paikaparka
{
	public static class Program
	{
		private static string ApiKey = "";
		public static async Task Main(string[] args)
		{
			var botClient = new TelegramBotClient(ApiKey);

			var cts = new CancellationTokenSource();

			botClient.StartReceiving(
				updateHandler: UpdateHandler,
				pollingErrorHandler: PollingErrorHandler,
				cancellationToken: cts.Token,
				receiverOptions: new ReceiverOptions
				{
					AllowedUpdates = Array.Empty<UpdateType>()
				});

			Console.Read();
			await cts.CancelAsync();
		}

		private static async Task PollingErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
		{
			Console.WriteLine("Dead");
		}

		private static async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken ct)
		{
			Console.WriteLine($"{update.Type} {update.Message.Text}");
		}
	}
}