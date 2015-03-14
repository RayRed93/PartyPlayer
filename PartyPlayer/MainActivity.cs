using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using PartyPlayer.Bluetooth;
using System.Threading.Tasks;
using System.Text;

namespace PartyPlayer
{
	[Activity(Label = "PartyPlayer", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			
			Button button = FindViewById<Button>(Resource.Id.MyButton);

			button.Click += delegate 
			{ 
				StartActivityForResult(new Intent(this, typeof(SelectDevice)), 1); 
			};
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			var address = data.GetStringExtra(SelectDevice.EXTRA_DEVICE_ADDRESS);
			AsyncConnect(address);
			
		}

		private async Task AsyncConnect(string address)
		{
			await Bluetooth.Bluetooth.DeviceConnect(address);
			Bluetooth.Bluetooth.SendData(Encoding.Default.GetBytes("slawek pedal"));

		}
	}
}

