using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EFClassGenerator
{
    public class MsSql
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool useWindowsAuth { get; set; }
        public string Catalog { get; set; }
        private string ConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = Server;
                if (!String.IsNullOrEmpty(Catalog)) builder.InitialCatalog = Catalog;
                builder.IntegratedSecurity = false; ;
                builder.ConnectTimeout = 5;
                if (useWindowsAuth)
                {
                    builder.IntegratedSecurity = true;
                }
                else
                {
                    builder.UserID = Username;
                    builder.Password = Password;
                }
                return builder.ConnectionString;
            }
        }

        public MsSql(string server, string username, string password)
        {
            this.Server = server;
            this.useWindowsAuth = false;
            this.Username = username;
            this.Password = password;
            CheckConnection();
        }
        public MsSql(string server)
        {
            this.Server = server;
            this.Username = "";
            this.Password = "";
            this.useWindowsAuth = true;
            CheckConnection();
        }


        public bool CheckConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    conn.Open();
                    conn.Close();
                }
                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable GetDataTable(string QueryString)
        {
            DataTable functionReturnValue = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(QueryString, conn);
                    DataSet ds = new DataSet("Data");
                    da.Fill(ds);
                    if (ds.Tables.Count == 1)
                    {
                        return ds.Tables[0];
                    }
                    else
                    {
                        throw new Exception("More than 1 table returned by query");
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }



        }
        public bool ExecuteQueryCMD(SqlCommand cmd)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    long RecordsEffected = cmd.ExecuteNonQuery();
                    return (RecordsEffected > 0);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable GetDataTableCMD(SqlCommand cmd)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet("Data");
                    da.Fill(ds);
                    if (ds.Tables.Count == 1)
                    {
                        return ds.Tables[0];
                    }
                    else
                    {
                        throw new Exception("More than 1 table returned by query");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public object GetSingleValue(string QueryString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(QueryString, conn);
                    DataSet ds = new DataSet("Data");
                    da.Fill(ds);
                    if (ds.Tables.Count == 1 & ds.Tables[0].Rows.Count == 1)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        return dr[0];
                    }
                    else
                    {
                        //Throw New Exception("More than 1 row returned by query")
                        return null;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }


        }


        public DataTable GetCatalogList()
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT");
            sb.AppendLine("    d.name AS [name]");
            sb.AppendLine("FROM");
            sb.AppendLine("    sys.databases AS d ");
            sb.AppendLine("    INNER JOIN sys.master_files AS m ON d.database_id = m.database_id");
            sb.AppendLine("WHERE");
            sb.AppendLine("    d.state_desc = 'ONLINE'");
            sb.AppendLine("    AND m.type_desc = 'ROWS'");
            sb.AppendLine("AND m.name not in ('master','tempdev','modeldev','MSDBData')");
            sb.AppendLine("ORDER BY");
            sb.AppendLine("    m.name;");

            return GetDataTable(sb.ToString());
        }

        public DataTable GetTableList()
        {
            return GetDataTable("select TABLE_NAME from Information_schema.TABLES WHERE TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME");
        }
        public DataTable GetViewList()
        {
            return GetDataTable("select TABLE_NAME from Information_schema.TABLES WHERE TABLE_TYPE = 'VIEW' order by TABLE_NAME;");
        }
        public DataTable GetColumnList(string tablename)
        {
            return GetDataTable("SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tablename + "';");
        }
        public object GetColumnDataType(string tablename, string columnName)
        {
            string dataType= GetSingleValue("SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tablename + "' AND COLUMN_NAME = '" + columnName + "';").ToString();

            if (dataType== "uniqueidentifier")
            {
                return "Guid";
            }
            else
            {
                return dataType;
            }
        }

    }
}
