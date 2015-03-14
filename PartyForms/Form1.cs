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
        List<string> listdevices = new List<string>();
        bool flags = true;
        public Form1()
        {
            InitializeComponent();
            Rectangle r = Screen.PrimaryScreen.WorkingArea;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            this.TopMost = true;
            pictureBox1.BackColor = Color.Red;
            connectDevice();

        }
        private void transparentStop(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void transparentStart(object sender, EventArgs e)
        {
            this.Opacity = .5;
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
                case (Keys)0x97:
                    flags = false;
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

        BluetoothListener listener;

        private void connectDevice()
        {
            Guid serviceClass;
            Guid UUID = new Guid("00001101-0000-1000-8000-00805f9b34fb");

            serviceClass = BluetoothService.RFCommProtocol;

            listener = new BluetoothListener(UUID)
            {
                ServiceName = "MyService"
            };
            listener.Start();

            Task.Run(() => Listener());

        }

        private void Listener()
        {
            StreamReader streamReader;
            try
            {
                while (true)
                {
                    flags = true;
                    using (var client = listener.AcceptBluetoothClient())
                    {

                        pictureBox1.BackColor = Color.Green;
                        if (label2.InvokeRequired)
                        {
                            label2.Invoke(new MethodInvoker(delegate { label2.Text = client.RemoteMachineName; }));
                        }
                        streamReader = new StreamReader(client.GetStream());
                        while (flags)
                        {
                            try
                            {
                                var buffer = new char[4];
                                var content = streamReader.ReadLine();
                                KeySimulationPC((Keys)int.Parse(content));
                            }
                            catch (IOException)
                            {
                                client.Close();
                                break;
                            }

                        }
                        if (!flags)
                        {
                            pictureBox1.BackColor = Color.Red;
                            if (label2.InvokeRequired)
                            {
                                label2.Invoke(new MethodInvoker(delegate { label2.Text = "none"; }));
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                // todo handle the exception
                // for the sample it will be ignored
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
            PartyPlayer.Dispose();
        }

    }
}
