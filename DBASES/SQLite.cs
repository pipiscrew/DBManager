using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Aga.Controls.Tree;
using System.Windows.Forms;
using Finisar.SQLite;
using System.IO;


namespace DBManager.DBASES
{
    class SQLite : IdbType
    {

        internal SQLiteClass Connection = null;
        internal string ConnectionString;
        internal TreeModel TRmodel;
        internal ImageList imageList;

        public SQLite(int index, ImageList imageList)
        {
            if (!File.Exists(General.Connections[index].filename))
                throw new ArgumentException("Could not find file '" + General.Connections[index].filename + "'");

            ConnectionString = "Data Source=" + General.Connections[index].filename + ";Version=3;";  
            //"Data Source=" + General.Connections[index].serverName +
            //        ";Initial Catalog=" + General.Connections[index].dbaseName +
            //        ";User ID=" + General.Connections[index].user +
            //         ";Password=" + General.Connections[index].password;

            this.imageList = imageList;
        }

        public string Connect()
        {
            SQLiteException err = null;
            Connection = new SQLiteClass(ConnectionString, out err);

            if (err != null)
            {
                Connection = null;
                return err.Message;
            }
            else
                return "";
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

            Node table = null;
            
            DataTable dT=null;
            DataTable dT_Fields = null;
            DataTable dT_FieldsFK = null;
            string buffer4fields = "";


            try
            {

                dT = Connection.GetDATATABLE("SELECT tbl_name FROM sqlite_master where type = 'table' ORDER BY name;");

                if (dT.Rows.Count == 0)
                {
                    MessageBox.Show("No tables present, you can still create tables via script from here!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return TRmodel;
                }

                int pos;
                foreach (DataRow dR in dT.Rows)
                {
                    if (dR["tbl_name"].ToString().ToLower() == "sqlite_sequence")
                        continue;

                    //CREATE NEW NODE
                    table = new Node();
                    table = (Node)new treeItem(dR["tbl_name"].ToString(), "", "", false, false, "", 0, imageList);


                    //ask for TABLE FIELDS
                    dT_Fields = Connection.GetDATATABLE("PRAGMA table_info('" + dR["tbl_name"].ToString() + "')");
                    dT_FieldsFK = Connection.GetDATATABLE("PRAGMA foreign_key_list('" + dR["tbl_name"].ToString() + "')");

                    foreach (DataRow dR2 in dT_Fields.Rows)
                    {
                        buffer4fields = TrimAccessFieldType(dR2["type"].ToString());
                        pos = buffer4fields.IndexOf("(");
                        if (pos > -1)
                            buffer4fields = buffer4fields.Substring(0, pos);

                        buffer4fields = buffer4fields.Replace("(", "").Replace(")", "").Replace(" ", "").Trim();

                        //hung to parent Node (TABLE) the FIELD name
                        if (dR2["pk"].ToString() == "1")
                            table.Nodes.Add(new treeItem(dR2["name"].ToString(), buffer4fields.ToLower(), "9999", false, false, ConvertSQL2fieldType(buffer4fields), 2, imageList));
                        //baseNode.Nodes.Add("", dR2["name"].ToString() + " -> " + buffer4fields.ToLower(), 2, 2); // + " (" + schemaFields["columnsize"].ToString() + ")", 1, 1);
                        else
                        {
                            if (dT_FieldsFK.Rows.Count > 0)
                            {
                                DataRow[] dtFK = dT_FieldsFK.Select("[from] = '" + dR2["name"].ToString() + "'");

                                if (dtFK.Length > 0)
                                    table.Nodes.Add(new treeItem(dR2["name"].ToString(), buffer4fields.ToLower(), "9999", false, false, ConvertSQL2fieldType(buffer4fields), 3, imageList));
                                //  baseNode.Nodes.Add("", dR2["name"].ToString() + " -> " + buffer4fields.ToLower(), 3, 3); // + " (" + schemaFields["columnsize"].ToString() + ")", 1, 1);
                                else
                                    table.Nodes.Add(new treeItem(dR2["name"].ToString(), buffer4fields.ToLower(), "9999", false, false, ConvertSQL2fieldType(buffer4fields), 1, imageList));
                                //baseNode.Nodes.Add("", dR2["name"].ToString() + " -> " + buffer4fields.ToLower(), 1, 1); // + " (" + schemaFields["columnsize"].ToString() + ")", 1, 1);
                            }
                            else
                                table.Nodes.Add(new treeItem(dR2["name"].ToString(), buffer4fields.ToLower(), "9999", false, false, ConvertSQL2fieldType(buffer4fields), 1, imageList));
                            //baseNode.Nodes.Add("", dR2["name"].ToString() + " -> " + buffer4fields.ToLower(), 1, 1); // + " (" + schemaFields["columnsize"].ToString() + ")", 1, 1);
                        }
                    }

                    //add it to form treeview
                    //tr.Nodes.Add(baseNode);
                    //ADD TABLE INFO TO TREE
                    TRmodel.Nodes.Add(table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {

                if (dT != null)
                    dT.Dispose();

                if (dT_Fields != null)
                    dT_Fields.Dispose();

                if (dT_FieldsFK != null)
                    dT_FieldsFK.Dispose();

            }

            if (TRmodel == null)
                TRmodel = new TreeModel();

            return TRmodel;
        }

        private string ConvertSQL2fieldType(string SQLfieldType)
        {
            SQLfieldType = SQLfieldType.ToUpper();
            //SQLfieldType = SQLfieldType.Replace(" identity", "");



            switch (SQLfieldType)
            {
                case "TEXT":
                    return "string";
                case "INTEGER":
                    return "int";
                case "REAL":
                    return "decimal";
                case "BLOB":
                    return "object";
                default:
                    return "string";
            }
        }

        private string TrimAccessFieldType(string input)
        {
            return input.Replace("System.", "").Replace("64", "").Replace("32", "").Replace("16", "").Replace("[]", "");
        }

        public DataTable ExecuteSQL(string SQL, out string rowsAffected, out string error)
        {
            DataSet dataSet = new DataSet();
            SQLiteDataAdapter objDataAdapter;
            DataTable table = new DataTable();
            SQLiteTransaction trans = null;

            rowsAffected = "";
            error = "";

            try
            {
  
                if (!SQL.ToLower().StartsWith("pragma") && !SQL.ToLower().StartsWith("vacuum"))
                    trans = Connection.GetConnection().BeginTransaction();

                objDataAdapter = new SQLiteDataAdapter(SQL, Connection.GetConnection());
                //objDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                objDataAdapter.Fill(dataSet);

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
                error=(ex.Message + " ");
            }
            finally
            {
                if (dataSet != null)
                    dataSet.Dispose();


                if (!SQL.ToLower().StartsWith("pragma") && !SQL.ToLower().StartsWith("vacuum"))
                {
                    trans.Commit();
                    trans.Dispose();
                }
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
            return "SELECT * FROM [" + table + "] LIMIT 100";
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
                SQLiteDataAdapter da;
                DataTable dt;
                int x = 0;

                // Setup SQL (This is the original table where the data was retrieved when grid was filled, it will also be the table updated)
                sql = DG.Tag.ToString();
                //"select * from " & MainForm.tr.SelectedNode.Text.Substring(0, MainForm.tr.SelectedNode.Text.LastIndexOf("[") - 1)

                // Setup DataAdapter
                da = new SQLiteDataAdapter(sql, Connection.GetConnection());

                // Create a command builder (this is needed for the update to work)
                SQLiteCommandBuilder cb = new SQLiteCommandBuilder(da);

                // Get Current Data from datagrid
                dt = (DataTable)DG.DataSource;

                // Update Table through DataAdapter
                x = da.Update(dt);

                return(x.ToString() + " record(s) updated.");


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
            throw new NotImplementedException();
        }


        public string generatePROCupdate(string tablename, List<ListStrings> fields, string PK)
        {
            throw new NotImplementedException();
        }


        public string generatePROCdelete(string tblName, string PK)
        {
            throw new NotImplementedException();
        }


        public string generatePROCnodeJS(string procName)
        {
            throw new NotImplementedException();
        }


        public List<string> getTables()
        {
            List<string> tbls = new List<string>();

            DataTable dT;

            dT = Connection.GetDATATABLE("SELECT tbl_name FROM sqlite_master where type = 'table' ORDER BY name;");
            //dT.DefaultView.Sort = "TABLE_NAME";

            foreach (DataRowView dR in dT.DefaultView)
            {
                if (dR["tbl_name"].ToString().ToLower() == "sqlite_sequence")
                    continue;

                //  if (dR["TABLE_TYPE"].ToString().ToLower() == "table")
                tbls.Add(dR["tbl_name"].ToString());
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
            throw new NotImplementedException();
        }


        public string GenerateLast100(string table, string ID)
        {
            return "SELECT * FROM " + table + " ORDER BY " + ID + " DESC LIMIT 100";
        }


        public List<string> getTableFields(string table)
        {
            List<string> tbls = new List<string>();

            DataTable dT_Fields;

            dT_Fields = Connection.GetDATATABLE("PRAGMA table_info('" + table + "')");

            for (int i = 0; i < dT_Fields.Rows.Count; i++)
            {
                tbls.Add(dT_Fields.Rows[i]["name"].ToString());
            }

            dT_Fields.Dispose();

            return tbls;
        }

        public DataTable getDatatable(string q)
        {
            return Connection.GetDATATABLE(q);
        }

        public void Disconnect()
        {
            if (Connection != null)
            Connection.ConnectionClose();
        }


        public string generateTableScript(string table)
        {
            DataTable cols = Connection.GetDATATABLE("PRAGMA table_info('" + table + "')");

            List<string> list = new List<string>();

            foreach (DataRow dR2 in cols.Rows)
            {
                list.Add("\t" + dR2["name"].ToString() + " " + TrimAccessFieldType(dR2["type"].ToString()));
            }

            return "CREATE TABLE [" + table + "] (\r\n" + string.Join(",\r\n", list.ToArray()) + "\r\n)";
        }
        
        public bool optionsShowRestoreScript()
        {
            return false;
        }

        public bool optionsProceduresFunctions()
        {
            return false;
        }
    }
}
