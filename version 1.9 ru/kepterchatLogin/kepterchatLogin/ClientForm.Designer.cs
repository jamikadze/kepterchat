namespace kepterchatLogin
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.lbUsers = new System.Windows.Forms.ListBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.messagebodytextbox = new System.Windows.Forms.TextBox();
            this.totextbox = new System.Windows.Forms.TextBox();
            this.btnX = new System.Windows.Forms.Button();
            this.btnState = new System.Windows.Forms.Button();
            this.SendMsgButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 21;
            this.listBox1.Location = new System.Drawing.Point(243, 110);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(464, 319);
            this.listBox1.TabIndex = 19;
            // 
            // lbUsers
            // 
            this.lbUsers.BackColor = System.Drawing.SystemColors.Desktop;
            this.lbUsers.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbUsers.ForeColor = System.Drawing.Color.Gold;
            this.lbUsers.FormattingEnabled = true;
            this.lbUsers.ItemHeight = 23;
            this.lbUsers.Location = new System.Drawing.Point(12, 71);
            this.lbUsers.Name = "lbUsers";
            this.lbUsers.Size = new System.Drawing.Size(212, 395);
            this.lbUsers.TabIndex = 21;
            this.lbUsers.SelectedIndexChanged += new System.EventHandler(this.lbUsers_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Century Gothic", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label11.Location = new System.Drawing.Point(21, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 25);
            this.label11.TabIndex = 22;
            this.label11.Text = "Коллеги";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblName.ForeColor = System.Drawing.Color.Gold;
            this.lblName.Location = new System.Drawing.Point(122, 44);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(41, 19);
            this.lblName.TabIndex = 23;
            this.lblName.Text = "Имя";
            // 
            // messagebodytextbox
            // 
            this.messagebodytextbox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.messagebodytextbox.Location = new System.Drawing.Point(243, 439);
            this.messagebodytextbox.Name = "messagebodytextbox";
            this.messagebodytextbox.Size = new System.Drawing.Size(357, 27);
            this.messagebodytextbox.TabIndex = 27;
            // 
            // totextbox
            // 
            this.totextbox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.totextbox.ForeColor = System.Drawing.Color.Orange;
            this.totextbox.Location = new System.Drawing.Point(243, 71);
            this.totextbox.Name = "totextbox";
            this.totextbox.ReadOnly = true;
            this.totextbox.Size = new System.Drawing.Size(218, 27);
            this.totextbox.TabIndex = 25;
            // 
            // btnX
            // 
            this.btnX.BackColor = System.Drawing.Color.Transparent;
            this.btnX.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnX.BackgroundImage")));
            this.btnX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnX.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnX.Location = new System.Drawing.Point(691, 8);
            this.btnX.Name = "btnX";
            this.btnX.Size = new System.Drawing.Size(24, 24);
            this.btnX.TabIndex = 29;
            this.btnX.UseVisualStyleBackColor = false;
            this.btnX.Click += new System.EventHandler(this.btnX_Click);
            // 
            // btnState
            // 
            this.btnState.BackColor = System.Drawing.Color.Transparent;
            this.btnState.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnState.BackgroundImage")));
            this.btnState.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnState.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnState.Location = new System.Drawing.Point(661, 8);
            this.btnState.Name = "btnState";
            this.btnState.Size = new System.Drawing.Size(24, 24);
            this.btnState.TabIndex = 28;
            this.btnState.UseVisualStyleBackColor = false;
            this.btnState.Click += new System.EventHandler(this.btnState_Click);
            // 
            // SendMsgButton
            // 
            this.SendMsgButton.BackColor = System.Drawing.Color.Transparent;
            this.SendMsgButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SendMsgButton.BackgroundImage")));
            this.SendMsgButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SendMsgButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendMsgButton.Font = new System.Drawing.Font("Century Gothic", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SendMsgButton.ForeColor = System.Drawing.Color.Turquoise;
            this.SendMsgButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SendMsgButton.Location = new System.Drawing.Point(607, 436);
            this.SendMsgButton.Margin = new System.Windows.Forms.Padding(4);
            this.SendMsgButton.Name = "SendMsgButton";
            this.SendMsgButton.Size = new System.Drawing.Size(100, 30);
            this.SendMsgButton.TabIndex = 102;
            this.SendMsgButton.Text = "Отправить";
            this.SendMsgButton.UseVisualStyleBackColor = false;
            this.SendMsgButton.Click += new System.EventHandler(this.SendMsgButton_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(730, 483);
            this.Controls.Add(this.SendMsgButton);
            this.Controls.Add(this.btnX);
            this.Controls.Add(this.btnState);
            this.Controls.Add(this.messagebodytextbox);
            this.Controls.Add(this.totextbox);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lbUsers);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClientForm";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox lbUsers;
        public System.Windows.Forms.Label label11;
        public System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox messagebodytextbox;
        private System.Windows.Forms.TextBox totextbox;
        private System.Windows.Forms.Button btnX;
        private System.Windows.Forms.Button btnState;
        private System.Windows.Forms.Button SendMsgButton;
    }
}