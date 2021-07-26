using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using CIT.Adapters;
using CIT.Models;
using FFImageLoading;
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using ID.IonBit.IonAlertLib;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;

namespace CIT.Dialogs
{
    public class ViewSuspectDlgFragment : Fragment
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
            View view = inflater.Inflate(Resource.Layout.officer_view_case_fragment, container, false);

            ConnectViews(view);

            return view;
        }

        private void ConnectViews(View view)
        {
            
        }
    }
}