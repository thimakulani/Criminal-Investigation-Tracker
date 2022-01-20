using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using CIT.Models;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIT.Dialogs
{
    public class OfficeProfileDlgFragment : DialogFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        private readonly string id;

        public OfficeProfileDlgFragment(string id)
        {
            this.id = id;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.profile_fragment, container, false);
            ConnectViews(view);
            return view;
        }
        private TextInputEditText Input_name;
        private TextInputEditText Input_surname;
        private TextInputEditText Input_phone;
        private TextInputEditText Input_email;
        private TextInputEditText Input_id;
        private void ConnectViews(View view)
        {
            Input_name = view.FindViewById<TextInputEditText>(Resource.Id.profile_name);
            Input_surname = view.FindViewById<TextInputEditText>(Resource.Id.profile_lastname);
            Input_phone = view.FindViewById<TextInputEditText>(Resource.Id.profile_phone);
            Input_email = view.FindViewById<TextInputEditText>(Resource.Id.profile_email);
            Input_id = view.FindViewById<TextInputEditText>(Resource.Id.officert_id);
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("OFFICERS")
                .Document(id)
                .AddSnapshotListener((value, error) =>
                {
                    if(value != null)
                    {
                        OfficerModel user = value.ToObject<OfficerModel>();
                        Input_name.Text = user.Name;
                        Input_surname.Text = user.Surname;
                        Input_phone.Text = user.Phone;
                        Input_id.Text = user.Id;
                        Input_email.Text = user.Email;
                    }
                });
        }
        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }

    }
}