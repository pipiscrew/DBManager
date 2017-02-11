using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace DBManager
{
    public partial class frmMySQLServerConnection : BlueForm
    {
        private bool importSchema;
        private List<string> tbls;

        public frmMySQLServerConnection()
        {
            InitializeComponent();
        }

        public frmMySQLServerConnection(bool importSchema, List<string> tbls)
        {
            InitializeComponent();

            this.importSchema = importSchema;
            this.tbls = tbls;
        }

        private void txtServer_DropDown(object sender, EventArgs e)
        {
            //if (txtServer.Items.Count > 0)
            //    return;

            //Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //try
            //{
            //    // Retrieve the enumerator instance and then the data.
            //    System.Data.Sql.SqlDataSourceEnumerator instance = System.Data.Sql.SqlDataSourceEnumerator.Instance;
            //    System.Data.DataTable table = instance.GetDataSources();

            //    foreach (System.Data.DataRow row in table.Rows)
            //    {
            //        txtServer.Items.Add(row["ServerName"] + (row["InstanceName"].ToString().Length > 0 ? "\\" + row["InstanceName"].ToString() : ""));
            //    }
            //}
            //finally
            //{
            //    Cursor = System.Windows.Forms.Cursors.Default;
            //}
        }

        private void txtDBASE_DropDown(object sender, EventArgs e)
        {
            if (txtServer.Text.Trim().Length > 0 && txtUser.Text.Trim().Length > 0 && txtPassword.Text.Trim().Length > 0)
            {
                MySqlConnection objConn = null;

                txtDBASE.Items.Clear();

                try
                {
                    //

                    Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    objConn = new MySqlConnection("server=" + txtServer.Text + ";" +
                                                       "database=" + txtDBASE.Text + ";" +
                                                       "user id=" + txtUser.Text + ";" +
                                                       "password=" + txtPassword.Text + ";" +
                                                                        "Port=" + txtPort.Text + ";");
                    objConn.Open();


                    MySqlDataReader sqlread = null;
                    MySqlCommand sqlco = new MySqlCommand();

                    sqlco.Connection = objConn;
                    sqlco.CommandText = "SHOW DATABASES";

                    sqlread = sqlco.ExecuteReader();

                    while (sqlread.Read())
                    {

                        if (sqlread["DATABASE"].ToString().ToLower() != "information_schema")
                            txtDBASE.Items.Add(sqlread["DATABASE"]);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    if (objConn != null)
                    {
                        objConn.Close();
                        objConn.Dispose();
                        objConn = null;
                    }

                    Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtServer.Text.Trim().Length > 0 && txtUser.Text.Trim().Length > 0 && txtPassword.Text.Trim().Length > 0)
            {
                //   General.ConneADO = new ADOnet("Provider=SQLOLEDB;Data Source=" + txtServer.Text + ";Initial Catalog=" + txtDBASE.Text + ";User ID=" + txtUser.Text + ";Password=" + txtPassword.Text);
            }
            else
            {
                MessageBox.Show("Please fill all infos required!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlConnection objConn = null;
            bool errorOccured = false;

            try
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;

                objConn = new MySqlConnection("server=" + txtServer.Text + ";" +
                                                       "database=" + txtDBASE.Text + ";" +
                                                       "user id=" + txtUser.Text + ";" +
                                                       "password=" + txtPassword.Text + ";" +
                                                                        "Port=" + txtPort.Text + ";");
                objConn.Open();
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);
                errorOccured = true;
            }
            finally
            {
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                    objConn = null;
                }

                Cursor = System.Windows.Forms.Cursors.Default;
            }

            if (!errorOccured)
            {
                General.Connections.Add(new dbConnection
                {
                    dbaseName = txtDBASE.Text,
                    filename = "",
                    password = txtPassword.Text,
                    port = txtPort.Text,
                    serverName = txtServer.Text,
                    TYPE = (int)General.dbTypes.MySQL,
                    user = txtUser.Text
                });


                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }

        }

        private void frmMySQLServerConnection_Load(object sender, EventArgs e)
        {
            //txtServer.Text = General.GetAppRegistrySetting("SQLServer");
            //txtUser.Text = General.GetAppRegistrySetting("SQLServerU");
            //txtPassword.Text = General.GetAppRegistrySetting("SQLServerP");
            //txtDBASE.Text = General.GetAppRegistrySetting("SQLServerD");
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(8))
                return;
            else if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
