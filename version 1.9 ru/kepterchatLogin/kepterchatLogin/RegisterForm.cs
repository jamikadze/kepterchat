using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace kepterchatLogin
{
    public partial class RegisterForm : Form
    {
        MySqlConnection conn;

        public string login;
        DataTable dtUser = new DataTable();
        DataSet dsBranch = new DataSet();
        DataSet dsPost = new DataSet();
        DataTable dt = new DataTable();
        DataTable dtPort = new DataTable();
        string txtIP;        
        public bool reg,upd;
        int uid,txtPort;
       
        public RegisterForm(string servname)
        {
            InitializeComponent();
            conn = new MySqlConnection("SERVER= "+servname+";port=3306;DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
            this.AcceptButton = btnOk; 
        }

        private void btnX_Click(object sender, EventArgs e)
        {
           Close();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            RefcbBranch();
            RefcbPost();
            if(upd==true){
            ShowUser();}
            // getting local ip
            string host = Dns.GetHostName();
            IPHostEntry ipE = Dns.GetHostEntry(host);
            IPAddress[] IpA = ipE.AddressList;
            for (int i = 0; i < IpA.Length; i++)
            {
                txtIP = IpA[i].ToString();
            }
            dtPort.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("Select port from users order by idusers desc", conn);
            da.Fill(dtPort);
            if (dtPort.Rows.Count > 0)
            {
                txtPort = (Int32)dtPort.Rows[0]["port"];                
            }
            
        }
        private void ShowUser()
        {
            dtUser.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("Select * from users where login='" + login + "'", conn);
            da.Fill(dtUser);
            string[] namesur = ((string)dtUser.Rows[0]["namesur"]).Split(' ');
            tbName.Text = namesur[0];
            tbSurname.Text = namesur[1];
            tbEmail.Text=(string)dtUser.Rows[0]["mail"];
            tbTel.Text = (string)dtUser.Rows[0]["tel"];
            cbBranch.SelectedValue = (string)dtUser.Rows[0]["branch"];
            cbPost.SelectedValue = (string)dtUser.Rows[0]["position"];
            tbUserName.Text = (string)dtUser.Rows[0]["login"];
            tbPass.Text = (string)dtUser.Rows[0]["password"];
            tbPass2.Text = (string)dtUser.Rows[0]["password"];
            uid = (Int32)dtUser.Rows[0]["idusers"];
        }
        private void RefcbBranch()
        {
            dsBranch.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("Select name from branch", conn);
            da.Fill(dsBranch);
            cbBranch.DataSource = dsBranch.Tables[0];
            cbBranch.DisplayMember = "name";
            cbBranch.ValueMember = "name";
        }
        private void RefcbPost()
        {
            dsPost.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("Select name from position", conn);
            da.Fill(dsPost);
            cbPost.DataSource = dsPost.Tables[0];
            cbPost.DisplayMember = "name";
            cbPost.ValueMember = "name";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();           
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (upd == true)
            {
                if (tbPass.Text == tbPass2.Text)
                {
                    if (conn.State.Equals(ConnectionState.Closed))
                        conn.Open();
                    try
                    {
                        
                        string sSQL = "UPDATE `users`SET  `namesur`=";
                        sSQL += "'" + tbName.Text + " " + tbSurname.Text + "',";
                        sSQL += " `position`='" + cbPost.SelectedValue.ToString() + "',";
                        sSQL += " `branch`='" + cbBranch.SelectedValue.ToString() + "',";
                        sSQL += " `login`='" + tbUserName.Text + "',";
                        sSQL += " `password`='" + tbPass.Text + "',";
                        sSQL += " `tel`='" + tbTel.Text + "',";
                        sSQL += " `mail`='" + tbEmail.Text + "',";
                        sSQL += " `ip`='" + txtIP + "'";
                        sSQL += " WHERE idusers=" + uid;
                        MySqlCommand update = new MySqlCommand(sSQL, conn);

                        int rowAffected = update.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Сохранено!", "Пользователь", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         Close();
                    }
                    catch (Exception ex)
                    {
                        conn.Close(); MessageBox.Show("Ошибка" + ex);
                    }
                }
                else { lblPass.Text = "Пароли не совпадают!"; }
            }
            else
            {
                MySqlDataAdapter da = new MySqlDataAdapter("Select login from users", conn);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (tbUserName.Text == (string)dt.Rows[i]["login"])
                    { lblLogin.Text = "Логин уже существует, выберите другой"; return; }
                }

                if (tbPass.Text == tbPass2.Text)
                {
                    saveNewUser();
                    Close();
                }
                else { lblPass.Text = "Пароли не совпадают!"; }
            }
        }
        private void saveNewUser()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                //MessageBox.Show(txtIP);
                string sSQL = "INSERT INTO `users`(`namesur`,`position`,`branch`,`login`,`password`,`tel`,`mail`,`ip`,`port`) VALUES (";
                sSQL += "'" + tbName.Text + " " + tbSurname.Text + "',";
                sSQL += "'" + cbPost.SelectedValue.ToString() + "',";
                sSQL += "'" + cbBranch.SelectedValue.ToString() + "',";
                sSQL += "'" + tbUserName.Text + "',";
                sSQL += "'" + tbPass.Text + "',";
                sSQL += "'" + tbTel.Text + "',";
                sSQL += " '" + tbEmail.Text + "',";
                sSQL += " '" + txtIP + "',";
                sSQL += " '" + (txtPort+1) + "'";
                sSQL += ");";
                MySqlCommand insert = new MySqlCommand(sSQL, conn);

                int rowAffected = insert.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Сохранено!", "Пользователь", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                conn.Close(); MessageBox.Show("Ошибка" + ex);
            }
            
        }

        private void btnState_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
