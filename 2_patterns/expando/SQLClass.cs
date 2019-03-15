using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace WindowsFormsApp1
{
    public class SQLClass
    {

        private SqlConnection objConn;
        private string m_ConnectionString;

        public SQLClass(string ConnectionString, out SqlException ExceptionObject)
        {
            try
            {
                m_ConnectionString = ConnectionString;
                objConn = new SqlConnection(ConnectionString);
                objConn.Open();

                ExceptionObject = null;
            }
            catch (SqlException ex)
            {
                objConn = null;
                ExceptionObject = ex;
            }
        }

        public bool IsConnected
        {
            get
            {
                if (objConn == null | objConn.State != ConnectionState.Open)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public string ConnectionString
        {
            get { return m_ConnectionString; }
        }

        public SqlDataAdapter GetAdapter(string sql)
        {
            return new SqlDataAdapter(sql, objConn);
        }

        public SqlCommand GetCommand(string Query)
        {
            return new SqlCommand(Query, objConn);
        }

        public SqlConnection GetConnection()
        {
            return objConn;
        }

        public IEnumerable<ExpandoObject> ExecuteReadQuery(string query)
        {
            using (var command = new SqlCommand(query, objConn))
            {
                using (var rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var row = new ExpandoObject() as IDictionary<string, object>;
                        for (var fieldCount = 0; fieldCount < rdr.FieldCount; fieldCount++)
                        {
                            row.Add(rdr.GetName(fieldCount), rdr[fieldCount]);
                        }
                        yield return (ExpandoObject)row;
                    }
                }
            }
        }


        public int ExecuteUpdateQuery(string query)
        {
            int rows = -1;
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConn))
                {
                    rows = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rows;
        }


        public int ExecuteUpdateQuery(string query, Dictionary<string, string> parameters)
        { //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlparametercollection.addwithvalue
            int rows = -1;
            try
            {
                    using (SqlCommand cmd = new SqlCommand(query, objConn))
                    {
                        foreach (KeyValuePair<string, string> field in parameters)
                        {
                            cmd.Parameters.AddWithValue(field.Key, field.Value);
                        }

                        rows = cmd.ExecuteNonQuery();
                    }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rows;
        }
        
        
       public Int64 ExecuteInsertQuery(string query, Dictionary<string, string> parameters, SqlConnection conn)
        {  //Int64 because SQL return the newly created record id. (https://stackoverflow.com/a/10999467/) ex.
           //INSERT INTO table(fieldA,fieldB) output INSERTED.tableID VALUES(@fieldA,@fieldB)
            Int64 result = -1;
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    foreach (KeyValuePair<string, string> field in parameters)
                    {
                        cmd.Parameters.AddWithValue(field.Key, field.Value);
                    }

                    object RecordID = cmd.ExecuteScalar();

                    if (RecordID != null)
                        result = Int64.Parse(RecordID.ToString());
                    else
                        result = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public object ExecuteScalarQuery(string query)
        {
            object result = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConn))
                {
                    result = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public DataTable GetDataTable(string query)
        {
            SqlDataAdapter sqlAD = new SqlDataAdapter();
            DataTable sqlSET = new DataTable();
            SqlCommand sqlco = new SqlCommand();

            try
            {
                sqlco.CommandText = query;
                sqlco.Connection = objConn;

                sqlAD.SelectCommand = sqlco;
                //sqlAD.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                sqlAD.Fill(sqlSET);

                return sqlSET;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }
        }

        public DataSet GetDataSet(string query)
        {
            SqlDataAdapter sqlAD = new SqlDataAdapter();
            DataSet sqlSET = new DataSet();
            SqlCommand sqlco = new SqlCommand();

            try
            {
                sqlco.CommandText = query;
                sqlco.Connection = objConn;

                sqlAD.SelectCommand = sqlco;
                sqlAD.Fill(sqlSET, "tabl");
                return sqlSET;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }
        }
    }
}
