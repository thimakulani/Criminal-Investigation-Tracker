using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using CIT.Models;
using Google.Android.Material.TextView;
using System;
using System.Collections.Generic;

namespace CIT.Adapters
{
    class OfficersAdapter : RecyclerView.Adapter
    {
        public event EventHandler<OfficersAdapterClickEventArgs> ItemClick;
        public event EventHandler<OfficersAdapterClickEventArgs> ItemLongClick;

        private readonly List<OfficerModel> items = new List<OfficerModel>();

        public OfficersAdapter(List<OfficerModel> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.officer_row;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new OfficersAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            // Replace the contents of the view with that element
            var holder = viewHolder as OfficersAdapterViewHolder;
            holder.Row_officer_name.Text = items[position].Name;
            holder.Row_officer_surname.Text = items[position].Surname;
        }

        public override int ItemCount => items.Count;

        void OnClick(OfficersAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(OfficersAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class OfficersAdapterViewHolder : RecyclerView.ViewHolder
    {
        public MaterialTextView Row_officer_name { get; set; }
        public MaterialTextView Row_officer_surname { get; set; }


        public OfficersAdapterViewHolder(View itemView, Action<OfficersAdapterClickEventArgs> clickListener,
                            Action<OfficersAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            Row_officer_name = itemView.FindViewById<MaterialTextView>(Resource.Id.row_officer_name);
            Row_officer_surname = itemView.FindViewById<MaterialTextView>(Resource.Id.row_officer_surname);

            itemView.Click += (sender, e) => clickListener(new OfficersAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new OfficersAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
        }
    }

    public class OfficersAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}