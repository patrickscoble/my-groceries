using Android.Content;
using Android.Views;
using MyGroceries.Enums;
using MyGroceries.Helpers;
using MyGroceries.Models;

namespace MyGroceries.Adapters
{
	internal class ItemAdapter : BaseAdapter<Item>
	{
		private ItemActivity _itemActivity;
		private DbHelper _dbHelper;

		public ItemAdapter(ItemActivity itemActivity, List<Item> items, DbHelper dbHelper)
			: base(items)
        {
            this._itemActivity = itemActivity;
			this._dbHelper = dbHelper;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			LayoutInflater inflater = (LayoutInflater)_itemActivity.GetSystemService(Context.LayoutInflaterService);
			View view = inflater.Inflate(Resource.Layout.item, null);

			Item item = Items[position];

			TextView name = view.FindViewById<TextView>(Resource.Id.item_name);
			name.Text = item.Name;
			name.PaintFlags = item.Done ? name.PaintFlags | Android.Graphics.PaintFlags.StrikeThruText : name.PaintFlags;

			// Set the image for the item type.
			ImageView imageView = view.FindViewById<ImageView>(Resource.Id.item_item_type);
			int resId = GetItemTypeImage(item.ItemType);
			imageView.SetImageResource(resId);

			CheckBox checkBox = view.FindViewById<CheckBox>(Resource.Id.item_done);
			checkBox.Checked = item.Done;
			checkBox.Click += delegate
			{
				item.Done = !item.Done;

				_dbHelper.UpdateItem(item);
				_itemActivity.LoadData();
			};

			return view;
		}

		private int GetItemTypeImage(ItemTypeEnum itemType)
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
