namespace EFClassGenerator
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Label1 = new System.Windows.Forms.Label();
            this.coTabellen = new System.Windows.Forms.ComboBox();
            this.cmdGenerateClass = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.coPKColumn = new System.Windows.Forms.ComboBox();
            this.txtOut = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEFContext = new System.Windows.Forms.TextBox();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdSaveToFile = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.coOrderColumn = new System.Windows.Forms.ComboBox();
            this.optViews = new System.Windows.Forms.RadioButton();
            this.optTables = new System.Windows.Forms.RadioButton();
            this.cmdCopyClip = new System.Windows.Forms.Button();
            this.cmdGenerateException = new System.Windows.Forms.Button();
            this.cmdGenerateSelectList = new System.Windows.Forms.Button();
            this.cmdTemplateOpen = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(8, 59);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(34, 13);
            this.Label1.TabIndex = 6;
            this.Label1.Text = "Table";
            // 
            // coTabellen
            // 
            this.coTabellen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coTabellen.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coTabellen.FormattingEnabled = true;
            this.coTabellen.Location = new System.Drawing.Point(85, 56);
            this.coTabellen.Name = "coTabellen";
            this.coTabellen.Size = new System.Drawing.Size(525, 21);
            this.coTabellen.TabIndex = 7;
            this.coTabellen.SelectedIndexChanged += new System.EventHandler(this.coTabellen_SelectedIndexChanged);
            // 
            // cmdGenerateClass
            // 
            this.cmdGenerateClass.Location = new System.Drawing.Point(616, 54);
            this.cmdGenerateClass.Name = "cmdGenerateClass";
            this.cmdGenerateClass.Size = new System.Drawing.Size(73, 51);
            this.cmdGenerateClass.TabIndex = 10;
            this.cmdGenerateClass.Text = "Generate Class";
            this.toolTip1.SetToolTip(this.cmdGenerateClass, "Generate the EF data class");
            this.cmdGenerateClass.UseVisualStyleBackColor = true;
            this.cmdGenerateClass.Click += new System.EventHandler(this.cmdGenerateClass_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "PK Column";
            // 
            // coPKColumn
            // 
            this.coPKColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coPKColumn.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coPKColumn.FormattingEnabled = true;
            this.coPKColumn.Location = new System.Drawing.Point(85, 83);
            this.coPKColumn.Name = "coPKColumn";
            this.coPKColumn.Size = new System.Drawing.Size(219, 21);
            this.coPKColumn.TabIndex = 12;
            this.toolTip1.SetToolTip(this.coPKColumn, "Select the column used as primary key");
            // 
            // txtOut
            // 
            this.txtOut.AcceptsReturn = true;
            this.txtOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOut.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOut.Location = new System.Drawing.Point(11, 111);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOut.Size = new System.Drawing.Size(965, 568);
            this.txtOut.TabIndex = 13;
            this.txtOut.WordWrap = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(314, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "EF Context";
            // 
            // txtEFContext
            // 
            this.txtEFContext.Location = new System.Drawing.Point(391, 5);
            this.txtEFContext.Name = "txtEFContext";
            this.txtEFContext.Size = new System.Drawing.Size(219, 20);
            this.txtEFContext.TabIndex = 15;
            this.txtEFContext.Text = "EFContext";
            this.toolTip1.SetToolTip(this.txtEFContext, "Enter the object name of the EF context (found app.config or web.config)");
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(85, 5);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(219, 20);
            this.txtNamespace.TabIndex = 17;
            this.txtNamespace.Text = "Namespace";
            this.toolTip1.SetToolTip(this.txtNamespace, "Enter the base namespace fo your destination project");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Namespace";
            // 
            // cmdSaveToFile
            // 
            this.cmdSaveToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSaveToFile.Image = ((System.Drawing.Image)(resources.GetObject("cmdSaveToFile.Image")));
            this.cmdSaveToFile.Location = new System.Drawing.Point(982, 111);
            this.cmdSaveToFile.Name = "cmdSaveToFile";
            this.cmdSaveToFile.Size = new System.Drawing.Size(23, 23);
            this.cmdSaveToFile.TabIndex = 18;
            this.toolTip1.SetToolTip(this.cmdSaveToFile, "Save this class directly into your destination project folder");
            this.cmdSaveToFile.UseVisualStyleBackColor = true;
            this.cmdSaveToFile.Click += new System.EventHandler(this.cmdSaveToFile_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(314, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Order By Column";
            // 
            // coOrderColumn
            // 
            this.coOrderColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coOrderColumn.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coOrderColumn.FormattingEnabled = true;
            this.coOrderColumn.Location = new System.Drawing.Point(406, 84);
            this.coOrderColumn.Name = "coOrderColumn";
            this.coOrderColumn.Size = new System.Drawing.Size(204, 21);
            this.coOrderColumn.TabIndex = 20;
            this.toolTip1.SetToolTip(this.coOrderColumn, "Select the column used to sort lists by (ex. last_name)");
            // 
            // optViews
            // 
            this.optViews.AutoSize = true;
            this.optViews.Location = new System.Drawing.Point(159, 33);
            this.optViews.Name = "optViews";
            this.optViews.Size = new System.Drawing.Size(53, 17);
            this.optViews.TabIndex = 22;
            this.optViews.Text = "Views";
            this.toolTip1.SetToolTip(this.optViews, "List all views");
            this.optViews.UseVisualStyleBackColor = true;
            this.optViews.Click += new System.EventHandler(this.optViews_Click);
            // 
            // optTables
            // 
            this.optTables.AutoSize = true;
            this.optTables.Checked = true;
            this.optTables.Location = new System.Drawing.Point(85, 33);
            this.optTables.Name = "optTables";
            this.optTables.Size = new System.Drawing.Size(57, 17);
            this.optTables.TabIndex = 21;
            this.optTables.TabStop = true;
            this.optTables.Text = "Tables";
            this.toolTip1.SetToolTip(this.optTables, "List all tables");
            this.optTables.UseVisualStyleBackColor = true;
            this.optTables.Click += new System.EventHandler(this.optTables_Click);
            // 
            // cmdCopyClip
            // 
            this.cmdCopyClip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCopyClip.Image = ((System.Drawing.Image)(resources.GetObject("cmdCopyClip.Image")));
            this.cmdCopyClip.Location = new System.Drawing.Point(982, 140);
            this.cmdCopyClip.Name = "cmdCopyClip";
            this.cmdCopyClip.Size = new System.Drawing.Size(23, 23);
            this.cmdCopyClip.TabIndex = 23;
            this.toolTip1.SetToolTip(this.cmdCopyClip, "Copy class to clipboard");
            this.cmdCopyClip.UseVisualStyleBackColor = true;
            this.cmdCopyClip.Click += new System.EventHandler(this.cmdCopyClip_Click);
            // 
            // cmdGenerateException
            // 
            this.cmdGenerateException.Location = new System.Drawing.Point(774, 54);
            this.cmdGenerateException.Name = "cmdGenerateException";
            this.cmdGenerateException.Size = new System.Drawing.Size(73, 51);
            this.cmdGenerateException.TabIndex = 24;
            this.cmdGenerateException.Text = "Generate Exception";
            this.toolTip1.SetToolTip(this.cmdGenerateException, "Generate the database exception class (only needed once in destination project)");
            this.cmdGenerateException.UseVisualStyleBackColor = true;
            this.cmdGenerateException.Click += new System.EventHandler(this.cmdGenerateException_Click);
            // 
            // cmdGenerateSelectList
            // 
            this.cmdGenerateSelectList.Location = new System.Drawing.Point(695, 54);
            this.cmdGenerateSelectList.Name = "cmdGenerateSelectList";
            this.cmdGenerateSelectList.Size = new System.Drawing.Size(73, 51);
            this.cmdGenerateSelectList.TabIndex = 25;
            this.cmdGenerateSelectList.Text = "Generate SelList";
            this.toolTip1.SetToolTip(this.cmdGenerateSelectList, "Generate a class used for MVC model views only to fill combo boxes with data");
            this.cmdGenerateSelectList.UseVisualStyleBackColor = true;
            this.cmdGenerateSelectList.Click += new System.EventHandler(this.cmdGenerateSelectList_Click);
            // 
            // cmdTemplateOpen
            // 
            this.cmdTemplateOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTemplateOpen.Image = ((System.Drawing.Image)(resources.GetObject("cmdTemplateOpen.Image")));
            this.cmdTemplateOpen.Location = new System.Drawing.Point(982, 191);
            this.cmdTemplateOpen.Name = "cmdTemplateOpen";
            this.cmdTemplateOpen.Size = new System.Drawing.Size(23, 23);
            this.cmdTemplateOpen.TabIndex = 26;
            this.toolTip1.SetToolTip(this.cmdTemplateOpen, "Open template file in text editor");
            this.cmdTemplateOpen.UseVisualStyleBackColor = true;
            this.cmdTemplateOpen.Click += new System.EventHandler(this.cmdTemplateOpen_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Tip";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 691);
            this.Controls.Add(this.cmdTemplateOpen);
            this.Controls.Add(this.cmdGenerateSelectList);
            this.Controls.Add(this.cmdGenerateException);
            this.Controls.Add(this.cmdCopyClip);
            this.Controls.Add(this.optViews);
            this.Controls.Add(this.optTables);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.coOrderColumn);
            this.Controls.Add(this.cmdSaveToFile);
            this.Controls.Add(this.txtNamespace);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtEFContext);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.coPKColumn);
            this.Controls.Add(this.cmdGenerateClass);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.coTabellen);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(862, 665);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EF Code Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox coTabellen;
        private System.Windows.Forms.Button cmdGenerateClass;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.ComboBox coPKColumn;
        private System.Windows.Forms.TextBox txtOut;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEFContext;
        private System.Windows.Forms.TextBox txtNamespace;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdSaveToFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.ComboBox coOrderColumn;
        internal System.Windows.Forms.RadioButton optViews;
        internal System.Windows.Forms.RadioButton optTables;
        private System.Windows.Forms.Button cmdCopyClip;
        private System.Windows.Forms.Button cmdGenerateException;
        private System.Windows.Forms.Button cmdGenerateSelectList;
        private System.Windows.Forms.Button cmdTemplateOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}