using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using CIT.Adapters;
using CIT.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Fragments
{
    public class CaseHistoryFragment : Fragment
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
            View view = inflater.Inflate(Resource.Layout.case_history_fragment, container, false);
            ConnectViews(view);
            return view;
        }
        private RecyclerView recycler;
        private List<Case> Items = new List<Case>();
        private void ConnectViews(View view)
        {
            recycler = view.FindViewById<RecyclerView>(Resource.Id.recycler_case_history);
            HistoryAdapter adapter = new HistoryAdapter(Items);
            recycler.SetLayoutManager(new LinearLayoutManager(view.Context));
            recycler.SetAdapter(adapter);
            adapter.NotifyDataSetChanged();

            CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .OrderBy("DateCreated", true)
                .WhereEqualsTo("Status", "CLOSED")
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
    }
}