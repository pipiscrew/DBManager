namespace DBManager
{
    partial class frmSQLcopyrows
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSourceServer = new System.Windows.Forms.ComboBox();
            this.cmbFROM = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbTO = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DG = new System.Windows.Forms.DataGridView();
            this.btnCopyRows = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.chkIDENTITY = new System.Windows.Forms.CheckBox();
            this.txtCustomSQL = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.mySQLPHPtransLBL = new System.Windows.Forms.LinkLabel();
            this.chkMYSQLtunnelBATCH = new System.Windows.Forms.CheckBox();
            this.lblMYSQLtunnel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DG)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source Server :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label2.Location = new System.Drawing.Point(12, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(248, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Destination Server :  (existing connection)";
            // 
            // cmbSourceServer
            // 
            this.cmbSourceServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceServer.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.cmbSourceServer.FormattingEnabled = true;
            this.cmbSourceServer.Location = new System.Drawing.Point(139, 38);
            this.cmbSourceServer.Name = "cmbSourceServer";
            this.cmbSourceServer.Size = new System.Drawing.Size(400, 24);
            this.cmbSourceServer.TabIndex = 2;
            this.cmbSourceServer.SelectedIndexChanged += new System.EventHandler(this.cmbSourceServer_SelectedIndexChanged);
            // 
            // cmbFROM
            // 
            this.cmbFROM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFROM.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.cmbFROM.FormattingEnabled = true;
            this.cmbFROM.Location = new System.Drawing.Point(101, 83);
            this.cmbFROM.Name = "cmbFROM";
            this.cmbFROM.Size = new System.Drawing.Size(182, 24);
            this.cmbFROM.TabIndex = 4;
            this.cmbFROM.SelectedIndexChanged += new System.EventHandler(this.cmbFROM_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label3.Location = new System.Drawing.Point(16, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "From table :";
            // 
            // cmbTO
            // 
            this.cmbTO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTO.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.cmbTO.FormattingEnabled = true;
            this.cmbTO.Location = new System.Drawing.Point(412, 83);
            this.cmbTO.Name = "cmbTO";
            this.cmbTO.Size = new System.Drawing.Size(182, 24);
            this.cmbTO.TabIndex = 6;
            this.cmbTO.SelectedIndexChanged += new System.EventHandler(this.cmbTO_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label4.Location = new System.Drawing.Point(343, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "To table :";
            // 
            // DG
            // 
            this.DG.AllowUserToAddRows = false;
            this.DG.AllowUserToDeleteRows = false;
            this.DG.AllowUserToResizeColumns = false;
            this.DG.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.DG.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DG.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DG.Location = new System.Drawing.Point(15, 213);
            this.DG.MultiSelect = false;
            this.DG.Name = "DG";
            this.DG.RowHeadersVisible = false;
            this.DG.RowHeadersWidth = 20;
            this.DG.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DG.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.DG.RowTemplate.Height = 25;
            this.DG.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DG.Size = new System.Drawing.Size(579, 243);
            this.DG.TabIndex = 16;
            // 
            // btnCopyRows
            // 
            this.btnCopyRows.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.btnCopyRows.Location = new System.Drawing.Point(455, 462);
            this.btnCopyRows.Name = "btnCopyRows";
            this.btnCopyRows.Size = new System.Drawing.Size(139, 37);
            this.btnCopyRows.TabIndex = 18;
            this.btnCopyRows.Text = "copy rows";
            this.btnCopyRows.UseVisualStyleBackColor = true;
            this.btnCopyRows.Click += new System.EventHandler(this.btnCopyRows_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button1.Location = new System.Drawing.Point(12, 462);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 37);
            this.button1.TabIndex = 19;
            this.button1.Text = "back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkIDENTITY
            // 
            this.chkIDENTITY.AutoSize = true;
            this.chkIDENTITY.BackColor = System.Drawing.Color.Transparent;
            this.chkIDENTITY.Location = new System.Drawing.Point(181, 473);
            this.chkIDENTITY.Name = "chkIDENTITY";
            this.chkIDENTITY.Size = new System.Drawing.Size(244, 17);
            this.chkIDENTITY.TabIndex = 20;
            this.chkIDENTITY.Text = "SET IDENTITY_INSERT ON (write to ID field)";
            this.chkIDENTITY.UseVisualStyleBackColor = false;
            this.chkIDENTITY.Visible = false;
            // 
            // txtCustomSQL
            // 
            this.txtCustomSQL.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtCustomSQL.Location = new System.Drawing.Point(120, 127);
            this.txtCustomSQL.Multiline = true;
            this.txtCustomSQL.Name = "txtCustomSQL";
            this.txtCustomSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCustomSQL.Size = new System.Drawing.Size(389, 68);
            this.txtCustomSQL.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label5.Location = new System.Drawing.Point(16, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 16);
            this.label5.TabIndex = 22;
            this.label5.Text = "Custom Query :";
            // 
            // btnHelp
            // 
            this.btnHelp.Image = global::DBManager.Properties.Resources.help;
            this.btnHelp.Location = new System.Drawing.Point(555, 30);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(39, 32);
            this.btnHelp.TabIndex = 24;
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Visible = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::DBManager.Properties.Resources.refresh24;
            this.btnRefresh.Location = new System.Drawing.Point(555, 127);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(39, 32);
            this.btnRefresh.TabIndex = 23;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Visible = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // mySQLPHPtransLBL
            // 
            this.mySQLPHPtransLBL.AutoSize = true;
            this.mySQLPHPtransLBL.BackColor = System.Drawing.Color.Transparent;
            this.mySQLPHPtransLBL.Location = new System.Drawing.Point(533, 182);
            this.mySQLPHPtransLBL.Name = "mySQLPHPtransLBL";
            this.mySQLPHPtransLBL.Size = new System.Drawing.Size(61, 13);
            this.mySQLPHPtransLBL.TabIndex = 25;
            this.mySQLPHPtransLBL.TabStop = true;
            this.mySQLPHPtransLBL.Text = "tunnel PHP";
            this.mySQLPHPtransLBL.Visible = false;
            this.mySQLPHPtransLBL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // chkMYSQLtunnelBATCH
            // 
            this.chkMYSQLtunnelBATCH.AutoSize = true;
            this.chkMYSQLtunnelBATCH.BackColor = System.Drawing.Color.Transparent;
            this.chkMYSQLtunnelBATCH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.chkMYSQLtunnelBATCH.Location = new System.Drawing.Point(191, 496);
            this.chkMYSQLtunnelBATCH.Name = "chkMYSQLtunnelBATCH";
            this.chkMYSQLtunnelBATCH.Size = new System.Drawing.Size(225, 20);
            this.chkMYSQLtunnelBATCH.TabIndex = 26;
            this.chkMYSQLtunnelBATCH.Text = "batch import (900records / POST)";
            this.chkMYSQLtunnelBATCH.UseVisualStyleBackColor = false;
            this.chkMYSQLtunnelBATCH.Visible = false;
            this.chkMYSQLtunnelBATCH.CheckedChanged += new System.EventHandler(this.chkMYSQLtunnelBATCH_CheckedChanged);
            // 
            // lblMYSQLtunnel
            // 
            this.lblMYSQLtunnel.AutoSize = true;
            this.lblMYSQLtunnel.BackColor = System.Drawing.Color.Transparent;
            this.lblMYSQLtunnel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lblMYSQLtunnel.ForeColor = System.Drawing.Color.Red;
            this.lblMYSQLtunnel.Location = new System.Drawing.Point(30, 64);
            this.lblMYSQLtunnel.Name = "lblMYSQLtunnel";
            this.lblMYSQLtunnel.Size = new System.Drawing.Size(546, 16);
            this.lblMYSQLtunnel.TabIndex = 27;
            this.lblMYSQLtunnel.Text = "on destination mysql dont use boolean field, alo if you dont use all the fields u" +
                "se custom query";
            this.lblMYSQLtunnel.Visible = false;
            // 
            // frmSQLcopyrows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 510);
            this.Controls.Add(this.lblMYSQLtunnel);
            this.Controls.Add(this.chkMYSQLtunnelBATCH);
            this.Controls.Add(this.mySQLPHPtransLBL);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCustomSQL);
            this.Controls.Add(this.chkIDENTITY);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCopyRows);
            this.Controls.Add(this.DG);
            this.Controls.Add(this.cmbTO);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbFROM);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbSourceServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSQLcopyrows";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "copy rows from another dbase";
            this.Load += new System.EventHandler(this.frmSQLcopyrows_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSourceServer;
        private System.Windows.Forms.ComboBox cmbFROM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbTO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView DG;
        private System.Windows.Forms.Button btnCopyRows;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkIDENTITY;
        private System.Windows.Forms.TextBox txtCustomSQL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.LinkLabel mySQLPHPtransLBL;
        private System.Windows.Forms.CheckBox chkMYSQLtunnelBATCH;
        private System.Windows.Forms.Label lblMYSQLtunnel;
        
    }
}