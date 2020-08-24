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
    public partial class AdminForm : Form
    {
        MySqlConnection conn;
        DataSet dsUser1 = new DataSet();
        DataSet dsUser2 = new DataSet();
        DataSet dsUser3 = new DataSet();
        DataSet dsCus = new DataSet();
        DataTable dtUser = new DataTable();
        DataTable dtCustomer = new DataTable();
        string servname;
        public string login;
        public AdminForm(string servname)
        {
            InitializeComponent();
            conn = new MySqlConnection("SERVER= "+servname+";port=3306;DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
            this.servname = servname;
        }

        private void btnState_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            rbAll.Checked = true;
            dtpRaportOt.Value = DateTime.Now.AddDays(-1);
            RefcbUser1();
            RefcbUser2();
            RefcbUser3();
            RefcbCustomer();
        }
        private void RefcbCustomer()
        {
            dsCus.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT  customerscode as name FROM customers", conn);
            da.Fill(dsCus);
            cbCustomers.DataSource = dsCus.Tables[0];
            cbCustomers.DisplayMember = "name";
            cbCustomers.ValueMember = "name";
        }
        private void RefcbUser1()
        {
            dsUser1.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("Select namesur as name from users where position='Дизайнер' or 'Сотрудник'", conn);
            da.Fill(dsUser1);
            cbUsers.DataSource = dsUser1.Tables[0];
            cbUsers.DisplayMember = "name";
            cbUsers.ValueMember = "name";
           
        }

        private void RefcbUser2()
        {
           /* dsUser2.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("Select namesur as name from users", conn);
            da.Fill(dsUser2);
            cbUsersMessage.DataSource = dsUser2.Tables[0];
            cbUsersMessage.DisplayMember = "name";
            cbUsersMessage.ValueMember = "name";*/
           
        }

        private void RefcbUser3()
        {
            dsUser3.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("Select namesur as name from users where position='Менеджер' or 'Директор'", conn);
            da.Fill(dsUser3);            
            cbUsersRaport.DataSource = dsUser3.Tables[0];
            cbUsersRaport.DisplayMember = "name";
            cbUsersRaport.ValueMember = "name";
        }
        private void btnX_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            RegisterForm regf = new RegisterForm(servname);
            Hide();
            regf.reg = false;            
            regf.ShowDialog();
            Close();
        }

        private void btnDelUser_Click(object sender, EventArgs e)
        {
            DeleteUserForm del = new DeleteUserForm(servname);
            Hide();
            del.ShowDialog();
            Show();
        }

        private void btnRaport_Click(object sender, EventArgs e)
        {
            if (rbAll.Checked)
            { AllRaport(); }
            else if (rbUser.Checked)
            { UserRaport(); }
            else if (rbCustomers.Checked)
            { CustomerRaport(); }
                
        }
        private void AllRaport()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                dtUser.Clear();
                string sSQL = "SELECT name,begindate,enddate,amount,price,sum,status FROM orders where begindate between '" + dtpRaportOt.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtpRaportDo.Value.ToString("yyyy-MM-dd HH:mm:ss") + "';";

                MySqlDataAdapter raport = new MySqlDataAdapter(sSQL, conn);
                raport.Fill(dtUser);

                if (dtUser.Rows.Count <= 0)
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

                xlWorkSheet.Cells[1, 1] = "Заказ";
                xlWorkSheet.Cells[1, 2] = "Дата создания заказа";
                xlWorkSheet.Cells[1, 3] = "Дата выполнения заказа";
                xlWorkSheet.Cells[1, 4] = "Количество";
                xlWorkSheet.Cells[1, 5] = "Цена";
                xlWorkSheet.Cells[1, 6] = "Сумма";
                xlWorkSheet.Cells[1, 7] = "Статус";

                for (i = 1; i <= dtUser.Rows.Count; i++)
                {
                    for (j = 0; j <= dtUser.Columns.Count - 1; j++)
                    {
                        xlWorkSheet.Cells[i + 1, j + 1] = dtUser.Rows[i - 1][j];
                    }
                }
                xlWorkSheet.Columns.AutoFit();
                xlWorkBook.SaveAs("d:\\All_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                MessageBox.Show("Excel файл создан!");

                FileInfo fi = new FileInfo("d:\\All_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
                if (fi.Exists)
                {
                    System.Diagnostics.Process.Start("d:\\All_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
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
        private void UserRaport()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                dtUser.Clear();
                string sSQL = "SELECT orders.name,begindate,enddate,amount,price,sum,status FROM orders INNER JOIN users ON orders.manager=users.idUsers and users.namesur='"+cbUsersRaport.SelectedValue+"' and orders.begindate between '"+dtpRaportOt.Value.ToString("yyyy-MM-dd HH:mm:ss")+"' and '"+dtpRaportDo.Value.ToString("yyyy-MM-dd HH:mm:ss")+"';" ;

                MySqlDataAdapter raport = new MySqlDataAdapter(sSQL, conn);
                raport.Fill(dtUser);

                if (dtUser.Rows.Count <= 0)
                { MessageBox.Show("В базе нету записей!"); return; }

                Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                if (xlApp == null)
                {
                    MessageBox.Show("Excel не установлено!");
                    return;
                }


                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                int i = 0;
                int j = 0;

                xlWorkSheet.Cells[1, 1] = "Заказ";
                xlWorkSheet.Cells[1, 2] = "Дата создания заказа";
                xlWorkSheet.Cells[1, 3] = "Дата выполнения заказа";
                xlWorkSheet.Cells[1, 4] = "Количество";
                xlWorkSheet.Cells[1, 5] = "Цена";
                xlWorkSheet.Cells[1, 6] = "Сумма";
                xlWorkSheet.Cells[1, 7] = "Статус";

                for (i = 1; i <= dtUser.Rows.Count; i++)
                {
                    for (j = 0; j <= dtUser.Columns.Count - 1; j++)
                    {
                        xlWorkSheet.Cells[i + 1, j + 1] = dtUser.Rows[i - 1][j];
                    }
                }
                xlWorkSheet.Columns.AutoFit();
                xlWorkBook.SaveAs("d:\\"+cbUsersRaport.SelectedValue+"_raport_"+DateTime.Now.ToString("yyyy_MM_dd")+".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                MessageBox.Show("Excel файл создан!");

                FileInfo fi = new FileInfo("d:\\" + cbUsersRaport.SelectedValue + "_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
                if (fi.Exists)
                {
                    System.Diagnostics.Process.Start("d:\\" + cbUsersRaport.SelectedValue + "_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
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
        private void CustomerRaport()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                dtCustomer.Clear();
                string sSQL = "SELECT orders.name,begindate,enddate,amount,price,sum,status FROM orders INNER JOIN customers ON orders.customer=customers.idcustomers and customers.customerscode='" + cbCustomers.SelectedValue + "' and orders.begindate between '" + dtpRaportOt.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtpRaportDo.Value.ToString("yyyy-MM-dd HH:mm:ss") + "';";

                MySqlDataAdapter raport = new MySqlDataAdapter(sSQL, conn);
                raport.Fill(dtCustomer);

                if (dtCustomer.Rows.Count <= 0)
                { MessageBox.Show("В базе нету записей!"); return; }

                Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                if (xlApp == null)
                {
                    MessageBox.Show("Excel не установлено!");
                    return;
                }


                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                int i = 0;
                int j = 0;

                xlWorkSheet.Cells[1, 1] = "Заказ";
                xlWorkSheet.Cells[1, 2] = "Дата создания заказа";
                xlWorkSheet.Cells[1, 3] = "Дата выполнения заказа";
                xlWorkSheet.Cells[1, 4] = "Количество";
                xlWorkSheet.Cells[1, 5] = "Цена";
                xlWorkSheet.Cells[1, 6] = "Сумма";
                xlWorkSheet.Cells[1, 7] = "Статус";

                for (i = 1; i <= dtCustomer.Rows.Count; i++)
                {
                    for (j = 0; j <= dtCustomer.Columns.Count - 1; j++)
                    {
                        xlWorkSheet.Cells[i + 1, j + 1] = dtCustomer.Rows[i - 1][j];
                    }
                }
                xlWorkSheet.Columns.AutoFit();
                xlWorkBook.SaveAs("d:\\" + cbCustomers.SelectedValue + "_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                MessageBox.Show("Excel файл создан!");

                FileInfo fi = new FileInfo("d:\\" + cbCustomers.SelectedValue + "_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
                if (fi.Exists)
                {
                    System.Diagnostics.Process.Start("d:\\" + cbCustomers.SelectedValue + "_raport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls");
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnManager_Click(object sender, EventArgs e)
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();

            try
            {

                string sSQL = "UPDATE USERS SET position='Менеджер' WHERE namesur='" + cbUsers.SelectedValue + "';";

                    MySqlCommand update = new MySqlCommand(sSQL, conn);

                    int rowAffected = update.ExecuteNonQuery();
                    conn.Close(); RefcbUser1();
                    MessageBox.Show("Изменено!", "Пользователь", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
               
            }
            catch (Exception ex)
            {
                conn.Close(); MessageBox.Show("Ошибка" + ex);
            }
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            ProductForm prodf = new ProductForm(servname);
            Hide();
            prodf.ShowDialog();
            Show();
        }

        private void btnCase_Click(object sender, EventArgs e)
        {
            CaseForm casef = new CaseForm(servname);
            Hide();
            casef.ShowDialog();
            Show();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            CariForm carif = new CariForm(servname);
            Hide();
            carif.ShowDialog();
            Show();
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            RegisterForm regf = new RegisterForm(servname);
            Hide();
            regf.upd = true;
            regf.login = login;
            regf.ShowDialog();

            Show();
        }

    }
}
