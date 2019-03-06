using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

        dynamic available_products = ExecuteReadQuery("select * from products", connectionSTR).ToList();
        Console.WriteLine(available_products[0].ProductID);


        public IEnumerable<ExpandoObject> ExecuteReadQuery(string query, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
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
        }

//IEnumerable structured - https://stackoverflow.com/a/11634411

public IEnumerable<Favorites> GetFavorites()
{
    using (SqlConnection sqlConnection = new SqlConnection(connString))
    {
        sqlConnection.Open();
        using (SqlCommand cmd = sqlConnection.CreateCommand())
        {
            cmd.CommandText = "Select * from favorites";
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Create a Favorites instance
                    var favorites = new Favorites();
                    favorites.Foo = reader["foo"];
                    // ... etc ...
                    yield return favorites;
                }
            }
        }
    }
}


/*
https://stackoverflow.com/a/31738645
https://stackoverflow.com/a/33238806
*/
SqlConnection con = new SqlConnection("connection string");
con.Open();
SqlCommand cmd = new SqlCommand("select top 10 documentid from document order by 1 desc", con);
SqlDataReader dr = cmd.ExecuteReader();             

List<string> docids = (from IDataRecord r in dr select (string)r["documentid"]).ToList<string>();
con.Close();

---------------------------------------------------------------
        
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAPP
{
    public class MsqlDbActions
    {

        public IEnumerable<ExpandoObject> ExecuteReadQuery(string query, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
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
        }

        public int ExecuteInsertQuery(string query, string connectionString)
        {
            int rows = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        rows = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rows;
        }

        public int ExecuteUpdateQuery(string query, string connectionString)
        {
            int rows = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        rows = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rows;
        }


        public object ExecuteScalarQuery(string query, string connectionString)
        {
            object result = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        result = cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        public IEnumerable<Doctor> GetAthlets(string query, string connectionString)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var doctor = new Doctor();

                            doctor.AthletID = Int64.Parse(reader["AthletID"].ToString());
                            doctor.Summation = Int64.Parse(reader["Summation"].ToString());

                            yield return doctor;
                        }
                    }
                }
            }
        }

		public IEnumerable<Bike> GetBikes(string query, string connectionString)
		{
			using (SqlConnection sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();
				using (SqlCommand cmd = sqlConnection.CreateCommand())
				{
					cmd.CommandText = query;
					cmd.CommandType = CommandType.Text;
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var Bike = new Bike();
		
							Bike.BikeID = Int64.Parse(reader["BikeID"].ToString());
							Bike.XYZID = reader["XYZID"] is DBNull ? (long?)null : Int64.Parse(reader["XYZID"].ToString());
							Bike.XYZDescription = reader["XYZDescription"] is DBNull ? null : reader["XYZDescription"].ToString();
							Bike.IsUrgent = reader["IsUrgent"] is DBNull ? (bool?) null : bool.Parse(reader["IsUrgent"].ToString());
							Bike.BikeDateSubmitted = reader["BikeDateSubmitted"] is DBNull ? (DateTime?)null : DateTime.Parse(reader["BikeDateSubmitted"].ToString());
							Bike.DateInserted =DateTime.Parse(reader["DateInserted"].ToString());
							Bike.IsActive = bool.Parse(reader["IsActive"].ToString());
		
							yield return Bike;
						}
					}
				}
			}
		}
    }
}
