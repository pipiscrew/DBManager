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
