using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using CIT.Models;
using FFImageLoading;
using FirebaseAdmin.Messaging;
using Google.Android.Material.Button;
using Google.Android.Material.Dialog;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using ID.IonBit.IonAlertLib;
using MimeKit;
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
        private TextInputEditText input_victim_name;
        private TextInputEditText input_victim_phone;
        private TextInputEditText input_victim_email;

        private TextInputEditText input_case_number;
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


            input_victim_email = view.FindViewById<TextInputEditText>(Resource.Id.input_victim_email);
            input_victim_name = view.FindViewById<TextInputEditText>(Resource.Id.input_victim_name);
            input_victim_phone = view.FindViewById<TextInputEditText>(Resource.Id.input_victim_phone);


            officer_profile_img = view.FindViewById<AppCompatImageView>(Resource.Id.officer_profile_img);
            input_case_name = view.FindViewById<TextInputEditText>(Resource.Id.input_case_name);
            input_case_note = view.FindViewById<TextInputEditText>(Resource.Id.input_case_note);
            input_case_number = view.FindViewById<TextInputEditText>(Resource.Id.input_case_number);
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
            if (string.IsNullOrEmpty(input_case_name.Text))
            {
                input_case_name.Error = "cannot be empty";
                return;
            }
            if (string.IsNullOrEmpty(input_case_note.Text))
            {
                input_case_note.Error = "cannot be empty";
                return;
            }
            if (string.IsNullOrEmpty(input_case_name.Text))
            {
                input_case_name.Error = "cannot be empty";
                return;
            }
            if (string.IsNullOrEmpty(input_case_note.Text))
            {
                input_case_note.Error = "cannot be empty";
                return;
            }
            if (string.IsNullOrEmpty(input_case_number.Text))
            {
                input_case_number.Error = "cannot be empty";
                return;
            }
            if (string.IsNullOrEmpty(input_victim_name.Text))
            {
                input_victim_name.Error = "cannot be empty";
                return;
            }
            if (string.IsNullOrEmpty(input_victim_email.Text))
            {
                input_victim_email.Error = "cannot be empty";
                return;
            }
            if (string.IsNullOrEmpty(input_victim_phone.Text))
            {
                input_victim_phone.Error = "cannot be empty";
                return;
            }


            loadingDialog = new IonAlert(context, IonAlert.ProgressType);
            loadingDialog.SetSpinKit("DoubleBounce")
                .ShowCancelButton(false)
                .Show();
            Dictionary<string, object> keyValues = new Dictionary<string, object>
            {
                { "CaseNo",input_case_number.Text },
                { "CaseName", input_case_name.Text },
                { "OfficerId", officer_id },
                { "Note", input_case_note.Text },
                { "Status", "PROGRESS" },
                { "DateCreated", FieldValue.ServerTimestamp },
                { "PrimeSuspect", null },
                { "LastUpdate", FieldValue.ServerTimestamp },
                { "VictimName", input_victim_name.Text },
                { "VictimPhone", input_victim_phone.Text },
                { "VictimEmail", input_victim_email.Text },
                { "Request", "0" },
            };

            await CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .AddAsync(keyValues);
            AndHUD.Shared.ShowSuccess(context, "You have successfully added a case record", MaskType.Clear, TimeSpan.FromSeconds(2));
            loadingDialog.Dismiss();
            this.Dismiss();
            if (officer_id != null)
            {
                var stream = Resources.Assets.Open("service_account.json");
                var fcm = FirebaseHelper.FirebaseAdminSDK.GetFirebaseMessaging(stream);
                FirebaseAdmin.Messaging.Message fmessage = new FirebaseAdmin.Messaging.Message()
                {
                    Topic = officer_id,
                    Notification = new Notification()
                    {
                        Title = "New Case Added",
                        Body = $"You have been assigned to a Case {input_case_name.Text}",
                    },
                };
                await fcm.SendAsync(fmessage);
            }



            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CRIME INVESTIGATION TRACK", "sigauquetk@gmail.com"));
            message.To.Add(new MailboxAddress($"{input_victim_name.Text}", $"{input_victim_email.Text.Trim()}"));
            message.Subject = "CASE OPEN";
            string body = generateBody(input_case_number.Text, input_victim_name.Text);


            message.Body = new TextPart("html")
            {
                Text = body,

                //Text = $"Book title: {Items[e.Position].Title}" +
                //$" Download Url: {Items[e.Position].ImageUrl}",
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 587);
                client.Authenticate("sigauquetk@gmail.com", "THIma$!305");
                await client.SendAsync(message);
            };

        }
        private string generateBody(string cn, string c_name)
        {

            var body = @"<!DOCTYPE html>
                <html>
                <head>
                <style>
                table {
                  font-family: arial, sans-serif;
                  border-collapse: collapse;
                  width: 100%;
                }

                td, th {
                  border: 1px solid #dddddd;
                  text-align: left;
                  padding: 8px;
                }

                tr:nth-child(even) {
                  background-color: #dddddd;
                }
                </style>
                </head>
                <body>

                <h2>YOUR CASE HAS BEEN ACTIVELY OPEN</h2>

                <table>
                  <tr>
                    <th>Case Name</th>
                    <th>Case Number</th>
                    <th>Assigned To</th>
                  </tr>";
            body += @$"
                  <tr>
                    <td>{c_name}</td>
                    <td>{cn}</td>
                    <td>{txt_lastname.Text} {txt_name.Text}</td>
                  </tr>
                </table>

                </body>
                </html>";
            return body;
        }
        private async void Btn_select_officer_Click(object sender, EventArgs e)
        {
            var query = await CrossCloudFirestore
                .Current
                .Instance
                .Collection("OFFICERS")
                .GetAsync();
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