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
    class OfficerCaseAdapter : RecyclerView.Adapter
    {
        public event EventHandler<OfficerCaseAdapterClickEventArgs> ItemClick;
        public event EventHandler<OfficerCaseAdapterClickEventArgs> BtnClick;
        public event EventHandler<OfficerCaseAdapterClickEventArgs> ItemLongClick;
        private readonly List<Case> items = new List<Case>();

        public OfficerCaseAdapter(List<Case> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.row_officer_case;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new OfficerCaseAdapterViewHolder(itemView, OnClick, OnLongClick, OnBtnClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {

            // Replace the contents of the view with that element
            var holder = viewHolder as OfficerCaseAdapterViewHolder;
            holder.TxtCaseName.Text = items[position].CaseName;
            holder.TxtCreatedDay.Text = $"{items[position].DateCreated.ToDateTime():dd-MMM-yyyy}";
            holder.TxtLastUpdate.Text = $"{items[position].LastUpdate.ToDateTime():dd-MMM-yyyy}";
            holder.TxtStatus.Text = items[position].Status;
        }

        public override int ItemCount => items.Count;

        void OnBtnClick(OfficerCaseAdapterClickEventArgs args) => BtnClick?.Invoke(this, args);
        void OnClick(OfficerCaseAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(OfficerCaseAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class OfficerCaseAdapterViewHolder : RecyclerView.ViewHolder
    {
        public MaterialTextView TxtCaseName { get; set; }
        public MaterialTextView TxtCreatedDay { get; set; }
        public MaterialTextView TxtLastUpdate { get; set; }
        public MaterialTextView TxtStatus { get; set; }
        public MaterialTextView TxtBtnAction { get; set; }
        public OfficerCaseAdapterViewHolder(View itemView, Action<OfficerCaseAdapterClickEventArgs> clickListener,
                            Action<OfficerCaseAdapterClickEventArgs> longClickListener, Action<OfficerCaseAdapterClickEventArgs> actionClickListener) : base(itemView)
        {
            TxtCaseName = itemView.FindViewById<MaterialTextView>(Resource.Id.row_officer_case_name);
            TxtCreatedDay = itemView.FindViewById<MaterialTextView>(Resource.Id.row_officer_date_created);
            TxtLastUpdate = itemView.FindViewById<MaterialTextView>(Resource.Id.row_officer_case_last_update);
            TxtStatus = itemView.FindViewById<MaterialTextView>(Resource.Id.row_officer_case_status);
            TxtBtnAction = itemView.FindViewById<MaterialTextView>(Resource.Id.btn_row_officer_case_action);

            TxtBtnAction.Click += (sender, e) => actionClickListener(new OfficerCaseAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.Click += (sender, e) => clickListener(new OfficerCaseAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new OfficerCaseAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
        }
    }

    public class OfficerCaseAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}