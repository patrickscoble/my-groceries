using Android.Content;
using Android.Views;
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

			view.Click += delegate
			{
				LayoutInflater layoutInflater = LayoutInflater.From(_itemActivity);
				View view = layoutInflater.Inflate(Resource.Layout.update_item, null);

				AlertDialog.Builder builder = new AlertDialog.Builder(_itemActivity);
				builder.SetTitle("Update Item");
				builder.SetView(view);
				builder.SetPositiveButton("Update", _itemActivity.UpdateItemAction);
				builder.SetNeutralButton("Delete", _itemActivity.DeleteItemAction);
				builder.SetNegativeButton("Cancel", _itemActivity.CancelAction);

				// Populate the dropdown.
				ArrayAdapter adapter = ArrayAdapter.CreateFromResource(_itemActivity, Resource.Array.create_update_item_item_types, Android.Resource.Layout.SimpleSpinnerItem);
				adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
				Spinner spinner = view.FindViewById<Spinner>(Resource.Id.update_item_item_type);
				spinner.Adapter = adapter;
				spinner.SetSelection((int)item.ItemType);

				// Prepopulate the fields.
				view.FindViewById<TextView>(Resource.Id.update_item_id).Text = item.Id.ToString();
				view.FindViewById<TextView>(Resource.Id.update_item_name).Text = item.Name;
				view.FindViewById<CheckBox>(Resource.Id.update_item_done).Checked = item.Done;

				builder.Show();
			};

			return view;
		}
	}
}
