namespace EFClassGenerator
{
    partial class frmLogin
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.cmdGetCatalog = new System.Windows.Forms.Button();
            this.coCatalog = new System.Windows.Forms.ComboBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdTestConnection = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chIntegratedSecurity = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdExit = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdGetCatalog
            // 
            this.cmdGetCatalog.Enabled = false;
            this.cmdGetCatalog.Image = ((System.Drawing.Image)(resources.GetObject("cmdGetCatalog.Image")));
            this.cmdGetCatalog.Location = new System.Drawing.Point(395, 208);
            this.cmdGetCatalog.Name = "cmdGetCatalog";
            this.cmdGetCatalog.Size = new System.Drawing.Size(30, 21);
            this.cmdGetCatalog.TabIndex = 11;
            this.toolTip1.SetToolTip(this.cmdGetCatalog, "Reload list of databases");
            this.cmdGetCatalog.UseVisualStyleBackColor = true;
            this.cmdGetCatalog.Click += new System.EventHandler(this.cmdGetCatalog_Click);
            // 
            // coCatalog
            // 
            this.coCatalog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coCatalog.Enabled = false;
            this.coCatalog.FormattingEnabled = true;
            this.coCatalog.Location = new System.Drawing.Point(94, 208);
            this.coCatalog.Name = "coCatalog";
            this.coCatalog.Size = new System.Drawing.Size(295, 21);
            this.coCatalog.TabIndex = 10;
            this.toolTip1.SetToolTip(this.coCatalog, "List of found databases. Select one database you want to create EF data classes");
            // 
            // cmdSave
            // 
            this.cmdSave.Enabled = false;
            this.cmdSave.Location = new System.Drawing.Point(351, 235);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(74, 43);
            this.cmdSave.TabIndex = 12;
            this.cmdSave.Text = "Next >";
            this.toolTip1.SetToolTip(this.cmdSave, "Use this database and go on with next step");
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdNext_Click);
            // 
            // cmdTestConnection
            // 
            this.cmdTestConnection.Location = new System.Drawing.Point(94, 130);
            this.cmdTestConnection.Name = "cmdTestConnection";
            this.cmdTestConnection.Size = new System.Drawing.Size(331, 53);
            this.cmdTestConnection.TabIndex = 8;
            this.cmdTestConnection.Text = "Test Connection";
            this.toolTip1.SetToolTip(this.cmdTestConnection, "Test the connection to the SQL server and go further");
            this.cmdTestConnection.UseVisualStyleBackColor = true;
            this.cmdTestConnection.Click += new System.EventHandler(this.cmdTestConnection_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(94, 104);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(331, 20);
            this.txtPassword.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txtPassword, "Password of SQL server user");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(94, 78);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(331, 20);
            this.txtUsername.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtUsername, "Username of SQL server");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Username";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Catalog";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(94, 29);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(331, 20);
            this.txtServer.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtServer, "Enter SQL server / Instance name or IP address (localhost\\SQLEXPRESS)");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "SQL Server";
            // 
            // chIntegratedSecurity
            // 
            this.chIntegratedSecurity.AutoSize = true;
            this.chIntegratedSecurity.Location = new System.Drawing.Point(94, 55);
            this.chIntegratedSecurity.Name = "chIntegratedSecurity";
            this.chIntegratedSecurity.Size = new System.Drawing.Size(141, 17);
            this.chIntegratedSecurity.TabIndex = 3;
            this.chIntegratedSecurity.Text = "Windows Authentication";
            this.toolTip1.SetToolTip(this.chIntegratedSecurity, "Use current windows user for log on in SQL server");
            this.chIntegratedSecurity.UseVisualStyleBackColor = true;
            this.chIntegratedSecurity.CheckedChanged += new System.EventHandler(this.OnclickWindowsAuthentication);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Tip";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.chIntegratedSecurity);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cmdGetCatalog);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.coCatalog);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmdSave);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.cmdTestConnection);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 294);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SQL Server Connection";
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(12, 312);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(75, 23);
            this.cmdExit.TabIndex = 13;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 345);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.Text = "EF Code Generator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button cmdGetCatalog;
        internal System.Windows.Forms.ComboBox coCatalog;
        internal System.Windows.Forms.Button cmdSave;
        internal System.Windows.Forms.Button cmdTestConnection;
        internal System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox txtUsername;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.TextBox txtServer;
        internal System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chIntegratedSecurity;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdExit;
    }
}

