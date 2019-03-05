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
