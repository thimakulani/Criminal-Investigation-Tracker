using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using System;

namespace CIT.Dialogs
{
    public class AddOfficerDlgFragment : DialogFragment
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
            View view = inflater.Inflate(Resource.Layout.add_officer_fragment, container, false);
            ConnectViews(View);
            return view;
        }

        private TextInputEditText Input_name;
        private TextInputEditText Input_surname;
        private TextInputEditText Input_phone;
        private TextInputEditText Input_email;
        private TextInputEditText Input_address;
        private MaterialButton btn_attachement;

        private void ConnectViews(View view)
        {
            Input_name = view.FindViewById<TextInputEditText>(Resource.Id.input_name);
            Input_phone = view.FindViewById<TextInputEditText>(Resource.Id.input_mobile);
            Input_surname = view.FindViewById<TextInputEditText>(Resource.Id.input_lastname);
            Input_address = view.FindViewById<TextInputEditText>(Resource.Id.input_address);
            Input_email = view.FindViewById<TextInputEditText>(Resource.Id.input_email_address);

            btn_attachement.Click += Btn_attachement_Click;
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