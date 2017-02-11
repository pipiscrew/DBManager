using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DBManager
{
    //http://forums.codeguru.com/showthread.php?503925-RESOLVED-Serializing-an-Enum
   public class dbConnection
    {
        public string serverName { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string dbaseName { get; set; }
        public string port { get; set; }
        public string filename { get; set; } //sqlite // mdb
        public int TYPE { get; set; }
    }
}
