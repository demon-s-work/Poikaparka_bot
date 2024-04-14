using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Services;

namespace Telegram
{
	public class TelegramService : IHostedService
	{
		private readonly TelegramSettings _settings;
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly ILogger _logger;
		private readonly TelegramClientService _clientService;

		public TelegramService(ILoggerFactory loggerFactory,
		                          IOptions<TelegramSettings> settings,
		                          IServiceScopeFactory scopeFactory,
		                          TelegramClientService clientService)
		{
			_logger = loggerFactory.CreateLogger(GetType().Name);
			_scopeFactory = scopeFactory;
			_clientService = clientService;
			_settings = settings.Value;
			_clientService = clientService;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_clientService.SetClient(InitNewClient());
		}

		private TelegramBotClient InitNewClient()
		{
			var client = new TelegramBotClient(_settings.ApiKey);
			client.StartReceiving(updateHandler: UpdateHandler,
				pollingErrorHandler: PollingErrorHandler,
				receiverOptions: new ReceiverOptions());

			return client;
		}

		private async Task PollingErrorHandler(ITelegramBotClient client, Exception ex, CancellationToken ct)
		{
			_logger.LogError(ex.Message, ex);
			_logger.LogInformation("Restarting");
			await _clientService.StopAsync(ct);
			_clientService.SetClient(InitNewClient());
		}

		private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken ct)
		{
			using var scope = _scopeFactory.CreateScope();

			var bot = await _clientService.GetMeAsync(ct);

			switch (update.Type)
			{
				case UpdateType.Unknown:
					break;
				case UpdateType.Message:
					var _messageHandler = scope.ServiceProvider.GetService<MessageHandlerService>();
					await _messageHandler.HandleNew(update.Message, bot.Username);
					break;
			}
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _clientService.StopAsync(cancellationToken);
		}
	}
}