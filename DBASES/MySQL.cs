using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aga.Controls.Tree;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;

namespace DBManager.DBASES
{
    class MySQL : IdbType
    {
        internal MySQLClass Connection = null;
        internal string ConnectionString;
        internal TreeModel TRmodel;
        internal ImageList imageList;
        internal int ConnIndex = 0;

        public MySQL(int index, ImageList imageList)
        {
            // ":" + General.Connections[index].port +
            ConnectionString = "Data Source=" + General.Connections[index].serverName + 
                    ";Initial Catalog=" + General.Connections[index].dbaseName +
                    ";User ID=" + General.Connections[index].user +
                     ";Password=" + General.Connections[index].password;

            if (General.Connections[index].port != "3306")
                ConnectionString += ";Port=" + General.Connections[index].port;

            ConnIndex = index;

            this.imageList = imageList;
        }

        public string Connect()
        {
            MySqlException err = null;
            Connection = new MySQLClass(ConnectionString, out err);
            
            if (err != null)
            {
                Connection = null;
                return err.Message;
            }
            else
                return "";
        }

        public void Disconnect()
        {
            if (Connection != null)
                Connection.Close();
        }

        public string parseProcedure(string procName, bool replaceCreate)
        {
            return "";
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
                if (Connection == null)
                    return null;

                //GET ALL TABLES
                t = Connection.GetDATATABLE("show tables");

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
                    string err = null;
                    cols = Connection.GetDATATABLEex("SHOW COLUMNS FROM " + General.Connections[ConnIndex].dbaseName + "." + item[0].ToString(), out err);

                    if (cols == null)
                    {
                        sendMessage(this, new MyEventArgs(err, true));
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
                            fieldSize = colItem["TYPE"].ToString().Substring(fieldDelim , fieldDelimE - fieldDelim); //colItem["TYPE"].ToString().Length - (fieldDelim + 2));
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

        public System.Data.DataTable ExecuteSQL(string sSQL, out string rowsAffected, out string error)
        {

            DataSet dataSet = null;

            rowsAffected = "";
            error = "";


            try
            {
                dataSet = Connection.GetDATASET2(sSQL, out error);

                if (dataSet == null)
                    return null;
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                        rowsAffected = dataSet.Tables[0].Rows.Count.ToString();


                    return dataSet.Tables[0];
                }
                else
                    return null;
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
            return Connection.ExecuteSQLScalar(SQL).ToString();
        }

        public string UpdateGrid(DataGridView DG)
        {
            try
            {
                string sql = null;
                MySqlDataAdapter da;
                DataTable dt;
                int x = 0;

                // Setup SQL (This is the original table where the data was retrieved when grid was filled, it will also be the table updated)
                sql = DG.Tag.ToString();
                //"select * from " & MainForm.tr.SelectedNode.Text.Substring(0, MainForm.tr.SelectedNode.Text.LastIndexOf("[") - 1)

                // Setup DataAdapter
                da = new MySqlDataAdapter(sql, Connection.GetConnection());

                // Create a command builder (this is needed for the update to work)
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da);

                // Get Current Data from datagrid
                dt = (DataTable)DG.DataSource;

                // Update Table through DataAdapter
                x = da.Update(dt);

                return (x.ToString() + " record(s) updated.");


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
            string str = "CREATE PROCEDURE `" + tablename + "_list` " + (PK.Length > 0 ? "\r\n(IN `rec_idVAR` INT)" : "()");
            string flds = "";

            if (fields.Count == 0)
                flds = "* ";
            else
                foreach (string item in fields)
                {
                    flds += item + ",";
                }

            if (PK.Length > 0)
                return str + "\r\nBEGIN \r\n" +
                    "SELECT " + flds.Substring(0, flds.Length - 1) + " FROM " + tablename + " WHERE " + PK + " = " + "rec_idVAR;\r\n\r\nEND";
            else
            return str + "\r\nBEGIN \r\n" +
                    "SELECT " + flds.Substring(0, flds.Length - 1) + " FROM " + tablename + ";\r\n\r\nEND";

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
                        procParams += "IN `" + item.item1.ToLower() + "VAR` " + item.item2 + " CHARSET utf8mb4,";
                    else
                        procParams += "IN `" + item.item1.ToLower() + "VAR` " + item.item2 + ",";
                    // CHARSET utf8
                }

            return str + "(" + procParams.Substring(0, procParams.Length - 1) + ") \r\nBEGIN \r\n" +
                    "INSERT INTO " + tablename + "(" + flds.Substring(0, flds.Length - 1) + ") VALUES (" + fldsParams.Substring(0, fldsParams.Length - 1) + ");\r\n\r\nEND";
        }


        public string generatePROCMerge(string tablename, List<ListStrings> fields)
        { 
            string str = "CREATE PROCEDURE `" + tablename + "_merge` ";
            string flds = "";
            string fldsParams = "";
            string procParams = "";
            string duplicParams = "";
            string bindParams = "";
            string vals = "";

            if (fields.Count == 0)
                flds = "* ";
            else
                foreach (ListStrings item in fields)
                {
                    flds += item.item1 + ", ";
                    fldsParams += item.item1.ToLower() + "VAR,";
                    bindParams += ":" + item.item1 + ", ";
                    duplicParams += item.item1 + "=:" + item.item1 + ", ";
                    vals += item.item1 + "=" + item.item1.ToLower() + "VAR, ";

                    if (item.item2.ToLower().Contains("varchar") || item.item2.ToLower().Contains("nvarchar") || item.item2.ToLower().Contains("text"))
                        procParams += "IN `" + item.item1.ToLower() + "VAR` " + item.item2 + " CHARSET utf8mb4,";
                    else
                        procParams += "IN `" + item.item1.ToLower() + "VAR` " + item.item2 + ",";
                    // CHARSET utf8
                }

            flds = flds.Substring(0, flds.Length - 2);
            duplicParams = duplicParams.Substring(0, duplicParams.Length - 2);
            fldsParams = fldsParams.Substring(0, fldsParams.Length - 1);
            bindParams = bindParams.Substring(0, bindParams.Length - 2);
            vals = vals.Substring(0, vals.Length - 2);
            return str + "(" + procParams.Substring(0, procParams.Length - 1) + ") \r\nBEGIN \r\n" +
                    "INSERT INTO " + tablename + "(" + flds + ") VALUES (" + fldsParams + ") \r\nON DUPLICATE KEY UPDATE " + vals + ";\r\n\r\nEND\r\n" +
                    "/*\r\nraw SQL with bind (ref - https://dev.mysql.com/doc/refman/5.7/en/insert-on-duplicate.html)\r\nINSERT INTO " + tablename + "(" + flds + ") VALUES (" + bindParams + ") \r\nON DUPLICATE KEY UPDATE " + duplicParams + "\r\n*/";

        }

        public string generatePROCupdate(string tablename, List<ListStrings> fields, string PK)
        {
            //string dbase = General.Connections[connectionIndex].dbaseName;
            string str = "CREATE PROCEDURE `" + tablename + "_update`\r\n";
            string where = "\r\n(IN `rec_idVAR` INT,";
            string flds = "";
            string procParams = where;

            if (fields.Count == 0)
                flds = "* ";
            else
                foreach (ListStrings item in fields)
                {
                    flds += item.item1 + " = " + item.item1.ToLower() + "VAR,";

                    if (item.item2.ToLower().Contains("varchar") || item.item2.ToLower().Contains("nvarchar") || item.item2.ToLower().Contains("text"))
                        procParams += "IN `" + item.item1.ToLower() + "VAR` " + item.item2 + " CHARSET utf8mb4,";
                    else
                        procParams += "IN `" + item.item1.ToLower() + "VAR` " + item.item2 + ",";

                    //procParams += "@" + item.item1.ToLower() + " " + item.item2 + ",";
                }


            return str + procParams.Substring(0, procParams.Length - 1) + ")\r\nBEGIN\r\n" +
                "UPDATE " + tablename + " SET " + flds.Substring(0, flds.Length - 1) + "\r\n" +
                " WHERE " + PK + " = rec_idVAR;" + "\r\n\r\nEND";
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
            //return Connection.getTables();
            List<string> tbls = new List<string>();

            DataTable dT;

            dT = Connection.GetDATATABLE("SHOW TABLES");
            //dT.DefaultView.Sort = "TABLE_NAME";

            foreach (DataRowView dR in dT.DefaultView)
            {

                //  if (dR["TABLE_TYPE"].ToString().ToLower() == "table")
                tbls.Add(dR[0].ToString());
            }

            dT.Dispose();

            return tbls;

        }


        public IDbConnection getConnection()
        {
            return Connection.GetConnection();
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

            DataTable dT_Fields;

            dT_Fields = Connection.GetDATATABLE("SHOW COLUMNS FROM " + table);

            for (int i = 0; i < dT_Fields.Rows.Count; i++)
            {
                tbls.Add(dT_Fields.Rows[i][0].ToString());
            }

            dT_Fields.Dispose();

            return tbls;
        }


        public DataTable getDatatable(string q)
        {
            return Connection.GetDATATABLE(q);
        }



        public string generateTableScript(string table)
        {
            DataTable dT = Connection.GetDATATABLE("SHOW CREATE TABLE " + table);

            return dT.Rows[0][1].ToString();
     
            //string err = null;
            
            //           DataTable cols = Connection.GetDATATABLEex("SHOW COLUMNS FROM " + General.Connections[ConnIndex].dbaseName + "." + table, out err);
            
            //if (cols == null)
            //    return "";

            //List<string> list = new List<string>();
            //int fieldDelim, fieldDelimE = 0;
            //string fieldType,fieldSize="";

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

            //    list.Add("\t" + colItem["Field"].ToString() + ' ' + fieldType + '(' +  fieldSize + ')');
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
