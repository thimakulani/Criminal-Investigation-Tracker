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
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.Dialog;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Dialogs
{
    public class ViewCaseDlgFragment : DialogFragment
    {
        private readonly string id;

        public ViewCaseDlgFragment(string id)
        {
            this.id = id;
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
            View view = inflater.Inflate(Resource.Layout.view_full_case_fragemnt, container, false);

            ConnectViews(view);

            return view;
        }
        private MaterialToolbar view_toolbar;
        private MaterialButton view_btn_select_officer;
        private MaterialButton view_btn_update_case;
        private MaterialTextView txt_name;
        private MaterialTextView txt_surname;
        private AppCompatImageView profile_img;
        private TextInputEditText input_case_name;
        private TextInputEditText input_case_note;
        private TextInputEditText input_case_date_created;
        private TextInputEditText view_input_case_status;
        private LinearLayout view_include_officer;
        private Context context;
        private void ConnectViews(View view)
        {
            context = view.Context;
            txt_name = view.FindViewById<MaterialTextView>(Resource.Id.txt_name);
            txt_surname = view.FindViewById<MaterialTextView>(Resource.Id.txt_lastname);
            profile_img = view.FindViewById<AppCompatImageView>(Resource.Id.officer_profile_img);
            input_case_name = view.FindViewById<TextInputEditText>(Resource.Id.view_input_case_name);
            input_case_note = view.FindViewById<TextInputEditText>(Resource.Id.view_input_case_note);
            input_case_date_created = view.FindViewById<TextInputEditText>(Resource.Id.view_input_case_dates_created);
            view_input_case_status = view.FindViewById<TextInputEditText>(Resource.Id.view_input_case_status);
            view_include_officer = view.FindViewById<LinearLayout>(Resource.Id.view_include_officer);

            view_toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.view_toolbar);
            view_btn_select_officer = view.FindViewById<MaterialButton>(Resource.Id.view_btn_select_officer);
            view_btn_update_case = view.FindViewById<MaterialButton>(Resource.Id.view_btn_update_case);



            view_toolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back_black_18dp);
            view_toolbar.NavigationClick += View_toolbar_NavigationClick;
            view_include_officer.Visibility = ViewStates.Gone;
            view_btn_select_officer.Visibility = ViewStates.Gone;
            view_btn_select_officer.Click += View_btn_select_officer_Click;
            view_btn_update_case.Click += View_btn_update_case_Click;

            view_input_case_status.Enabled = false;
            input_case_date_created.Enabled = false;
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .Document(id)
                .AddSnapshotListener((values, error) =>
                {
                    if (values.Exists)
                    {
                        var c = values.ToObject<Case>();

                        input_case_name.Text = c.CaseName;
                        input_case_note.Text = c.Note;
                        input_case_date_created.Text = $"{c.DateCreated.ToDateTime():dddd, dd-MMM-yyyy, HH:mm tt}";
                        view_input_case_status.Text = c.Status;


                        if (c.OfficerId != null)
                        {
                            CrossCloudFirestore
                                .Current
                                .Instance
                                .Collection("OFFICERS")
                                .Document(c.OfficerId)
                                .AddSnapshotListener((value, error) =>
                                {
                                    if (value.Exists)
                                    {
                                        var user = value.ToObject<OfficerModel>();
                                        txt_name.Text = user.Name;
                                        txt_surname.Text = user.Surname;
                                        ImageService.Instance.LoadUrl(user.ImageUrl)
                                            .DownSampleInDip(150, 150)
                                            .Retry(3, 200)
                                            .IntoAsync(profile_img);
                                        

                                    }
                                });
                            view_include_officer.Visibility = ViewStates.Visible;
                            view_btn_select_officer.Visibility = ViewStates.Gone;
                        }
                        else
                        {
                            view_btn_select_officer.Visibility = ViewStates.Visible;
                            view_include_officer.Visibility = ViewStates.Gone;
                        }
                    }
                });
        }

        private async void View_btn_update_case_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>
            {
                { "CaseName", input_case_name.Text },
                { "OfficerId", officer_id },
                { "Note", input_case_note.Text },
                { "Status", "PROGRESS" },
                { "LastUpdate", FieldValue.ServerTimestamp }
            };
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .Document(id)
                .UpdateAsync(keyValues);
            if(officer_id != null)
            {
                if (officer_id != null)
                {
                    var stream = Resources.Assets.Open("service_account.json");
                    var fcm = FirebaseHelper.FirebaseAdminSDK.GetFirebaseMessaging(stream);
                    FirebaseAdmin.Messaging.Message message = new FirebaseAdmin.Messaging.Message()
                    {
                        Topic = officer_id,
                        Notification = new Notification()
                        {
                            Title = "New Case Added",
                            Body = $"You have been assigned to a Case {input_case_name.Text}",

                        },
                    };
                    await fcm.SendAsync(message);

                }
            }
            AndHUD.Shared.ShowSuccess(context, "You have successfully updated a case record", MaskType.Clear, TimeSpan.FromSeconds(2));

        }

        private async void View_btn_select_officer_Click(object sender, EventArgs e)
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
                if (officer_id != null)
                {
                    view_include_officer.Visibility = ViewStates.Visible;
                    view_btn_select_officer.Visibility = ViewStates.Gone;
                    txt_surname.Text = officer.Surname;
                    txt_name.Text = officer.Name;
                    
                }
            });
            builder.Show();
        }

        private void View_toolbar_NavigationClick(object sender, AndroidX.AppCompat.Widget.Toolbar.NavigationClickEventArgs e)
        {
            Dismiss();
        }
    }
}