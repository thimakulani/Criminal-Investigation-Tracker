﻿using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using CIT.Adapters;
using CIT.Models;
using FFImageLoading;
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using ID.IonBit.IonAlertLib;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Dialogs
{
    public class OfficerViewCaseDlgFragment : DialogFragment
    {

        private string case_id = null;

        public OfficerViewCaseDlgFragment(string case_id)
        {
            this.case_id = case_id;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            SetStyle(StyleNoFrame, Resource.Style.FullScreenDialogStyle);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.officer_view_case_fragment, container, false);

            ConnectViews(view);

            return view;
        }
        private MaterialToolbar view_toolbar;
        private MaterialButton btn_add_suspect;
  
        private TextInputEditText input_case_name;
        private TextInputEditText input_case_note;
        private TextInputEditText input_case_date_created;
        private TextInputEditText input_case_last_update;
        private TextInputEditText input_case_status;
        private RecyclerView recycler;
        private readonly List<Suspect> items = new List<Suspect>();
        private Context context;
        private void ConnectViews(View view)
        {
            context = view.Context;
            input_case_name = view.FindViewById<TextInputEditText>(Resource.Id.o_view_input_case_name);
            input_case_note = view.FindViewById<TextInputEditText>(Resource.Id.o_view_input_case_note);
            input_case_date_created = view.FindViewById<TextInputEditText>(Resource.Id.o_view_input_case_dates_created);
            input_case_last_update = view.FindViewById<TextInputEditText>(Resource.Id.o_view_input_case_last_update);
            input_case_status = view.FindViewById<TextInputEditText>(Resource.Id.o_view_input_case_status);
            btn_add_suspect = view.FindViewById<MaterialButton>(Resource.Id.o_view_btn_add_suspect);

            recycler = view.FindViewById<RecyclerView>(Resource.Id.recycler_suspect);
            SuspectsAdapter adapter = new SuspectsAdapter(items);
            recycler.SetLayoutManager(new LinearLayoutManager(context));
            recycler.SetAdapter(adapter);
            adapter.NotifyDataSetChanged();
            adapter.BtnViewItemClick += Adapter_BtnViewItemClick;
            adapter.DeleteItemClick += Adapter_DeleteItemClick;
            

            view_toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.officer_view_toolbar);
            view_toolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back_black_18dp);
            btn_add_suspect.Click += Btn_add_suspect_Click;
            view_toolbar.NavigationClick += View_toolbar_NavigationClick;

            CrossCloudFirestore.Current
                .Instance
                .Collection("CASES")
                .Document(case_id)
                .AddSnapshotListener((values, errors) =>
                {
                    if (values.Exists)
                    {
                        var data = values.ToObject<Case>();
                        input_case_name.Text = data.CaseName;
                        input_case_date_created.Text = $"{data.DateCreated.ToDateTime():dddd, dd-MMM-yyyy}";
                        input_case_last_update.Text = $"{data.LastUpdate.ToDateTime():dddd, dd-MMM-yyyy}";
                        input_case_note.Text = data.Note;
                        input_case_status.Text = data.Status;
                    }
                });

            CrossCloudFirestore.Current
                .Instance
                .Collection("CASES")
                .Document(case_id)
                .Collection("Suspect")
                .AddSnapshotListener((values, errors) =>
                {
                    if (!values.IsEmpty)
                    {
                        foreach (var dc in values.DocumentChanges)
                        {
                            switch (dc.Type)
                            {
                                case DocumentChangeType.Added:
                                    var _case = dc.Document.ToObject<Suspect>();
                                    items.Add(_case);
                                    adapter.NotifyDataSetChanged();
                                    break;
                                case DocumentChangeType.Modified:
                                    items[dc.OldIndex] = dc.Document.ToObject<Suspect>();
                                    adapter.NotifyDataSetChanged();
                                    break;
                                case DocumentChangeType.Removed:
                                    items.RemoveAt(dc.OldIndex);
                                    adapter.NotifyDataSetChanged();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                });
        }

        private void Adapter_DeleteItemClick(object sender, SuspectsAdapterClickEventArgs e)
        {
            IonAlert ionAlert = new IonAlert(context);
        }

        private void Adapter_BtnViewItemClick(object sender, SuspectsAdapterClickEventArgs e)
        {
        }

        private void Btn_add_suspect_Click(object sender, EventArgs e)
        {
            AddSuspectDlgFragment dlg = new AddSuspectDlgFragment(case_id);
            dlg.Show(ChildFragmentManager.BeginTransaction(), null);
        }

        private void View_toolbar_NavigationClick(object sender, AndroidX.AppCompat.Widget.Toolbar.NavigationClickEventArgs e)
        {
            Dismiss();
        }
    }
}