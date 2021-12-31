using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aga.Controls.Tree;
using System.Data;
using System.Windows.Forms;

namespace DBManager.DBASES
{
    public class MyEventArgs : EventArgs
    {
        // ... your event args properties and methods here...
        public string Message { get; set; }
        public bool isWarning { get; set; }

        public MyEventArgs(string message, bool iswarning)
        {
            Message = message;
            isWarning = iswarning;
        }

    }

    public interface IdbType
    {
            event EventHandler<MyEventArgs> AddMessage;
            
            TreeModel GetSchemaModel();
            ListViewItem[] GetProcedures();
            string Connect();
            DataTable ExecuteSQL(string SQL, out string rowsAffected, out string error);
            string GenerateParameterInsert(TreeNodeAdv table);
            string GenerateParameterUpdate(TreeNodeAdv table);
            string GenerateSelect100(string table);
            string GenerateLast100(string table, string ID);
            string GenerateCountRows(string table);
            string ExecuteScalar(string SQL);
            string UpdateGrid(DataGridView DG);
            string parseProcedure(string procName,bool replaceCreate);
            string generatePROCselect(string tablename,List<string> fields,string PK);
            string generatePROCinsert(string tablename, List<ListStrings> fields);
            string generatePROCMerge(string tablename, List<ListStrings> fields);
            string generatePROCupdate(string tablename, List<ListStrings> fields, string PK);
            string generatePROCdelete(string tablename, string PK);
            string generatePROCnodeJS(string procName);
            string generateTableScript(string table);

            List<string> getTables();
            List<string> getTableFields(string table);
            DataTable getDatatable(string q);

            //Object getConnection();
            IDbConnection getConnection();
            string generateFORM(string procName, string formName);
            string generateFORMboostrap(string procName, string formName);
            void Disconnect();

            bool optionsShowRestoreScript();
            bool optionsProceduresFunctions();

          
    }
}
