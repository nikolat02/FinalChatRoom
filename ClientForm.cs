using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1   //NameSpace ConsoleApplication1 is the ChatRoomClient side of the project
{
    public partial class ClientForm : Form
    {
        private static Stream stm;
        public ClientForm()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread replyThread = new Thread(ReceiveReplies);
            replyThread.Start();
            string str = WriteBox.Text;
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);
            stm.Write(ba, 0, ba.Length); //This writes "the wire"
        }

        private void ReceiveReplies()
        {
            while (true)
            {
                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                setRecieve(System.Text.Encoding.Default.GetString(bb));
                //ReceiveBox.AppendText(System.Text.Encoding.Default.GetString(bb));
            }
        }

        public void setRecieve(string appendText)
        {
            if(InvokeRequired)
            {
                this.Invoke(new Action<string>(setRecieve), new object[] { appendText });
                return;
            }
            ReceiveBox.AppendText(appendText);
            ReceiveBox.AppendText(Environment.NewLine);
            WriteBox.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            textBox1.Enabled = false;
            buttonCreate.Enabled = false;
            User clientUser;
            clientUser = new User(user);
            TcpClient tcpclnt = new TcpClient();

            tcpclnt.Connect("192.168.101.9", 8001); //192.168.101.9
                                                    // use the ipaddress as in the server program

            stm = tcpclnt.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(clientUser.UserName);
            stm.Write(ba, 0, ba.Length);
        }
    }
}
