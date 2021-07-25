using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if(savedInstanceState == null)
            {
                CrossCloudFirestore.Current.Instance.FirestoreSettings = new FirestoreSettings
                {
                    IsPersistenceEnabled = false
                };
            }
            // Set our view from the "main" layout resource
            Task startWork = new Task(() =>
            {
                Task.Delay(3000);
            });
            startWork.ContinueWith(t =>
            {
                var user = FirebaseAuth.Instance.CurrentUser;
                if (user != null)
                {
                    Intent intent = new Intent(Application.Context, typeof(HomeActivity));
                    StartActivity(intent);
                    //OverridePendingTransition(Resource.Animation.Side_in_right, Resource.Animation.Side_out_left);
                    Finish();
                }
                else
                {
                    Intent intent = new Intent(Application.Context, typeof(LoginActivity));
                    StartActivity(intent);
                   // OverridePendingTransition(Resource.Animation.Side_in_right, Resource.Animation.Side_out_left);
                    Finish();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            startWork.Start();
        }

    }
}