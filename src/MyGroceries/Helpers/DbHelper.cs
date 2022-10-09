using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using MyGroceries.Enums;
using MyGroceries.Models;
using Newtonsoft.Json;

namespace MyGroceries.Helpers
{
	public class DbHelper : SQLiteOpenHelper
	{
		private static string DB_NAME = "MyGroceries";
		private static int DB_VERSION = 1;

		private static string DB_TABLE_ITEM = "Item";
		private static string DB_ITEM_COLUMN_ID = "Id";
		private static string DB_ITEM_COLUMN_NAME = "Name";
		private static string DB_ITEM_COLUMN_ITEM_TYPE = "ItemType";
		private static string DB_ITEM_COLUMN_DONE = "Done";

		private static string _data;

		public DbHelper(Context context)
			: base(context, DB_NAME, null, DB_VERSION)
		{
			using (StreamReader reader = new StreamReader(context.Assets.Open("Data/items.json")))
			{
				_data = reader.ReadToEnd();
			}
		}

		public override void OnCreate(SQLiteDatabase db)
		{
			string query = $@"CREATE TABLE {DB_TABLE_ITEM} ({DB_ITEM_COLUMN_ID} INTEGER PRIMARY KEY AUTOINCREMENT, {DB_ITEM_COLUMN_NAME} TEXT NOT NULL, {DB_ITEM_COLUMN_ITEM_TYPE} TEXT NOT NULL, {DB_ITEM_COLUMN_DONE} BOOLEAN NOT NULL);";
			db.ExecSQL(query);

			// Preopulate the DB with default data.
			List<Item> items = JsonConvert.DeserializeObject<List<Item>>(_data);
			foreach (Item item in items)
			{
				ContentValues values = new ContentValues();
				values.Put(DB_ITEM_COLUMN_NAME, item.Name);
				values.Put(DB_ITEM_COLUMN_ITEM_TYPE, item.ItemType.ToString());
				values.Put(DB_ITEM_COLUMN_DONE, false);
				db.Insert(DB_TABLE_ITEM, null, values);
			}
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			string query = $"DELETE TABLE IF EXISTS {DB_TABLE_ITEM}";
			db.ExecSQL(query);

			OnCreate(db);
		}

		public List<Item> GetAllItems()
		{
			List<Item> items = new List<Item>();
			SQLiteDatabase db = this.ReadableDatabase;
			ICursor cursor = db.Query(DB_TABLE_ITEM, new string[] { DB_ITEM_COLUMN_ID, DB_ITEM_COLUMN_NAME, DB_ITEM_COLUMN_ITEM_TYPE, DB_ITEM_COLUMN_DONE }, null, null, null, null, null);

			while (cursor.MoveToNext())
			{
				int idIndex = cursor.GetColumnIndex(DB_ITEM_COLUMN_ID);
				int id = cursor.GetInt(idIndex);

				int nameIndex = cursor.GetColumnIndex(DB_ITEM_COLUMN_NAME);
				string name = cursor.GetString(nameIndex) ?? string.Empty;

				int itemTypeIndex = cursor.GetColumnIndex(DB_ITEM_COLUMN_ITEM_TYPE);
				string itemType = cursor.GetString(itemTypeIndex) ?? string.Empty;

				int doneIndex = cursor.GetColumnIndex(DB_ITEM_COLUMN_DONE);
				bool done = cursor.GetInt(doneIndex) > 0;

				items.Add(new Item()
				{
					Id = id,
					Name = name,
					ItemType = Enum.Parse<ItemTypeEnum>(itemType),
					Done = done,
				});
			}

			return items;
		}

		public void CreateItem(Item item)
		{
			SQLiteDatabase db = this.WritableDatabase;
			ContentValues values = new ContentValues();
			values.Put(DB_ITEM_COLUMN_NAME, item.Name);
			values.Put(DB_ITEM_COLUMN_ITEM_TYPE, item.ItemType.ToString());
			values.Put(DB_ITEM_COLUMN_DONE, item.Done);
			db.Insert(DB_TABLE_ITEM, null, values);
			db.Close();
		}

		public void UpdateItem(Item item)
		{
			SQLiteDatabase db = this.WritableDatabase;
			ContentValues values = new ContentValues();
			values.Put(DB_ITEM_COLUMN_NAME, item.Name);
			values.Put(DB_ITEM_COLUMN_ITEM_TYPE, item.ItemType.ToString());
			values.Put(DB_ITEM_COLUMN_DONE, item.Done);
			db.Update(DB_TABLE_ITEM, values, $"{DB_ITEM_COLUMN_ID} = ?", new string[] { item.Id.ToString() });
			db.Close();
		}

		public void DeleteAllItems()
		{
			SQLiteDatabase db = this.WritableDatabase;
			db.Delete(DB_TABLE_ITEM, null, null);
			db.Close();
		}

		public void DeleteItem(int id)
		{
			SQLiteDatabase db = this.WritableDatabase;
			db.Delete(DB_TABLE_ITEM, $"{DB_ITEM_COLUMN_ID} = ?", new string[] { id.ToString() });
			db.Close();
		}
	}
}