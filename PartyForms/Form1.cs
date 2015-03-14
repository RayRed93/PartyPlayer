using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;


namespace PartyForms
{
    public partial class Form1 : Form
    {
        private BluetoothAddress ddd;
        List<string> listdevices = new List<string>();
        public Form1()
        {
            InitializeComponent();
            Rectangle r = Screen.PrimaryScreen.WorkingArea;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            this.TopMost = true;
            pictureBox1.BackColor = Color.Red;
            //KeySimulationPC(Keys.MediaNextTrack);
        }

        private void transparentStop(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void transparentStart(object sender, EventArgs e)
        {
            this.Opacity = .5;
        }
        private void listBluetoth()
        {
            BluetoothClient client = new BluetoothClient();


            Task t = Task.Run(() =>
            {
                var devices = client.DiscoverDevicesInRange();
                foreach (BluetoothDeviceInfo d in devices)
                {
                    ddd = d.DeviceAddress;
                    //comboBox1.Items.Add(d.DeviceName);
                    //listdevices.Add(d.DeviceName);
                    listdevices.Add(d.DeviceAddress.ToString());
                }

            });
            t.Wait();
            if (t.IsCompleted)
            {
                foreach (string d in listdevices)
                {
                    comboBox1.Items.Add(d);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            comboBox1.Text = "Searching...";
            listBluetoth();
            comboBox1.Text = "Found:" + listdevices.Count;

        }

        private void KeySimulationPC(Keys key)
        {
            switch (key)
            {
                case Keys.MediaNextTrack:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MEDIA_NEXT_TRACK);
                    break;
                case Keys.MediaPlayPause:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MEDIA_PLAY_PAUSE);
                    break;
                case Keys.MediaPreviousTrack:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MEDIA_PREV_TRACK);
                    break;
                case Keys.MediaStop:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MEDIA_STOP);
                    break;
                case Keys.VolumeDown:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VOLUME_DOWN);
                    break;
                case Keys.VolumeUp:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VOLUME_UP);
                    break;
                case Keys.VolumeMute:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VOLUME_MUTE);
                    break;
            }
        }
        private void KeySimulationYT(Keys key)
        {
            switch (key)
            {
                case Keys.MediaNextTrack:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.END);
                    break;
                case Keys.MediaPlayPause:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
                    break;
                case Keys.MediaPreviousTrack:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.BACK);
                    break;
                case Keys.MediaStop:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
                    break;
                case Keys.VolumeDown:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.UP);
                    break;
                case Keys.VolumeUp:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.DOWN);
                    break;
                case Keys.VolumeMute:
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_M);
                    break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string n = comboBox1.Text;
            MessageBox.Show(n);
            BluetoothAddress addr = BluetoothAddress.Parse(n);
            Guid serviceClass;
            Guid UUID = new Guid("00001101-0000-1000-8000-00805f9b34fb");

            serviceClass = BluetoothService.RFCommProtocol;
           
            var ep = new BluetoothEndPoint(addr, serviceClass);
            var cli = new BluetoothClient();
            cli.Connect(ep);
            
            Stream peerStream = cli.GetStream();
            byte[] buf = new byte[1000];
            int readLen = peerStream.Read(buf, 0, buf.Length);
            if (readLen == 0) {
                Console.WriteLine("Connection is closed");
            } else {
                Console.WriteLine("Recevied {0} bytes", readLen);
            }
                MessageBox.Show("Koniec połączenia");
        }

        static void Process(IAsyncResult result)
        {
            MessageBox.Show("Dupa");
        }

        private static void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
        {
            MessageBox.Show("Dupa");
        }

      
        

    }
}
