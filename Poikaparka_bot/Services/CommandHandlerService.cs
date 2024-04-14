using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Poikaparka.Abstractions.BaseServices;
using Poikaparka.Attributes;

namespace Poikaparka.Services
{
	public class CommandHandlerService : BaseService
	{
		public CommandHandlerService(ILoggerFactory loggerFactory,
		                             OperationContext operationContext) : base(operationContext, loggerFactory)
		{
		}

		public void HandleCommand(string command)
		{
			var watch = Stopwatch.StartNew();
			_logger.LogInformation($"User id: {UserId} command {command}");
			var methodInfo = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(m =>
				m.GetCustomAttributes(typeof(TelegramCommandAttribute), true)[0] is not null && m.Name.ToLower() == command);
			if (methodInfo is not null)
				methodInfo.Invoke(this, null);
			watch.Stop();
			_logger.LogInformation(watch.ElapsedTicks.ToString());
		}

		[TelegramCommand]
		private void Feed()
		{
		}

		[TelegramCommand]
		private void NewPet()
		{
		}
	}
}