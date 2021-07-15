using Android.Views;
using AndroidX.RecyclerView.Widget;
using System;

namespace CIT.Adapters
{
    class CaseAdapter : RecyclerView.Adapter
    {
        public event EventHandler<CaseAdapterClickEventArgs> ItemClick;
        public event EventHandler<CaseAdapterClickEventArgs> ItemLongClick;
        string[] items;

        public CaseAdapter(string[] data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            //var id = Resource.Layout.__YOUR_ITEM_HERE;
            //itemView = LayoutInflater.From(parent.Context).
            //       Inflate(id, parent, false);

            var vh = new CaseAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as CaseAdapterViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => items.Length;

        void OnClick(CaseAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(CaseAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class CaseAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }


        public CaseAdapterViewHolder(View itemView, Action<CaseAdapterClickEventArgs> clickListener,
                            Action<CaseAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new CaseAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new CaseAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
        }
    }

    public class CaseAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}