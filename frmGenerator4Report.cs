using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace DBManager
{
    public partial class frmGenerator4Report : BlueForm
    {
        private string tblName;
        private string DBsourceName;
        private string TSQL;
        private List<string> reportParams;
        int ReadStartIndex = 0;

        public frmGenerator4Report()
        {
            InitializeComponent();
        }

        public frmGenerator4Report(string tableName, List<string> reportParams, string DBsourceName = null, string TSQL = null)
        {
            InitializeComponent();

            tblName = tableName;
            this.reportParams = reportParams;
            this.DBsourceName = DBsourceName;
            this.TSQL = TSQL;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void Generator_Load(object sender, EventArgs e)
        {
            txtFields.Lines = reportParams.ToArray();

            button3.PerformClick();
        }

        private void GenerateReportParameters()
        {
            short paramIndex = short.Parse(txtStartIndex.Text);

            string str = "";

            paramIndex += 1;

            for (int i = 0; i < reportParams.Count; i++)
            {
                str += "Item type=\"PerpetuumSoft.Reporting.DOM.Parameter\" id=\"" + paramIndex + "\" Name=\"" + reportParams[i] + "\" /&gt;&lt;";

                paramIndex += 1;
            }

            reportParameters.Text = str;

        }


        private void GenerateReportCTLS()
        {
            short paramIndex = short.Parse(txtStartIndexCTL.Text);

            string str = "";
            double topCTL = 0;
            for (int i = 0; i < reportParams.Count; i++)
            {
                if (DBsourceName == null)
                {
                    str += "&gt;&lt;Item type=\"PerpetuumSoft.Reporting.DOM.TextBox\" id=\"" + paramIndex + "\" Location=\"236.2204;" + topCTL.ToString().Replace(",", ".") + "\" Name=\"lbl" + reportParams[i] + "\" Text=\"" + reportParams[i] + " :\" Size=\"236.2204;59.0551\" TextAlign=\"MiddleLeft\"&gt;&lt;DataBindings type=\"PerpetuumSoft.Reporting.DOM.ReportDataBindingCollection\"";

                    paramIndex += 1;
                    str += " id=\"" + paramIndex + "\"&gt;&lt;Item type=\"PerpetuumSoft.Reporting.DOM.ReportDataBinding\" ";

                    paramIndex += 1;
                    str += "id=\"" + paramIndex + "\" Expression=\"GetParameter(&amp;quot;" + reportParams[i] + "&amp;quot;).ToString().Length &amp;gt; 0\" PropertyName=\"Visible\" /&gt;&lt;/DataBindings&gt;&lt;";

                    paramIndex += 1;
                    str += "Font type=\"PerpetuumSoft.Framework.Drawing.FontDescriptor\" id=\"" + paramIndex + "\" FamilyName=\"Segoe UI\" /&gt;&lt;/Item";

                    paramIndex += 1;
                }

                str += "&gt;&lt;Item type=\"PerpetuumSoft.Reporting.DOM.TextBox\" id=\"" + paramIndex + "\" Location=\"590.5512;" + topCTL.ToString().Replace(",", ".") + "\"";
                if (DBsourceName == null)
                    str += " Name=\"txt" + reportParams[i] + "\" Text=\"GetParameter(&amp;quot;" + reportParams[i] + "&amp;quot;)\" Size=\"649.6063;59.0551\"";
                else
                    str += " Name=\"txt" + reportParams[i] + "\" Text=\"GetData(&amp;quot;" + reportParams[i] + "&amp;quot;)\" Size=\"649.6063;59.0551\"";

                str += " TextAlign=\"MiddleLeft\"&gt;&lt;DataBindings type=\"PerpetuumSoft.Reporting.DOM.ReportDataBindingCollection\" ";

                paramIndex += 1;
                str += "id=\"" + paramIndex + "\"&gt;&lt;Item type=\"PerpetuumSoft.Reporting.DOM.ReportDataBinding\" ";

                paramIndex += 1;

                if (DBsourceName == null)
                    str += "id=\"" + paramIndex + "\" Expression=\"GetParameter(&amp;quot;" + reportParams[i] + "&amp;quot;)\" PropertyName=\"Value\" /&gt;&lt;/DataBindings&gt;&lt;";
                else
                    str += "id=\"" + paramIndex + "\" Expression=\"GetData(&amp;quot;" + DBsourceName + "." + reportParams[i] + "&amp;quot;)\" PropertyName=\"Value\" /&gt;&lt;/DataBindings&gt;&lt;";

                paramIndex += 1;
                str += "Font type=\"PerpetuumSoft.Framework.Drawing.FontDescriptor\" id=\"" + paramIndex + "\" FamilyName=\"Segoe UI\" /&gt;&lt;/Item";


                if (!chkOnTOP.Checked)
                    topCTL += 59.055;

                paramIndex += 1;
            }

            reportControls.Text = str;
            //'reportControls.Text = str.Substring(0,str.Length -15);
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Replace(txtSearch.Text, txtReplace.Text);
        }

        private void textBox3_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList[0].ToLower().EndsWith(".resx"))
            {
                textBox3.Text = FileList[0];
                ReadFindLastIndex();
            }
        }

        private void textBox3_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ReadFindLastIndex()
        {
            string preview = File.ReadAllText(textBox3.Text);


            string ReportName = "";
            Regex reg;
            MatchCollection match;

            reg = new Regex("<data name=\"(.*?).DocumentStream\"");
            match = reg.Matches(preview);

            if (match.Count > 1)
            {
                List<string> reportNames = new List<string>();
                foreach (Match item in match)
                    reportNames.Add(item.Groups[1].Value);

                frmGenerator4ReportChooser frmG = new frmGenerator4ReportChooser(reportNames);

                if (frmG.ShowDialog(out ReportName) != System.Windows.Forms.DialogResult.OK)
                {
                    textBox3.Text = "";
                    return;
                }
            }
            else
                ReportName = match[0].Groups[1].Value;


            button3.PerformClick(); //regenerate fields for fun, if user has made changes.

            textBox1.Text = textBox1.Text.Replace("yourInlineReportNameHere", ReportName);

            int ReadEndIndex = 0;

            ReadStartIndex = preview.IndexOf("<data name=\"" + ReportName + ".DocumentStream\"");

            if (ReadStartIndex == -1)
            {
                MessageBox.Show("Couldnt slice the report (find the start)!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ReadEndIndex = preview.IndexOf("</data>", ReadStartIndex);

            if (ReadEndIndex == -1)
            {
                MessageBox.Show("Couldnt slice the report (find the end)!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            preview = preview.Substring(ReadStartIndex, ReadEndIndex - ReadStartIndex);

            reg = new Regex("id=\"(.*?)\"");
            match = reg.Matches(preview);

            int maxID;

            List<int> sortINT = new List<int>();

            foreach (Match item in match)
            {
                sortINT.Add(int.Parse(item.Groups[1].Value));
            }

            sortINT.Sort();

            maxID = sortINT[sortINT.Count - 1] + 1;


            txtStartIndex.Text = maxID.ToString();

            txtStartIndexCTL.Text = (maxID + reportParams.Count).ToString();
            //}

            GenerateReportParameters();
            GenerateReportCTLS();

            if (reportParameters.Text.Length == 0)
                MessageBox.Show("No parameters exported!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            if (reportControls.Text.Length == 0)
                MessageBox.Show("No CTLS exported!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox3.Text + ".bak"))
            {
                MessageBox.Show(textBox3.Text + ".bak\r\n\r\nAlready exists!.", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (DBsourceName == null)
            {
                if (reportParameters.Text.Length == 0)
                {
                    MessageBox.Show("No parameters aborted!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }


            if (reportControls.Text.Length == 0)
            {
                MessageBox.Show("No CTLS aborted!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!File.Exists(textBox3.Text))
            {
                MessageBox.Show("Drop a filename!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string txt = File.ReadAllText(textBox3.Text);


            string export = "";
            int pos = -1;

            //PARAMATERS
            if (DBsourceName == null)
            {
                pos = txt.IndexOf("/Parameters", ReadStartIndex);

                if (pos == -1)
                {
                    MessageBox.Show("Please switch to your PRJ, add at least 1parameter!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                export = txt.Insert(pos, reportParameters.Text);
            }
            else
            {
                pos = 0;
                export = txt;
            }

            //CTLS
            pos = export.IndexOf("ReportControlCollection", ReadStartIndex);
            pos = export.IndexOf("&gt", pos);

            if (pos == -1)
            {
                MessageBox.Show("Please switch to your PRJ, add a template/add a detail!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            export = export.Insert(pos, reportControls.Text);

            File.Copy(textBox3.Text, textBox3.Text + ".bak");

            File.WriteAllText(textBox3.Text, export);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DBsourceName == null)
            {
                //string tbl = tr.SelectedNode.Text;
                string reportTXT = "//main form - print button\r\n" + "            Dictionary<string, object> " + tblName + "VARS = new Dictionary<string, object>();\r\n";

                reportParams = Regex.Split(txtFields.Text, "\r\n").ToList();
                //            string[] lines = Regex.Split(txtFields.Text, "\r\n");

                foreach (string line in reportParams)
                {
                    reportTXT += "            " + tblName + "VARS.Add(\"" + line + "\", " + tblName + "OBJ." + line + ");\r\n";
                }

                textBox1.Text = reportTXT + "\r\n            frmReport frmR = new frmReport(" + tblName + "VARS, null, \"yourInlineReportNameHere\");\r\n" +
                                "            frmR.ShowDialog();\r\n" +
                                "            frmR.Dispose();\r\n\r\n\r\n";
            }
            else
            {
                textBox1.Text = "//main form - print button\r\n" + "            Dictionary<string, object> Datasources = new Dictionary<string, object>();\r\n" +
                                "            Datasources.Add(\"" + DBsourceName + "\", General.Conne.GetDATATABLE(\"" + TSQL + "\"));\r\n" +
                                "\r\n" +
                                "            frmReport frmR = new frmReport(null, Datasources, \"yourInlineReportNameHere\");\r\n" +
                                "            frmR.ShowDialog();\r\n" +
                                "            frmR.Dispose();\r\n\r\n\r\n";
            }


        }

        private void chkOnTOP_CheckedChanged(object sender, EventArgs e)
        {
            GenerateReportParameters();
            GenerateReportCTLS();
        }


    }
}
