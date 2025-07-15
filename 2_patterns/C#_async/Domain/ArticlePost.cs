using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asyncExample.Domain
{
    // Comment myDeserializedClass = JsonConvert.DeserializeObject<List<Comment>>(myJsonResponse);
    public class ArticlePost
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int userId { get; set; }
    }
}
