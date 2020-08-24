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
    public partial class CaseForm : Form
    {
        MySqlConnection conn;
        DataSet ds=new DataSet();
        DataSet dsrap=new DataSet();
        DataTable dt = new DataTable();
        DataTable dtpsum = new DataTable();
        DataTable dtrsum = new DataTable();
        DataTable dtresult= new DataTable();

        BindingSource bs1 = new BindingSource();
        public CaseForm(string servname)
        {
            InitializeComponent();
            conn = new MySqlConnection("SERVER= "+servname+";DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
            this.AcceptButton = btnOK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbSum.Text = "";            
            tbName.Text = "";
            tbDescription.Text = "";
        }
        private void RefreshBalance()
        {
            dt.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT SUM( gelirsum ) - SUM( gidersum ) as balance FROM  `case`", conn);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            { tbBalance.Text = dt.Rows[0]["balance"].ToString();
            }
        }
        private void CaseForm_Load(object sender, EventArgs e)
        {
            dtpEnd.Value = DateTime.Now.AddDays(1);
            cbgd.SelectedIndex = 0;
            showDGV();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            saveNewCase();
            
        }
        private void saveNewCase()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                if (cbgd.SelectedIndex == 0)
                {
                    Decimal a = Decimal.Parse(tbSum.Text);
                    Decimal b = Decimal.Parse(tbBalance.Text);
                    string sSQL = "INSERT INTO `case`(`casename`,`gelirname`,`gelirsum`,`balance`,`takvim`,`description`)VALUES(";
                    sSQL += "N'" + tbName.Text + "',";
                    sSQL += "N'" + tbName.Text + "',";
                    sSQL += "" + (a.ToString()).Replace(",", ".") + ",";                   
                    sSQL += "" + (b.ToString()).Replace(",", ".") + ",";
                    sSQL += "'" + dtp.Value.ToString("yyyy-MM-dd") + "',";
                    sSQL += "N'" + tbDescription.Text + "'";                    
                    sSQL += ");";
                    MySqlCommand insert = new MySqlCommand(sSQL, conn);

                    int rowAffected = insert.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Сохранено!", "Касса", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showDGV();
                }
                else
                {
                    Decimal a = Decimal.Parse(tbSum.Text);
                    Decimal b = Decimal.Parse(tbBalance.Text);
                    string sSQL = "INSERT INTO `case`(`casename`,`gidername`,`gidersum`,`balance`,`takvim`,`description`)VALUES(";
                    sSQL += "N'" + tbName.Text + "',"; 
                    sSQL += "N'" + tbName.Text + "',";
                    sSQL += "" + (a.ToString()).Replace(",", ".") + ",";
                    sSQL += "" + (b.ToString()).Replace(",", ".") + ",";
                    sSQL += "'" + dtp.Value.ToString("yyyy-MM-dd") + "',";
                    sSQL += "N'" + tbDescription.Text + "'";
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
        private void showDGV()
        {
            
            try
            {
                ds.Clear();
                MySqlDataAdapter dag = new MySqlDataAdapter("Select idCase as '№',casename as 'Наименование',gelirsum as 'Приход',gidersum as 'Расход',description as 'Описание',gelirname,gidername,takvim from `case` where takvim between '" + dtpBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpEnd.Value.ToString("yyyy-MM-dd") + "';", conn);
                dag.Fill(ds, "case");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bs1.DataSource = ds.Tables[0];
                    dgvCase.DataSource = bs1;
                    dgvCase.BorderStyle = BorderStyle.Fixed3D;
                    dgvCase.Columns[5].Visible = false;
                    dgvCase.Columns[6].Visible = false;
                    dgvCase.Columns[7].Visible = false;

                    dgvCase.Columns[0].Width = 35;
                    dgvCase.Columns[1].Width = 138;
                    dgvCase.Columns[2].Width = 110;
                    dgvCase.Columns[3].Width = 110;                    
                    dgvCase.Columns[4].Width = 185;
                    foreach (DataGridViewColumn column in dgvCase.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                   dtpsum.Clear();
                   MySqlDataAdapter dapsum = new MySqlDataAdapter("SELECT SUM(gelirsum) AS prihod FROM `case`;", conn);
                    dapsum.Fill(dtpsum);
                    if (dtpsum.Rows.Count > 0)
                    { label6.Text = dtpsum.Rows[0]["prihod"].ToString(); }

                    dtrsum.Clear();
                    MySqlDataAdapter darsum = new MySqlDataAdapter("SELECT SUM(gidersum) AS rashod FROM `case`;", conn);
                    darsum.Fill(dtrsum);
                    if (dtrsum.Rows.Count > 0)
                    { label7.Text = dtrsum.Rows[0]["rashod"].ToString(); }

                    dtresult.Clear();
                    MySqlDataAdapter daresult = new MySqlDataAdapter("SELECT SUM(gelirsum)-SUM(gidersum) AS itogo FROM `case` where takvim = '"+dtp.Value.ToString("yyyy-MM-dd HH:mm:ss")+"';", conn);
                    daresult.Fill(dtresult);
                    if (dtresult.Rows.Count > 0)
                    { label9.Text = dtresult.Rows[0]["itogo"].ToString(); }

                    RefreshBalance();
                    ReadDgv();
                    
                   
                }
                else { 
                   // savebd = true;
                }
                conn.Close();

            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка!" + ex); }
        }
        private void ReadDgv()
        {

            if (ds.Tables["case"].Rows.Count > 0)
            {
                if(dgvCase.CurrentRow.Cells[5].Value.ToString()!="")
                {tbName.Text=dgvCase.CurrentRow.Cells[5].Value.ToString();
                cbgd.SelectedIndex=0;
                    tbSum.Text=dgvCase.CurrentRow.Cells[2].Value.ToString();
                }
                else{ tbName.Text=dgvCase.CurrentRow.Cells[6].Value.ToString();cbgd.SelectedIndex=1;
                tbSum.Text = dgvCase.CurrentRow.Cells[3].Value.ToString();
                }
                tbDescription.Text=dgvCase.CurrentRow.Cells[4].Value.ToString();
               
                DateTime takvim = DateTime.Parse(dgvCase.CurrentRow.Cells[7].Value.ToString());
                dtp.Value = DateTime.Parse(takvim.ToShortDateString());               
            }
            else
            {
                tbSum.Text = "";
                tbBalance.Text = "";
                tbName.Text = "";
                tbDescription.Text = "";
            }
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
                if (cbgd.SelectedIndex == 0)
                {
                    Decimal a = Decimal.Parse(tbSum.Text);
                    Decimal b = Decimal.Parse(tbBalance.Text);
                    string sSQL = "Update `case` set `casename`=";
                    sSQL += "N'" + tbName.Text + "',";
                    sSQL += "`gelirname`=N'" + tbName.Text + "',";
                    sSQL += "`gidername`= '',";
                    sSQL += "`gidersum`=0,";
                    sSQL += "`gelirsum`=" + (a.ToString()).Replace(",", ".") + ",";
                    sSQL += "`balance`=" + (b.ToString()).Replace(",", ".") + ",";
                    sSQL += "`takvim`='" + dtp.Value.ToString("yyyy-MM-dd") + "',";
                    sSQL += "`description`=N'" + tbDescription.Text + "'";
                    sSQL += " where `idcase`=" + dgvCase.CurrentRow.Cells[0].Value.ToString(); 
                    MySqlCommand upd = new MySqlCommand(sSQL, conn);

                    int rowAffected = upd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Сохранено!", "Касса", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showDGV();
                }
                else
                {
                    Decimal a = Decimal.Parse(tbSum.Text);
                    Decimal b = Decimal.Parse(tbBalance.Text);
                    string sSQL = "Update `case` set `casename`=";
                    sSQL += "N'" + tbName.Text + "',";
                    sSQL += "`gidername`=N'" + tbName.Text + "',";
                    sSQL += "`gidersum`=" + (a.ToString()).Replace(",", ".") + ",";
                    sSQL += "`gelirname`= '',";
                    sSQL += "`gelirsum`=0,";
                    sSQL += "`balance`=" + (b.ToString()).Replace(",", ".") + ",";
                    sSQL += "`takvim`='" + dtp.Value.ToString("yyyy-MM-dd") + "',";
                    sSQL += "`description`=N'" + tbDescription.Text + "'";
                    sSQL += " where `idcase`=" + dgvCase.CurrentRow.Cells[0].Value.ToString(); 
                    MySqlCommand upd = new MySqlCommand(sSQL, conn);

                    int rowAffected = upd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Сохранено!", "Касса", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showDGV();
                }
            }
            catch (Exception ex)
            { conn.Close(); MessageBox.Show("Ошибка" + ex); }
        }

        private void dgvCase_SelectionChanged(object sender, EventArgs e)
        {
            ReadDgv();
        }

        private void btnState_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                string sSQL = "delete from `case`";
                sSQL += " WHERE `idcase` = " + dgvCase.CurrentRow.Cells[0].Value.ToString()+";";
                MySqlCommand deletecase = new MySqlCommand(sSQL, conn);

                int rowAffected = deletecase.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Удалено!", "Касса", MessageBoxButtons.OK, MessageBoxIcon.Information);
                showDGV();
            }
            catch (Exception ex)
            { conn.Close(); MessageBox.Show("Ошибка" + ex); }
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

        private void dtpBegin_ValueChanged(object sender, EventArgs e)
        {
            showDGV();
        }

        private void btnRaport_Click(object sender, EventArgs e)
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                dsrap.Clear();
                MySqlDataAdapter dag = new MySqlDataAdapter("Select idCase as '№',casename as 'Наименование',gelirsum as 'Приход',gidersum as 'Расход',description as 'Описание',takvim from `case` where takvim between '" + dtpBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpBegin.Value.ToString("yyyy-MM-dd") + "';", conn);
                dag.Fill(dsrap, "case");
                
                if (dsrap.Tables[0].Rows.Count <= 0)
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
                xlWorkSheet.Cells[1, 2] = "Наименование";
                xlWorkSheet.Cells[1, 3] = "Приход";
                xlWorkSheet.Cells[1, 4] = "Расход";
                xlWorkSheet.Cells[1, 5] = "описание";
                xlWorkSheet.Cells[1, 6] = "Дата";

                for (i = 1; i <= dsrap.Tables[0].Rows.Count; i++)
                {
                    for (j = 0; j <= dsrap.Tables[0].Columns.Count - 1; j++)
                    {
                        xlWorkSheet.Cells[i + 1, j + 1] = dsrap.Tables[0].Rows[i - 1][j];
                    }
                }
                xlWorkSheet.Cells[dsrap.Tables[0].Rows.Count + 2, 3].Font.Bold = true;
                xlWorkSheet.Cells[dsrap.Tables[0].Rows.Count + 2, 4].Font.Bold = true;
                xlWorkSheet.Cells[dsrap.Tables[0].Rows.Count + 2, 5].Font.Bold = true;
                xlWorkSheet.Cells[dsrap.Tables[0].Rows.Count + 2, 3] = label6.Text.Replace(",", ".");
                xlWorkSheet.Cells[dsrap.Tables[0].Rows.Count + 2, 4] = label7.Text.Replace(",", ".");
                xlWorkSheet.Cells[dsrap.Tables[0].Rows.Count + 2, 5] = label8.Text + label9.Text.Replace(",", ".");
                xlWorkSheet.Columns.AutoFit();
                xlWorkBook.SaveAs("d:\\kasa_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                MessageBox.Show("Excel файл создан!");

                FileInfo fi = new FileInfo("d:\\kasa_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
                if (fi.Exists)
                {
                    System.Diagnostics.Process.Start("d:\\kasa_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
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
     
    }
}
