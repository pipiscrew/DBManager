using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBManager
{
    public partial class frmGeneratePHP_CRUDexpandable : BlueForm
    {
        public frmGeneratePHP_CRUDexpandable(string tbl, string[] tbls)
        {
            InitializeComponent();

            label1.Text = string.Format("Table {0} found as FK to the following tables, please which one will be shown as expandable. By double click on it.", tbl);

            lst.Items.AddRange(tbls);

            lst.SelectedIndex = 0;
        }

        public DialogResult ShowDialog(out int result)
        {
            DialogResult dialogResult = base.ShowDialog();

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
                result = lst.SelectedIndex;
            else
                result = -1;

            return dialogResult;
        }

        private void lst_DoubleClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
