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
    class SQLServerTunnel : IdbType
    {
        public delegate void IsConnected(string isSuccess);
        public event IsConnected isConnected;

        internal TreeModel TRmodel;
        internal ImageList imageList;
        internal int ConnIndex = 0;

        public SQLServerTunnel(int index, ImageList imageList)
        {
            ConnIndex = index;

            ////enable HTTPS - avoid "The request was aborted: Could not create SSL/TLS secure channel" - https://stackoverflow.com/a/51346252

            if (General.Connections[ConnIndex].serverName.ToLower().StartsWith("https"))
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            else
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(48 | 192);

            this.imageList = imageList;
        }

        public SQLServerTunnel() { }

        public string ReadServer_and_Unwrap(string sql)
        {
            using (var wb = new WebClient())
            {
                var parameters = new NameValueCollection();
                parameters["sql"] = "{\"q\": \"" + General.SafeJSON(sql) + "\"}";
                parameters["p"] = General.Connections[ConnIndex].password;

                var response = wb.UploadValues(General.Connections[ConnIndex].serverName, "POST", parameters);
                return unwrap(Encoding.UTF8.GetString(response));
            }

        }

        public string GetString(string sql)
        {
            return ReadServer_and_Unwrap(General.SafeJSON(sql));

        }


        private string unwrap(string server_resp)
        {
            if (string.IsNullOrEmpty(server_resp))
                return null;

            if (server_resp.StartsWith("Error:"))
            {
                return server_resp;
            }

            JObject JData = JObject.Parse(server_resp);

            if (JData == null)
                return null;

            if (JData["data"] == null)
                return null;

            if (JData["compression"] == null)
                return null;

            string output = "";
            if (JData["compression"].ToString() == "bzip")
                output = General.DecompressStringBZIP(JData["data"].ToString());
            else if (JData["compression"].ToString() == "gzip")
                output = General.DecompressStringGZIP(JData["data"].ToString());
            else if (JData["compression"].ToString() == "none")
                output = General.Base64Decode(JData["data"].ToString());

            return output;
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

            if (JData["result"] == null)
                return null;

            if (JData["affected"] != null)
                rowsaffected = JData["affected"].ToString();


            DataTable TBL = null;
            DataColumn col = null;
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
                    foreach (KeyValuePair<string, object> kvp in tmp)
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

            return TBL;
        }

        public Dictionary<string, object> Parse2(string array, bool internCALL = false)
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

        public void testConnection(string url, string password)
        {
            using (var wb = new WebClient())
            {
                var parameters = new NameValueCollection();
                parameters["sql"] = "{\"q\": \"" + "testconnection" + "\"}";
                parameters["p"] = password;

                var response = wb.UploadValues(url, "POST", parameters);
                string k = Encoding.UTF8.GetString(response);

                if (isConnected != null)
                {
                    if (k != "true")
                        isConnected(k);
                    else
                        isConnected("ok");

                }
                else
                    isConnected(k);
            }

        }


        public string Connect()
        {
            return "";
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
            return null;
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
                //string k =  GetString("SELECT sobjects.name FROM sysobjects sobjects WHERE sobjects.xtype = 'U' order by name");
                string k = GetString("dbschema");
                string errorRESP, affected = "";
                //Console.WriteLine(k);
                t = parseJSON(k, out affected, out errorRESP);
                //t = Connection.GetDATATABLE("show tables");

                if (t == null)
                    return null;



                //FOR EACH TABLE
                foreach (DataRow item in t.Rows)
                {
                    //CREATE NEW NODE
                    table = new Node();
                    table = (Node)new treeItem(item[0].ToString(), "", "", false, false, "", 0, imageList);

                    cols = parseJSON("{\"result\": " + item[1].ToString() + "}", out affected, out errorRESP);

                    if (cols == null)
                    {
                        sendMessage(this, new MyEventArgs("Couldnt fetch column info for " + item[0].ToString(), true));
                        continue;
                    }

                    //FOR EACH COLUMN
                    foreach (DataRow colItem in cols.Rows)
                    {

                        fieldType = colItem["TYPE_NAME"].ToString();
                        fieldSize = colItem["PRECISION"].ToString(); ;

                        //if (colItem["Key"].ToString().ToUpper() == "PRI")
                        //{
                        //    table.Nodes.Add(new treeItem(colItem["Field"].ToString(), fieldType, fieldSize, colItem["Null"].ToString().Length > 2 ? true : false, false, ConvertMySQL2fieldType(fieldType), 2, imageList));
                        //}
                        //else if (colItem["Key"].ToString().ToUpper() == "MUL")
                        //    table.Nodes.Add(new treeItem(colItem["Field"].ToString(), fieldType, fieldSize, colItem["Null"].ToString().Length > 2 ? true : false, false, ConvertMySQL2fieldType(fieldType), 3, imageList));
                        //else
                        table.Nodes.Add(new treeItem(colItem["COLUMN_NAME"].ToString(), fieldType, fieldSize, colItem["NULLABLE"].ToString() == "0" ? false : true, false, ConvertMySQL2fieldType(fieldType), 1, imageList));
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


        public System.Data.DataTable ExecuteSQL(string sSQL, out string rowsAffected, out string error)
        {

            DataTable dataSet = null;

            rowsAffected = "";
            error = "";


            try
            {
                //dd(sSQL);
                string k = ReadServer_and_Unwrap(sSQL);
                //using (var wb = new WebClient())
                //{
                //    var parameters = new NameValueCollection();
                //    parameters["sql"] = "{\"q\": \"" + General.SafeJSON(sSQL) + "\"}";
                //    parameters["p"] = General.Connections[ConnIndex].password;

                //    var response = wb.UploadValues(General.Connections[ConnIndex].serverName, "POST", parameters);
                //    k = Encoding.UTF8.GetString(response);
                //}

                //string k = GetString(sSQL);
                string errorRESP = "";
                string affected = "";

                dataSet = parseJSON(k, out affected, out errorRESP);

                error = errorRESP;
                rowsAffected = affected;

                return dataSet;
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
            return "SELECT TOP 100 * FROM [" + table + "]";
        }


        public string ExecuteScalar(string SQL)
        {
            string tmp = "";
            string err = "";
            DataTable dT = ExecuteSQL(SQL, out tmp, out err);

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

            for (int i = 0; i < dT.Rows.Count; i++)
            {
                tbls.Add(dT.Rows[i][0].ToString());

            }

            dT.Dispose();

            return tbls;
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
            return "SELECT TOP 100 * FROM [" + table + "] ORDER BY " + ID + " DESC";
        }


        public List<string> getTableFields(string table)
        {
            List<string> tbls = new List<string>();

            string k = GetString("SHOW COLUMNS FROM " + table);
            string affected, errorRESP = "";
            DataTable dT = parseJSON(k, out affected, out errorRESP);

            if (dT == null)
                return null;

            for (int i = 0; i < dT.Rows.Count; i++)
            {
                tbls.Add(dT.Rows[i][0].ToString());

            }

            dT.Dispose();

            return tbls;

        }


        public DataTable getDatatable(string q)
        {
            string tmp = "";
            string err = "";
            DataTable dT = ExecuteSQL(q, out tmp, out err);

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
