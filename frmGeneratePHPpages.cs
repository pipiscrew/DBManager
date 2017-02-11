using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DBManager
{
    public partial class frmGeneratePHPpages : BlueForm
    {
        public frmGeneratePHPpages()
        {
            InitializeComponent();

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        string PHPSelectDetailTemplate = DBManager.Properties.Resources.PHPpagesSELECTdetail;
        string PHPSelectDetailTemplateDB2FORM = "		$('[name=*field*]').val(jArray[\"*field*\"]);";
        string PHPSelectDetailTemplateCOL = DBManager.Properties.Resources.PHPpagesSELECTdetailCOL;


        string PHPSelectPaginationTemplate = DBManager.Properties.Resources.PHPpagesSELECTpagination;
        string PHPSelectPaginationTemplateTableRow = "	$rowTBL = str_replace('{{*field*}}', $row[\"*field*\"], $rowTBL);";

        string PHPSelectTemplate = DBManager.Properties.Resources.PHPpagesSELECT;
        string PHPSelectTemplateSearchCombo = "			<option value=\"*field*\">*field*</option>";
        string PHPSelectTemplateTableColumn = "			<th tabindex=\"0\" rowspan=\"1\" colspan=\"1\">*field*</th>";
        

        private void txtSelect_TextChanged(object sender, EventArgs e)
        {
            refreshtextboxes();
        }

        private void buttonSELECT_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard(txtSelectResult.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard(txtSelectPagination.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard(txtSelectDetail.Text);
        }

        private void txtTable_TextChanged(object sender, EventArgs e)
        {
            refreshtextboxes();
        }

        private void refreshtextboxes()
        {
            if (txtTable.Text.Length == 0)
            {
                label3.Text = ">>>>>";
                return;
            }
            else
                label3.Text = "Table Name :";


            //txtTable_TextChanged
            txt_table.Text = "tab_" + txtTable.Text + ".php";
            txt_pg.Text = "tab_" + txtTable.Text + "_pagination.php";
            txt_detail.Text = "tab_" + txtTable.Text + "_details.php";
            txt_save.Text = "tab_" + txtTable.Text + "_details_save.php";
            txt_delete.Text = "tab_" + txtTable.Text + "_delete.php";

            string t = DBManager.Properties.Resources.PHPpagesPORTAL.Replace("#table#", txtTable.Text);


            txtPortalMerge.Text = t;



            if (txtSelect.Text.Length == 0)
            {
                label3.Text = "select fields?";
                return;
            }
            
            ////////txtSelect_TextChanged
            string tmp = PHPSelectTemplate;
            //replace table name
            tmp = tmp.Replace("**table**", txtTable.Text);

            string[] fls;
            fls = txtSelect.Text.Split(Convert.ToChar(","));

            string tmp2 = "";
            string tmp3 = "";

            //pagination
            string tmp5 = "";
            string tmp6 = "";
            int count = 0;
            string first_field = "";

            //DETAILS
            string tmp8 = "";
            string rows = "";
            int countDetail = 0;

            foreach (string line in fls)
            {
                if (line.Trim().Length == 0 && (line.Trim().Length == 1 && line.Trim() != ","))
                    continue;

                tmp2 += PHPSelectTemplateSearchCombo.Replace("*field*", line.Trim()) + "\r\n";
                tmp3 += PHPSelectTemplateTableColumn.Replace("*field*", line.Trim()) + "\r\n";


                //pagination <TD>
                if (count == 2) //isd=recID
                    tmp5 += "							    	<td data-name='{{isd}}'>{{" + line.Trim() + "}}</td>\r\n";
                else if (count == 0) //dont add td for ID!
                    first_field = line.Trim();
                else
                {
                    tmp5 += "							    	<td>{{" + line.Trim() + "}}</td>\r\n";
                    tmp6 += PHPSelectPaginationTemplateTableRow.Replace("*field*", line.Trim()) + "\r\n";
                }

                //DETAILS
                if (count > 0)
                    tmp8 += PHPSelectDetailTemplateDB2FORM.Replace("*field*", line.Trim()) + "\r\n";

                if (countDetail % 3 == 0)
                {
                    if (countDetail > 0)
                        rows += "		</div>\r\n\r\n";

                    if (count > 0)
                        rows += "		<div class=\"row\">\r\n";
                }

                if (count > 0)
                {
                    rows += PHPSelectDetailTemplateCOL.Replace("*field*", line.Trim()) + "\r\n";

                    countDetail += 1;
                }


                count += 1;

                //if (count > 0)
                //    countDetail += 1;
            }

            //always merge a close div
            rows += "		</div>\r\n\r\n";

            //replace fields for search+sort combo
            tmp = tmp.Replace("**search**", tmp2);
            tmp = tmp.Replace("**table_cols**", tmp3);

            txtSelectResult.Text = tmp;

            ////////////////////////////////////////////// PAGINATION
            string tmp4 = PHPSelectPaginationTemplate.Replace("**table**", txtTable.Text);
            tmp4 = tmp4.Replace("**query**", txtSelect.Text);
            tmp4 = tmp4.Replace("**tds**", tmp5);
            tmp4 = tmp4.Replace("**id**", first_field);
            tmp4 = tmp4.Replace("**table_row**", tmp6);

            txtSelectPagination.Text = tmp4;

            ////////////////////////////////////////////// DETAIL
            string tmp7 = PHPSelectDetailTemplate.Replace("**table**", txtTable.Text);
            tmp7 = tmp7.Replace("**id**", first_field);
            tmp7 = tmp7.Replace("**DB2FORM**", tmp8);
            tmp7 = tmp7.Replace("**rows**", rows);


            txtSelectDetail.Text = tmp7;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard(txtPortalMerge.Text);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInformation f = new frmInformation(DBManager.Properties.Resources.PHPpagesPORTALexample);
            f.ShowDialog();
                f.Dispose();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInformation f = new frmInformation(DBManager.Properties.Resources.PHPpagesPORTALprocedureSAVEexample);
            f.ShowDialog();
            f.Dispose();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInformation f = new frmInformation(DBManager.Properties.Resources.PHPpagesPORTALprocedureSAVE_callexample);
            f.ShowDialog();
            f.Dispose();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInformation f = new frmInformation(DBManager.Properties.Resources.PHPpagesPORTALprocedureDELETE_callexample);
            f.ShowDialog();
            f.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllBytes("c:\\PHPtemplate.zip", DBManager.Properties.Resources.PHPtemplate);
                General.Mes(@"Successfully extracted to 'c:\PHPtemplate.zip'");
            }
            catch (Exception ex) {
                General.Mes(ex.Message, MessageBoxIcon.Error);
            }
        }
    }
}
