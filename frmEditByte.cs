using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DBManager
{
    public partial class frmEditByte : BlueForm
    {
        public frmEditByte()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(out string result)
        {
            DialogResult dialogResult = base.ShowDialog();

            result = txtFL.Text;
            return dialogResult;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Images (*.PNG;*.BMP;*.JPG;*.GIF)|*.PNG;*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFL.Text = (openFileDialog1.FileName);//Do what you want here
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtFL.Text.Length == 0)
            {
                MessageBox.Show("You should select a file via 'browse' button", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        #region txtConnection DRGDRP

        private void txtFL_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            txtFL.Text = FileList[0];
        }

        private void txtFL_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        #endregion

        private void frmEditByte_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }


    }
}
