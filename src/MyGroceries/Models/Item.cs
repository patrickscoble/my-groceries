using MyGroceries.Enums;

namespace MyGroceries.Models
{
	public class Item
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public ItemTypeEnum ItemType { get; set; }

		public bool Done { get; set; }
	}
}
