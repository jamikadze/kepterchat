using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace kepterchatLogin
{
    public partial class CustomerForm : Form
    {
        MySqlConnection conn;
        DataTable dtCode = new DataTable();

        public CustomerForm(string servname)
        {
            InitializeComponent();
            conn = new MySqlConnection("SERVER= "+servname+";DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
            this.AcceptButton = btnOk; 
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            saveNewUser();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void saveNewUser()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {

                string sSQL = "INSERT INTO `customers`(`name`,`surname`,`tel`,`mail`,`customerscode`,`companyname`, `address`, `tel2`, `bank`, `vergid`, `vergin`, `extra`)VALUES(";
                sSQL += "'" + tbName.Text + "',";
                sSQL += "'" + tbSurname.Text + "',";
                sSQL += "'" + tbTel.Text + "',";
                sSQL += " '" + tbEmail.Text + "',";
                sSQL += " '" + tbCode.Text + "',";
                sSQL += "'" + tbCompanyName.Text+ "',";
                sSQL += "'" + tbAddress.Text + "',";
                sSQL += "'" + tbTel2.Text + "',";
                sSQL += " '" + tbBank.Text + "',";
                sSQL += " '" +tbVergid.Text + "',";
                sSQL += " '" + tbVergin.Text + "',";
                sSQL += " '" + tbExtra.Text + "'";
                sSQL += ");";
                MySqlCommand insert = new MySqlCommand(sSQL, conn);

                int rowAffected = insert.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Сохранено!", "Контакты", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                conn.Close(); MessageBox.Show("Ошибка" + ex);
            }

        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            CustomerCode();
        }
        private void CustomerCode()
        {
            dtCode.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("Select `customerscode` from customers", conn);
            da.Fill(dtCode);
            string var;
            int j = dtCode.Rows.Count;
            if (j == 0) { tbCode.Text = "ZCode - " + (j).ToString("00#"); return; }
            
            for (int i = 0; i < dtCode.Rows.Count; i++) {
                if (Convert.ToInt32(((string)dtCode.Rows[i]["customerscode"]).Substring(((string)dtCode.Rows[i]["customerscode"]).Length - 3, 3)) >= j )
                {
                    var = ((string)dtCode.Rows[i]["customerscode"]).Substring(((string)dtCode.Rows[i]["customerscode"]).Length - 3, 3);
                    j = Convert.ToInt32(var);                    
                    tbCode.Text = "ZCode - " + (j+1).ToString("00#");  }
                else tbCode.Text =  "ZCode - " + (j).ToString("00#");
            }
            
            
            
            
        }
    }
}
