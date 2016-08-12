using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UdpTestWinForm
{
    public partial class Form1 : Form
    {
        private IPAddress addr = null;
        private int port = -1;
        private int exitPort = -1;
        private UdpClient client = null;
        private UdpClient exitClient = null;
        private int i = 0;

        // hi this is a comment
        // this is antoher comment
        public Form1()
        {
            InitializeComponent();
        }

        private void ConfigureClients()
        {
            client = new UdpClient(port);
            exitClient = new UdpClient(exitPort);

            client.Connect(addr, port);
            exitClient.Connect(addr, exitPort);
        }

        private void CloseClients()
        {
            if (client != null)
            {
                client.Close();
            }
            if (exitClient != null)
            {
                exitClient.Close();
            }
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            if ((addr == null) || (port == -1) || (exitPort == -1))
            {
                if (!(IPAddress.TryParse(tb_IP.Text, out addr)))
                {
                    tb_IP.Text = "Try Again";
                    return;
                }

                if (!(int.TryParse(tb_Port.Text, out port)))
                {
                    tb_Port.Text = "Try Again.";
                    return;
                }

                if (!(int.TryParse(tb_ExitPort.Text, out exitPort)))
                {
                    tb_ExitPort.Text = "Try Again.";
                    return;
                }

                ConfigureClients();
            }

            if (string.IsNullOrEmpty(tb_Message.Text))
                return;

            try
            {

                if (tb_Message.Text == "exit")
                {
                    byte[] toSend = Encoding.UTF8.GetBytes("exit");
                    exitClient.Send(toSend, toSend.Length);
                }
                else if (tb_Message.Text == "data")
                {
                    timer1.Enabled = true;
                    timer1.Start();
                }
                else
                {
                    byte[] toSend = Encoding.UTF8.GetBytes(tb_Message.Text);
                    client.Send(toSend, toSend.Length);
                }
            }
            catch
            {

            }
            finally
            {
                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string str = i.ToString();

            byte[] bs = Encoding.UTF8.GetBytes(str);

            client.Send(bs, bs.Length);

            if (i == 100)
                i = 0;

            i++;
        }

        private void btn_StopTimer_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb_Message.Text = "lol";
            tb_ExitPort.Text = "7979";
            tb_Port.Text = "6969";
            tb_IP.Text = "192.168.1.227";

            timer1.Interval = 100;
        }
    }
}
