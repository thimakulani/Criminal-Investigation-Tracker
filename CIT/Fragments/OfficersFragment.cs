using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using CIT.Adapters;
using CIT.Dialogs;
using CIT.Models;
using Google.Android.Material.FloatingActionButton;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Fragments
{
    public class OfficersFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.officers_fragments, container, false);
            ConnectViews(view);
            return view;
        }
        private ExtendedFloatingActionButton fab_add_officer;
        private readonly List<OfficerModel> Items = new List<OfficerModel>();
        private void ConnectViews(View view)
        {
            RecyclerView recycler = view.FindViewById<RecyclerView>(Resource.Id.recycler_officers);
            fab_add_officer = view.FindViewById<ExtendedFloatingActionButton>(Resource.Id.fab_add_officer);

            fab_add_officer.Click += Fab_add_officer_Click;
            OfficersAdapter adapter = new OfficersAdapter(Items);
            recycler.SetLayoutManager(new LinearLayoutManager(view.Context));
            recycler.SetAdapter(adapter);
            adapter.OfficerClick += Adapter_OfficerClick;
            adapter.NotifyDataSetChanged();


            CrossCloudFirestore
                .Current
                .Instance
                .Collection("OFFICERS")
                .AddSnapshotListener((value, error) =>
                {
                    if (!value.IsEmpty)
                    {
                        foreach (var item in value.DocumentChanges)
                        {
                            switch (item.Type)
                            {
                                case DocumentChangeType.Added:
                                    Items.Add(item.Document.ToObject<OfficerModel>());
                                    adapter.NotifyDataSetChanged();
                                    break;
                                case DocumentChangeType.Modified:
                                    Items[item.OldIndex] = item.Document.ToObject<OfficerModel>();
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

        private void Adapter_OfficerClick(object sender, OfficersAdapterClickEventArgs e)
        {
            OfficeProfileDlgFragment officeProfileDlgFragment = new OfficeProfileDlgFragment(Items[e.Position].Id);
            officeProfileDlgFragment.Show(ChildFragmentManager.BeginTransaction(), "");
        }

        private void Fab_add_officer_Click(object sender, EventArgs e)
        {
            AddOfficerDlgFragment addOfficer = new AddOfficerDlgFragment();

            addOfficer.Show(ChildFragmentManager.BeginTransaction(), null);
        }
    }
}