using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Android.Util;
using Java.Util;
namespace PartyPlayer.Bluetooth
{
	class Bluetooth
	{
		private static BluetoothSocket btSocket;
		public static bool IsConnected 
		{ 
			get
			{
				return btSocket.IsConnected;
			}
		}

		
		public static async Task DeviceConnect()
		{
			BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
			
			
			if (btAdapter == null)
				Toast.MakeText(Application.Context, "No Bluetooth adapter found.", ToastLength.Short);

			if (!btAdapter.IsEnabled)
				Toast.MakeText(Application.Context, "Bluetooth adapter is not enabled", ToastLength.Short);

			if (PPApplication.device == null)
				Toast.MakeText(Application.Context, "Named device not found", ToastLength.Short);
			btSocket = PPApplication.device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

			await btSocket.ConnectAsync();
			
		
		}
		public static async Task SendData(string keycode)
		{
			
				
				byte[] buffer = Encoding.Default.GetBytes(keycode + "\n");
				btSocket.OutputStream.Write(buffer, 0, buffer.Length);

		}
		
	}
}