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
	[Activity(Label = "", MainLauncher = true, Icon = "@drawable/icons")]
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
			if (resultCode != Result.Ok)
				return;

			var address = data.GetStringExtra(SelectDevice.EXTRA_DEVICE_ADDRESS);

			BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
			PPApplication.device = (from bd in btAdapter.BondedDevices
									where bd.Address == address
									  select bd).FirstOrDefault();
			AsyncConnect();
			
		}

		private async Task AsyncConnect()
		{
			await Bluetooth.Bluetooth.DeviceConnect();
			if (!Bluetooth.Bluetooth.IsConnected)
			{
				RunOnUiThread(() => FindViewById<Button>(Resource.Id.MyButtonn).Text ="Dupa");
			}
			else
				StartActivity(new Intent(this, typeof(PlayerActivity)));
			
		}
	}
}

