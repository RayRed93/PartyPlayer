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
using Android.Util;
using Android.Bluetooth;

namespace PartyPlayer.Bluetooth
{

	public class BluetoothDevicesAdapter : ArrayAdapter<string>
	{
		public BluetoothDevicesAdapter(Context context, int ResourceId) : base(context, ResourceId)
		{
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (view == null)
				view =  (base.Context.GetSystemService(Activity.LayoutInflaterService) as LayoutInflater).Inflate(Resource.Layout.bluetooth_device, null);
		    view.FindViewById<TextView>(Resource.Id.txtView).Text = base.GetItem(position);
			if (position < SelectDevice.pariedDevicesCount)
			{
                view.FindViewById<TextView>(Resource.Id.txtView).SetCompoundDrawablesWithIntrinsicBounds(base.Context.Resources.GetDrawable(Resource.Drawable.paired),
					null, null, null);
			}

			return view;
		}
	}

	[Activity(Label = "SelectDevice")]
	public class SelectDevice : Activity
	{
		
		private const string TAG = "DeviceListActivity";
		private const bool Debug = true;

		public const string EXTRA_DEVICE_ADDRESS = "device_address";
		public static int pariedDevicesCount = 0;

		
		private BluetoothAdapter btAdapter;
		private static BluetoothDevicesAdapter pairedDevicesArrayAdapter;
		private static BluetoothDevicesAdapter newDevicesArrayAdapter;
		private Receiver receiver;
		private static Button scanButton;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			
			RequestWindowFeature(WindowFeatures.IndeterminateProgress);
			SetContentView(Resource.Layout.device_list);

			
			SetResult(Result.Canceled);

						
			scanButton = FindViewById<Button>(Resource.Id.button_scan);
			scanButton.Click += (sender, e) =>
			{
				DoDiscovery();
				(sender as View).Enabled = false;
			};

			pairedDevicesArrayAdapter = new BluetoothDevicesAdapter(this, Resource.Layout.bluetooth_device);
			
			var pairedListView = FindViewById<ListView>(Resource.Id.paired_devices);
			pairedListView.Adapter = pairedDevicesArrayAdapter;
			pairedListView.ItemClick += DeviceListClick;


			
			receiver = new Receiver(this);
			var filter = new IntentFilter(BluetoothDevice.ActionFound);
			RegisterReceiver(receiver, filter);

			
			filter = new IntentFilter(BluetoothAdapter.ActionDiscoveryFinished);
			RegisterReceiver(receiver, filter);
			btAdapter = BluetoothAdapter.DefaultAdapter;

			
			var pairedDevices = btAdapter.BondedDevices;

			
			if (pairedDevices.Count > 0)
			{
				FindViewById<View>(Resource.Id.title_paired_devices).Visibility = ViewStates.Visible;
				foreach (var device in pairedDevices)
				{
					pairedDevicesArrayAdapter.Add(device.Name + "\n" + device.Address);
					pariedDevicesCount++;
				}
			}
			else
			{
				String noDevices = Resources.GetText(Resource.String.none_paired);
				pairedDevicesArrayAdapter.Add(noDevices);
			}

		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			
			if (btAdapter != null)
			{
				btAdapter.CancelDiscovery();
			}
			UnregisterReceiver(receiver);
		}

		private void DoDiscovery()
		{
			if (Debug)
				Log.Debug(TAG, "doDiscovery()");

			
			SetProgressBarIndeterminateVisibility(true);
			SetTitle(Resource.String.scanning);

			
			if (btAdapter.IsDiscovering)
			{
				btAdapter.CancelDiscovery();
			}
			btAdapter.StartDiscovery();
		}


		
		void DeviceListClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			
			btAdapter.CancelDiscovery();

			if(e.Position > pariedDevicesCount - 1)
			{
				Intent intentOpenBluetoothSettings = new Intent(Android.Provider.Settings.ActionBluetoothSettings);
				StartActivity(intentOpenBluetoothSettings);
			}
			
			var info = (e.View as TextView).Text.ToString();
			var address = info.Substring(info.Length - 17);

			
			Intent intent = new Intent();
			intent.PutExtra(EXTRA_DEVICE_ADDRESS, address);

			
			SetResult(Result.Ok, intent);
			Finish();
		}
		public class Receiver : BroadcastReceiver
		{
			Activity _chat;

			public Receiver(Activity chat)
			{
				_chat = chat;
			}

			public override void OnReceive(Context context, Intent intent)
			{
				string action = intent.Action;

				if (action == BluetoothDevice.ActionFound)
				{
					BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
					if (device.BondState != Bond.Bonded)
					{
						pairedDevicesArrayAdapter.Add(device.Name + "\n" + device.Address);
					}
				}
				else if (action == BluetoothAdapter.ActionDiscoveryFinished)
				{
					_chat.SetProgressBarIndeterminateVisibility(false);
					_chat.SetTitle(Resource.String.select_device);
					if (pairedDevicesArrayAdapter.Count == 0)
					{
						var noDevices = _chat.Resources.GetText(Resource.String.none_found).ToString();
						pairedDevicesArrayAdapter.Add(noDevices);
					}
					scanButton.Enabled = true;
				}
			}
		}
	}
}