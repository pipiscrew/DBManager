using Oracle.ManagedDataAccess.Client;  
//https://www.nuget.org/packages/Oracle.ManagedDataAccess/
//No additional Oracle Client software is required to be installed to connect to Oracle Database

using System;
using System.Data;
using System.Windows.Forms;

namespace CHelper
{
    public class ORACLEClass
    {
        private OracleConnection objConn;
        private string m_ConnectionString;


        public ORACLEClass(string ConnectionString, out OracleException ExceptionObject)
        {
            try
            {
                m_ConnectionString = ConnectionString;
                Connect();

                   ExceptionObject = null;
            }
            catch (OracleException ex)
            {
                objConn = null;
                ExceptionObject = ex;
            }
        }

        private void Connect()
        {
            objConn = new OracleConnection(m_ConnectionString);
            objConn.Open();
            objConn.ModuleName = "SQL Developer"; //let admin know that you are Oracle.SQLDeveloper ;)
        }

        public DataTable GetDATATABLE(string SQLSTR)
        {
            OracleDataAdapter sqlAD = new OracleDataAdapter();
            DataTable sqlSET = new DataTable();
            OracleCommand sqlco = new OracleCommand();

            try
            {
                sqlco.CommandText = SQLSTR;
                sqlco.Connection = objConn;

                sqlAD.SelectCommand = sqlco;
                sqlAD.Fill(sqlSET);

                return sqlSET;

            }
            catch (OracleException ex)
            {

                if (ex.Number == 3113)
                {
                    Connect();
                    return GetDATATABLE(SQLSTR);
                }
                else
                {
                    MessageBox.Show(ex.Message, "OracleClass - GetDATATABLE");

                    return null;
                }
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }
        }

        public object ExecuteSQLScalar(string SQLSTR)
        {
            OracleCommand sqlco = new OracleCommand();
            try
            {
                sqlco.Connection = objConn;
                sqlco.CommandText = SQLSTR;
                return sqlco.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "OracleClass - ExecuteSQLScalar");
                return "";
            }
            finally
            {
                sqlco.Dispose();
            }
        }

        public OracleConnection GetConnection()
        {
            return objConn;
        }

        public bool IsConnected()
        {

            if (objConn == null | objConn.State != ConnectionState.Open)
                return false;
            else
                return true;

        }

        public void Close()
        {
            if ((objConn != null))
            {
                objConn.Close();
                objConn.Dispose();
            }
        }


    }
}
