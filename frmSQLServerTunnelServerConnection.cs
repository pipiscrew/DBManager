using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DBManager.DBASES;
using System.Net;

namespace DBManager
{
    public partial class frmSQLServerTunnelServerConnection : BlueForm
    {
        private bool importSchema;
        private List<string> tbls;
        SQLServerTunnel c = null;

        public frmSQLServerTunnelServerConnection()
        {
            InitializeComponent();
        }

        public frmSQLServerTunnelServerConnection(bool importSchema, List<string> tbls)
        {
            InitializeComponent();

            this.importSchema = importSchema;
            this.tbls = tbls;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtURL.Text.Trim().Length == 0 || txtPassword.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please fill all infos!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (txtPassword.Text.Contains("$"))
                {
                    MessageBox.Show("The symbol '$' forbidden!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (!txtURL.Text.ToLower().StartsWith("https"))
                {
                    if (MessageBox.Show("Server must be https enabled, are you sure you want to continue ?", General.apTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                        return;
                }
            }


            if (txtURL.Text.ToLower().StartsWith("https"))
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            else
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(48 | 192);


            Cursor = System.Windows.Forms.Cursors.WaitCursor;

            c = new SQLServerTunnel();
            c.isConnected += new SQLServerTunnel.IsConnected(c_isConnected);
            try
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;

                c.testConnection(txtURL.Text.Trim(), txtPassword.Text.Trim());

            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);

            }
            finally
            {
            }
        }


        delegate void c_isConnectedCallback(string isSuccess);
        void c_isConnected(string isSuccess)
        {
            if (txtURL.InvokeRequired)
            {
                txtURL.BeginInvoke(new c_isConnectedCallback(this.c_isConnected), new object[] { isSuccess });
                return;
            }

            Cursor = System.Windows.Forms.Cursors.Default;

            if (isSuccess == "ok")
            {
                General.Connections.Add(new dbConnection
                {
                    dbaseName = "",
                    filename = "",
                    password = txtPassword.Text,
                    port = "",
                    serverName = txtURL.Text,
                    TYPE = (int)General.dbTypes.SQLSERVERtunnel,
                    user = ""
                });


                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
            else
                General.Mes("Could not connect! due : " + isSuccess);
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInformation i = new frmInformation(DBManager.Properties.Resources.tunnelPHP_PDO_SQLServer);
            i.ShowDialog();
        }
    }
}
