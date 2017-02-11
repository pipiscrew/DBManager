using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DBManager
{
    public partial class frmGenerator4ReportChooser : BlueForm
    {
        List<string> rNames;
        public frmGenerator4ReportChooser( List<string>  rNames)
        {
            InitializeComponent();

            this.rNames = rNames;
        }

        public DialogResult ShowDialog(out string result)
        {
            DialogResult dialogResult = base.ShowDialog();

            result = listBox1.Text;
            return dialogResult;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void frmGenerator4ReportChooser_Load(object sender, EventArgs e)
        {
            listBox1.DataSource = rNames.ToArray();
        }
    }
}
