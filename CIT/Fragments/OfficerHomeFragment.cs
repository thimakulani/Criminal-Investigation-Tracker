using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using CIT.Adapters;
using CIT.Dialogs;
using CIT.Models;
using Firebase.Auth;
using Plugin.CloudFirestore;
using System.Collections.Generic;

namespace CIT.Fragments
{
    public class OfficerHomeFragment : Fragment
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
            View view = inflater.Inflate(Resource.Layout.home_officer_fragment, container, false);
            ConnectViews(view);
            context = view.Context;
            return view;
        }

        private readonly List<Case> Items = new List<Case>();
        private RecyclerView recycler;
        private void ConnectViews(View view)
        {
            recycler = view.FindViewById<RecyclerView>(Resource.Id.recycler_cases);
            OfficerCaseAdapter adapter = new OfficerCaseAdapter(Items);
            adapter.BtnClick += Adapter_BtnClick;
            recycler.SetLayoutManager(new LinearLayoutManager(context));
            recycler.SetAdapter(adapter);

            adapter.NotifyDataSetChanged();
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .OrderBy("DateCreated", true)
                .WhereEqualsTo("OfficerId", FirebaseAuth.Instance.CurrentUser.Uid)
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

        private void Adapter_BtnClick(object sender, OfficerCaseAdapterClickEventArgs e)
        {

            OfficerViewCaseDlgFragment dlg = new OfficerViewCaseDlgFragment(Items[e.Position].Id);
            dlg.Show(ChildFragmentManager.BeginTransaction(), null);
        }
    }
}