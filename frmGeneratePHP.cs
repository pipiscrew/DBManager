using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aga.Controls.Tree;
using System.IO;
using System.Reflection;

namespace DBManager
{
    public partial class frmGeneratePHP : BlueForm
    {

        public class TBLStructure
        {
            public string fieldName { get; set; }
            public string fieldType { get; set; }
            public string fieldSize { get; set; }
            public bool fieldPK { get; set; }
            public bool fieldFK { get; set; }
            public string tableFK { get; set; }

            public TBLStructure(string _fieldname, string _fieldType, string _fieldSize, bool _PK, bool _FK, string _TBL)
            {
                fieldName = _fieldname;
                fieldType = _fieldType;
                fieldSize = _fieldSize;
                fieldPK = _PK;
                fieldFK = _FK;
                tableFK = _TBL;

            }
        }

        TreeModel TRmodel = new TreeModel();

        public frmGeneratePHP(List<treeItem> items, string tablename)//TreeNodeAdv item)
        {
            InitializeComponent();

            Node table = (Node)new treeItem(tablename, "", "", false, false, "", 0, imageList1);

            foreach (treeItem item in items)
            {
                table.Nodes.Add(new treeItem(item.nodeText, item.fieldType, item.fieldSize, item.allowNull, item.nodeCheck, item.fieldTypeInternal, item.imageIndex, item.imageList));//, ImageList imgList
                    //item);

            }
            TRmodel.Nodes.Add(table);
            TR.Model = TRmodel;

            //Node table = null;
            //var ea = item.Tree.AllNodes.FirstOrDefault();
            //table = new Node();
            //table = (Node)(ea.Tag as treeItem);


            //foreach (var ea in item.Children)
            //{
            //    //table = new Node();   
            //    //table = (Node)(ea.Tag as treeItem);
            //    TRmodel.Nodes.Add((Node)(ea.Tag as treeItem));
            //}

            //TR.Model = TRmodel;

        }

        private void frmGeneratePHP_Load(object sender, EventArgs e)
        {
            //Node table = null;
            //var ea = iftem.Tree.AllNodes.FirstOrDefault();
            //table = new Node();
            //table = (Node)(ea.Tag as treeItem);

            //TRmodel.Nodes.Add(table);
            //TR.Model=TRmodel;

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


            List<TBLStructure> cols = new List<TBLStructure>();
            treeItem itemTR;

            //grab all fields to our list of
            foreach (var nodeR in TR.Root.Children)
                foreach (var node in nodeR.Children)
                {

                    itemTR = (node.Tag as treeItem);

                    cols.Add(new TBLStructure(itemTR.nodeText, itemTR.fieldType, itemTR.fieldSize, itemTR.imageIndex == 2, itemTR.imageIndex == 1, (nodeR.Tag as treeItem).nodeText));
                }

            //page_template***********
            string pageTemplate = ReadASCIIfromResources("DBManager.ResourcesPHPexport.page_template.txt");
            string pageTemplateAppender = "";


            //modal element
            string modalElementTemplate = ReadASCIIfromResources("DBManager.ResourcesPHPexport.page_template_modal_element.txt");
            string modalElementTemplateAppender = "";

            //modal elements Edit button get from DB to modal
            string modalElementEDITtemplate = "							$('[name={{field}}]').val(data.{{field}});\r\n";
            string modalElementEDITtemplateAppender = "";

            //table element
            string tableColTemplate = "					<th tabindex=\"0\" rowspan=\"1\" colspan=\"1\">{{field}}</th>\r\n";
            string tableColTemplateAppender = "";


            //validator Declaration***********
            string validateDeclareAppender = "";
            string validateErrMessagesTemplateAppender = "";

            //validator String Declare
            string validateDeclareStringTemplate = "				{{field}} : {\r\n					required : true,\r\n					minlength : 1,\r\n					maxlength : {{size}}\r\n				},\r\n";
            
            //validator String Error Messages
            string validateErrMessagesStringTemplate = "				{{field}} : 'Please enter a value up to {{size}} characters',\r\n";

            //validator Decimal Declare
            string validateDeclareDecimalTemplate = "				{{field}} : {\r\n					required : true,\r\n					currency : true\r\n				},\r\n";

            //validator Decimal Error Messages
            string validateErrMessagesDecimalTemplate = "				{{field}} : 'Please enter a decimal value',\r\n";


            //page_pagination template***********
            string pagePaginationTemplate = ReadASCIIfromResources("DBManager.ResourcesPHPexport.table_pagination.txt");
            string pagePaginationTemplateAppender = "";

            //page_pagination table item 
            string pagePaginationColTemplate = "							    	<td>{{{{field}}}}</td>\r\n";
            string pagePaginationColTemplateAppender = "";

            //page_pagination while
            string pagePaginationWhileTemplateOnlyFirstField = "	$rowTBL = str_replace('{{{{field}}}}', $row['{{field}}'], $RowTemplate);\r\n";
            string pagePaginationWhileTemplate = "	$rowTBL = str_replace('{{{{field}}}}', $row['{{field}}'], $rowTBL);\r\n";
            string pagePaginationWhileTemplateAppender = "";


            //table_save template***********
            string tableSaveTemplate = ReadASCIIfromResources("DBManager.ResourcesPHPexport.page_save_template.txt");
            string tableSaveTemplateAppender = "";
            
            string ID="";
            string secondField = "";
            string insFields = "";
            string qmarks="";
            string updFields="";
            string post2vars="";
            string paramFields = "";
            string paramFieldsType = "";


            //table_fetch template***********
            string tableFetchTemplate = ReadASCIIfromResources("DBManager.ResourcesPHPexport.page_fetch_template.txt");
            string tableFetchTemplateAppender = "";

            //table_delete template***********
            string tableDeleteTemplate = ReadASCIIfromResources("DBManager.ResourcesPHPexport.page_delete_template.txt");
            string tableDeleteTemplateAppender = "";

            //table_export template***********
            string tableExportTemplate = ReadASCIIfromResources("DBManager.ResourcesPHPexport.page_export_template.txt");
            string tableExportTemplateAppender = "";

            bool isFirst = true;
            foreach (TBLStructure item in cols)
            {
                if (item.fieldPK)
                    ID= item.fieldName;

                //for TABLE.php
                modalElementTemplateAppender += modalElementTemplate.Replace("{{field}}",item.fieldName);
                tableColTemplateAppender += tableColTemplate.Replace("{{field}}", item.fieldName);
                modalElementEDITtemplateAppender += modalElementEDITtemplate.Replace("{{field}}", item.fieldName);

                //for TABLE_pagination.php
                pagePaginationColTemplateAppender += pagePaginationColTemplate.Replace("{{field}}", item.fieldName);

                if (isFirst)
                {
                    pagePaginationWhileTemplateAppender += pagePaginationWhileTemplateOnlyFirstField.Replace("{{field}}", item.fieldName);
                    isFirst = false;
                }
                else
                {
                    pagePaginationWhileTemplateAppender += pagePaginationWhileTemplate.Replace("{{field}}", item.fieldName);
                    if (secondField.Length==0)
                        secondField = item.fieldName;
                }

                //table_save.php
                insFields+="                                " + item.fieldName + ",\r\n";
                qmarks += "?,";
                updFields += "                                " + item.fieldName + " = ?,\r\n";
                post2vars += "	$" + item.fieldName + " = $_POST['" + item.fieldName + "'];\r\n";
                paramFields += " $" + item.fieldName + ",";
                paramFieldsType += (item.fieldType == "decimal" ? "d" : "s");

                if (item.fieldType == "decimal")
                {
                    validateDeclareAppender += validateDeclareDecimalTemplate.Replace("{{field}}", item.fieldName);
                    validateErrMessagesTemplateAppender += validateErrMessagesDecimalTemplate.Replace("{{field}}", item.fieldName);
                }
                else
                {
                    validateDeclareAppender += validateDeclareStringTemplate.Replace("{{field}}", item.fieldName).Replace("{{size}}",item.fieldSize);
                    validateErrMessagesTemplateAppender += validateErrMessagesStringTemplate.Replace("{{field}}", item.fieldName).Replace("{{size}}", item.fieldSize);
                }

            }

            //replace validation delcaration
            pageTemplateAppender = pageTemplate.Replace("{{validationDECLARATION}}", validateDeclareAppender);

            //replace validation Error messages
            pageTemplateAppender = pageTemplateAppender.Replace("{{validationErrMessages}}", validateErrMessagesTemplateAppender);

            //replace EDIT elements (fill modal form with data from server)
            pageTemplateAppender = pageTemplateAppender.Replace("{{elementsEDIT}}", modalElementEDITtemplateAppender);

            //replace modal elements
            pageTemplateAppender = pageTemplateAppender.Replace("{{modalElements}}", modalElementTemplateAppender);

            //replace table cols
            pageTemplateAppender = pageTemplateAppender.Replace("{{tableCOLS}}", tableColTemplateAppender);

            var tblName = TR.Root.Children.FirstOrDefault();
            string tableName = (tblName.Tag as treeItem).nodeText;

            pageTemplateAppender = pageTemplateAppender.Replace("{{Ufield}}", tableName.ToUpper())
                    .Replace("{{Lfield}}", tableName.ToLower())
                    .Replace("{{Cfield}}",General.UppercaseFirst(tableName.ToLower().Substring(0,tableName.Length-1)));

            //table.php - export page
            File.WriteAllText(exportDIR + tableName.ToLower() + ".php", pageTemplateAppender, Encoding.ASCII);

            //table_pagination.php
            pagePaginationTemplateAppender = pagePaginationTemplate.Replace("{{table}}", tableName)
                                        .Replace("{{Ctable}}", General.UppercaseFirst(tableName.ToLower()))
                                        .Replace("{{tableCol}}", pagePaginationColTemplateAppender)
                                        .Replace("{{while}}", pagePaginationWhileTemplateAppender);

            File.WriteAllText(exportDIR + tableName.ToLower() + "_pagination.php", pagePaginationTemplateAppender, Encoding.ASCII);

            //table_save.php 
            tableSaveTemplateAppender = tableSaveTemplate.Replace("{{2ndfield}}", secondField)
                .Replace("{{Utable}}", tableName.ToUpper())
                .Replace("{{table}}", tableName)
                .Replace("{{insFields}}", insFields.Substring(0,insFields.Length -3))
                .Replace("{{qmarks}}", qmarks.Substring(0, qmarks.Length - 3))
                .Replace("{{updFields}}", updFields.Substring(0,updFields.Length-3) + " ")
                .Replace("{{id}}", ID)
                .Replace("{{post2vars}}", post2vars)
                .Replace("{{parameterFields}}", paramFields.Substring(0,paramFields.Length-1))
                .Replace("{{parameterFieldsType}}", paramFieldsType);

            File.WriteAllText(exportDIR + tableName.ToLower() + "_save.php", tableSaveTemplateAppender, Encoding.ASCII);

            //table_fetch.php
            tableFetchTemplateAppender = tableFetchTemplate.Replace("{{Utable}}", tableName.ToUpper())
                .Replace("{{table}}", tableName)
                 .Replace("{{id}}", ID);

            File.WriteAllText(exportDIR + tableName.ToLower() + "_fetch.php", tableFetchTemplateAppender, Encoding.ASCII);


            //table_delete.php
            tableDeleteTemplateAppender = tableDeleteTemplate.Replace("{{Utable}}", tableName.ToUpper())
                .Replace("{{table}}", tableName)
                 .Replace("{{id}}", ID);

            File.WriteAllText(exportDIR + tableName.ToLower() + "_delete.php", tableDeleteTemplateAppender, Encoding.ASCII);

            //table_export.php
            tableExportTemplateAppender = tableExportTemplate.Replace("{{Utable}}", tableName.ToUpper())
                .Replace("{{table}}", tableName);

            File.WriteAllText(exportDIR + tableName.ToLower() + "_export.php", tableExportTemplateAppender, Encoding.ASCII);

            //config.php - mysql
            File.WriteAllText(exportDIR + "config.php", ReadASCIIfromResources("DBManager.ResourcesPHPexport.config.txt"), Encoding.ASCII);

            //EXCEL export REQ files
            File.WriteAllText(exportDIR + "ExcelWriterXML.php", ReadASCIIfromResources("DBManager.ResourcesPHPexport.ExcelWriterXML.txt"), Encoding.ASCII);
            File.WriteAllText(exportDIR + "ExcelWriterXML_Sheet.php", ReadASCIIfromResources("DBManager.ResourcesPHPexport.ExcelWriterXML_Sheet.txt"), Encoding.ASCII);
            File.WriteAllText(exportDIR + "ExcelWriterXML_Style.php", ReadASCIIfromResources("DBManager.ResourcesPHPexport.ExcelWriterXML_Style.txt"), Encoding.ASCII);

            //login+authorization
            File.WriteAllText(exportDIR + "admin.php", ReadASCIIfromResources("DBManager.ResourcesPHPexport.auth_admin.txt"), Encoding.ASCII);
            File.WriteAllText(exportDIR + "login.php", ReadASCIIfromResources("DBManager.ResourcesPHPexport.auth_login.txt"), Encoding.ASCII);
            File.WriteAllText(exportDIR + "portal.php", ReadASCIIfromResources("DBManager.ResourcesPHPexport.portal_template.txt").Replace("{{Ltable}}",tableName.ToLower()), Encoding.ASCII);

            //bootstrap+jquery
            Directory.CreateDirectory(exportDIR + "js");
            Directory.CreateDirectory(exportDIR + "css");
            File.WriteAllText(exportDIR + "css\\bootstrap.min.css", ReadASCIIfromResources("DBManager.ResourcesPHPexport.bootstrap.min.css"), Encoding.ASCII);
            File.WriteAllText(exportDIR + "css\\signin.css", ReadASCIIfromResources("DBManager.ResourcesPHPexport.signin.css"), Encoding.ASCII);
            File.WriteAllText(exportDIR + "js\\bootstrap.min.js", ReadASCIIfromResources("DBManager.ResourcesPHPexport.bootstrap.min.js"), Encoding.ASCII);
            File.WriteAllText(exportDIR + "js\\jquery-1.10.2.min.js", ReadASCIIfromResources("DBManager.ResourcesPHPexport.jquery-1.10.2.min.js"), Encoding.ASCII);
            File.WriteAllText(exportDIR + "js\\jquery.validate.min.js", ReadASCIIfromResources("DBManager.ResourcesPHPexport.jquery.validate.min.js"), Encoding.ASCII);

        }

        private string ReadASCIIfromResources(string filename)
        {
            byte[] Buffer;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename))
            {
                Buffer = new byte[stream.Length];
                stream.Read(Buffer, 0, Buffer.Length);
            }

            return Encoding.ASCII.GetString(Buffer);
        }
    }
}
