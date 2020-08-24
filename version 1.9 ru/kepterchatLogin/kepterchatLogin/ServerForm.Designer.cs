namespace kepterchatLogin
{
    partial class ServerForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.stopListnerButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.startListnerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Conencted Clients List";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 101);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(229, 225);
            this.listBox1.TabIndex = 16;
            // 
            // stopListnerButton
            // 
            this.stopListnerButton.Location = new System.Drawing.Point(12, 41);
            this.stopListnerButton.Name = "stopListnerButton";
            this.stopListnerButton.Size = new System.Drawing.Size(86, 23);
            this.stopListnerButton.TabIndex = 15;
            this.stopListnerButton.Text = "Stop Listener";
            this.stopListnerButton.UseVisualStyleBackColor = true;
            this.stopListnerButton.Click += new System.EventHandler(this.stopListnerButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Running";
            // 
            // startListnerButton
            // 
            this.startListnerButton.Location = new System.Drawing.Point(12, 12);
            this.startListnerButton.Name = "startListnerButton";
            this.startListnerButton.Size = new System.Drawing.Size(86, 23);
            this.startListnerButton.TabIndex = 9;
            this.startListnerButton.Text = "Start Listener";
            this.startListnerButton.UseVisualStyleBackColor = true;
            this.startListnerButton.Click += new System.EventHandler(this.startListnerButton_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 339);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.stopListnerButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.startListnerButton);
            this.Name = "ServerForm";
            this.Text = "ServerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button stopListnerButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button startListnerButton;
    }
}