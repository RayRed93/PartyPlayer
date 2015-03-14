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
	[Activity(Label = "Player")]
	public class PlayerActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
			SetContentView(Resource.Layout.Player);

			FindViewById<Button>(Resource.Id.muteButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("173"); };
			FindViewById<Button>(Resource.Id.prevButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("177"); };
			FindViewById<Button>(Resource.Id.playButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("179"); };
			FindViewById<Button>(Resource.Id.nextButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("176"); };
			FindViewById<Button>(Resource.Id.volumeUpButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("175"); };
			FindViewById<Button>(Resource.Id.volumeDownButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("174"); };
			
		}

		protected override void OnStop()
		{
			base.OnStop();
			Bluetooth.Bluetooth.SendData("151");
		}

		//protected override void OnResume()
		//{
		//	base.OnResume();
		//	if(!Bluetooth.Bluetooth.IsConnected)
		//		Bluetooth.Bluetooth.DeviceConnect();
		//}

		
	}
}