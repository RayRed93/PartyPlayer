using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using PartyPlayer.Bluetooth;

namespace PartyPlayer
{

	

	[Activity(Label = "Player")]
	public class PlayerActivity : Activity, ISensorEventListener
	{

		private Sensor accel;
		private Sensor ori;
		private float _shake = 0;
		private float _flip = 0;
		public bool Shaked = false;
		public bool Upside = false;
		private bool previous_Upside;
		private static readonly object SyncLock = new object();
		private SensorManager sensorManager;
		
		
		
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			sensorManager = (SensorManager)GetSystemService(SensorService);
			accel = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
			ori = sensorManager.GetDefaultSensor(SensorType.Orientation);
			sensorManager.RegisterListener(this, ori, SensorDelay.Fastest);
			RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
			SetContentView(Resource.Layout.Player);

			FindViewById<Button>(Resource.Id.muteButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("173"); };
			FindViewById<Button>(Resource.Id.prevButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("177"); };
			FindViewById<Button>(Resource.Id.playButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("179"); };
			FindViewById<Button>(Resource.Id.nextButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("176"); };
			FindViewById<Button>(Resource.Id.volumeUpButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("175"); };
			FindViewById<Button>(Resource.Id.volumeDownButton).Click += (sender, e) => { Bluetooth.Bluetooth.SendData("174"); };
			
		}

		protected override void OnPause()
		{
			base.OnPause();
			sensorManager.UnregisterListener(this);
		}

		protected override async void OnStop()
		{
			base.OnStop();
			await Bluetooth.Bluetooth.SendData("151");
		}



		public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
		{
		//nothing
		}

		public void OnSensorChanged(SensorEvent e)
		{
		   lock (SyncLock)
			{

				if (e.Sensor.Type == SensorType.Orientation)
					_flip = e.Values[1];
				if (e.Sensor.Type == SensorType.Accelerometer)
					_shake = (e.Values[0] * e.Values[1] * e.Values[2])/3;


				if (Math.Abs(_flip) > 170 && Math.Abs(_flip) < 190)
					Upside = true;
				else 
					Upside = false;

				if (_shake > 4)
					Bluetooth.Bluetooth.SendData("176");

				Log.Debug("d", _shake.ToString());

				if (Upside != previous_Upside)
				   Bluetooth.Bluetooth.SendData("173");
				previous_Upside = Upside;
			}

		
		}
	}
}