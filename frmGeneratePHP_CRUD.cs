using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DBManager.DBASES;
using Aga.Controls.Tree;
using System.Diagnostics;

namespace DBManager
{
    public partial class frmGeneratePHP_CRUD : BlueForm
    {
        Encoding outputEnc = new UTF8Encoding(false);


        List<treeItem2> tables = null;
        public frmGeneratePHP_CRUD(List<treeItem2> tables)
        {
            InitializeComponent();
            this.tables = tables;

            this.Text = "Tables : " + tables.Count.ToString();
            cmbTemplate.SelectedIndex = 0;
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
            else
                do_template_bootstraptable5();

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

                General.Mes(string.Format("Successfully extracted to '{0}'", f));
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message, MessageBoxIcon.Error);
            }
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

            //sub table
            string refTableMySQL = @"SELECT 
                                            TABLE_NAME,COLUMN_NAME,CONSTRAINT_NAME, REFERENCED_TABLE_NAME,REFERENCED_COLUMN_NAME
                                        FROM
                                            INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                                        WHERE
                                            REFERENCED_TABLE_NAME = '{0}'"; //https://stackoverflow.com/a/201678
            string refTable = "";
            string refTableField = "";
            string refFieldsCSV = "";
            string refFieldsBind = "";
            string refinsertSQLFields= "";
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
                                if (!refCol.Equals( refTableField))
                                    refinsertSQLFields+= string.Format(refinsertSQLFieldsTemplate, refCol);
	                        }

                            lst.Items.Add("At " + table_name + " found reference with table : " + refTable);
                            lst.Items.Add("ref field is : " + refTableField);
                            lst.Items.Add("~~Table referenced~~");
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
                else {
                    helper = helperTemplate.Replace("{GetRecordDetails}","")
                                            .Replace("{vendorGetterInsertArr}",helperTableCOLSvendorGetterInsertArr)
                                            .Replace("{AfterBaseRecordInsert}", "")
                                            .Replace("{swGetRecordDetails}","");

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
