using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using Aga.Controls.Tree;
using System.Collections.Generic;
using System.IO;


namespace DBManager.DBASES
{
    internal class ADOnet : IdbType
    {


        internal ADOnetClass Connection = null;
        internal string ConnectionString;
        internal TreeModel TRmodel;
        internal ImageList imageList;
        internal bool isXLS = false;

        public ADOnet(int index, ImageList imageList)
        {
            if (!File.Exists(General.Connections[index].filename))
                throw new ArgumentException("Could not find file '" + General.Connections[index].filename + "'");
   
            if (General.Connections[index].filename.ToLower().EndsWith("xls"))
            {
                isXLS = true;
                ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + General.Connections[index].filename + ";Extended Properties=Excel 8.0;";
            }
            else if (General.Connections[index].filename.ToLower().EndsWith("accdb"))
            {
                ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + General.Connections[index].filename + ";Jet OLEDB:Database Password=" + General.Connections[index].password;
            }
            else if (General.Connections[index].filename.ToLower().EndsWith("xlsx"))
            {
                if (!File.Exists(General.Connections[index].filename))
                    throw new ArgumentException("File doesnt exists\r\n\r\n" + General.Connections[index].filename, "notfound");

                isXLS = true;
                ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + General.Connections[index].filename + ";Extended Properties=Excel 12.0;";
            }
            else 
                ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + General.Connections[index].filename + ";Jet OLEDB:Database Password=" + General.Connections[index].password;

            this.imageList = imageList;
        }


        public string Connect()
        {
            OleDbException err = null;
            Connection = new ADOnetClass(ConnectionString, out err);

            if (err != null)
            {
                Connection = null;

                if (isXLS)
                    return "Keep in mind you should provide pure XLS/XLSX, no in XML format\r\n\r\n" + err.Message;
                else 
                    return err.Message;
            }
            else
                return "";
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

            //ASK FOR COLUMN INFO
            OleDbDataReader cols = null;
            DataTable dT_Fields;

            Node table = null;
            int iconIndex = 0;
            string buffer4fields = "";

            try
            {
                //GET ALL TABLES
                t = Connection.GetConnection().GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "TABLE" });

                //FOR EACH TABLE
                foreach (DataRow item in t.Rows)
                {
                    //CREATE NEW NODE
                    table = new Node();
                    table = (Node)new treeItem(item["TABLE_NAME"].ToString(), "", "", false, false, "", 0, imageList);

                    //GET PRIMARY
                    keys = Connection.GetConnection().GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, new Object[] { null, null, item["TABLE_NAME"].ToString() });
                    //ADD ALSO FOREIGN?

                    //ASK FOR COLUMN INFO
                    cols = Connection.GetDATAREADER("select top 1 * from [" + item["TABLE_NAME"].ToString() + "]");
                    //GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new Object[] {null, null, item["TABLE_NAME"].ToString()});

                    //get TABLE FIELDS schema
                    dT_Fields = cols.GetSchemaTable();

                    //for each TABLE FIELD
                    foreach (DataRow colItem in dT_Fields.Rows)
                    {
                        dR_Keys = keys.Select("COLUMN_NAME='" + colItem["columnname"].ToString() + "'");
                        if (dR_Keys != null && dR_Keys.Length > 0)
                            iconIndex = 2; //primary
                        else
                            iconIndex = 1;//default field

                        buffer4fields = TrimAccessFieldType(colItem["datatype"].ToString());
                        //hung to parent Node (TABLE) the FIELD name
                        //table.Nodes.Add("", schemaFields["columnname"].ToString() + " -> " + buffer4fields.ToLower() + " (" + schemaFields["columnsize"].ToString() + ")", 1, 1);
                        table.Nodes.Add(new treeItem(colItem["columnname"].ToString(), buffer4fields.ToLower(), colItem["columnsize"].ToString(), colItem["allowdbnull"].ToString() == "0" ? false : true, false, buffer4fields, iconIndex, imageList));
                        //table.Nodes.Add(new treeItem(colItem["columnname"].ToString(), buffer4fields.ToLower(), colItem["columnsize"].ToString(),  false, false, buffer4fields, iconIndex, imageList));
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
            }


            if (TRmodel == null)
                TRmodel = new TreeModel();

            return TRmodel;

        }

        private string TrimAccessFieldType(string input)
        {
            return input.Replace("System.", "").Replace("64", "").Replace("32", "").Replace("16", "").Replace("[]", "");
        }

        public DataTable ExecuteSQL(string SQL, out string rowsAffected, out string error)
        {
      
            DataSet dataSet = new DataSet();

            error = "";
            rowsAffected = "0";
    
                OleDbDataAdapter objDataAdapter=null;
                try
                {
                    if (SQL.Trim().ToLower().StartsWith("select"))
                    {
                        objDataAdapter = new OleDbDataAdapter(SQL, Connection.GetConnection());

                        objDataAdapter.Fill(dataSet, "tabl");
                        rowsAffected = dataSet.Tables["tabl"].Rows.Count.ToString();
                        return dataSet.Tables["tabl"];
                    }
                    else
                    {
                        OleDbCommand dC = Connection.GetCommand(SQL);
                        rowsAffected = dC.ExecuteNonQuery().ToString();
                        return null;

                    }
                }
                catch (Exception ex)
                {
                    if (dataSet != null)
                        dataSet.Dispose();

                    if (objDataAdapter != null)
                        objDataAdapter.Dispose();

                    error = ex.Message;

                    return null;
                }

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
            return "SELECT COUNT(*) FROM [" + table + "]";
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
                OleDbDataAdapter da;
                DataTable dt;
                int x = 0;

                // Setup SQL (This is the original table where the data was retrieved when grid was filled, it will also be the table updated)
                sql = DG.Tag.ToString();
                //"select * from " & MainForm.tr.SelectedNode.Text.Substring(0, MainForm.tr.SelectedNode.Text.LastIndexOf("[") - 1)

                // Setup DataAdapter
                da = new OleDbDataAdapter(sql, Connection.GetConnection());

                // Create a command builder (this is needed for the update to work)
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);

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

        public List<string> getTables()
        {
            List<string> tbls = new List<string>();

            DataTable dT;

            dT = Connection.GetConnection().GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "TABLE" });
            dT.DefaultView.Sort = "TABLE_NAME";

            foreach (DataRowView dR in dT.DefaultView)
            {
                tbls.Add(dR["TABLE_NAME"].ToString());
            }

            dT.Dispose();

            return tbls;

        }

        public List<string> getTableFields(string table)
        {
            List<string> tbls = new List<string>();

            DataTable dT_Fields;

            dT_Fields = Connection.GetConnection().GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new Object[] { null, null, table });
            //dT_Fields.DefaultView.Sort = "ORDINAL_POSITION";
            DataView dv = new DataView(dT_Fields);
            dv.Sort = "ORDINAL_POSITION";


            for (int i = 0; i < dv.Count; i++)
            {
                tbls.Add(dv[i]["COLUMN_NAME"].ToString());
            }

            dT_Fields.Dispose();

            return tbls;


        }

        public DataTable getDatatable(string q)
        {
            return Connection.GetDATATABLE(q);
        }

        public IDbConnection getConnection()
        {
            return Connection.GetConnection();
        }

        public void Disconnect()
        {
            if (Connection != null)
            {
                Connection.ConnectionClose();
                //Connection.Dispose();
            }
        }

        public string generateFORM(string procName, string formName)
        {
            throw new NotImplementedException();
        }

        public string generateFORMboostrap(string procName, string formName)
        {
            throw new NotImplementedException();
        }

        public bool optionsShowRestoreScript()
        {
            return false;
        }

        public bool optionsProceduresFunctions()
        {
            return false;
        }

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

        public string generatePROCdelete(string tablename, string PK)
        {
            throw new NotImplementedException();
        }

        public string generatePROCnodeJS(string procName)
        {
            throw new NotImplementedException();
        }

        public string generateTableScript(string table)
        {
            OleDbDataReader cols = Connection.GetDATAREADER("select top 1 * from [" + table + "]");

            DataTable dt = cols.GetSchemaTable();
            List<string> list = new List<string>();

            foreach (DataRow colItem in dt.Rows)
            {
                list.Add("\t" + colItem["columnname"].ToString() + " " + TrimAccessFieldType(colItem["datatype"].ToString()));
            }
            return "CREATE TABLE [" + table + "] (\r\n" + string.Join(",\r\n", list.ToArray()) + "\r\n)";
        }

        public string GenerateParameterInsert(TreeNodeAdv table)
        {
            throw new NotImplementedException();
        }

        public string GenerateParameterUpdate(TreeNodeAdv table)
        {
            throw new NotImplementedException();
        }


        public string parseProcedure(string procName, bool replaceCreate)
        {
            return "";
        }

        public ListViewItem[] GetProcedures()
        {
            return null;
        }



        public string generatePROCMerge(string tablename, List<ListStrings> fields)
        {
            throw new NotImplementedException();
        }
    }
}