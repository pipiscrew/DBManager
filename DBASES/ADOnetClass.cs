using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace DBManager.DBASES
{
    internal class ADOnetClass : IDisposable
    {
        private OleDbConnection objConn;
        private OleDbCommand cmd;
        private string m_ConnectionString;
        private bool disposed = false;

        public ADOnetClass(string ConnectionString, out OleDbException ExceptionObject)
        {
            try
            {
                m_ConnectionString = ConnectionString;

                objConn = new OleDbConnection(ConnectionString);
                objConn.Open();
                ExceptionObject = null;
            }
            catch (OleDbException ex)
            {
                objConn = null;
                ExceptionObject = ex;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if ((cmd != null))
                    {
                        cmd.Dispose();
                    }

                    if ((objConn != null))
                    {
                        objConn.Close();
                        objConn.Dispose();
                    }
                }
                disposed = true;
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


        public OleDbDataAdapter GetAdapter(string sql)
        {
            return new OleDbDataAdapter(sql, objConn);
        }

        public void ConnectionClose()
        {
            if ((cmd != null))
            {
                cmd.Dispose();
            }

     
            if ((objConn != null))
            {
                objConn.Close();
                objConn.Dispose();
            }
        }

        public OleDbCommand GetCommand(string Query)
        {
            return new OleDbCommand(Query, objConn);
        }


        public DataSet GetDATASET(string Query)
        {
            try
            {
                DataSet objDataSet = new DataSet();
                DataTable objDataTable = new DataTable();

                OleDbDataAdapter objDataAdapter = new OleDbDataAdapter(Query, objConn);

                objDataAdapter.Fill(objDataSet, "tabl");

                return objDataSet;
            }
            catch (Exception ex)
            {
                //Return Nothing
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }


        public DataSet GetDATASET2(string SQLSTR, out string error)
        {
            OleDbDataAdapter sqlAD = new OleDbDataAdapter();
            DataSet sqlSET = new DataSet();
            OleDbCommand sqlco = new OleDbCommand();

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

        public OleDbDataReader GetDATAREADER(string Query)
        {
             OleDbDataReader sqlread = null;
             OleDbCommand sqlco = new OleDbCommand();
            try
            {
                sqlco.Connection = objConn;
                sqlco.CommandText = Query;

                sqlread = sqlco.ExecuteReader();

                return sqlread;

            }
            catch (Exception ex)
            {
                //Return Nothing
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                sqlco.Dispose();
            }
        }


        public OleDbDataAdapter GetDataAdapter(String SQLSTR)
        {
            OleDbDataAdapter sqlAD = new OleDbDataAdapter();
            DataTable sqlSET = new DataTable();
            OleDbCommand sqlco = new OleDbCommand();

            try
            {
                sqlco.CommandText = SQLSTR;
                sqlco.Connection = objConn;

                sqlAD.SelectCommand = sqlco;
                sqlAD.Fill(sqlSET);

                return sqlAD;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ADOnetClass - GetDataAdapter");
                return null;
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }
        }


        public DataTable GetDATATABLE(string Query)
        {

            OleDbDataAdapter sqlAD = new OleDbDataAdapter();
            DataTable sqlSET = new DataTable();
            OleDbCommand sqlco = new OleDbCommand();

            try
            {
                sqlco.CommandText = Query;
                sqlco.Connection = objConn;

                sqlAD.SelectCommand = sqlco;
                sqlAD.Fill(sqlSET);

                return sqlSET;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLiteClass - GetDATATABLE");
                return null;
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }

            
        }

  

        public OleDbConnection GetConnection()
        {
            return objConn;
        }


        public object ExecuteSQLScalar(string Query)
        {
            OleDbCommand sqlco = new OleDbCommand();
            try
            {
                sqlco.Connection = objConn;
                sqlco.CommandText = Query;
                return sqlco.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ADOnetClass - ExecuteSQLScalar");
                return "";
            }
            finally
            {
                sqlco.Dispose();
            }

        }


        public OleDbDataReader ExecuteParameterDataReader(string sqlStatement)
        {
            try
            {
                cmd.CommandText = sqlStatement;
                cmd.CommandType = CommandType.Text;

                cmd.Prepare();
                return cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ADOClass - ExecuteParameterDataReader", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }
        }


        public int ExecuteNonQuery(string CommandString)
        {
            int functionReturnValue = 0;
            OleDbCommand sqlco = new OleDbCommand();
            try
            {
                sqlco.Connection = objConn;
                sqlco.CommandText = CommandString;
                return sqlco.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ADOnetClass - ExecuteNonQuery");
                functionReturnValue = 3;
            }
            finally
            {
                sqlco.Dispose();
            }
            return functionReturnValue;
        }

        public int ExecuteNonQuery(string CommandString, out OleDbException ErrReport)
        {
            int functionReturnValue = 0;
            OleDbCommand sqlco = new OleDbCommand();
            ErrReport = null;

            try
            {

                sqlco.Connection = objConn;
                sqlco.CommandText = CommandString;

                return sqlco.ExecuteNonQuery();
            }
            catch (OleDbException sEX)
            {
                ErrReport = sEX;
                functionReturnValue = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ADOnetClass - ExecuteNonQuery");
                functionReturnValue = 0;
            }
            finally
            {
                sqlco.Dispose();
            }
            return functionReturnValue;
        }


    }
}
