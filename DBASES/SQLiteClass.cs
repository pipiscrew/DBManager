using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Finisar.SQLite;

namespace DBManager.DBASES
{
    internal class SQLiteClass : IDisposable
    {
        private SQLiteConnection objConn;
        private SQLiteCommand cmd=null;
        private string m_ConnectionString;
        private bool disposed = false;

        #region "Constructors"
        public SQLiteClass(string ConnectionString, out SQLiteException ExceptionObject)
        {
            try
            {
                m_ConnectionString = ConnectionString;
                objConn = new SQLiteConnection(ConnectionString);
                objConn.Open();

                ExceptionObject = null;
            }
            catch (SQLiteException ex)
            {
                objConn = null;
                ExceptionObject = ex;
            }
        }
        #endregion

        #region "Methods"

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

        public SQLiteDataAdapter GetAdapter(string sql)
        {
            return new SQLiteDataAdapter(sql, objConn);
        }

        public void ConnectionClose()
        {
            if ((cmd != null))
            {
                cmd.Dispose();
            }

            //if ((cnn != null))
            //{
            //    objConn.Close();
            //    objConn.Dispose();
            //}

            if ((objConn != null))
            {
                objConn.Close();
                objConn.Dispose();
            }
        }

        public SQLiteCommand GetCommand(string Query)
        {
            return new SQLiteCommand(Query, objConn);
        }

        public DataSet GetDATASET(string SQLSTR)
        {
            SQLiteDataAdapter sqlAD = new SQLiteDataAdapter();
            DataSet sqlSET = new DataSet();
            SQLiteCommand sqlco = new SQLiteCommand();

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
                MessageBox.Show(ex.Message, "SQLiteClass - GetDATASET");
                return null;
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }
        }

        public SQLiteDataReader GetDATAREADER(string SQLSTR)
        {
            SQLiteDataReader sqlread = null;
            SQLiteCommand sqlco = new SQLiteCommand();
            try
            {
                sqlco.Connection = objConn;

                sqlco.CommandText = SQLSTR;

                sqlread = sqlco.ExecuteReader();
                return sqlread;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLiteClass - GetDATAREADER");
                return null;
            }
            finally
            {
                sqlco.Dispose();
                //sqlread.Close()
            }
        }

        public SQLiteDataAdapter GetDataAdapter(String SQLSTR)
        {
            SQLiteDataAdapter sqlAD = new SQLiteDataAdapter();
            DataTable sqlSET = new DataTable();
            SQLiteCommand sqlco = new SQLiteCommand();

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
                MessageBox.Show(ex.Message, "SQLiteClass - GetDataAdapter");
                return null;
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }
        }

        public DataTable GetDATATABLE(string SQLSTR)
        {

            //SQLiteCommand command = new SQLiteCommand(SQLSTR, objConn);
            //DataTable dataTable = new DataTable();
            //SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter();
            //dataAdapter.SelectCommand = command;
            //dataAdapter.Fill(dataTable);

            //return dataTable;

            SQLiteDataAdapter sqlAD = new SQLiteDataAdapter();
            DataTable sqlSET = new DataTable();
            SQLiteCommand sqlco = new SQLiteCommand();

            try
            {
                sqlco.CommandText = SQLSTR;
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

        public SQLiteConnection GetConnection()
        {
            return objConn;
        }


        public object ExecuteSQLScalar(string SQLSTR)
        {
            SQLiteCommand sqlco = new SQLiteCommand();
            try
            {
                sqlco.Connection = objConn;
                sqlco.CommandText = SQLSTR;
                return sqlco.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLiteClass - ExecuteSQLScalar");
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
            SQLiteCommand sqlco = new SQLiteCommand();
            try
            {
                sqlco.Connection = objConn;
                sqlco.CommandText = SQLSTR;
                return sqlco.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLiteClass - ExecuteSQL");
                functionReturnValue = 3;
            }
            finally
            {
                sqlco.Dispose();
            }
            return functionReturnValue;
        }

        public int ExecuteNonQuery(string SQLSTR, out SQLiteException ErrReport)
        {
            int functionReturnValue = 0;
            SQLiteCommand sqlco = new SQLiteCommand();
            ErrReport = null;

            try
            {

                sqlco.Connection = objConn;
                sqlco.CommandText = SQLSTR;

                return sqlco.ExecuteNonQuery();
            }
            catch (SQLiteException sEX)
            {
                ErrReport = sEX;
                functionReturnValue = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQLiteClass - ExecuteSQL");
                functionReturnValue = 0;
            }
            finally
            {
                sqlco.Dispose();
            }
            return functionReturnValue;
        }

 

        #endregion

    }

}
