using MyGroceries.Enums;

namespace MyGroceries.Adapters
{
	public abstract class BaseAdapter<T> : BaseAdapter
	{
		protected List<T> Items;

		public override int Count { get { return Items.Count; } }

		public BaseAdapter(List<T> items)
		{
			this.Items = items;
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return position;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		protected int GetItemTypeImage(ItemTypeEnum itemType)
		{
			if (itemType == ItemTypeEnum.FruitAndVeg) return Resource.Drawable.fruit_and_veg_24;
			if (itemType == ItemTypeEnum.MeatSeafoodAndDeli) return Resource.Drawable.meat_seafood_and_deli_24;
			if (itemType == ItemTypeEnum.Bakery) return Resource.Drawable.bakery_24;
			if (itemType == ItemTypeEnum.DairyEggsAndFridge) return Resource.Drawable.dairy_eggs_and_fridge_24;
			if (itemType == ItemTypeEnum.Pantry) return Resource.Drawable.pantry_24;
			if (itemType == ItemTypeEnum.Freezer) return Resource.Drawable.freezer_24;
			if (itemType == ItemTypeEnum.HealthAndBeauty) return Resource.Drawable.health_and_beauty_24;
			if (itemType == ItemTypeEnum.Household) return Resource.Drawable.household_24;

			return 0;
		}
	}
}
