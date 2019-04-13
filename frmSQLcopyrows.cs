using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBManager.DBASES;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Finisar.SQLite;
using System.Data.OleDb;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using System.Web;
using System.IO;

namespace DBManager
{
    public partial class frmSQLcopyrows : BlueForm
    {
        //SQLClass sourceDB = null;
        IdbType sourceDB;

        //http://stackoverflow.com/questions/3404467/mysql-rollback-on-the-myisam-engine
        //MyISAM does not support transactions. Therefore, every individual statement runs as if it is enclosed by a transaction. You cannot roll it back.

        private DataGridViewComboBoxColumn cmbColumn;

        public frmSQLcopyrows()
        {
            InitializeComponent();

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void frmSQLcopyrows_Load(object sender, EventArgs e)
        {
            if (General.DB is SQLServer)
                chkIDENTITY.Visible = true;
            else if (General.DB is MySQLTunnel)
            {

                lblMYSQLtunnel.Visible=chkMYSQLtunnelBATCH.Visible = mySQLPHPtransLBL.Visible = true;
                chkMYSQLtunnelBATCH.Top = chkIDENTITY.Top;
            }

            DG.Columns.Add("FROM_FIELD", "FROM_FIELD");

            //TO table fields 
            cmbColumn = new DataGridViewComboBoxColumn();
            cmbColumn.HeaderText = "TO_FIELD";
            cmbColumn.Width = 120;

            DG.Columns[0].ReadOnly = true;
            DG.Columns[0].Width = 150;
            DG.Columns[0].SortMode = DataGridViewColumnSortMode.Programmatic;
            DG.Columns.Add(cmbColumn);


            foreach (var item in General.Connections)
            {
                if ((General.dbTypes)item.TYPE == General.dbTypes.SQLite || (General.dbTypes)item.TYPE == General.dbTypes.Access)
                    cmbSourceServer.Items.Add(item.filename);
                else
                    cmbSourceServer.Items.Add(item.serverName + " " + item.dbaseName);
            }



        }


        private void cmbFROM_SelectedIndexChanged(object sender, EventArgs e)
        {
            DG.Rows.Clear();

            if (cmbFROM.SelectedIndex == 0)
            {
                btnRefresh.Visible = true;
                txtCustomSQL.Focus();
            }
            else
            {
                btnRefresh.Visible = false;
                List<string> fields = sourceDB.getTableFields(cmbFROM.Text);

                foreach (string item in fields)
                    DG.Rows.Add(item);
            }


        }

        private void cmbTO_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> fields = General.DB.getTableFields(cmbTO.Text);

            cmbColumn.Items.Clear();

            cmbColumn.Items.Add("");
            foreach (string item in fields)
                cmbColumn.Items.Add(item);


            foreach (DataGridViewRow item in DG.Rows)
            {
                if (item.Cells[0].Value != null)
                {
                    if (cmbColumn.Items.Contains(item.Cells[0].Value.ToString()))
                    {
                        item.Cells[1].Value = item.Cells[0].Value.ToString();
                    }
                    else
                        item.Cells[1].Value = "";
                }
            }
        }

        private void btnCopyRows_Click(object sender, EventArgs e)
        {
            if (cmbFROM.Text.Length == 0 || cmbTO.Text.Length == 0 || DG.Rows.Count == 0)
                return;

            //if ((General.dbTypes)General.DB. == General.dbTypes.Access)
            //    if (General.Connections[cmbSourceServer.SelectedIndex].filename.ToLower().EndsWith("xlsx"))
            //        if (General.Mes("WARNING! XLSX import is broken, I couldnt find a solution, shows that rows imported but in the end there is no rows in sheet!\r\n\r\nContinue?",MessageBoxIcon.Exclamation,MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            //            return;

            DataTable sourceDT;

            if (cmbFROM.SelectedIndex == 0)
                sourceDT = sourceDB.getDatatable(txtCustomSQL.Text);
            else
            {
                //for xls
                if (sourceDB is ADOnet)
                    sourceDT = sourceDB.getDatatable("select * from [" + cmbFROM.Text + "]");
                else
                    sourceDT = sourceDB.getDatatable("select * from " + cmbFROM.Text);
            }

            if (sourceDT != null && sourceDT.Rows.Count == 0)
            {
                MessageBox.Show("Source has no rows!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                sourceDT.Dispose();

                return;
            }

            List<string> sourceFields = new List<string>();
            List<string> destFields = new List<string>();
            string INSsql = "";
            string INSfields = "";
            string INSvalues = "";

            string mySQLtunnel_Q_VARS = "";
            string mySQLtunnel_Q_S_VARS = "";

            for (int i = DG.Rows.Count - 1; i >= 0; i--)
            {
                if (DG.Rows[i].Cells[1].Value == null || DG.Rows[i].Cells[1].Value.ToString().Length == 0)
                {
                    DG.Rows.RemoveAt(i);
                }
                else
                {
                    //INSfields += "[" + DG.Rows[i].Cells[1].Value.ToString() + "],";

                    if (General.DB is MySQLTunnel)
                    {
                        INSfields += DG.Rows[i].Cells[1].Value.ToString() + ",";
                        INSvalues += " ?,";
                    }
                    else
                    {
                        INSfields += DG.Rows[i].Cells[1].Value.ToString() + ",";
                        INSvalues += "@" + DG.Rows[i].Cells[1].Value.ToString() + ",";
                    }

                    sourceFields.Add(DG.Rows[i].Cells[0].Value.ToString());
                    destFields.Add(DG.Rows[i].Cells[1].Value.ToString());
                }
            }

            if (INSfields.Length == 0)
            {
                MessageBox.Show("Nothing to do!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            INSfields = INSfields.Substring(0, INSfields.Length - 1);
            INSvalues = INSvalues.Substring(0, INSvalues.Length - 1);

            INSsql = "INSERT INTO " + cmbTO.Text + "(" + INSfields + ") VALUES (" + INSvalues + ")";


            General.dbTypes destDBtype = General.dbTypes.Access;
            if (General.DB is SQLServer)
                destDBtype = General.dbTypes.SQLSERVER;
            else if (General.DB is MySQL)
                destDBtype = General.dbTypes.MySQL;
            else if (General.DB is MySQLTunnel)
                destDBtype = General.dbTypes.MySQLtunnel;
            else if (General.DB is SQLite)
                destDBtype = General.dbTypes.SQLite;
            else if (General.DB is ADOnet)
            {
                INSsql = "INSERT INTO [" + cmbTO.Text + "](" + INSfields + ") VALUES (" + INSvalues + ")";
                destDBtype = General.dbTypes.Access;
            }


            object result = null;

            if (destDBtype != General.dbTypes.MySQLtunnel)
            {
                result = WaitWindow.Show(CopyRows, "Init...", INSsql, sourceDT, sourceFields, destFields, cmbTO.Text, destDBtype);
            }
            else
            {
                if (chkMYSQLtunnelBATCH.Checked)
                    result = WaitWindow.Show(mySQLtunnel_CopyRowsBATCH, "Init...", INSsql, sourceDT, sourceFields, destFields, cmbTO.Text, destDBtype);
                else
                    result = WaitWindow.Show(mySQLtunnel_CopyRows, "Init...", INSsql, sourceDT, sourceFields, destFields, cmbTO.Text, destDBtype);
            }


            if (sourceDT != null)
                sourceDT.Dispose();

            if (result == null)
                MessageBox.Show("Aborted by user!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (result.ToString() == "ok")
            {
                sourceDB.Disconnect();
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
            else
            {
                if (destDBtype != General.dbTypes.MySQLtunnel)
                    MessageBox.Show(result.ToString(), General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    frmInformation i = new frmInformation(result.ToString());
                    i.ShowDialog();
                }
            }

            if (cmbFROM.Text.Length == 0 || cmbTO.Text.Length == 0 || DG.Rows.Count == 0)
                return;

        }

        private DataTable copyTo_FROM(DataTable src, int startPOS, out int endPOS)
        {
            DataTable dst = src.Clone();

            int counter = 1;
            for (int i = startPOS; i < src.Rows.Count; i++)
            {
                dst.ImportRow(src.Rows[i]);

                if (counter == 900)
                    break;

                counter += 1;
            }

            endPOS = counter;

            return dst;
        }

        private void mySQLtunnel_CopyRowsBATCH(object sender, WaitWindowEventArgs e)
        {
            string INSsql = (string)e.Arguments[0];
            DataTable sourceDTpre = (DataTable)e.Arguments[1];
            List<string> sourceFields = (List<string>)e.Arguments[2];
            List<string> destFields = (List<string>)e.Arguments[3];
            INSsql = General.SafeJSON(INSsql);

            //clone source table to new datatable to change columns datatype
            DataTable sourceDTpre2 = sourceDTpre.Clone();

            for (int i = 0; i < sourceDTpre2.Columns.Count; i++)
            {
                sourceDTpre2.Columns[i].DataType = typeof(string);
            }

            foreach (DataRow row in sourceDTpre.Rows)
            {
                sourceDTpre2.ImportRow(row);
            }

            //clone source table to new datatable to change columns datatype

            DataTable sourceDT = null;
            try
            {

                //parse server name 
                string url = General.Connections[General.activeConnection].serverName;
                int pos = url.LastIndexOf("/");
                url = url.Substring(0, pos) + "/trans_batch.php";

                string errors = "";
                int errorsCount = 0;
                string result = null;

                //used for per 900records counter
                int copyRowsIndexPosition = 0;
                int copyRowsIndexEND_Position = 0;


            repeatSTEP:

                //slice source datatable per 900recs
                sourceDT = copyTo_FROM(sourceDTpre2, copyRowsIndexPosition, out copyRowsIndexEND_Position);

                //update status
                e.Window.Message = "Trans " + copyRowsIndexPosition.ToString() + " of " + ((copyRowsIndexPosition + copyRowsIndexEND_Position) - 1).ToString() + "  -  errors : " + errorsCount.ToString() + "  -  total : " + sourceDTpre2.Rows.Count.ToString();


                ////////////////////JSON
                DataTable sourceDT2 = CleanDataTable(sourceDT);
                string h = GetJson4Datatable(sourceDT2);
                ////////////////////JSON

                ////POST
                //var keyValues = new Dictionary<string, string>
                //{
                //    { "records", h },
                //    { "statement", "{\"q\": \"" + INSsql + "\"}" },
                //    {"p",General.Connections[General.activeConnection].password}
                //};
                //used because there is no timeout
                //result = HttpPostRequest(url, keyValues);

                using (var wb = new WebClient())
                {
                    var parameters = new NameValueCollection();
                    parameters["records"] = h;
                    parameters["statement"] = "{\"q\": \"" + INSsql + "\"}";
                    parameters["p"] = General.Connections[General.activeConnection].password;

                    var response = wb.UploadValues(url, "POST", parameters);
                    result = Encoding.UTF8.GetString(response);
                }


                //report error
                if (result.Contains("error"))
                {
                    errorsCount += 1;
                    errors += "ERROR on record : \r\n" + copyRowsIndexPosition.ToString() + " - " + (copyRowsIndexPosition + copyRowsIndexEND_Position).ToString() + "\r\n" + result + "\r\n\r\n";
                }
                //report error

                ////POST

                if (copyRowsIndexEND_Position == 900)
                {
                    copyRowsIndexPosition = copyRowsIndexPosition + copyRowsIndexEND_Position;//+1;
                    goto repeatSTEP;
                }

                if (errors.Length > 0)
                    e.Result = errors;
                else
                    e.Result = "ok";

            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        private string HttpPostRequest(string url, Dictionary<string, string> postParameters)
        {
            string postData = "";

            foreach (string key in postParameters.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(postParameters[key]) + "&";
            }

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.Timeout = 20 * 60 * 1000;

            byte[] data = Encoding.ASCII.GetBytes(postData);

            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = data.Length;

            Stream requestStream = myHttpWebRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            Stream responseStream = myHttpWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

            string pageContent = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            responseStream.Close();

            myHttpWebResponse.Close();

            return pageContent;
        }

        public DataTable CleanDataTable(DataTable dt)
        {
            for (int a = 0; a < dt.Rows.Count; a++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Rows[a][i] == DBNull.Value)
                    {
                        Type type = dt.Columns[i].DataType;
                        if (type == typeof(decimal) || type == typeof(int) || type == typeof(float) || type == typeof(double) ||
                           type == typeof(SqlInt16) || type == typeof(SqlInt32) || type == typeof(SqlInt64) || type == typeof(SqlDecimal) || type == typeof(SqlDouble))
                        {
                            dt.Columns[i].ReadOnly = false;
                            dt.Rows[a][i] = 0;
                        }
                        else if (type == typeof(string) || type == typeof(SqlString))
                        {
                            dt.Columns[i].ReadOnly = false;
                            dt.Rows[a][i] = "";
                        }
                        else if (type == typeof(Boolean) || type == typeof(SqlBoolean))
                        {
                            dt.Columns[i].ReadOnly = false;
                            dt.Rows[a][i] = false;
                        }
                        else if (type == typeof(DateTime) || type == typeof(SqlDateTime))
                        {
                            dt.Rows[a][i] = "01/01/1970";
                        }
                        else
                            dt.Rows[a][i] = "";
                    }
                }
            }

            return dt;
        }

        public string GetJson4Datatable(DataTable dt)
        {
            // int.max - http://brianreiter.org/tag/net/
            //http://msdn.microsoft.com/en-us/library/system.web.configuration.scriptingjsonserializationsection.maxjsonlength(v=vs.90).aspx


            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            //MessageBox.Show(HasNull(dt).ToString());
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName.Trim(), dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        private void mySQLtunnel_CopyRows(object sender, WaitWindowEventArgs e)
        {
            string INSsql = (string)e.Arguments[0];
            DataTable sourceDT = (DataTable)e.Arguments[1];
            List<string> sourceFields = (List<string>)e.Arguments[2];
            List<string> destFields = (List<string>)e.Arguments[3];
            INSsql = General.SafeJSON(INSsql);

            try
            {
                //parse server name + add tablename.php
                string url = General.Connections[General.activeConnection].serverName;
                int pos = url.LastIndexOf("/");
                url = url.Substring(0, pos) + "/trans.php";

                string errors = "";
                int errorsCount = 0;
                string result = null;

                //for all records 
                int Count = sourceDT.Rows.Count;
                for (int i = 0; i < sourceDT.Rows.Count; i++)
                {
                    result = null;

                    e.Window.Message = "Copying " + i.ToString() + " of " + Count.ToString() + "  - errors : " + errorsCount.ToString();

                    //JSON OBJECT
                    var rec = new JObject();

                    //for all columns
                    for (int x = 0; x < sourceFields.Count; x++)
                    {

                        rec[destFields[x]] = sourceDT.Rows[i][sourceFields[x]].ToString();

                        //create new parameter
                        //parameter = command.CreateParameter();
                        //parameter.ParameterName = "@" + destFields[x];
                        //parameter.Value = sourceDT.Rows[i][sourceFields[x]];
                    }

                    var serialized = JsonConvert.SerializeObject(rec);

                    using (var wb = new WebClient())
                    {
                        var parameters = new NameValueCollection();
                        parameters["record"] = serialized;
                        parameters["statement"] = "{\"q\": \"" + INSsql + "\"}";
                        parameters["p"] = General.Connections[General.activeConnection].password;

                        var response = wb.UploadValues(url, "POST", parameters);
                        result = Encoding.UTF8.GetString(response);
                    }

                    if (result.Contains("error"))
                    {
                        errorsCount += 1;
                        errors += "ERROR on record : \r\n" + rec.ToString() + "\r\n\r\n";
                    }

                }


                if (errors.Length > 0)
                    e.Result = errors;
                else
                    e.Result = "ok";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }


        //List<string> sourceFields4mySQLtunnel = new List<string>();
        //List<string> destFieldsT4mySQLtunnel = new List<string>();
        //DataTable sourceDT4mySQLtunnel=null;
        //int sourceDT4mySQLtunnel_counter = 0;
        //private void mySQLtunnel_CopyRows(object sender, WaitWindowEventArgs e)
        //{
        //    sourceDT4mySQLtunnel_counter = -1;
        //    mySQLtunnel_CopyRows2();
        //}

        //private void mySQLtunnel_CopyRows2()
        //{
        //    sourceDT4mySQLtunnel_counter += 1;

        //    if (sourceDT4mySQLtunnel_counter < sourceDT4mySQLtunnel.Rows.Count)
        //    {
        //        //WebPostRequest myPost = new WebPostRequest("http://localhost/sensor.php");
        //        //myPost.Add("keyword", "void");
        //        //myPost.Add("data", "hello&+-[]");
        //        //Console.WriteLine(myPost.GetResponse());
        //        //Console.ReadLine();
        //        string result = null;
        //        using (var wb = new WebClient())
        //        {
        //            var parameters = new NameValueCollection();
        //            parameters["sql"] = "{\"q\": \"" + General.SafeJSON(sql) + "\"}";
        //            parameters["p"] = General.Connections[ConnIndex].password;

        //            var response = wb.UploadValues(General.Connections[ConnIndex].serverName, "POST", parameters);
        //            result=Encoding.UTF8.GetString(response);
        //        }
        //    }

        //}



        private void CopyRows(object sender, WaitWindowEventArgs e)
        {
            string INSsql = (string)e.Arguments[0];
            DataTable sourceDT = (DataTable)e.Arguments[1];
            List<string> sourceFields = (List<string>)e.Arguments[2];
            List<string> destFields = (List<string>)e.Arguments[3];
            string toTable = (string)e.Arguments[4];
            General.dbTypes destDBtype = (General.dbTypes)e.Arguments[5];

            if (chkIDENTITY.Checked)
                INSsql = "SET IDENTITY_INSERT " + toTable + " ON; " + INSsql;

            IDbCommand command = null;

            if (destDBtype == General.dbTypes.SQLSERVER)
                command = new SqlCommand(INSsql);
            else if (destDBtype == General.dbTypes.MySQL)
                command = new MySqlCommand(INSsql);
            else if (destDBtype == General.dbTypes.SQLite)
                command = new SQLiteCommand(INSsql);
            else if (destDBtype == General.dbTypes.Access)
                command = new OleDbCommand(INSsql);


            //transaction
            //http://msdn.microsoft.com/en-us/library/system.data.idbtransaction.aspx
            //http://msdn.microsoft.com/en-us/library/86773566.aspx
            IDbTransaction transaction = null;
            transaction = General.DB.getConnection().BeginTransaction();

            //command
            command.Connection = General.DB.getConnection();
            command.Transaction = transaction;

            //for all records 
            int Count = sourceDT.Rows.Count;
            IDbDataParameter parameter = null;




            try
            {
                for (int i = 0; i < sourceDT.Rows.Count; i++)
                {
                    e.Window.Message = "Copying " + i.ToString() + " of " + Count.ToString();

                    command.Parameters.Clear();
                    command.CommandText = INSsql;

                    //for all columns
                    for (int x = 0; x < sourceFields.Count; x++)
                    {


                        //create new parameter
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@" + destFields[x];
                        parameter.Value = sourceDT.Rows[i][sourceFields[x]];

                        //sqlserver get from accdb works without this
                        //if (sourceDT.Rows[i][sourceFields[x]].GetType() == typeof(System.Byte[]))
                        //    parameter.DbType = DbType.Binary;

                        //add to command
                        command.Parameters.Add(parameter);

                        //command.Parameters.Add( AddWithValue("@" + destFields[x], sourceDT.Rows[i][sourceFields[x]]);
                    }

                    command.ExecuteNonQuery();
                }

                e.Result = "ok";

                transaction.Commit();
                transaction.Dispose();

            }
            catch (Exception ex)
            {
                // Attempt to roll back the transaction. 
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred 
                    // on the server that would cause the rollback to fail, such as 
                    // a closed connection.
                    General.Mes("Rollback Exception Type: " + ex2.GetType() + "\r\nMessage: " + ex2.Message);
                }

                e.Result = ex.Message;
            }
            finally
            {
                if (command != null)
                    command.Dispose();

                if (sourceDT != null)
                    sourceDT.Dispose();
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            try
            {

                DataTable dT_Fields;

                DG.Rows.Clear();
                string tmp1, tmp2;

                dT_Fields = sourceDB.ExecuteSQL(txtCustomSQL.Text, out tmp1, out tmp2);

                if (tmp2.Length > 0)
                    General.Mes(tmp2);

                if (dT_Fields == null)
                    return;

                foreach (DataColumn column in dT_Fields.Columns)
                {
                    DG.Rows.Add(column.ColumnName, "");
                }

            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);
            }
        }

        private void cmbSourceServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSourceServer.Enabled = false;
            Cursor = System.Windows.Forms.Cursors.WaitCursor;

            if (cmbSourceServer.Text.Length == 0)
                return;
            else
                btnHelp.Visible = false;

            try
            {
                if ((General.dbTypes)General.Connections[cmbSourceServer.SelectedIndex].TYPE == General.dbTypes.SQLSERVER)
                    sourceDB = new SQLServer(cmbSourceServer.SelectedIndex, null);
                else if ((General.dbTypes)General.Connections[cmbSourceServer.SelectedIndex].TYPE == General.dbTypes.MySQL)
                    sourceDB = new MySQL(cmbSourceServer.SelectedIndex, null);
                else if ((General.dbTypes)General.Connections[cmbSourceServer.SelectedIndex].TYPE == General.dbTypes.SQLite)
                    sourceDB = new SQLite(cmbSourceServer.SelectedIndex, null);
                else if ((General.dbTypes)General.Connections[cmbSourceServer.SelectedIndex].TYPE == General.dbTypes.MySQLtunnel)
                    sourceDB = new MySQLTunnel(cmbSourceServer.SelectedIndex, null);
                else if ((General.dbTypes)General.Connections[cmbSourceServer.SelectedIndex].TYPE == General.dbTypes.Access)
                {
                    sourceDB = new ADOnet(cmbSourceServer.SelectedIndex, null);
                    btnHelp.Visible = true;
                    btnHelp.Tag = DBManager.Properties.Resources.helpEXCEL;
                }
                else if ((General.dbTypes)General.Connections[cmbSourceServer.SelectedIndex].TYPE == General.dbTypes.SQLSERVERtunnel)
                    sourceDB = new SQLServerTunnel(cmbSourceServer.SelectedIndex, null);

                //   sourceDB.Connect();

                General.Mes(sourceDB.Connect(), MessageBoxIcon.Exclamation); //IF ERROR occured in class


                List<String> tblsFROM = sourceDB.getTables();
                if (tblsFROM.Count == 0)
                    return;

                cmbFROM.Items.Add("[-Custom Query-]");

                foreach (string item in tblsFROM)
                    cmbFROM.Items.Add(item);

                List<String> tblsTO = General.DB.getTables();
                foreach (string item in tblsTO)
                    cmbTO.Items.Add(item);

                cmbFROM.DroppedDown = true;
            }
            catch (Exception ex)
            {
                cmbFROM.Items.Clear();
                General.Mes(ex.Message, MessageBoxIcon.Exclamation);
            }
            finally
            {
                //cmbSourceServer.Enabled = true;
                Cursor = System.Windows.Forms.Cursors.Default;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (btnHelp.Tag != null)
            {
                frmPicture frmH = new frmPicture((Bitmap)btnHelp.Tag);
                frmH.ShowDialog();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (chkMYSQLtunnelBATCH.Checked)
            {
                frmInformation i = new frmInformation(DBManager.Properties.Resources.tunnelPHPtransBATCH);
                i.ShowDialog();
            }
            else
            {
                frmInformation i = new frmInformation(DBManager.Properties.Resources.tunnelPHPtrans);
                i.ShowDialog();
            }
        }

        private void chkMYSQLtunnelBATCH_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMYSQLtunnelBATCH.Checked)
            {
                mySQLPHPtransLBL.Text = "batch PHP";

            }
            else
            {
                mySQLPHPtransLBL.Text = "tunnel PHP";
            }
        }


    }
}
