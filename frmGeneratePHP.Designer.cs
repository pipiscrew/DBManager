namespace DBManager
{
    partial class frmGeneratePHP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGeneratePHP));
            this.TR = new Aga.Controls.Tree.TreeViewAdv();
            this.colName = new Aga.Controls.Tree.TreeColumn();
            this.colType = new Aga.Controls.Tree.TreeColumn();
            this.colSize = new Aga.Controls.Tree.TreeColumn();
            this.colNULL = new Aga.Controls.Tree.TreeColumn();
            this.nodeIcon1 = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeCheck = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.nodeName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeFieldType = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeFieldSize = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeAllowNulls = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // TR
            // 
            this.TR.AutoRowHeight = true;
            this.TR.BackColor = System.Drawing.SystemColors.Window;
            this.TR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TR.Columns.Add(this.colName);
            this.TR.Columns.Add(this.colType);
            this.TR.Columns.Add(this.colSize);
            this.TR.Columns.Add(this.colNULL);
            this.TR.DefaultToolTipProvider = null;
            this.TR.DragDropMarkColor = System.Drawing.Color.Black;
            this.TR.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.TR.FullRowSelect = true;
            this.TR.LineColor = System.Drawing.SystemColors.ControlDark;
            this.TR.Location = new System.Drawing.Point(3, 3);
            this.TR.Model = null;
            this.TR.Name = "TR";
            this.TR.NodeControls.Add(this.nodeIcon1);
            this.TR.NodeControls.Add(this.nodeCheck);
            this.TR.NodeControls.Add(this.nodeName);
            this.TR.NodeControls.Add(this.nodeFieldType);
            this.TR.NodeControls.Add(this.nodeFieldSize);
            this.TR.NodeControls.Add(this.nodeAllowNulls);
            this.TR.SelectedNode = null;
            this.TR.Size = new System.Drawing.Size(386, 518);
            this.TR.TabIndex = 0;
            this.TR.Text = "treeViewAdv1";
            this.TR.UseColumns = true;
            // 
            // colName
            // 
            this.colName.Header = "Name";
            this.colName.SortOrder = System.Windows.Forms.SortOrder.None;
            this.colName.TooltipText = null;
            this.colName.Width = 190;
            // 
            // colType
            // 
            this.colType.Header = "Type";
            this.colType.SortOrder = System.Windows.Forms.SortOrder.None;
            this.colType.TooltipText = null;
            this.colType.Width = 80;
            // 
            // colSize
            // 
            this.colSize.Header = "Size";
            this.colSize.SortOrder = System.Windows.Forms.SortOrder.None;
            this.colSize.TooltipText = null;
            // 
            // colNULL
            // 
            this.colNULL.Header = "Nulls";
            this.colNULL.SortOrder = System.Windows.Forms.SortOrder.None;
            this.colNULL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colNULL.TooltipText = null;
            // 
            // nodeIcon1
            // 
            this.nodeIcon1.DataPropertyName = "nodeIcon";
            this.nodeIcon1.LeftMargin = 1;
            this.nodeIcon1.ParentColumn = this.colName;
            this.nodeIcon1.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // nodeCheck
            // 
            this.nodeCheck.DataPropertyName = "nodeCheck";
            this.nodeCheck.EditEnabled = true;
            this.nodeCheck.LeftMargin = 8;
            this.nodeCheck.ParentColumn = this.colName;
            // 
            // nodeName
            // 
            this.nodeName.DataPropertyName = "nodeText";
            this.nodeName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.nodeName.IncrementalSearchEnabled = true;
            this.nodeName.LeftMargin = 3;
            this.nodeName.ParentColumn = this.colName;
            // 
            // nodeFieldType
            // 
            this.nodeFieldType.DataPropertyName = "fieldType";
            this.nodeFieldType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.nodeFieldType.IncrementalSearchEnabled = true;
            this.nodeFieldType.LeftMargin = 3;
            this.nodeFieldType.ParentColumn = this.colType;
            // 
            // nodeFieldSize
            // 
            this.nodeFieldSize.DataPropertyName = "fieldSize";
            this.nodeFieldSize.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.nodeFieldSize.IncrementalSearchEnabled = true;
            this.nodeFieldSize.LeftMargin = 3;
            this.nodeFieldSize.ParentColumn = this.colSize;
            // 
            // nodeAllowNulls
            // 
            this.nodeAllowNulls.DataPropertyName = "allowNull";
            this.nodeAllowNulls.LeftMargin = 15;
            this.nodeAllowNulls.ParentColumn = this.colNULL;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(65)))), ((int)(((byte)(160)))));
            this.label2.Location = new System.Drawing.Point(418, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Folder Name :";
            // 
            // txtFolder
            // 
            this.txtFolder.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtFolder.Location = new System.Drawing.Point(519, 12);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(230, 23);
            this.txtFolder.TabIndex = 16;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.btnExport.Location = new System.Drawing.Point(519, 67);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 34);
            this.btnExport.TabIndex = 17;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "table16trans.png");
            this.imageList1.Images.SetKeyName(1, "field16.png");
            this.imageList1.Images.SetKeyName(2, "pk16.png");
            this.imageList1.Images.SetKeyName(3, "fk16.png");
            // 
            // frmGeneratePHP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 530);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TR);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmGeneratePHP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GeneratePHP";
            this.Load += new System.EventHandler(this.frmGeneratePHP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv TR;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeName;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeFieldType;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeFieldSize;
        private Aga.Controls.Tree.TreeColumn colName;
        private Aga.Controls.Tree.TreeColumn colType;
        private Aga.Controls.Tree.TreeColumn colSize;
        private Aga.Controls.Tree.TreeColumn colNULL;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon1;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox nodeAllowNulls;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox nodeCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ImageList imageList1;
    }
}