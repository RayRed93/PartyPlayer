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
		private BluetoothSocket btSocket;
		public async Task DeviceConnect(byte[] buffer)
		{
			BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
			
			
			if (btAdapter == null)
				Toast.MakeText(Application.Context, "No Bluetooth adapter found.", ToastLength.Short);

			if (!btAdapter.IsEnabled)
				Toast.MakeText(Application.Context, "Bluetooth adapter is not enabled", ToastLength.Short);
			
			
			
			BluetoothDevice device = (from bd in btAdapter.BondedDevices
									  where bd.Name == "NameOfTheDevice"
									  select bd).FirstOrDefault();

			if (device == null)
				Toast.MakeText(Application.Context, "Named device not found", ToastLength.Short);

			btSocket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
			await btSocket.ConnectAsync();

			//await _socket.InputStream.ReadAsync(buffer, 0, buffer.Length);

			

			

		}
		private async Task SendData(byte[] buffer)
		{

			await btSocket.OutputStream.WriteAsync(buffer, 0, buffer.Length);

		}
		
	}
}