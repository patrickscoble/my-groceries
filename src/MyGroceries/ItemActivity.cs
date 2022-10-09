using Android.Content;
using Android.Views;
using MyGroceries.Adapters;
using MyGroceries.Enums;
using MyGroceries.Helpers;
using MyGroceries.Models;

namespace MyGroceries
{
	[Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/freezer_48")]
	public class ItemActivity : Activity
	{
		private ListView _itemsListView;
		private DbHelper _dbHelper;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.items);

			this._itemsListView = FindViewById<ListView>(Resource.Id.item_list);
			this._dbHelper = new DbHelper(this);

			LoadData();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.items_menu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_clear_all_items:
				{
					AlertDialog.Builder builder = new AlertDialog.Builder(this);
					builder.SetTitle("Clear All");
					builder.SetMessage("Are you sure you want to clear all items?");
					builder.SetPositiveButton("OK", ClearAllItemsAction);
					builder.SetNegativeButton("Cancel", CancelAction);

					builder.Show();
					return true;
				}
				case Resource.Id.action_add_item:
				{
					LayoutInflater layoutInflater = LayoutInflater.From(this);
					View view = layoutInflater.Inflate(Resource.Layout.create_update_item, null);

					AlertDialog.Builder builder = new AlertDialog.Builder(this);
					builder.SetTitle("Create Item");
					builder.SetView(view);
					builder.SetPositiveButton("Create", CreateItemAction);
					builder.SetNegativeButton("Cancel", CancelAction);

					// Populate the dropdown.
					ArrayAdapter adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.item_types, Android.Resource.Layout.SimpleSpinnerItem);
					adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
					view.FindViewById<Spinner>(Resource.Id.create_update_item_item_type).Adapter = adapter;

					builder.Show();
					return true;
				}
			}
			return base.OnOptionsItemSelected(item);
		}

		private void ClearAllItemsAction(object sender, DialogClickEventArgs e)
		{
			_dbHelper.DeleteAllItems();
			LoadData();
		}

		private void CreateItemAction(object sender, DialogClickEventArgs e)
		{
			AlertDialog alertDialog = (AlertDialog)sender;

			string name = alertDialog.FindViewById<EditText>(Resource.Id.create_update_item_name).Text;
			ItemTypeEnum itemType = (ItemTypeEnum)alertDialog.FindViewById<Spinner>(Resource.Id.create_update_item_item_type).SelectedItemPosition;

			Item item = new Item()
			{
				Name = name,
				ItemType = itemType,
			};

			_dbHelper.CreateItem(item);
			LoadData();
		}

		public void UpdateItemAction(object sender, DialogClickEventArgs e)
		{
			AlertDialog alertDialog = (AlertDialog)sender;

			string id = alertDialog.FindViewById<TextView>(Resource.Id.create_update_item_id).Text;
			string name = alertDialog.FindViewById<EditText>(Resource.Id.create_update_item_name).Text;
			ItemTypeEnum itemType = (ItemTypeEnum)alertDialog.FindViewById<Spinner>(Resource.Id.create_update_item_item_type).SelectedItemPosition;
			bool done = alertDialog.FindViewById<CheckBox>(Resource.Id.create_update_item_done).Checked;

			Item item = new Item()
			{
				Id = Convert.ToInt32(id),
				Name = name,
				ItemType = itemType,
				Done = done,
			};

			_dbHelper.UpdateItem(item);
			LoadData();
		}

		public void DeleteItemAction(object sender, DialogClickEventArgs e)
		{
			AlertDialog alertDialog = (AlertDialog)sender;

			string id = alertDialog.FindViewById<TextView>(Resource.Id.create_update_item_id).Text;
			string name = alertDialog.FindViewById<EditText>(Resource.Id.create_update_item_name).Text;

			_dbHelper.DeleteItem(Convert.ToInt32(id));
			LoadData();

			string text = $"{name} has been deleted";
			Toast.MakeText(Application, text, ToastLength.Short).Show();
		}

		public void CancelAction(object sender, DialogClickEventArgs e)
		{
		}

		public void LoadData()
		{
			List<Item> items = _dbHelper.GetAllItems().OrderBy(item => item.Done).ThenBy(item => item.ItemType).ToList();
			ItemAdapter itemAdapter = new ItemAdapter(this, items, _dbHelper);
			_itemsListView.Adapter = itemAdapter;
		}
	}
}
