using Android.Content;
using Android.Views;
using MyGroceries.Adapters;
using MyGroceries.Enums;
using MyGroceries.Helpers;
using MyGroceries.Models;

namespace MyGroceries
{
	[Activity(Label = "@string/title_saved_items")]
	public class SavedItemActivity : Activity
	{
		private ListView _savedItemsListView;
		private DbHelper _dbHelper;

		public List<Item> SelectedItems;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.saved_items);

			this._savedItemsListView = FindViewById<ListView>(Resource.Id.saved_item_list);
			this._dbHelper = new DbHelper(this);

			this.SelectedItems = new List<Item>();

			LoadData();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.saved_items_menu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem menuItem)
		{
			switch (menuItem.ItemId)
			{
				case Resource.Id.action_cancel:
				{
					Finish();
					return true;
				}
				case Resource.Id.action_done:
				{
					foreach (Item item in SelectedItems)
					{
						_dbHelper.CreateItem(item);
					}

					Finish();
					return true;
				}
			}
			return base.OnOptionsItemSelected(menuItem);
		}

		public void UpdateSavedItemAction(object sender, DialogClickEventArgs e)
		{
			AlertDialog alertDialog = (AlertDialog)sender;

			string id = alertDialog.FindViewById<TextView>(Resource.Id.update_item_id).Text;
			string name = alertDialog.FindViewById<EditText>(Resource.Id.update_item_name).Text;
			ItemTypeEnum itemType = (ItemTypeEnum)alertDialog.FindViewById<Spinner>(Resource.Id.update_item_item_type).SelectedItemPosition;

			Item item = new Item()
			{
				Id = Convert.ToInt32(id),
				Name = name,
				ItemType = itemType,
			};

			_dbHelper.UpdateSavedItem(item);
			LoadData();
		}

		public void DeleteSavedItemAction(object sender, DialogClickEventArgs e)
		{
			AlertDialog alertDialog = (AlertDialog)sender;

			string id = alertDialog.FindViewById<TextView>(Resource.Id.update_item_id).Text;
			string name = alertDialog.FindViewById<EditText>(Resource.Id.update_item_name).Text;

			_dbHelper.DeleteSavedItem(Convert.ToInt32(id));
			LoadData();

			string text = $"{name} has been deleted";
			Toast.MakeText(Application, text, ToastLength.Short).Show();
		}

		public void CancelAction(object sender, DialogClickEventArgs e)
		{
		}

		public void LoadData()
		{
			List<Item> items = _dbHelper.GetAllSavedItems().OrderBy(item => item.ItemType).ThenBy(item => item.Name).ToList();
			SavedItemAdapter savedItemAdapter = new SavedItemAdapter(this, items, _dbHelper);
			_savedItemsListView.Adapter = savedItemAdapter;
		}
	}
}
