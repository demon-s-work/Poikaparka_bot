namespace Poikaparka.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class TelegramCommandAttribute : Attribute
	{
		public TelegramCommandAttribute(string commandName)
		{
			CommandName = commandName;
		}

		public TelegramCommandAttribute()
		{

		}
		public string CommandName { get; set; }
	}
}