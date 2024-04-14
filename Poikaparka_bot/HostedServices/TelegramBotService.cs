using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Poikaparka.Abstractions.BaseServices;
using Poikaparka.Services;
using Poikaparka.Settings;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Poikaparka.HostedServices
{
	public class TelegramBotService : LoggedService, IHostedService
	{
		private readonly TelegramSettings _settings;
		private readonly IServiceScopeFactory _scopeFactory;
		private TelegramBotClient _telegramBot;

		public TelegramBotService(ILoggerFactory loggerFactory,
		                          IOptions<TelegramSettings> settings,
		                          IServiceScopeFactory scopeFactory) : base(loggerFactory)
		{
			_scopeFactory = scopeFactory;
			_settings = settings.Value;
			_telegramBot = new TelegramBotClient(_settings.ApiKey);
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_telegramBot.StartReceiving(cancellationToken: cancellationToken,
				updateHandler: UpdateHandler,
				pollingErrorHandler: PollingErrorHandler,
				receiverOptions: new ReceiverOptions());
		}

		private async Task PollingErrorHandler(ITelegramBotClient client, Exception ex, CancellationToken ct)
		{
			_logger.LogError(ex.Message, ex);
		}

		private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken ct)
		{
			using var scope = _scopeFactory.CreateScope();
			var opContext = scope.ServiceProvider.GetService<OperationContext>();

			var bot = await _telegramBot.GetMeAsync(cancellationToken: ct);
			opContext[ItemNames.BotName] = bot.Username;

			switch (update.Type)
			{
				case UpdateType.Unknown:
					break;
				case UpdateType.Message:
					var _messageHandler = scope.ServiceProvider.GetService<MessageHandlerService>();
					_messageHandler.HandleNew(update.Message);
					break;
			}
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _telegramBot.CloseAsync(cancellationToken: cancellationToken);
			_telegramBot = null;
		}
	}
}