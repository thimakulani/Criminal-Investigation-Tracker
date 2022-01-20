using Android.Views;
using AndroidX.RecyclerView.Widget;
using CIT.Models;
using Google.Android.Material.TextView;
using System;
using System.Collections.Generic;

namespace CIT.Adapters
{
    class PossibleSuspectsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<PossibleSuspectsAdapterClickEventArgs> ItemClick;
        public event EventHandler<PossibleSuspectsAdapterClickEventArgs> ItemLongClick;
        private readonly List<Suspect> items;

        public PossibleSuspectsAdapter(List<Suspect> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.possible_suspect_row;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new PossibleSuspectsAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {

            // Replace the contents of the view with that element
            var holder = viewHolder as PossibleSuspectsAdapterViewHolder;
            holder.TxtName.Text = items[position].Name + " " + items[position].Surname;
            holder.TxtLogical.Text = $"{items[position].LScore}";
            holder.TxtTotal.Text = $"{items[position].LScore + items[position].LScore}";
            holder.TxtPhysical.Text = $"{items[position].PScore}";
        }

        public override int ItemCount => items.Count;

        void OnClick(PossibleSuspectsAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(PossibleSuspectsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class PossibleSuspectsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public MaterialTextView TxtName { get; set; }
        public MaterialTextView TxtLogical { get; set; }
        public MaterialTextView TxtPhysical { get; set; }
        public MaterialTextView TxtTotal { get; set; }


        public PossibleSuspectsAdapterViewHolder(View itemView, Action<PossibleSuspectsAdapterClickEventArgs> clickListener,
                            Action<PossibleSuspectsAdapterClickEventArgs> longClickListener) : base(itemView)
        {

            TxtLogical = itemView.FindViewById<MaterialTextView>(Resource.Id.row_p_lscore);
            TxtPhysical = itemView.FindViewById<MaterialTextView>(Resource.Id.row_p_pscore);
            TxtName = itemView.FindViewById<MaterialTextView>(Resource.Id.row_s_name);
            TxtTotal = itemView.FindViewById<MaterialTextView>(Resource.Id.btn_row_p_total);
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new PossibleSuspectsAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new PossibleSuspectsAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
        }
    }

    public class PossibleSuspectsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}