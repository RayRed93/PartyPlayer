using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using PartyPlayer.Bluetooth;

namespace PartyPlayer
{
	[Activity(Label = "PlayerActivity")]
	public class PlayerActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Player);

			FindViewById<Button>(Resource.Id.backButton).Click += (sender, e) => { StartActivity(new Intent(this, typeof(MainActivity))); };
			FindViewById<Button>(Resource.Id.prevButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("177"); };
			FindViewById<Button>(Resource.Id.playButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("179"); };
			FindViewById<Button>(Resource.Id.nextButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("176"); };
			FindViewById<Button>(Resource.Id.volumeUpButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("175"); };
			FindViewById<Button>(Resource.Id.volumeDownButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("174"); };
		}
	}
}