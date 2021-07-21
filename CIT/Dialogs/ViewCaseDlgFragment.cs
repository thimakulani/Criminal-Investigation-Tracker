using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using CIT.Models;
using Plugin.CloudFirestore;
using System;

namespace CIT.Dialogs
{
    public class ViewCaseDlgFragment : DialogFragment
    {
        private readonly string id;

        public ViewCaseDlgFragment(string id)
        {
            this.id = id;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            SetStyle(StyleNoFrame, Resource.Style.FullScreenDialogStyle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.view_full_case_fragemnt, container, false);

            ConnectViews(id);

            return view;
        }

        private void ConnectViews(string id)
        {
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("CASES")
                .Document(id)
                .AddSnapshotListener((values, error) =>
                {
                    if (values.Exists)
                    {
                        var c = values.ToObject<Case>();

                        if(c.OfficerId != null)
                        {
                            CrossCloudFirestore
                                .Current
                                .Instance
                                .Collection("OFFICERS")
                                .Document(c.OfficerId)
                                .AddSnapshotListener((value, error) =>
                                {
                                    if (value.Exists)
                                    {
                                        var user = value.ToObject<OfficerModel>();
                                        
                                    }
                                });
                        }
                    }
                });
        }
    }
}