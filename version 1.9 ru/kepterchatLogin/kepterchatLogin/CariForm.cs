using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel; 

namespace kepterchatLogin
{
    public partial class CariForm : Form
    {
        MySqlConnection conn;
        DataSet dscari = new DataSet();
        DataSet dsCus = new DataSet();
        DataSet dsCari = new DataSet();

        DataTable dtCari = new DataTable();
        DataTable dtCus = new DataTable();
           DataTable dt = new DataTable();

        DataTable dtp = new DataTable();
        MySqlDataAdapter dasum;
        DataTable dtsum = new DataTable();

        string servname;

        
        public CariForm(string servname)
        {
            InitializeComponent();
            cbTip.Items.Add("Дебитор / Borçlu");
            cbTip.Items.Add("Кредитор / Alacaklı");
            conn = new MySqlConnection("SERVER= "+servname+";port=3306;DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
            this.AcceptButton = btnOK; 
            this.servname=servname;
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CariForm_Load(object sender, EventArgs e)
        {
            dtpCariEnd.Value = DateTime.Now.AddDays(1);
            cbTip.SelectedIndex = 0;
            cbFilter.SelectedIndex = 0;
            RefcbCustomer();
            RefCari();
            this.dgvCari.AllowUserToAddRows = false;
            showDGV();
        }
        private void RefcbCustomer()
        {
            dsCus.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT  customerscode as name FROM customers", conn);
            da.Fill(dsCus);
            MySqlDataAdapter dap = new MySqlDataAdapter("SELECT  namesur as name FROM users", conn);
            dap.Fill(dsCus);
            if (dsCus.Tables[0].Rows.Count > 0)
            {
                cbCustomers.DataSource = dsCus.Tables[0];
                cbCustomers.DisplayMember = "name";
                cbCustomers.ValueMember = "name";               
            }           
        }
        private void RefCari()
        {
            dsCari.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT  customerscode as name FROM customers", conn);
            da.Fill(dsCari);
            MySqlDataAdapter dap = new MySqlDataAdapter("SELECT  namesur as name FROM users", conn);
            dap.Fill(dsCari);
            if (dsCari.Tables[0].Rows.Count > 0)
            {
                cbCariFilter.DataSource = dsCari.Tables[0];
                cbCariFilter.DisplayMember = "name";
                cbCariFilter.ValueMember = "name";
            }
        }
        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            CustomerForm cusf = new CustomerForm(servname);
            Hide();
            cusf.ShowDialog();
            RefcbCustomer();
            RefCari();
            Show();
        }

        private void tbSum_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch == 44 && tbSum.Text.IndexOf(",") != -1)
            {
                e.Handled = true;
                return;
            }
            if (!Char.IsDigit(ch) && ch != 8 && ch != 44)
            {
                e.Handled = true;
                return;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            saveNewRow();
        }
        private void saveNewRow()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                if (cbTip.SelectedIndex == 0)
                {
                    Decimal a = Decimal.Parse(tbSum.Text);
                    string sSQL = "INSERT INTO `cari` (`operation`, `cariname`, `operationname`, `debit`,`kredit`, `description`, `tarih`) VALUES (";
                    sSQL += "N'" + cbTip.SelectedItem + "',";
                    sSQL += "N'" + cbCustomers.SelectedValue + "',";
                    sSQL += "N'" + tbName.Text + "',";
                    sSQL += "" + (a.ToString()).Replace(",", ".") + ",";
                    sSQL += " 0 ,";                    
                    sSQL += "N'" + tbDescription.Text + "',";
                    sSQL += "'" +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";                    
                    sSQL += ");";
                    MySqlCommand insert = new MySqlCommand(sSQL, conn);

                    int rowAffected = insert.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Сохранено!", "Касса", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showDGV();
                }
                else if(cbTip.SelectedIndex==1)
                {
                    Decimal a = Decimal.Parse(tbSum.Text);
                    string sSQL = "INSERT INTO `cari` (`operation`, `cariname`, `operationname`, `debit`,`kredit`, `description`, `tarih`) VALUES (";
                    sSQL += "N'" + cbTip.SelectedItem + "',";
                    sSQL += "N'" + cbCustomers.SelectedValue + "',";
                    sSQL += "N'" + tbName.Text + "',";
                    sSQL += " 0 ,";
                    sSQL += "" + (a.ToString()).Replace(",", ".") + ",";                    
                    sSQL += "N'" + tbDescription.Text + "',";
                    sSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    sSQL += ");";
                    MySqlCommand insert = new MySqlCommand(sSQL, conn);
                    int rowAffected = insert.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Сохранено!", "Касса", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showDGV();
                }
            }
            catch (Exception ex)
            { conn.Close(); MessageBox.Show("Ошибка" + ex); }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            updateBD();
        }

        private void updateBD()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                if (cbTip.SelectedIndex == 0)
                {
                    Decimal a = Decimal.Parse(tbSum.Text);                   
                    string sSQL = "Update `cari` set `operation`='" + cbTip.SelectedValue + "',";
                    sSQL += "`debit`=" + (a.ToString()).Replace(",", ".") + ",";
                    sSQL += "`kredit`=0,";
                    sSQL += "`cariname`='" + cbCustomers.SelectedValue + "',";
                    sSQL += "`tarih`='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                    sSQL += "`description`=N'" + tbDescription.Text + "'";
                    sSQL += " where `idcari`=" + dgvCari.CurrentRow.Cells[0].Value.ToString();
                    MySqlCommand upd = new MySqlCommand(sSQL, conn);

                    int rowAffected = upd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Сохранено!", "Расчетный счет", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showDGV();
                }
                else
                {
                    Decimal a = Decimal.Parse(tbSum.Text);
                    string sSQL = "Update `cari` set `operation`=";
                    sSQL += "N'" + cbTip.SelectedItem + "',";
                    sSQL += "`kredit`=" + (a.ToString()).Replace(",", ".") + ",";
                    sSQL += "`debit`=0,";
                    sSQL += "`cariname`='" + cbCustomers.SelectedValue + "',";
                    sSQL += "`tarih`='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                    sSQL += "`description`=N'" + tbDescription.Text + "'";
                    sSQL += " where `idcari`=" + dgvCari.CurrentRow.Cells[0].Value.ToString();
                    MySqlCommand upd = new MySqlCommand(sSQL, conn);

                    int rowAffected = upd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Сохранено!", "Расчетный счет", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showDGV();
                }
            }
            catch (Exception ex)
            { conn.Close(); MessageBox.Show("Ошибка" + ex); }
        }

        private void showDGV()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                
                if (cbFilter.SelectedIndex == 0)
                {   dt.Clear();                 
                    MySqlDataAdapter dag = new MySqlDataAdapter("Select idCari as '№',operation as 'Операция / Ünvan',operationname as 'Наименование',cariname as 'Расчетный счет / Cari',debit as 'Дебитор / Borçlu',kredit as 'Кредитор / Alacaklı',description as 'Описание',tarih from `cari` where (tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "');", conn);
                    dag.Fill(dt);
                    dtsum.Clear();
                    dasum = new MySqlDataAdapter("SELECT SUM(debit)-SUM(kredit) AS Sum FROM cari where (tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "');", conn);
                    dasum.Fill(dtsum);
                    if (dtsum.Rows.Count > 0)
                    { label6.Text = "Сумма: " + dtsum.Rows[0]["Sum"]; }                    
                   
                }
               else if (cbFilter.SelectedIndex == 1)
                {
                    dt.Clear();
                    MySqlDataAdapter dag = new MySqlDataAdapter("Select idCari as '№',operation as 'Операция / Ünvan',operationname as 'Наименование',cariname as 'Расчетный счет / Cari',debit as 'Дебитор / Borçlu',kredit as 'Кредитор / Alacaklı',description as 'Описание',tarih from `cari` where (tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "' and operation='" + cbFilter.SelectedItem + "')", conn);
                    dag.Fill(dt);
                    dtsum.Clear();
                    dasum = new MySqlDataAdapter("SELECT SUM(debit) AS Sum FROM cari where (tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "' and operation='" + cbFilter.SelectedItem + "');", conn);
                    dasum.Fill(dtsum);
                    if (dtsum.Rows.Count > 0)
                    { label6.Text = "Сумма: " + dtsum.Rows[0]["Sum"]; }                    
                }
                else if (cbFilter.SelectedIndex == 2)
                {
                    dt.Clear();
                    MySqlDataAdapter dag = new MySqlDataAdapter("Select idCari as '№',operation as 'Операция / Ünvan',operationname as 'Наименование',cariname as 'Расчетный счет / Cari',debit as 'Дебитор / Borçlu',kredit as 'Кредитор / Alacaklı',description,tarih from `cari` where (tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "' and operation='" + cbFilter.SelectedItem + "')", conn);
                    dag.Fill(dt);
                    dtsum.Clear();
                    dasum = new MySqlDataAdapter("SELECT SUM(kredit) AS Sum FROM cari where (tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "'and operation='" + cbFilter.SelectedItem + "');", conn);
                    dasum.Fill(dtsum);
                    if (dtsum.Rows.Count > 0)
                    { label6.Text = "Сумма: " + dtsum.Rows[0]["Sum"]; }                    
                    }
                if (dt.Rows.Count>0 )
                    {
                        dgvCari.DataSource = dt;
                        dgvCari.BorderStyle = BorderStyle.Fixed3D;
                        dgvCari.Columns[0].Width = 30;
                        dgvCari.Columns[1].Width = 110;
                        dgvCari.Columns[2].Width = 120;
                        dgvCari.Columns[3].Width = 125;
                        dgvCari.Columns[4].Width = 100;
                        dgvCari.Columns[5].Width = 115;
                        dgvCari.Columns[6].Visible = false;
                        dgvCari.Columns[7].Visible = false;
                        foreach (DataGridViewColumn column in dgvCari.Columns)
                        {
                            column.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }               
                   ReadDgv();                
                conn.Close();                
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка!" + ex); }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            showDGV();
        }
      private void ReadDgv()
        {

            if (dt.Rows.Count > 0)
            {
                if (dgvCari.CurrentRow.Cells[1].Value.ToString() == "Дебитор / Borçlu")
                { cbTip.SelectedIndex = 0;
                tbSum.Text = dgvCari.CurrentRow.Cells[4].Value.ToString();
                }
                else if ((dgvCari.CurrentRow.Cells[1].Value.ToString() == "Кредитор / Alacaklı"))
                { cbTip.SelectedIndex = 1;tbSum.Text = dgvCari.CurrentRow.Cells[5].Value.ToString();}
                cbCustomers.SelectedValue = dgvCari.CurrentRow.Cells[3].Value.ToString();
                tbName.Text =  dgvCari.CurrentRow.Cells[2].Value.ToString();
                tbDescription.Text = dgvCari.CurrentRow.Cells[6].Value.ToString();
                label3.Text = "Дата: " + dgvCari.CurrentRow.Cells[7].Value.ToString();
            }
            else
            {
                tbSum.Text = "";                
                tbName.Text = "";
                tbDescription.Text = "";
            }
        }

      private void dgvCari_SelectionChanged(object sender, EventArgs e)
      {
          ReadDgv();
      }

      private void dtpCariBegin_ValueChanged(object sender, EventArgs e)
      {
          showDGV();
      }

      private void btnSearch_Click(object sender, EventArgs e)
      {
          search();
      }
      private void search()
      {
          if (conn.State.Equals(ConnectionState.Closed))
              conn.Open();
          try
          {
              if (cbFilter.SelectedIndex == 0)
              {
                  dt.Clear();
                  MySqlDataAdapter dag = new MySqlDataAdapter("Select idCari as '№',operation as 'Операция / Ünvan',operationname as 'Наименование',cariname as 'Расчетный счет / Cari',debit as 'Дебитор / Borçlu',kredit as 'Кредитор / Alacaklı',description as 'Описание',tarih from `cari` where cariname='" + cbCariFilter.SelectedValue + "' and tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "';", conn);
                  dag.Fill(dt);
                  dtsum.Clear();
                  dasum = new MySqlDataAdapter("SELECT SUM(debit)-SUM(kredit) AS Sum FROM cari where cariname='" + cbCariFilter.SelectedValue + "' and tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "';", conn);
                  dasum.Fill(dtsum);
                  if (dtsum.Rows.Count > 0)
                  { label6.Text = "Сумма: " + dtsum.Rows[0]["Sum"]; }

              }
              else if (cbFilter.SelectedIndex == 1)
              {
                  dt.Clear();
                  MySqlDataAdapter dag = new MySqlDataAdapter("Select idCari as '№',operation as 'Операция / Ünvan',operationname as 'Наименование',cariname as 'Расчетный счет / Cari',debit as 'Дебитор / Borçlu',kredit as 'Кредитор / Alacaklı',description as 'Описание',tarih from `cari` where cariname='" + cbCariFilter.SelectedValue + "' and tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "' and operation='" + cbFilter.SelectedItem + "'", conn);
                  dag.Fill(dt);
                  dtsum.Clear();
                  dasum = new MySqlDataAdapter("SELECT SUM(debit) AS Sum FROM cari where cariname='" + cbCariFilter.SelectedValue + "' and tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "' and operation='" + cbFilter.SelectedItem + "';", conn);
                  dasum.Fill(dtsum);
                  if (dtsum.Rows.Count > 0)
                  { label6.Text = "Сумма: " + dtsum.Rows[0]["Sum"]; }
              }
              else if (cbFilter.SelectedIndex == 2)
              {
                  dt.Clear();
                  MySqlDataAdapter dag = new MySqlDataAdapter("Select idCari as '№',operation as 'Операция / Ünvan',operationname as 'Наименование',cariname as 'Расчетный счет / Cari',debit as 'Дебитор / Borçlu',kredit as 'Кредитор / Alacaklı',description,tarih from `cari` where cariname='" + cbCariFilter.SelectedValue + "' and tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "' and operation='" + cbFilter.SelectedItem + "'", conn);
                  dag.Fill(dt);
                  dtsum.Clear();
                  dasum = new MySqlDataAdapter("SELECT SUM(kredit) AS Sum FROM cari where cariname='" + cbCariFilter.SelectedValue + "' and tarih between '" + dtpCariBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpCariEnd.Value.ToString("yyyy-MM-dd") + "'and operation='" + cbFilter.SelectedItem + "';", conn);
                  dasum.Fill(dtsum);
                  if (dtsum.Rows.Count > 0)
                  { label6.Text = "Сумма: " + dtsum.Rows[0]["Sum"]; }
              }
              if (dt.Rows.Count > 0)
              {

                  dgvCari.DataSource = dt;
                  dgvCari.BorderStyle = BorderStyle.Fixed3D;
                  dgvCari.Columns[0].Width = 30;
                  dgvCari.Columns[1].Width = 110;
                  dgvCari.Columns[2].Width = 120;
                  dgvCari.Columns[3].Width = 125;
                  dgvCari.Columns[4].Width = 100;
                  dgvCari.Columns[5].Width = 115;
                  dgvCari.Columns[6].Visible = false;
                  dgvCari.Columns[7].Visible = false;
                  foreach (DataGridViewColumn column in dgvCari.Columns)
                  {
                      column.SortMode = DataGridViewColumnSortMode.NotSortable;
                  }
              }
              ReadDgv();
              conn.Close();


          }
          catch (Exception ex)
          { MessageBox.Show("Ошибка!" + ex); }
      }
      private void btnRaport_Click(object sender, EventArgs e)
      {
          if (conn.State.Equals(ConnectionState.Closed))
              conn.Open();
          try
          {


              if (dscari.Tables[0].Rows.Count <= 0)
              { MessageBox.Show("В базе нету записей!"); return; }

              Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

              if (xlApp == null)
              {
                  MessageBox.Show("Excel не установлено!!!");
                  return;
              }


              Excel.Workbook xlWorkBook;
              Excel.Worksheet xlWorkSheet;
              object misValue = System.Reflection.Missing.Value;

              xlWorkBook = xlApp.Workbooks.Add(misValue);
              xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
              int i = 0;
              int j = 0;

              xlWorkSheet.Cells[1, 1] = "№";
              xlWorkSheet.Cells[1, 2] = "Тип";
              xlWorkSheet.Cells[1, 3] = "Наименование";
              xlWorkSheet.Cells[1, 4] = "Расчетный счет";
              xlWorkSheet.Cells[1, 5] = "Дебит(Должен)";
              xlWorkSheet.Cells[1, 6] = "Кредит(Мы должны)";              
              xlWorkSheet.Cells[1, 7] = "Описание";
              xlWorkSheet.Cells[1, 8] = "Дата";
              for (i = 1; i <= dscari.Tables[0].Rows.Count; i++)
              {
                  for (j = 0; j <= dscari.Tables[0].Columns.Count - 1; j++)
                  {
                      xlWorkSheet.Cells[i + 1, j + 1] = dscari.Tables[0].Rows[i - 1][j];
                  }
              }
              xlWorkSheet.Cells[dscari.Tables[0].Rows.Count + 2, 7].Font.Bold = true;
              xlWorkSheet.Cells[dscari.Tables[0].Rows.Count + 2, 7] = label6.Text.Replace(",", ".");

              xlWorkSheet.Columns.AutoFit();
              xlWorkBook.SaveAs("d:\\Cari_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
              xlWorkBook.Close(true, misValue, misValue);
              xlApp.Quit();

              releaseObject(xlWorkSheet);
              releaseObject(xlWorkBook);
              releaseObject(xlApp);

              MessageBox.Show("Excel файл создан!");

              FileInfo fi = new FileInfo("d:\\Cari_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
              if (fi.Exists)
              {
                  System.Diagnostics.Process.Start("d:\\Cari_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
              }
              else
              {
                  //file doesn't exist
              }
          }
          catch (Exception ex)
          {
              conn.Close(); MessageBox.Show("Ошибка" + ex);
          }
      }
      private void releaseObject(object obj)
      {
          try
          {
              System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
              obj = null;
          }
          catch (Exception ex)
          {
              obj = null;
              MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
          }
          finally
          {
              GC.Collect();
          }
      }

      private void btnState_Click(object sender, EventArgs e)
      {
          this.WindowState = FormWindowState.Minimized;
      }

      private void btnClear_Click(object sender, EventArgs e)
      {
          tbDescription.Text = "";
          tbName.Text = "";
          tbSum.Text = "";
      }
    }
}
