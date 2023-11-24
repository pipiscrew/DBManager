using DBManager.DBASES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBManager
{
    public partial class frmGeneratePHP_CRUD : BlueForm
    {
        Encoding outputEnc = new UTF8Encoding(false);
        //sub table
        string refTableMySQL = @"SELECT 
                                            TABLE_NAME,COLUMN_NAME,CONSTRAINT_NAME, REFERENCED_TABLE_NAME,REFERENCED_COLUMN_NAME
                                        FROM
                                            INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                                        WHERE
                                            REFERENCED_TABLE_NAME = '{0}'"; //https://stackoverflow.com/a/201678

        string refTableSQL = @"SELECT 
                                    TABLE_NAME,COLUMN_NAME,CONSTRAINT_NAME, REFERENCED_TABLE_NAME,REFERENCED_COLUMN_NAME
                                FROM
                                    INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                                WHERE
                                TABLE_NAME = '{0}' and REFERENCED_TABLE_NAME is not null and REFERENCED_COLUMN_NAME is not null";

        List<treeItem2> tables = null;
        public frmGeneratePHP_CRUD(List<treeItem2> tables)
        {
            InitializeComponent();
            this.tables = tables;

            this.Text = "Tables : " + tables.Count.ToString();
            cmbTemplate.SelectedIndex = 4;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (txtFolder.Text.Trim().Length == 0)
            {
                General.Mes("please type a folder");
                return;
            }

            //create export folder 
            string exportDIR = Application.StartupPath + "\\" + txtFolder.Text + "\\";
            if (Directory.Exists(exportDIR))
            {
                General.Mes("Folder already exists!");
                return;
            }
            else
                Directory.CreateDirectory(exportDIR);

            if (cmbTemplate.SelectedIndex == 0)
                do_template_adminLTE();
            else if (cmbTemplate.SelectedIndex == 1)
                do_template_bootstraptable();
            else if (cmbTemplate.SelectedIndex == 2)
                do_template_bootstraptable5();
            else if (cmbTemplate.SelectedIndex == 3)
                do_template_vue2_vuetify();
            else if (cmbTemplate.SelectedIndex == 4)
                do_template_vue3_vuetify();

            Process.Start(exportDIR);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string f = string.Empty;

                if (cmbTemplate.SelectedIndex == 0)
                {
                    f = Path.Combine(Application.StartupPath, "PHPtemplateCRUD.zip");
                    File.WriteAllBytes(f, DBManager.Properties.Resources.PHPtemplateCRUD);
                }
                else if (cmbTemplate.SelectedIndex == 1)
                {
                    f = Path.Combine(Application.StartupPath, "PHPtemplateCRUDbootstraptable.zip");
                    File.WriteAllBytes(f, DBManager.Properties.Resources.PHPtemplateCRUDbootstraptable);
                }
                else if (cmbTemplate.SelectedIndex == 2)
                {
                    f = Path.Combine(Application.StartupPath, "PHPtemplateAPIbootstraptable.zip");
                    File.WriteAllBytes(Path.Combine(Application.StartupPath, "PHPtemplateAPIbootstraptable.zip"), DBManager.Properties.Resources.PHPtemplateAPIbootstraptable);
                }
                else if (cmbTemplate.SelectedIndex == 3)
                {
                    f = Path.Combine(Application.StartupPath, "PHPtemplateVue2.zip");
                    File.WriteAllBytes(Path.Combine(Application.StartupPath, "PHPtemplateVue2.zip"), DBManager.Properties.Resources.PHPtemplateVue2);
                }
                else if (cmbTemplate.SelectedIndex == 4)
                {
                    f = Path.Combine(Application.StartupPath, "PHPtemplateVue3.zip");
                    File.WriteAllBytes(Path.Combine(Application.StartupPath, "PHPtemplateVue3.zip"), DBManager.Properties.Resources.PHPtemplateVue3);
                }

                General.Mes(string.Format("Successfully extracted to '{0}'", f));
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message, MessageBoxIcon.Error);
            }
        }


        private void do_template_vue3_vuetify()
        {
            string appDIR = Path.Combine(Application.StartupPath, txtFolder.Text);
            string apiDIR = Path.Combine(appDIR, "api");
            Directory.CreateDirectory(apiDIR);
            string compDIR = Path.Combine(appDIR, "src/components");
            Directory.CreateDirectory(compDIR);
            string entitiesDIR = Path.Combine(appDIR, "src/entities");
            Directory.CreateDirectory(entitiesDIR);
            string srcDIR = Path.Combine(appDIR, "src");

            string table_name;

            //API
            string CRUD4phpGetRecodsFKtemplate = DBManager.Properties.Resources.CRUD4phpGetRecodsFK;
            string CRUD4phpGetRecodsFKcallTemplate = DBManager.Properties.Resources.CRUD4phpGetRecodsFKcall;
            string CRUD4phpAPItemplate = DBManager.Properties.Resources.CRUD4phpAPI;
            string dummy;


            //////////////////// Enitity Vue
            string CRUD4entityVUETemplate = DBManager.Properties.Resources.CRUD5entityVUE;


            string appVueTemplate = "        <v-tab to=\"/{0}\">{1}</v-tab>\n";
            string appVue = "";


            string CRUD4appVueMainTemplate = DBManager.Properties.Resources.CRUD5appVueMain;


            string CRUD4routeTemplate = DBManager.Properties.Resources.CRUD5route;
            string CRUD4route = "";
            string CRUD4routeMainTemplate = DBManager.Properties.Resources.CRUD5routeTemplate;
            string firstTable = "";

            foreach (treeItem2 tbl in tables)
            {
                if (string.IsNullOrEmpty(firstTable))
                    firstTable = tbl.table_name.ToLower();

                table_name = tbl.table_name.Capitalize();


                //TABLE API
                string phpAPI = "";
                string selectSQLTemplate = "SELECT {0} FROM {1}\n{2}";
                string selectSQLJoinTemplate = "LEFT JOIN {0} ON {0}.{1} = {2}.{3}\n";
                string selectSQL = "";
                string selectSQLJoin = "";
                string insertSQLSelect = string.Join(", ", tbl.table_fields.Skip(1).Select(x => x.field_name));
                string insertSQLBind = ":" + string.Join(", :", tbl.table_fields.Skip(1).Select(x => x.field_name));
                string insertSQL = string.Format("INSERT INTO `{0}` ({1}) VALUES ({2})", table_name, insertSQLSelect, insertSQLBind);
                string updateSQLTemplate = "UPDATE `{0}` set {1} WHERE {2}";
                string updateSQLset = "";
                string updateSQLwhere = "";
                string updateSQL = "";
                string counterSQLTemplate = "select count({0}) from {1}";
                string counterSQL = "";
                string phpGetRecodsFK = "";
                string phpGetRecodsFKcall = "";

                string pk = "";
                string bindPHPcodeTemplate = "	$stmt->bindValue(':{0}' , $_POST['{0}']);\n";
                string bindPHPcodeTemplateBoolean = "	$stmt->bindValue(':{0}' , ${0}, PDO::PARAM_INT);\n";
                string bindPHPcodeTemplateBooleanVARtemplate = "	${0} = filter_var($_POST['{0}'], FILTER_VALIDATE_BOOLEAN);\n";
                string bindPHPcodeTemplateVARS = "";
                string bindPHPcode = "";

                string PHPvalidationTemplate = "!isset($_POST['{0}']) || ";
                string PHPvalidation = "";

                string PHPfillgridTemplate = "							 '{0}',\n";
                string PHPfillgrid = "";

                //Entity.vue
                string templatePK = "        { title: \"{0}\", key: \"{0}\", align: \" d-none\" }, // ' d-none' hides the column but keeps the search ability\n";
                string templatePlain = "        { title: \"{0}\", key: \"{0}\" },\n";
                string entinyVueHeaders = "";

                DataTable refTables = null;

                if (General.DB is MySQL)
                {
                    refTables = General.DB.ExecuteSQL(string.Format(refTableSQL, table_name), out dummy, out dummy);
                    if (refTables == null || refTables.Rows.Count == 0)
                        refTables = null;
                }

                ///////////////////////////////////////////   TABLE API START  ///////////////////////////////////////////   
                foreach (treeItem2fields field in tbl.table_fields)
                {
                    PHPfillgrid += string.Format(PHPfillgridTemplate, field.field_name);
                    if (field.field_PK)
                    {
                        pk = field.field_name;
                        updateSQLwhere = string.Format("{0}=:{0}", field.field_name);

                        entinyVueHeaders += templatePK.Replace("{0}", pk);
                    }
                    else
                    {
                        entinyVueHeaders += templatePlain.Replace("{0}", field.field_name);

                        updateSQLset += string.Format("{0}=:{0}, ", field.field_name);
                        PHPvalidation += string.Format(PHPvalidationTemplate, field.field_name);

                        //BIND PHP CODE
                        if (field.field_type.ToLower() == "bit" && chkBIT.Checked)
                        {
                            bindPHPcode += string.Format(bindPHPcodeTemplateBoolean, field.field_name);
                            bindPHPcodeTemplateVARS += string.Format(bindPHPcodeTemplateBooleanVARtemplate, field.field_name);
                        }
                        else if (field.field_type.ToLower() == "tinyint" && chkTINYINT.Checked)
                        {
                            bindPHPcode += string.Format(bindPHPcodeTemplateBoolean, field.field_name);
                            bindPHPcodeTemplateVARS += string.Format(bindPHPcodeTemplateBooleanVARtemplate, field.field_name);
                        }
                        else // ALL OTHER TYPES
                            bindPHPcode += string.Format(bindPHPcodeTemplate, field.field_name);
                    }

                    //selectSQL += string.Format("{0}, ", check4date(field));

                    ///////////////////////////////////////////   TABLE API END  ///////////////////////////////////////////  


                    ///////////////////////////////////////////   FIND REFERENCE TABLE [start]  ///////////////////////////////////////////  

                    if (General.DB is MySQL)
                    {
                        if (refTables != null)
                        {
                            var matchFieldREF = refTables.AsEnumerable().Where(row => row.Field<string>("COLUMN_NAME").Equals(field.field_name)).FirstOrDefault();
                            if (matchFieldREF != null)
                            {
                                string refTable = matchFieldREF["REFERENCED_TABLE_NAME"].ToString();
                                string refTableID = matchFieldREF["REFERENCED_COLUMN_NAME"].ToString();
                                selectSQL += string.Format("{0}.title as {1}, ", refTable, field.field_name);
                                selectSQLJoin += string.Format(selectSQLJoinTemplate, refTable, refTableID, table_name, field.field_name);

                                phpGetRecodsFK += CRUD4phpGetRecodsFKtemplate.Replace("{0}", refTable.Capitalize()).Replace("{1}", refTableID).Replace("{2}", "title");
                                phpGetRecodsFKcall += CRUD4phpGetRecodsFKcallTemplate.Replace("{0}", refTable.Capitalize());

                                lst.Items.Add("At " + table_name + " found reference with table : " + refTable);
                                lst.Items.Add("ref field is : " + refTableID);
                                lst.Items.Add("~~Table referenced~~");
                                lst.Items.Add("");
                            }
                            else
                                selectSQL += string.Format("{0}, ", check4date(field));
                        }
                        else
                            selectSQL += string.Format("{0}, ", check4date(field));
                    }
                    else
                    { //other db systems
                        string[] returnVAL = check4tie(field.field_name);
                        //0 - ID // 1 - TXT // 2 - TABLE // 3 - DEST TABLE FIELDS COUNT
                        if (returnVAL[0] != null && returnVAL[1] != null && returnVAL[2] != null)
                        {
                            string refTable = returnVAL[2];
                            string refTableID = returnVAL[0];
                            string refTableTXT = returnVAL[1];
                            selectSQL += string.Format("{0}.{1} as {2}, ", refTable, refTableTXT, field.field_name);
                            selectSQLJoin += string.Format(selectSQLJoinTemplate, refTable, refTableID, table_name, field.field_name);

                            phpGetRecodsFK += CRUD4phpGetRecodsFKtemplate.Replace("{0}", refTable.Capitalize()).Replace("{1}", refTableID).Replace("{2}", refTableTXT);
                            phpGetRecodsFKcall += CRUD4phpGetRecodsFKcallTemplate.Replace("{0}", refTable.Capitalize());

                            lst.Items.Add("At " + table_name + " found reference with table : " + refTable);
                            lst.Items.Add("ref field is : " + refTableID);
                            lst.Items.Add("~~Table referenced~~");
                            lst.Items.Add("");
                        }
                        else
                            selectSQL += string.Format("{0}, ", check4date(field)); //field.field_name);
                    }
                    ///////////////////////////////////////////   FIND REFERENCE TABLE [end]  ///////////////////////////////////////////  



                } // ~~ loop fields end~~




                string CRUD4entityVUE = "";

                //construct entityVUE
                /*
                {0} - Entiity Capitalize
                {1} - Entity PK
                {2} - Expandable ELEMENT TAG ======== if not expandable should be add the -> '>'
                {3} - headers
                */
                CRUD4entityVUE = CRUD4entityVUETemplate.Replace("{0}", table_name).Replace("{0l}", table_name.ToLower()).Replace("{1}", pk)
                    .Replace("{3}", entinyVueHeaders);


                updateSQLset = updateSQLset.Substring(0, updateSQLset.Length - 2);
                updateSQL = string.Format(updateSQLTemplate, table_name, updateSQLset, updateSQLwhere);

                PHPvalidation = PHPvalidation.Substring(0, PHPvalidation.Length - 4);
                //Console.WriteLine(PHPvalidation);

                PHPfillgrid = PHPfillgrid.Substring(0, PHPfillgrid.Length - 2);
                //Console.WriteLine(PHPfillgrid);

                if (selectSQLJoin.Length > 5)
                    selectSQLJoin = selectSQLJoin.Substring(0, selectSQLJoin.Length - 1);

                selectSQL = selectSQL.Substring(0, selectSQL.Length - 2);
                selectSQL = string.Format(selectSQLTemplate, selectSQL, table_name, selectSQLJoin);

                counterSQL = string.Format(counterSQLTemplate, pk, table_name);

                /*
                {0} = tbl name
                {1} = PK
                {2} = boolean validation
                {3} = update SQL
                {4} = insert SQL
                {5} = bind
                {6} = GetRecordsFK                              [for entityDetail - SELECT elements]
                {7} = fillgrid cols
                {8} = fillgrid select SQL
                {9} = fillgrid COUNTER select SQL
                {10} = API FK call
                {11} = PHP Save $POST fields validation
                {12} = when is FK somewhere PROC API CALL       [expanded]
                {13} = when is FK somewhere function GetRecords [expanded]
                 */

                phpAPI = CRUD4phpAPItemplate.Replace("{0}", table_name).Replace("{1}", pk)
                    .Replace("{2}", bindPHPcodeTemplateVARS).Replace("{3}", updateSQL)
                    .Replace("{4}", insertSQL).Replace("{5}", bindPHPcode)
                    .Replace("{6}", phpGetRecodsFK).Replace("{7}", PHPfillgrid)
                    .Replace("{8}", selectSQL).Replace("{9}", counterSQL)
                    .Replace("{10}", phpGetRecodsFKcall).Replace("{11}", PHPvalidation)
                    .Replace("{12}", "").Replace("{13}", ""); //12+13 is for vue2 expandable


                ///////////////////////////////////////////PHP API
                File.WriteAllText(Path.Combine(apiDIR, string.Format("{0}API.php", table_name)), phpAPI, outputEnc);

                ///////////////////////////////////////////JS Entity
                string tableEntityJS = helper_template_vuetify_entityJS_VUE3(table_name, tbl.table_fields);
                File.WriteAllText(Path.Combine(entitiesDIR, string.Format("{0}.js", table_name)), tableEntityJS, outputEnc);

                ///////////////////////////////////////////Entity Vue
                File.WriteAllText(Path.Combine(compDIR, string.Format("{0}.vue", table_name)), CRUD4entityVUE, outputEnc);

                ////////////////////////////////// DETAILS COMPONENT ////////////////////////////////// 
                string componentDetail = helper_template_vuetify_details_component_VUE3(refTables, tbl);
                File.WriteAllText(Path.Combine(compDIR, string.Format("{0}Detail.vue", table_name)), componentDetail, outputEnc);

                ///////// COMMON FILES /////////
                appVue += appVueTemplate.Replace("{0}", table_name.ToLower()).Replace("{1}", table_name);
                //appVueMenu += appVueMenuTemplate.Replace("{0}", table_name.ToLower()).Replace("{1}", table_name);
                CRUD4route += CRUD4routeTemplate.Replace("{0}", table_name.ToLower()).Replace("{1}", table_name);
            }


            ///COMMON FILES
            //router.js
            string CRUD4routeMain = CRUD4routeMainTemplate.Replace("{0}", firstTable).Replace("{1}", CRUD4route);
            File.WriteAllText(Path.Combine(srcDIR, "router.js"), CRUD4routeMain, outputEnc);

            string CRUD4appVueMain = CRUD4appVueMainTemplate.Replace("{0}", appVue);
            File.WriteAllText(Path.Combine(srcDIR, "App.vue"), CRUD4appVueMain, outputEnc);
        }


        private void do_template_vue2_vuetify()
        {
            string appDIR = Path.Combine(Application.StartupPath, txtFolder.Text);
            string apiDIR = Path.Combine(appDIR, "api");
            Directory.CreateDirectory(apiDIR);
            string compDIR = Path.Combine(appDIR, "src/components");
            Directory.CreateDirectory(compDIR);
            string entitiesDIR = Path.Combine(appDIR, "src/entities");
            Directory.CreateDirectory(entitiesDIR);
            string srcDIR = Path.Combine(appDIR, "src");

            string table_name;

            string CRUD4phpGetRecodsFKtemplate = DBManager.Properties.Resources.CRUD4phpGetRecodsFK;
            string CRUD4phpGetRecodsFKcallTemplate = DBManager.Properties.Resources.CRUD4phpGetRecodsFKcall;
            string CRUD4phpAPItemplate = DBManager.Properties.Resources.CRUD4phpAPI;
            string dummy;

            //////////////////// Enitity Vue
            string CRUD4entityVUETemplate = DBManager.Properties.Resources.CRUD4entityVUE;
            string CRUD4entityVUEexpandableElementTemplate = DBManager.Properties.Resources.CRUD4entityVUEexpandableElement;
            string CRUD4entityVUEexpandableCodeTemplate = DBManager.Properties.Resources.CRUD4entityVUEexpandableCode;
            string CRUD4entityVUEexpandableCSS = DBManager.Properties.Resources.CRUD4entityVUEexpandableCSS;
            //Enitity Vue - subtable (expandable)
            string CRUD4phpGetRecordDetailsCallTemplate = DBManager.Properties.Resources.CRUD4phpGetRecordDetailsCall;
            string CRUD4phpGetRecordDetailsTemplate = DBManager.Properties.Resources.CRUD4phpGetRecordDetails;
            string appVueTemplate = "        <v-tab to=\"/{0}\">{1}</v-tab>\n";
            string appVue = "";
            string appVueMenuTemplate = "          <v-list-item to=\"/{0}\"><v-list-item-title>{1}</v-list-item-title></v-list-item>\n";
            string appVueMenu = "";

            string CRUD4appVueMainTemplate = DBManager.Properties.Resources.CRUD4appVueMain;


            string CRUD4routeTemplate = DBManager.Properties.Resources.CRUD4route;
            string CRUD4route = "";
            string CRUD4routeMainTemplate = DBManager.Properties.Resources.CRUD4routeTemplate;
            string firstTable = "";

            foreach (treeItem2 tbl in tables)
            {
                if (string.IsNullOrEmpty(firstTable))
                    firstTable = tbl.table_name.ToLower();

                table_name = tbl.table_name.Capitalize();

                //TABLE API
                string phpAPI = "";
                string selectSQLTemplate = "SELECT {0} FROM {1}\n{2}";
                string selectSQLJoinTemplate = "LEFT JOIN {0} ON {0}.{1} = {2}.{3}\n";
                string selectSQL = "";
                string selectSQLJoin = "";
                string insertSQLSelect = string.Join(", ", tbl.table_fields.Skip(1).Select(x => x.field_name));
                string insertSQLBind = ":" + string.Join(", :", tbl.table_fields.Skip(1).Select(x => x.field_name));
                string insertSQL = string.Format("INSERT INTO `{0}` ({1}) VALUES ({2})", table_name, insertSQLSelect, insertSQLBind);
                string updateSQLTemplate = "UPDATE `{0}` set {1} WHERE {2}";
                string updateSQLset = "";
                string updateSQLwhere = "";
                string updateSQL = "";
                string counterSQLTemplate = "select count({0}) from {1}";
                string counterSQL = "";
                string phpGetRecodsFK = "";
                string phpGetRecodsFKcall = "";

                string pk = "";
                string bindPHPcodeTemplate = "	$stmt->bindValue(':{0}' , $_POST['{0}']);\n";
                string bindPHPcodeTemplateBoolean = "	$stmt->bindValue(':{0}' , ${0}, PDO::PARAM_INT);\n";
                string bindPHPcodeTemplateBooleanVARtemplate = "	${0} = filter_var($_POST['{0}'], FILTER_VALIDATE_BOOLEAN);\n";
                string bindPHPcodeTemplateVARS = "";
                string bindPHPcode = "";

                string PHPvalidationTemplate = "!isset($_POST['{0}']) || ";
                string PHPvalidation = "";

                string PHPfillgridTemplate = "							 '{0}',\n";
                string PHPfillgrid = "";

                //Entity.vue
                string templatePK = "        { text: \"{0}\", value: \"{0}\", align: \" d-none\" }, // ' d-none' hides the column but keeps the search ability\n";
                string templatePlain = "        { text: \"{0}\", value: \"{0}\" },\n";
                string entinyVueHeaders = "";

                DataTable refTables = null;

                if (General.DB is MySQL)
                {
                    refTables = General.DB.ExecuteSQL(string.Format(refTableSQL, table_name), out dummy, out dummy);
                    if (refTables == null || refTables.Rows.Count == 0)
                        refTables = null;
                }

                ///////////////////////////////////////////   TABLE API  ///////////////////////////////////////////   
                foreach (treeItem2fields field in tbl.table_fields)
                {

                    PHPfillgrid += string.Format(PHPfillgridTemplate, field.field_name);
                    if (field.field_PK)
                    {
                        pk = field.field_name;
                        updateSQLwhere = string.Format("{0}=:{0}", field.field_name);

                        entinyVueHeaders += templatePK.Replace("{0}", pk);
                    }
                    else
                    {
                        entinyVueHeaders += templatePlain.Replace("{0}", field.field_name);

                        updateSQLset += string.Format("{0}=:{0}, ", field.field_name);
                        PHPvalidation += string.Format(PHPvalidationTemplate, field.field_name);

                        //BIND PHP CODE
                        if (field.field_type.ToLower() == "bit" && chkBIT.Checked)
                        {
                            bindPHPcode += string.Format(bindPHPcodeTemplateBoolean, field.field_name);
                            bindPHPcodeTemplateVARS += string.Format(bindPHPcodeTemplateBooleanVARtemplate, field.field_name);
                        }
                        else if (field.field_type.ToLower() == "tinyint" && chkTINYINT.Checked)
                        {
                            bindPHPcode += string.Format(bindPHPcodeTemplateBoolean, field.field_name);
                            bindPHPcodeTemplateVARS += string.Format(bindPHPcodeTemplateBooleanVARtemplate, field.field_name);
                        }
                        else // ALL OTHER TYPES
                            bindPHPcode += string.Format(bindPHPcodeTemplate, field.field_name);
                    }

                    /////////////////////////////////////////////////// FIND REFERENCE TABLE [start]
                    if (General.DB is MySQL)
                    {
                        if (refTables != null)
                        {
                            var matchFieldREF = refTables.AsEnumerable().Where(row => row.Field<string>("COLUMN_NAME").Equals(field.field_name)).FirstOrDefault();
                            if (matchFieldREF != null)
                            {
                                string refTable = matchFieldREF["REFERENCED_TABLE_NAME"].ToString();
                                string refTableID = matchFieldREF["REFERENCED_COLUMN_NAME"].ToString();
                                selectSQL += string.Format("{0}.title as {1}, ", refTable, field.field_name);
                                selectSQLJoin += string.Format(selectSQLJoinTemplate, refTable, refTableID, table_name, field.field_name);

                                phpGetRecodsFK += CRUD4phpGetRecodsFKtemplate.Replace("{0}", refTable.Capitalize()).Replace("{1}", refTableID).Replace("{2}", "title");
                                phpGetRecodsFKcall += CRUD4phpGetRecodsFKcallTemplate.Replace("{0}", refTable.Capitalize());

                                lst.Items.Add("At " + table_name + " found reference with table : " + refTable);
                                lst.Items.Add("ref field is : " + refTableID);
                                lst.Items.Add("~~Table referenced~~");
                                lst.Items.Add("");
                            }
                            else
                                selectSQL += string.Format("{0}, ", check4date(field));
                        }
                        else
                            selectSQL += string.Format("{0}, ", check4date(field));
                    }
                    else
                    { //other db systems
                        string[] returnVAL = check4tie(field.field_name);
                        //0 - ID // 1 - TXT // 2 - TABLE // 3 - DEST TABLE FIELDS COUNT
                        if (returnVAL[0] != null && returnVAL[1] != null && returnVAL[2] != null)
                        {
                            string refTable = returnVAL[2];
                            string refTableID = returnVAL[0];
                            string refTableTXT = returnVAL[1];
                            selectSQL += string.Format("{0}.{1} as {2}, ", refTable, refTableTXT, field.field_name);
                            selectSQLJoin += string.Format(selectSQLJoinTemplate, refTable, refTableID, table_name, field.field_name);

                            phpGetRecodsFK += CRUD4phpGetRecodsFKtemplate.Replace("{0}", refTable.Capitalize()).Replace("{1}", refTableID).Replace("{2}", refTableTXT);
                            phpGetRecodsFKcall += CRUD4phpGetRecodsFKcallTemplate.Replace("{0}", refTable.Capitalize());

                            lst.Items.Add("At " + table_name + " found reference with table : " + refTable);
                            lst.Items.Add("ref field is : " + refTableID);
                            lst.Items.Add("~~Table referenced~~");
                            lst.Items.Add("");
                        }
                        else
                            selectSQL += string.Format("{0}, ", check4date(field)); //field.field_name);
                    }
                    /////////////////////////////////////////////////// FIND REFERENCE TABLE [end]
                } // ~~ loop fields end~~

                ////////// [EXPANDABLE] CHECK IF CURRENT TABLE IS FK SOMEWHERE [start]
                DataTable r = General.DB.ExecuteSQL(string.Format(refTableMySQL, table_name), out dummy, out dummy);
                bool hasSubTable = false;
                string subTableTH = "";
                string subTableTD = "";
                string subFieldsCSV = "";
                string subPK = "";
                string subTable = "";
                if (r != null && r.Rows.Count > 0)
                {
                    int tableIndex = 0;
                    if (r.Rows.Count > 1)
                    {
                        int frmExpandSelectionRET = -1;
                        frmGeneratePHP_CRUDexpandable frmExpandSelection = new frmGeneratePHP_CRUDexpandable(table_name, r.AsEnumerable().Select(x => x.Field<string>(0)).ToArray());
                        frmExpandSelection.ShowDialog(out frmExpandSelectionRET);
                        tableIndex = frmExpandSelectionRET;
                    }

                    if (tableIndex > -1) //when multiFK && user selected a table, otherwise when signleFK get 0
                    {
                        subTable = r.Rows[tableIndex][0].ToStrinX();
                        subPK = r.Rows[tableIndex][1].ToStrinX();
                    }

                    if (tableIndex > -1 && !string.IsNullOrEmpty(subTable) && !string.IsNullOrEmpty(subTable))
                    {
                        hasSubTable = true;
                        DataTable refField = General.DB.ExecuteSQL(string.Format("SHOW COLUMNS FROM {0}", subTable), out dummy, out dummy);
                        var refFieldsCSVpre = refField.AsEnumerable().Select(x => x.Field<string>("Field")).ToList();
                        subFieldsCSV = string.Join(", ", refFieldsCSVpre);
                        string subTableTHtemplate = "                          <th class=\"text-left\">{0}</th>\n";
                        string subTableTDtemplate = "                      <td class=\"text-left\">{{ rec.{0} }}</td>\n";

                        foreach (var refCol in refFieldsCSVpre)
                        {
                            subTableTH += subTableTHtemplate.Replace("{0}", refCol.Capitalize());
                            subTableTD += subTableTDtemplate.Replace("{0}", refCol);
                        }

                        lst.Items.Add("At " + table_name + " found reference with table : " + subTable);
                        lst.Items.Add("ref field is : " + subPK);
                        lst.Items.Add("~~Table referenced as Expanded~~");
                        lst.Items.Add("");
                    }
                }
                //////////  [EXPANDABLE]CHECK IF CURRENT TABLE IS FK SOMEWHERE [end]

                // [EXPANDABLE] functions [start]


                string CRUD4phpGetRecordDetailsCall = "";
                string CRUD4phpGetRecordDetails = "";
                string CRUD4entityVUE = "";
                string expandableElement = ">";
                string expandableCode = "";
                string expandableCSS = "";
                if (hasSubTable)
                {
                    //construct php API file - INJECTION
                    CRUD4phpGetRecordDetailsCall = CRUD4phpGetRecordDetailsCallTemplate.Replace("{0}", table_name);
                    CRUD4phpGetRecordDetails = CRUD4phpGetRecordDetailsTemplate.Replace("{0}", subPK)
                                                                                .Replace("{1}", subFieldsCSV)
                                                                                .Replace("{2}", subTable)
                                                                                .Replace("{3}", table_name);

                    expandableElement = CRUD4entityVUEexpandableElementTemplate.Replace("{0}", subTableTH).Replace("{1}", subTableTD);
                    expandableCode = CRUD4entityVUEexpandableCodeTemplate.Replace("{0}", table_name).Replace("{1}", pk).Replace("{4}", subPK);
                    expandableCSS = CRUD4entityVUEexpandableCSS;
                }



                // [EXPANDABLE] functions [end]


                //construct entityVUE
                /*
                {0} - Entiity Capitalize
                {1} - Entity PK
                {2} - Expandable ELEMENT TAG ======== if not expandable should be add the -> '>'
                {3} - headers
                {4} - Expandable CSS to disable border |-went to expandableCode-{4} - CHILD - refID
                {5} - Expandable Code
                    */
                CRUD4entityVUE = CRUD4entityVUETemplate.Replace("{0}", table_name).Replace("{1}", pk).Replace("{2}", expandableElement)
                    .Replace("{3}", entinyVueHeaders).Replace("{4}", expandableCSS).Replace("{5}", expandableCode);


                updateSQLset = updateSQLset.Substring(0, updateSQLset.Length - 2);
                updateSQL = string.Format(updateSQLTemplate, table_name, updateSQLset, updateSQLwhere);

                PHPvalidation = PHPvalidation.Substring(0, PHPvalidation.Length - 4);
                //Console.WriteLine(PHPvalidation);

                PHPfillgrid = PHPfillgrid.Substring(0, PHPfillgrid.Length - 2);
                //Console.WriteLine(PHPfillgrid);

                if (selectSQLJoin.Length > 5)
                    selectSQLJoin = selectSQLJoin.Substring(0, selectSQLJoin.Length - 1);

                selectSQL = selectSQL.Substring(0, selectSQL.Length - 2);
                selectSQL = string.Format(selectSQLTemplate, selectSQL, table_name, selectSQLJoin);

                counterSQL = string.Format(counterSQLTemplate, pk, table_name);

                /*
                {0} = tbl name
                {1} = PK
                {2} = boolean validation
                {3} = update SQL
                {4} = insert SQL
                {5} = bind
                {6} = GetRecordsFK                              [for entityDetail - SELECT elements]
                {7} = fillgrid cols
                {8} = fillgrid select SQL
                {9} = fillgrid COUNTER select SQL
                {10} = API FK call
                {11} = PHP Save $POST fields validation
                {12} = when is FK somewhere PROC API CALL       [expanded]
                {13} = when is FK somewhere function GetRecords [expanded]
                 */

                phpAPI = CRUD4phpAPItemplate.Replace("{0}", table_name).Replace("{1}", pk)
                    .Replace("{2}", bindPHPcodeTemplateVARS).Replace("{3}", updateSQL)
                    .Replace("{4}", insertSQL).Replace("{5}", bindPHPcode)
                    .Replace("{6}", phpGetRecodsFK).Replace("{7}", PHPfillgrid)
                    .Replace("{8}", selectSQL).Replace("{9}", counterSQL)
                    .Replace("{10}", phpGetRecodsFKcall).Replace("{11}", PHPvalidation)
                    .Replace("{12}", CRUD4phpGetRecordDetailsCall).Replace("{13}", CRUD4phpGetRecordDetails);


                ///////////////////////////////////////////PHP API
                File.WriteAllText(Path.Combine(apiDIR, string.Format("{0}API.php", table_name)), phpAPI, outputEnc);

                ///////////////////////////////////////////JS Entity
                string tableEntityJS = helper_template_vuetify_entityJS_VUE2(table_name, tbl.table_fields);
                File.WriteAllText(Path.Combine(entitiesDIR, string.Format("{0}.js", table_name)), tableEntityJS, outputEnc);

                ///////////////////////////////////////////Entity Vue
                File.WriteAllText(Path.Combine(compDIR, string.Format("{0}.vue", table_name)), CRUD4entityVUE, outputEnc);

                ////////////////////////////////// DETAILS COMPONENT ////////////////////////////////// 
                string componentDetail = helper_template_vuetify_details_component_VUE2(refTables, tbl);
                File.WriteAllText(Path.Combine(compDIR, string.Format("{0}Detail.vue", table_name)), componentDetail, outputEnc);

                ///////// COMMON FILES /////////
                appVue += appVueTemplate.Replace("{0}", table_name.ToLower()).Replace("{1}", table_name);
                appVueMenu += appVueMenuTemplate.Replace("{0}", table_name.ToLower()).Replace("{1}", table_name);
                CRUD4route += CRUD4routeTemplate.Replace("{0}", table_name.ToLower()).Replace("{1}", table_name);
            }


            ///COMMON FILES
            //router.js
            string CRUD4routeMain = CRUD4routeMainTemplate.Replace("{0}", firstTable).Replace("{1}", CRUD4route);
            File.WriteAllText(Path.Combine(srcDIR, "router.js"), CRUD4routeMain, outputEnc);

            string CRUD4appVueMain = CRUD4appVueMainTemplate.Replace("{0}", appVue).Replace("{1}", appVueMenu);
            File.WriteAllText(Path.Combine(srcDIR, "App.vue"), CRUD4appVueMain, outputEnc);
        }

        private string check4date(treeItem2fields f)
        {
            string fieldName = f.field_name;
            string fieldType = f.field_type;
            if (fieldType == "date")
                return string.Format("DATE_FORMAT({0},\\'%d-%m-%Y\\') as {0}", fieldName);
            else if (fieldType == "datetime")
                return string.Format("DATE_FORMAT({0},\\'%d-%m-%Y %H:%i\\') as {0}", fieldName);
            else
                return fieldName;
        }


        private string helper_template_vuetify_details_component_VUE3(DataTable refTables, treeItem2 tbl)
        {
            string table_name;

            string CRUD4detailsTemplate = DBManager.Properties.Resources.CRUD5details;
            string CRUD4detailsREFTablesPromise = DBManager.Properties.Resources.CRUD4detailsREFTablesPromise;
            string CRUD4detailsGetRefTable = DBManager.Properties.Resources.CRUD4detailsGetRefTable;
            string dataItemsTemplate = "{0}Items: [],\n";
            string importTemplate = "import {0} from \"../entities/{0}.js\";\n";
            string importElementsTemplate = "import {0} from \"@/elements/{0}\";\n";
            string promiseTemplate = "this.get{0}(),\n";


            //foreach (treeItem2 tbl in tables)
            //{
            string item = "";

            table_name = tbl.table_name.Capitalize();

            string pk = tbl.table_fields.Where(x => x.field_PK).Select(x => x.field_name).FirstOrDefault();
            if (pk == null)
                pk = "THEID";


            //generate markup
            object[] g = helper_template_vuetify_details_component4elements_VUE3(refTables, tbl.table_fields);
            //0 - markup // 1 - list of REFTABLES // 2 - list of FK key // 3 - bool vnumber import // 4 - bool vdate import // 5 - bool vdatatime import

            /////////////////////////////////// REFERENCE TABLES [ START ]                
            List<string> allREFtables = (List<string>)g[1];
            List<string> allREFtablesFK = (List<string>)g[2];

            string dataItems = "";
            string imports = "";
            string promises = "";
            string promisesLoad = "";
            string refTableRecordsLoad = "";
            int i = 0;
            if (allREFtables.Count > 0)
            {
                for (i = 0; i < allREFtables.Count; i++)
                {
                    dataItems += dataItemsTemplate.Replace("{0}", allREFtablesFK[i]);
                    imports += importTemplate.Replace("{0}", allREFtables[i]);
                    promises += promiseTemplate.Replace("{0}", allREFtables[i]);
                    promisesLoad += CRUD4detailsREFTablesPromise.Replace("{0}", allREFtablesFK[i]).Replace("{1}", allREFtables[i]).Replace("{2}", i.ToString());
                    refTableRecordsLoad += CRUD4detailsGetRefTable.Replace("{0}", allREFtables[i]).Replace("{1}", table_name);
                }
            }
            /////////////////////////////////// REFERENCE TABLES [ END ]

            List<string> componentsList = new List<string>();
            //if (g[3].ToBool())
            //{
            //    componentsList.Add("vnumber");
            //    imports += importElementsTemplate.Replace("{0}", "vnumber");
            //}

            //if (g[4].ToBool())
            //{
            //    componentsList.Add("vdatepickerex");
            //    imports += importElementsTemplate.Replace("{0}", "vdatepickerex");
            //}

            //if (g[5].ToBool())
            //{
            //    componentsList.Add("vdatetimepickerex");
            //    imports += importElementsTemplate.Replace("{0}", "vdatetimepickerex");
            //}

            item = CRUD4detailsTemplate.Replace("{0}", pk).Replace("{1}", (string)g[0]);
            item = item.Replace("{2}", imports).Replace("{3}", dataItems);
            item = item.Replace("{4}", promises).Replace("{5}", promisesLoad);
            item = item.Replace("{6}", i.ToString()).Replace("{7}", table_name);
            item = item.Replace("{8}", "import " + table_name + " from \"../entities/" + table_name + ".js\";").Replace("{9}", table_name);
            item = item.Replace("{10}", refTableRecordsLoad).Replace("{11}", string.Join(", ", componentsList));


            return item;
            //}

        }


        private string helper_template_vuetify_details_component_VUE2(DataTable refTables, treeItem2 tbl)
        {
            string table_name;

            string CRUD4detailsTemplate = DBManager.Properties.Resources.CRUD4details;
            string CRUD4detailsREFTablesPromise = DBManager.Properties.Resources.CRUD4detailsREFTablesPromise;
            string CRUD4detailsGetRefTable = DBManager.Properties.Resources.CRUD4detailsGetRefTable;
            string dataItemsTemplate = "{0}Items: [],\n";
            string importTemplate = "import {0} from \"@/entities/{0}\";\n";
            string importElementsTemplate = "import {0} from \"@/elements/{0}\";\n";
            string promiseTemplate = "this.get{0}(),\n";


            //foreach (treeItem2 tbl in tables)
            //{
            string item = "";

            table_name = tbl.table_name.Capitalize();

            string pk = tbl.table_fields.Where(x => x.field_PK).Select(x => x.field_name).FirstOrDefault();
            if (pk == null)
                pk = "THEID";


            //generate markup
            object[] g = helper_template_vuetify_details_component4elements_VUE2(refTables, tbl.table_fields);
            //0 - markup // 1 - list of REFTABLES // 2 - list of FK key // 3 - bool vnumber import // 4 - bool vdate import // 5 - bool vdatatime import

            /////////////////////////////////// REFERENCE TABLES [ START ]                
            List<string> allREFtables = (List<string>)g[1];
            List<string> allREFtablesFK = (List<string>)g[2];

            string dataItems = "";
            string imports = "";
            string promises = "";
            string promisesLoad = "";
            string refTableRecordsLoad = "";
            int i = 0;
            if (allREFtables.Count > 0)
            {
                for (i = 0; i < allREFtables.Count; i++)
                {
                    dataItems += dataItemsTemplate.Replace("{0}", allREFtablesFK[i]);
                    imports += importTemplate.Replace("{0}", allREFtables[i]);
                    promises += promiseTemplate.Replace("{0}", allREFtables[i]);
                    promisesLoad += CRUD4detailsREFTablesPromise.Replace("{0}", allREFtablesFK[i]).Replace("{1}", allREFtables[i]).Replace("{2}", i.ToString());
                    refTableRecordsLoad += CRUD4detailsGetRefTable.Replace("{0}", allREFtables[i]).Replace("{1}", table_name);
                }
            }
            /////////////////////////////////// REFERENCE TABLES [ END ]

            List<string> componentsList = new List<string>();
            if (g[3].ToBool())
            {
                componentsList.Add("vnumber");
                imports += importElementsTemplate.Replace("{0}", "vnumber");
            }

            if (g[4].ToBool())
            {
                componentsList.Add("vdatepickerex");
                imports += importElementsTemplate.Replace("{0}", "vdatepickerex");
            }

            if (g[5].ToBool())
            {
                componentsList.Add("vdatetimepickerex");
                imports += importElementsTemplate.Replace("{0}", "vdatetimepickerex");
            }

            item = CRUD4detailsTemplate.Replace("{0}", pk).Replace("{1}", (string)g[0]);
            item = item.Replace("{2}", imports).Replace("{3}", dataItems);
            item = item.Replace("{4}", promises).Replace("{5}", promisesLoad);
            item = item.Replace("{6}", i.ToString()).Replace("{7}", table_name);
            item = item.Replace("{8}", "import " + table_name + " from \"@/entities/" + table_name + "\";").Replace("{9}", table_name);
            item = item.Replace("{10}", refTableRecordsLoad).Replace("{11}", string.Join(", ", componentsList));


            return item;
            //}

        }


        private object[] helper_template_vuetify_details_component4elements_VUE3(DataTable refTables, List<treeItem2fields> fields)
        {
            string CRUD4detailsTextElement = DBManager.Properties.Resources.CRUD4detailsTextElement;
            //string TextElementREQ = "() => !!privateRecord.{0} || 'This field is required',";
            string TextElementREQ = "privateRecord.{0} && ";

            string CRUD4detailsAutoCompleteElement = DBManager.Properties.Resources.CRUD4detailsAutoCompleteElement;
            //string CRUD4detailsVNumberINT = DBManager.Properties.Resources.CRUD4detailsVNumberINT;
            //string CRUD4detailsVNumberDEC = DBManager.Properties.Resources.CRUD4detailsVNumberDEC;
            string CRUD4detailsSwitchElement = DBManager.Properties.Resources.CRUD5detailsSwitchElement;
            //string CRUD4detailsVDateElement = DBManager.Properties.Resources.CRUD4detailsVDateElement;
            //string CRUD4detailsVDateTimeElement = DBManager.Properties.Resources.CRUD4detailsVDateTimeElement;
            string all = "";

            List<string> refTablesImport = new List<string>();
            List<string> refFKfield = new List<string>();
            bool vnumberImport = false;
            bool vdateImport = false;
            bool vdatetimeImport = false;
            foreach (treeItem2fields field in fields)
            {
                if (field.field_PK)
                    continue;

                all += "            <v-col cols=\"12\" sm=\"6\" md=\"4\">\n";
                string item = "";
                string refTableID = "";
                string refTableTXT = "";
                string refTable = "";

                switch (field.field_type)
                {
                    case "int":
                    case "smallint":
                    case "mediumint":
                    case "bigint":

                        bool tieExists = false;

                        /////////////////////////////////////////////////// FIND REFERENCE TABLE [start]
                        if (General.DB is MySQL)
                        {
                            if (refTables != null)
                            {
                                var matchFieldREF = refTables.AsEnumerable().Where(row => row.Field<string>("COLUMN_NAME").Equals(field.field_name)).FirstOrDefault();
                                if (matchFieldREF != null)
                                {
                                    refTable = matchFieldREF["REFERENCED_TABLE_NAME"].ToString();
                                    refTableID = matchFieldREF["REFERENCED_COLUMN_NAME"].ToString();
                                    refTableTXT = "title";
                                    tieExists = true;
                                }
                            }
                        }
                        else
                        { //other db systems
                            string[] returnVAL = check4tie(field.field_name);
                            //0 - ID // 1 - TXT // 2 - TABLE // 3 - DEST TABLE FIELDS COUNT
                            if (returnVAL[0] != null && returnVAL[1] != null && returnVAL[2] != null)
                            {
                                refTableID = returnVAL[0];
                                refTableTXT = returnVAL[1];
                                refTable = returnVAL[2];
                                tieExists = true;
                            }
                        }
                        /////////////////////////////////////////////////// FIND REFERENCE TABLE [end]


                        if (tieExists && !string.IsNullOrEmpty(refTableTXT) && !string.IsNullOrEmpty(refTableID))
                        {//create #v-autocomplete#
                            refTablesImport.Add(refTable.Capitalize());
                            refFKfield.Add(field.field_name);
                            /*
                             {0} - field
                             {1} - item text
                             {2} - item value
                             */
                            all += CRUD4detailsAutoCompleteElement.Replace("{0}", field.field_name).Replace("{1}", refTableTXT).Replace("{2}", refTableID);
                        }
                        else
                        {//otherwise vnumber
                            //vnumberImport = true;
                            //all += CRUD4detailsVNumberINT.Replace("{0}", field.field_name);
                            all += CRUD4detailsTextElement.Replace("{0}", field.field_name).Replace("{1}", 4.ToString());
                        }
                        break;
                    case "tinyint":
                        if (chkTINYINT.Checked)
                        { //as boolean
                            all += CRUD4detailsSwitchElement.Replace("{0}", field.field_name);
                        }
                        else
                        { //as int
                            //vnumberImport = true;
                            //all += CRUD4detailsVNumberINT.Replace("{0}", field.field_name);
                            all += CRUD4detailsTextElement.Replace("{0}", field.field_name).Replace("{1}", 3.ToString());
                        }
                        break;
                    case "bit":

                        all += CRUD4detailsSwitchElement.Replace("{0}", field.field_name);
                        break;
                    case "decimal":
                    case "numeric":
                    //all += CRUD4detailsVNumberDEC.Replace("{0}", field.field_name);

                    //break;
                    case "date":
                    //vdateImport = true;
                    //all += CRUD4detailsVDateElement.Replace("{0}", field.field_name).Replace("{1}", field.field_allow_null ? "false" : "true");
                    //break;
                    case "datetime":
                    //vdatetimeImport = true;
                    //all += CRUD4detailsVDateTimeElement.Replace("{0}", field.field_name).Replace("{1}", field.field_allow_null ? "false" : "true");
                    //break;
                    case "varchar":
                    case "text":
                    case "longtext":
                    case "mediumtext":
                    case "tinytext":
                    default:
                        /*
                        {0} - field name
                        {1} - length
                        {2} - required rule
                        {3} - required HTML5
                         */
                        item = CRUD4detailsTextElement;

                        //required
                        if (field.field_allow_null)
                            item = item.Replace("{2}", "").Replace("{3}", "");
                        else
                            item = item.Replace("{2}", TextElementREQ).Replace("{3}", "required");

                        if (string.IsNullOrEmpty(field.field_size)) //when TEXT
                            field.field_size = "255";

                        all += item.Replace("{0}", field.field_name).Replace("{1}", field.field_size);
                        break;
                }

                all += "            </v-col>\n";
            }

            return new object[] { all, refTablesImport, refFKfield, vnumberImport, vdateImport, vdatetimeImport };
        }

        private object[] helper_template_vuetify_details_component4elements_VUE2(DataTable refTables, List<treeItem2fields> fields)
        {
            string CRUD4detailsTextElement = DBManager.Properties.Resources.CRUD4detailsTextElement;
            //string TextElementREQ = "() => !!privateRecord.{0} || 'This field is required',";
            string TextElementREQ = "privateRecord.{0} && ";

            string CRUD4detailsAutoCompleteElement = DBManager.Properties.Resources.CRUD4detailsAutoCompleteElement;
            string CRUD4detailsVNumberINT = DBManager.Properties.Resources.CRUD4detailsVNumberINT;
            string CRUD4detailsVNumberDEC = DBManager.Properties.Resources.CRUD4detailsVNumberDEC;
            string CRUD4detailsSwitchElement = DBManager.Properties.Resources.CRUD4detailsSwitchElement;
            string CRUD4detailsVDateElement = DBManager.Properties.Resources.CRUD4detailsVDateElement;
            string CRUD4detailsVDateTimeElement = DBManager.Properties.Resources.CRUD4detailsVDateTimeElement;
            string all = "";

            List<string> refTablesImport = new List<string>();
            List<string> refFKfield = new List<string>();
            bool vnumberImport = false;
            bool vdateImport = false;
            bool vdatetimeImport = false;
            foreach (treeItem2fields field in fields)
            {
                if (field.field_PK)
                    continue;

                all += "            <v-col cols=\"12\" sm=\"6\" md=\"4\">\n";
                string item = "";
                string refTableID = "";
                string refTableTXT = "";
                string refTable = "";

                switch (field.field_type)
                {
                    case "int":
                    case "smallint":
                    case "mediumint":
                    case "bigint":

                        bool tieExists = false;

                        /////////////////////////////////////////////////// FIND REFERENCE TABLE [start]
                        if (General.DB is MySQL)
                        {
                            if (refTables != null)
                            {
                                var matchFieldREF = refTables.AsEnumerable().Where(row => row.Field<string>("COLUMN_NAME").Equals(field.field_name)).FirstOrDefault();
                                if (matchFieldREF != null)
                                {
                                    refTable = matchFieldREF["REFERENCED_TABLE_NAME"].ToString();
                                    refTableID = matchFieldREF["REFERENCED_COLUMN_NAME"].ToString();
                                    refTableTXT = "title";
                                    tieExists = true;
                                }
                            }
                        }
                        else
                        { //other db systems
                            string[] returnVAL = check4tie(field.field_name);
                            //0 - ID // 1 - TXT // 2 - TABLE // 3 - DEST TABLE FIELDS COUNT
                            if (returnVAL[0] != null && returnVAL[1] != null && returnVAL[2] != null)
                            {
                                refTableID = returnVAL[0];
                                refTableTXT = returnVAL[1];
                                refTable = returnVAL[2];
                                tieExists = true;
                            }
                        }
                        /////////////////////////////////////////////////// FIND REFERENCE TABLE [end]


                        if (tieExists && !string.IsNullOrEmpty(refTableTXT) && !string.IsNullOrEmpty(refTableID))
                        {//create #v-autocomplete#
                            refTablesImport.Add(refTable.Capitalize());
                            refFKfield.Add(field.field_name);
                            /*
                             {0} - field
                             {1} - item text
                             {2} - item value
                             */
                            all += CRUD4detailsAutoCompleteElement.Replace("{0}", field.field_name).Replace("{1}", refTableTXT).Replace("{2}", refTableID);
                        }
                        else
                        {//otherwise vnumber
                            vnumberImport = true;
                            all += CRUD4detailsVNumberINT.Replace("{0}", field.field_name);
                        }
                        break;
                    case "tinyint":
                        if (chkTINYINT.Checked)
                        { //as boolean
                            all += CRUD4detailsSwitchElement.Replace("{0}", field.field_name);
                        }
                        else
                        { //as int
                            vnumberImport = true;
                            all += CRUD4detailsVNumberINT.Replace("{0}", field.field_name);
                        }
                        break;
                    case "bit":

                        all += CRUD4detailsSwitchElement.Replace("{0}", field.field_name);
                        break;
                    case "decimal":
                    case "numeric":
                        all += CRUD4detailsVNumberDEC.Replace("{0}", field.field_name);

                        break;
                    case "date":
                        vdateImport = true;
                        all += CRUD4detailsVDateElement.Replace("{0}", field.field_name).Replace("{1}", field.field_allow_null ? "false" : "true");
                        break;
                    case "datetime":
                        vdatetimeImport = true;
                        all += CRUD4detailsVDateTimeElement.Replace("{0}", field.field_name).Replace("{1}", field.field_allow_null ? "false" : "true");
                        break;
                    case "varchar":
                    case "text":
                    case "longtext":
                    case "mediumtext":
                    case "tinytext":
                    default:
                        /*
                        {0} - field name
                        {1} - length
                        {2} - required rule
                        {3} - required HTML5
                         */
                        item = CRUD4detailsTextElement;

                        //required
                        if (field.field_allow_null)
                            item = item.Replace("{2}", "").Replace("{3}", "");
                        else
                            item = item.Replace("{2}", TextElementREQ).Replace("{3}", "required");

                        if (string.IsNullOrEmpty(field.field_size)) //when TEXT
                            field.field_size = "255";

                        all += item.Replace("{0}", field.field_name).Replace("{1}", field.field_size);
                        break;
                }

                all += "            </v-col>\n";
            }

            return new object[] { all, refTablesImport, refFKfield, vnumberImport, vdateImport, vdatetimeImport };
        }


        private string helper_template_vuetify_entityJS_VUE3(string tablename, List<treeItem2fields> fields)
        {
            // 1 - string
            // 2 - integer
            // 3 - boolean
            // 4 - float
            // 5 - date
            string CRUD4entityJSTemaplate = DBManager.Properties.Resources.CRUD5entityJS;

            string type = "";
            string defaultValue = "";
            string regulatorTemplate = "        this.{0} = general.TypeRegulator(data.{0}, {1});\n";
            string initTemplate = "        this.{0} = {1};\n";

            string regulator = "";
            string init = "";
            foreach (treeItem2fields field in fields)
            {
                switch (field.field_type)
                {
                    case "int":
                    case "smallint":
                    case "mediumint":
                    case "bigint":
                        type = "2";
                        defaultValue = "0";
                        break;
                    case "tinyint":
                        if (chkTINYINT.Checked)
                        {
                            type = "3";
                            defaultValue = "false";
                        }
                        else
                        {
                            type = "2";
                            defaultValue = "0";
                        }
                        break;
                    case "bit":
                        type = "3";
                        defaultValue = "false";
                        break;
                    case "decimal":
                    case "numeric":
                        type = "4";
                        defaultValue = "0";
                        break;
                    case "date":
                    case "datetime":
                    //type = "5";
                    //defaultValue = "''";
                    //break;
                    case "varchar":
                    case "text":
                    case "longtext":
                    case "mediumtext":
                    case "tinytext":
                    default:
                        type = "1";
                        defaultValue = "''";
                        break;
                }

                regulator += string.Format(regulatorTemplate, field.field_name, type);
                init += string.Format(initTemplate, field.field_name, defaultValue);
            }
            /*
            {0} - table
            {1} - regulator
            {2} - init
            */
            return CRUD4entityJSTemaplate.Replace("{0}", tablename).Replace("{1}", regulator).Replace("{2}", init);
        }


        private string helper_template_vuetify_entityJS_VUE2(string tablename, List<treeItem2fields> fields)
        {
            // 1 - string
            // 2 - integer
            // 3 - boolean
            // 4 - float
            // 5 - date
            string CRUD4entityJSTemaplate = DBManager.Properties.Resources.CRUD4entityJS;

            string type = "";
            string defaultValue = "";
            string regulatorTemplate = "        this.{0} = general.TypeRegulator(data.{0}, {1});\n";
            string initTemplate = "        this.{0} = {1};\n";

            string regulator = "";
            string init = "";
            foreach (treeItem2fields field in fields)
            {
                switch (field.field_type)
                {
                    case "int":
                    case "smallint":
                    case "mediumint":
                    case "bigint":
                        type = "2";
                        defaultValue = "0";
                        break;
                    case "tinyint":
                        if (chkTINYINT.Checked)
                        {
                            type = "3";
                            defaultValue = "false";
                        }
                        else
                        {
                            type = "2";
                            defaultValue = "0";
                        }
                        break;
                    case "bit":
                        type = "3";
                        defaultValue = "false";
                        break;
                    case "decimal":
                    case "numeric":
                        type = "4";
                        defaultValue = "0";
                        break;
                    case "date":
                    case "datetime":
                        type = "5";
                        defaultValue = "''";
                        break;
                    case "varchar":
                    case "text":
                    case "longtext":
                    case "mediumtext":
                    case "tinytext":
                    default:
                        type = "1";
                        defaultValue = "''";
                        break;
                }

                regulator += string.Format(regulatorTemplate, field.field_name, type);
                init += string.Format(initTemplate, field.field_name, defaultValue);
            }
            /*
            {0} - table
            {1} - regulator
            {2} - init
            */
            return CRUD4entityJSTemaplate.Replace("{0}", tablename).Replace("{1}", regulator).Replace("{2}", init);
        }

        private void do_template_bootstraptable5()
        {
            string table_name;
            string index_tabTemplate = DBManager.Properties.Resources.CRUD3template_index_tab;
            string index_content_tabTemplate = DBManager.Properties.Resources.CRUD3template_index_tab_content;
            string index_JSbTemplate = DBManager.Properties.Resources.CRUD3template_index_JS_gridload;
            string indexTemplate = DBManager.Properties.Resources.CRUD3template_index;
            string index_tabs = "";
            string index_contents = "";
            string index_JS = "";
            string index = "";

            string pageTemplate = DBManager.Properties.Resources.CRUD3template_page;
            string page = "";
            string page_JS_wo_SubTable = "	    $(\"#table{tbl}\").bootstrapTable();";
            string page_JS_w_SubTable = DBManager.Properties.Resources.CRUD3template_page_w_subtable;
            //string page_HTML_tbl_properties = ""; // data-toggle="table" data-detail-view="true"
            string page_table_trID = "				<th data-field=\"{colname}\" data-visible=\"false\">{colname}</th>\n";
            string page_table_trOther = "				<th data-field=\"{colname}\" data-sortable=\"true\">{colname}</th>\n";


            string refTable = "";
            string refTableField = "";
            string refFieldsCSV = "";
            string refFieldsBind = "";
            string refinsertSQLFields = "";
            string refinsertSQLFieldsTemplate = "        '{0}' => array('JSONprop{0}'),\n";
            //sub table

            string helperTemplate = DBManager.Properties.Resources.CRUD3template_helper;
            string helperTemplateGetRecordDetails = DBManager.Properties.Resources.CRUD3template_helper_GetRecordDetails;
            //string helperGetRecordDetails = "";
            string helperTemplateSwitchGetRecordDetails = DBManager.Properties.Resources.CRUD3template_helper_Switch_GetRecordDetails;
            string helperTemplaterefAfterBaseRecordInsert = DBManager.Properties.Resources.CRUD3template_helper_AfterBaseRecordInsert;
            string helper = "";


            string appDIR = Path.Combine(Application.StartupPath, txtFolder.Text);
            string pagesDIR = Path.Combine(appDIR, "pages");
            Directory.CreateDirectory(pagesDIR);
            string helperDIR = Path.Combine(appDIR, "entities");
            Directory.CreateDirectory(helperDIR);




            bool isFirstTable = true;
            foreach (treeItem2 tbl in tables)
            {
                table_name = tbl.table_name.Capitalize();

                bool hasSubTable = false;
                refinsertSQLFields = "";

                //find table reference to current table - [CREATE SUB TABLE]
                if (General.DB is MySQL)
                {
                    string dummy;
                    DataTable r = General.DB.ExecuteSQL(string.Format(refTableMySQL, table_name), out dummy, out dummy);

                    if (r != null && r.Rows.Count > 0)
                    {
                        refTable = r.Rows[0][0].ToStrinX();
                        refTableField = r.Rows[0][1].ToStrinX();

                        if (!string.IsNullOrEmpty(refTable) && !string.IsNullOrEmpty(refTable))
                        {
                            hasSubTable = true;
                            DataTable refField = General.DB.ExecuteSQL(string.Format("SHOW COLUMNS FROM {0}", refTable), out dummy, out dummy);
                            var refFieldsCSVpre = refField.AsEnumerable().Select(x => x.Field<string>("Field")).ToList();
                            refFieldsCSV = string.Join(", ", refFieldsCSVpre.Skip(1));
                            refFieldsBind = string.Join(", :", refFieldsCSVpre.Skip(1));

                            foreach (var refCol in refFieldsCSVpre.Skip(1))
                            {
                                if (!refCol.Equals(refTableField))
                                    refinsertSQLFields += string.Format(refinsertSQLFieldsTemplate, refCol);
                            }

                            lst.Items.Add("At " + table_name + " found reference with table : " + refTable);
                            lst.Items.Add("ref field is : " + refTableField);
                            lst.Items.Add("~~Table referenced as Expanded~~");
                            lst.Items.Add("");
                        }
                    }
                }



                //index.php
                index_tabs += string.Format(index_tabTemplate, (index_tabs.Length > 0 ? "" : " active"), table_name);
                index_contents += string.Format(index_content_tabTemplate, (index_contents.Length > 0 ? "" : " show active"), table_name);
                index_JS += string.Format(index_JSbTemplate, table_name);


                string pageTableCOLS = "";
                string helperTableCOLScsv = "";
                string helperTableCOLSbind = "";
                string helperTableCOLSfillGrid = "";
                string helperTableCOLSvendorGetterInsertArr = "";
                foreach (treeItem2fields fields in tbl.table_fields)
                {
                    ////check for tie table
                    //string[] returnVAL = check4tie(fields.field_name);
                    ////0 - ID // 1 - TXT // 2 - TABLE // 3 - DEST TABLE FIELDS COUNT

                    //////////////////////////////page ENTITY accessory
                    if (fields.field_PK)
                        pageTableCOLS += page_table_trID.Replace("{colname}", fields.field_name);
                    else
                        pageTableCOLS += page_table_trOther.Replace("{colname}", fields.field_name);

                    /////////////////////////////helper accessory
                    helperTableCOLScsv += (helperTableCOLScsv.Length > 0 ? ", " : "") + fields.field_name;
                    helperTableCOLSbind += (helperTableCOLSbind.Length > 0 ? ", :" : ":") + fields.field_name;
                    helperTableCOLSfillGrid += string.Format("								'{0}',", fields.field_name) + "\n";

                    helperTableCOLSvendorGetterInsertArr += string.Format("        '{0}' => array('JSONArrName', '{0}'),", fields.field_name) + "\n";
                }

                //////////////////////////////////////////page ENTITY
                if (hasSubTable)
                    page = pageTemplate.Replace("{transformJS}", page_JS_w_SubTable).Replace("{tbl}", table_name);
                else
                    page = pageTemplate.Replace("{transformJS}", page_JS_wo_SubTable).Replace("{tbl}", table_name);


                //{tblprops}
                string tblprops = isFirstTable ? "		data-toggle=\"table\"" : "";
                if (hasSubTable)
                    tblprops += (tblprops.Length > 0 ? "\n" : "") + "		data-detail-view=\"true\"";

                //cols 
                page = page.Replace("{tblprops}", tblprops).Replace("{cols}", pageTableCOLS);

                File.WriteAllText(Path.Combine(pagesDIR, table_name + ".php"), page, outputEnc);


                //////////////////////////////////////////page HELPER
                if (hasSubTable)
                {
                    // helperGetRecordDetails = helperTemplateGetRecordDetails.Replace("{tblREF}", refTable);

                    helper = helperTemplate.Replace("{swGetRecordDetails}", helperTemplateSwitchGetRecordDetails);

                    helper = helper.Replace("{GetRecordDetails}", helperTemplateGetRecordDetails.Replace("{tbl}", table_name)
                        .Replace("{tblREF}", refTable)
                        .Replace("{pk}", refTableField));

                    helper = helper.Replace("{AfterBaseRecordInsert}",
                                            helperTemplaterefAfterBaseRecordInsert.Replace("{tblREF}", refTable)
                                            .Replace("{helperTableCOLScsv}", refFieldsCSV)
                                            .Replace("{helperTableCOLSbind}", refFieldsBind)
                                            .Replace("{vendorGetterBeforeInsertArr}", refinsertSQLFields)
                                            .Replace("{pk}", refTableField)
                                            .Replace("{tblref}", refTable)
                                            );

                    //helper = helper.Replace("{vendorGetterInsertArr}", helperTableCOLSvendorGetterInsertArr);
                    //helper = helper.Replace("{AfterBaseRecordInsert}", "");
                    helper = helper.Replace("{callback}", ", null, '{tbl}AfterBaseRecordInsert'");
                    helper = helper.Replace("{vendorGetterInsertArr}", helperTableCOLSvendorGetterInsertArr);
                    helper = helper.Replace("{tbl}", table_name);
                    helper = helper.Replace("{csv}", helperTableCOLScsv);
                    helper = helper.Replace("{bind}", helperTableCOLSbind);
                    helper = helper.Replace("{fillGrid}", helperTableCOLSfillGrid);
                }
                else
                {
                    helper = helperTemplate.Replace("{GetRecordDetails}", "")
                                            .Replace("{vendorGetterInsertArr}", helperTableCOLSvendorGetterInsertArr)
                                            .Replace("{AfterBaseRecordInsert}", "")
                                            .Replace("{swGetRecordDetails}", "");

                    //helper = helper.Replace("{vendorGetterInsertArr}", helperTableCOLSvendorGetterInsertArr);

                    helper = helper.Replace("{callback}", "");
                    helper = helper.Replace("{tbl}", table_name);
                    helper = helper.Replace("{csv}", helperTableCOLScsv);
                    helper = helper.Replace("{bind}", helperTableCOLSbind);
                    helper = helper.Replace("{fillGrid}", helperTableCOLSfillGrid);
                }

                File.WriteAllText(Path.Combine(helperDIR, table_name + "Helper.php"), helper, outputEnc);


                isFirstTable = false;
            }




            index = indexTemplate.Replace("{0}", index_tabs).Replace("{1}", index_contents).Replace("{2}", index_JS);


            File.WriteAllText(Path.Combine(appDIR, "index.php"), index, outputEnc);
        }

        private void do_template_bootstraptable()
        {
            string table_name;

            string top_item_template = DBManager.Properties.Resources.CRUD2template_main;
            string top_item = "";

            //table cols
            string CRUD2template_main_TABLE_Item_template = DBManager.Properties.Resources.CRUD2template_main_TABLE_Item;
            string CRUD2template_main_TABLE_Item = DBManager.Properties.Resources.CRUD2template_main_TABLE_Item;

            //pagination
            string CRUD2template_pagination_template = DBManager.Properties.Resources.CRUD2template_pagination;
            string CRUD2template_pagination = "";

            //fetch record for edit
            string CRUD2template_fetch_template = DBManager.Properties.Resources.CRUD2template_fetch;
            string CRUD2template_fetch = "";

            //delete record
            string CRUD2template_delete_template = DBManager.Properties.Resources.CRUD2template_delete;
            string CRUD2template_delete = "";

            string apppath = Application.StartupPath + "\\" + txtFolder.Text + "\\";

            string select_fields = "";
            string select_fields_wo_joins = "";
            string PK = "";

            //templates
            string PHPSelectDetailTemplateDB2FORM = "		$('[name=*field*]').val(data.*field*);";
            string PHPSelectDetailTemplateDB2FORMcheck = "		$('[name=*field*]').bootstrapSwitch('state',parseInt(data.*field*));";

            string jsonValidator_template = "				             *field* : { required : true },";
            string jsonValidator = "";

            string jsonValidator_message_template = "				            *field* : 'Required Field',";
            string jsonValidator_message = "";

            //classic control (aka input)
            string PHPSelectDetailTemplateCOL = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL;

            //PHP read recordset for combo
            string CRUDtemplate_FK_Control_FILL_template = DBManager.Properties.Resources.CRUDtemplate_FK_Control_FILL;
            string CRUDtemplate_FK_Control_FILL = "";

            //JS fill combo  from PHP variable
            string CRUDtemplate_FK_Control_FILL_COMBO_template = DBManager.Properties.Resources.CRUDtemplate_FK_Control_FILL_COMBO;
            string CRUDtemplate_FK_Control_FILL_COMBO = "";

            //switch initialization
            string CRUDtemplate_FK_Control_FILL_CHECKinit_template = "	$(\"[name='*field*']\").bootstrapSwitch();\r\n";
            string CRUDtemplate_FK_Control_FILL_CHECKinit = "";

            //DTP initialization
            string CRUDtemplate_FK_Control_FILL_DTPinit_template = DBManager.Properties.Resources.CRUDtemplate_Control_DTP_init;
            string CRUDtemplate_FK_Control_FILL_DTPinit = "";

            //DTPtime initialization
            string CRUDtemplate_Control_DTPtime_init_template = DBManager.Properties.Resources.CRUDtemplate_Control_DTPtime_init;
            string CRUDtemplate_Control_DTPtime_init = "";

            string CRUDtemplate_Control_DTPmalot_init_template = DBManager.Properties.Resources.CRUDtemplate_Control_DTPmalot;
            //string CRUDtemplate_Control_DTPmalot_init = "";



            //form-control templates
            string PHPSelectDetailTemplateCOL_COMBO = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL_COMBO;
            string PHPSelectDetailTemplateCOL_CHECK = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL_CHECK;
            string PHPSelectDetailTemplateCOL_DTP = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL_DTP;
            string PHPSelectDetailTemplateCOL_DTPtime = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL_DTPtime;
            string PHPSelectDetailTemplateCOL_DTPmalot = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL_DTPmalot;

            //php_save templates 
            string CRUDtemplate_SAVE_CHECK_validation_template = DBManager.Properties.Resources.CRUDtemplate_save_checkbox_validation;
            string CRUDtemplate_SAVE_CHECK_validation = "";

            string CRUDtemplate_SAVE_DTPTime_validation_template = DBManager.Properties.Resources.CRUDtemplate_save_dtptime_validation;
            string CRUDtemplate_SAVE_DTPTime_validation = "";

            string CRUDtemplate_SAVE_DTPmalot_validation_template = DBManager.Properties.Resources.CRUDtemplate_save_dtpmalot_validation;
            //string CRUDtemplate_SAVE_DTPmalot_validation = "";



            string col_names = "";

            //save vars
            string insertFields = "";
            string insertVAL = "";
            string updateVAL = "";
            string save_prepared = "";
            string dbase2form = "";
            string post_validation = "";
            string joins = "";
            string rows = "";
            string fieldnames = "";
            string first_field = "";

            foreach (treeItem2 tbl in tables)
            {
                table_name = tbl.table_name;

                joins = select_fields = select_fields_wo_joins = fieldnames = PK = dbase2form = rows = insertFields = insertVAL = updateVAL = post_validation = save_prepared =
     CRUDtemplate_FK_Control_FILL = CRUDtemplate_FK_Control_FILL_COMBO = CRUDtemplate_FK_Control_FILL_CHECKinit = CRUDtemplate_SAVE_CHECK_validation =
     CRUDtemplate_SAVE_CHECK_validation = CRUDtemplate_FK_Control_FILL_DTPinit = col_names = CRUD2template_main_TABLE_Item = CRUD2template_delete = CRUD2template_fetch =
     jsonValidator = CRUD2template_pagination = jsonValidator_message = first_field = CRUDtemplate_Control_DTPtime_init = CRUDtemplate_SAVE_DTPTime_validation = "";
                //CRUDtemplate_SAVE_DTPmalot_validation=CRUDtemplate_Control_DTPmalot_init

                foreach (treeItem2fields fields in tbl.table_fields)
                {
                    col_names += "'" + fields.field_name + "',\r\n";

                    if (fields.field_PK)
                    {
                        PK = fields.field_name;
                        select_fields += PK + ", ";
                        select_fields_wo_joins += PK + ", ";

                        //when is PK, turn the visibility OFF
                        CRUD2template_main_TABLE_Item += CRUD2template_main_TABLE_Item_template.Replace("*field*", fields.field_name).Replace("*fieldv*", "false");
                    }
                    else
                    {
                        //when is PK, turn the visibility ON
                        CRUD2template_main_TABLE_Item += CRUD2template_main_TABLE_Item_template.Replace("*field*", fields.field_name).Replace("data-visible=\"*fieldv*\"", "data-sortable=\"true\""); //.Replace("*fieldv*", "true");

                        insertFields += fields.field_name + ", ";
                        insertVAL += ":" + fields.field_name + ", ";

                        updateVAL += fields.field_name + "=:" + fields.field_name + ", ";

                        //details.php
                        if (fields.field_type.ToLower() == "bit" && chkBIT.Checked)
                        {


                            save_prepared += "$stmt->bindValue(':" + fields.field_name + "' , $" + fields.field_name + ", PDO::PARAM_INT);\r\n";
                            dbase2form += PHPSelectDetailTemplateDB2FORMcheck.Replace("*field*", fields.field_name) + "\r\n";
                        }
                        else if (fields.field_type.ToLower() == "tinyint" && chkTINYINT.Checked)
                        {
                            save_prepared += "$stmt->bindValue(':" + fields.field_name + "' , $" + fields.field_name + ", PDO::PARAM_INT);\r\n";
                            dbase2form += PHPSelectDetailTemplateDB2FORMcheck.Replace("*field*", fields.field_name) + "\r\n";
                        }
                        else
                        {
                            post_validation += "!isset($_POST['" + fields.field_name + "']) || ";

                            //on datetime make custom save_prepared
                            if ((chkDATETIME.Checked && fields.field_type.ToLower() == "datetime") || (chkDATEmalot.Checked && fields.field_type.ToLower() == "date"))
                                Console.WriteLine("");
                            else
                                save_prepared += "$stmt->bindValue(':" + fields.field_name + "' , $_POST['" + fields.field_name + "']);\r\n";

                            dbase2form += PHPSelectDetailTemplateDB2FORM.Replace("*field*", fields.field_name) + "\r\n";
                        }

                        string[] returnVAL = check4tie(fields.field_name);
                        //0 - ID // 1 - TXT // 2 - TABLE

                        if (returnVAL[0] != null && returnVAL[1] != null && returnVAL[2] != null)
                        {
                            //PHP - read values from original tables (aka FOREIGN)
                            CRUDtemplate_FK_Control_FILL += CRUDtemplate_FK_Control_FILL_template.Replace("#table#", returnVAL[2]).Replace("#txt#", returnVAL[1]);
                            //CRUDtemplate_FK_Control_FILL += CRUDtemplate_FK_Control_FILL.Replace("#txt#", returnVAL[1]);

                            //JS - store it to COMBO
                            CRUDtemplate_FK_Control_FILL_COMBO += CRUDtemplate_FK_Control_FILL_COMBO_template.Replace("#table#", returnVAL[2]).Replace("#TXT#", returnVAL[1]).Replace("#PK#", returnVAL[0]).Replace("#FK_NAME#", fields.field_name);

                            joins += "\r\n LEFT JOIN " + returnVAL[2] + " ON " + returnVAL[2] + "." + returnVAL[0] + " = " + table_name + "." + fields.field_name;

                            //add select element when foreign key found!
                            rows += PHPSelectDetailTemplateCOL_COMBO.Replace("*field*", fields.field_name) + "\r\n";

                            select_fields += returnVAL[2] + "." + returnVAL[1] + " as " + fields.field_name + ", ";
                            select_fields_wo_joins += fields.field_name + ", ";
                        }
                        else if (fields.field_type.ToLower() == "bit" && chkBIT.Checked)
                        {
                            rows += PHPSelectDetailTemplateCOL_CHECK.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += fields.field_name + ", ";
                            select_fields_wo_joins += fields.field_name + ", ";

                            CRUDtemplate_FK_Control_FILL_CHECKinit += CRUDtemplate_FK_Control_FILL_CHECKinit_template.Replace("*field*", fields.field_name);

                            CRUDtemplate_SAVE_CHECK_validation += CRUDtemplate_SAVE_CHECK_validation_template.Replace("*field*", fields.field_name).Replace("*0var*", "0");
                        }
                        else if (fields.field_type.ToLower() == "tinyint" && chkTINYINT.Checked)
                        {
                            rows += PHPSelectDetailTemplateCOL_CHECK.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += fields.field_name + ", ";
                            select_fields_wo_joins += fields.field_name + ", ";

                            CRUDtemplate_FK_Control_FILL_CHECKinit += CRUDtemplate_FK_Control_FILL_CHECKinit_template.Replace("*field*", fields.field_name);

                            CRUDtemplate_SAVE_CHECK_validation += CRUDtemplate_SAVE_CHECK_validation_template.Replace("*field*", fields.field_name).Replace("*0var*", "\"0\"");
                        }
                        else if (chkDATE.Checked && fields.field_type.ToLower() == "date")
                        {
                            rows += PHPSelectDetailTemplateCOL_DTP.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += fields.field_name + ", ";
                            select_fields_wo_joins += fields.field_name + ", ";

                            CRUDtemplate_FK_Control_FILL_DTPinit += CRUDtemplate_FK_Control_FILL_DTPinit_template.Replace("*field*", fields.field_name);
                        }
                        else if (chkDATEmalot.Checked && fields.field_type.ToLower() == "date")
                        {
                            rows += PHPSelectDetailTemplateCOL_DTPmalot.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += "DATE_FORMAT(" + fields.field_name + ",'%d-%m-%Y') as " + fields.field_name + ", ";
                            select_fields_wo_joins += "DATE_FORMAT(" + fields.field_name + ",'%d-%m-%Y') as " + fields.field_name + ", ";

                            CRUDtemplate_Control_DTPtime_init += CRUDtemplate_Control_DTPmalot_init_template.Replace("*field*", fields.field_name);

                            CRUDtemplate_SAVE_DTPTime_validation += CRUDtemplate_SAVE_DTPmalot_validation_template.Replace("*field*", fields.field_name);

                            save_prepared += "$stmt->bindValue(':" + fields.field_name + "' , $" + fields.field_name + ");\r\n";

                        }
                        else if (chkDATETIME.Checked && fields.field_type.ToLower() == "datetime")
                        {
                            rows += PHPSelectDetailTemplateCOL_DTPtime.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += "DATE_FORMAT(" + fields.field_name + ",'%d-%m-%Y %H:%i') as " + fields.field_name + ", ";
                            select_fields_wo_joins += "DATE_FORMAT(" + fields.field_name + ",'%d-%m-%Y %H:%i') as " + fields.field_name + ", ";

                            CRUDtemplate_Control_DTPtime_init += CRUDtemplate_Control_DTPtime_init_template.Replace("*field*", fields.field_name);

                            CRUDtemplate_SAVE_DTPTime_validation += CRUDtemplate_SAVE_DTPTime_validation_template.Replace("*field*", fields.field_name);

                            //if (chkDATETIME.Checked && fields.field_type.ToLower() == "datetime")
                            save_prepared += "$stmt->bindValue(':" + fields.field_name + "' , $" + fields.field_name + ");\r\n";
                        }
                        else
                        {
                            if (first_field.Length == 0)
                                first_field = fields.field_name;


                            rows += PHPSelectDetailTemplateCOL.Replace("*field*", fields.field_name).Replace("*fieldsize*", fields.field_size) + "\r\n";
                            select_fields += fields.field_name + ", ";
                            select_fields_wo_joins += fields.field_name + ", ";

                            jsonValidator += jsonValidator_template.Replace("*field*", fields.field_name) + "\r\n";
                            jsonValidator_message += jsonValidator_message_template.Replace("*field*", fields.field_name) + "\r\n";

                        }

                        //countDetail += 1;
                    }


                    //tab_customers.php
                    fieldnames += "	'" + fields.field_name + "',\r\n";



                }//table fields end

                //.php
                top_item = top_item_template.Replace("#utable#", table_name.ToUpper());
                top_item = top_item.Replace("#ltable#", table_name.ToLower());
                top_item = top_item.Replace("#PK#", PK.ToLower());
                top_item = top_item.Replace("#table_cols#", CRUD2template_main_TABLE_Item);
                top_item = top_item.Replace("#controls#", rows.Replace("<div class=\"col-xs-6 col-md-4\">", "").Replace("</div>\r\n			</div>", "</div>"));
                top_item = top_item.Replace("#first_field#", first_field);
                top_item = top_item.Replace("#dbase2form#", dbase2form);
                top_item = top_item.Replace("#jsonValidator_template#", jsonValidator);
                top_item = top_item.Replace("#jsonValidator_message#", jsonValidator_message);
                top_item = top_item.Replace("#FKs#", CRUDtemplate_FK_Control_FILL);
                top_item = top_item.Replace("#FKs_JS#", CRUDtemplate_FK_Control_FILL_COMBO + "\r\n" + CRUDtemplate_FK_Control_FILL_CHECKinit + CRUDtemplate_FK_Control_FILL_DTPinit + CRUDtemplate_Control_DTPtime_init);

                File.WriteAllText(apppath + "tab_" + table_name + ".php", top_item, outputEnc);


                //_pagination.php
                if (select_fields.Length > 2)
                {
                    select_fields = select_fields.Substring(0, select_fields.Length - 2);
                    select_fields_wo_joins = select_fields_wo_joins.Substring(0, select_fields_wo_joins.Length - 2);
                }

                CRUD2template_pagination = CRUD2template_pagination_template.Replace("#cols#", col_names);
                CRUD2template_pagination = CRUD2template_pagination.Replace("#select_cols#", select_fields);
                CRUD2template_pagination = CRUD2template_pagination.Replace("#table#", table_name);
                CRUD2template_pagination = CRUD2template_pagination.Replace("#joins#", joins);
                File.WriteAllText(apppath + "tab_" + table_name + "_pagination.php", CRUD2template_pagination, outputEnc);


                //_fetch.php 
                CRUD2template_fetch = CRUD2template_fetch_template.Replace("*table*", table_name).Replace("*PK*", PK).Replace("*select*", select_fields_wo_joins);//select_fields);
                File.WriteAllText(apppath + "tab_" + table_name + "_fetch.php", CRUD2template_fetch, outputEnc);

                //_delete.php
                CRUD2template_delete = CRUD2template_delete_template.Replace("*table*", table_name).Replace("*PK*", PK);
                File.WriteAllText(apppath + "tab_" + table_name + "_delete.php", CRUD2template_delete, outputEnc);

                //_save.php
                //tab_customers_details_save
                if (insertFields.Length > 0)
                {
                    insertFields = insertFields.Substring(0, insertFields.Length - 2);
                    insertVAL = insertVAL.Substring(0, insertVAL.Length - 2);
                    updateVAL = updateVAL.Substring(0, updateVAL.Length - 2);

                    if (post_validation.Length > 4)
                        post_validation = post_validation.Substring(0, post_validation.Length - 4);


                    string template = DBManager.Properties.Resources.CRUD2template_save;

                    template = template.Replace("#validation#", post_validation);
                    template = template.Replace("#updateWhere#", PK);
                    template = template.Replace("#tblname#", table_name);
                    template = template.Replace("#updateVAL#", updateVAL);
                    template = template.Replace("#insertFields#", insertFields);
                    template = template.Replace("#insertVAL#", insertVAL);
                    template = template.Replace("#stmt#", save_prepared);
                    template = template.Replace("#checkboxes#", CRUDtemplate_SAVE_CHECK_validation);
                    template = template.Replace("#dtptimes#", CRUDtemplate_SAVE_DTPTime_validation);

                    File.WriteAllText(apppath + "tab_" + table_name + "_save.php", template, outputEnc);
                }
                else
                    General.Mes("Error on " + table_name + "_save.php", MessageBoxIcon.Exclamation);

            }//tables loop

        }

        private void do_template_adminLTE()
        {
            string table_name;

            string top_item_template = DBManager.Properties.Resources.CRUDtemplate_top_item;
            string top_item = "";

            string table_template = DBManager.Properties.Resources.CRUDtemplate_table;
            string table_item = "";
            string fieldnames = "";
            string PK = "";

            string table_delete_template = DBManager.Properties.Resources.CRUDtemplate_delete;
            string table_delete = "";

            //DETAILS
            string table_detail_template = DBManager.Properties.Resources.CRUDtemplate_detail;
            string table_detail = "";
            string PHPSelectDetailTemplateCOL = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL;
            string PHPSelectDetailTemplateCOL_COMBO = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL_COMBO;
            string PHPSelectDetailTemplateCOL_CHECK = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL_CHECK;
            string PHPSelectDetailTemplateCOL_DTP = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL_DTP;
            string PHPSelectDetailTemplateCOL_DTPtime = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL_DTPtime;

            string CRUDtemplate_FK_Control_FILL_template = DBManager.Properties.Resources.CRUDtemplate_FK_Control_FILL;
            string CRUDtemplate_FK_Control_FILL = "";
            string CRUDtemplate_FK_Control_FILL_COMBO_template = DBManager.Properties.Resources.CRUDtemplate_FK_Control_FILL_COMBO;
            string CRUDtemplate_FK_Control_FILL_COMBO = "";
            string CRUDtemplate_FK_Control_FILL_CHECKinit_template = "	$(\"[name='*field*']\").bootstrapSwitch();\r\n";
            string CRUDtemplate_FK_Control_FILL_CHECKinit = "";
            string CRUDtemplate_FK_Control_FILL_DTPinit_template = DBManager.Properties.Resources.CRUDtemplate_Control_DTP_init;
            string CRUDtemplate_FK_Control_FILL_DTPinit = "";
            string CRUDtemplate_Control_DTPtime_init_template = DBManager.Properties.Resources.CRUDtemplate_Control_DTPtime_init;
            string CRUDtemplate_Control_DTPtime_init = "";
            string joins = "";

            string CRUDtemplate_SAVE_CHECK_validation_template = DBManager.Properties.Resources.CRUDtemplate_save_checkbox_validation;
            string CRUDtemplate_SAVE_CHECK_validation = "";

            string CRUDtemplate_SAVE_DTPTime_validation_template = DBManager.Properties.Resources.CRUDtemplate_save_dtptime_validation;
            string CRUDtemplate_SAVE_DTPTime_validation = "";

            string CRUDtemplate_Control_DTPmalot_init_template = DBManager.Properties.Resources.CRUDtemplate_Control_DTPmalot;
            string PHPSelectDetailTemplateCOL_DTPmalot = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL_DTPmalot;
            string CRUDtemplate_SAVE_DTPmalot_validation_template = DBManager.Properties.Resources.CRUDtemplate_save_dtpmalot_validation;

            string PHPSelectDetailTemplateDB2FORM = "		$('[name=*field*]').val(jArray[0][\"*field*\"]);";
            string PHPSelectDetailTemplateDB2FORMcheck = "		$('[name=*field*]').bootstrapSwitch('state',parseInt(jArray[0][\"*field*\"]));";
            string tmp8 = "";
            string rows = "";
            //int count = 0;
            int countDetail = 0;


            //save vars
            string insertFields = "";
            string insertVAL = "";
            string updateVAL = "";
            string bind = "";
            string post_validation = "";

            string apppath = Application.StartupPath + "\\" + txtFolder.Text + "\\";

            string select_fields = "";


            foreach (treeItem2 tbl in tables)
            {
                table_name = tbl.table_name;

                joins = select_fields = fieldnames = PK = table_item = tmp8 = rows = insertFields = insertVAL = updateVAL = post_validation = bind =
                     CRUDtemplate_FK_Control_FILL = CRUDtemplate_FK_Control_FILL_COMBO = CRUDtemplate_FK_Control_FILL_CHECKinit = CRUDtemplate_SAVE_CHECK_validation =
                     CRUDtemplate_SAVE_CHECK_validation = CRUDtemplate_FK_Control_FILL_DTPinit = CRUDtemplate_Control_DTPtime_init = CRUDtemplate_SAVE_DTPTime_validation = "";

                countDetail = 0;
                //table name - generic use


                //template_top.php
                top_item += top_item_template.Replace("#table#", table_name);

                foreach (treeItem2fields fields in tbl.table_fields)
                {
                    if (fields.field_PK)
                    {
                        PK = fields.field_name;
                        select_fields += PK + ", ";
                    }
                    else
                    {
                        insertFields += fields.field_name + ", ";
                        insertVAL += ":" + fields.field_name + ", ";

                        updateVAL += fields.field_name + "=:" + fields.field_name + ", ";



                        //details.php
                        if (fields.field_type.ToLower() == "bit" && chkBIT.Checked)
                        {
                            bind += "$stmt->bindValue(':" + fields.field_name + "' , $" + fields.field_name + ", PDO::PARAM_INT);\r\n";
                            tmp8 += PHPSelectDetailTemplateDB2FORMcheck.Replace("*field*", fields.field_name) + "\r\n";
                        }
                        else if (fields.field_type.ToLower() == "tinyint" && chkTINYINT.Checked)
                        {
                            bind += "$stmt->bindValue(':" + fields.field_name + "' , $" + fields.field_name + ", PDO::PARAM_INT);\r\n";
                            tmp8 += PHPSelectDetailTemplateDB2FORMcheck.Replace("*field*", fields.field_name) + "\r\n";
                        }
                        else
                        {
                            post_validation += "!isset($_POST['" + fields.field_name + "']) || ";

                            bind += "$stmt->bindValue(':" + fields.field_name + "' , $_POST['" + fields.field_name + "']);\r\n";
                            tmp8 += PHPSelectDetailTemplateDB2FORM.Replace("*field*", fields.field_name) + "\r\n";
                        }


                        if (countDetail % 3 == 0)
                        {
                            if (countDetail > 0)
                                rows += "		</div>\r\n\r\n";


                            rows += "		<div class=\"row\">\r\n";
                        }

                        string[] returnVAL = check4tie(fields.field_name);
                        //0 - ID // 1 - TXT // 2 - TABLE

                        if (returnVAL[0] != null && returnVAL[1] != null && returnVAL[2] != null)
                        {

                            //PHP - read values from original tables (aka FOREIGN)
                            CRUDtemplate_FK_Control_FILL += CRUDtemplate_FK_Control_FILL_template.Replace("#table#", returnVAL[2]).Replace("#txt#", returnVAL[1]);
                            //CRUDtemplate_FK_Control_FILL += CRUDtemplate_FK_Control_FILL.Replace("#txt#", returnVAL[1]);

                            //JS - store it to COMBO
                            CRUDtemplate_FK_Control_FILL_COMBO += CRUDtemplate_FK_Control_FILL_COMBO_template.Replace("#table#", returnVAL[2]).Replace("#TXT#", returnVAL[1]).Replace("#PK#", returnVAL[0]).Replace("#FK_NAME#", fields.field_name);

                            joins += "\r\n LEFT JOIN " + returnVAL[2] + " ON " + returnVAL[2] + "." + returnVAL[0] + " = " + table_name + "." + fields.field_name;

                            //add select element when foreign key found!
                            rows += PHPSelectDetailTemplateCOL_COMBO.Replace("*field*", fields.field_name) + "\r\n";

                            select_fields += returnVAL[2] + "." + returnVAL[1] + " as " + fields.field_name + ", ";
                        }
                        else if (fields.field_type.ToLower() == "bit" && chkBIT.Checked)
                        {
                            rows += PHPSelectDetailTemplateCOL_CHECK.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += fields.field_name + ", ";

                            CRUDtemplate_FK_Control_FILL_CHECKinit += CRUDtemplate_FK_Control_FILL_CHECKinit_template.Replace("*field*", fields.field_name);


                            CRUDtemplate_SAVE_CHECK_validation += CRUDtemplate_SAVE_CHECK_validation_template.Replace("*field*", fields.field_name).Replace("*0var*", "0");
                        }
                        else if (fields.field_type.ToLower() == "tinyint" && chkTINYINT.Checked)
                        {
                            rows += PHPSelectDetailTemplateCOL_CHECK.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += fields.field_name + ", ";

                            CRUDtemplate_FK_Control_FILL_CHECKinit += CRUDtemplate_FK_Control_FILL_CHECKinit_template.Replace("*field*", fields.field_name);

                            CRUDtemplate_SAVE_CHECK_validation += CRUDtemplate_SAVE_CHECK_validation_template.Replace("*field*", fields.field_name).Replace("*0var*", "\"0\"");
                        }
                        else if (chkDATE.Checked && fields.field_type.ToLower() == "date")
                        {
                            rows += PHPSelectDetailTemplateCOL_DTP.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += fields.field_name + ", ";

                            CRUDtemplate_FK_Control_FILL_DTPinit += CRUDtemplate_FK_Control_FILL_DTPinit_template.Replace("*field*", fields.field_name);
                        }
                        else if (chkDATEmalot.Checked && fields.field_type.ToLower() == "date")
                        {
                            rows += PHPSelectDetailTemplateCOL_DTPmalot.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += "DATE_FORMAT(" + fields.field_name + ",'%d-%m-%Y') as " + fields.field_name + ", ";

                            CRUDtemplate_Control_DTPtime_init += CRUDtemplate_Control_DTPmalot_init_template.Replace("*field*", fields.field_name);

                            CRUDtemplate_SAVE_DTPTime_validation += CRUDtemplate_SAVE_DTPmalot_validation_template.Replace("*field*", fields.field_name);



                        }
                        else if (chkDATETIME.Checked && fields.field_type.ToLower() == "datetime")
                        {
                            rows += PHPSelectDetailTemplateCOL_DTPtime.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += "DATE_FORMAT(" + fields.field_name + ",'%d-%m-%Y %H:%i') as " + fields.field_name + ", ";

                            CRUDtemplate_Control_DTPtime_init += CRUDtemplate_Control_DTPtime_init_template.Replace("*field*", fields.field_name);

                            CRUDtemplate_SAVE_DTPTime_validation += CRUDtemplate_SAVE_DTPTime_validation_template.Replace("*field*", fields.field_name);
                        }
                        else
                        {
                            rows += PHPSelectDetailTemplateCOL.Replace("*field*", fields.field_name) + "\r\n";
                            select_fields += fields.field_name + ", ";
                        }

                        countDetail += 1;
                    }

                    //tab_customers.php
                    fieldnames += "	'" + fields.field_name + "',\r\n";
                }//table fields end

                rows += "		</div>\r\n\r\n";

                //tab_customers.php
                table_item += table_template.Replace("#table#", table_name);
                table_item = table_item.Replace("#fields#", fieldnames);
                table_item = table_item.Replace("#PK#", PK);
                table_item = table_item.Replace("#joins#", joins);

                if (select_fields.Length > 2)
                {
                    select_fields = select_fields.Substring(0, select_fields.Length - 2);
                    table_item = table_item.Replace("#select#", select_fields);
                }
                else
                    General.Mes("Error on " + table_name + "tab_table.php", MessageBoxIcon.Exclamation);


                File.WriteAllText(apppath + "tab_" + table_name + ".php", table_item, outputEnc);

                //tab_customers_delete.php
                table_delete = table_delete_template.Replace("#table#", table_name);
                table_delete = table_delete.Replace("#PK#", PK);
                File.WriteAllText(apppath + "tab_" + table_name + "_delete.php", table_delete, outputEnc);

                //tab_customers_details_save
                if (insertFields.Length > 0)
                {
                    insertFields = insertFields.Substring(0, insertFields.Length - 2);
                    insertVAL = insertVAL.Substring(0, insertVAL.Length - 2);
                    updateVAL = updateVAL.Substring(0, updateVAL.Length - 2);
                    post_validation = post_validation.Substring(0, post_validation.Length - 4);


                    string template = DBManager.Properties.Resources.CRUDtemplate_save;

                    template = template.Replace("#validation#", post_validation);
                    template = template.Replace("#updateWhere#", PK);
                    template = template.Replace("#tblname#", table_name);
                    template = template.Replace("#updateVAL#", updateVAL);
                    template = template.Replace("#insertFields#", insertFields);
                    template = template.Replace("#insertVAL#", insertVAL);
                    template = template.Replace("#stmt#", bind);
                    template = template.Replace("#checkboxes#", CRUDtemplate_SAVE_CHECK_validation);
                    template = template.Replace("#dtptimes#", CRUDtemplate_SAVE_DTPTime_validation);

                    File.WriteAllText(apppath + "tab_" + table_name + "_details_save.php", template, outputEnc);
                }
                else
                    General.Mes("Error on " + table_name + "_save.php", MessageBoxIcon.Exclamation);


                //details.php
                table_detail = table_detail_template.Replace("#table#", table_name);
                table_detail = table_detail.Replace("#PK#", PK);
                table_detail = table_detail.Replace("#db2form#", tmp8);
                table_detail = table_detail.Replace("#rows#", rows);
                table_detail = table_detail.Replace("#FKs#", CRUDtemplate_FK_Control_FILL);
                table_detail = table_detail.Replace("#FKs_JS#", CRUDtemplate_FK_Control_FILL_COMBO + "\r\n" + CRUDtemplate_FK_Control_FILL_CHECKinit + CRUDtemplate_FK_Control_FILL_DTPinit + CRUDtemplate_Control_DTPtime_init);


                File.WriteAllText(apppath + "tab_" + table_name + "_details.php", table_detail, outputEnc);
            }//table loop

            //DONT WRITE UTF8 MARGIN TOP+DOWN appear! LOL
            File.WriteAllText(apppath + "template_top.php", DBManager.Properties.Resources.CRUDtemplate_top.Replace("#tables#", top_item));

            if (General.DB is SQLite)
                File.WriteAllText(apppath + "config.php", DBManager.Properties.Resources.CRUDtemplate_config_sqlite.Replace("#filename#", Path.GetFileName(General.Connections[General.activeConnection].filename)), outputEnc);
            else
                File.WriteAllText(apppath + "config.php", DBManager.Properties.Resources.CRUDtemplate_config_mysql, outputEnc);

        }

        private string[] check4tie(string field_name)
        {
            string[] returnVAL = { null, null, null, null };

            if (field_name.ToLower().EndsWith("_id"))
            {


                string table = field_name.Substring(0, field_name.Length - 3);

                returnVAL[2] = table;

                lst.Items.Add("Ends with _id : " + table);

                int res;
                res = tables.FindIndex(delegate(treeItem2 employee) { return employee.table_name.ToLower() == table.ToLower(); });

                if (table.ToLower().StartsWith("tak"))
                    Console.WriteLine("tak");

                if (res == -1)
                {
                    res = tables.FindIndex(delegate(treeItem2 employee) { return employee.table_name.ToLower() == table.ToLower() + "s"; });
                    if (res > -1)
                        returnVAL[2] += "s";

                    if (res == -1)
                    {
                        res = tables.FindIndex(delegate(treeItem2 employee) { return employee.table_name.ToLower() == table.ToLower() + "es"; });
                        if (res > -1)
                            returnVAL[2] += "es";
                    }

                    if (res == -1)
                    {
                        res = tables.FindIndex(delegate(treeItem2 employee) { return employee.table_name.ToLower() == table.ToLower() + "ies"; });
                        if (res > -1)
                            returnVAL[2] += "ies";
                    }

                    if (res == -1)
                    {
                        res = tables.FindIndex(delegate(treeItem2 employee) { return employee.table_name.ToLower() == table.ToLower().Substring(0, table.Length - 1) + "ies"; });

                        if (res > -1)
                            returnVAL[2] = table.ToLower().Substring(0, table.Length - 1) + "ies";
                    }

                }

                if (res > -1)
                {
                    lst.Items.Add("Found reference with table : " + table);

                    foreach (treeItem2fields i in tables[res].table_fields)
                    {

                        if (returnVAL[0] != null)
                        {
                            lst.Items.Add("TextField is : " + i.field_name);
                            returnVAL[1] = i.field_name;
                            returnVAL[3] = tables[res].table_fields.Count.ToString();
                            lst.Items.Add("~~Table referenced~~");
                            lst.Items.Add("");

                            break;
                        }

                        if (i.field_PK)
                        {
                            lst.Items.Add("PK is : " + i.field_name);
                            returnVAL[0] = i.field_name;
                        }


                    }
                }
                else
                {
                    lst.Items.Add("Cannot find reference table : " + table);
                    lst.Items.Add("");
                }

            }



            return returnVAL;

        }

        private void cmbTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTemplate.SelectedIndex == 0)
                img_template_preview.Image = DBManager.Properties.Resources.templateCRUDadminLTE;
            else if (cmbTemplate.SelectedIndex == 1)
                img_template_preview.Image = DBManager.Properties.Resources.templateCRUDbootstraptable;
            else if (cmbTemplate.SelectedIndex == 2)
                img_template_preview.Image = DBManager.Properties.Resources.templateCRUDbootstraptable5;
            else if (cmbTemplate.SelectedIndex == 3)
            {
                img_template_preview.Image = DBManager.Properties.Resources.templateCRUDvue2;
                chkDATE.Checked = chkDATEmalot.Checked = chkDATETIME.Checked = false;
            }
            else if (cmbTemplate.SelectedIndex == 4)
            {
                img_template_preview.Image = DBManager.Properties.Resources.templateCRUDvue3;
                chkDATE.Checked = chkDATEmalot.Checked = chkDATETIME.Checked = false;
            }

            chkDATE.Enabled = chkDATEmalot.Enabled = chkDATETIME.Enabled = (cmbTemplate.SelectedIndex != 3 && cmbTemplate.SelectedIndex != 4);
        }

        private void chkDATE_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDATE.Checked)
                chkDATEmalot.Checked = !chkDATE.Checked;
        }

        private void chkDATEmalot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDATEmalot.Checked)
                chkDATE.Checked = !chkDATEmalot.Checked;
        }



    }
}
