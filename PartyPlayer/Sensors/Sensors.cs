using System.Text;
using Android.App;
using Android.Hardware;
using Android.OS;
using Android.Widget;


namespace PartyPlayer.Sensors
{

    [Activity(Label = "MotionDetector", MainLauncher = true, Icon = "@drawable/icons")]
    public class Sensors: Activity, ISensorEventListener
    {
        private float _shake;
        public bool Shaked = false;
        public bool Upside = false;
        private static readonly object _syncLock = new object();
        private SensorManager _sensorManager;
        private TextView _sensorTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _sensorManager = (SensorManager)GetSystemService(SensorService);
           // _sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);

            _sensorTextView.TextSize = 30;
        }

        protected override void OnResume()
        {
            base.OnResume();
            _sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Ui);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // We don't want to do anything here.
        }

        public void OnSensorChanged(SensorEvent e)
        {
            lock (_syncLock)
            {
                var text = new StringBuilder("x = ")
                    .Append(e.Values[0])
                    .Append(", y=")
                    .Append(e.Values[1])
                    .Append(", z=")
                    .Append(e.Values[2]);

                _sensorTextView.Text = text.ToString();
                 _shake = (e.Values[0] + e.Values[1] + e.Values[2]) / 3;

                  if (_shake >= 5 && e.Values[2] > 0)
                     Shaked = true;
               // _sensorTextView.Text = "dupa";
                  else
                     Shaked = false;

                if (e.Values[2] > 9)
                     Upside = true;
                    //_sensorTextView.Text=text.ToString();
                else
                    Upside = false;
            }

        }

    }
  
}
