using asyncExample.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace asyncExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var httpClientExample = new daHttpClient();
            //see https://dummyjson.com/docs
            var url = "https://jsonplaceholder.typicode.com/posts";
            var data = new { title = "foo", body = "bar", userId = 1 };

            try
            {
                var result = await httpClientExample.PostAsync<ArticlePost>(url, data);
                Console.WriteLine("Response received: " + JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var httpClientExample = new daHttpClient();

            var url = "https://www.pipiscrew.com/test/login.php";
            var data = new Dictionary<string, string>
                        {
                            {"umail" , "sdf@sdf.com"},
                            {"upassword" , "test"},
                        };

            try
            {
                var result = await httpClientExample.PostAsync(url, data);
                Console.WriteLine("Response received: " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var httpClientExample = new daHttpClient();

            var url = "http://192.168.1.55:5247/Home/popay";

            object data = null;
            //or can be 
            //List<ArticlePost> data = new List<ArticlePost>(){
            //    new ArticlePost(){
                    
            //        body = "12312@1232131.com", title = "12312321321",  id=1,userId=7
            //    }
            //};
 
            try
            {
                var result = await httpClientExample.PostAsync<List<ArticlePost>>(url, data);
                Console.WriteLine("Response received: " + JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
