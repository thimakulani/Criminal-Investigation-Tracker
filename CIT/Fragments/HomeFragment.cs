﻿using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using CIT.Adapters;
using CIT.Dialogs;
using CIT.Models;
using Google.Android.Material.FloatingActionButton;
using Plugin.CloudFirestore;
using System.Collections.Generic;

namespace CIT.Fragments
{
    public class HomeFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        private Context context;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.home_fragment, container, false);
            ConnectViews(view);
            context = view.Context;
            return view;
        }
        private ExtendedFloatingActionButton fab_add_case;
        private ExtendedFloatingActionButton fab_pending_case;
        private readonly List<Case> Items = new List<Case>();
        private void ConnectViews(View view)
        {
            fab_add_case = view.FindViewById<ExtendedFloatingActionButton>(Resource.Id.fab_add_case);
            fab_pending_case = view.FindViewById<ExtendedFloatingActionButton>(Resource.Id.fab_pending_case);

            fab_pending_case.Click += Fab_pending_case_Click;
            fab_add_case.Click += Fab_add_case_Click;

            RecyclerView recycler = view.FindViewById<RecyclerView>(Resource.Id.recycler_new_case);
            CaseAdapter adapter = new CaseAdapter(Items);
            adapter.BtnActionClick += Adapter_BtnActionClick;
            recycler.SetLayoutManager(new LinearLayoutManager(context));
            recycler.SetAdapter(adapter);
            adapter.NotifyDataSetChanged();
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .OrderBy("DateCreated", true)
                .WhereEqualsTo("Status", "PROGRESS")
                .AddSnapshotListener((value, error) =>
                {
                    if (!value.IsEmpty)
                    {
                        foreach (var item in value.DocumentChanges)
                        {
                            switch (item.Type)
                            {
                                case DocumentChangeType.Added:
                                    Items.Add(item.Document.ToObject<Case>());
                                    adapter.NotifyDataSetChanged();
                                    break;
                                case DocumentChangeType.Modified:
                                    Items[item.OldIndex] = item.Document.ToObject<Case>();
                                    adapter.NotifyDataSetChanged();
                                    break;
                                case DocumentChangeType.Removed:
                                    Items.RemoveAt(item.OldIndex);
                                    adapter.NotifyDataSetChanged();
                                    break;
                            }
                        }
                    }
                });
        }

        private void Fab_pending_case_Click(object sender, System.EventArgs e)
        {
            PenddingCaseDlgFragment dlg = new PenddingCaseDlgFragment();
            dlg.Show(ChildFragmentManager.BeginTransaction(), "");
        }

        private void Adapter_BtnActionClick(object sender, CaseAdapterClickEventArgs e)
        {
            ViewCaseDlgFragment view = new ViewCaseDlgFragment(Items[e.Position].Id); 
            view.Show(ChildFragmentManager.BeginTransaction(), null);
        }

        private void Fab_add_case_Click(object sender, System.EventArgs e)
        {
            AddCaseDlgFragment addCase = new AddCaseDlgFragment();
            addCase.Show(ChildFragmentManager.BeginTransaction(), null);
        }
    }
}