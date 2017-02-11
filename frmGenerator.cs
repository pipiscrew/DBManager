using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DBManager
{
    public partial class frmGenerator : BlueForm
    {
        private string txt;

        public frmGenerator()
        {
            InitializeComponent();
        }

        public frmGenerator(string Gdata)
        {
            InitializeComponent();

            txt = Gdata;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void Generator_Load(object sender, EventArgs e)
        {
            textBox1.Text = txt;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
          textBox1.Text = textBox1.Text.Replace(txtSearch.Text, txtReplace.Text);
        }
    }
}
