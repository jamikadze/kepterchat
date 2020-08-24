using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kepterchatLogin
{
    public partial class DeleteUserForm : Form
    {
        MySqlConnection conn;
        DataSet dsUser = new DataSet();
        DataTable dtCheck = new DataTable();
        public DeleteUserForm(string servname)
        {
            InitializeComponent();
            conn = new MySqlConnection("SERVER= "+servname+";DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DeleteUserForm_Load(object sender, EventArgs e)
        {
            RefcbUser();
        }
        private void RefcbUser()
        {
            dsUser.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("Select namesur as name from users ", conn);
            da.Fill(dsUser);
            cbUsers.DataSource = dsUser.Tables[0];
            cbUsers.DisplayMember = "name";
            cbUsers.ValueMember = "name";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();

            try
            {
                dtCheck.Clear();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT Orders.idorders FROM Orders INNER JOIN users ON Orders.user=users.idusers and Users.namesur='" + cbUsers.SelectedValue + "';", conn);
                da.Fill(dtCheck);
                if (dtCheck.Rows.Count == 0)
                {
                    string sSQL = "delete  FROM users where namesur='" + cbUsers.SelectedValue + "';";

                    MySqlCommand delete = new MySqlCommand(sSQL, conn);

                    int rowAffected = delete.ExecuteNonQuery();
                    conn.Close(); RefcbUser();
                    MessageBox.Show("Удалено!", "Пользователь", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else { MessageBox.Show("Сначала удалите заказы менеджера!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            catch (Exception ex)
            {
                conn.Close(); MessageBox.Show("Ошибка" + ex);
            }
        }

        
            
        }
    }

