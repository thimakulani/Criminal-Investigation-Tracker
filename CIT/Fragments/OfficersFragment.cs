using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using System;

namespace CIT.Fragments
{
    public class OfficersFragment : Fragment
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
            View view = inflater.Inflate(Resource.Layout.officers_fragments, container, false);
            ConnectViews(view);
            return view;
        }

        private void ConnectViews(View view)
        {
            
        }
    }
}