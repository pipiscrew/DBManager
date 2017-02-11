namespace DBManager
{
    partial class frmGeneratePHPpages
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
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPortalMerge = new System.Windows.Forms.TextBox();
            this.txt_detail = new System.Windows.Forms.TextBox();
            this.txt_pg = new System.Windows.Forms.TextBox();
            this.txt_table = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSelectDetail = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSelectPagination = new System.Windows.Forms.TextBox();
            this.buttonSELECT = new System.Windows.Forms.Button();
            this.txtTable = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSelectResult = new System.Windows.Forms.TextBox();
            this.txtSelect = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.txt_save = new System.Windows.Forms.TextBox();
            this.txt_delete = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.linkLabel3.Location = new System.Drawing.Point(337, 564);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(220, 16);
            this.linkLabel3.TabIndex = 29;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "procedure save - PHP call example";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.linkLabel2.Location = new System.Drawing.Point(18, 564);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(286, 16);
            this.linkLabel2.TabIndex = 28;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "procedure save (insert/update in one) example";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.linkLabel1.Location = new System.Drawing.Point(178, 427);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(123, 16);
            this.linkLabel1.TabIndex = 27;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "portal.php example";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button3.Location = new System.Drawing.Point(747, 421);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 28);
            this.button3.TabIndex = 26;
            this.button3.Text = "copy";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.label6.Location = new System.Drawing.Point(15, 427);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 17);
            this.label6.TabIndex = 25;
            this.label6.Text = "merge to portal.php :";
            // 
            // txtPortalMerge
            // 
            this.txtPortalMerge.AllowDrop = true;
            this.txtPortalMerge.BackColor = System.Drawing.Color.White;
            this.txtPortalMerge.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtPortalMerge.Location = new System.Drawing.Point(18, 447);
            this.txtPortalMerge.MaxLength = 0;
            this.txtPortalMerge.Multiline = true;
            this.txtPortalMerge.Name = "txtPortalMerge";
            this.txtPortalMerge.ReadOnly = true;
            this.txtPortalMerge.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPortalMerge.Size = new System.Drawing.Size(802, 72);
            this.txtPortalMerge.TabIndex = 24;
            // 
            // txt_detail
            // 
            this.txt_detail.AllowDrop = true;
            this.txt_detail.BackColor = System.Drawing.Color.White;
            this.txt_detail.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txt_detail.Location = new System.Drawing.Point(352, 317);
            this.txt_detail.Name = "txt_detail";
            this.txt_detail.ReadOnly = true;
            this.txt_detail.Size = new System.Drawing.Size(301, 25);
            this.txt_detail.TabIndex = 23;
            // 
            // txt_pg
            // 
            this.txt_pg.AllowDrop = true;
            this.txt_pg.BackColor = System.Drawing.Color.White;
            this.txt_pg.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txt_pg.Location = new System.Drawing.Point(352, 216);
            this.txt_pg.Name = "txt_pg";
            this.txt_pg.ReadOnly = true;
            this.txt_pg.Size = new System.Drawing.Size(301, 25);
            this.txt_pg.TabIndex = 22;
            // 
            // txt_table
            // 
            this.txt_table.AllowDrop = true;
            this.txt_table.BackColor = System.Drawing.Color.White;
            this.txt_table.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txt_table.Location = new System.Drawing.Point(352, 113);
            this.txt_table.Name = "txt_table";
            this.txt_table.ReadOnly = true;
            this.txt_table.Size = new System.Drawing.Size(301, 25);
            this.txt_table.TabIndex = 21;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button2.Location = new System.Drawing.Point(747, 317);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 28);
            this.button2.TabIndex = 20;
            this.button2.Text = "copy";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.label5.Location = new System.Drawing.Point(15, 323);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(273, 17);
            this.label5.TabIndex = 19;
            this.label5.Text = "SELECT Detail (ex. tab_customers_detail.php) :";
            // 
            // txtSelectDetail
            // 
            this.txtSelectDetail.AllowDrop = true;
            this.txtSelectDetail.BackColor = System.Drawing.Color.White;
            this.txtSelectDetail.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtSelectDetail.Location = new System.Drawing.Point(18, 343);
            this.txtSelectDetail.MaxLength = 0;
            this.txtSelectDetail.Multiline = true;
            this.txtSelectDetail.Name = "txtSelectDetail";
            this.txtSelectDetail.ReadOnly = true;
            this.txtSelectDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSelectDetail.Size = new System.Drawing.Size(802, 72);
            this.txtSelectDetail.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button1.Location = new System.Drawing.Point(747, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 17;
            this.button1.Text = "copy";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.label4.Location = new System.Drawing.Point(15, 222);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(331, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "SELECT Pagination (ex. tab_customers_pagination.php) :";
            // 
            // txtSelectPagination
            // 
            this.txtSelectPagination.AllowDrop = true;
            this.txtSelectPagination.BackColor = System.Drawing.Color.White;
            this.txtSelectPagination.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtSelectPagination.Location = new System.Drawing.Point(18, 242);
            this.txtSelectPagination.MaxLength = 0;
            this.txtSelectPagination.Multiline = true;
            this.txtSelectPagination.Name = "txtSelectPagination";
            this.txtSelectPagination.ReadOnly = true;
            this.txtSelectPagination.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSelectPagination.Size = new System.Drawing.Size(802, 72);
            this.txtSelectPagination.TabIndex = 3;
            // 
            // buttonSELECT
            // 
            this.buttonSELECT.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.buttonSELECT.Location = new System.Drawing.Point(747, 113);
            this.buttonSELECT.Name = "buttonSELECT";
            this.buttonSELECT.Size = new System.Drawing.Size(75, 28);
            this.buttonSELECT.TabIndex = 14;
            this.buttonSELECT.Text = "copy";
            this.buttonSELECT.UseVisualStyleBackColor = true;
            this.buttonSELECT.Click += new System.EventHandler(this.buttonSELECT_Click);
            // 
            // txtTable
            // 
            this.txtTable.AllowDrop = true;
            this.txtTable.BackColor = System.Drawing.Color.White;
            this.txtTable.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtTable.Location = new System.Drawing.Point(521, 7);
            this.txtTable.Name = "txtTable";
            this.txtTable.Size = new System.Drawing.Size(301, 25);
            this.txtTable.TabIndex = 0;
            this.txtTable.TextChanged += new System.EventHandler(this.txtTable_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(429, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Table Name :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.label2.Location = new System.Drawing.Point(17, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(275, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "SELECT Fields Result (ex. tab_customers.php) :";
            // 
            // txtSelectResult
            // 
            this.txtSelectResult.AllowDrop = true;
            this.txtSelectResult.BackColor = System.Drawing.Color.White;
            this.txtSelectResult.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtSelectResult.Location = new System.Drawing.Point(20, 139);
            this.txtSelectResult.MaxLength = 0;
            this.txtSelectResult.Multiline = true;
            this.txtSelectResult.Name = "txtSelectResult";
            this.txtSelectResult.ReadOnly = true;
            this.txtSelectResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSelectResult.Size = new System.Drawing.Size(802, 72);
            this.txtSelectResult.TabIndex = 2;
            // 
            // txtSelect
            // 
            this.txtSelect.AllowDrop = true;
            this.txtSelect.BackColor = System.Drawing.Color.White;
            this.txtSelect.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtSelect.Location = new System.Drawing.Point(20, 35);
            this.txtSelect.Multiline = true;
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSelect.Size = new System.Drawing.Size(802, 72);
            this.txtSelect.TabIndex = 1;
            this.txtSelect.TextChanged += new System.EventHandler(this.txtSelect_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(17, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "SELECT Fields *ONLY* (ex. custname,custtel) :";
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.linkLabel4.Location = new System.Drawing.Point(592, 564);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(228, 16);
            this.linkLabel4.TabIndex = 30;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "procedure delete - PHP call example";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // txt_save
            // 
            this.txt_save.AllowDrop = true;
            this.txt_save.BackColor = System.Drawing.Color.White;
            this.txt_save.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txt_save.Location = new System.Drawing.Point(21, 525);
            this.txt_save.Name = "txt_save";
            this.txt_save.ReadOnly = true;
            this.txt_save.Size = new System.Drawing.Size(301, 25);
            this.txt_save.TabIndex = 31;
            // 
            // txt_delete
            // 
            this.txt_delete.AllowDrop = true;
            this.txt_delete.BackColor = System.Drawing.Color.White;
            this.txt_delete.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txt_delete.Location = new System.Drawing.Point(519, 525);
            this.txt_delete.Name = "txt_delete";
            this.txt_delete.ReadOnly = true;
            this.txt_delete.Size = new System.Drawing.Size(301, 25);
            this.txt_delete.TabIndex = 32;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button4.Location = new System.Drawing.Point(368, 525);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(98, 28);
            this.button4.TabIndex = 33;
            this.button4.Text = "template ZIP";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // frmGeneratePHPpages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 584);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txt_delete);
            this.Controls.Add(this.txt_save);
            this.Controls.Add(this.linkLabel4);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPortalMerge);
            this.Controls.Add(this.txt_detail);
            this.Controls.Add(this.txt_pg);
            this.Controls.Add(this.txt_table);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSelectDetail);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSelectPagination);
            this.Controls.Add(this.buttonSELECT);
            this.Controls.Add(this.txtTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSelectResult);
            this.Controls.Add(this.txtSelect);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmGeneratePHPpages";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GeneratePHPpages";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSelectResult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTable;
        private System.Windows.Forms.Button buttonSELECT;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSelectPagination;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSelectDetail;
        private System.Windows.Forms.TextBox txt_table;
        private System.Windows.Forms.TextBox txt_pg;
        private System.Windows.Forms.TextBox txt_detail;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPortalMerge;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.TextBox txt_save;
        private System.Windows.Forms.TextBox txt_delete;
        private System.Windows.Forms.Button button4;
    }
}