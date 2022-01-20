using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using CIT.Fragments;
using CIT.Models;
using Firebase.Auth;
using Firebase.Messaging;
using Google.Android.Material.AppBar;
using IsmaelDiVita.ChipNavigationLib;
using Plugin.CloudFirestore;

namespace CIT.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class HomeActivity : AppCompatActivity, ChipNavigationBar.IOnItemSelectedListener
    {
        private ChipNavigationBar nav_bar;
        private MaterialToolbar toolbar_home;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);


            
            



            nav_bar = FindViewById<ChipNavigationBar>(Resource.Id.bottom_nav);
            toolbar_home = FindViewById<MaterialToolbar>(Resource.Id.toolbar_home);


            if (FirebaseAuth.Instance.CurrentUser.Email.Contains("admin"))
            {
                
                nav_bar.SetMenuResource(Resource.Menu.admin_nav_menu);
                nav_bar.SetOnItemSelectedListener(this);
                nav_bar.SetItemSelected(Resource.Id.nav_home);
                SupportFragmentManager
                    .BeginTransaction()
                    .Add(Resource.Id.frag_host, new HomeFragment())
                    .Commit();
            }
            else
            {
                nav_bar.SetMenuResource(Resource.Menu.officer_nav_menu);
                nav_bar.SetOnItemSelectedListener(this);
                nav_bar.SetItemSelected(Resource.Id.officer_nav_home);
                FirebaseMessaging.Instance
                    .SubscribeToTopic(FirebaseAuth.Instance.Uid);
                SupportFragmentManager
                    .BeginTransaction()
                    .Add(Resource.Id.frag_host, new OfficerHomeFragment())
                    .Commit();
                CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("CASES")
                    .OrderBy("DateCreated", true)
                    .WhereEqualsTo("OfficerId", FirebaseAuth.Instance.CurrentUser.Uid)
                    .AddSnapshotListener((value, error) =>
                    {
                        if (!value.IsEmpty)
                        {
                            nav_bar.ShowBadge(Resource.Id.officer_nav_home, value.Count);
                        }
                        else
                        {
                            nav_bar.ShowBadge(Resource.Id.officer_nav_home, 0);
                        }
                    });
                CrossCloudFirestore
                   .Current
                   .Instance
                   .Collection("OFFICERS")
                   .Document(FirebaseAuth.Instance.CurrentUser.Uid)
                   .AddSnapshotListener((value, error) =>
                   {
                       if (value.Exists)
                       {
                           var user = value.ToObject<OfficerModel>();
                           toolbar_home.Title = $"{user.Name} {user.Surname}";
                       }
                   });
            }

           

            //var user = results.ToObject<OfficerModel>();
            //if (user.Role == "A")
            //{

            //}
            //else
            //{

            //}

        }

        public void OnItemSelected(int id)
        {
            if(Resource.Id.nav_home == id)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.frag_host, new HomeFragment())
                    .Commit();
                
            }
            if (Resource.Id.nav_add_officer == id)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.frag_host, new OfficersFragment())
                    .Commit();
            }
            //if (Resource.Id.nav_predict_results == id)
            //{
            //    SupportFragmentManager
            //        .BeginTransaction()
            //        .Replace(Resource.Id.frag_host, new PredictSuspectFragment())
            //        .Commit();
            //}
            if (Resource.Id.nav_case_history == id)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.frag_host, new CaseHistoryFragment())
                    .Commit();
            }



            if (Resource.Id.officer_nav_home == id)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.frag_host, new OfficerHomeFragment())
                    .Commit();
            }
            if (Resource.Id.officer_nav_case_history == id)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.frag_host, new CaseHistoryFragment())
                    .Commit();
            }
            //if (Resource.Id.officer_nav_predict_results == id)
            //{
            //    SupportFragmentManager
            //        .BeginTransaction()
            //        .Replace(Resource.Id.frag_host, new CaseHistoryFragment())
            //        .Commit();
            //}
            //if (Resource.Id.officer_nav_profile == id)
            //{
            //    SupportFragmentManager
            //        .BeginTransaction()
            //        .Replace(Resource.Id.frag_host, new CaseHistoryFragment())
            //        .Commit();
            //}
            if (Resource.Id.nav_logout == id)
            {
                FirebaseAuth.Instance.SignOut();
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    base.FinishAndRemoveTask();
                }
                else
                {
                    base.Finish();
                }
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

}