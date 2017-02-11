using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace DBManager
{
    public partial class frmGenerateFormCode : BlueForm
    {
        internal string tbl;
        internal string firstField;
        internal string Form2Class;

        public frmGenerateFormCode()
        {
            InitializeComponent();
        }

        private void frmGenerateButtons_Load(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath + "\\FormCode");

            foreach (System.IO.FileInfo f in dir.GetFiles())
            {
                cmbButton.Items.Add(f.Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbButton.Text.Length == 0)
            {       MessageBox.Show("Please select template!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CultureInfo culture = new CultureInfo("el-GR");
            int codePage = culture.TextInfo.ANSICodePage;
            Encoding GreekEncoding = Encoding.GetEncoding(codePage);
            string fl = File.ReadAllText(Application.StartupPath + "\\FormCode\\" + cmbButton.Text, GreekEncoding);

            fl=fl.Replace("{table}",tbl);
            fl = fl.Replace("{firstField}", firstField);

            textBox1.Text = Form2Class + "\r\n\r\n" + fl;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x1')
            {
                ((TextBox)sender).SelectAll();
                e.Handled = true;
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Replace(txtSearch.Text, txtReplace.Text);
        }
    }
}
