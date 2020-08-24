using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace kepterchatLogin
{
    public partial class MainForm : Form
    {
        

        MySqlConnection conn;
        DataSet dsUser = new DataSet();
        DataSet dsCus = new DataSet();

        DataTable dtUser = new DataTable();
        DataTable dtCus = new DataTable();
        DataTable dtOrd = new DataTable();
        DataTable dtIp = new DataTable();
        DataTable dtIpF = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtManager = new DataTable();

        DataTable dtCheck = new DataTable();

        public int uid;
        public string position;
        public bool admin;
        Decimal a, b,c,balance;
        string servname,LocalIp,LocalPort;
        DataSet ds = new DataSet();
        
        
       
        int manager;

        

        public MainForm(string servname)
        {
            this.servname = servname;
            InitializeComponent();
            conn = new MySqlConnection("SERVER= "+servname+";port=3306;DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
            this.servname = servname;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefcbUser();
            RefcbCustomer();           
            GetBuhIP();            
        }
       
        private void GetBuhIP()
        {
            
            dtIpF.Clear();
            MySqlDataAdapter da1 = new MySqlDataAdapter("select ip,port from users where login ='" +"Kris" + "'", conn);
            da1.Fill(dtIpF);
            if (dtIpF.Rows.Count > 0)
            {
                LocalIp = dtIpF.Rows[0]["ip"].ToString();
                LocalPort = dtIpF.Rows[0]["port"].ToString();
            }
        }

        private void LbOrdersManager()
        {
            dtOrd.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT orders.name,customer, begindate, enddate, amount, price, sum, user FROM orders INNER JOIN users ON orders.manager=users.idUsers and orders.manager= " + uid, conn);
            da.Fill(dtOrd);
            if (dtOrd.Rows.Count > 0)
            {
                
                //listView1.Items.Clear();
               lvOrders.Items.Clear();

               for (int i = 0; i < dtOrd.Rows.Count; i++)
                {

                    lvOrders.Items.Add(dtOrd.Rows[i]["name"].ToString());
                    
                    //listView1.Items.Add(dt);
                    //  else lvOrders.Items.Add(dtOrd.Rows[i]["name"].ToString());
                }
               lvOrders.SelectedIndex = 0;
            }
        }

        private void LbOrdersDesigner()
        {
            dtOrd.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT orders.name,customer, begindate, enddate, amount, price, sum, user FROM orders INNER JOIN users ON orders.user=users.idUsers and orders.user= " + uid, conn);
            da.Fill(dtOrd);
            if (dtOrd.Rows.Count > 0)
            {

                //listView1.Items.Clear();
                lvOrders.Items.Clear();

                for (int i = 0; i < dtOrd.Rows.Count; i++)
                {

                    lvOrders.Items.Add(dtOrd.Rows[i]["name"].ToString());

                    //listView1.Items.Add(dt);
                    //  else lvOrders.Items.Add(dtOrd.Rows[i]["name"].ToString());
                }
                lvOrders.SelectedIndex = 0;
            }
        }
        private void LbOrdersBuh()
        {
            dtOrd.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT orders.name,customer, begindate, enddate, amount, price, sum, user FROM orders", conn);
            da.Fill(dtOrd);
            if (dtOrd.Rows.Count > 0)
            {

                //listView1.Items.Clear();
                lvOrders.Items.Clear();

                for (int i = 0; i < dtOrd.Rows.Count; i++)
                {

                    lvOrders.Items.Add(dtOrd.Rows[i]["name"].ToString());

                    //listView1.Items.Add(dt);
                    //  else lvOrders.Items.Add(dtOrd.Rows[i]["name"].ToString());
                }
                lvOrders.SelectedIndex = 0;
            }
        }
        private void RefcbUser()
        {
            dsUser.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("Select namesur as name from users where position='Дизайнер'", conn);
            da.Fill(dsUser);
            if (dsUser.Tables[0].Rows.Count == 0)
                { cbUsers.SelectedIndex = 0; }
            else 
                { 
                 cbUsers.DataSource = dsUser.Tables[0];
                 cbUsers.DisplayMember = "name";
                 cbUsers.ValueMember = "name";cbUsers.SelectedIndex = 0;
                }
            if (position == "Директор"||position == "Бухгалтер")
                   
            { admin=true; btnAddNewCustomer.Visible = true;
            //btnAddOrder.Visible = true;
            btnDel.Visible = true;
            btnDelCus.Visible = true;
            btnSettings.Visible = true;
            btnAllright.Visible = true;
            label1.Visible = true;
            tbAvans.Visible = true;            
            LbOrdersBuh();
            }
            else if (position == "Дизайнер")
            { dtpEndDate.Visible = true; btnEndDate.Visible = true;
            btnAddNewCustomer.Visible = false;
            btnAddOrder.Visible = false;
            btnDel.Visible = false;
            btnDelCus.Visible = false;
            LbOrdersDesigner();
            }
            else if (position == "Менеджер")
            { btnAddNewCustomer.Visible = true;
            btnAddOrder.Visible = true;
            btnDel.Visible = true;
            btnDelCus.Visible = true;

            dtpEndDate.Visible = false; btnEndDate.Visible =false;

            LbOrdersManager();
            }
        }
        private void btnX_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }



        private void lblvOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOk.Visible = false;
            btnCancel.Visible = false;
            try{
            dtOrd.Clear();
            dtUser.Clear();
            dtCus.Clear();


            MySqlDataAdapter da = new MySqlDataAdapter("Select name,customer, begindate, enddate, amount, price, sum, user,status,manager from orders where name='" + lvOrders.SelectedItem.ToString() + "'", conn);
            da.Fill(dtOrd);

            if (dtOrd.Rows.Count > 0)
            { manager = (Int32)dtOrd.Rows[0]["manager"];
                tbOrderName.Text = dtOrd.Rows[0]["name"].ToString();
                tbAmount.Text = ((Decimal)dtOrd.Rows[0]["amount"]).ToString();
                lblDate.Text = dtOrd.Rows[0]["begindate"].ToString();
                tbPrice.Text = ((Decimal)dtOrd.Rows[0]["price"]).ToString();
                lblSum.Text = ((Decimal)dtOrd.Rows[0]["sum"]).ToString();
                lblStatus.Text = dtOrd.Rows[0]["status"].ToString() + " " + dtOrd.Rows[0]["enddate"].ToString();
                MySqlDataAdapter dusers = new MySqlDataAdapter("Select namesur  from users where idUsers ="+(Int32)dtOrd.Rows[0]["user"], conn);
                dusers.Fill(dtUser);
                cbUsers.SelectedValue = dtUser.Rows[0]["namesur"].ToString();
                MySqlDataAdapter dcus= new MySqlDataAdapter("Select customerscode  from customers where idcustomers ="+(Int32)dtOrd.Rows[0]["customer"], conn);
                dcus.Fill(dtCus);
                cbCustomers.SelectedValue = dtCus.Rows[0]["customerscode"].ToString();
            }
            }
                catch(Exception ex)
            {MessageBox.Show("Hata"+ex);}
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            btnOk.Visible = true;
            btnCancel.Visible = true;
            cbCustomers.SelectedIndex = 0;
            cbUsers.SelectedIndex = 0;
            tbOrderName.Text = "";
            tbAmount.Text = "0";
            tbPrice.Text = "0,00";
            lblSum.Text = "";
            lblDate.Text = "";
            lblStatus.Text = "";

            dtpEndDate.Visible = false;
            btnEndDate.Visible = false;
        }

       

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            CustomerForm cusf = new CustomerForm(servname);
            Hide();
            cusf.ShowDialog();
            RefcbCustomer();
            Show();
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            saveNewOrder();
            LbOrdersManager();
        }
       
       
        
        private void saveNewOrder()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                a = Decimal.Parse(tbAmount.Text);
                b = Decimal.Parse(tbPrice.Text);
                c = Decimal.Parse(lblSum.Text);
                
                dtUser.Clear();
                MySqlDataAdapter du = new MySqlDataAdapter("Select idusers from users where namesur='" + cbUsers.SelectedValue + "'", conn);
                du.Fill(dtUser);

                dtCus.Clear();
                MySqlDataAdapter dc = new MySqlDataAdapter("Select idcustomers from customers where customerscode='" + cbCustomers.SelectedValue + "'", conn);
                dc.Fill(dtCus);

                if (dtUser.Rows.Count > 0&&dtCus.Rows.Count>0)
                {
                    DateTime now = DateTime.Now;
                    string sSQL = "INSERT INTO `orders`(`name`,`customer`,`begindate`,`amount`,`price`,`sum`,`user`,`status`,manager)VALUES(";
                    sSQL += "'" + tbOrderName.Text + "'," + (Int32)dtCus.Rows[0]["idcustomers"] + ",";
                    sSQL += "'" + now.ToString("yyyy.MM.dd HH:mm:ss") + "','" + (a.ToString()).Replace(",", ".") + "','" + (b.ToString()).Replace(",", ".") + "','" + (c.ToString()).Replace(",", ".") + "',";
                    sSQL += (Int32)dtUser.Rows[0]["idusers"]+",'В процессе',"+uid+"";
                    sSQL += ");";
                    MySqlCommand insert = new MySqlCommand(sSQL, conn);                   

                    int rowAffected = insert.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Сохранено!", "Заказы", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else MessageBox.Show("Ошибка!", "Заказы", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                conn.Close(); MessageBox.Show("Ошибка!" + ex);
            }

        }

        private void tbAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch=e.KeyChar;
            if (ch == 44 && tbAmount.Text.IndexOf(",") != -1)
            { e.Handled = true;
            return;
            }
            if (!Char.IsDigit(ch)&&ch!=8&&ch!=44)
            {
                e.Handled = true;
                return;
            }
        }

        private void tbAmount_TextChanged(object sender, EventArgs e)
        {
            if (tbPrice.Text != "" && tbAmount.Text != "")
             {
                 a = Decimal.Parse(tbAmount.Text);
                 b = Decimal.Parse(tbPrice.Text);
                 lblSum.Text = (a * b).ToString();
             }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cbCustomers.SelectedIndex = 0;
            cbUsers.SelectedIndex = 0;
            tbOrderName.Text = "";
            tbAmount.Text = "0";
            tbPrice.Text = "0,00";
            lblSum.Text = "";
            lblDate.Text = "";
            lblStatus.Text = "";
        }

        private void btnState_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            dtCus.Clear();
            MySqlDataAdapter dc = new MySqlDataAdapter("Select name,surname,tel,mail from customers where customerscode='" + cbCustomers.SelectedValue + "'", conn);
            dc.Fill(dtCus);
            lblCus.Text = (string)dtCus.Rows[0]["name"] + " " + (string)dtCus.Rows[0]["surname"] + "     " + (string)dtCus.Rows[0]["tel"] + " - " + (string)dtCus.Rows[0]["mail"];
        }

        private void cbCustomers_SelectedValueChanged(object sender, EventArgs e)
        {
            lblCus.Text = "";
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch == 44 && tbPrice.Text.IndexOf(",") != -1)
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

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                string sSQL = "delete  FROM orders where name='" + lvOrders.GetItemText(lvOrders.SelectedItem) + "';";
                   
                    MySqlCommand delete = new MySqlCommand(sSQL, conn);

                    int rowAffected = delete.ExecuteNonQuery();
                    conn.Close(); 
                    MessageBox.Show("Удалено!", "Заказы", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LbOrdersManager();
            }
            catch (Exception ex)
            {
                conn.Close(); MessageBox.Show("Ошибка" + ex);
            }
        }

      

        private void btnDelCus_Click(object sender, EventArgs e)
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
            dtCheck.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT Orders.idorders FROM Orders INNER JOIN Customers ON Orders.customer=Customers.idcustomers and Customers.customerscode='" + cbCustomers.SelectedValue + "';", conn);
            da.Fill(dtCheck);
            if (dtCheck.Rows.Count == 0)
            {
                string sSQL = "delete  FROM customers where customerscode='" + cbCustomers.SelectedValue + "';";

                MySqlCommand delete = new MySqlCommand(sSQL, conn);

                int rowAffected = delete.ExecuteNonQuery();
                conn.Close(); RefcbCustomer();
                MessageBox.Show("Удалено!", "Заказы", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else { MessageBox.Show("Сначала удалите заказы заказчика!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            catch (Exception ex)
            {
                conn.Close(); MessageBox.Show("Ошибка" + ex);
            }
        }

        private void btnEndDate_Click(object sender, EventArgs e)
        {
           
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                string sSQL = "UPDATE orders SET enddate='" + dtpEndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', status='Завершен " + dtpEndDate.Value.ToString("yyyy-MM-dd HH:mm:ss")  + "' where name='" + lvOrders.GetItemText(lvOrders.SelectedItem) + "';";

                MySqlCommand upd = new MySqlCommand(sSQL, conn);

                int rowAffected = upd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Сохранено!", "Заказы", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                
            }
            catch (Exception ex)
            {
                conn.Close(); MessageBox.Show("Ошибка " + ex);
            }
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;

            g.FillRectangle(new SolidBrush(Color.Silver), e.Bounds);

            // Print text

            e.DrawFocusRectangle();
        }

       
        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (admin == true)
            {
                AdminForm admf = new AdminForm(servname);
                Hide();
                admf.login = lblName.Text;
                admf.ShowDialog();
                Show();
            }
            else { RegisterForm regf = new RegisterForm(servname);
            Hide();
            regf.upd = true;
            regf.login = lblName.Text;  
            regf.ShowDialog();
                      
            Show();
            }
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
             Hide();
             if (lblName.Text == "Kris")
             {
                 ServerForm chf = new ServerForm(LocalIp,LocalPort);
                 chf.ShowDialog();
             }
             else
             {
                 ClientForm clf=new ClientForm(LocalIp,LocalPort);
                 clf.UserName = lblName.Text;
                 clf.ShowDialog();
             }
                 Show();
        }

        private void btnAllright_Click(object sender, EventArgs e)
        {
            dt.Clear();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT SUM( gelirsum ) - SUM( gidersum ) as balance FROM  `case`", conn);
            da.Fill(dt);
            dtManager.Clear();
            MySqlDataAdapter dm = new MySqlDataAdapter("SELECT namesur  FROM  `users` where `idusers`=" + manager, conn);
            dm.Fill(dtManager);
            if (dt.Rows.Count > 0)
            {
                balance = Convert.ToDecimal(dt.Rows[0]["balance"]);
            }
            else balance = 0;
            Decimal a = Decimal.Parse(lblSum.Text) - Convert.ToDecimal(tbAvans.Text);

            if (Convert.ToDecimal(tbAvans.Text) > 0 && Convert.ToDecimal(tbAvans.Text) < Convert.ToDecimal(lblSum.Text))
            {
                if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
                                    
                    string sSQL = "INSERT INTO `cari` (`operation`, `cariname`, `operationname`, `debit`,`kredit`, `description`, `tarih`) VALUES (";
                    sSQL += "N'Дебитор / Borçlu',";
                    sSQL += "N'" + cbCustomers.SelectedValue + "',";
                    sSQL += "N'" + tbOrderName.Text + " - " + dtManager.Rows[0]["namesur"].ToString() + "',";
                    sSQL += "" + (a.ToString()).Replace(",", ".") + ",";
                    sSQL += " 0 ,";                    
                    sSQL += "N'Количество: " + tbAmount.Text + "; Цена: "+tbPrice.Text+"; Всего: "+lblSum.Text+";',";
                    sSQL += "'" +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";                    
                    sSQL += "); ";//insert cari

                    sSQL += "INSERT INTO `case`(`casename`,`gelirname`,`gelirsum`,`balance`,`takvim`,`description`)VALUES(";
                    sSQL += "N'" + tbOrderName.Text + " - " + dtManager.Rows[0]["namesur"].ToString() + "',";
                    sSQL += "N'" + tbOrderName.Text + " - " + dtManager.Rows[0]["namesur"].ToString() + "',";
                    sSQL += "" + (tbAvans.Text).Replace(",", ".") + ",";
                    sSQL += "" + (balance.ToString()).Replace(",", ".") + ",";
                    sSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                    sSQL += "N'Количество: " + tbAmount.Text + "; Цена: " + tbPrice.Text + "; Всего: " + lblSum.Text + ";'";
                    sSQL += ");";//insert kasa

                    MySqlCommand insert = new MySqlCommand(sSQL, conn);

                    int rowAffected = insert.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Сохранено!", "Касса", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            catch (Exception ex)
            { conn.Close(); MessageBox.Show("Ошибка" + ex); }
                
                
            }
            else if (Convert.ToInt32(tbAvans.Text) == Convert.ToInt32(lblSum.Text))
            {
                string sSQL = "INSERT INTO `case`(`casename`,`gelirname`,`gelirsum`,`balance`,`takvim`,`description`)VALUES(";
                sSQL += "N'" + tbOrderName.Text + " - " + dtManager.Rows[0]["namesur"].ToString() + "',";
                sSQL += "N'" + tbOrderName.Text + " - " + dtManager.Rows[0]["namesur"].ToString() + "',";
                sSQL += "" + (tbAvans.Text).Replace(",", ".") + ",";
                sSQL += "" + (balance.ToString()).Replace(",", ".") + ",";
                sSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                sSQL += "N'Количество: " + tbAmount.Text + "; Цена: " + tbPrice.Text + "; Всего: " + lblSum.Text + ";',";
                sSQL += ");";//insert kasa

                MySqlCommand insert = new MySqlCommand(sSQL, conn);

                int rowAffected = insert.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Сохранено!", "Касса", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //insert kasa
            }
            //insert order to designer
        }

        private void показатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }
       
    }     
}
