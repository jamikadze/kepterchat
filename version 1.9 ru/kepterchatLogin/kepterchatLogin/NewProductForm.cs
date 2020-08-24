using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kepterchatLogin
{
    public partial class NewProductForm : Form
    {
        MySqlConnection conn;
        public string  name;
        public bool upd;
        DataTable dt = new DataTable();
        public int id;
        public NewProductForm(string servname)
        {
            InitializeComponent();
            conn = new MySqlConnection("SERVER="+servname+";DATABASE=haktan_kepterchat;UID=haktan;PASSWORD=tamyr;CharSet=utf8;");
            this.AcceptButton = btnOk; 
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
              Close();
        }

        private void btnState_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (upd)
            { UpdateGoods(); }
            else 
            saveNewRow();                       
        }
        private void UpdateGoods()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
               
                string sSQL = "update `goods` set `code`=";
                sSQL += "'" + tbCode.Text + "',";
                sSQL += "`name`=N'" + tbGoodName.Text + "',";
                sSQL += "`measure`=N'" + tbMeasure.Text + "',";               
                sSQL += "`description`=N'" + tbDescription.Text + "',";
                sSQL += "`photo`=@img";
                sSQL += " where `idgoods`=" + id + ";";                
                MySqlCommand update = new MySqlCommand(sSQL, conn);

                MySqlParameter imgParam1 = update.Parameters.AddWithValue("@img", ConvertToByte(pbProduct.Image));               
                imgParam1.Size = ConvertToByte(pbProduct.Image).Length;

                int rowAffected = update.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Изменено!", "Продукты", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbCode.Text = "";
                tbDescription.Text = "";
                tbGoodName.Text = "";
                tbMeasure.Text = "";
                pbProduct.Image = null;
            }
            catch (Exception ex)
            { conn.Close(); MessageBox.Show("Error" + ex); }            
        }
        
        
        private void saveNewRow()
        {
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            try
            {
               
                string sSQL = "INSERT INTO `goods`(`code`,`name`,`measure`,`description`,`photo`)VALUES(";
                sSQL += "'" + tbCode.Text + "',";
                sSQL += "N'" + tbGoodName.Text + "',";
                sSQL += "N'" + tbMeasure.Text + "',";               
                sSQL += "N'" + tbDescription.Text + "',";
                sSQL += "@img";
                sSQL += ");";
                MySqlCommand insert = new MySqlCommand(sSQL, conn);

                MySqlParameter imgParam1 = insert.Parameters.AddWithValue("@img", ConvertToByte(pbProduct.Image));               
                imgParam1.Size = ConvertToByte(pbProduct.Image).Length;

                int rowAffected = insert.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Сохранено!", "Продукты", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbCode.Text = "";
                tbDescription.Text = "";
                tbGoodName.Text = "";
                tbMeasure.Text = "";
                pbProduct.Image = null;
            }
            catch (Exception ex)
            { conn.Close(); MessageBox.Show("Error" + ex); }            
        }
        byte[] ConvertToByte(System.Drawing.Image InputImage)
        {
            Bitmap bmpImage = new Bitmap(InputImage);
            MemoryStream myStream = new MemoryStream();
            bmpImage.Save(myStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imageAsBytes = myStream.ToArray();

            return imageAsBytes;
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPEG file | *.jpg|PNG file | *.png|BMP file | *.bmp|GIF file | *.gif";
            ofd.Title = "ВЫберите фотографию:";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {                              
                pbProduct.Image = System.Drawing.Image.FromFile(ofd.FileName);             
            } 
        }

        private void NewProductForm_Load(object sender, EventArgs e)
        {
            if (upd)
            {
                dt.Clear();
                MySqlDataAdapter da = new MySqlDataAdapter("Select idGoods,code, name,description,measure,photo from goods where code='" + name + "'", conn);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                { 
                    tbCode.Text = (string)dt.Rows[0]["code"];
                    tbGoodName.Text = (string)dt.Rows[0]["name"];
                    tbMeasure.Text = (string)dt.Rows[0]["measure"];
                    tbDescription.Text = (string)dt.Rows[0]["description"];                    
                    byte[] FetchedImgBytes = (byte[])dt.Rows[0]["photo"];
                    MemoryStream streamImg = new MemoryStream(FetchedImgBytes);
                    System.Drawing.Image FetchedImage;
                    FetchedImage = System.Drawing.Image.FromStream(streamImg);
                    pbProduct.Image = FetchedImage;
                                    
                }

            }
        }

    }
}
