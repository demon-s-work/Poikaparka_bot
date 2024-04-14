using Telegram.Bot.Types;

namespace Poikaparka.Extensions
{
	public static class MessageExtension
	{
		public static bool IsCommand(this Message message, string prefix)
		{
			return !string.IsNullOrEmpty(message.Text) && message.Text.StartsWith(prefix);
		}
	}
}