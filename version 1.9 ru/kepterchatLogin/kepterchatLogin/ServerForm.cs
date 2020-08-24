using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Net.Sockets;

namespace kepterchatLogin
{
    public partial class ServerForm : Form
    {
        Server server;
        string ipAddress, port;
        public ServerForm(string ipAddress,string port)
        {
            InitializeComponent();

            label3.Text = "Server is Not Running";

            this.ipAddress = ipAddress;
            this.port = port;
        }
        void server_OnclientConnected(object sender, EventArgs args)
        {
            User usr = (User)sender;

            listBox1.Items.Add(usr.UserName + " Joined to The Server");



        }

        private void startListnerButton_Click(object sender, EventArgs e)
        {
            server = new Server(ipAddress,int.Parse(port));
            server.startListener();
            server.OnclientConnected += new Server.PropertyChangeHandler(server_OnclientConnected);

            label3.Text = "Server is Up and Running";
        }

        private void stopListnerButton_Click(object sender, EventArgs e)
        {

        }
    }
}
