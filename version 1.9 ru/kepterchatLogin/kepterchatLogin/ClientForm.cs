using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net.Sockets;
using System.IO;
using System.Threading;
using MySql.Data.MySqlClient;

namespace kepterchatLogin
{
    public partial class ClientForm : Form
    {
        TcpClient tcpClient;
        bool isConnectedToServer = false;
        private StreamWriter strWritter;
        private StreamReader strReader;
        private Thread incomingMessageHandler;

        string ipAddress, port;
        public string UserName;
        DataTable dt = new DataTable();
        MySqlConnection conn;
        public ClientForm(string ipAddress, string port)
        {
            InitializeComponent();
            conn = new MySqlConnection("SERVER= " + ipAddress + ";port=3306;DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
            this.ipAddress = ipAddress;
            this.port = port;
        }
        void getResponse()
        {
            Stream stm = tcpClient.GetStream();
            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);

            string msg = "";

            for (int i = 0; i < k; i++)
            {
                msg += Convert.ToChar(bb[i]).ToString();

            }

            setClientMessage(msg);
        }
        void setClientMessage(string msg)
        {

            if (!InvokeRequired)
            {
                listBox1.Items.Add(msg.ToString());

            }
            else
            {
                Invoke(new Action<string>(setClientMessage), msg);
            }
        }

        private void SendMsgButton_Click(object sender, EventArgs e)
        {
            
            string message;

            message = "SEND_MSG;" + totextbox.Text + ";" + messagebodytextbox.Text;

            strWritter.WriteLine(message);
            strWritter.Flush();
            listBox1.Items.Add("Я: "+messagebodytextbox.Text);
            messagebodytextbox.Clear();
        }

        private void ConnectToServ()
        {
            if (!isConnectedToServer)
            {
                string connectionEstablish;

                connectionEstablish = "CONNECT_REQUEST;" + UserName;


                tcpClient = new TcpClient();
                tcpClient.Connect(ipAddress, int.Parse(port));
                this.isConnectedToServer = true;

                strWritter = new StreamWriter(tcpClient.GetStream());
                strWritter.WriteLine(connectionEstablish);
                strWritter.Flush();

                incomingMessageHandler = new Thread(() => ReceiveMessages());
                incomingMessageHandler.IsBackground = true;
                incomingMessageHandler.Start();


            }
            if (isConnectedToServer)
            {

            }
        }
        private void ReceiveMessages()
        {


            strReader = new StreamReader(this.tcpClient.GetStream());

            // While we are successfully connected, read incoming lines from the server
            while (this.isConnectedToServer)
            {
                string serverResponse = strReader.ReadLine();
                string[] data = serverResponse.Split(';');

                if (data[0].Equals("INCOMING_MSG"))
                {
                    string source = data[1];
                    string message = data[2];
                    setClientMessage(source + " :" + " " + message);
                }

            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            LbUsers();
            lblName.Text = UserName;
        }
        private void LbUsers()
        {
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT login as name FROM  users where login not like 'Kris' and login not like '"+UserName+"'", conn);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                //listView1.Items.Clear();
                lbUsers.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    lbUsers.Items.Add(dt.Rows[i]["name"].ToString());

                    //listView1.Items.Add(dt);
                    //  else lvOrders.Items.Add(dtOrd.Rows[i]["name"].ToString());
                }
                lbUsers.SelectedIndex = 0;
            }

            ConnectToServ();
        }

        private void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            totextbox.Text = lbUsers.SelectedItem.ToString();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnState_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
