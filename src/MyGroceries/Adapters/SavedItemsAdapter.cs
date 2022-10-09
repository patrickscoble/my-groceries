using Android.Content;
using Android.Views;
using MyGroceries.Helpers;
using MyGroceries.Models;

namespace MyGroceries.Adapters
{
	internal class SavedItemAdapter : BaseAdapter<Item>
	{
		private SavedItemActivity _savedItemActivity;

		public SavedItemAdapter(SavedItemActivity savedItemActivity, List<Item> items, DbHelper dbHelper)
			: base(items)
        {
            this._savedItemActivity = savedItemActivity;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			LayoutInflater inflater = (LayoutInflater)_savedItemActivity.GetSystemService(Context.LayoutInflaterService);
			View view = inflater.Inflate(Resource.Layout.item, null);

			Item item = Items[position];

			TextView name = view.FindViewById<TextView>(Resource.Id.item_name);
			name.Text = item.Name;

			// Set the image for the item type.
			ImageView imageView = view.FindViewById<ImageView>(Resource.Id.item_item_type);
			int resId = GetItemTypeImage(item.ItemType);
			imageView.SetImageResource(resId);

			CheckBox checkBox = view.FindViewById<CheckBox>(Resource.Id.item_done);
			checkBox.Checked = item.Done;
			checkBox.Click += delegate
			{
				_savedItemActivity.SelectedItems.Add(item);
			};

			view.Click += delegate
			{
				LayoutInflater layoutInflater = LayoutInflater.From(_savedItemActivity);
				View view = layoutInflater.Inflate(Resource.Layout.update_item, null);

				AlertDialog.Builder builder = new AlertDialog.Builder(_savedItemActivity);
				builder.SetTitle("Update Saved Item");
				builder.SetView(view);
				builder.SetPositiveButton("Update", _savedItemActivity.UpdateSavedItemAction);
				builder.SetNeutralButton("Delete", _savedItemActivity.DeleteSavedItemAction);
				builder.SetNegativeButton("Cancel", _savedItemActivity.CancelAction);

				// Populate the dropdown.
				ArrayAdapter adapter = ArrayAdapter.CreateFromResource(_savedItemActivity, Resource.Array.item_types, Android.Resource.Layout.SimpleSpinnerItem);
				adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
				Spinner spinner = view.FindViewById<Spinner>(Resource.Id.update_item_item_type);
				spinner.Adapter = adapter;
				spinner.SetSelection((int)item.ItemType);

				// Prepopulate the fields.
				view.FindViewById<TextView>(Resource.Id.update_item_id).Text = item.Id.ToString();
				view.FindViewById<TextView>(Resource.Id.update_item_name).Text = item.Name;

				builder.Show();
			};

			return view;
		}
	}
}
