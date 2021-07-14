using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using IsmaelDiVita.ChipNavigationLib;

namespace CIT.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class HomeActivity : AppCompatActivity, ChipNavigationBar.IOnItemSelectedListener
    {
        private ChipNavigationBar nav_bar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            nav_bar = FindViewById<ChipNavigationBar>(Resource.Id.bottom_nav);
            nav_bar.SetMenuResource(Resource.Menu.admin_nav_menu);
            nav_bar.SetOnItemSelectedListener(this);
        }

        public void OnItemSelected(int id)
        {
            Toast.MakeText(this, "", ToastLength.Long).Show();
        }
    }

}