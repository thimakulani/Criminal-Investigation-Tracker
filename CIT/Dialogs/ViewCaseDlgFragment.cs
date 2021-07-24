using Android.OS;
using Android.Views;
using Android.Widget;
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
        private MaterialTextView txt_name;
        private MaterialTextView txt_surname;
        private AppCompatImageView profile_img;
        private TextInputEditText input_case_name;
        private TextInputEditText input_case_note;
        private TextInputEditText input_case_date_created;
        private TextInputEditText view_input_case_status;
        private LinearLayout view_include_officer;

        private void ConnectViews(View view)
        {

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



            view_toolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back_black_18dp);
            view_toolbar.NavigationClick += View_toolbar_NavigationClick;
            view_include_officer.Visibility = ViewStates.Gone;
            view_btn_select_officer.Visibility = ViewStates.Gone;




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

        private void View_toolbar_NavigationClick(object sender, AndroidX.AppCompat.Widget.Toolbar.NavigationClickEventArgs e)
        {
            Dismiss();
        }
    }
}