namespace prjIcedOutWheelz
{
    partial class frmRegistreer
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
            this.btnaccount = new System.Windows.Forms.Button();
            this.txtwachtwoord = new System.Windows.Forms.TextBox();
            this.txtherhaal = new System.Windows.Forms.TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.txtadres = new System.Windows.Forms.TextBox();
            this.txtstraatnr = new System.Windows.Forms.TextBox();
            this.txttelefoon = new System.Windows.Forms.TextBox();
            this.txtemail = new System.Windows.Forms.TextBox();
            this.txtvoornaam = new System.Windows.Forms.TextBox();
            this.txtnaam = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnaccount
            // 
            this.btnaccount.BackColor = System.Drawing.Color.Cyan;
            this.btnaccount.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnaccount.FlatAppearance.BorderSize = 4;
            this.btnaccount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnaccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnaccount.Font = new System.Drawing.Font("Bell MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnaccount.Location = new System.Drawing.Point(56, 416);
            this.btnaccount.Name = "btnaccount";
            this.btnaccount.Size = new System.Drawing.Size(168, 34);
            this.btnaccount.TabIndex = 9;
            this.btnaccount.Text = "Maak account aan";
            this.btnaccount.UseVisualStyleBackColor = false;
            this.btnaccount.Click += new System.EventHandler(this.btnaccount_Click);
            // 
            // txtwachtwoord
            // 
            this.txtwachtwoord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtwachtwoord.ForeColor = System.Drawing.Color.Black;
            this.txtwachtwoord.Location = new System.Drawing.Point(56, 359);
            this.txtwachtwoord.Name = "txtwachtwoord";
            this.txtwachtwoord.Size = new System.Drawing.Size(168, 20);
            this.txtwachtwoord.TabIndex = 7;
            this.txtwachtwoord.Enter += new System.EventHandler(this.txtwachtwoord_Enter);
            this.txtwachtwoord.Leave += new System.EventHandler(this.txtwachtwoord_Leave);
            // 
            // txtherhaal
            // 
            this.txtherhaal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtherhaal.ForeColor = System.Drawing.Color.Black;
            this.txtherhaal.Location = new System.Drawing.Point(56, 388);
            this.txtherhaal.Name = "txtherhaal";
            this.txtherhaal.Size = new System.Drawing.Size(168, 20);
            this.txtherhaal.TabIndex = 8;
            this.txtherhaal.Enter += new System.EventHandler(this.txtherhaal_Enter);
            this.txtherhaal.Leave += new System.EventHandler(this.txtherhaal_Leave);
            // 
            // picLogo
            // 
            this.picLogo.Image = global::prjIcedOutWheelz.Properties.Resources.Logo;
            this.picLogo.Location = new System.Drawing.Point(56, 12);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(168, 167);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 5;
            this.picLogo.TabStop = false;
            // 
            // txtadres
            // 
            this.txtadres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtadres.ForeColor = System.Drawing.Color.Black;
            this.txtadres.Location = new System.Drawing.Point(56, 330);
            this.txtadres.Name = "txtadres";
            this.txtadres.Size = new System.Drawing.Size(168, 20);
            this.txtadres.TabIndex = 6;
            this.txtadres.Enter += new System.EventHandler(this.txtadres_Enter);
            this.txtadres.Leave += new System.EventHandler(this.txtadres_Leave);
            // 
            // txtstraatnr
            // 
            this.txtstraatnr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtstraatnr.ForeColor = System.Drawing.Color.Black;
            this.txtstraatnr.Location = new System.Drawing.Point(56, 301);
            this.txtstraatnr.Name = "txtstraatnr";
            this.txtstraatnr.Size = new System.Drawing.Size(168, 20);
            this.txtstraatnr.TabIndex = 5;
            this.txtstraatnr.Enter += new System.EventHandler(this.txtstraatnr_Enter);
            this.txtstraatnr.Leave += new System.EventHandler(this.txtstraatnr_Leave);
            // 
            // txttelefoon
            // 
            this.txttelefoon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txttelefoon.ForeColor = System.Drawing.Color.Black;
            this.txttelefoon.Location = new System.Drawing.Point(56, 272);
            this.txttelefoon.Name = "txttelefoon";
            this.txttelefoon.Size = new System.Drawing.Size(168, 20);
            this.txttelefoon.TabIndex = 4;
            this.txttelefoon.Enter += new System.EventHandler(this.txttelefoon_Enter);
            this.txttelefoon.Leave += new System.EventHandler(this.txttelefoon_Leave);
            // 
            // txtemail
            // 
            this.txtemail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtemail.ForeColor = System.Drawing.Color.Black;
            this.txtemail.Location = new System.Drawing.Point(56, 243);
            this.txtemail.Name = "txtemail";
            this.txtemail.Size = new System.Drawing.Size(168, 20);
            this.txtemail.TabIndex = 3;
            this.txtemail.Enter += new System.EventHandler(this.txtemail_Enter);
            this.txtemail.Leave += new System.EventHandler(this.txtemail_Leave);
            // 
            // txtvoornaam
            // 
            this.txtvoornaam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtvoornaam.ForeColor = System.Drawing.Color.Black;
            this.txtvoornaam.Location = new System.Drawing.Point(56, 214);
            this.txtvoornaam.Name = "txtvoornaam";
            this.txtvoornaam.Size = new System.Drawing.Size(168, 20);
            this.txtvoornaam.TabIndex = 2;
            this.txtvoornaam.Enter += new System.EventHandler(this.txtvoornaam_Enter);
            this.txtvoornaam.Leave += new System.EventHandler(this.txtvoornaam_Leave);
            // 
            // txtnaam
            // 
            this.txtnaam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtnaam.ForeColor = System.Drawing.Color.Black;
            this.txtnaam.Location = new System.Drawing.Point(56, 185);
            this.txtnaam.Name = "txtnaam";
            this.txtnaam.Size = new System.Drawing.Size(168, 20);
            this.txtnaam.TabIndex = 1;
            this.txtnaam.Enter += new System.EventHandler(this.txtnaam_Enter);
            this.txtnaam.Leave += new System.EventHandler(this.txtnaam_Leave);
            // 
            // frmRegistreer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(278, 469);
            this.Controls.Add(this.txtnaam);
            this.Controls.Add(this.txtvoornaam);
            this.Controls.Add(this.txtemail);
            this.Controls.Add(this.txttelefoon);
            this.Controls.Add(this.txtstraatnr);
            this.Controls.Add(this.txtadres);
            this.Controls.Add(this.btnaccount);
            this.Controls.Add(this.txtwachtwoord);
            this.Controls.Add(this.txtherhaal);
            this.Controls.Add(this.picLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmRegistreer";
            this.Text = "frmRegistreer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRegistreer_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnaccount;
        private System.Windows.Forms.TextBox txtwachtwoord;
        private System.Windows.Forms.TextBox txtherhaal;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.TextBox txtadres;
        private System.Windows.Forms.TextBox txtstraatnr;
        private System.Windows.Forms.TextBox txttelefoon;
        private System.Windows.Forms.TextBox txtemail;
        private System.Windows.Forms.TextBox txtvoornaam;
        private System.Windows.Forms.TextBox txtnaam;
    }
}