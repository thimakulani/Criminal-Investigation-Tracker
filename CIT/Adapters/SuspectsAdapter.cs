using Android.Views;
using AndroidX.RecyclerView.Widget;
using CIT.Models;
using Google.Android.Material.Button;
using Google.Android.Material.TextView;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Adapters
{
    class SuspectsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<SuspectsAdapterClickEventArgs> ItemClick;
        public event EventHandler<SuspectsAdapterClickEventArgs> ItemLongClick;
        public event EventHandler<SuspectsAdapterClickEventArgs> BtnViewItemClick;
        public event EventHandler<SuspectsAdapterClickEventArgs> DeleteItemClick;
        private readonly List<Suspect> items;

        public SuspectsAdapter(List<Suspect> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.suspect_row;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new SuspectsAdapterViewHolder(itemView, OnClick, OnLongClick, OnActinBtnClick, OnBntDelete);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {

            // Replace the contents of the view with that element
            var holder = viewHolder as SuspectsAdapterViewHolder;
            holder.TxtEvidence.Text = items[position].Note;
            holder.TxtName.Text = items[position].Name;
            holder.TxtSurname.Text = items[position].Surname;
            holder.TxtRelation.Text = items[position].Relation;
        }

        public override int ItemCount => items.Count;

        void OnBntDelete(SuspectsAdapterClickEventArgs args) => DeleteItemClick?.Invoke(this, args);
        void OnActinBtnClick(SuspectsAdapterClickEventArgs args) => BtnViewItemClick?.Invoke(this, args);
        void OnClick(SuspectsAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(SuspectsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class SuspectsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public MaterialTextView TxtName { get; set; }
        public MaterialTextView TxtSurname { get; set; }
        public MaterialTextView TxtEvidence { get; set; }
        public MaterialTextView TxtRelation { get; set; }
        public MaterialTextView BtnAction { get; set; }
        public MaterialTextView BtnDelete { get; set; }


        public SuspectsAdapterViewHolder(View itemView, Action<SuspectsAdapterClickEventArgs> clickListener,
                            Action<SuspectsAdapterClickEventArgs> longClickListener, Action<SuspectsAdapterClickEventArgs> actionBtnClickListener,
                            Action<SuspectsAdapterClickEventArgs> btnDeleteClickListener) : base(itemView)
        {

            TxtEvidence = itemView.FindViewById<MaterialTextView>(Resource.Id.row_suspect_evidence);
            TxtSurname = itemView.FindViewById<MaterialTextView>(Resource.Id.row_suspect_surname);
            TxtRelation = itemView.FindViewById<MaterialTextView>(Resource.Id.row_suspect_relation);
            TxtName = itemView.FindViewById<MaterialTextView>(Resource.Id.row_suspect_name);
            BtnAction = itemView.FindViewById<MaterialTextView>(Resource.Id.row_suspect_btn_action);
            BtnDelete = itemView.FindViewById<MaterialTextView>(Resource.Id.row_suspect_btn_delete);
            //TextView = v;
            BtnDelete.Click += (sender, e) => btnDeleteClickListener(new SuspectsAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            BtnAction.Click += (sender, e) => actionBtnClickListener(new SuspectsAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.Click += (sender, e) => clickListener(new SuspectsAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new SuspectsAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
        }
    }

    public class SuspectsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}