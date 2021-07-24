using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using CIT.Activities;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIT.FirebaseHelper
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseMessagingModel : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage remoteMessage)
        {
            if (remoteMessage.GetNotification() != null)
            {
                SendNotification(remoteMessage);
            }
            base.OnMessageReceived(remoteMessage);
        }

        private void SendNotification(RemoteMessage remoteMessage)
        {
            var intent = new Intent(this, typeof(HomeActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0 /* Request code */, intent, PendingIntentFlags.OneShot);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this,"100")
                .SetSmallIcon(Resource.Mipmap.ic_exit_to_app_black_18dp)
                .SetContentTitle(remoteMessage.GetNotification().Title)
                .SetContentText(remoteMessage.GetNotification().Body)
                .SetAutoCancel(true)
                .SetSound(defaultSoundUri)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);

            notificationManager.Notify(0 /* ID of notification */, notificationBuilder.Build());
        }
    }
}