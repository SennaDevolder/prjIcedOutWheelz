﻿namespace prjIcedOutWheelz
{
    partial class frmStartscherm
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
            this.lblIcedOutWheelz = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnRegistreer = new System.Windows.Forms.Button();
            this.picBanner = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIcedOutWheelz
            // 
            this.lblIcedOutWheelz.AutoSize = true;
            this.lblIcedOutWheelz.BackColor = System.Drawing.Color.Black;
            this.lblIcedOutWheelz.Font = new System.Drawing.Font("Bell MT", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIcedOutWheelz.ForeColor = System.Drawing.Color.White;
            this.lblIcedOutWheelz.Location = new System.Drawing.Point(416, 115);
            this.lblIcedOutWheelz.Name = "lblIcedOutWheelz";
            this.lblIcedOutWheelz.Size = new System.Drawing.Size(275, 42);
            this.lblIcedOutWheelz.TabIndex = 1;
            this.lblIcedOutWheelz.Text = "Iced Out Wheelz";
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Cyan;
            this.btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnLogin.FlatAppearance.BorderSize = 4;
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Bell MT", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Location = new System.Drawing.Point(152, 297);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(457, 62);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnRegistreer
            // 
            this.btnRegistreer.BackColor = System.Drawing.Color.Cyan;
            this.btnRegistreer.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRegistreer.FlatAppearance.BorderSize = 4;
            this.btnRegistreer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnRegistreer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistreer.Font = new System.Drawing.Font("Bell MT", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistreer.Location = new System.Drawing.Point(198, 381);
            this.btnRegistreer.Name = "btnRegistreer";
            this.btnRegistreer.Size = new System.Drawing.Size(364, 51);
            this.btnRegistreer.TabIndex = 3;
            this.btnRegistreer.Text = "Registreer";
            this.btnRegistreer.UseVisualStyleBackColor = false;
            this.btnRegistreer.Click += new System.EventHandler(this.btnRegistreer_Click);
            // 
            // picBanner
            // 
            this.picBanner.Image = global::prjIcedOutWheelz.Properties.Resources.Banner;
            this.picBanner.Location = new System.Drawing.Point(12, 12);
            this.picBanner.Name = "picBanner";
            this.picBanner.Size = new System.Drawing.Size(735, 250);
            this.picBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBanner.TabIndex = 0;
            this.picBanner.TabStop = false;
            // 
            // frmStartscherm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(760, 467);
            this.Controls.Add(this.btnRegistreer);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblIcedOutWheelz);
            this.Controls.Add(this.picBanner);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmStartscherm";
            this.Text = "Startscherm";
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBanner;
        private System.Windows.Forms.Label lblIcedOutWheelz;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegistreer;
    }
}

