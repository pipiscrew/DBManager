using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFClassGenerator
{
    public class CodeGenerator
    {
        public enum enumTemplateFile
        {
            Table,
            View,
            SelList,
            Exception
        }
        public MsSql sql { get; set; }
        public bool useDELETED { get; set; }
        public bool useUPDATED { get; set; }
        public bool useCREATED { get; set; }

        public string Namespace { get; set; }
        public string Context { get; set; }
        public string TableName { get; set; }
        public string IndexColumn { get; set; }
        public string IndexColumnDataType { get; set; }
        public string OrderColumn { get; set; }
        public string SaveFileName { get; set; }

        public CodeGenerator() { }
        public CodeGenerator(string ns, string context, string tablename, string indexcolumn, string sortcolumn)
        {
            this.Context = context;
            this.Namespace = ns;
            this.TableName = tablename;
            this.IndexColumn = indexcolumn;
            this.OrderColumn = sortcolumn;
            this.IndexColumnDataType = "int";
        }
        public string Generate(enumTemplateFile template)
        {
            return TemplateRead(template);
        }


        private string TemplateRead(enumTemplateFile fileTemplate)
        {
            string file = "";
            switch (fileTemplate)
            {
                case enumTemplateFile.Table:
                    file = "tplTable.txt";
                    SaveFileName = Helper.String.ToTitle(TableName);
                    break;
                case enumTemplateFile.View:
                    file = "tplView.txt";
                    SaveFileName = Helper.String.ToTitle(TableName);
                    break;
                case enumTemplateFile.SelList:
                    file = "tplSelList.txt";
                    SaveFileName = "SelList_" + Helper.String.ToTitle(TableName);
                    break;
                case enumTemplateFile.Exception:
                    file = "tplException.txt";
                    SaveFileName = "clsDatabaseException.cs";
                    break;
                default:
                    break;
            }

            StringBuilder sb = new StringBuilder();

            // Read the file and display it line by line.
            string line;
            using (StreamReader sr = new StreamReader(Path.Combine(Application.StartupPath, file)))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    bool add = true;

                    if (line.Contains("//CREATED:TRUE") && useCREATED == false) add = false;
                    if (line.Contains("//CREATED:FALSE") && useCREATED == true) add = false;
                    line = line.Replace("//CREATED:TRUE ", "");
                    line = line.Replace("//CREATED:FALSE ", "");


                    if (line.Contains("//DELETED:TRUE") && useDELETED == false) add = false;
                    if (line.Contains("//DELETED:FALSE") && useDELETED == true) add = false;
                    line = line.Replace("//DELETED:TRUE ", "");
                    line = line.Replace("//DELETED:FALSE ", "");

                    if (line.Contains("//UPDATED:TRUE") && useUPDATED == false) add = false;
                    if (line.Contains("//UPDATED:FALSE") && useUPDATED == true) add = false;
                    line = line.Replace("//UPDATED:TRUE ", "");
                    line = line.Replace("//UPDATED:FALSE ", "");

                    if (add) sb.AppendLine(line);

                }

            }


            sb.Replace("%GENERATED%", String.Format("Code generated {0} by EFClassGenerator version {1}", DateTime.Now, Application.ProductVersion.ToString()));
            sb.Replace("%CLASS%", Helper.String.ToTitle(TableName));
            sb.Replace("%NAMESPACE%", Namespace);
            sb.Replace("%EFCONTEXT%", Context);
            sb.Replace("%TABLE%", TableName);
            sb.Replace("%COL_PK%", IndexColumn);
            sb.Replace("%COL_DATATYPE%", IndexColumnDataType);
            sb.Replace("%ORDER_COL%", OrderColumn);


            return sb.ToString();
        }


    }


}
