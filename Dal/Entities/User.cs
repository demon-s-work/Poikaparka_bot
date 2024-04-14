using System.ComponentModel.DataAnnotations;

namespace Dal.Entities
{
	public class User : TrackedEntity<string>
	{
		[Required]
		public string TelegramId { get; set; }
	}
}