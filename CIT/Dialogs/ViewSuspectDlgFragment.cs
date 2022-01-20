using Android.Content;
using Android.OS;
using Android.Views;
using AndroidHUD;
using AndroidX.Fragment.App;
using CIT.Models;
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Dialogs
{
    public class ViewSuspectDlgFragment : DialogFragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNoFrame, Resource.Style.FullScreenDialogStyle);

            // Create your fragment here
        }
        private readonly string suspect_id;
        private readonly string case_id;

        public ViewSuspectDlgFragment(string suspect_id, string case_id = null)
        {
            this.suspect_id = suspect_id;
            this.case_id = case_id;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.view_suspect_fragment, container, false);

            ConnectViews(view);
            return view;
        }
        private TextInputEditText input_relation;
        private TextInputEditText input_phone;
        private TextInputEditText input_name;
        private TextInputEditText input_surname;
        private TextInputEditText input_logical_score;
        private TextInputEditText input_physical_score;
        private TextInputEditText input_note;
        private TextInputEditText input_evidence;
        private MaterialButton btn_update;
        private MaterialToolbar view_suspect_toolbar;
        private Context context;
        private void ConnectViews(View view)
        {
            context = view.Context;
            input_relation = (TextInputEditText)view.FindViewById(Resource.Id.view_input_suspect_relation);
            input_phone = (TextInputEditText)view.FindViewById(Resource.Id.view_input_suspect_phone);
            input_evidence = (TextInputEditText)view.FindViewById(Resource.Id.view_input_suspect_evidence_type);
            input_physical_score = (TextInputEditText)view.FindViewById(Resource.Id.view_input_suspect_physical_score);
            input_name = (TextInputEditText)view.FindViewById(Resource.Id.view_input_suspect_name);
            input_surname = (TextInputEditText)view.FindViewById(Resource.Id.view_input_suspect_surname);
            input_logical_score = (TextInputEditText)view.FindViewById(Resource.Id.view_input_suspect_logical_score);
            input_note = (TextInputEditText)view.FindViewById(Resource.Id.suspect_input_note);
            btn_update = (MaterialButton)view.FindViewById(Resource.Id.view_btn_update_suspect);
            view_suspect_toolbar = (MaterialToolbar)view.FindViewById(Resource.Id.view_suspect_toolbar);
            btn_update.Click += Btn_update_Click;
            view_suspect_toolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back_black_18dp);
            view_suspect_toolbar.NavigationClick += (sender, o) =>
            {
                Dismiss();
            };

            CrossCloudFirestore
                .Current
                .Instance
                .Collection("SUSPECTS")
                .Document(suspect_id).AddSnapshotListener((value, error) =>
                {
                    if (value.Exists)
                    {
                        var suspect = value.ToObject<Suspect>();
                        input_evidence.Text = suspect.EvidenceType;
                        input_logical_score.Text = $"{suspect.LScore}";
                        input_physical_score.Text = $"{suspect.PScore}";
                        input_name.Text = suspect.Name;
                        input_surname.Text = suspect.Surname;
                        input_note.Text = suspect.Note;
                        input_relation.Text = suspect.Relation;
                        input_phone.Text = suspect.PhoneNumber;
                    }
                });


            
        }
        private void Btn_update_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "Name", input_name.Text },
                { "Surname", input_surname.Text },
                { "PhoneNumber", input_phone.Text },
                { "Relation", input_relation.Text },
                { "Notice", input_note.Text },
                { "EvidenceType", input_evidence.Text },
                { "LScore", int.Parse(input_logical_score.Text) },
                { "PScore",  int.Parse(input_physical_score.Text) }
            };
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("SUSPECTS")
                .Document(suspect_id)
                .UpdateAsync(data);
            AndHUD.Shared.ShowSuccess(context, "You have successfully updated suspect record", MaskType.Clear, TimeSpan.FromSeconds(2));
        }
    }
}