namespace DBManager
{
    partial class frmGeneratePHP_CRUD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGeneratePHP_CRUD));
            this.label3 = new System.Windows.Forms.Label();
            this.cmbTemplate = new System.Windows.Forms.ComboBox();
            this.img_template_preview = new System.Windows.Forms.PictureBox();
            this.chkDATETIME = new System.Windows.Forms.CheckBox();
            this.chkDATE = new System.Windows.Forms.CheckBox();
            this.chkTINYINT = new System.Windows.Forms.CheckBox();
            this.chkBIT = new System.Windows.Forms.CheckBox();
            this.lst = new System.Windows.Forms.ListBox();
            this.button4 = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDATEmalot = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.img_template_preview)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(65)))), ((int)(((byte)(160)))));
            this.label3.Location = new System.Drawing.Point(505, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 17);
            this.label3.TabIndex = 43;
            this.label3.Text = "Template Name :";
            // 
            // cmbTemplate
            // 
            this.cmbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTemplate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.cmbTemplate.FormattingEnabled = true;
            this.cmbTemplate.Items.AddRange(new object[] {
            "AdminLTE",
            "bootstrap-table by wenzhixin",
            "bootstrap-table b5 for API",
            "vue2 with vuetify (nodeJS)",
            "vue3 with vuetify (vanilla)"});
            this.cmbTemplate.Location = new System.Drawing.Point(620, 175);
            this.cmbTemplate.Name = "cmbTemplate";
            this.cmbTemplate.Size = new System.Drawing.Size(191, 24);
            this.cmbTemplate.TabIndex = 42;
            this.cmbTemplate.SelectedIndexChanged += new System.EventHandler(this.cmbTemplate_SelectedIndexChanged);
            // 
            // img_template_preview
            // 
            this.img_template_preview.Image = ((System.Drawing.Image)(resources.GetObject("img_template_preview.Image")));
            this.img_template_preview.Location = new System.Drawing.Point(440, 245);
            this.img_template_preview.Name = "img_template_preview";
            this.img_template_preview.Size = new System.Drawing.Size(371, 151);
            this.img_template_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.img_template_preview.TabIndex = 41;
            this.img_template_preview.TabStop = false;
            // 
            // chkDATETIME
            // 
            this.chkDATETIME.AutoSize = true;
            this.chkDATETIME.BackColor = System.Drawing.Color.Transparent;
            this.chkDATETIME.Checked = true;
            this.chkDATETIME.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDATETIME.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.chkDATETIME.Location = new System.Drawing.Point(2, 149);
            this.chkDATETIME.Name = "chkDATETIME";
            this.chkDATETIME.Size = new System.Drawing.Size(379, 20);
            this.chkDATETIME.TabIndex = 40;
            this.chkDATETIME.Text = "treat DATETIME as datetimer picker (malot.fr.datetimepicker)";
            this.chkDATETIME.UseVisualStyleBackColor = false;
            // 
            // chkDATE
            // 
            this.chkDATE.AutoSize = true;
            this.chkDATE.BackColor = System.Drawing.Color.Transparent;
            this.chkDATE.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.chkDATE.Location = new System.Drawing.Point(2, 97);
            this.chkDATE.Name = "chkDATE";
            this.chkDATE.Size = new System.Drawing.Size(290, 20);
            this.chkDATE.TabIndex = 38;
            this.chkDATE.Text = "treat DATE as date picker (eyecon.datepicker)";
            this.chkDATE.UseVisualStyleBackColor = false;
            this.chkDATE.CheckedChanged += new System.EventHandler(this.chkDATE_CheckedChanged);
            // 
            // chkTINYINT
            // 
            this.chkTINYINT.AutoSize = true;
            this.chkTINYINT.BackColor = System.Drawing.Color.Transparent;
            this.chkTINYINT.Checked = true;
            this.chkTINYINT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTINYINT.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.chkTINYINT.Location = new System.Drawing.Point(2, 72);
            this.chkTINYINT.Name = "chkTINYINT";
            this.chkTINYINT.Size = new System.Drawing.Size(178, 20);
            this.chkTINYINT.TabIndex = 37;
            this.chkTINYINT.Text = "treat TINYINT as checkbox";
            this.chkTINYINT.UseVisualStyleBackColor = false;
            // 
            // chkBIT
            // 
            this.chkBIT.AutoSize = true;
            this.chkBIT.BackColor = System.Drawing.Color.Transparent;
            this.chkBIT.Checked = true;
            this.chkBIT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBIT.Enabled = false;
            this.chkBIT.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.chkBIT.Location = new System.Drawing.Point(2, 47);
            this.chkBIT.Name = "chkBIT";
            this.chkBIT.Size = new System.Drawing.Size(150, 20);
            this.chkBIT.TabIndex = 36;
            this.chkBIT.Text = "treat BIT as checkbox";
            this.chkBIT.UseVisualStyleBackColor = false;
            // 
            // lst
            // 
            this.lst.FormattingEnabled = true;
            this.lst.Location = new System.Drawing.Point(2, 175);
            this.lst.Name = "lst";
            this.lst.Size = new System.Drawing.Size(300, 290);
            this.lst.TabIndex = 35;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button4.Location = new System.Drawing.Point(307, 437);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 28);
            this.button4.TabIndex = 34;
            this.button4.Text = "template ZIP";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.btnExport.Location = new System.Drawing.Point(307, 175);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 34);
            this.btnExport.TabIndex = 20;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtFolder.Location = new System.Drawing.Point(110, 12);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(230, 23);
            this.txtFolder.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(65)))), ((int)(((byte)(160)))));
            this.label2.Location = new System.Drawing.Point(9, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "Folder Name :";
            // 
            // chkDATEmalot
            // 
            this.chkDATEmalot.AutoSize = true;
            this.chkDATEmalot.BackColor = System.Drawing.Color.Transparent;
            this.chkDATEmalot.Checked = true;
            this.chkDATEmalot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDATEmalot.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.chkDATEmalot.Location = new System.Drawing.Point(2, 123);
            this.chkDATEmalot.Name = "chkDATEmalot";
            this.chkDATEmalot.Size = new System.Drawing.Size(295, 20);
            this.chkDATEmalot.TabIndex = 44;
            this.chkDATEmalot.Text = "treat DATE as date picker (malot.fr.datepicker)";
            this.chkDATEmalot.UseVisualStyleBackColor = false;
            this.chkDATEmalot.CheckedChanged += new System.EventHandler(this.chkDATEmalot_CheckedChanged);
            // 
            // frmGeneratePHP_CRUD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 508);
            this.Controls.Add(this.chkDATEmalot);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbTemplate);
            this.Controls.Add(this.img_template_preview);
            this.Controls.Add(this.chkDATETIME);
            this.Controls.Add(this.chkDATE);
            this.Controls.Add(this.chkTINYINT);
            this.Controls.Add(this.chkBIT);
            this.Controls.Add(this.lst);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmGeneratePHP_CRUD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CRUD";
            ((System.ComponentModel.ISupportInitialize)(this.img_template_preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox lst;
        private System.Windows.Forms.CheckBox chkBIT;
        private System.Windows.Forms.CheckBox chkTINYINT;
        private System.Windows.Forms.CheckBox chkDATE;
        private System.Windows.Forms.CheckBox chkDATETIME;
        private System.Windows.Forms.PictureBox img_template_preview;
        private System.Windows.Forms.ComboBox cmbTemplate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkDATEmalot;
    }
}