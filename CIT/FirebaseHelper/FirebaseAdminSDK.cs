using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CIT.FirebaseHelper
{
    public class FirebaseAdminSDK
    {
        public static FirebaseAdmin.Auth.FirebaseAuth GetFirebaseAdminAuth(Stream input)
        {
            FirebaseAdmin.Auth.FirebaseAuth auth;
            FirebaseAdmin.AppOptions options = new FirebaseAdmin.AppOptions()
            {
                Credential = GoogleCredential.FromStream(input),
                ProjectId = "criminal-investigator-tracker",
                ServiceAccountId = "firebase-adminsdk-8150l@criminal-investigator-tracker.iam.gserviceaccount.com",

            };
            if (FirebaseAdmin.FirebaseApp.DefaultInstance == null)
            {
                var app = FirebaseAdmin.FirebaseApp.Create(options);
                auth = FirebaseAdmin.Auth.FirebaseAuth.GetAuth(app);
            }
            else
            {
                auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            }
            return auth;
        }
    }
}