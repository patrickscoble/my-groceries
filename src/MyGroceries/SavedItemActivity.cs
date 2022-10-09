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
		private ItemTypeEnum _itemTypeFilter;

		public List<Item> SelectedItems;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.saved_items);

			this._savedItemsListView = FindViewById<ListView>(Resource.Id.saved_item_list);
			this._dbHelper = new DbHelper(this);

			this.SelectedItems = new List<Item>();

			Spinner itemTypeFilter = FindViewById<Spinner>(Resource.Id.filter_item_item_type);

			// Populate the dropdown.
			ArrayAdapter adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.filter_item_types, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			itemTypeFilter.Adapter = adapter;

			itemTypeFilter.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
			itemTypeFilter.SetSelection((int)ItemTypeEnum.All);

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

		private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			_itemTypeFilter = (ItemTypeEnum)e.Position;
			LoadData();
		}

		public void LoadData()
		{
			List<Item> items = _dbHelper.GetAllSavedItems();

			items = _itemTypeFilter == ItemTypeEnum.All ?
				items.OrderBy(item => item.ItemType).ThenBy(item => item.Name).ToList() :
				items.Where(item => item.ItemType == _itemTypeFilter).OrderBy(item => item.Name).ToList();

			SavedItemAdapter savedItemAdapter = new SavedItemAdapter(this, items, _dbHelper);
			_savedItemsListView.Adapter = savedItemAdapter;
		}
	}
}
