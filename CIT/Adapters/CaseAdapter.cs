﻿using Android.Views;
using AndroidX.RecyclerView.Widget;
using CIT.Models;
using Google.Android.Material.TextView;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Adapters
{
    class CaseAdapter : RecyclerView.Adapter
    {
        public event EventHandler<CaseAdapterClickEventArgs> ItemClick;
        public event EventHandler<CaseAdapterClickEventArgs> ItemLongClick;
        private List<Case> items = new List<Case>();

        public CaseAdapter(List<Case> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.case_row;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new CaseAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as CaseAdapterViewHolder;
            holder.row_case_name.Text = items[position].CaseName;
            holder.row_case_status.Text = items[position].Status;
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
                            holder.row_officer_names.Text = $"{user.Name} {user.Surname}";
                        }
                    });
            }
            
        }

        public override int ItemCount => items.Count;

        void OnClick(CaseAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(CaseAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class CaseAdapterViewHolder : RecyclerView.ViewHolder
    {
       public MaterialTextView row_case_name { get; set; }
       public MaterialTextView row_officer_names { get; set; }
       public MaterialTextView row_case_status { get; set; }


        public CaseAdapterViewHolder(View itemView, Action<CaseAdapterClickEventArgs> clickListener,
                            Action<CaseAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            row_case_status = itemView.FindViewById<MaterialTextView>(Resource.Id.row_case_status);
            row_officer_names = itemView.FindViewById<MaterialTextView>(Resource.Id.row_officer_names);
            row_case_name = itemView.FindViewById<MaterialTextView>(Resource.Id.row_case_name);
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