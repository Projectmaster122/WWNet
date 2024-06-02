namespace WWNet_Browser
{
    partial class Form1
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
            this.Page = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Forward = new System.Windows.Forms.PictureBox();
            this.Refresh = new System.Windows.Forms.PictureBox();
            this.Back = new System.Windows.Forms.PictureBox();
            this.AddressBar = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Forward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Refresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Back)).BeginInit();
            this.SuspendLayout();
            // 
            // Page
            // 
            this.Page.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Page.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Page.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Page.Location = new System.Drawing.Point(-2, 80);
            this.Page.Margin = new System.Windows.Forms.Padding(6);
            this.Page.Name = "Page";
            this.Page.Size = new System.Drawing.Size(2108, 1118);
            this.Page.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.Forward);
            this.panel1.Controls.Add(this.AddressBar);
            this.panel1.Controls.Add(this.Back);
            this.panel1.Controls.Add(this.Refresh);
            this.panel1.Location = new System.Drawing.Point(-13, -8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2131, 90);
            this.panel1.TabIndex = 1;
            // 
            // Forward
            // 
            this.Forward.Image = global::WWNet_Browser.Properties.Resources.forward;
            this.Forward.Location = new System.Drawing.Point(96, 21);
            this.Forward.Name = "Forward";
            this.Forward.Size = new System.Drawing.Size(40, 40);
            this.Forward.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Forward.TabIndex = 1;
            this.Forward.TabStop = false;
            this.Forward.Click += new System.EventHandler(this.Forward_Click);
            // 
            // Refresh
            // 
            this.Refresh.Image = global::WWNet_Browser.Properties.Resources.refresh;
            this.Refresh.Location = new System.Drawing.Point(157, 21);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(40, 40);
            this.Refresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Refresh.TabIndex = 2;
            this.Refresh.TabStop = false;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // Back
            // 
            this.Back.Image = global::WWNet_Browser.Properties.Resources.back;
            this.Back.Location = new System.Drawing.Point(33, 21);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(40, 40);
            this.Back.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Back.TabIndex = 0;
            this.Back.TabStop = false;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // AddressBar
            // 
            this.AddressBar.AllowDrop = true;
            this.AddressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressBar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddressBar.Location = new System.Drawing.Point(219, 16);
            this.AddressBar.Name = "AddressBar";
            this.AddressBar.Size = new System.Drawing.Size(1868, 50);
            this.AddressBar.TabIndex = 3;
            this.AddressBar.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.AddressBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressBar_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2105, 1199);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Page);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "WWeb Browser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Forward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Refresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Back)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel Page;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox Refresh;
        private System.Windows.Forms.PictureBox Forward;
        private System.Windows.Forms.PictureBox Back;
        private System.Windows.Forms.TextBox AddressBar;
    }
}

