using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;



namespace udplocalsend
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        UdpClient udp = new UdpClient();
        IPAddress ipaddress;
        IPEndPoint ipendpoint;
        Thread track;
        string s;
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            track = new Thread(position);
            track.Start();
            try
            {
                ipaddress = IPAddress.Parse(textBoxAddress.Text);
                ipendpoint = new IPEndPoint(ipaddress, 15000);
            }
            catch
            {
                MessageBox.Show("Указан недопустимый адрес IP");
            }
            
        }

        private void position()
        {
            try
            {
                while (true)
                {
                    s = MousePosition.X.ToString() + " " + MousePosition.Y.ToString();
                    Action action = () => label1.Text = MousePosition.X.ToString() + " " + MousePosition.Y.ToString();
                    MouseClick += new MouseEventHandler(mouseClickedResponse);
                    MouseDoubleClick += new MouseEventHandler(mouseDoubleClick);
                    MouseDown += new MouseEventHandler(mouseDown);
                    MouseUp += new MouseEventHandler(mouseUp);
                    MouseWheel += new MouseEventHandler(mouseWheel);
                    if (InvokeRequired)
                        Invoke(action);
                    else
                        action();

                    byte[] message = Encoding.Default.GetBytes(s);
                    udp.Send(message, message.Length, ipendpoint);
                }
            }
            catch
            {
                MessageBox.Show("Указан недопустимый адрес IP");
            }
        }
        private void mouseWheel(object sender, MouseEventArgs e)
        {
                
                s = "wheel";
                label2.Text = s;
            
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                s = "ldown";
                label2.Text = s;
            }
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                s = "lup";
                label2.Text = s;
            }
        }

        private void mouseDoubleClick(object sender, MouseEventArgs e)
        {
            
                s = "dclick";
                label2.Text = s;
            
        }

        private void mouseClickedResponse(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                s = "lclick";
                label2.Text = s;
            }
            else if (e.Button == MouseButtons.Right)
            {
                s = "rclick";
                label2.Text = s;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                s = "mclick";
                label2.Text = s;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            udp.Close();
            if (track != null) { track.Abort(); }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Process[] processList = Process.GetProcessesByName("udplocalreceive");
            if (processList.Length > 0)
            {
                //
            }
            else
            {
                MessageBox.Show("Ошибка соединения. Программа не запущена на удаленном ПК.");
            }
        }
    }
}
                                              