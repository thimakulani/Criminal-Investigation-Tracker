using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Views;
using AndroidHUD;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using CIT.Models;
using FFImageLoading;
using Firebase.Auth;
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using ID.IonBit.IonAlertLib;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;


namespace CIT.Dialogs
{
    public class ResetPasswordDlgFragment : DialogFragment, IOnSuccessListener, IOnFailureListener, IOnCompleteListener
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        private FloatingActionButton BtnCloseDialog;
        private TextInputEditText ResetInputEmail;
        private MaterialButton BtnReset;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
             base.OnCreateView(inflater, container, savedInstanceState);
             View view = inflater.Inflate(Resource.Layout.reset_password, container, false);

            ConnectViews(view);
            return view;
        }

        private void ConnectViews(View view)
        {
            context = view.Context;
            ResetInputEmail = view.FindViewById<TextInputEditText>(Resource.Id.ResetInputEmail);
            BtnReset = view.FindViewById<MaterialButton>(Resource.Id.BtnReset);
            BtnCloseDialog = view.FindViewById<FloatingActionButton>(Resource.Id.FabCloseResetDialog);
            BtnCloseDialog.Click += BtnCloseDialog_Click;
            BtnReset.Click += BtnReset_Click;
        }

        private void BtnCloseDialog_Click(object sender, EventArgs e)
        {
            Dismiss();
        }

        private Context context;
        IonAlert loadingDialog;
        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ResetInputEmail.Text))
            {
                ResetInputEmail.Error = "Please provide your email address";//, ToastLength.Long).Show();
                //ResetInputEmail.RequestFocus();
                return;
            }
            loadingDialog = new IonAlert(context, IonAlert.ProgressType);
            loadingDialog.SetSpinKit("DoubleBounce")
                .ShowCancelButton(false)
                .Show();
            FirebaseAuth.Instance.SendPasswordResetEmail(ResetInputEmail.Text.Trim())
                .AddOnSuccessListener(this)
                .AddOnCompleteListener(this)
                .AddOnFailureListener(this);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            AndHUD.Shared.ShowSuccess(context, "An email has been sent to you.", MaskType.Clear, TimeSpan.FromSeconds(2));
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            AndHUD.Shared.ShowError(context, e.Message, MaskType.Clear, TimeSpan.FromSeconds(2));
        }

        public void OnComplete(Task task)
        {
            loadingDialog.Dismiss();
        }
        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }
    }
}