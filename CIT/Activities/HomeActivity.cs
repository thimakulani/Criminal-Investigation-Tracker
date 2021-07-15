using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using CIT.Fragments;
using CIT.Models;
using Firebase.Auth;
using IsmaelDiVita.ChipNavigationLib;
using Plugin.CloudFirestore;

namespace CIT.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class HomeActivity : AppCompatActivity, ChipNavigationBar.IOnItemSelectedListener
    {
        private ChipNavigationBar nav_bar;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            if(savedInstanceState == null)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Add(Resource.Id.frag_host, new HomeFragment())
                    .Commit();
            }



            nav_bar = FindViewById<ChipNavigationBar>(Resource.Id.bottom_nav);
     

            var results = await CrossCloudFirestore
                .Current
                .Instance
                .Collection("OFFICERS")
                .Document("12345")
                .GetAsync();

            var user = results.ToObject<OfficerModel>();
            //if (user.Role == "A")
            //{
            nav_bar.SetMenuResource(Resource.Menu.admin_nav_menu);
            nav_bar.SetOnItemSelectedListener(this);
            //}
            //else
            //{

            //}

        }

        public void OnItemSelected(int id)
        {
            
        }
    }

}