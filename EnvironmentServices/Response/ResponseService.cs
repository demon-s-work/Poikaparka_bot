using System.Collections.Concurrent;
using BaseServices;
using Microsoft.Extensions.Logging;

namespace EnvironmentService.Response
{
	public class ResponseService : BaseService
	{
		private ConcurrentDictionary<string, Action<string>> Waiters { get; set; } = new ConcurrentDictionary<string, Action<string>>();

		public ResponseService(ILoggerFactory loggerFactory) : base(loggerFactory)
		{
		}

		public void WaitForResponse(string userId, Action<string> then)
		{
			Waiters.TryAdd(userId, then);
		}

		public void UserResponsed(string userId, string text)
		{
			Waiters.TryRemove(userId, out var a);
			a?.Invoke(text);
		}

		public void UnwaitResponse(string userId)
		{
			Waiters.TryRemove(userId, out var _);
		}
	}
}