using Android.Content;
using Android.OS;
using Android.Views;
using AndroidHUD;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Plugin.CloudFirestore;
using Plugin.Media;
using System;
using System.Collections.Generic;

namespace CIT.Dialogs
{
    public class AddSuspectDlgFragment : DialogFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        private TextInputEditText input_name;
        private TextInputEditText input_surname;
        private TextInputEditText input_phone_number;
        private TextInputEditText input_relation;
        private TextInputEditText input_evidance_note;
        private MaterialButton btn_evidence;
        private MaterialButton btn_attach_file;
        private MaterialButton btn_add_suspect;
        private Context context;
        private readonly string case_id;

        public AddSuspectDlgFragment(string case_id)
        {
            this.case_id = case_id;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.add_suspect_fragment, container, false);
            context = view.Context;
            ConnectViews(view);
            return view;
        }

        private void ConnectViews(View view)
        {
            input_name = view.FindViewById<TextInputEditText>(Resource.Id.suspect_input_name);
            input_surname = view.FindViewById<TextInputEditText>(Resource.Id.suspect_input_lastname);
            input_phone_number = view.FindViewById<TextInputEditText>(Resource.Id.suspect_input_mobile);
            input_evidance_note = view.FindViewById<TextInputEditText>(Resource.Id.suspect_input_evidence);
            input_relation = view.FindViewById<TextInputEditText>(Resource.Id.suspect_input_relation);

            btn_attach_file = view.FindViewById<MaterialButton>(Resource.Id.btn_attach_file);
            btn_evidence = view.FindViewById<MaterialButton>(Resource.Id.btn_evidence_type);
            btn_add_suspect = view.FindViewById<MaterialButton>(Resource.Id.btn_add_suspect);

            btn_attach_file.Visibility = ViewStates.Gone;

            btn_add_suspect.Click += Btn_add_suspect_Click;
            btn_evidence.Click += Btn_evidence_Click;
            btn_attach_file.Click += Btn_attach_file_Click;
        }

        private void Btn_attach_file_Click(object sender, EventArgs e)
        {
            ChosePicture();
        }
        private byte[] imageArray;
        private async void ChosePicture()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Android.Widget.Toast.MakeText(context, "Upload not supported on this device", Android.Widget.ToastLength.Short).Show();
                return;
            }
            try
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    CompressionQuality = 40,

                });
                imageArray = System.IO.File.ReadAllBytes(file.Path);

            }
            catch (Exception E)
            {
                Android.Widget.Toast.MakeText(context, "Upload not supported on this device  " + E.Message, Android.Widget.ToastLength.Short).Show();
            }
        }
        private void Btn_evidence_Click(object sender, EventArgs e)
        {
            PopupMenu popup = new PopupMenu(context, btn_evidence);
            popup.Menu.Add(IMenu.None, 0, 1, "LOGICAL");
            popup.Menu.Add(IMenu.None, 1, 1, "PHYSICAL");
            popup.Show();
            popup.MenuItemClick += Popup_MenuItemClick;
        }
        int type = 0;
        private void Popup_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            btn_evidence.Text = e.Item.TitleFormatted.ToString();
            if (e.Item.ItemId == 0)
            {
                btn_attach_file.Visibility = ViewStates.Gone;
                type = 0;
            }
            if (e.Item.ItemId == 1)
            {
                btn_attach_file.Visibility = ViewStates.Visible;
                type = 1;
            }
        }

        private async void Btn_add_suspect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(input_name.Text))
            {
                input_name.Error = "cannot be empty";
                return;
            }
            if (string.IsNullOrEmpty(input_surname.Text))
            {
                input_surname.Error = "cannot be empty";
                return;
            }
            if (string.IsNullOrEmpty(input_phone_number.Text))
            {
                input_phone_number.Error = "cannot be empty";
                return;
            }
            if (string.IsNullOrEmpty(input_relation.Text))
            {
                input_relation.Error = "cannot be empty";
                return;
            }

            if (string.IsNullOrEmpty(input_evidance_note.Text))
            {
                input_evidance_note.Error = "cannot be empty";
                return;
            }
            if (btn_evidence.Text == "-EVIDANCE-")
            {
                btn_evidence.Error = "select evidence";
                return;
            }
            if(type == 1 && imageArray == null)
            {
                btn_attach_file.Error = "SELECT EVIDANCE";
            }
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "Name", input_name.Text },
                    { "Surname", input_surname.Text },
                    { "PhoneNumber", input_phone_number.Text },
                    { "Relation", input_relation.Text },
                    { "Notice", input_evidance_note.Text },
                    { "EvidenceType", btn_evidence.Text },
                    { "EvidanceUrl", null },
                    { "PScore", 0 },
                    { "LScore", 0 },
                    { "PrimeSuspect", null },
                    { "Case_Id", case_id },
                };
                // data.Add("", "");
                var query = await CrossCloudFirestore.Current
                    .Instance
                    .Collection("SUSPECTS")
                    .AddAsync(data);

                if(type == 1)
                {
                    var storage_ref = Plugin.FirebaseStorage.CrossFirebaseStorage
                     .Current
                     .Instance
                     .RootReference.Child(query.Id);
                     

                    await storage_ref.PutBytesAsync(imageArray);

                    var url = await storage_ref.GetDownloadUrlAsync();

                    //    .PutStreamAsync(file.GetStream());
                    await query
                        .UpdateAsync("EvidanceUrl", url.ToString());
                }
                AndHUD.Shared.ShowSuccess(context, "You have successfully added suspect record", MaskType.Clear, TimeSpan.FromSeconds(2));
                Dismiss();
            }
            catch (CloudFirestoreException ex)
            {
                Android.Widget.Toast.MakeText(context, ex.Message, Android.Widget.ToastLength.Long).Show();
            }
        }

        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }

    }
}