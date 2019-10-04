using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;

namespace udplocalreceive
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Thread rec = null;
        UdpClient udp = new UdpClient(15000);
        bool stopReceive = false;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long cButtons);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_MBUTTONDBLCLK = 0x0203;
        private const int MOUSEEVENTF_MOUSEWHEEL = 0x020A;

        void Receive()
        {
            try
            {
                while (true)
                {

                    IPEndPoint ipendpoint = null;
                    byte[] message = udp.Receive(ref ipendpoint);
                    ShowMessage(Encoding.Default.GetString(message));

                    // Если дана команда остановить поток, останавливаем бесконечный цикл.
                    if (stopReceive == true) break;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            stopReceive = false;
            rec = new Thread(new ThreadStart(Receive));
            rec.Start();
        }

        delegate void ShowMessageCallback(string message);
        void ShowMessage(string message)
        {

            if (message != "lclick" && message != "rclick" && message != "dclick" && message != "ldown" && message != "lup" && message != "mclick" && message != "wheel")
            {
                String[] x = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Cursor.Position = new Point(Convert.ToInt32(x[0]), Convert.ToInt32(x[1]));
            }

            else if (message == "lclick")
                {
                    label1.Text = "lclick";
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP);
                }
            else if (message == "ldown")
            {
                label1.Text = "ldown";
                mouse_event(MOUSEEVENTF_LEFTDOWN);
            }
            else if (message == "lup")
            {
                label1.Text = "lup";
                mouse_event(MOUSEEVENTF_LEFTUP);
            }
            else if (message == "rclick")
                {
                    label1.Text = "rclick";
                    mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP);
                }
            else if (message == "mclick")
            {
                label1.Text = "mclick";
                mouse_event(MOUSEEVENTF_MBUTTONDBLCLK);
            }
            else if (message == "wheel")
            {
                label1.Text = "wheel";
                mouse_event(MOUSEEVENTF_MOUSEWHEEL);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            udp.Close();
        }
    }
}
    
