using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using System;

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
        private MaterialButton btn_select_officer;
        private MaterialButton btn_add_officer;
        private void ConnectViews(View view)
        {
            
        }
        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }
    }
}