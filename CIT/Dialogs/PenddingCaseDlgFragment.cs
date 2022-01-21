using Android.Content;
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


namespace CIT.Dialogs
{
    public class PenddingCaseDlgFragment : DialogFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            SetStyle(StyleNoFrame, Resource.Style.FullScreenDialogStyle);
        }
        Context context;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.pandding_fragment, container, false);
            ConnectViews(view);
            context = view.Context;
            return view;
        }
        private readonly List<Case> Items = new List<Case>();
        private void ConnectViews(View view)
        {
            RecyclerView recycler = view.FindViewById<RecyclerView>(Resource.Id.recycler_pannding_case);
            PenddingCaseAdapter adapter = new PenddingCaseAdapter(Items);
            recycler.SetLayoutManager(new LinearLayoutManager(context));
            adapter.BtnActionClick += Adapter_BtnActionClick;
            recycler.SetAdapter(adapter);

            CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .WhereEqualsTo("Request", "1")
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
                                    for (int i = 0; i < Items.Count; i++)
                                    {
                                        if (Items[i].Id == item.Document.ToObject<Case>().Id)
                                        {
                                            if (item.Document.ToObject<Case>().Status == "CLOSED")
                                            {
                                                Items.RemoveAt(i);// = data;
                                                //  IsVisible = false;
                                                adapter.NotifyDataSetChanged();
                                                break;
                                            }
                                        }
                                    }
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

        private void Adapter_BtnActionClick(object sender, PenddingCaseAdapterClickEventArgs e)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Status", "CLOSED");
            data.Add("Request", "2");
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .Document(Items[e.Position].Id)
                .UpdateAsync(data);
        }
    }
}