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
	}
}
