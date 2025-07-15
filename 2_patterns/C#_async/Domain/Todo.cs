
//https://json2csharp.com/
namespace asyncExample.Domain
{
    // Todo myDeserializedClass = JsonConvert.DeserializeObject<Todo>(myJsonResponse);
    public class Todo
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }


}
