using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aga.Controls.Tree;
using System.Windows.Forms;
using System.Data;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections.Specialized;


namespace DBManager.DBASES
{
    class MySQLTunnel : IdbType
    {
        public delegate void IsConnected(bool isSuccess);
        public event IsConnected isConnected;

        //internal MySQLClass Connection = null;
        //internal string ConnectionString;
        internal TreeModel TRmodel;
        internal ImageList imageList;
        internal int ConnIndex = 0;

        public MySQLTunnel(int index, ImageList imageList)
        {
            //ConnectionString = "Data Source=" + General.Connections[index].serverName +
            //        ";Initial Catalog=" + General.Connections[index].dbaseName +
            //        ";User ID=" + General.Connections[index].user +
            //         ";Password=" + General.Connections[index].password;

            ConnIndex = index;

            this.imageList = imageList;
        }

        public MySQLTunnel() { }

        public string GetString(string sql)
        {
            //sql = sql.Replace("\r\n", " ");

            using (var wb = new WebClient())
            {
                var parameters = new NameValueCollection();
                parameters["sql"] = "{\"q\": \"" + General.SafeJSON(sql) + "\"}";
                parameters["p"] = General.Connections[ConnIndex].password;

                var response = wb.UploadValues(General.Connections[ConnIndex].serverName, "POST", parameters);
                return Encoding.UTF8.GetString(response);
            }


            //WebClient web = new WebClient();
            //string s = web.DownloadString(General.Connections[ConnIndex].serverName + "?sql={\"q\": \"" + General.SafeJSON(sql) + "\"}&p=" + General.Connections[ConnIndex].password);
            //return s;
        }

        private DataTable parseJSON(string JSON, out string rowsaffected, out string err)
        {
            err = "";
            rowsaffected = "";
            if (JSON.StartsWith("Error:"))
            {
                err = JSON;
                return null;
            }

            JObject JData = JObject.Parse(JSON);

            if (JData == null)
                return null;

            if (JData["result"] == null )
                return null;

            if (JData["affected"] != null)
                rowsaffected = JData["affected"].ToString();
            

            DataTable TBL=null;
            DataColumn col=null;
            DataRow TableRows;
            bool firstPass = true;
            foreach (var d in JData["result"])
            {
                //Console.WriteLine(d);
                Dictionary<string, object> tmp = Parse2(d.ToString());

                //setup the COLUMNS
                if (firstPass)
                {
                    TBL = new DataTable("TBL");
                    foreach(KeyValuePair<string,object> kvp in tmp)
                    {
                            
                            col = new DataColumn(kvp.Key.ToString(), Type.GetType("System.String"));
                            TBL.Columns.Add(col);
                            
                        }
                    TBL.AcceptChanges();
                   
                    firstPass = false;
                 }

                TableRows = TBL.NewRow();
                //add the rows!
                foreach (KeyValuePair<string, object> kvp in tmp)
                {
                    
                    TableRows[kvp.Key.ToString()] = kvp.Value;
                    
                }

                TBL.Rows.Add(TableRows);
            
            }

            //Console.WriteLine(TBL.Rows.Count.ToString());
            return TBL;
            //Parse2(JData["result"].ToString());
            //return null;
            //foreach (var x in JData)
            //{
            //    string name = x.Key;
            //    JToken value = x.Value;

            //    //Console.WriteLine(name + " - " + value);

            //    foreach (var d in value)
            //    {
            //        //Console.WriteLine(d);
            //        Parse2(d.ToString());
            //    }
            //}
        }

        public Dictionary<string, object> Parse2(string array, bool internCALL=false)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            Newtonsoft.Json.Linq.JObject obj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(array);
            foreach (KeyValuePair<string, Newtonsoft.Json.Linq.JToken> kvp in obj)
            {
                //if (kvp.Value.ToString().Contains('{')) <- at first, then when a field contains { and is not JSON should be like? :

                if (kvp.Value.ToString().Trim().StartsWith("{"))
                {
                    result.Add(kvp.Key, Parse2(kvp.Value.ToString().Replace("[", "").Replace("]", "")));
                }
                else
                {
                    result.Add(kvp.Key, kvp.Value.ToString());
                }
            }
            return result;
        }

        public void testConnection(string url,string password)
        {
            //var handler = new HttpHandler();
            //handler.RequestCompleted += testConnection;
            //handler.GetAsync(url + "?sql={\"q\": \"" + "testconnection" + "\"}&p=" + password);

            using (var wb = new WebClient())
            {
                var parameters = new NameValueCollection();
                parameters["sql"] = "{\"q\": \"" + "testconnection" + "\"}";
                parameters["p"] = password;

                var response = wb.UploadValues(url, "POST", parameters);
                string k= Encoding.UTF8.GetString(response);

                if (isConnected != null)
                {
                        if (k != "true")
                            isConnected(false);
                        else
                            isConnected(true);

                }
                else
                    isConnected(false);
            }

        }

        //delegate void testConnectionCallback(object sender, GenericEventArgs<ResponseArgs> e);
        //private void testConnection(object sender, GenericEventArgs<ResponseArgs> e)
        //{
        //    //if (textBox1.InvokeRequired)
        //    //{
        //    //    textBox1.BeginInvoke(new SourceDownloadedCallback(this.SourceDownloaded), new object[] { sender, e });
        //    //    return;
        //    //}

        //    if (isConnected != null)
        //    {
        //        if (e.Value.error == null)
        //        {
        //            if (e.Value.httpwebResponse != "true")
        //                isConnected(false);
        //            else
        //                isConnected(true);
        //        }
        //        else
        //            isConnected(false);
        //    }
        //    else
        //        isConnected(false);
        //}

        public string Connect()
        {
            return "";
            //MySqlException err = null;
            //Connection = new MySQLClass(ConnectionString, out err);

            //if (err != null)
            //{
            //    Connection = null;
            //    return err.Message;
            //}
            //else
            //    return "";
        }

        public void Disconnect()
        {
           
        }

        public string parseProcedure(string procName, bool replaceCreate)
        {
            //SHOW CREATE PROCEDURE 
            string k = GetString("SHOW CREATE PROCEDURE " + procName);
            string affected, errorRESP = "";
            DataTable dT = parseJSON(k, out affected, out errorRESP);

            return dT.Rows[0]["Create Procedure"].ToString();

        }

        public ListViewItem[] GetProcedures()
        {
  

            //DataTable dT = Connection.GetDATATABLE("select name,convert(CHAR(10),crdate,105) from sysobjects where xtype = 'p' AND CATEGORY=0 order by 1");
                             
            string k = GetString("SHOW PROCEDURE STATUS");
            string affected,errorRESP="";
            DataTable dT = parseJSON(k, out affected, out errorRESP);

            if (dT == null)
                return null;

            ListViewItem[] procs = new ListViewItem[dT.Rows.Count];

            for (int i = 0; i < dT.Rows.Count; i++)
            {
                procs[i] = new ListViewItem();
                procs[i].Text = dT.Rows[i]["Name"].ToString();
                procs[i].SubItems.Add(dT.Rows[i]["Modified"].ToString());
            }

            return procs;
        }

        public Aga.Controls.Tree.TreeModel GetSchemaModel()
        {

            TRmodel = new TreeModel();


            //GET ALL TABLES
            DataTable t = null;

            //ASK FOR COLUMN INFO
            DataTable cols = null;

            Node table = null;

            string fieldType = "";
            string fieldSize = "";
            int fieldDelim, fieldDelimE = 0;
            try
            {
                //if (Connection == null)
                //    return null;

                //GET ALL TABLES
                string k =  GetString("show tables");
                string errorRESP,affected = "";
                //Console.WriteLine(k);
                t = parseJSON(k, out affected,out errorRESP);
                //t = Connection.GetDATATABLE("show tables");

                if (t == null)
                    return null;

                //FOR EACH TABLE
                foreach (DataRow item in t.Rows)
                {
                    //CREATE NEW NODE
                    table = new Node();
                    table = (Node)new treeItem(item[0].ToString(), "", "", false, false, "", 0, imageList);



                    //ASK FOR COLUMN INFO
                    //SHOW COLUMNS FROM mydb.mytable;
                    //string err = null;
                    //cols = Connection.GetDATATABLEex("SHOW COLUMNS FROM " + General.Connections[ConnIndex].dbaseName + "." + item[0].ToString(), out err);
                    k = GetString("SHOW COLUMNS FROM " + item[0].ToString());
                    cols = parseJSON(k, out affected, out errorRESP);

                    if (cols == null)
                    {
                        sendMessage(this, new MyEventArgs("Couldnt fetch column info for " + item[0].ToString(), true));
                        continue;
                    }

                    //FOR EACH COLUMN
                    foreach (DataRow colItem in cols.Rows)
                    {
                        fieldDelim = colItem["TYPE"].ToString().IndexOf("(");
                        fieldDelimE = colItem["TYPE"].ToString().IndexOf(")");

                        if (fieldDelim > -1 && fieldDelimE > fieldDelim)
                        {

                            fieldType = colItem["TYPE"].ToString().Substring(0, fieldDelim);
                            fieldDelim += 1;
                            fieldSize = colItem["TYPE"].ToString().Substring(fieldDelim, fieldDelimE - fieldDelim); //colItem["TYPE"].ToString().Length - (fieldDelim + 2));
                        }
                        else
                        {
                            fieldType = colItem["TYPE"].ToString();
                            fieldSize = "";
                        }


                        if (colItem["Key"].ToString().ToUpper() == "PRI")
                        {
                            table.Nodes.Add(new treeItem(colItem["Field"].ToString(), fieldType, fieldSize, colItem["Null"].ToString().Length > 2 ? true : false, false, ConvertMySQL2fieldType(fieldType), 2, imageList));
                        }
                        else if (colItem["Key"].ToString().ToUpper() == "MUL")
                            table.Nodes.Add(new treeItem(colItem["Field"].ToString(), fieldType, fieldSize, colItem["Null"].ToString().Length > 2 ? true : false, false, ConvertMySQL2fieldType(fieldType), 3, imageList));
                        else
                            table.Nodes.Add(new treeItem(colItem["Field"].ToString(), fieldType, fieldSize, colItem["Null"].ToString().Length > 2 ? true : false, false, ConvertMySQL2fieldType(fieldType), 1, imageList));
                    }

                    //ADD TABLE INFO TO TREE
                    TRmodel.Nodes.Add(table);
                }
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message, MessageBoxIcon.Error);
            }
            finally
            {


                if (t != null)
                    t.Dispose();

                if (cols != null)
                    cols.Dispose();

            }


            if (TRmodel == null)
                TRmodel = new TreeModel();

            return TRmodel;

        }

        private string ConvertMySQL2fieldType(string SQLfieldType)
        {
            SQLfieldType = SQLfieldType.ToLower();

            switch (SQLfieldType)
            {
                case "int":
                    return "int";
                case "varchar":
                    return "string";
                case "decimal":
                    return "decimal";
                case "datetime":
                    return "DateTime";
                case "blob":
                    return "byte[]";
                case "binary":
                    return "byte[]";
                case "longblob":
                    return "byte[]";
                case "mediumblob":
                    return "byte[]";
                case "tinyblob":
                    return "byte[]";
                case "varbinary":
                    return "byte[]";
                case "date":
                    return "DateTime";
                case "time":
                    return "TimeSpan";
                case "timestamp":
                    return "DateTime";
                case "year":
                    return "int";
                case "geometry":
                    return "byte[]";
                case "geometrycollection":
                    return "byte[]";
                case "linestring":
                    return "byte[]";
                case "multilinestring":
                    return "byte[]";
                case "multipoint":
                    return "byte[]";
                case "multipolygon":
                    return "byte[]";
                case "point":
                    return "byte[]";
                case "polygon":
                    return "byte[]";
                case "bigint":
                    return "int";
                case "double":
                    return "Double";
                case "float":
                    return "Double";
                case "mediumint":
                    return "int";
                case "smallint":
                    return "int";
                case "tinyint":
                    return "int";
                case "char":
                    return "string";
                case "longtext":
                    return "string";
                case "mediumtext":
                    return "string";
                case "text":
                    return "string";
                case "tinytext":
                    return "string";
                case "bit":
                    return "Boolean";
                default:
                    return "object";
            }
        }

        //private void dd(string sql)
        //{
        //    var handler = new HttpHandler();

        //    var url = General.Connections[ConnIndex].serverName; // +"?sql={\"q\": \"" + General.SafeJSON(sql) + "\"}&p=" + General.Connections[ConnIndex].password;

        //    //NameValueCollection m = new NameValueCollection();

        //    //m.Add("Authorization", "token " + toolStripToken.Text);

        //    Dictionary<string,string> kk = new Dictionary<string,string>();

        //    kk.Add("sql","{\"q\": \"" + General.SafeJSON(sql) + "\"}");
        //    kk.Add("p", General.Connections[ConnIndex].password);

        //    //Asynchronous request sent
        //    handler.RequestCompleted += delGistCompleted;
        //    handler.Post(url, kk);
        //}

        //delegate void delGistCompletedCallback(object sender, GenericEventArgs<ResponseArgs> e);
        //private void delGistCompleted(object sender, GenericEventArgs<ResponseArgs> e)
        //{

        //    //if (TR.InvokeRequired)
        //    //{
        //    //    TR.BeginInvoke(new delGistCompletedCallback(this.delGistCompleted), new object[] { sender, e });
        //    //    return;
        //    //}

        //    if (e.Value.error != null)
        //    {
        //        MessageBox.Show(e.Value.error, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //    }
        //    else
        //        Console.WriteLine(e.Value.httpwebResponse);

        //    //if (204 == (int)e.Value.StatusCode)
        //    //{
        //    //    //System.Threading.Thread.Sleep(200);
        //    //    refreshLists();//toolStripRefresh.PerformClick();
        //    //}
        //    //else
        //    //    MessageBox.Show("Gist response is invalid (statuscode != 204)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        //    //Cursor = System.Windows.Forms.Cursors.Default;
        //}

        public System.Data.DataTable ExecuteSQL(string sSQL, out string rowsAffected, out string error)
        {

            DataTable dataSet = null;

            rowsAffected = "";
            error = "";
            

            try
            {
                //dd(sSQL);
                string k = "";
                using (var wb = new WebClient())
                {
                    var parameters = new NameValueCollection();
                    parameters["sql"] = "{\"q\": \"" + General.SafeJSON(sSQL) + "\"}";
                    parameters["p"] = General.Connections[ConnIndex].password;

                    var response = wb.UploadValues(General.Connections[ConnIndex].serverName, "POST", parameters);
                    k = Encoding.UTF8.GetString(response);
                }

                //var handler = new HttpHandler();

                //var url = General.Connections[ConnIndex].serverName;
                //Dictionary<string, string> parameters = new Dictionary<string, string>();

                //parameters.Add("sql", "{\"q\": \"" + General.SafeJSON(sSQL) + "\"}");
                //parameters.Add("p", General.Connections[ConnIndex].password);

                //var response = handler.Get(url, parameters);
                //Console.WriteLine(response);

                //using (Stream stream = response.GetResponseStream())
                //{
                //    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                //    String responseString = reader.ReadToEnd();
                //    Console.WriteLine(responseString);
                //}

                //string k = GetString(sSQL);
                string errorRESP = "";
                string affected = "";

                dataSet = parseJSON(k, out affected, out errorRESP);
                
                error=errorRESP;
                rowsAffected = affected;

                //if (dataSet.Rows.Count > 0)
                //    rowsAffected = dataSet.Rows.Count.ToString();

                return dataSet;

                //dataSet = Connection.GetDATASET2(sSQL, out error);

                //if (dataSet == null)
                //    return null;
                //if (dataSet.Tables.Count > 0)
                //{
                //    if (dataSet.Tables[0].Rows.Count > 0)
                //        rowsAffected = dataSet.Tables[0].Rows.Count.ToString();


                //    return dataSet.Tables[0];
                //}
                //else
                //    return null;
            }
            catch (Exception ex)
            {
                error = (ex.Message + " ");
            }
            finally
            {
                if (dataSet != null)
                    dataSet.Dispose();
            }

            return null;
        }

        public string GenerateParameterInsert(Aga.Controls.Tree.TreeNodeAdv table)
        {
            throw new NotImplementedException();
        }

        public string GenerateParameterUpdate(Aga.Controls.Tree.TreeNodeAdv table)
        {
            throw new NotImplementedException();
        }

        public string GenerateSelect100(string table)
        {
            return "SELECT * FROM " + table + " LIMIT 100";
        }


        public string ExecuteScalar(string SQL)
        {
            string tmp="";
            string err="";
            DataTable dT= ExecuteSQL(SQL,out  tmp, out err);

            if (dT != null && dT.Rows.Count > 0)
                return dT.Rows[0][0].ToString();
            else
                return "";
            //return Connection.ExecuteSQLScalar(SQL).ToString();
        }

        public string UpdateGrid(DataGridView DG)
        {
            try
            {
                return "";
                //string sql = null;
                //MySqlDataAdapter da;
                //DataTable dt;
                //int x = 0;

                //// Setup SQL (This is the original table where the data was retrieved when grid was filled, it will also be the table updated)
                //sql = DG.Tag.ToString();
                ////"select * from " & MainForm.tr.SelectedNode.Text.Substring(0, MainForm.tr.SelectedNode.Text.LastIndexOf("[") - 1)

                //// Setup DataAdapter
                //da = new MySqlDataAdapter(sql, Connection.GetConnection());

                //// Create a command builder (this is needed for the update to work)
                //MySqlCommandBuilder cb = new MySqlCommandBuilder(da);

                //// Get Current Data from datagrid
                //dt = (DataTable)DG.DataSource;

                //// Update Table through DataAdapter
                //x = da.Update(dt);

                //return (x.ToString() + " record(s) updated.");


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        #region " EVENT "

        event EventHandler<MyEventArgs> sendMessage;

        event EventHandler<MyEventArgs> IdbType.AddMessage
        {

            add
            {
                if (sendMessage != null)
                {
                    lock (sendMessage)
                    {
                        sendMessage += value;
                    }
                }
                else
                {
                    sendMessage = new EventHandler<MyEventArgs>(value);
                }
            }
            remove
            {
                if (sendMessage != null)
                {
                    lock (sendMessage)
                    {
                        sendMessage -= value;
                    }
                }
            }
        }

        #endregion



        public string generatePROCselect(string tablename, List<string> fields, string PK)
        {
            throw new NotImplementedException();
        }


        public string generatePROCinsert(string tablename, List<ListStrings> fields)
        {
            //string dbase = General.Connections[connectionIndex].dbaseName;
            //FOR MYSQL PHP string str = "DELIMITER $$\r\n\r\nCREATE PROCEDURE `" + tablename + "_add` ";
            string str = "CREATE PROCEDURE `" + tablename + "_add` ";
            string flds = "";
            string fldsParams = "";
            string procParams = "";

            if (fields.Count == 0)
                flds = "* ";
            else
                foreach (ListStrings item in fields)
                {
                    flds += item.item1 + ",";
                    fldsParams += item.item1.ToLower() + "VAR,";

                    if (item.item2.ToLower().Contains("varchar") || item.item2.ToLower().Contains("nvarchar") || item.item2.ToLower().Contains("text"))
                        procParams += "IN `" + item.item1.ToLower() + "VAR` " + item.item2 + "  CHARSET utf8,";
                    else 
                        procParams += "IN `" + item.item1.ToLower() + "VAR` " + item.item2 + ",";
                    // CHARSET utf8
                }

            return str + "(" + procParams.Substring(0, procParams.Length - 1) + ") \r\nBEGIN \r\n" +
                    "INSERT INTO " + tablename + "(" + flds.Substring(0, flds.Length - 1) + ") VALUES (" + fldsParams.Substring(0, fldsParams.Length - 1) + ");\r\n\r\nEND";

            //FOR MYSQL PHP
            //return str + "(" + procParams.Substring(0, procParams.Length - 1) + ") \r\nBEGIN \r\n" +
            //    "INSERT INTO " + tablename + "(" + flds.Substring(0, flds.Length - 1) + ") VALUES (" + fldsParams.Substring(0, fldsParams.Length - 1) + ")\r\n\r\nEND IF;\r\n\r\nEND$$\r\n\r\nDELIMITER ;";

            //throw new NotImplementedException();
        }


        public string generatePROCupdate(string tablename, List<ListStrings> fields, string PK)
        {
            //string dbase = General.Connections[connectionIndex].dbaseName;
            string str = "CREATE PROCEDURE `" + tablename + "_update`\r\n";
            string where = "\r\n( IN `rec_idVAR` INT,";
            string flds = "";
            string procParams = where;

            if (fields.Count == 0)
                flds = "* ";
            else
                foreach (ListStrings item in fields)
                {
                    flds += item.item1 + " = " + item.item1.ToLower() + "VAR,";

                    if (item.item2.ToLower().Contains("varchar") || item.item2.ToLower().Contains("nvarchar") || item.item2.ToLower().Contains("text"))
                        procParams += "IN `" + item.item1.ToLower() + "VAR` " + item.item2 + "  CHARSET utf8,";
                    else
                        procParams += "IN `" + item.item1.ToLower() + "VAR` " + item.item2 + ",";

                    //procParams += "@" + item.item1.ToLower() + " " + item.item2 + ",";
                }


            return str + procParams.Substring(0, procParams.Length - 1) + ")\r\nBEGIN\r\n" +
                "UPDATE " + tablename + " SET " + flds.Substring(0, flds.Length - 1) + "\r\n" +
                " WHERE " + PK + " =  rec_idVAR;" + "\r\n\r\nEND";
        }


        public string generatePROCdelete(string tblName, string PK)
        {
            string str = "CREATE PROCEDURE `" + tblName + "_delete`\r\n" +
                "\r\n(IN `rec_idVAR` INT)";

            return str + "BEGIN\r\nDELETE FROM " + tblName + " WHERE " + PK + " = rec_idVAR;\r\nEND";
        }


        public string generatePROCnodeJS(string procName)
        {
            throw new NotImplementedException();
        }


        public List<string> getTables()
        {
            List<string> tbls = new List<string>();

            string k = GetString("SHOW TABLES");
            string affected, errorRESP = "";
            DataTable dT = parseJSON(k, out affected, out errorRESP);

            if (dT == null)
                return null;

            //ListViewItem[] procs = new ListViewItem[dT.Rows.Count];

            for (int i = 0; i < dT.Rows.Count; i++)
            {
                tbls.Add( dT.Rows[i][0].ToString());

            }

            dT.Dispose();

            return tbls;

            //return Connection.getTables();
            //List<string> tbls = new List<string>();

            //DataTable dT;

            //dT = Connection.GetDATATABLE("SHOW TABLES");
            ////dT.DefaultView.Sort = "TABLE_NAME";

            //foreach (DataRowView dR in dT.DefaultView)
            //{

            //    //  if (dR["TABLE_TYPE"].ToString().ToLower() == "table")
            //    tbls.Add(dR[0].ToString());
            //}

            //dT.Dispose();

            //return tbls;

        }


        public IDbConnection getConnection()
        {
            throw new NotImplementedException();
        }


        public string generateFORM(string procName, string formName)
        {
            throw new NotImplementedException();
        }


        public string generateFORMboostrap(string procName, string formName)
        {
            throw new NotImplementedException();
        }


        public string GenerateCountRows(string table)
        {
            return "SELECT COUNT(*) FROM " + table;
        }


        public string GenerateLast100(string table, string ID)
        {
            return "SELECT * FROM " + table + " ORDER BY " + ID + " DESC LIMIT 100";
        }


        public List<string> getTableFields(string table)
        {
            List<string> tbls = new List<string>();

            string k = GetString("SHOW COLUMNS FROM " + table);
            string affected, errorRESP = "";
            DataTable dT = parseJSON(k, out affected, out errorRESP);

            if (dT == null)
                return null;

            //ListViewItem[] procs = new ListViewItem[dT.Rows.Count];

            for (int i = 0; i < dT.Rows.Count; i++)
            {
                tbls.Add(dT.Rows[i][0].ToString());

            }

            dT.Dispose();

            return tbls;

            //List<string> tbls = new List<string>();

            //DataTable dT_Fields;

            //dT_Fields = Connection.GetDATATABLE("SHOW COLUMNS FROM " + table);

            //for (int i = 0; i < dT_Fields.Rows.Count; i++)
            //{
            //    tbls.Add(dT_Fields.Rows[i][0].ToString());
            //}

            //dT_Fields.Dispose();

            //return tbls;
        }


        public DataTable getDatatable(string q)
        {
            string tmp = "";
            string err = "";
            DataTable dT = ExecuteSQL(q, out  tmp, out err);

            if (dT != null && dT.Rows.Count > 0)
                return dT;
            else
                return null;
        }



        public string generateTableScript(string table)
        {
            string tmp = null;
            string err = null;
            DataTable cols = ExecuteSQL("SHOW CREATE TABLE " + table, out tmp, out err);

            if (cols == null)
                return "";

            return cols.Rows[0][1].ToString();

            //string tmp = null;
            //string err = null;

            ////DataTable cols = Connection.GetDATATABLEex("SHOW COLUMNS FROM " + General.Connections[ConnIndex].dbaseName + "." + table, out err);
            //DataTable cols = ExecuteSQL("SHOW COLUMNS FROM " + General.Connections[ConnIndex].dbaseName + "." + table,out tmp, out err);

            //if (cols == null)
            //    return "";

            //List<string> list = new List<string>();
            //int fieldDelim, fieldDelimE = 0;
            //string fieldType, fieldSize = "";

            //foreach (DataRow colItem in cols.Rows)
            //{
            //    fieldDelim = colItem["TYPE"].ToString().IndexOf("(");
            //    fieldDelimE = colItem["TYPE"].ToString().IndexOf(")");

            //    if (fieldDelim > -1 && fieldDelimE > fieldDelim)
            //    {

            //        fieldType = colItem["TYPE"].ToString().Substring(0, fieldDelim);
            //        fieldDelim += 1;
            //        fieldSize = colItem["TYPE"].ToString().Substring(fieldDelim, fieldDelimE - fieldDelim);
            //    }
            //    else
            //    {
            //        fieldType = colItem["TYPE"].ToString();
            //        fieldSize = "";
            //    }

            //    list.Add("\t" + colItem["Field"].ToString() + ' ' + fieldType + '(' + fieldSize + ')');
            //}

            //return "CREATE TABLE " + table + " (\r\n" + string.Join(",\r\n", list.ToArray()) + "\r\n)";

        }


        public bool optionsShowRestoreScript()
        {
            return false;
        }

        public bool optionsProceduresFunctions()
        {
            return true;
        }

    }

}
