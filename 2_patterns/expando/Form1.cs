using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        SQLClass db;

        public Form1()
        {
            InitializeComponent();

            SqlException x;

            db = new SQLClass(@"data source=x;initial catalog=x;user id=x;Password=x;multipleactiveresultsets=True;application name=RequestProcessesApplication"
                               , out x);

            if (x != null)
                MessageBox.Show(x.Message);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("@ID", "1");
            fields.Add("@StatusID", "2");
            fields.Add("@DateSubmitted", DateTime.Now.ToString());

            db.ExecuteUpdateQuery(
                "update orders set " +
                "StatusID = @StatusID," +
                "DateSubmitted = @DateSubmitted " +
                "where ID = @ID",
                fields
            );
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dynamic available_bikes = db.ExecuteReadQuery("select * from bikes").ToList();


            if (available_bikes.Count == 0)
            {
                MessageBox.Show("There is no bike!");
                return;
            }
            else 
                Console.WriteLine(available_bikes[0].XYZDescription);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IEnumerable<ExpandoObject> BikeTest2 = db.ExecuteReadQuery("select * from bikes with statusid=2").ToList();

            if (BikeTest2.Count() == 0)
            {
                MessageBox.Show("There is no bike!");
                return;
            }


            //prepare a csv string
            
            string BikeCSV2 = string.Join(",", BikeTest2.Select(i => (i as IDictionary<string, object>)["BikeID"]));

            // OR //

            List<Bike> BikeTest = GetBikes("select * from bikes with statusid=2").ToList();

            foreach (Bike item in BikeTest)
            {
                Console.WriteLine(item.XYZDescription);
            }

            //prepare a csv string
            string BikeCSV = string.Join(",", BikeTest.Select(i => i.BikeID).ToArray());

        }


        public IEnumerable<Bike> GetBikes(string query)
        {
            using (SqlCommand cmd = db.GetConnection().CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var Bike = new Bike();

                        Bike.BikeID = long.Parse(reader["BikeID"].ToString());
                        Bike.XYZID = reader["XYZID"] is DBNull ? (long?)null : long.Parse(reader["XYZID"].ToString());
                        Bike.XYZDescription = reader["XYZDescription"] is DBNull ? null : reader["XYZDescription"].ToString();
                        Bike.IsUrgent = reader["IsUrgent"] is DBNull ? (bool?)null : bool.Parse(reader["IsUrgent"].ToString());
                        Bike.BikeDateSubmitted = reader["BikeDateSubmitted"] is DBNull ? (DateTime?)null : DateTime.Parse(reader["BikeDateSubmitted"].ToString());
                        Bike.DateInserted = DateTime.Parse(reader["DateInserted"].ToString());
                        Bike.IsActive = bool.Parse(reader["IsActive"].ToString());

                        yield return Bike;
                    }
                }
            }
        }


        public class Bike
        {
            public long BikeID { get; set; }
            public long? XYZID { get; set; }
            public string XYZDescription { get; set; }
            public bool? IsUrgent { get; set; }
            public DateTime? BikeDateSubmitted { get; set; }
            public DateTime DateInserted { get; set; }
            public bool IsActive { get; set; }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            //LINQ2Datable - https://stackoverflow.com/a/11593
            DataTable dT = db.GetDataTable("select * from bikes");

            Console.WriteLine(dT.Rows.Count.ToString());

            dg.DataSource = dT;           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            long statusID = long.Parse(db.ExecuteScalarQuery("select statusid from bikes where bikeid=12").ToString());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            /*
            https://stackoverflow.com/a/31738645
            https://stackoverflow.com/a/33238806
            */

            SqlCommand cmd = new SqlCommand("select top 10 documentid from document order by 1 desc", db.GetConnection());
            SqlDataReader dr = cmd.ExecuteReader();

            List<string> BikeIDs = (from IDataRecord r in dr select (string)r["BikeID"]).ToList<string>();
        }

    }

}
