using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using CIT.Models;
using FFImageLoading;
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Dialogs
{
    public class PossibleSuspectDlgFragment : DialogFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        private Context context;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.possible_suspect_fragment, container, false);
            ConnectViews(view);
            return view;

        }
        private MaterialButton btn_suspect;
        private MaterialButton btn_submit_suspect;
        private TextInputEditText input_comment;
        private List<Suspect> items;
        private string case_id;
        public PossibleSuspectDlgFragment(List<Suspect> items, string case_id)
        {
            this.items = items;
            this.case_id = case_id;
        }
        string suspect_name = null;
        string suspect_id = null;
        private void ConnectViews(View view)
        {
            context = view.Context;
            btn_suspect = view.FindViewById<MaterialButton>(Resource.Id.btn_suspect);
            btn_submit_suspect = view.FindViewById<MaterialButton>(Resource.Id.btn_submit_suspect);
            input_comment = view.FindViewById<TextInputEditText>(Resource.Id.input_comment);

            btn_suspect.Click += Btn_suspect_Click;
            btn_submit_suspect.Click += Btn_submit_suspect_Click;
            int heightst = 0;
            foreach (var data in items)
            {
                if(heightst <data.LScore + data.PScore)
                {
                    suspect_id = data.Id;
                    suspect_name = $"{data.Name} {data.Surname}";
                }
            }
            btn_suspect.Text = $"{suspect_name}";
        }

        private async void Btn_submit_suspect_Click(object sender, EventArgs e)
        {
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .Document(case_id)
                .Collection("Suspect")
                .Document(suspect_id)
                .UpdateAsync("PrimeSuspect", "1");

            await CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .Document(case_id)
                .UpdateAsync("Status", "CLOSED");
        }

        private void Btn_suspect_Click(object sender, EventArgs e)
        {
            PopupMenu popupMenu = new PopupMenu(context, btn_suspect);
            int counter = 0;
            foreach (var data in items)
            {
                popupMenu.Menu.Add(IMenu.None, counter, 0, $"{data.Name} {data.Surname}");
                counter++;
            }
            popupMenu.Show();
            popupMenu.MenuItemClick += PopupMenu_MenuItemClick;
        }

        private void PopupMenu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            suspect_id = items[e.Item.ItemId].Id;
            suspect_name = $"{items[e.Item.ItemId].Name} {items[e.Item.ItemId].Name}";
        }
        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }
    }
}