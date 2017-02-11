namespace DBManager
{
    partial class frmGenerateDesigner
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DG = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SizeF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Control = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cmbTextbox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbLabel = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbParent = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtDeclare = new System.Windows.Forms.TextBox();
            this.txtProperties = new System.Windows.Forms.TextBox();
            this.txtParent = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtInit = new System.Windows.Forms.TextBox();
            this.cmbDateTime = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbCombo = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DG)).BeginInit();
            this.SuspendLayout();
            // 
            // DG
            // 
            this.DG.AllowUserToAddRows = false;
            this.DG.AllowUserToDeleteRows = false;
            this.DG.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DG.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DG.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.SizeF,
            this.Control});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DG.DefaultCellStyle = dataGridViewCellStyle5;
            this.DG.Location = new System.Drawing.Point(9, 6);
            this.DG.MultiSelect = false;
            this.DG.Name = "DG";
            this.DG.RowHeadersVisible = false;
            this.DG.RowHeadersWidth = 45;
            this.DG.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DG.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DG.Size = new System.Drawing.Size(259, 588);
            this.DG.TabIndex = 24;
            this.DG.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DG_CellEnter);
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            // 
            // SizeF
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.SizeF.DefaultCellStyle = dataGridViewCellStyle3;
            this.SizeF.Frozen = true;
            this.SizeF.HeaderText = "Size";
            this.SizeF.Name = "SizeF";
            this.SizeF.Width = 50;
            // 
            // Control
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Control.DefaultCellStyle = dataGridViewCellStyle4;
            this.Control.HeaderText = "Control";
            this.Control.Items.AddRange(new object[] {
            "textBox",
            "ComboBox",
            "DateTime"});
            this.Control.Name = "Control";
            // 
            // cmbTextbox
            // 
            this.cmbTextbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTextbox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.cmbTextbox.FormattingEnabled = true;
            this.cmbTextbox.Location = new System.Drawing.Point(761, 6);
            this.cmbTextbox.Name = "cmbTextbox";
            this.cmbTextbox.Size = new System.Drawing.Size(162, 25);
            this.cmbTextbox.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(65)))), ((int)(((byte)(160)))));
            this.label5.Location = new System.Drawing.Point(593, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 17);
            this.label5.TabIndex = 22;
            this.label5.Text = "Field Textbox Template :";
            // 
            // cmbLabel
            // 
            this.cmbLabel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.cmbLabel.FormattingEnabled = true;
            this.cmbLabel.Location = new System.Drawing.Point(421, 6);
            this.cmbLabel.Name = "cmbLabel";
            this.cmbLabel.Size = new System.Drawing.Size(162, 25);
            this.cmbLabel.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(65)))), ((int)(((byte)(160)))));
            this.label4.Location = new System.Drawing.Point(270, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 17);
            this.label4.TabIndex = 20;
            this.label4.Text = "Field Label Template :";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.textBox3.Location = new System.Drawing.Point(819, 502);
            this.textBox3.MaxLength = 3;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(48, 25);
            this.textBox3.TabIndex = 19;
            this.textBox3.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(65)))), ((int)(((byte)(160)))));
            this.label3.Location = new System.Drawing.Point(703, 506);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 17);
            this.label3.TabIndex = 18;
            this.label3.Text = "Tab Index Start :";
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button7.Location = new System.Drawing.Point(777, 564);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(77, 30);
            this.button7.TabIndex = 15;
            this.button7.Text = "back";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(65)))), ((int)(((byte)(160)))));
            this.label2.Location = new System.Drawing.Point(272, 506);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = ".Designer.cs";
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.textBox1.Location = new System.Drawing.Point(273, 531);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(594, 25);
            this.textBox1.TabIndex = 13;
            this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(65)))), ((int)(((byte)(160)))));
            this.label1.Location = new System.Drawing.Point(270, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "Parent of Controls :";
            // 
            // cmbParent
            // 
            this.cmbParent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParent.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.cmbParent.FormattingEnabled = true;
            this.cmbParent.Location = new System.Drawing.Point(405, 37);
            this.cmbParent.Name = "cmbParent";
            this.cmbParent.Size = new System.Drawing.Size(178, 25);
            this.cmbParent.TabIndex = 11;
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button6.Location = new System.Drawing.Point(356, 564);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(139, 30);
            this.button6.TabIndex = 10;
            this.button6.Text = "Write2Designer file";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button5.Location = new System.Drawing.Point(873, 407);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 28);
            this.button5.TabIndex = 9;
            this.button5.Text = "copy";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button4.Location = new System.Drawing.Point(872, 272);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 28);
            this.button4.TabIndex = 8;
            this.button4.Text = "copy";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button3.Location = new System.Drawing.Point(872, 188);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 28);
            this.button3.TabIndex = 7;
            this.button3.Text = "copy";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button2.Location = new System.Drawing.Point(872, 102);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 28);
            this.button2.TabIndex = 6;
            this.button2.Text = "copy";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtDeclare
            // 
            this.txtDeclare.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtDeclare.Location = new System.Drawing.Point(274, 407);
            this.txtDeclare.Multiline = true;
            this.txtDeclare.Name = "txtDeclare";
            this.txtDeclare.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDeclare.Size = new System.Drawing.Size(593, 89);
            this.txtDeclare.TabIndex = 5;
            // 
            // txtProperties
            // 
            this.txtProperties.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtProperties.Location = new System.Drawing.Point(276, 275);
            this.txtProperties.Multiline = true;
            this.txtProperties.Name = "txtProperties";
            this.txtProperties.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProperties.Size = new System.Drawing.Size(593, 122);
            this.txtProperties.TabIndex = 4;
            // 
            // txtParent
            // 
            this.txtParent.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtParent.Location = new System.Drawing.Point(273, 188);
            this.txtParent.Multiline = true;
            this.txtParent.Name = "txtParent";
            this.txtParent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtParent.Size = new System.Drawing.Size(593, 79);
            this.txtParent.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.button1.Location = new System.Drawing.Point(273, 564);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 30);
            this.button1.TabIndex = 2;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtInit
            // 
            this.txtInit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtInit.Location = new System.Drawing.Point(273, 102);
            this.txtInit.Multiline = true;
            this.txtInit.Name = "txtInit";
            this.txtInit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInit.Size = new System.Drawing.Size(593, 80);
            this.txtInit.TabIndex = 1;
            // 
            // cmbDateTime
            // 
            this.cmbDateTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateTime.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.cmbDateTime.FormattingEnabled = true;
            this.cmbDateTime.Location = new System.Drawing.Point(761, 37);
            this.cmbDateTime.Name = "cmbDateTime";
            this.cmbDateTime.Size = new System.Drawing.Size(162, 25);
            this.cmbDateTime.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(65)))), ((int)(((byte)(160)))));
            this.label6.Location = new System.Drawing.Point(593, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 17);
            this.label6.TabIndex = 25;
            this.label6.Text = "Field DateTime Template :";
            // 
            // cmbCombo
            // 
            this.cmbCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCombo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.cmbCombo.FormattingEnabled = true;
            this.cmbCombo.Location = new System.Drawing.Point(761, 68);
            this.cmbCombo.Name = "cmbCombo";
            this.cmbCombo.Size = new System.Drawing.Size(162, 25);
            this.cmbCombo.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(65)))), ((int)(((byte)(160)))));
            this.label7.Location = new System.Drawing.Point(593, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(147, 17);
            this.label7.TabIndex = 27;
            this.label7.Text = "Field Combo Template :";
            // 
            // frmGenerateDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 606);
            this.Controls.Add(this.cmbCombo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbDateTime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.DG);
            this.Controls.Add(this.cmbTextbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbParent);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtDeclare);
            this.Controls.Add(this.txtProperties);
            this.Controls.Add(this.txtParent);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtInit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmGenerateDesigner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate Designer";
            this.Load += new System.EventHandler(this.frmGenerateDesigner_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtParent;
        private System.Windows.Forms.TextBox txtProperties;
        private System.Windows.Forms.TextBox txtDeclare;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ComboBox cmbParent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbLabel;
        private System.Windows.Forms.ComboBox cmbTextbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView DG;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SizeF;
        private System.Windows.Forms.DataGridViewComboBoxColumn Control;
        private System.Windows.Forms.ComboBox cmbDateTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbCombo;
        private System.Windows.Forms.Label label7;
    }
}