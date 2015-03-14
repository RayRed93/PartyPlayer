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
        private static readonly object SyncLock = new object();
        private SensorManager sensorManager;
        
        
        
        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

            sensorManager = (SensorManager)GetSystemService(SensorService);
            accel = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            ori = sensorManager.GetDefaultSensor(SensorType.Orientation);

			RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
			SetContentView(Resource.Layout.Player);

			FindViewById<Button>(Resource.Id.backButton).Click += (sender, e) => { StartActivity(new Intent(this, typeof(MainActivity))); };
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

	    protected override void OnStop()
		{
			base.OnStop();
			Bluetooth.Bluetooth.SendData("151");
		}



        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
        }

        public void OnSensorChanged(SensorEvent e)
        {
           lock (SyncLock)
            {

                if (e.Sensor.Type == SensorType.Orientation)
                    _flip = e.Values[0];
                if (e.Sensor.Type == SensorType.Accelerometer)
                    _shake = (e.Values[0] * e.Values[1] * e.Values[2])/3;


                if (_flip > 80 || _flip < 100) Upside = true;
                else Upside = false;
                if (_shake > 4) Shaked = true;
                else Shaked =false;
                Log.Debug(_flip.ToString(),_shake.ToString());

            }

        
        }
    }
}