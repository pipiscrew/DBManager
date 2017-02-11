using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aga.Controls.Tree;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace DBManager.DBASES
{


    class SQLServer : IdbType
    {
        internal SQLClass Connection = null;
        internal string ConnectionString;
        internal TreeModel TRmodel;
        internal ImageList imageList;
        internal int connectionIndex = 0;

        public SQLServer(int index, ImageList imageList)
        {
            connectionIndex = index;
            ConnectionString = "Data Source=" + General.Connections[index].serverName +
                    ";Initial Catalog=" + General.Connections[index].dbaseName +
                    ";User ID=" + General.Connections[index].user +
                     ";Password=" + General.Connections[index].password;

            this.imageList = imageList;
        }


        public string Connect()
        {
            SqlException err = null;
            Connection = new SQLClass(ConnectionString, out err);

            if (err != null)
            {
                Connection = null;
                return err.Message;
            }
            else
                return "";
        }

        public List<string> getTables()
        {
            List<string> tbls = new List<string>();

            DataTable dT=null;

            try
            {
                dT = Connection.GetConnection().GetSchema("Tables");
                dT.DefaultView.Sort = "TABLE_NAME";


                foreach (DataRowView dR in dT.DefaultView)
                {

                    //  if (dR["TABLE_TYPE"].ToString().ToLower() == "table")
                    tbls.Add(dR["TABLE_NAME"].ToString());
                }


            }
            catch
            {
                //General.Mes("error!");
            }


            if (dT!=null)
                dT.Dispose();

            return tbls;

         //   return Connection.getTables();
        }

        public string parseProcedure(string procName, bool replaceCreate)
        {
            SqlDataReader sqlread;

            sqlread = Connection.GetDATAREADER("EXEC sp_helptext " + procName);

            //connection closed
            if (sqlread == null)
                return null;

            String append = "";
            while (sqlread.Read())
            {
                if (sqlread[0].ToString().Trim().Length > 0)
                {
                    if (sqlread[0].ToString().ToUpper().Contains("CREATE PROCEDURE "))
                    {
                        if (replaceCreate)
                            append += sqlread[0].ToString().Replace("CREATE PROCEDURE ", "ALTER PROCEDURE ");
                        else
                            append += sqlread[0].ToString();
                        // append += sqlread[0].ToString().Substring(sqlread[0].ToString().LastIndexOf("]")+1);
                    }
                    else
                        append += sqlread[0];
                }
            }

            sqlread.Close();
            sqlread.Dispose();

            return append;
        }

        public ListViewItem[] GetProcedures()
        {
            if (Connection == null)
                return null;

            DataTable dT = Connection.GetDATATABLE("select name,convert(CHAR(10),crdate,105) from sysobjects where xtype = 'p' AND CATEGORY=0 order by 1");

            ListViewItem[] procs = new ListViewItem[dT.Rows.Count];

            for (int i = 0; i < dT.Rows.Count; i++)
            {
                procs[i] = new ListViewItem();
                procs[i].Text = dT.Rows[i][0].ToString();
                procs[i].SubItems.Add(dT.Rows[i][1].ToString());
            }

            return procs;
        }

        public TreeModel GetSchemaModel()
        {
            if (Connection == null)
            {
                General.DB = null;
                return null;
            }

            TRmodel = new TreeModel();

            //lists the PR/FK keys 
            DataTable keys = null;
            DataRow[] dR_Keys = null;

            //GET ALL TABLES
            DataTable t = null;

            //SORT TABLES BY NAME
            DataRow[] filterResult = null;

            //ASK FOR COLUMN INFO
            DataTable cols = null;

            Node table = null;
            int iconIndex = 0;

            try
            {
                //GET ALL TABLES
                t = Connection.GetConnection().GetSchema("Tables");

                //SORT TABLES BY NAME
                filterResult = t.Select("[TABLE_TYPE] = 'BASE TABLE' ", "TABLE_NAME");

                //FOR EACH TABLE
                foreach (DataRow item in filterResult)
                {
                    //filter tables added for AZURE
                    if (General.Connections[connectionIndex].filename.Length > 0)
                    {
                        if (!item["TABLE_NAME"].ToString().ToLower().StartsWith(General.Connections[connectionIndex].filename.ToLower()))
                            continue;
                    }

                    //CREATE NEW NODE
                    table = new Node();
                    table = (Node)new treeItem(item["TABLE_NAME"].ToString(), "", "", false, false, "", 0, imageList);

                    //GET PRIMARY + FOREIGN KEYS
                    keys = Connection.GetDATATABLE("select 	c.COLUMN_NAME, " +
                                                    "CASE CONSTRAINT_TYPE " +
                                                    "WHEN 'FOREIGN KEY' THEN 3 " +
                                                    "WHEN 'PRIMARY KEY' THEN 2 " +
                                                    "END AS ISPRIMARY " +
                                                    "from 	INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk,  " +
                                                    "INFORMATION_SCHEMA.KEY_COLUMN_USAGE c  " +
                                                    "where 	pk.TABLE_NAME = '" + item["TABLE_NAME"].ToString() + "' " +
                                                    "and	(CONSTRAINT_TYPE = 'FOREIGN KEY' OR CONSTRAINT_TYPE = 'PRIMARY KEY') " +
                                                    "and	c.TABLE_NAME = pk.TABLE_NAME  " +
                                                    "and	c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME order by ISPRIMARY");



                    //ASK FOR COLUMN INFO
                    cols = Connection.GetDATATABLE("exec sp_Columns [" + item["TABLE_NAME"].ToString() + "]");

                    //FOR EACH COLUMN
                    foreach (DataRow colItem in cols.Rows)
                    {
                        dR_Keys = keys.Select("COLUMN_NAME='" + colItem["COLUMN_NAME"].ToString() + "'");
                        if (dR_Keys != null && dR_Keys.Length > 0)
                            iconIndex = int.Parse(dR_Keys[0]["ISPRIMARY"].ToString());
                        else
                            iconIndex = 1;//default field


                        table.Nodes.Add(new treeItem(colItem["COLUMN_NAME"].ToString(), colItem["TYPE_NAME"].ToString(), colItem["PRECISION"].ToString(), colItem["NULLABLE"].ToString() == "0" ? false : true, false, ConvertSQL2fieldType(colItem["TYPE_NAME"].ToString()), iconIndex, imageList));
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
                if (keys != null)
                    keys.Dispose();

                if (t != null)
                    t.Dispose();

                if (cols != null)
                    cols.Dispose();

                dR_Keys = null;

                filterResult = null;
            }


            if (TRmodel == null)
                TRmodel = new TreeModel();

            return TRmodel;

        }



        public DataTable ExecuteSQL(string sSQL, out string rowsAffected, out string error)
        {
            if (sSQL.Trim().ToUpper().StartsWith("ALTER PROCEDURE") || sSQL.Trim().ToUpper().StartsWith("CREATE PROCEDURE"))
            {
                string errPROC = null;
                DataSet dS = Connection.GetDATASET2(sSQL, out errPROC);
                error = errPROC;
                rowsAffected = "0";
                return null;
            }

            string err = null;
            DataSet ds = Connection.GetDATASET2(sSQL + ";SELECT ROWCOUNT_BIG()", out err);

            error = err;

            if (ds != null)
            {
                if (ds.Tables.Count == 2)
                {
                    if (ds.Tables[1].Rows.Count == 1)
                    {
                        int res;
                        if (int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out res))
                            rowsAffected = ds.Tables[1].Rows[0][0].ToString();
                        else
                            rowsAffected = "";
                    }
                    else
                        rowsAffected = "";
                }
                else if (ds.Tables.Count == 1)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        int res;
                        if (int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out res))
                            rowsAffected = ds.Tables[0].Rows[0][0].ToString();
                        else
                            rowsAffected = "";
                    }
                    else
                        rowsAffected = "";
                }
                else
                    rowsAffected = "";

                if (ds.Tables.Count > 1)
                    return ds.Tables[0];
                else
                {
                    rowsAffected = "";
                    return null;
                }
            }
            else
            {
                rowsAffected = "";
                return null;
            }
        }


        private string ConvertSQL2fieldType(string SQLfieldType)
        {
            //source : http://msdn2.microsoft.com/en-us/library/ms191530.aspx

            //LCASE conversion + " identity" string
            SQLfieldType = SQLfieldType.ToLower();
            SQLfieldType = SQLfieldType.Replace(" identity", "");
            //LCASE conversion + " identity" string


            switch (SQLfieldType)
            {
                case "char":
                    return "string";
                case "nchar":
                    return "string";
                case "varchar":
                    return "string";
                case "text":
                    return "string";
                case "nvarchar":
                    return "string";
                case "ntext":
                    return "string";
                case "decimal":
                    return "decimal";
                case "numeric":
                    return "decimal";
                case "bit":
                    return "Boolean";
                case "binary":
                    return "byte[]";
                case "varbinary":
                    return "byte[]";
                case "image":
                    return "Image";
                case "int":
                    return "int";
                case "smallint":
                    return "int";
                case "tinyint":
                    return "int";
                case "float":
                    return "Double";
                case "real":
                    return "Single";
                case "money":
                    return "decimal";
                case "smallmoney":
                    return "decimal";
                case "datetime":
                    return "DateTime";
                case "smalldatetime":
                    return "DateTime";
                default:
                    return "object";
            }
        }

        public string GenerateParameterInsert(TreeNodeAdv table)
        {
            string finalSQL1 = "";
            string finalSQL2 = "";
            string src = "		SqlCommand command = new SqlCommand($statement$, connection);\r\n\r\n";
            string fieldName;
            //string src = "";


            foreach (var node in table.Children)
            {
                if ((node.Tag as treeItem).nodeCheck && (node.Tag as treeItem).imageIndex != 2)
                {
                    fieldName = ((node.Tag as treeItem).nodeText);
                    //fieldType = GetOnlyFieldTypeSQLite(node.Text);

                    finalSQL1 += "\t\t\"[" + fieldName + "],\" +\r\n";
                    finalSQL2 += "@" + fieldName + ",";
                    src += "		command.Parameters.AddWithValue(\"@" + fieldName + "\", txt" + fieldName + ".Text)\r\n";

                }
            }

            if (finalSQL1.Length > 0 && finalSQL2.Length > 0)
            {
                string INST = "\"INSERT INTO [" + (table.Tag as treeItem).nodeText + "] (\" + \r\n\t\t" + finalSQL1.Substring(2, finalSQL1.Length - 8) + ") VALUES (" + finalSQL2.Substring(0, finalSQL2.Length - 1) + ");\"";

                src = src.Replace("$statement$", INST) + "\r\n		Int32 rowsAffected = command.ExecuteNonQuery();" + "\r\n";

                return src;
            }
            else
                return null;
        }


        public string GenerateParameterUpdate(TreeNodeAdv table)
        {
            string finalSQL = "";
            string src = "		SqlCommand command = new SqlCommand($statement$, connection);\r\n\r\n";
            string PK = "yourIDhere";
            string fieldName;

            foreach (var node in table.Children)
            {
                fieldName = ((node.Tag as treeItem).nodeText);

                if ((node.Tag as treeItem).nodeCheck && (node.Tag as treeItem).imageIndex != 2)
                {
                    finalSQL += "\t\t\"[" + fieldName + "] = ?,\" +\r\n";

                    src += "		command.Parameters.AddWithValue(\"@" + fieldName + "\", txt" + fieldName + ".Text)\r\n";
                }
                else if ((node.Tag as treeItem).imageIndex == 2)
                    PK = fieldName;
            }

            if (finalSQL.Length > 0)
            {

                string UPD = "\"UPDATE [" + (table.Tag as treeItem).nodeText + "] SET \" & _ \r\n" + finalSQL.Substring(0, finalSQL.Length - 6) + " where " + PK + "=yourIDhere;\"";

                src = src.Replace("$statement$", UPD) + "\r\n		Int32 rowsAffected = command.ExecuteNonQuery();" + "\r\n";

                return src;
            }
            else
                return null;
        }

        public string GenerateSelect100(string table)
        {
            return "SELECT TOP 100 * FROM [" + table + "]";
        }

        public string GenerateLast100(string table, string ID)
        {
            return "SELECT TOP 100 * FROM [" + table + "] ORDER BY " + ID + " DESC";
        }

        public string GenerateCountRows(string table)
        {
            return "SELECT COUNT(*) FROM " + table;
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
                SqlDataAdapter da;
                DataTable dt;
                int x = 0;

                // Setup SQL (This is the original table where the data was retrieved when grid was filled, it will also be the table updated)
                sql = DG.Tag.ToString();
                //"select * from " & MainForm.tr.SelectedNode.Text.Substring(0, MainForm.tr.SelectedNode.Text.LastIndexOf("[") - 1)

                // Setup DataAdapter
                da = new SqlDataAdapter(sql, Connection.GetConnection());

                // Create a command builder (this is needed for the update to work)
                SqlCommandBuilder cb = new SqlCommandBuilder(da);

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

        public string generatePROCselect(string tablename, List<string> fields, string PK)
        {

            string dbase = General.Connections[connectionIndex].dbaseName;
            string str = "CREATE PROCEDURE [dbo].[p_" + dbase + "_" + tablename + "_list" + (PK.Length > 0 ? "where" : "") + "]";
            string where = "\r\n@" + tablename.ToLower() + "_" + PK + " INT\r\n";
            
            string flds = "";

            if (fields.Count == 0)
                flds = "* ";
            else
                foreach (string item in fields)
                    flds += item + ",";

            if (PK.Length > 0)
                return str + where + "\r\nAS\r\nSELECT " + flds.Substring(0, flds.Length - 1) + " FROM " + tablename + " WHERE " + PK + " = " + "@" + tablename.ToLower() + "_" + PK;
            else
                return str + "\r\nAS\r\nSELECT " + flds.Substring(0, flds.Length - 1) + " FROM " + tablename;

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



        public string generatePROCinsert(string tablename, List<ListStrings> fields)
        {
            string dbase = General.Connections[connectionIndex].dbaseName;
            string str = "CREATE PROCEDURE [dbo].[p_" + dbase + "_" + tablename + "_add]\r\n";
            string flds = "";
            string fldsParams = "";
            string procParams = "";

            if (fields.Count == 0)
                flds = "* ";
            else
                foreach (ListStrings item in fields)
                {
                    flds += item.item1 + ",";
                    fldsParams += "@" + item.item1.ToLower() + ",";
                    procParams += "@" + item.item1.ToLower() + " " + item.item2 + ",";
                }

            return str + procParams.Substring(0, procParams.Length - 1) + "\r\nAS\r\n" +
                "INSERT INTO " + tablename + "(" + flds.Substring(0, flds.Length - 1) + ") VALUES (" + fldsParams.Substring(0, fldsParams.Length - 1) + ")";

        }


        public string generatePROCupdate(string tablename, List<ListStrings> fields, string PK)
        {
            string dbase = General.Connections[connectionIndex].dbaseName;
            string str = "CREATE PROCEDURE [dbo].[p_" + dbase + "_" + tablename + "_update]\r\n";
            string where = "\r\n@" + tablename.ToLower() + "_" + PK + " INT,";
            string flds = "";
            string procParams = where;

            if (fields.Count == 0)
                flds = "* ";
            else
                foreach (ListStrings item in fields)
                {
                    flds += item.item1 + " = @" + item.item1.ToLower() + ",";
                    procParams += "@" + item.item1.ToLower() + " " + item.item2 + ",";
                }


            return str + procParams.Substring(0, procParams.Length - 1) + "\r\nAS\r\n" +
                "UPDATE " + tablename + " SET " + flds.Substring(0, flds.Length - 1) + "\r\n" +
                " WHERE " + PK + " = @" + tablename.ToLower() + "_" + PK;


        }


        public string generatePROCdelete(string tablename, string PK)
        {
            string dbase = General.Connections[connectionIndex].dbaseName;
            string str = "CREATE PROCEDURE [dbo].[p_" + dbase + "_" + tablename + "_delete]\r\n" +
                "\r\n@" + tablename.ToLower() + "_" + PK + " INT AS ";

            return str + "DELETE FROM " + tablename + " WHERE " + PK + " = " + "@" + tablename.ToLower() + "_" + PK;

        }


        public string generatePROCnodeJS(string procName)
        {
            string dbase = General.Connections[connectionIndex].dbaseName;

            string functionName;
            int pronPOS1 = procName.LastIndexOf("_");
            int pronPOS2 = procName.LastIndexOf("_", pronPOS1 - 1) + 1;
            functionName = procName.Substring(pronPOS2).ToLower();

            string tableName = functionName.Substring(0, functionName.IndexOf("_"));

            DataTable dT = Connection.GetDATATABLE("select * from information_schema.parameters where specific_name='" + procName + "'");
            string dbaseJS = "";
            string appJS = "";

            //////////////////////////////// when proc has paremeters
            //if (dT.Rows.Count > 0)
            //{
            //////////////////////////////////app.js
            //select with ID parameter
            if (procName.ToLower().EndsWith("_list") || procName.ToLower().EndsWith("_listwhere"))
            {
                appJS = General.ReadASCIIfromResources("DBManager.ResourceSQLServer.proc_node.appJS_get.txt");
                appJS = appJS.Replace("{{dbase}}", dbase)
                       .Replace("{{function}}", functionName)
                       .Replace("{{id}}", (dT.Rows.Count > 0 ? "/:id" : ""));
            }
            else
            {
                appJS = General.ReadASCIIfromResources("DBManager.ResourceSQLServer.proc_node.appJS_post.txt");
                appJS = appJS.Replace("{{dbase}}", dbase)
                       .Replace("{{function}}", functionName)
                       .Replace("{{id}}", (dT.Rows.Count > 0 ? "/:id" : ""));
            }



            //////////////////////////////////dbase.js
            if (procName.ToLower().EndsWith("_list") || procName.ToLower().EndsWith("_listwhere"))
            {
                dbaseJS = General.ReadASCIIfromResources("DBManager.ResourceSQLServer.proc_node.dbaseJS_get.txt");
                dbaseJS = dbaseJS.Replace("{{function}}", functionName)
                        .Replace("{{procname}}", procName)
                        .Replace("{{id1}}", (dT.Rows.Count > 0 ? " ?" : ""))
                        .Replace("{{id2}}", (dT.Rows.Count > 0 ? ", [params.id]" : ""));

            }
            else
            {
                dbaseJS = General.ReadASCIIfromResources("DBManager.ResourceSQLServer.proc_node.dbaseJS_post.txt");

                string qMarks = "";
                string param = "";
                string validation = "(" + getPROCvariablesValidation(dT, out qMarks, out param) + ") {";
                param = ", [" + param + "]";

                dbaseJS = dbaseJS.Replace("{{function}}", functionName)
                        .Replace("{{procname}}", procName)
                        .Replace("{{validation}}", validation)
                        .Replace("{{params}}", (dT.Rows.Count > 0 ? param : ""))
                        .Replace("{{qmarks}}", (dT.Rows.Count > 0 ? " ?," + qMarks : ""));
            }

            //}
            //else
            //{
            //    //template = General.ReadASCIIfromResources("DBManager.ResourceSQLServer.proc_node.appJS.txt");
            //    //template = template.Replace("{{dbase}}", dbase);
            //    //template = template.Replace("{{function}}", );
            //}

            string dbaseJSdeclaration = General.ReadASCIIfromResources("DBManager.ResourceSQLServer.proc_node.dbaseJS_declaration.txt");
            dbaseJSdeclaration = dbaseJSdeclaration.Replace("{{function}}", functionName);
            return appJS + dbaseJS + dbaseJSdeclaration;
        }

        public string generateFORM(string procName, string formName)
        {
            DataTable dT = Connection.GetDATATABLE("select * from information_schema.parameters where specific_name='" + procName + "'");

            string parameterName;
            string formTemplate = General.ReadASCIIfromResources("DBManager.ResourceSQLServer.proc_form_template.txt");
            string rowTemplate = General.ReadASCIIfromResources("DBManager.ResourceSQLServer.proc_form_template_from_row.txt");
            string rowTemplateAppender = "";
            string submitFields = "";
            foreach (DataRow item in dT.Rows)
            {
                parameterName = item["PARAMETER_NAME"].ToString().Substring(1).ToLower();
                rowTemplateAppender += rowTemplate.Replace("{{paramname}}", parameterName);
                submitFields += "                                    \"" + parameterName + "\": $('[name=" + parameterName + "]').val() ,\r\n";
            }

            return formTemplate.Replace("{{name}}", formName)
                    .Replace("{{fields}}", submitFields)
                    .Replace("{{formfields}}", rowTemplateAppender);
        }

        public string generateFORMboostrap(string procName, string formName)
        {
            DataTable dT = Connection.GetDATATABLE("select * from information_schema.parameters where specific_name='" + procName + "'");

            string parameterName;
            string formTemplate = General.ReadASCIIfromResources("DBManager.ResourceSQLServer.proc_form_bootstrap_template.txt");
            string rowTemplate = "										<div class='form-group'>\r\n											<label>txt{{field}} :</label>\r\n											<input name='{{field}}' class='form-control' placeholder='txt{{field}}'>\r\n										</div>\r\n";
            string rowTemplateAppender = "";
            string submitFields = "";
            string listwhere = "";
            string listwheres = "";
            foreach (DataRow item in dT.Rows)
            {
                parameterName = item["PARAMETER_NAME"].ToString().Substring(1).ToLower();
                rowTemplateAppender += rowTemplate.Replace("{{field}}", parameterName);
                submitFields += "                                    \"" + parameterName + "\": $('[name=" + parameterName + "]').val() ,\r\n";
                listwhere += "         $('[name=" + parameterName + "]').val(e[0][\"" + parameterName + "\"]);\r\n";
                listwheres += "            inj+=\"<td>\"+ e[it][\"=" + parameterName + "\"] +\"</td>\";\r\n";

            }

            return formTemplate.Replace("{{modalElements}}", rowTemplateAppender)
                    .Replace("{{fields}}", submitFields)
                    .Replace("{{listwhere}}", listwhere)
                    .Replace("{{listwheres}}", listwheres)
                    .Replace("{{table}}", formName);
        }


        private string getPROCvariablesValidation(DataTable dT, out string qMarks, out string param)
        {
            qMarks = "";
            param = "params.id,";
            string validation = "";
            foreach (DataRow item in dT.Rows)
            {
                qMarks += "?,";
                validation += "req.body." + item["PARAMETER_NAME"].ToString().Substring(1) + " == null || ";
                param += " req.body." + item["PARAMETER_NAME"].ToString().Substring(1) + ",";
            }

            if (validation.Length > 3)
            {
                validation = validation.Substring(0, validation.Length - 3);

                qMarks = qMarks.Substring(0, qMarks.Length - 1);

                param = param.Substring(0, param.Length - 1);
            }

            return validation;
        }



        public IDbConnection getConnection()
        {
            return Connection.GetConnection();
        }


        public List<string> getTableFields(string table)
        {
            List<string> tbls = new List<string>();

            DataTable dT_Fields;

            dT_Fields = Connection.GetDATATABLE("exec sp_Columns " + table);

            for (int i = 0; i < dT_Fields.Rows.Count; i++)
            {
                tbls.Add(dT_Fields.Rows[i]["COLUMN_NAME"].ToString());
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
            return Connection.ExecuteSQLScalar("DECLARE @table_name SYSNAME SELECT @table_name = 'dbo." + table  + "'  DECLARE        @object_name SYSNAME     , @object_id INT  SELECT        @object_name = '[' + s.name + '].[' + o.name + ']'     , @object_id = o.[object_id] FROM sys.objects o WITH (NOWAIT) JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id] WHERE s.name + '.' + o.name = @table_name     AND o.[type] = 'U'     AND o.is_ms_shipped = 0  DECLARE @SQL NVARCHAR(MAX) = ''  ;WITH index_column AS  (     SELECT            ic.[object_id]         , ic.index_id         , ic.is_descending_key         , ic.is_included_column         , c.name     FROM sys.index_columns ic WITH (NOWAIT)     JOIN sys.columns c WITH (NOWAIT) ON ic.[object_id] = c.[object_id] AND ic.column_id = c.column_id     WHERE ic.[object_id] = @object_id ), fk_columns AS  (      SELECT            k.constraint_object_id         , cname = c.name         , rcname = rc.name     FROM sys.foreign_key_columns k WITH (NOWAIT)     JOIN sys.columns rc WITH (NOWAIT) ON rc.[object_id] = k.referenced_object_id AND rc.column_id = k.referenced_column_id      JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = k.parent_object_id AND c.column_id = k.parent_column_id     WHERE k.parent_object_id = @object_id ) SELECT @SQL = 'CREATE TABLE ' + @object_name + CHAR(13) + '(' + CHAR(13) + STUFF((     SELECT CHAR(9) + ', [' + c.name + '] ' +          CASE WHEN c.is_computed = 1             THEN 'AS ' + cc.[definition]              ELSE UPPER(tp.name) +                  CASE WHEN tp.name IN ('varchar', 'char', 'varbinary', 'binary', 'text')                        THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length AS VARCHAR(5)) END + ')'                      WHEN tp.name IN ('nvarchar', 'nchar', 'ntext')                        THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length / 2 AS VARCHAR(5)) END + ')'                      WHEN tp.name IN ('datetime2', 'time2', 'datetimeoffset')                         THEN '(' + CAST(c.scale AS VARCHAR(5)) + ')'                      WHEN tp.name = 'decimal'                         THEN '(' + CAST(c.[precision] AS VARCHAR(5)) + ',' + CAST(c.scale AS VARCHAR(5)) + ')'                     ELSE ''                 END +                 CASE WHEN c.collation_name IS NOT NULL THEN ' COLLATE ' + c.collation_name ELSE '' END +                 CASE WHEN c.is_nullable = 1 THEN ' NULL' ELSE ' NOT NULL' END +                 CASE WHEN dc.[definition] IS NOT NULL THEN ' DEFAULT' + dc.[definition] ELSE '' END +                  CASE WHEN ic.is_identity = 1 THEN ' IDENTITY(' + CAST(ISNULL(ic.seed_value, '0') AS CHAR(1)) + ',' + CAST(ISNULL(ic.increment_value, '1') AS CHAR(1)) + ')' ELSE '' END          END + CHAR(13)     FROM sys.columns c WITH (NOWAIT)     JOIN sys.types tp WITH (NOWAIT) ON c.user_type_id = tp.user_type_id     LEFT JOIN sys.computed_columns cc WITH (NOWAIT) ON c.[object_id] = cc.[object_id] AND c.column_id = cc.column_id     LEFT JOIN sys.default_constraints dc WITH (NOWAIT) ON c.default_object_id != 0 AND c.[object_id] = dc.parent_object_id AND c.column_id = dc.parent_column_id     LEFT JOIN sys.identity_columns ic WITH (NOWAIT) ON c.is_identity = 1 AND c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id     WHERE c.[object_id] = @object_id     ORDER BY c.column_id     FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, CHAR(9) + ' ')     + ISNULL((SELECT CHAR(9) + ', CONSTRAINT [' + k.name + '] PRIMARY KEY (' +                      (SELECT STUFF((                          SELECT ', [' + c.name + '] ' + CASE WHEN ic.is_descending_key = 1 THEN 'DESC' ELSE 'ASC' END                          FROM sys.index_columns ic WITH (NOWAIT)                          JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id                          WHERE ic.is_included_column = 0                              AND ic.[object_id] = k.parent_object_id                               AND ic.index_id = k.unique_index_id                               FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, ''))             + ')' + CHAR(13)             FROM sys.key_constraints k WITH (NOWAIT)             WHERE k.parent_object_id = @object_id                  AND k.[type] = 'PK'), '') + ')'  + CHAR(13)     + ISNULL((SELECT (         SELECT CHAR(13) +              'ALTER TABLE ' + @object_name + ' WITH'              + CASE WHEN fk.is_not_trusted = 1                  THEN ' NOCHECK'                  ELSE ' CHECK'                END +                ' ADD CONSTRAINT [' + fk.name  + '] FOREIGN KEY('                + STUFF((                 SELECT ', [' + k.cname + ']'                 FROM fk_columns k                 WHERE k.constraint_object_id = fk.[object_id]                 FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')                + ')' +               ' REFERENCES [' + SCHEMA_NAME(ro.[schema_id]) + '].[' + ro.name + '] ('               + STUFF((                 SELECT ', [' + k.rcname + ']'                 FROM fk_columns k                 WHERE k.constraint_object_id = fk.[object_id]                 FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')                + ')'             + CASE                  WHEN fk.delete_referential_action = 1 THEN ' ON DELETE CASCADE'                  WHEN fk.delete_referential_action = 2 THEN ' ON DELETE SET NULL'                 WHEN fk.delete_referential_action = 3 THEN ' ON DELETE SET DEFAULT'                  ELSE ''                END             + CASE                  WHEN fk.update_referential_action = 1 THEN ' ON UPDATE CASCADE'                 WHEN fk.update_referential_action = 2 THEN ' ON UPDATE SET NULL'                 WHEN fk.update_referential_action = 3 THEN ' ON UPDATE SET DEFAULT'                   ELSE ''                END              + CHAR(13) + 'ALTER TABLE ' + @object_name + ' CHECK CONSTRAINT [' + fk.name  + ']' + CHAR(13)         FROM sys.foreign_keys fk WITH (NOWAIT)         JOIN sys.objects ro WITH (NOWAIT) ON ro.[object_id] = fk.referenced_object_id         WHERE fk.parent_object_id = @object_id         FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)')), '')     + ISNULL(((SELECT          CHAR(13) + 'CREATE' + CASE WHEN i.is_unique = 1 THEN ' UNIQUE' ELSE '' END                  + ' NONCLUSTERED INDEX [' + i.name + '] ON ' + @object_name + ' (' +                 STUFF((                 SELECT ', [' + c.name + ']' + CASE WHEN c.is_descending_key = 1 THEN ' DESC' ELSE ' ASC' END                 FROM index_column c                 WHERE c.is_included_column = 0                     AND c.index_id = i.index_id                 FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')'                   + ISNULL(CHAR(13) + 'INCLUDE (' +                      STUFF((                     SELECT ', [' + c.name + ']'                     FROM index_column c                     WHERE c.is_included_column = 1                         AND c.index_id = i.index_id                     FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')', '')  + CHAR(13)         FROM sys.indexes i WITH (NOWAIT)         WHERE i.[object_id] = @object_id             AND i.is_primary_key = 0             AND i.[type] = 2         FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')     ), '')  SELECT @SQL").ToString();
        }

        public void Disconnect()
        {
            if (Connection != null)
            Connection.Close();
        }


        public bool optionsShowRestoreScript()
        {
            return true;
        }

        public bool optionsProceduresFunctions()
        {
            return true;
        }
    }
}
