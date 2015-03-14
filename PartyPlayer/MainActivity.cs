using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;

using PartyPlayer.Bluetooth;
using System.Threading.Tasks;
using System.Text;

namespace PartyPlayer
{
	[Activity(Label = "PartyPlayer", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			
			SetContentView(Resource.Layout.Main);
			
			Button button = FindViewById<Button>(Resource.Id.MyButtonn);

			button.Click += delegate 
			{ 
				StartActivityForResult(new Intent(this, typeof(SelectDevice)), 1); 
			};
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			var address = data.GetStringExtra(SelectDevice.EXTRA_DEVICE_ADDRESS);

			BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
			PPApplication.device = (from bd in btAdapter.BondedDevices
									where bd.Address == address
									  select bd).FirstOrDefault();
			AsyncConnect();
			StartActivity(new Intent(this, typeof(PlayerActivity)));
			
		}

		private void AsyncConnect()
		{
			Bluetooth.Bluetooth.DeviceConnect();
			Bluetooth.Bluetooth.SendData("176");
		}
	}
}

