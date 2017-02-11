using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBManager
{
    public partial class frmSQLServerConnection : BlueForm
    {
        private bool importSchema;
        private List<string> tbls;

        public frmSQLServerConnection()
        {
            InitializeComponent();
        }

        public frmSQLServerConnection(bool importSchema, List<string> tbls)
        {
            InitializeComponent();

            this.importSchema = importSchema;
            this.tbls = tbls;
        }

        private void txtServer_DropDown(object sender, EventArgs e)
        {
            if (txtServer.Items.Count > 0)
                return;

            Cursor = System.Windows.Forms.Cursors.WaitCursor;

            try
            {
                // Retrieve the enumerator instance and then the data.
                System.Data.Sql.SqlDataSourceEnumerator instance = System.Data.Sql.SqlDataSourceEnumerator.Instance;
                System.Data.DataTable table = instance.GetDataSources();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    txtServer.Items.Add(row["ServerName"] + (row["InstanceName"].ToString().Length > 0 ? "\\" + row["InstanceName"].ToString() : ""));
                }
            }
            finally
            {
                Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void txtDBASE_DropDown(object sender, EventArgs e)
        {
            if (txtServer.Text.Trim().Length > 0 && txtUser.Text.Trim().Length > 0 && txtPassword.Text.Trim().Length > 0)
            {
                SqlConnection objConn = null;

                txtDBASE.Items.Clear();

                try
                {
                    //

                    Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    objConn = new SqlConnection("Data Source=" + txtServer.Text + ";Initial Catalog=master;User ID=" + txtUser.Text + ";Password=" + txtPassword.Text);
                    objConn.Open();


                    SqlDataReader sqlread = null;
                    SqlCommand sqlco = new SqlCommand();

                    sqlco.Connection = objConn;
                    sqlco.CommandText = "EXEC sp_databases";

                    bool isAzure = false;
                    try
                    {
                        sqlread = sqlco.ExecuteReader();
                    }
                    catch {
                        isAzure = true; 
                    }



                    if (!isAzure)
                    {
                        while (sqlread.Read())
                        {
                            //
                            if (sqlread["DATABASE_NAME"].ToString().ToLower() == "master" || sqlread["DATABASE_NAME"].ToString().ToLower() == "model" || sqlread["DATABASE_NAME"].ToString().ToLower() == "msdb" || sqlread["DATABASE_NAME"].ToString().ToLower() == "tempdb" || sqlread["DATABASE_NAME"].ToString().ToLower().StartsWith("reportserver$"))
                            { }
                            else
                                txtDBASE.Items.Add(sqlread["DATABASE_NAME"]);
                        }
                    }
                    else
                    {
                        sqlco = new SqlCommand();

                        sqlco.Connection = objConn;
                        sqlco.CommandText = "SELECT * FROM sys.databases";
                        sqlread = sqlco.ExecuteReader();
                        while (sqlread.Read())
                        {
                                txtDBASE.Items.Add(sqlread["NAME"]);
                        }
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

            SqlConnection objConn = null;
            bool errorOccured = false;

            try
            {

                Cursor = System.Windows.Forms.Cursors.WaitCursor;

                objConn = new SqlConnection("Data Source=" + txtServer.Text + ";Initial Catalog=master;User ID=" + txtUser.Text + ";Password=" + txtPassword.Text);
                objConn.Open();
            }
            catch
            {
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
                    filename = txtFilter.Text,
                    password = txtPassword.Text,
                    port = "",
                    serverName = txtServer.Text,
                    TYPE = (int)General.dbTypes.SQLSERVER,
                    user = txtUser.Text
                });


                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }

        }

        private void frmSQLServerConnection_Load(object sender, EventArgs e)
        {
            //txtServer.Text = General.GetAppRegistrySetting("SQLServer");
            //txtUser.Text = General.GetAppRegistrySetting("SQLServerU");
            //txtPassword.Text = General.GetAppRegistrySetting("SQLServerP");
            //txtDBASE.Text = General.GetAppRegistrySetting("SQLServerD");
        }
    }
}
