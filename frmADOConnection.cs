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
    public partial class frmADOConnection : BlueForm
    {
        public frmADOConnection()
        {
            InitializeComponent();
        }


        #region txtConnection DRGDRP

        private void txtConnection_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList[0].ToLower().EndsWith(".mdb") || FileList[0].ToLower().EndsWith(".accdb") || FileList[0].ToLower().EndsWith(".xls") || FileList[0].ToLower().EndsWith(".xlsx"))
                makeConnection(FileList[0]);
        }

        private void txtConnection_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        #endregion

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Access database (mdb,accdb)|*.mdb;*.accdb|Excel (xls,xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                makeConnection(openFileDialog1.FileName);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void makeConnection(string filepath)
        {
            General.Connections.Add(new dbConnection
            {
                dbaseName = "",
                filename = filepath,
                password = txtPassword.Text,
                port = "",
                serverName = "",
                TYPE = (int)General.dbTypes.Access,
                user = ""
            });


            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
