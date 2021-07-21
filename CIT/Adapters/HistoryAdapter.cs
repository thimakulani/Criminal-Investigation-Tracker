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
    class HistoryAdapter : RecyclerView.Adapter
    {
        public event EventHandler<HistoryAdapterClickEventArgs> BtnActionClick;
        public event EventHandler<HistoryAdapterClickEventArgs> ItemClick;
        public event EventHandler<HistoryAdapterClickEventArgs> ItemLongClick;
        private readonly List<Case> items = new List<Case>();

        public HistoryAdapter(List<Case> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.history_row;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new HistoryAdapterViewHolder(itemView, OnClick, OnLongClick, OnBtnActionClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as HistoryAdapterViewHolder;
            holder.TxtCaseName.Text = items[position].CaseName;
            holder.TxtDateCreated.Text = items[position].DateCreated;
            holder.TxtStatus.Text = items[position].Status;
            if(items[position].OfficerId != null)
            {
                CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("OFFICERS")
                    .Document(items[position].OfficerId)
                    .AddSnapshotListener((value, error) =>
                    {
                        if (value.Exists)
                        {
                            var user = value.ToObject<OfficerModel>();
                            holder.TxtOfficer.Text = $"{user.Name} {user.Surname}";
                        }
                    });

            }
        }

        public override int ItemCount => items.Count;

        void OnBtnActionClick(HistoryAdapterClickEventArgs args) => BtnActionClick?.Invoke(this, args);
        void OnClick(HistoryAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(HistoryAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class HistoryAdapterViewHolder : RecyclerView.ViewHolder
    {
        public MaterialTextView TxtCaseName { get; set; }
        public MaterialTextView TxtOfficer { get; set; }
        public MaterialTextView TxtDateCreated { get; set; }
        public MaterialTextView TxtStatus { get; set; }
        public MaterialButton BtnAction { get; set; }


        public HistoryAdapterViewHolder(View itemView, Action<HistoryAdapterClickEventArgs> clickListener,
                            Action<HistoryAdapterClickEventArgs> longClickListener, Action<HistoryAdapterClickEventArgs> actionBtnClickListener) : base(itemView)
        {
            //TextView = v;
            TxtCaseName = itemView.FindViewById<MaterialTextView>(Resource.Id.row_histry_case_name);
            TxtOfficer = itemView.FindViewById<MaterialTextView>(Resource.Id.row_histry_officer_names);
            TxtDateCreated = itemView.FindViewById<MaterialTextView>(Resource.Id.row_histry_dates_created);
            TxtStatus = itemView.FindViewById<MaterialTextView>(Resource.Id.row_histry_status);
            BtnAction = itemView.FindViewById<MaterialButton>(Resource.Id.btn_history_action);


            BtnAction.Click += (sender, e) => actionBtnClickListener(new HistoryAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.Click += (sender, e) => clickListener(new HistoryAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new HistoryAdapterClickEventArgs { View = itemView, Position = AbsoluteAdapterPosition });
        }
    }

    public class HistoryAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}