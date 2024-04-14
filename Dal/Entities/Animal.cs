namespace Dal.Entities
{
	public class Animal : TrackedEntity<string>
	{
		public string Name { get; set; }
		public AnimalType Type { get; set; }
	}
}