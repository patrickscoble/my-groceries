using System.ComponentModel;

namespace MyGroceries.Enums
{
	public enum ItemTypeEnum
	{
		[Description("Fruit & Veg")]
		FruitAndVeg,
		[Description("Meat, Seafood & Deli")]
		MeatSeafoodAndDeli,
		[Description("Bakery")]
		Bakery,
		[Description("Dairy, Eggs & Fridge")]
		DairyEggsAndFridge,
		[Description("Pantry")]
		Pantry,
		[Description("Freezer")]
		Freezer,
		[Description("Health & Beauty")]
		HealthAndBeauty,
		[Description("Household")]
		Household,
		[Description("All")]
		All,
	}
}
