using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBManager
{
    public  class treeItem2
    {

        public string table_name { get; set; }
        public List<treeItem2fields> table_fields { get; set; }

        public treeItem2(string table_name)
        {
            this.table_name = table_name;
            table_fields = new List<treeItem2fields>();
        }
    }

    public class treeItem2fields
    {
        public string field_name { get; set; }
        public string field_type { get; set; }
        public bool field_PK { get; set; }
        public string field_size { get; set; }
        public bool field_allow_null { get; set; }

        public treeItem2fields(string field_name, string field_type, string field_size, bool field_PK,bool field_allow_null)
        {
            this.field_name = field_name;
            this.field_type = field_type;
            this.field_size = field_size;
            this.field_PK = field_PK;
            this.field_allow_null = field_allow_null;
        }
    }
}
