using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Finisar.SQLite;

namespace DBManager
{
    public partial class frmSQLiteConnection : BlueForm
    {
        public frmSQLiteConnection()
        {
            InitializeComponent();
        }

        #region txtConnection DRGDRP

        private void txtConnection_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList[0].ToLower().EndsWith(".db") || FileList[0].ToLower().EndsWith(".db3"))
            {
               // txtConnection.Text = "Data Source=" + FileList[0] + ";Version=3;";
                makeConnection(FileList[0]);
            }
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
            openFileDialog1.Filter = "SQLite dbases (db,db3)|*.db;*.db3|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //txtConnection.Text = "Data Source=" + openFileDialog1.FileName + ";Version=3;";

                makeConnection(openFileDialog1.FileName);
            }

             
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void makeConnection(string filepath)
        {
            General.Connections.Add(new dbConnection
            {
                dbaseName ="",
                filename = filepath,
                password = "",
                port = "",
                serverName = "",
                TYPE = (int)General.dbTypes.SQLite,
                user = ""
            });


            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
