using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using CIT.Models;
using FFImageLoading;
using Google.Android.Material.Button;
using Google.Android.Material.Dialog;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using ID.IonBit.IonAlertLib;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Dialogs
{
    public class AddCaseDlgFragment : DialogFragment
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
            View view =  inflater.Inflate(Resource.Layout.add_case_fragment, container, false);

            ConnectViews(view);

            return view;
        }
        private TextInputEditText input_case_name;
        private TextInputEditText input_case_note;
        private MaterialTextView txt_name;
        private MaterialTextView txt_lastname;
        private MaterialButton btn_select_officer;
        private LinearLayout include_officer;
        private MaterialButton btn_add_case;
        private AppCompatImageView officer_profile_img;
        private Context context;
        private void ConnectViews(View view)
        {
            context = view.Context;
            officer_profile_img = view.FindViewById<AppCompatImageView>(Resource.Id.officer_profile_img);
            input_case_name = view.FindViewById<TextInputEditText>(Resource.Id.input_case_name);
            input_case_note = view.FindViewById<TextInputEditText>(Resource.Id.input_case_note);
            include_officer = view.FindViewById<LinearLayout>(Resource.Id.include_officer);
            txt_name = view.FindViewById<MaterialTextView>(Resource.Id.txt_name);
            txt_lastname = view.FindViewById<MaterialTextView>(Resource.Id.txt_lastname);
            btn_add_case = view.FindViewById<MaterialButton>(Resource.Id.btn_add_case);
            btn_select_officer = view.FindViewById<MaterialButton>(Resource.Id.btn_select_officer);

            btn_select_officer.Click += Btn_select_officer_Click;
            btn_add_case.Click += Btn_add_case_Click;
            include_officer.Visibility = ViewStates.Gone;


        }
        private IonAlert loadingDialog;
        private async void Btn_add_case_Click(object sender, EventArgs e)
        {
            loadingDialog = new IonAlert(context, IonAlert.ProgressType);
            loadingDialog.SetSpinKit("DoubleBounce")
                .ShowCancelButton(false)
                .Show();
            Dictionary<string, object> keyValues = new Dictionary<string, object>
            {
                { "CaseName", input_case_name.Text },
                { "OfficerId", officer_id },
                { "Note", input_case_note.Text },
                { "Evidence", null },
                { "Suspect", null },
                { "Status", "P" },
                { "TimeStamp", FieldValue.ServerTimestamp }
            };

            await CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .AddAsync(keyValues);
            loadingDialog.Dismiss();
            this.Dismiss();

        }

        private async void Btn_select_officer_Click(object sender, EventArgs e)
        {
            var query = await CrossCloudFirestore
                .Current
                .Instance
                .Collection("OFFICERS")
                .GetAsync(Source.Server);
            List<OfficerModel> users = new List<OfficerModel>();
            List<string> names = new List<string>();

            foreach (var item in query.Documents)
            {
                var obj = item.ToObject<OfficerModel>();
               
                users.Add(obj);
                names.Add($"{obj.Name} {obj.Surname}");
            }


            CustomeDialog(users, names);

        }
        private string officer_id = null;
        private void CustomeDialog(List<OfficerModel> users, List<string> names)
        {

            OfficerModel officer = new OfficerModel();
            MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(context);
            builder.SetTitle("SELECT OFFICER");
            builder.SetSingleChoiceItems(names.ToArray(), -1, (dlg, which) =>
            {
                officer_id = users[which.Which].Id;//$"{users[which.Which].Name} {users[which.Which].Surname}";
                officer = users[which.Which];
            });
            builder.SetCancelable(false);
            builder.SetNegativeButton("CANCEL", delegate
            {
                officer_id = null;
                builder.Dispose();
            });
            builder.SetPositiveButton("CONTINUE", delegate
            {
                builder.Dispose();
                if(officer_id != null)
                {
                    include_officer.Visibility = ViewStates.Visible;
                    txt_lastname.Text = officer.Surname;
                    txt_name.Text = officer.Name;
                    if(officer.ImageUrl != null)
                    {
                        ImageService
                        .Instance
                        .LoadUrl(officer.ImageUrl)
                        .Retry(3, 200)
                        .DownSampleInDip(150, 150)
                        .IntoAsync(officer_profile_img);
                    }
                }
            });
            builder.Show(); 
        }
        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }
    }
}