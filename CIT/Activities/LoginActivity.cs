using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.App;
using CIT.Dialogs;
using Firebase.Auth;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using ID.IonBit.IonAlertLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIT.Activities
{
    [Activity(Label = "LandingPage")]
    public class LoginActivity : AppCompatActivity, IOnSuccessListener, IOnFailureListener, IOnCompleteListener
    {

        private TextView TxtForgotPassword;
        private TextInputEditText InputEmail;
        private TextInputEditText InputPassword;
        private MaterialButton BtnLogin;
        private IonAlert loadingDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.login_frag);
            BtnLogin = FindViewById<MaterialButton>(Resource.Id.BtnLogin);
            // TxtSignUp = FindViewById<TextView>(Resource.Id.TxtCreateAccount);
            TxtForgotPassword = FindViewById<TextView>(Resource.Id.TxtForgotPassword);
            InputEmail = FindViewById<TextInputEditText>(Resource.Id.LoginInputEmail);
            InputPassword = FindViewById<TextInputEditText>(Resource.Id.LoginInputPassword);
            TxtForgotPassword.Click += TxtForgotPassword_Click;
            BtnLogin.Click += BtnLogin_Click;
        }

        private void TxtForgotPassword_Click(object sender, EventArgs e)
        {
            ResetPasswordDlgFragment dlg = new ResetPasswordDlgFragment();
            dlg.Show(SupportFragmentManager.BeginTransaction(), null);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputEmail.Text) && string.IsNullOrWhiteSpace(InputEmail.Text))
            {
                InputEmail.Error = "Please provide your email";
                return;
            }
            if (string.IsNullOrEmpty(InputPassword.Text) && string.IsNullOrWhiteSpace(InputPassword.Text))
            {
                InputPassword.Error = "Please provide password";
                return;
            }
            BtnLogin.Enabled = false;
            loadingDialog = new IonAlert(this, IonAlert.ProgressType);
            loadingDialog.SetSpinKit("DoubleBounce")
                .ShowCancelButton(false)
                .Show();
            FirebaseAuth.Instance.SignInWithEmailAndPassword(InputEmail.Text.Trim(), InputPassword.Text.Trim())
                .AddOnSuccessListener(this)
                .AddOnCompleteListener(this)
                .AddOnFailureListener(this);
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            AndHUD.Shared.ShowError(this, e.Message, MaskType.Clear, TimeSpan.FromSeconds(3));
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            Intent intent = new Intent(Application.Context, typeof(HomeActivity));
            StartActivity(intent);
            //OverridePendingTransition(Resource.Animation.Side_in_right, Resource.Animation.Side_out_left);
            Finish();
        }

        public void OnComplete(Task task)
        {
            loadingDialog.Dismiss();
            BtnLogin.Enabled = true;
        }
    }
}