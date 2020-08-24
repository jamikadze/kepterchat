using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using kepterchatLogin.Properties;
using System.Net;

namespace kepterchatLogin
{
    public partial class LoginForm : Form
    {
        MySqlConnection conn,conn1;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        string servname,txtIP;
        public LoginForm()
        {
            InitializeComponent();
            tbPassword.Text= Settings.Default["chkPass"].ToString();
            chkPass.Checked = (bool)Settings.Default["chk"];
            tbUserName.Text = Settings.Default["chkName"].ToString();
            conn1 = new MySqlConnection("SERVER= srvc78.trwww.com;DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=1q2w3e4r;CharSet=utf8;");
            this.AcceptButton = btnOk; 
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm regf = new RegisterForm(servname);
            regf.reg = true;            
            Hide();
            regf.ShowDialog();
            Show();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            MySqlDataAdapter da1 = new MySqlDataAdapter("Select servname from serv", conn1);
            da1.Fill(dt1);
            servname = (string)dt1.Rows[0]["servname"];
            conn = new MySqlConnection("SERVER= " + servname + ";port=3306;DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipE = Dns.GetHostEntry(host);
            IPAddress[] IpA = ipE.AddressList;
            for (int i = 0; i < IpA.Length; i++)
            {
                txtIP = IpA[i].ToString();
            }
            if (tbUserName.Text == "Kris")
            {
                if (conn1.State.Equals(ConnectionState.Closed))
                    conn1.Open();
                try
                {
                    string sSQL = "UPDATE  `serv` SET  `servname` =  '"+txtIP+"' WHERE  `idserv` =1;";
                    
                    MySqlCommand update = new MySqlCommand(sSQL, conn1);
                    int rowAffected = update.ExecuteNonQuery();
                    conn1.Close();
                }
                catch (Exception ex)
                {
                    conn1.Close(); MessageBox.Show("Ошибка" + ex);
                }
            }
            try
            {                
                MySqlDataAdapter da = new MySqlDataAdapter("Select idUsers,login,position,ip from users where login='" + tbUserName.Text + "' and password='" + tbPassword.Text + "'", conn);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    if (chkPass.Checked)
                    { Settings.Default["chkPass"] = tbPassword.Text; Settings.Default["chkName"] = tbUserName.Text; Settings.Default["chk"] = chkPass.Checked; Settings.Default.Save(); }
                    else { Settings.Default["chkPass"] = ""; Settings.Default["chkName"] = ""; Settings.Default["chk"] = chkPass.Checked; Settings.Default.Save(); }
                    if (txtIP != dt.Rows[0]["ip"].ToString())
                    {
                        if (conn.State.Equals(ConnectionState.Closed))
                            conn.Open();
                        try
                        {
                            string sSQL = "UPDATE `users`SET `ip`='" + txtIP + "'";
                            sSQL += " WHERE idusers=" + (Int32)dt.Rows[0]["idUsers"];
                            MySqlCommand update = new MySqlCommand(sSQL, conn);

                            int rowAffected = update.ExecuteNonQuery();
                            conn.Close();                           
                        }
                        catch (Exception ex)
                        {
                            conn.Close(); MessageBox.Show("Ошибка" + ex);
                        }
                    }
                        MainForm mf = new MainForm(servname);
                        mf.lblName.Text = dt.Rows[0]["login"].ToString();
                        mf.uid = (Int32)dt.Rows[0]["idUsers"];
                        mf.position = (string)dt.Rows[0]["position"];
                        Hide();
                        mf.ShowDialog();
                        Show();
                        Close();
                    
                }
                else
                {
                    lblError.Text = "Неверное имя пользователя или пароль.";     //Проверьте правильность введенных данных.           
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

      

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

      


    }
}
