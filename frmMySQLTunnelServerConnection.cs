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
    public partial class frmMySQLTunnelServerConnection : BlueForm
    {
        private bool importSchema;
        private List<string> tbls;
        MySQLTunnel c = null;

        public frmMySQLTunnelServerConnection()
        {
            InitializeComponent();
        }

        public frmMySQLTunnelServerConnection(bool importSchema, List<string> tbls)
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
            if (txtURL.Text.Trim().Length > 0 && txtPassword.Text.Trim().Length > 0)
            {
                //   General.ConneADO = new ADOnet("Provider=SQLOLEDB;Data Source=" + txtServer.Text + ";Initial Catalog=" + txtDBASE.Text + ";User ID=" + txtUser.Text + ";Password=" + txtPassword.Text);
            }
            else
            {
                MessageBox.Show("Please fill all infos required!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //MySqlConnection objConn = null;
            //bool errorOccured = false;
            c = new MySQLTunnel();
            c.isConnected += new MySQLTunnel.IsConnected(c_isConnected);
            try
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                //General.DB = new MySQLTunnel((int)tmp.Tag, imageList1);

                c.testConnection(txtURL.Text.Trim(), txtPassword.Text.Trim());

                //////objConn = new MySqlConnection("server=" + txtServer.Text + ";" +
                //////                                       "database=" + txtDBASE.Text + ";" +
                //////                                       "user id=" + txtURL.Text + ";" +
                //////                                       "password=" + txtPassword.Text + ";" +
                //////                                                        "Port=" + txtPort.Text + ";");
                //////objConn.Open();
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);
                //errorOccured = true;
            }
            finally
            {
                //if (objConn != null)
                //{
                //    objConn.Close();
                //    objConn.Dispose();
                //    objConn = null;
                //}

               
            }

            //if (!errorOccured)
            //{
            //    General.Connections.Add(new dbConnection
            //    {
            //        dbaseName = "",
            //        filename = "",
            //        password = txtPassword.Text,
            //        port = "",
            //        serverName = txtURL.Text,
            //        TYPE = (int)General.dbTypes.MySQLtunnel,
            //        user = ""
            //    });


            //    this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //}

        }


        delegate void c_isConnectedCallback(bool isSuccess);
           void c_isConnected(bool isSuccess)
        {
                        if (txtURL.InvokeRequired)
            {
                txtURL.BeginInvoke(new c_isConnectedCallback(this.c_isConnected), new object[] { isSuccess });
                return;
            }

            Cursor = System.Windows.Forms.Cursors.Default;

            if (isSuccess)
            {
                General.Connections.Add(new dbConnection
                {
                    dbaseName = "",
                    filename = "",
                    password = txtPassword.Text,
                    port = "",
                    serverName = txtURL.Text,
                    TYPE = (int)General.dbTypes.MySQLtunnel,
                    user = ""
                });


                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
            else
                General.Mes("Could not connect!");
        }

           private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
           {
               frmInformation i = new frmInformation(DBManager.Properties.Resources.tunnelPHP);
               i.ShowDialog();
           }

           private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
           {
               frmInformation i = new frmInformation(DBManager.Properties.Resources.tunnelPHP_PDO);
               i.ShowDialog();
           }
    }
}
