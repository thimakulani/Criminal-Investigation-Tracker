using Android.Content;
using Android.OS;
using Android.Views;
using AndroidHUD;
using AndroidX.Fragment.App;
using CIT.Models;
using FirebaseAdmin.Auth;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using ID.IonBit.IonAlertLib;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Dialogs
{
    public class AddOfficerDlgFragment : DialogFragment
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
            View view = inflater.Inflate(Resource.Layout.add_officer_fragment, container, false);
            ConnectViews(View);
            context = view.Context;
            return view;
        }
        
        private TextInputEditText Input_name;
        private TextInputEditText Input_surname;
        private TextInputEditText Input_phone;
        private TextInputEditText Input_email;
        private TextInputEditText Input_address;
        private MaterialButton btn_attachement;
        private MaterialButton btn_add_officer;

        private void ConnectViews(View view)
        {
            Input_name = view.FindViewById<TextInputEditText>(Resource.Id.input_name);
            Input_phone = view.FindViewById<TextInputEditText>(Resource.Id.input_mobile);
            Input_surname = view.FindViewById<TextInputEditText>(Resource.Id.input_lastname);
            Input_address = view.FindViewById<TextInputEditText>(Resource.Id.input_address);
            Input_email = view.FindViewById<TextInputEditText>(Resource.Id.input_email_address);
            btn_add_officer = view.FindViewById<MaterialButton>(Resource.Id.btn_add_officer);
            btn_attachement = view.FindViewById<MaterialButton>(Resource.Id.btn_attachement);

            btn_attachement.Click += Btn_attachement_Click;
            btn_add_officer.Click += Btn_add_officer_Click;
        }
        IonAlert loadingDialog;
        private async void Btn_add_officer_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>
            {
                { "Name", Input_name.Text.Trim() },
                { "Surname", Input_surname.Text.Trim() },
                { "ImageUrl", null },
                { "Email", Input_email.Text.Trim() },
                { "Address", Input_address.Text.Trim() },
                { "Role", "O" },
                { "Phone", Input_name.Text.Trim() }
            };
            var stream = Resources.Assets.Open("service_account.json");
            var _auth = FirebaseHelper.FirebaseAdminSDK.GetFirebaseAdminAuth(stream);

            try
            {
                loadingDialog = new IonAlert(context, IonAlert.ProgressType);
                loadingDialog.SetSpinKit("DoubleBounce")
                    .ShowCancelButton(false)
                    .Show();

                UserRecordArgs user = new UserRecordArgs()
                {
                    Email = Input_email.Text.Trim(),
                    Password = Input_phone.Text.Trim(),
                };
                var results = await _auth.CreateUserAsync(user);
                await CrossCloudFirestore.Current
                    .Instance
                    .Collection("OFFICERS")
                    .Document(results.Uid)
                    .SetAsync(keyValues);
                AndHUD.Shared.ShowSuccess(context, "Account successfully created", MaskType.Clear, TimeSpan.FromSeconds(2));
            }
            catch (Exception ex)
            {
                AndHUD.Shared.ShowError(context, ex.Message, MaskType.Clear, TimeSpan.FromSeconds(2));
            }
           
        }

        private void Btn_attachement_Click(object sender, EventArgs e)
        {
            
        }
        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }
    }
}