using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;

namespace DBManager.DBASES
{
    internal class SQLClass
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

        public DataSet GetDATASET(string SQLSTR)
        {
            SqlDataAdapter sqlAD = new SqlDataAdapter();
            DataSet sqlSET = new DataSet();
            SqlCommand sqlco = new SqlCommand();

            try
            {
                sqlco.CommandText = SQLSTR;
                sqlco.Connection = objConn;

                sqlAD.SelectCommand = sqlco;
                sqlAD.Fill(sqlSET, "tabl");
                return sqlSET;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLClass - GetDATASET");
                return null;
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }
        }


        public DataSet GetDATASET2(string SQLSTR,out string error)
        {
            SqlDataAdapter sqlAD = new SqlDataAdapter();
            DataSet sqlSET = new DataSet();
            SqlCommand sqlco = new SqlCommand();

            try
            {
                sqlco.CommandText = SQLSTR;
                sqlco.Connection = objConn;

                sqlAD.SelectCommand = sqlco;
                sqlAD.Fill(sqlSET, "tabl");
                error = "";
                return sqlSET;

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }
        }

        public SqlDataReader GetDATAREADER(string SQLSTR)
        {
            SqlDataReader sqlread = null;
            SqlCommand sqlco = new SqlCommand();
            try
            {
                sqlco.Connection = objConn;

                sqlco.CommandText = SQLSTR;

                sqlread = sqlco.ExecuteReader();
                return sqlread;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLClass - GetDATAREADER");
                return null;
            }
            finally
            {
                sqlco.Dispose();
                //sqlread.Close()
            }
        }

        public DataTable GetDATATABLE(string SQLSTR)
        {
            SqlDataAdapter sqlAD = new SqlDataAdapter();
            DataTable sqlSET = new DataTable();
            SqlCommand sqlco = new SqlCommand();

            try
            {
                sqlco.CommandText = SQLSTR;
                sqlco.Connection = objConn;

                sqlAD.SelectCommand = sqlco;
                //sqlAD.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                sqlAD.Fill(sqlSET);

                return sqlSET;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLClass - GetDATASET");
                return null;
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }
        }

        public SqlConnection GetConnection()
        {
            return objConn;
        }


        public object ExecuteSQLScalar(string SQLSTR)
        {
            SqlCommand sqlco = new SqlCommand();
            try
            {
                sqlco.Connection = objConn;
                sqlco.CommandText = SQLSTR;
                return sqlco.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLClass - ExecuteSQLScalar");
                return "";
            }
            finally
            {
                sqlco.Dispose();
            }
        }

        public int ExecuteNonQuery(string SQLSTR)
        {
            int functionReturnValue = 0;
            SqlCommand sqlco = new SqlCommand();
            try
            {
                sqlco.Connection = objConn;
                sqlco.CommandText = SQLSTR;
                return sqlco.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLClass - ExecuteSQL");
                functionReturnValue = 0;
            }
            finally
            {
                sqlco.Dispose();
            }
            return functionReturnValue;
        }

        public int ExecuteNonQuery(string SQLSTR, out SqlException ErrReport)
        {
            int functionReturnValue = 0;
            SqlCommand sqlco = new SqlCommand();
            ErrReport = null;

            try
            {

                sqlco.Connection = objConn;
                sqlco.CommandText = SQLSTR;

                return sqlco.ExecuteNonQuery();
            }
            catch (SqlException sEX)
            {
                ErrReport = sEX;
                functionReturnValue = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLClass - ExecuteSQL");
                functionReturnValue = 0;
            }
            finally
            {
                sqlco.Dispose();
            }
            return functionReturnValue;
        }

        public void Close()
        {
            if ((objConn != null))
            {
                objConn.Close();
                objConn.Dispose();
            }
        }

 
        //public List<string> getTables()
        //{
        //    List<string> tbls = new List<string>();

        //    DataTable dT;

        //    dT = objConn.GetSchema("Tables");
        //    dT.DefaultView.Sort = "TABLE_NAME";

        //    foreach (DataRowView dR in dT.DefaultView)
        //    {

        //      //  if (dR["TABLE_TYPE"].ToString().ToLower() == "table")
        //            tbls.Add(dR["TABLE_NAME"].ToString());
        //    }

        //    dT.Dispose();

        //    return tbls;
        //}

    }

}
