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
    public partial class ProductForm : Form
    {

        MySqlConnection conn;
        DataSet ds = new DataSet();
       DataSet dsCus = new DataSet();
       DataSet dsStok = new DataSet();
        DataTable dtProduct = new DataTable();
        DataTable dtCus = new DataTable();
       
        DataTable dtp = new DataTable();
        MySqlDataAdapter dag;
        MySqlDataAdapter dasum;
        DataTable dtsum = new DataTable();

        BindingSource bs1 = new BindingSource();
        string servname;
        Decimal a, b;

        NewProductForm npf;
        public ProductForm(string servname)
        {
            InitializeComponent();
            cbTip.Items.Add("Продажа");
            cbTip.Items.Add("Покупка");
            conn = new MySqlConnection("SERVER=" + servname + ";DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
            this.AcceptButton = btnOK;
            this.servname = servname;
        }
        private void RefcbCustomer()
        {
            dsCus.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT  customerscode as name FROM customers", conn);
            da.Fill(dsCus);            
            if(dsCus.Tables[0].Rows.Count>0){
            cbCustomers.DataSource = dsCus.Tables[0];
            cbCustomers.DisplayMember = "name";
            cbCustomers.ValueMember = "name";}
        }
        private void RefcbStok()
        {
            dsStok.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT  code as name,measure,photo FROM goods", conn);
            da.Fill(dsStok);
            if (dsStok.Tables[0].Rows.Count > 0)
            {
                cbStok.DataSource = dsStok.Tables[0];
                cbStok.DisplayMember = "name";
                cbStok.ValueMember = "name";
                tbMeasure.Text = dsStok.Tables[0].Rows[0]["measure"].ToString();
                byte[] FetchedImgBytes = (byte[])dsStok.Tables[0].Rows[0]["photo"];
                MemoryStream streamImg = new MemoryStream(FetchedImgBytes);
                System.Drawing.Image FetchedImage;
                FetchedImage = System.Drawing.Image.FromStream(streamImg);
                pbProduct.Image = FetchedImage;                
            }

        }
        private void ProductForm_Load(object sender, EventArgs e)
        {
            cbTip.SelectedIndex = 0;
            cbFilter.SelectedIndex = 0;
            dtpGoodsOperationsEnd.Value = DateTime.Now.AddDays(1);
            RefcbCustomer();
            RefcbStok();
            showDGV();
            this.dgvGoods.AllowUserToAddRows = false;
        }
        #region Methods
        private void showDGV()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                ds.Clear();  dtp.Clear();
                if (cbFilter.SelectedIndex == 0)
                {
                    dag = new MySqlDataAdapter("select tip as 'Тип',idgoodsoperations as '№',customerscode as 'Заказчик',code as 'Продукт',amount as 'Кол-во',price as 'Цена',summa as 'Сумма',concat(goods.name,'; ',goods.description) as description from goodsoperations,goods,customers where goodsoperations.goodscode=goods.idgoods and goodsoperations.customers=customers.idcustomers and tarih between '" + dtpGoodsOperationsBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpGoodsOperationsEnd.Value.ToString("yyyy-MM-dd") + "' order by idgoodsoperations;", conn);
                    dag.Fill(ds);
                    dtsum.Clear();
                    dasum = new MySqlDataAdapter("SELECT SUM(summa) AS Sum FROM goodsoperations where tarih between '" + dtpGoodsOperationsBegin.Value.ToShortDateString() + "' and '" + dtpGoodsOperationsEnd.Value.ToShortDateString() + "';", conn);
                    dasum.Fill(dtsum);
                    if (dtsum.Rows.Count > 0)
                    { label6.Text = "Сумма: " + dtsum.Rows[0]["Sum"]; }
                }
                else if (cbFilter.SelectedIndex == 1)
                {
                    dag = new MySqlDataAdapter("select tip as 'Тип',idgoodsoperations as '№',customerscode as 'Заказчик',code as 'Продукт',amount as 'Кол-во',price as 'Цена',summa as 'Сумма',concat(goods.name,'; ',goods.description)  from goodsoperations,goods,customers where goodsoperations.goodscode=goods.idgoods and goodsoperations.customers=customers.idcustomers and tip='" + cbFilter.SelectedItem + "' and tarih between '" + dtpGoodsOperationsBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpGoodsOperationsEnd.Value.ToString("yyyy-MM-dd") + "' order by idgoodsoperations;", conn);
                    dag.Fill(ds);
                    dtsum.Clear();
                    dasum = new MySqlDataAdapter("SELECT SUM(summa) AS Sum FROM goodsoperations where tip='" + cbFilter.SelectedItem + "' and tarih between '" + dtpGoodsOperationsBegin.Value.ToShortDateString() + "' and '" + dtpGoodsOperationsEnd.Value.ToShortDateString() + "';", conn);
                    dasum.Fill(dtsum);
                    if (dtsum.Rows.Count > 0)
                    { label6.Text = "Сумма: " + dtsum.Rows[0]["Sum"]; }
                }
                else if (cbFilter.SelectedIndex == 2)
                {
                    dag = new MySqlDataAdapter("select tip as 'Тип',idgoodsoperations as '№',customerscode as 'Заказчик',code as 'Продукт',amount as 'Кол-во',price as 'Цена',summa as 'Сумма',concat(goods.name,'; ',goods.description)  from goodsoperations,goods,customers where goodsoperations.goodscode=goods.idgoods and goodsoperations.customers=customers.idcustomers and tip='" + cbFilter.SelectedItem + "' and tarih between '" + dtpGoodsOperationsBegin.Value.ToString("yyyy-MM-dd") + "' and '" + dtpGoodsOperationsEnd.Value.ToString("yyyy-MM-dd") + "' order by idgoodsoperations;", conn);
                    dag.Fill(ds);
                    dtsum.Clear();
                    dasum = new MySqlDataAdapter("SELECT SUM(summa) AS Sum FROM goodsoperations where tip='" + cbFilter.SelectedItem + "' and tarih between '" + dtpGoodsOperationsBegin.Value.ToShortDateString() + "' and '" + dtpGoodsOperationsEnd.Value.ToShortDateString() + "';", conn);
                    dasum.Fill(dtsum);
                    if (dtsum.Rows.Count > 0)
                    { label6.Text = "Сумма: " + dtsum.Rows[0]["Sum"]; }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bs1.DataSource = ds.Tables[0];                   
                    dgvGoods.DataSource = bs1;
                    dgvGoods.BorderStyle = BorderStyle.Fixed3D;
                    dgvGoods.Columns[7].Visible = false;
                   
                   
                    dgvGoods.Columns[0].Width = 60;
                    dgvGoods.Columns[1].Width = 30;
                    dgvGoods.Columns[2].Width = 90;
                    dgvGoods.Columns[3].Width = 60;
                    dgvGoods.Columns[4].Width = 60;
                    dgvGoods.Columns[5].Width = 60;
                    dgvGoods.Columns[6].Width = 70;
                    foreach (DataGridViewColumn column in dgvGoods.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    ReadDgv();
                }
                
                conn.Close();

            }
            catch (Exception ex)
            { MessageBox.Show("Hata!" + ex); }
        }

        private void updateBD()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                dtProduct.Clear();
                MySqlDataAdapter du = new MySqlDataAdapter("Select idgoods from goods where code='" + cbStok.SelectedValue + "'", conn);
                du.Fill(dtProduct);

               
                string sSQL = "UPDATE `goodsoperations` SET `tip` =";
                sSQL += "'" + cbTip.SelectedItem + "',";                
                sSQL += "`amount` =" + tbAmount.Text.Replace(",", ".") + ",";
                sSQL += "`summa` =" + tbSum.Text.Replace(",", ".") + ",";
                sSQL += "`price` =" + tbPrice.Text.Replace(",", ".") + ",";                
                sSQL += "`description` = N'" + tbDescription.Text + "',";
                sSQL += "`customers` =" + Convert.ToInt32(dtCus.Rows[0]["idcustomers"]) +",";
                sSQL += "`goodscode` = N'" + Convert.ToInt32(dtProduct.Rows[0]["idgoods"])  + "'";          
                sSQL += " WHERE `idgoodsoperations` = " + dgvGoods.CurrentRow.Cells[1].Value.ToString()+";";
                MySqlCommand updategoods = new MySqlCommand(sSQL, conn);
                int rowAffected = updategoods.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Saklandı!", "Ürünler", MessageBoxButtons.OK, MessageBoxIcon.Information);
                showDGV();
            }
            catch (Exception ex)
            { conn.Close(); MessageBox.Show("Error" + ex); }
        }
        private void ReadDgv()
        {

           if (ds.Tables[0].Rows.Count > 0)
            {
                if (dgvGoods.CurrentRow.Cells[0].Value.ToString() == "Продажа")
                { cbTip.SelectedIndex = 0; }
               else if(dgvGoods.CurrentRow.Cells[0].Value.ToString() == "Покупка")
                { cbTip.SelectedIndex = 1; }
                cbCustomers.SelectedValue = dgvGoods.CurrentRow.Cells[2].Value.ToString();
                cbStok.SelectedValue = dgvGoods.CurrentRow.Cells[3].Value.ToString();
                tbAmount.Text = dgvGoods.CurrentRow.Cells[4].Value.ToString();
                tbPrice.Text = dgvGoods.CurrentRow.Cells[5].Value.ToString();
                tbSum.Text=dgvGoods.CurrentRow.Cells[6].Value.ToString();
                tbDescription.Text = dgvGoods.CurrentRow.Cells[7].Value.ToString();
            }
            else
            {
                tbAmount.Text = "0";
                tbSum.Text = "";
                tbDescription.Text = "";                
                tbPrice.Text = "0";
            }
        }
        private void saveNewRow()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                dtProduct.Clear();
                MySqlDataAdapter du = new MySqlDataAdapter("Select idgoods from goods where code='" + cbStok.SelectedValue + "'", conn);
                du.Fill(dtProduct);

                dtCus.Clear();
                MySqlDataAdapter dc = new MySqlDataAdapter("Select idcustomers from customers where customerscode='" + cbCustomers.SelectedValue + "'", conn);
                dc.Fill(dtCus);               

                if (dtProduct.Rows.Count > 0 && dtCus.Rows.Count > 0)
                {
                    Decimal a = Decimal.Parse(tbAmount.Text);
                    Decimal b = Decimal.Parse(tbPrice.Text);
                     Decimal c = Decimal.Parse(tbSum.Text);                    
                    string sSQL = "INSERT INTO `goodsoperations` (`customers`, `goodscode`, `tip`, `amount`, `price`,`summa`, `description`, `tarih`) VALUES (";
                    sSQL += "" + Convert.ToInt32(dtCus.Rows[0]["idcustomers"]) + ",";
                    sSQL += "" + Convert.ToInt32(dtProduct.Rows[0]["idgoods"]) + ",";
                    sSQL += "N'" + cbTip.SelectedItem + "',";
                    sSQL += "" + (a.ToString()).Replace(",", ".") + ",";                    
                    sSQL += "" + (b.ToString()).Replace(",", ".") + ",";
                    sSQL += "" + (c.ToString()).Replace(",", ".") + ",";
                    sSQL += "N'" + tbDescription.Text + "',";
                    sSQL += "N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    sSQL += ");";
                    MySqlCommand insert = new MySqlCommand(sSQL, conn);

                    int rowAffected = insert.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Saklandı!", "Ürünler", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showDGV();
                }
            }
            catch (Exception ex)
            { conn.Close(); MessageBox.Show("Error" + ex); }     
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
           Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            saveNewRow();
        }

     

        private void tbAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch == 44 && tbAmount.Text.IndexOf(",") != -1)
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                string sSQL = "delete from goods where code='" + cbStok.SelectedValue + "'"; 

                MySqlCommand deletegoods = new MySqlCommand(sSQL, conn);

                int rowAffected = deletegoods.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Silindi!", "Ürünler", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefcbStok();
            }
            catch (Exception ex)
            { conn.Close(); MessageBox.Show("Error" + ex); }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            updateBD();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            npf = new NewProductForm(servname);
            Hide();
            npf.ShowDialog();
            RefcbStok();
            Show();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbStok_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (dsStok.Tables[0].Rows.Count > 0)
            {  
              
                tbMeasure.Text = dsStok.Tables[0].Rows[cbStok.SelectedIndex]["measure"].ToString();
                byte[] FetchedImgBytes = (byte[])dsStok.Tables[0].Rows[cbStok.SelectedIndex]["photo"];
                MemoryStream streamImg = new MemoryStream(FetchedImgBytes);
                System.Drawing.Image FetchedImage;
                FetchedImage = System.Drawing.Image.FromStream(streamImg);
                pbProduct.Image = FetchedImage;
            }
        }

        private void tbAmount_TextChanged(object sender, EventArgs e)
        {
            if (tbPrice.Text != "" && tbAmount.Text != "")
            {
                a = Decimal.Parse(tbAmount.Text);
                b = Decimal.Parse(tbPrice.Text);
                tbSum.Text = (a * b).ToString();
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            showDGV();
        }

        private void dgvGoods_SelectionChanged(object sender, EventArgs e)
        {
            ReadDgv();
        }

        private void dtpGoodsOperationsBegin_ValueChanged(object sender, EventArgs e)
        {
            showDGV();
        }

        private void btnRaport_Click(object sender, EventArgs e)
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                

                if (ds.Tables[0].Rows.Count <= 0)
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

                xlWorkSheet.Cells[1, 1] = "Тип";
                xlWorkSheet.Cells[1, 2] = "№";
                xlWorkSheet.Cells[1, 3] = "Клиент";
                xlWorkSheet.Cells[1, 4] = "Продукт";
                xlWorkSheet.Cells[1, 5] = "Количество";
                xlWorkSheet.Cells[1, 6] = "Цена";
                xlWorkSheet.Cells[1, 7] = "Сумма";
                xlWorkSheet.Cells[1, 8] = "Описание";
                for (i = 1; i <= ds.Tables[0].Rows.Count; i++)
                {
                    for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                    {
                        xlWorkSheet.Cells[i + 1, j + 1] = ds.Tables[0].Rows[i - 1][j];
                    }
                }
                xlWorkSheet.Cells[ds.Tables[0].Rows.Count + 2, 7].Font.Bold = true;
                xlWorkSheet.Cells[ds.Tables[0].Rows.Count + 2, 7] = label6.Text;
                xlWorkSheet.Columns.AutoFit();
                xlWorkBook.SaveAs("d:\\Product_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                MessageBox.Show("Excel файл создан!");

                FileInfo fi = new FileInfo("d:\\Product_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
                if (fi.Exists)
                {
                    System.Diagnostics.Process.Start("d:\\Product_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
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

        private void button3_Click(object sender, EventArgs e)
        {
            tbAmount.Text = "0";
            tbPrice.Text = "0";
            tbSum.Text = "";
            tbDescription.Text = "";
        }

        private void btnAddCari_Click(object sender, EventArgs e)
        {
            CustomerForm cusf = new CustomerForm(servname);
            Hide();
            cusf.ShowDialog();
            RefcbCustomer();
            Show();
        }

        private void btnEditProd_Click(object sender, EventArgs e)
        {
            npf = new NewProductForm(servname);
            npf.name = cbStok.SelectedValue.ToString();
            npf.upd=true;
            npf.id = cbStok.SelectedIndex+1;
            Hide();
            npf.ShowDialog();
            RefcbStok();
            Show();
        }

       

    }
}
