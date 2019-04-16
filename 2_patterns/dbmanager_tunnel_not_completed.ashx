<%@ WebHandler Language="C#" Class="dbmanager" %>

using System;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

public class dbmanager : IHttpHandler
{

    private SQLClass db = null;


    public void ProcessRequest(HttpContext context)
    {

        //read POST parameters - https://stackoverflow.com/a/21020454
        string sql = context.Request.Params["sql"];
        string p = context.Request.Params["p"];

        if (sql == null || p == null)
        {
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize("Error: no token1");

            context.Response.ContentType = "application/json";
            context.Response.Write(json);
            return;

        }
        else if (p != "takis")
        {
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize("Error: no token2");

            context.Response.ContentType = "application/json";
            context.Response.Write(json);
        }

        //context.Response.ContentType = "application/json";
        //context.Response.Write(sql);
        //return;


        string ConnectionString = @"Data Source=.\sqlexpress
                                                ;Initial Catalog=x
                                                ;User ID=sa
                                                 ;Password=123456";

        SqlException err = null;
        db = new SQLClass(ConnectionString, out err);
        if (err != null)
        {
            context.Response.Write("Connect failed");
            return;
        }


        ClientRequest req = null;

        //https://www.c-sharpcorner.com/article/json-serialization-and-deserialization-in-c-sharp/
        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(sql)))
        {
            // Deserialization from JSON  
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(ClientRequest));
            req = (ClientRequest)deserializer.ReadObject(ms);
        }

        if (req != null)
        {
            if (req.q == "testconnection")
            {
                context.Response.Write("true");
                //ClientResponse j = new ClientResponse();
                //j.result = "true";

                //DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(ClientResponse));
                //MemoryStream msObj = new MemoryStream();
                //js.WriteObject(msObj, j);
                //msObj.Position = 0;
                //StreamReader sr = new StreamReader(msObj);

                //string json = sr.ReadToEnd();
                //sr.Close();
                //msObj.Close();

                //    Console.WriteLine(json);

            }
            else if (req.q == "dbschema")
            {
                context.Response.Write(dbschema());
            }
        }

        //$x = json_decode(sql);

        //if (p != "takis")
        //{
        //    var serializer = new JavaScriptSerializer();
        //    string json = serializer.Serialize("Error: no token2"); //https://docs.microsoft.com/en-us/dotnet/api/system.web.script.serialization.javascriptserializer?view=netframework-4.0
        //                                                            //new { param1 = "data1", param2 = "data2" });


        //    return;
        //}

        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");

        try
        {



        }
        catch (Exception t)
        {
            context.Response.Write(t.Message);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }



    public string dbschema()
    {
        base_response output = new base_response();
        output.data = new List<db_schema_tables>();
        output.method = ".net";

        DataTable dt = db.GetDATATABLE("SELECT sobjects.name FROM sysobjects sobjects WHERE sobjects.xtype = 'U' order by name");
        DataTable dtFields = null;


        db_schema_tables tbls = null;

        foreach (DataRow item in dt.Rows)
        {
            tbls = new db_schema_tables(item["name"].ToString());
            tbls.fields = new List<db_schema_table_fields>();



            dtFields = db.GetDATATABLE("exec sp_Columns [" + item["name"] + "]");

            foreach (DataRow fld in dtFields.Rows)
            {
                tbls.fields.Add(new db_schema_table_fields(fld["TYPE_NAME"].ToString(), fld["PRECISION"].ToString(), fld["COLUMN_NAME"].ToString(), fld["NULLABLE"].ToString()));
            }

            //table name 
            output.data.Add(tbls);
        }

        //        DataContractJsonSerializer js2 = new DataContractJsonSerializer(typeof(db_schema_head));
        //MemoryStream msObj2 = new MemoryStream();
        //js2.WriteObject(msObj2, output);
        //msObj2.Position = 0;
        //StreamReader sr2 = new StreamReader(msObj2);

        //string json2 = sr2.ReadToEnd();
        //sr2.Close();
        //msObj2.Close();



        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(base_response));
        MemoryStream msObj = new MemoryStream();
        js.WriteObject(msObj, output);
        msObj.Position = 0;
        StreamReader sr = new StreamReader(msObj);

        string json = sr.ReadToEnd();
        sr.Close();
        msObj.Close();



        return json;
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    //public send_to_client(string value)
    //{
    //        base_response
    //}

    public class ClientRequest
    {
        public string q { get; set; }
    }

    public class ClientResponse
    {
        public string result { get; set; }
    }


    [DataContract]
    public class base_response
    {
        [DataMember]
        public List<db_schema_tables> data { get; set; }

        [DataMember]
        public string compression { get; set; }

        [DataMember]
        public string method { get; set; }

        //public base_response(List<db_schema_tables> data, string compression )
        //{
        //    this.data = data;
        //    this.compression = compression;
        //}

    }

    //[DataContract]
    //public class db_schema_head
    //{
    //    [DataMember]
    //    public  result { get; set; }

    //    public db_schema_head()
    //    {
    //        this.result = new List<db_schema_tables>();
    //    }
    //}

    [DataContract]
    public class db_schema_tables
    {
        [DataMember]
        public string tablename { get; set; }
        [DataMember]
        public List<db_schema_table_fields> fields { get; set; }

        public db_schema_tables(string tablename)
        {
            this.tablename = tablename;
            this.fields = new List<db_schema_table_fields>();
        }
    }

    [DataContract]
    public class db_schema_table_fields
    {
        [DataMember]
        public string TYPE_NAME { get; set; }
        [DataMember]
        public string PRECISION { get; set; }
        [DataMember]
        public string COLUMN_NAME { get; set; }
        [DataMember]
        public string NULLABLE { get; set; }

        public db_schema_table_fields() { }

        public db_schema_table_fields(string TYPE_NAME, string PRECISION, string COLUMN_NAME, string NULLABLE)
        {
            this.TYPE_NAME = TYPE_NAME;
            this.PRECISION = PRECISION;
            this.COLUMN_NAME = COLUMN_NAME;
            this.NULLABLE = NULLABLE;
        }
    }





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
                return null;
            }
            finally
            {
                sqlco.Dispose();
                sqlAD.Dispose();
                sqlSET.Dispose();
            }
        }


        public DataSet GetDATASET2(string SQLSTR, out string error)
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
    }



}