using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using System;

namespace CIT.Fragments
{
    public class PredictFragment : Fragment
    {
        private RecyclerView recycler_predict;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            base.OnCreateView(inflater, container, savedInstanceState);
            var view =  inflater.Inflate(Resource.Layout.predict_suspect_fragment, container, false);
            ConnectViews(view);
            return view;
        }

        private void ConnectViews(View view)
        {
            recycler_predict = view.FindViewById<RecyclerView>(Resource.Id.recycler_predict);
        }
    }
}