using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using DBManager.DBASES;
using DBManager.StripRender;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using System.Data.OleDb;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace DBManager
{
    public partial class MainForm : BlueForm
    {
        public MainForm()
        {
            InitializeComponent();

            CustomProfessionalColors pct = new CustomProfessionalColors();
            pct.UseSystemColors = false;
            pct.color_3 = ControlPaint.Light(this.BackColor);
            pct.color_4 = this.BackColor;
            pct.color_5 = CustomProfessionalHelper.smethod_9(this.BackColor, ControlPaint.Dark(this.BackColor), 220);
            pct.color_11 = this.BackColor;
            pct.color_12 = this.BackColor;
            pct.color_15 = CustomProfessionalHelper.smethod_9(Color.White, pct.MenuItemSelectedGradientBegin, 150);
            pct.color_14 = CustomProfessionalHelper.smethod_9(pct.color_15, ControlPaint.Dark(pct.color_15), 150);
            pct.color_17 = pct.color_14;
            pct.color_13 = pct.color_3;
            pct.color_18 = pct.color_3;
            pct.color_19 = pct.color_4;
            pct.color_20 = pct.color_5;
            pct.color_21 = pct.color_11;
            pct.color_22 = pct.color_12;
            pct.color_24 = pct.color_14;
            pct.color_23 = pct.color_14;
            pct.color_2 = pct.color_5;
            pct.color_6 = pct.color_5;
            pct.color_7 = pct.color_4;
            pct.color_9 = this.BackColor;
            pct.color_8 = pct.color_5;
            pct.color_10 = pct.color_2;
            pct.color_0 = pct.color_2;
            pct.color_1 = pct.color_3;
            ToolStripManager.Renderer = new CustomProfessionalRenderer(pct);

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            //this.txtSQL.ActiveTextAreaControl.TextArea.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSQL_KeyUp);
            //this.txtSQL.ActiveTextAreaControl.TextArea.KeyPress += new KeyPressEventHandler(this.txtSQL_KeyPress);
        }

        private void Connect(bool saveConnectionsFile)
        {
            Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //options
            toolStripSQLServerGenerateScript4Restore.Visible = General.DB.optionsShowRestoreScript();
            toolStripGeneratePROCList.Visible = toolStripGeneratePROCListWhere.Visible =
            toolStripGeneratePROCInsert.Visible = toolStripGeneratePROCUpdate.Visible =
            toolStripGeneratePROCDelete.Visible = toolStripSeparator10.Visible = toolStripGeneratePROCMerge.Visible = General.DB.optionsProceduresFunctions();

            try
            {
                General.DB.AddMessage += new EventHandler<MyEventArgs>(DB_AddMessage);

                string err = General.DB.Connect();
                if (err.Length > 0)
                {
                    General.Mes(err, MessageBoxIcon.Exclamation); //IF ERROR occured in class
                    General.DB = null;
                    return;
                }


              

                TR.Model = General.DB.GetSchemaModel();

                if (General.DB != null)
                {
                    lst.Items.Clear();
                    ListViewItem[] procs = General.DB.GetProcedures();

                    if (procs != null)
                        lst.Items.AddRange(procs);

                    if (saveConnectionsFile)
                        General.SerializeList2File(); //save connections
                }
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);
            }
            finally { Cursor = System.Windows.Forms.Cursors.Default; }

        }

        void DB_AddMessage(object sender, MyEventArgs e)
        {
            lstv.Items.Insert(0, e.Message, e.isWarning ? 1 : 0).SubItems.Add("");
        }

        private void toolStripNewSQLSERVER_Click(object sender, EventArgs e)
        {
            frmSQLServerConnection frmS = new frmSQLServerConnection();
            DialogResult s = frmS.ShowDialog();
            frmS.Dispose();

            if (s == System.Windows.Forms.DialogResult.OK)
            {
                General.DB = new SQLServer(General.Connections.Count - 1, imageList1);

                Connect(true);
                //General.DB = new SQLServer(General.Connections.Count - 1, imageList1);

                //General.Mes(General.DB.Connect(), MessageBoxIcon.Exclamation); //IF ERROR occured in class

                //TR.Model = General.DB.GetSchemaModel();

                //General.SerializeList2File(); //save connections
            }

        }

        private void toolStripSQLITE_Click(object sender, EventArgs e)
        {
            frmSQLiteConnection frmS = new frmSQLiteConnection();
            DialogResult s = frmS.ShowDialog();
            frmS.Dispose();

            if (s == System.Windows.Forms.DialogResult.OK)
            {

                try
                {
                    General.DB = new SQLite(General.Connections.Count - 1, imageList1);

                    Connect(true);
                }
                catch (Exception ex)
                {
                    General.Mes(ex.Message, MessageBoxIcon.Exclamation);
                }

                //General.DB = new SQLite(General.Connections.Count - 1, imageList1);

                //General.Mes(General.DB.Connect(), MessageBoxIcon.Exclamation); //IF ERROR occured in class

                //TR.Model = General.DB.GetSchemaModel();

                //General.SerializeList2File(); //save connections
            }
        }

        private void toolStripMYSQL_Click(object sender, EventArgs e)
        {
            frmMySQLServerConnection frmS = new frmMySQLServerConnection();
            DialogResult s = frmS.ShowDialog();
            frmS.Dispose();

            if (s == System.Windows.Forms.DialogResult.OK)
            {
                General.DB = new MySQL(General.Connections.Count - 1, imageList1);

                Connect(true);

                //General.DB = new MySQL(General.Connections.Count - 1, imageList1);

                //General.Mes(General.DB.Connect(), MessageBoxIcon.Exclamation); //IF ERROR occured in class

                //TR.Model = General.DB.GetSchemaModel();

                //General.SerializeList2File(); //save connections
            }

        }


        private void toolStripMYSQLTunnel_Click(object sender, EventArgs e)
        {
            frmMySQLTunnelServerConnection frmS = new frmMySQLTunnelServerConnection();
            DialogResult s = frmS.ShowDialog();
            frmS.Dispose();

            if (s == System.Windows.Forms.DialogResult.OK)
            {
                General.DB = new MySQLTunnel(General.Connections.Count - 1, imageList1);

                Connect(true);

                //General.DB = new MySQL(General.Connections.Count - 1, imageList1);

                //General.Mes(General.DB.Connect(), MessageBoxIcon.Exclamation); //IF ERROR occured in class

                //TR.Model = General.DB.GetSchemaModel();

                //General.SerializeList2File(); //save connections
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = General.apTitle;

            General.DeSerializeFile2List();

            ToolStripMenuItem ts;
            int i = 0;

            foreach (var item in General.Connections)
            {
                ts = new ToolStripMenuItem();
                ts.Tag = i;

                if ((General.dbTypes)item.TYPE == General.dbTypes.SQLSERVER)
                {
                    ts.Text = item.serverName + " " + item.dbaseName;
                    toolStripNewSQLSERVER.DropDownItems.Add(ts);
                    ts.Click += new System.EventHandler(SQLSERVERConnect_Clicked);
                }
                else if ((General.dbTypes)item.TYPE == General.dbTypes.SQLite)
                {
                    ts.Text = item.filename;
                    toolStripSQLITE.DropDownItems.Add(ts);
                    ts.Click += new System.EventHandler(SQLiteConnect_Clicked);
                }
                else if ((General.dbTypes)item.TYPE == General.dbTypes.MySQL)
                {
                    ts.Text = item.serverName + " " + item.dbaseName;
                    toolStripMYSQL.DropDownItems.Add(ts);
                    ts.Click += new System.EventHandler(MySQLConnect_Clicked);
                }
                else if ((General.dbTypes)item.TYPE == General.dbTypes.MySQLtunnel)
                {
                    ts.Text = item.serverName + " " + item.dbaseName;
                    toolStripMYSQLTunnel.DropDownItems.Add(ts);
                    ts.Click += new System.EventHandler(MySQLTunnelConnect_Clicked);
                }
                else if ((General.dbTypes)item.TYPE == General.dbTypes.Access)
                {
                    ts.Text = item.filename;
                    toolStripMDB.DropDownItems.Add(ts);
                    ts.Click += new System.EventHandler(MDBConnect_Clicked);
                }
                else if ((General.dbTypes)item.TYPE == General.dbTypes.SQLSERVERtunnel)
                {
                    ts.Text = item.serverName + " " + item.dbaseName;
                    toolStripSQLServerTunnel.DropDownItems.Add(ts);
                    ts.Click += new System.EventHandler(SQLServerTunnelConnect_Clicked);
                }

                i += 1;
            }

            //HighlightingManager.Manager.AddSyntaxModeFileProvider(new AppSyntaxModeProvider());
            addTAB();

        }

        private void clearCTLS()
        {
            lst.Items.Clear();
            TR.Model = null;
            DG.DataSource = null;
            DG.Tag = null;
        }

        private void SQLSERVERConnect_Clicked(object sender, EventArgs e)
        {
            try
            {
                clearCTLS();

                try
                {
                    if (General.DB != null)
                    {
                        General.DB.Disconnect();
                        General.DB = null;

                    }
                }
                catch { }

                ToolStripMenuItem tmp = (sender as ToolStripMenuItem);
                General.activeConnection = (int)tmp.Tag;
                General.DB = new SQLServer((int)tmp.Tag, imageList1);

                Connect(false);

                //General.DB = new SQLServer((int)tmp.Tag, imageList1);

                //General.Mes(General.DB.Connect(), MessageBoxIcon.Exclamation); //IF ERROR occured in class

                //TR.Model = General.DB.GetSchemaModel();
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);
            }
        }

        private void MDBConnect_Clicked(object sender, EventArgs e)
        {
            try
            {
                clearCTLS();

                try
                {
                    if (General.DB != null)
                    {
                        General.DB.Disconnect();
                        General.DB = null;
                    }
                }
                catch { }


                ToolStripMenuItem tmp = (sender as ToolStripMenuItem);
                General.activeConnection = (int)tmp.Tag;
                General.DB = new ADOnet((int)tmp.Tag, imageList1);

                Connect(false);
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);
            }
        }

        private void toolStripMDB_Click(object sender, EventArgs e)
        {
            frmADOConnection frmS = new frmADOConnection();
            DialogResult s = frmS.ShowDialog();
            frmS.Dispose();

            if (s == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    General.DB = new ADOnet(General.Connections.Count - 1, imageList1);

                    Connect(true);
                }
                catch (Exception ex)
                {
                    General.Mes(ex.Message, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void SQLiteConnect_Clicked(object sender, EventArgs e)
        {
            try
            {
                clearCTLS();

                try
                {
                    if (General.DB != null)
                    {
                        General.DB.Disconnect();
                        General.DB = null;
                    }
                }
                catch { }


                ToolStripMenuItem tmp = (sender as ToolStripMenuItem);
                General.activeConnection = (int)tmp.Tag;
                General.DB = new SQLite((int)tmp.Tag, imageList1);

                Connect(false);


                //General.DB = new SQLite((int)tmp.Tag, imageList1);

                //General.Mes(General.DB.Connect(), MessageBoxIcon.Exclamation); //IF ERROR occured in class

                //TR.Model = General.DB.GetSchemaModel();
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);
            }
        }

        private void MySQLConnect_Clicked(object sender, EventArgs e)
        {
            try
            {
                clearCTLS();

                try
                {
                    if (General.DB != null)
                    {
                        General.DB.Disconnect();
                        General.DB = null;
                    }
                }
                catch { }


                ToolStripMenuItem tmp = (sender as ToolStripMenuItem);
                General.activeConnection = (int)tmp.Tag;
                General.DB = new MySQL((int)tmp.Tag, imageList1);

                Connect(false);


                ////General.DB = new MySQL((int)tmp.Tag, imageList1);

                ////General.Mes(General.DB.Connect(), MessageBoxIcon.Exclamation); //IF ERROR occured in class

                ////TR.Model = General.DB.GetSchemaModel();
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);
            }
        }



        private void MySQLTunnelConnect_Clicked(object sender, EventArgs e)
        {
            try
            {
                clearCTLS();

                try
                {
                    if (General.DB != null)
                    {
                        General.DB.Disconnect();
                        General.DB = null;
                    }
                }
                catch { }


                ToolStripMenuItem tmp = (sender as ToolStripMenuItem);
                General.activeConnection = (int)tmp.Tag;
                General.DB = new MySQLTunnel(General.activeConnection, imageList1);

                Connect(false);


                ////General.DB = new MySQL((int)tmp.Tag, imageList1);

                ////General.Mes(General.DB.Connect(), MessageBoxIcon.Exclamation); //IF ERROR occured in class

                ////TR.Model = General.DB.GetSchemaModel();
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);
            }
        }


        private void SQLServerTunnelConnect_Clicked(object sender, EventArgs e)
        {
            try
            {
                clearCTLS();

                try
                {
                    if (General.DB != null)
                    {
                        General.DB.Disconnect();
                        General.DB = null;
                    }
                }
                catch { }


                ToolStripMenuItem tmp = (sender as ToolStripMenuItem);
                General.activeConnection = (int)tmp.Tag;
                General.DB = new SQLServerTunnel(General.activeConnection, imageList1);

                Connect(false);


                ////General.DB = new MySQL((int)tmp.Tag, imageList1);

                ////General.Mes(General.DB.Connect(), MessageBoxIcon.Exclamation); //IF ERROR occured in class

                ////TR.Model = General.DB.GetSchemaModel();
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message);
            }
        }

        private void txtSQL_KeyUp(object sender, KeyEventArgs e)
        {
            var rtb = tabEdit.SelectedTab.Controls.Cast<Control>()
                                 .FirstOrDefault(x => x is TextEditorControl);

            ICSharpCode.TextEditor.TextArea snder = (sender as ICSharpCode.TextEditor.TextArea);
            if (e.KeyCode == Keys.F5)
            {
                if ((rtb as TextEditorControl).ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
                {
                    string selection = (rtb as TextEditorControl).ActiveTextAreaControl.SelectionManager.SelectedText;

                    runSQL(selection);
                }
                else
                    runSQL((rtb as TextEditorControl).Text);

                //ICSharpCode.TextEditor.TextArea snder = (sender as ICSharpCode.TextEditor.TextArea);
                //if (e.KeyCode == Keys.F5)
                //{
                //    if (snder.SelectionManager.HasSomethingSelected)
                //    {
                //        string selection = snder.SelectionManager.SelectedText;

                //        runSQL(selection);
                //    }
                //    else
                //        runSQL(snder.Text);

                //if (snder.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
                //{
                //    string selection = snder.ActiveTextAreaControl.
                //                                       SelectionManager.SelectedText;

                //    runSQL(selection);
                //}
                //else
                //    runSQL(txtSQL.Text);
            }
        }

        private void runSQL(string SQL)
        {
            if (General.DB == null)
                return;

            Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //store SQL for update 
            DG.Tag = SQL;

            //clear grid 
            DG.DataSource = null;


            string rowsAffected;
            string error;
            DataTable dT;
            dT = General.DB.ExecuteSQL(SQL, out rowsAffected, out error);
            //DataSet ds = new DataSet();
            //ds.Tables.Add(General.DB.ExecuteSQL(SQL, out rowsAffected, out error).Clone());

            if (error.Length > 0)
            {
                lstv.Items.Insert(0, error, 1).SubItems.Add(SQL);
                //General.Mes(error);
            }
            else
            {
                if (rowsAffected.Length > 0)
                    lstv.Items.Insert(0, "Executed successfully - Rows : " + rowsAffected, 0).SubItems.Add(SQL);
                else
                    lstv.Items.Insert(0, "Executed successfully" + rowsAffected, 0).SubItems.Add(SQL);

                DG.DataSource = dT;
            }

            if (DG.Columns.Count > 0)
            {
                for (int i = 0; i < DG.Columns.Count; i++)
                {
                    DG.Columns[i].HeaderText = DG.Columns[i].HeaderText + " (" + i + ")";
                }
            }

            Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void toolStripLSTVcopy_Click(object sender, EventArgs e)
        {
            if (lstv.SelectedItems.Count == 0)
                return;
            else
                General.Copy2Clipboard(lstv.SelectedItems[0].SubItems[1].Text);
        }

        private void TR_NodeMouseClick(object sender, TreeNodeAdvMouseEventArgs e)
        {
            if (e.Control == null)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (e.Node.Children.Count > 0)
                    contextTR.Show(System.Windows.Forms.Cursor.Position);
                else
                    General.Copy2Clipboard((TR.SelectedNode.Tag as treeItem).nodeText);
            }
            else
            {
                if (e.Control.ParentColumn == colName && e.Control.GetType() == typeof(NodeCheckBox))
                {
                    if (e.Node.Children.Count > 0)
                    {
                        foreach (var item in e.Node.Children)
                            (item.Tag as treeItem).nodeCheck = (e.Node.Tag as treeItem).nodeCheck;

                        TR.Refresh();
                    }
                }
            }

        }

        private void toolStripGenerateInsert_Click(object sender, EventArgs e)
        {
            string FieldName = "";
            string Fields = "";
            string vFields = "";
            string vFieldsSymbol = "";


            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).nodeCheck && (node.Tag as treeItem).imageIndex != 2)
                {
                    Fields += ((node.Tag as treeItem).nodeText) + ",";

                    vFieldsSymbol = DecideCharForField((node.Tag as treeItem).fieldTypeInternal.ToLower());

                    vFields += vFieldsSymbol + (char)(34) + " + " + ReplaceUnwanted(FieldName) + " + " + (char)(34) + vFieldsSymbol + ",";
                }
            }

            if ((Fields.Length == 0) || (vFields.Length == 0))
                return;


            Fields = Fields.Substring(0, Fields.Length - 1);
            vFields = vFields.Substring(0, vFields.Length - 1);

            string final = (char)(34) + "INSERT INTO [" + (TR.SelectedNode.Tag as treeItem).nodeText + "] (" + Fields + ") VALUES (" + vFields + ")" + (char)(34) + ";";

            //txtSQL.Text += "\r\n" + final;
            setCurrentTSQL(getCurrentTSQL() + "\r\n" + final);
            Scroll2END();
        }


        private void toolStripGenerateInsertParam_Click(object sender, EventArgs e)
        {
            string tmp = General.DB.GenerateParameterInsert(TR.SelectedNode);

            if (tmp != null)
            {
                setCurrentTSQL(getCurrentTSQL() + "\r\n" + tmp);

                //txtSQL.Text += "\r\n";
                //txtSQL.Text += tmp;
                Scroll2END();
            }
        }


        private void toolStripGenerateUpdate_Click(object sender, EventArgs e)
        {
            string FieldName = "";
            string Fields = "";
            string vFieldsSymbol = "";

            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).nodeCheck && (node.Tag as treeItem).imageIndex != 2)
                {
                    vFieldsSymbol = DecideCharForField((node.Tag as treeItem).fieldTypeInternal.ToLower());

                    FieldName = ((node.Tag as treeItem).nodeText);
                    Fields += (char)(34) + (FieldName) + " = " + vFieldsSymbol + (char)(34) + " + txt" + ReplaceUnwanted(FieldName) + ".Text + " + (char)(34) + vFieldsSymbol + "," + (char)(34) + " +\r\n";
                }
            }

            if (Fields.Length == 0)
                return;

            Fields = Fields.Substring(0, Fields.Length - (2 + " +\r\n".Length));

            if (Fields.EndsWith(@""""))
                Fields = Fields.Substring(0, Fields.Length - 4);
            else
                Fields += (char)(34);

            string final = (char)(34) + "UPDATE [" + (TR.SelectedNode.Tag as treeItem).nodeText + "]" + (char)(34) + " +\r\n" + (char)(34) + " SET " + (char)(34) + " +\r\n" + Fields + ";";

            setCurrentTSQL(getCurrentTSQL() + "\r\n" + final);
            //txtSQL.Text += "\r\n" + final;
        }

        private void toolStripGenerateUpdateParam_Click(object sender, EventArgs e)
        {


            string tmp = General.DB.GenerateParameterUpdate(TR.SelectedNode);

            if (tmp != null)
            {

                setCurrentTSQL(getCurrentTSQL() + "\r\n" + tmp);
                //txtSQL.Text += "\r\n";
                //txtSQL.Text += tmp;
                Scroll2END();
            }
        }

        private string ReplaceUnwanted(string input)
        {
            string tmp = input;
            tmp = tmp.Replace(" ", "").Replace("/", "").Replace("*", "").Replace("%", "").Replace("&", "").Replace("@", "").Replace("#", "");

            return tmp;
        }

        private string DecideCharForField(string input)
        {

            if (input == "")
                return "";

            switch (input)
            {
                case "string":
                    return "'";
                case "datetime":
                    return "'";
                default: //case "int" : case "decimal" : case "byte": case "boolean" :
                    return "";
            }
        }

        //private void Scroll2END()
        //{
        //    txtSQL.ActiveTextAreaControl.TextArea.SelectionManager.sel.SelectionStart = txtSQL.Text.Length;
        //    txtSQL.ScrollToCaret();
        //    txtSQL.Refresh();
        //}

        private void toolStripSelect100_Click(object sender, EventArgs e)
        {
            runSQL(General.DB.GenerateSelect100((TR.SelectedNode.Tag as treeItem).nodeText));
        }

        private void toolStripCopy2Clip_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard((TR.SelectedNode.Tag as treeItem).nodeText);
        }

        private void toolStripFormCodeFK_Click(object sender, EventArgs e)
        {
            string tmp;
            string firstField;

            if (TR.SelectedNode.Children.Count > 2)
                firstField = ((TR.SelectedNode.Children[2].Tag as treeItem).nodeText); //GetOnlyFieldName(tr.SelectedNode.Nodes[2].Text);
            else
            {
                MessageBox.Show("This option is for 3fields table REC_ID, PARENT_ID, TXTVAL\r\n\r\nYou will have to change the 2 FillList calls.", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                firstField = ((TR.SelectedNode.Children[1].Tag as treeItem).nodeText); // GetOnlyFieldName(tr.SelectedNode.Nodes[1].Text);
            }

            tmp = "//private void ClearForm\r\n            lst" + (TR.SelectedNode.Tag as treeItem).nodeText + ".DataSource = null;\r\n\r\n";
            tmp += "//private void GRID_SelectionChanged\r\n" +
                    "            if (GRID.SelectedRows.Count > 0)\r\n                FillListbox(\"" + (TR.SelectedNode.Tag as treeItem).nodeText + "\", \"" + firstField + "\", \"" + ((TR.SelectedNode.Children[1].Tag as treeItem).nodeText) + "\", PK_OBJ.REC_ID, lst" + (TR.SelectedNode.Tag as treeItem).nodeText + ");\r\n\r\n";

            tmp += "//private void btnAddButton_Click(object sender, EventArgs e)\r\n" +
                    "        {\r\n" +
                    "            if (PK_OBJ.REC_ID == 0)\r\n" +
                    "            {\r\n" +
                    "                MessageBox.Show(\"Παρακαλώ επιλέξτε ****!\", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);\r\n" +
                    "                return;\r\n" +
                    "            }\r\n" +
                    "\r\n" +
                    "            frmGenericSmall frmSP = new frmGenericSmall();\r\n" +
                    "            frmSP.Text = \" ****\";\r\n" +
                    "            frmSP.TableName = \"" + (TR.SelectedNode.Tag as treeItem).nodeText + "\";\r\n" +
                    "            frmSP.TableDataField = \"" + firstField + "\";\r\n" +
                    "            frmSP.LabelCaption = \"*** :\";\r\n" +
                    "            frmSP.DeleteMessage = \"Θέλετε να διαγράψετε τον *** :\";\r\n" +
                    "            frmSP.TableDataFieldMaxSize = **;\r\n" +
                    "            frmSP.ValidationTable = \"\";\r\n" +
                    "            frmSP.ValidationConstraint = \"\";\r\n" +
                    "            frmSP.ParentFieldName = \"" + ((TR.SelectedNode.Children[1].Tag as treeItem).nodeText) + "\";\r\n" +
                    "            frmSP.ParentID = PK_OBJ.PK_REC_ID;\r\n" +
                    "\r\n" +
                    "            if (frmSP.ShowDialog() == System.Windows.Forms.DialogResult.OK)\r\n" +
                    "                FillListbox(\"" + (TR.SelectedNode.Tag as treeItem).nodeText + "\", \"" + firstField + "\", \"" + ((TR.SelectedNode.Children[1].Tag as treeItem).nodeText) + "\", PK_OBJ.REC_ID, lst" + (TR.SelectedNode.Tag as treeItem).nodeText + ");\r\n" +
                    "\r\n" +
                    "        }\r\n\r\n";

            tmp += "//if needed\r\n" + "        private void FillListbox(string tableName, string fieldData, string whereFK_Fieldname, int whereFK_FieldEqual, KryptonListBox lst)\r\n" +
"        {\r\n" +
"            try\r\n" +
"            {\r\n" +
"                lst.DataSource = null;\r\n" +
"                DataTable dT = General.Conne.GetDATATABLE(\"select rec_id,\" + fieldData + \" from \" + tableName + \" where \" + whereFK_Fieldname + \"= \" + whereFK_FieldEqual + \" order by \" + fieldData);\r\n" +
"\r\n" +
"                lst.DataSource = dT;\r\n" +
"                lst.DisplayMember = fieldData;\r\n" +
"                lst.ValueMember = \"rec_id\";\r\n" +
"            }\r\n" +
"            catch (Exception ex)\r\n" +
"            {\r\n" +
"                MessageBox.Show(ex.Message, lst.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);\r\n" +
"            }\r\n" +
"        }\r\n";


            frmGenerator frmY = new frmGenerator(tmp);
            frmY.ShowDialog();
            frmY.Dispose();
        }

        private void toolStripFormCodeFKcombo_Click(object sender, EventArgs e)
        {

            string tmp;
            string firstField;
            string idField;
            string TableName = (TR.SelectedNode.Tag as treeItem).nodeText;

            firstField = ((TR.SelectedNode.Children[1].Tag as treeItem).nodeText);


            idField = ((TR.SelectedNode.Children[0].Tag as treeItem).nodeText);

            tmp = "//private void Form_Load\r\n             FillCombo(\"" + TableName + "\", \"" + firstField + "\", txt" + TableName + ");\r\n\r\n";
            tmp += "//private void Class2Form\r\n            txt" + TableName + ".SelectedValue = mainOBJ." + TableName + ";\r\n\r\n";

            tmp += "//private void ClearForm\r\n            txt" + TableName + " = \"\";\r\n\r\n";


            tmp += "//private void Form2Class\r\n" +
                    "            if (txt" + TableName + ".SelectedValue == null) //is null when a new value entered\r\n" +
                    "            {\r\n" +
                    "                txt" + TableName + ".Text = txt" + TableName + ".Text.Trim();\r\n" +
                    "\r\n" +
                    "                if (txt" + TableName + ".Text.Length > 0)\r\n" +
                    "                {\r\n" +
                    "                    mainOBJ." + idField + " = int.Parse(General.Conne.ExecuteSQLScalar(\"insert into " + TableName + " (" + firstField + ") values ('\" + General.QuoteMod(txt" + TableName + ".Text) + \"');SELECT SCOPE_IDENTITY();\").ToString());\r\n" +
                    "                    FillCombo(\"" + TableName + "\", \"" + firstField + "\", txt" + TableName + ");\r\n" +
                    "                }\r\n" +
                    "                else\r\n" +
                    "                    mainOBJ." + TableName + " = 0;\r\n" +
                    "            }\r\n" +
                    "            else\r\n" +
                    "                mainOBJ." + TableName + " = (int)txt" + TableName + ".SelectedValue;\r\n";



            frmGenerator frmY = new frmGenerator(tmp);
            frmY.ShowDialog();
            frmY.Dispose();
        }

        private void toolStripFormCodeFKmultiple_Click(object sender, EventArgs e)
        {
            if (TR.SelectedNode.Children.Count < 3)
            {
                MessageBox.Show("Nah, not enough fields!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string tmp;
            string TableName = (TR.SelectedNode.Tag as treeItem).nodeText;

            tmp = "//private void ClearForm\r\n            lst" + TableName + ".DataSource = null;\r\n\r\n";
            tmp += "//private void GRID_SelectionChanged\r\n" +
                    "            if (GRID.SelectedRows.Count > 0)\r\n                FillListbox(\"" + TableName + "\", \"" + ((TR.SelectedNode.Children[2].Tag as treeItem).nodeText) + "\", lst" + TableName + ");\r\n\r\n";

            tmp += "//private void btnAddButton_Click(object sender, EventArgs e)\r\n" +
                    "        {\r\n" +
                    "            if (mainSOBJ.REC_ID == 0)\r\n" +
                    "            {\r\n" +
                    "                MessageBox.Show(\"Παρακαλώ επιλέξτε ****!\", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);\r\n" +
                    "                return;\r\n" +
                    "            }\r\n" +
                    "\r\n" +
                    "            frmGeneric frmSP = new frmGeneric();\r\n" +
                    "            frmSP.Text = \" ****\";\r\n" +
                    "            frmSP.TableName = \"" + TableName + "\";\r\n" +
                    "            frmSP.TableDataField = new List<string>() { \"field1\", \"field2\", \"field3\" };\r\n" +
                    "            frmSP.TableDataCTLType = new List<string>() { \"txt\", \"txt\", \"dtp\" };\r\n" +
                    "            frmSP.LabelCaption = new List<string>() { \"capt1 :\", \"capt2 :\", \"capt2 :\" };\r\n" +
                    "            frmSP.DeleteMessage = \"Θέλετε να διαγράψετε την ***** :\";\r\n" +
                    "            frmSP.TableDataFieldMaxSize = new List<Int16>() { 50, 50, 0 };\r\n" +
                    "            frmSP.ValidationTable = \"\";\r\n" +
                    "            frmSP.ValidationConstraint = \"\";\r\n" +
                    "            frmSP.ParentFieldName = \"PK_ID_FKfield\";\r\n" +
                    "            frmSP.ParentID = PK_SOBJ.PK_REC_ID;\r\n" +
                    "            frmSP.GridColumns = new Dictionary<string, int>() {\r\n" +
                    "	                                                            {\"colname1\",100},\r\n" +
                    "	                                                            {\"colname2\",120},\r\n" +
                    "	                                                            {\"colname3\",120},\r\n" +
                    "	                                                        };\r\n" +
                    "\r\n" +
                    "            if (frmSP.ShowDialog() == System.Windows.Forms.DialogResult.OK)\r\n" +
                    "                FillListbox(\"" + TableName + "\", \"" + ((TR.SelectedNode.Children[2].Tag as treeItem).nodeText) + "\", lst" + TableName + ");\r\n" +
                    "        }\r\n";

            frmGenerator frmY = new frmGenerator(tmp);
            frmY.ShowDialog();
            frmY.Dispose();
        }

        private void toolStripFormCodeReport_Click(object sender, EventArgs e)
        {
            List<string> reportParams = new List<string>();

            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).nodeCheck)
                    reportParams.Add(((node.Tag as treeItem).nodeText));
            }

            frmGenerator4Report frmY = new frmGenerator4Report((TR.SelectedNode.Tag as treeItem).nodeText, reportParams);
            frmY.ShowDialog();
            frmY.Dispose();
        }

        private void toolStripGenerateDesigner_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();

            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).nodeCheck)
                {
                    fields.Add(((node.Tag as treeItem).nodeText), ((node.Tag as treeItem).fieldSize));
                }
            }

            if (fields.Count > 0)
            {
                frmGenerateDesigner frmG = new frmGenerateDesigner(fields);
                frmG.ShowDialog();
                frmG.Dispose();
            }
        }

        private void toolStripFormCode_Click(object sender, EventArgs e)
        {
            string TableName = (TR.SelectedNode.Tag as treeItem).nodeText;

            string Class2FormTEMP = "        private void Class2Form" + TableName + "()\r\n";
            string Form2ClassTEMP = "        private void Form2Class" + TableName + "()\r\n";
            string ClearFormTEMP = "        private void ClearForm" + TableName + "()\r\n";



            string Class2FormVALS = "";
            string Form2ClassVALS = "";
            string ClearFormVALS = "";

            string fieldNM = "";
            string firstField = "";


            foreach (var node in TR.SelectedNode.Children)
            {
                fieldNM = ((node.Tag as treeItem).nodeText);

                if ((node.Tag as treeItem).nodeCheck)
                {
                    if (firstField.Length == 0)
                        firstField = fieldNM;

                    Class2FormVALS += "            txt" + fieldNM + ".Text = " + TableName + "OBJ." + fieldNM + ";\r\n";
                    ClearFormVALS += "            txt" + fieldNM + ".Text = \"\";\r\n";
                    Form2ClassVALS += "            " + TableName + "OBJ." + fieldNM + " = txt" + fieldNM + ".Text" + ";\r\n";
                }
            }

            if (Class2FormVALS.Length == 0)
                Class2FormVALS = "            for Class2Form/Form2Class/ClearForm you must check fields!\r\n";

            Class2FormTEMP += "        {\r\n" + Class2FormVALS + "//if we have numeric field we format with .ToString(\"N\");\r\n//if we have DateTime field just replace with .Value\r\n        }";
            ClearFormTEMP += "        {\r\n" + ClearFormVALS + "            //lstSponsors.DataSource = null; *for listbox/combo\r\n//we have second grid depend on this FillGridX(0);\r\n        }";
            Form2ClassTEMP += "        {\r\n//when we have parent " + TableName + "OBJ.tied_ID = parentOBJ.REC_ID;\r\n" + Form2ClassVALS + "//if we have DateTime field - DateTime.Parse(txt.Value.ToShortDateString())\r\n        }";


            frmGenerateFormCode frmB = new frmGenerateFormCode();
            frmB.Text = "Generate form code for table : " + TableName;
            frmB.tbl = TableName;
            frmB.Form2Class = "#region \" " + TableName + "  - FORM2CLASS/CLASS2FORM/CLEARFORM \"\r\n\r\n" + Class2FormTEMP + "\r\n\r\n" + ClearFormTEMP + "\r\n\r\n" + Form2ClassTEMP + "\r\n\r\n#endregion";
            frmB.firstField = firstField;
            frmB.ShowDialog();
            frmB.Dispose();
        }

        private void txtSQL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x1')
            {
                ((TextBox)sender).SelectAll();
                e.Handled = true;
            }
        }

        private void lstv_Resize(object sender, EventArgs e)
        {
            lstv.Columns[1].Width = lstv.Width - 325;
            lstv.Invalidate();
        }

        private void toolStripAbout_Click(object sender, EventArgs e)
        {
            About frmA = new About();
            frmA.ShowDialog();
            frmA.Dispose();
        }



        public void Scroll2END()
        {
            var rtb = tabEdit.SelectedTab.Controls.Cast<Control>()
                                        .FirstOrDefault(x => x is TextEditorControl);

            int offset = (rtb as TextEditorControl).Document.TextLength;
            //if (offset > this.txtSQL.Document.TextLength)
            //{
            //    return;
            //}
            int line = (rtb as TextEditorControl).Document.GetLineNumberForOffset(offset);
            (rtb as TextEditorControl).ActiveTextAreaControl.Caret.Position =
                        (rtb as TextEditorControl).Document.OffsetToPosition(offset);
            (rtb as TextEditorControl).ActiveTextAreaControl.ScrollTo(line);//.CenterViewOn(line, 0);
        }



        private void lstv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lstv.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            lstv.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void toolStripCountRows_Click(object sender, EventArgs e)
        {
            treeItem Nod;

            foreach (var item in TR.AllNodes)
            {
                Nod = (item.Tag as treeItem);


                if (Nod.imageIndex == 0)
                {
                    Nod.fieldType = "";

                    Nod.fieldSize = General.DB.ExecuteScalar("select count(*) from " + Nod.nodeText);
                }
            }

            TR.Refresh();
        }

        private void DG_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void toolStripCountRows2_Click(object sender, EventArgs e)
        {
            treeItem Nod;
            long newRowCount = 0;
            long oldRowCount = 0;
            foreach (var item in TR.AllNodes)
            {
                Nod = (item.Tag as treeItem);

                if (Nod.imageIndex == 0)
                {
                    if (Nod.fieldSize.Length == 0)
                        return;

                    newRowCount = long.Parse(General.DB.ExecuteScalar("select count(*) from " + Nod.nodeText));
                    oldRowCount = long.Parse(Nod.fieldSize);

                    if (newRowCount != oldRowCount)
                        Nod.fieldType = (newRowCount - oldRowCount).ToString();
                    //else if  (newRowCount < oldRowCount )
                    //    Nod.fieldType = (oldRowCount - newRowCount).ToString();
                    else
                        Nod.fieldType = "";

                }
            }

            TR.Refresh();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\a3\graphics.mdb;Jet OLEDB:Database Password=");
            //objConn.Open();
            //OleDbDataReader cols = null;
            //DataTable dT_Fields;

            //OleDbDataReader sqlread = null;
            //OleDbCommand sqlco = new OleDbCommand();

            ////sqlco.Connection = objConn;
            ////sqlco.CommandText = "select top 1 * from [customers]";

            ////cols = sqlco.ExecuteReader();




            ////get TABLE FIELDS schema
            //DG.DataSource = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new Object[] { null, null, "CUSTOMERS" });
            //    //cols.GetSchemaTable();

            try
            {
                if (DG.Tag != null && General.DB != null)
                    lstv.Items.Insert(0, General.DB.UpdateGrid(DG), 0).SubItems.Add("");
            }
            catch (Exception ex)
            {
                General.Mes(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void DG_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                if (DG.CurrentCell.ValueType == typeof(System.Byte[]))
                {
                    string val = null;
                    frmEditByte frmTMP = new frmEditByte();
                    DialogResult s = frmTMP.ShowDialog(out val);
                    frmTMP.Dispose();

                    if (s == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!File.Exists(val))
                        {
                            MessageBox.Show("File doesnt exist!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        try
                        {
                            DG.CurrentCell.Value = System.IO.File.ReadAllBytes(val);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                    }
                    else if (s == System.Windows.Forms.DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                File.WriteAllBytes(saveFileDialog1.FileName, (byte[])DG.CurrentCell.Value);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }
                }
        }

        private void toolStripLSTVcopyStatus_Click(object sender, EventArgs e)
        {
            if (lstv.SelectedItems.Count == 0)
                return;
            else
                General.Copy2Clipboard(lstv.SelectedItems[0].Text);
        }

        private void toolStripCreateQuery4TSQL_Click(object sender, EventArgs e)
        {

        }

        private void lst_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lst.SelectedItems.Count == 0)
                return;

            TabPage mypage = new TabPage();

            TextEditorControl tmp = new TextEditorControl();
            tmp.Name = "txtSQL";
            tmp.Dock = DockStyle.Fill;

            //attach events
            tmp.ActiveTextAreaControl.TextArea.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSQL_KeyUp);
            tmp.ActiveTextAreaControl.TextArea.KeyPress += new KeyPressEventHandler(this.txtSQL_KeyPress);
            tmp.ShowVRuler = false;
            tmp.SetHighlighting("SQL");

            if (General.DB is MySQLTunnel)
                tmp.Text = General.DB.parseProcedure(lst.SelectedItems[0].Text, true);
            else
                tmp.Text = runFormatter(General.DB.parseProcedure(lst.SelectedItems[0].Text, true));

            //connection closed
            if (tmp.Text == null)
                return;

            mypage.Text = lst.SelectedItems[0].Text;

            mypage.Controls.Add(tmp);

            tabEdit.TabPages.Add(mypage);

            tabEdit.SelectedTab = mypage;


        }

        internal string runFormatter(String val)
        {
            if (val == null || val.Trim().Length == 0)
                return "";

            try
            {

                String tmpFile = Path.GetTempFileName();
                tmpFile = Path.GetDirectoryName(tmpFile) + "\\" + Path.GetFileNameWithoutExtension(tmpFile) + ".sql";
                String tmpFile2 = Path.GetTempFileName();
                File.WriteAllText(tmpFile, val);

                Process msbProcess = new Process();
                msbProcess.StartInfo.FileName = Application.StartupPath + "\\SqlFormatter.exe";
                msbProcess.StartInfo.Arguments = "\"" + tmpFile + "\" /o:\"" + tmpFile2 + "\"";
                //msbProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(Application.StartupPath);
                msbProcess.StartInfo.CreateNoWindow = true;
                msbProcess.StartInfo.RedirectStandardError = false;
                msbProcess.StartInfo.RedirectStandardOutput = true;
                msbProcess.StartInfo.UseShellExecute = false;
                msbProcess.Start();
                msbProcess.WaitForExit();

                string flOutput = File.ReadAllText(tmpFile2);

                if (flOutput.Length > 5)
                    return flOutput.Substring(0, flOutput.Length - 4);
                else
                    return val;
            }
            catch
            {
                return val;
            }

        }

        private String getCurrentTSQL()
        {
            var rtb = tabEdit.SelectedTab.Controls.Cast<Control>()
                                 .FirstOrDefault(x => x is TextEditorControl);

            return rtb.Text;
        }

        private void setCurrentTSQL(string val)
        {
            var rtb = tabEdit.SelectedTab.Controls.Cast<Control>()
                                          .FirstOrDefault(x => x is TextEditorControl);

            rtb.Text = val;
        }

        private void toolStripTABSformat_Click(object sender, EventArgs e)
        {
            setCurrentTSQL(runFormatter(getCurrentTSQL()));
        }

        private void toolStripTABSaddNew_Click(object sender, EventArgs e)
        {
            addTAB();
        }

        private void addTAB(string tabName = "Query")
        {
            TabPage mypage = new TabPage();

            TextEditorControl tmp = new TextEditorControl();
            tmp.Name = "txtSQL";
            tmp.Dock = DockStyle.Fill;
            mypage.Text = tabName;

            //attach events
            tmp.ActiveTextAreaControl.TextArea.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSQL_KeyUp);
            tmp.ActiveTextAreaControl.TextArea.KeyPress += new KeyPressEventHandler(this.txtSQL_KeyPress);
            tmp.ShowVRuler = false;
            tmp.SetHighlighting("SQL");

            mypage.Controls.Add(tmp);

            tabEdit.TabPages.Add(mypage);

            tabEdit.SelectedTab = mypage;
        }

        private void toolStripTABSclose_Click(object sender, EventArgs e)
        {
            if (tabEdit.TabPages.Count > 1)
            {
                var rtb = tabEdit.SelectedTab.Controls.Cast<Control>()
                                .FirstOrDefault(x => x is TextEditorControl);

                if ((rtb as TextEditorControl).Text.Length > 0)
                    if (General.Mes("You would like to close??", MessageBoxIcon.Information, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        return;

                (rtb as TextEditorControl).ActiveTextAreaControl.TextArea.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.txtSQL_KeyUp);
                (rtb as TextEditorControl).ActiveTextAreaControl.TextArea.KeyPress -= new KeyPressEventHandler(this.txtSQL_KeyPress);

                tabEdit.TabPages.Remove(tabEdit.SelectedTab);
            }
        }

        private void toolStripTABScloseOthers_Click(object sender, EventArgs e)
        {
            bool shift = false;

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                shift = true;

            if (tabEdit.TabPages.Count > 1)
            {
                TabPage sel = tabEdit.SelectedTab;

                foreach (TabPage tab in tabEdit.TabPages)
                {
                    if (tab != sel)
                    {
                        var rtb = tab.Controls.Cast<Control>()
                         .FirstOrDefault(x => x is TextEditorControl);

                        if (!shift)
                        {
                            if ((rtb as TextEditorControl).Text.Length > 0)
                            {
                                tabEdit.SelectedTab = tab;

                                if (General.Mes("You would like to close??", MessageBoxIcon.Information, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                                    continue;
                            }
                        }
                        (rtb as TextEditorControl).ActiveTextAreaControl.TextArea.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.txtSQL_KeyUp);
                        (rtb as TextEditorControl).ActiveTextAreaControl.TextArea.KeyPress -= new KeyPressEventHandler(this.txtSQL_KeyPress);

                        tabEdit.TabPages.Remove(tab);

                    }
                }

            }
        }

        private void tabEdit_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Rectangle rect = this.tabEdit.GetTabRect(this.tabEdit.SelectedIndex);
                Point pt = new Point(e.X, e.Y);
                if (rect.Contains(pt))
                    this.contextTABS.Show(this.tabEdit, pt);
                //else
                //    this.contextMakeReport4Query.Show(this.tabEdit, pt);
            }
        }


        private void toolStripREFRESH_Click(object sender, EventArgs e)
        {
            if (General.DB == null)
                return;
            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            TR.BeginUpdate();
            TR.Model = General.DB.GetSchemaModel();
            TR.EndUpdate();
            Cursor = System.Windows.Forms.Cursors.Default;
        }


        private void toolStripProcsREFRESH_Click(object sender, EventArgs e)
        {
            if (General.DB == null)
                return;

            ListViewItem[] procs = General.DB.GetProcedures();

            if (procs != null)
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                lst.BeginUpdate();
                lst.Items.Clear();
                lst.Items.AddRange(procs);
                lst.EndUpdate();
                Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void toolStripProcsCopyName_Click(object sender, EventArgs e)
        {
            if (lst.SelectedItems.Count == 0)
                return;
            else
                General.Copy2Clipboard(lst.SelectedItems[0].Text);
        }

        private void toolStripGeneratePROCList_Click(object sender, EventArgs e)
        {
            List<string> fields = new List<string>();
            string tblName = (TR.SelectedNode.Tag as treeItem).nodeText;

            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).nodeCheck && (node.Tag as treeItem).imageIndex != 2)
                    fields.Add((node.Tag as treeItem).nodeText);

            }

            string gen = General.DB.generatePROCselect(tblName, fields, "");

            addTAB("table [" + tblName + "] procedure SELECT");
            setCurrentTSQL(runFormatter(gen));
        }

        private void toolStripGeneratePROCListWhere_Click(object sender, EventArgs e)
        {
            List<string> fields = new List<string>();
            string tblName = (TR.SelectedNode.Tag as treeItem).nodeText;
            string PK = "";
            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).imageIndex == 2)
                    PK = (node.Tag as treeItem).nodeText.ToLower();
                else if ((node.Tag as treeItem).nodeCheck)
                    fields.Add((node.Tag as treeItem).nodeText);

            }

            string gen = General.DB.generatePROCselect(tblName, fields, PK);

            addTAB("table [" + tblName + "] procedure SELECT where");
            setCurrentTSQL(runFormatter(gen));
        }

        private void toolStripGeneratePROCInsert_Click(object sender, EventArgs e)
        {
            List<ListStrings> fields = new List<ListStrings>();
            string tblName = (TR.SelectedNode.Tag as treeItem).nodeText;

            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).nodeCheck)
                    if ((node.Tag as treeItem).fieldType.ToLower() == "varchar" || (node.Tag as treeItem).fieldType.ToLower() == "nvarchar")
                        fields.Add(new ListStrings((node.Tag as treeItem).nodeText, (node.Tag as treeItem).fieldType.ToLower() + "(" +
                           (node.Tag as treeItem).fieldSize + ")"));
                    else
                        fields.Add(new ListStrings((node.Tag as treeItem).nodeText, (node.Tag as treeItem).fieldType.ToLower()));

            }


            if (fields.Count == 0)
                General.Mes("Please check the fields");
            else
            {
                string gen = General.DB.generatePROCinsert(tblName, fields);

                addTAB("table [" + tblName + "] procedure INSERT");
                setCurrentTSQL(runFormatter(gen));
                //setCurrentTSQL(gen);
            }

        }

        private void toolStripGeneratePROCMerge_Click(object sender, EventArgs e)
        {
            List<ListStrings> fields = new List<ListStrings>();
            string tblName = (TR.SelectedNode.Tag as treeItem).nodeText;

            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).nodeCheck)
                    if ((node.Tag as treeItem).fieldType.ToLower() == "varchar" || (node.Tag as treeItem).fieldType.ToLower() == "nvarchar")
                        fields.Add(new ListStrings((node.Tag as treeItem).nodeText, (node.Tag as treeItem).fieldType.ToLower() + "(" +
                           (node.Tag as treeItem).fieldSize + ")"));
                    else
                        fields.Add(new ListStrings((node.Tag as treeItem).nodeText, (node.Tag as treeItem).fieldType.ToLower()));

            }


            if (fields.Count == 0)
                General.Mes("Please check the fields");
            else
            {
                string gen = General.DB.generatePROCMerge(tblName, fields);

                addTAB("table [" + tblName + "] MERGE");
                setCurrentTSQL(runFormatter(gen));
                //setCurrentTSQL(gen);
            }
        }

        private void toolStripGeneratePROCInsertWhere_Click(object sender, EventArgs e)
        {
            List<ListStrings> fields = new List<ListStrings>();
            string tblName = (TR.SelectedNode.Tag as treeItem).nodeText;
            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).nodeCheck)
                    if ((node.Tag as treeItem).fieldType.ToLower() == "varchar" || (node.Tag as treeItem).fieldType.ToLower() == "nvarchar")
                        fields.Add(new ListStrings((node.Tag as treeItem).nodeText, (node.Tag as treeItem).fieldType.ToLower() + "(" +
                           (node.Tag as treeItem).fieldSize + ")"));
                    else
                        fields.Add(new ListStrings((node.Tag as treeItem).nodeText, (node.Tag as treeItem).fieldType.ToLower()));

            }

            if (fields.Count == 0)
                General.Mes("Please check the fields");
            else
            {
                string gen = General.DB.generatePROCinsert(tblName, fields);

                addTAB("table [" + tblName + "] procedure INSERT");
                setCurrentTSQL(runFormatter(gen));
            }
        }



        private void toolStripGeneratePROCUpdate_Click(object sender, EventArgs e)
        {
            List<ListStrings> fields = new List<ListStrings>();
            string tblName = (TR.SelectedNode.Tag as treeItem).nodeText;
            string PK = "";
            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).imageIndex == 2)
                    PK = (node.Tag as treeItem).nodeText.ToLower();
                else if ((node.Tag as treeItem).nodeCheck)
                    if ((node.Tag as treeItem).fieldType.ToLower() == "varchar" || (node.Tag as treeItem).fieldType.ToLower() == "nvarchar")
                        fields.Add(new ListStrings((node.Tag as treeItem).nodeText, (node.Tag as treeItem).fieldType.ToLower() + "(" +
                           (node.Tag as treeItem).fieldSize + ")"));
                    else
                        fields.Add(new ListStrings((node.Tag as treeItem).nodeText, (node.Tag as treeItem).fieldType.ToLower()));

            }

            if (fields.Count == 0)
                General.Mes("Please check the fields");
            else
            {
                string gen = General.DB.generatePROCupdate(tblName, fields, PK);

                addTAB("table [" + tblName + "] procedure UPDATE");
                setCurrentTSQL(runFormatter(gen));
            }
        }


        private void toolStripGeneratePROCDelete_Click(object sender, EventArgs e)
        {
            string tblName = (TR.SelectedNode.Tag as treeItem).nodeText;
            string PK = "";

            foreach (var node in TR.SelectedNode.Children)
                if ((node.Tag as treeItem).imageIndex == 2)
                    PK = (node.Tag as treeItem).nodeText.ToLower();

            string gen = General.DB.generatePROCdelete(tblName, PK);
            addTAB("table [" + tblName + "] procedure DELETE");
            setCurrentTSQL(runFormatter(gen));
        }

        private void toolStripProcsGenerateNODE_Click(object sender, EventArgs e)
        {
            if (lst.SelectedItems.Count == 0 || General.DB == null)
                return;

            string gen = General.DB.generatePROCnodeJS(lst.SelectedItems[0].Text);
            addTAB(lst.SelectedItems[0].Text + " - nodeJS");
            setCurrentTSQL(gen);

        }

        private void toolStripSQLServerCopySQLServer_Click(object sender, EventArgs e)
        {
            if (General.DB == null)
                return;

            frmSQLcopyrows frmCopy = new frmSQLcopyrows();
            frmCopy.ShowDialog();
        }

        private void toolStripProcsGenerateFORM_Click(object sender, EventArgs e)
        {
            if (lst.SelectedItems.Count == 0 || General.DB == null)
                return;

            string formName = "";
            if (General.InputBox("PipisCrew", "Please define the form name :", ref formName) == System.Windows.Forms.DialogResult.OK)
            {
                string gen = General.DB.generateFORM(lst.SelectedItems[0].Text, formName);
                addTAB(lst.SelectedItems[0].Text + " - Form");
                setCurrentTSQL(gen);
            }

        }

        private void toolStripProcsRENAME_Click(object sender, EventArgs e)
        {
            if (lst.SelectedItems.Count == 0 || General.DB == null)
                return;

            string newName = "";
            if (General.InputBox("PipisCrew", "Please define the form name :", ref newName) == System.Windows.Forms.DialogResult.OK)
            {
                string gen = "DROP PROCEDURE [" + lst.SelectedItems[0].Text + "];\r\n\r\n";
                gen += runFormatter(General.DB.parseProcedure(lst.SelectedItems[0].Text, false).Replace(lst.SelectedItems[0].Text, newName));
                //                    General.DB.generateFORM(lst.SelectedItems[0].Text, formName);
                addTAB(lst.SelectedItems[0].Text + " - RENAME");
                setCurrentTSQL(gen);
            }
        }

        private void toolStripProcsGenerateFORMbootstrap_Click(object sender, EventArgs e)
        {
            if (lst.SelectedItems.Count == 0 || General.DB == null)
                return;

            string formName = "";
            if (General.InputBox("PipisCrew", "Please define the form name :", ref formName) == System.Windows.Forms.DialogResult.OK)
            {
                string gen = General.DB.generateFORMboostrap(lst.SelectedItems[0].Text, formName);
                addTAB(lst.SelectedItems[0].Text + " - Form");
                setCurrentTSQL(gen);
            }

        }

        private void toolStripTRcountRows_Click(object sender, EventArgs e)
        {
            runSQL(General.DB.GenerateCountRows((TR.SelectedNode.Tag as treeItem).nodeText));
        }

        private void toolStripLast100_Click(object sender, EventArgs e)
        {
            string PK = null;
            string first_field = null;
            foreach (var node in TR.SelectedNode.Children)
            {
                if (first_field == null)
                    first_field = (node.Tag as treeItem).nodeText;

                if ((node.Tag as treeItem).imageIndex == 2)
                    PK = (node.Tag as treeItem).nodeText;
            }

            if (PK != null)
                runSQL(General.DB.GenerateLast100((TR.SelectedNode.Tag as treeItem).nodeText, PK));
            else
            {
                if (General.Mes("If there is no PK, you cant continue to this option! You would like to try the first field as PK?", MessageBoxIcon.Information, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    runSQL(General.DB.GenerateLast100((TR.SelectedNode.Tag as treeItem).nodeText, first_field));
            }
        }

        private void toolStripGridCopyCell_Click(object sender, EventArgs e)
        {
            try
            {
                General.Copy2Clipboard(DG.SelectedCells[DG.CurrentCellAddress.X].Value.ToString());
            }
            catch
            {

            }
        }

        private void DG_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (DG.SelectedRows.Count > 0)
                    contextGrid.Show(System.Windows.Forms.Cursor.Position);
            }
        }

        private void toolStripSQLServerGenerateScript4Restore_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataTable dT;
                string error;
                string rowsAffected;

                dT = General.DB.ExecuteSQL("RESTORE FILELISTONLY FROM DISK = N'" + openFileDialog1.FileName + "'", out rowsAffected, out error);

                if (error.Length > 0)
                {
                    lstv.Items.Insert(0, error, 1).SubItems.Add("RESTORE FILELISTONLY FROM DISK = N'" + openFileDialog1.FileName + "'");
                }
                else
                {
                    try
                    {
                        //http://blog.sqlauthority.com/2007/02/25/sql-server-restore-database-backup-using-sql-script-t-sql/
                        string tmp = "RESTORE DATABASE yourDBname\r\nFROM DISK = N'" + openFileDialog1.FileName + "'\r\nWITH MOVE '" + dT.Rows[0][0].ToString() + "' TO 'C:\\Program Files\\Microsoft SQL Server\\MSSQL11.SQLEXPRESS\\MSSQL\\DATA\\yourDBname.mdf',\r\nMOVE '" + dT.Rows[1][0].ToString() + "' TO 'C:\\Program Files\\Microsoft SQL Server\\MSSQL11.SQLEXPRESS\\MSSQL\\DATA\\yourDBname.LDF'";
                        tmp = tmp.Replace("yourDBname", Path.GetFileNameWithoutExtension(openFileDialog1.FileName));

                        addTAB("restore script");

                        setCurrentTSQL(tmp);
                    }
                    catch (Exception ex)
                    {
                        General.Mes(ex.Message);
                    }
                }
            }
        }

        private void toolStripGridCopyColName_Click(object sender, EventArgs e)
        {
            try
            {
                int t = DG.SelectedCells[DG.CurrentCellAddress.X].ColumnIndex;

                General.Copy2Clipboard(DG.Columns[t].HeaderText.Substring(0, DG.Columns[t].HeaderText.IndexOf("(") - 1));
            }
            catch
            {

            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (General.DB != null)
                General.DB.Disconnect();
        }

        private void toolStripGenerateTABLEscript_Click(object sender, EventArgs e)
        {
            string tablename = (TR.SelectedNode.Tag as treeItem).nodeText;
            string tmp = General.DB.generateTableScript(tablename);
            addTAB("table script [" + tablename + "]");

            setCurrentTSQL(tmp);
        }

        private void toolStripGridExportXLS_Click(object sender, EventArgs e)
        {


            if (!(DG.DataSource is DataTable))
                return;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.Filter = "Excel (*.xls)|*.xls";
            saveFileDialog1.FilterIndex = 1;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (ExcelWriter writer = new ExcelWriter(saveFileDialog1.FileName))
                    {
                        writer.WriteStartDocument();
                        DataTable table = (DataTable)DG.DataSource;

                        writer.WriteStartWorksheet(string.Format("{0}", Application.ProductName)); // Write the worksheet contents
                        writer.WriteStartRow(); //Write header row

                        foreach (DataColumn col in table.Columns)
                        {
                            writer.WriteExcelUnstyledCell(col.Caption.Replace(" ", "_"));
                        }
                        writer.WriteEndRow();
                        foreach (DataRow row in table.Rows)
                        { //write data
                            writer.WriteStartRow();
                            foreach (object o in row.ItemArray)
                            {
                                writer.WriteExcelAutoStyledCell(o);
                            }
                            writer.WriteEndRow();
                        }
                        writer.WriteEndWorksheet(); // Close up the document
                        writer.WriteEndDocument();
                        writer.Close();
                    }
                }
                catch (Exception ex)
                { General.Mes(ex.Message); }


            }

        }


        private void toolStripCreateQuery4TSQL2_Click(object sender, EventArgs e)
        {
            if (DG.Columns.Count == 0 || getCurrentTSQL().Length == 0)
            {
                MessageBox.Show("-You must type the SQL\r\n-You must run the SQL", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tblName = "";

            if (General.InputBox("Datasource", "Datasource Name :", ref tblName) != System.Windows.Forms.DialogResult.OK)
                return;

            List<string> reportParams = new List<string>();

            for (int i = 0; i < DG.Columns.Count; i++)
            {
                reportParams.Add(DG.Columns[i].HeaderText.Substring(0, DG.Columns[i].HeaderText.LastIndexOf(" ")));
            }

            frmGenerator4Report frmY = new frmGenerator4Report(tblName, reportParams, tblName, DG.Tag.ToString());
            frmY.ShowDialog();
        }

        private void toolStripGenerateBootstrap4Table_Click(object sender, EventArgs e)
        {
            List<ListStrings> fields = new List<ListStrings>();
            string tblName = (TR.SelectedNode.Tag as treeItem).nodeText;

            string parameterName;
            string formTemplate = General.ReadASCIIfromResources("DBManager.ResourceSQLServer.proc_form_bootstrap_template.txt");
            string rowTemplate = "										<div class='form-group'>\r\n											<label>txt{{field}} :</label>\r\n											<input name='{{field}}' class='form-control' placeholder='txt{{field}}'>\r\n										</div>\r\n";
            string rowTemplateAppender = "";
            string submitFields = "";
            string listwhere = "";
            string listwheres = "";

            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).nodeCheck)
                    if ((node.Tag as treeItem).fieldType.ToLower() == "varchar" || (node.Tag as treeItem).fieldType.ToLower() == "nvarchar")
                        fields.Add(new ListStrings((node.Tag as treeItem).nodeText, (node.Tag as treeItem).fieldType.ToLower() + "(" +
                           (node.Tag as treeItem).fieldSize + ")"));
                    else
                        fields.Add(new ListStrings((node.Tag as treeItem).nodeText, (node.Tag as treeItem).fieldType.ToLower()));


                ///////
                parameterName = (node.Tag as treeItem).nodeText.ToLower();
                rowTemplateAppender += rowTemplate.Replace("{{field}}", parameterName);
                submitFields += "                                    \"" + parameterName + "\": $('[name=" + parameterName + "]').val() ,\r\n";
                listwhere += "         $('[name=" + parameterName + "]').val(e[0][\"" + parameterName + "\"]);\r\n";
                listwheres += "            inj+=\"<td>\"+ e[it][\"=" + parameterName + "\"] +\"</td>\";\r\n";

            }



            if (rowTemplateAppender.Length == 0)
                General.Mes("Please check the fields");
            else
            {
                string gen = formTemplate.Replace("{{modalElements}}", rowTemplateAppender)
                    .Replace("{{fields}}", submitFields)
                    .Replace("{{listwhere}}", listwhere)
                    .Replace("{{listwheres}}", listwheres)
                    .Replace("{{table}}", tblName.ToLower() + "FORM"); ;

                addTAB("table [" + tblName + "] bootstrap form");
                setCurrentTSQL(gen);
            }

        }

        private void toolStripProcsGenerateSAVEPHP_Click(object sender, EventArgs e)
        {
            if (!(General.DB is MySQLTunnel))
            {
                General.Mes("Not supported on this DB system");
                return;
            }
            else
            {
                string tmp;

                tmp = General.DB.parseProcedure(lst.SelectedItems[0].Text, true);

                if (tmp.Length < 5)
                {
                    General.Mes("too short");
                    return;
                }

                tmp = General.SliceSTR(tmp, "(", "BEGIN", 0);

                string[] lines = Regex.Split(tmp, "in ", RegexOptions.IgnoreCase);

                List<string> fields = new List<string>();
                List<string> fields_type = new List<string>();

                string tmp2 = "";
                foreach (string line in lines)
                {
                    tmp = line.Trim();

                    if (tmp.Length < 5)
                        continue;

                    tmp = tmp.Replace("`", "").Replace("`", "").Trim();

                    int pos = tmp.IndexOf(" ");

                    if (pos > -1)
                    {
                        tmp2 = tmp.Substring(pos).Trim();
                        tmp = tmp.Substring(0, pos).Trim();


                        if (tmp.EndsWith("VAR"))
                        {
                            tmp = tmp.Replace("VAR", "");
                        }

                        fields.Add(tmp);
                        fields_type.Add(tmp2);
                    }
                }



                //string postValidationTemplate = "";
                string postValidation = "";
                string parameterQ = "";
                string parameterBind_S = "";
                string parameterBind = "";
                string parametersSET = "";
                string bitValidation = "";

                int c = 0;
                foreach (string line in fields)
                {

                    parameterQ += "?, ";

                    if (fields_type[c].ToLower().Contains("bit"))
                    {
                        parameterBind_S += "i";
                        bitValidation += "$is_" + line + " = 0;\r\n";
                        bitValidation += "if (isset($_POST['" + line + "'])) {\r\n" +
                                        "	if ($_POST['" + line + "'] == 'on')\r\n" +
                                        "		$is_" + line + " = 1;\r\n" +
                                        "	else\r\n" +
                                        "		$is_" + line + " = 0;\r\n" +
                                        "}\r\n\r\n";

                        parametersSET += "$" + line + " = $is_" + line + ";\r\n";

                    }
                    else
                    {
                        postValidation += "!isset($_POST['" + line + "']) || ";
                        parameterBind_S += "s";
                        parametersSET += "$" + line + " = $_POST['" + line + "'];\r\n";
                    }

                    parameterBind += "$" + line + ", ";


                    //Console.WriteLine(line);
                    c += 1;
                }

                if (postValidation.Length > 4)
                {
                    postValidation = postValidation.Substring(0, postValidation.Length - 4);
                    parameterQ = parameterQ.Substring(0, parameterQ.Length - 2);
                    parameterBind = parameterBind.Substring(0, parameterBind.Length - 2);
                }

                string template = DBManager.Properties.Resources.mySQLtunnelPHPproc_save;

                template = template.Replace("**post validation**", postValidation);
                template = template.Replace("**proc_name**", lst.SelectedItems[0].Text);
                template = template.Replace("**q**", parameterQ);
                template = template.Replace("**s**", parameterBind_S);
                template = template.Replace("**bind_param**", parameterBind);
                template = template.Replace("**bind_set**", parametersSET);
                template = template.Replace("**bit validation**", bitValidation);

                addTAB("PHP save.php [" + lst.SelectedItems[0].Text + "] procedure INSERT");
                setCurrentTSQL(template);

                //frmInformation f = new frmInformation(template);
                //f.ShowDialog();
                //f.Dispose();
            }
        }

        private void generatePHPPagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGeneratePHPpages f = new frmGeneratePHPpages();
            f.ShowDialog();
            f.Dispose();
        }

        private void toolStripCopy2SelClip_Click(object sender, EventArgs e)
        {
            string FieldName = "";


            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).nodeCheck)
                {

                    FieldName += ((node.Tag as treeItem).nodeText) + ", ";

                }
            }

            if (FieldName.Length > 0)
            {
                FieldName = FieldName.Substring(0, FieldName.Length - 2);
                General.Copy2Clipboard(FieldName);
            }


        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string insertFields = "";
            string insertVAL = "";
            string updateVAL = "";
            string bind = "";
            string post_validation = "";

            string field_name = "";
            string PK = "unknownPrimaryKey";
            string tblName = (TR.SelectedNode.Tag as treeItem).nodeText;

            foreach (var node in TR.SelectedNode.Children)
            {
                if ((node.Tag as treeItem).imageIndex == 2)
                {
                    PK = (node.Tag as treeItem).nodeText.ToLower();
                    continue;
                }

                if ((node.Tag as treeItem).nodeCheck)
                {
                    field_name = ((node.Tag as treeItem).nodeText);

                    insertFields += field_name + ", ";
                    insertVAL += ":" + field_name + ", ";

                    updateVAL += field_name + "=:" + field_name + ", ";

                    post_validation += "!isset($_POST['" + field_name + "']) || ";

                    bind += "$stmt->bindValue(':" + field_name + "' , $_POST['" + field_name + "']);\r\n";
                }
            }

            if (insertFields.Length > 0)
            {
                insertFields = insertFields.Substring(0, insertFields.Length - 2);
                insertVAL = insertVAL.Substring(0, insertVAL.Length - 2);
                updateVAL = updateVAL.Substring(0, updateVAL.Length - 2);
                post_validation = post_validation.Substring(0, post_validation.Length - 4);

                string template = DBManager.Properties.Resources.PDOprepared;

                template = template.Replace("#validation#", post_validation);
                template = template.Replace("#updateWhere#", PK);
                template = template.Replace("#tblname#", tblName);
                template = template.Replace("#updateVAL#", updateVAL);
                template = template.Replace("#insertFields#", insertFields);
                template = template.Replace("#insertVAL#", insertVAL);
                template = template.Replace("#stmt#", bind);

                General.Copy2Clipboard(template);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            List<treeItem2> tables = new List<treeItem2>();
            treeItem2 table = null;
            treeItem2fields f=null;

            bool add_table = false;
            TreeModel x = (TreeModel)TR.Model;

            //TreeModel TRmodel = new TreeModel();

            foreach (Node item in x.Nodes)
            {
                add_table = false; 
                //table
                treeItem y = (treeItem)item;

                table = new treeItem2(y.nodeText);

                foreach (Node item2 in item.Nodes)
                {
                    //fields
                    
                    treeItem t = (treeItem)item2;

                    if (t.nodeCheck)
                    {
                        add_table = true;
                        table.table_fields.Add(new treeItem2fields(t.nodeText, t.fieldType, t.fieldSize, t.imageIndex == 2 ? true : false, t.allowNull));
                    }
                    //TRmodel.Nodes.Add(t);
                   
                     
//                    Console.WriteLine(y.nodeText + "." + t.nodeText);
                }
                
                

                if (add_table)
                    tables.Add(table);


            }

            if (tables.Count > 0)
            {
                frmGeneratePHP_CRUD k = new frmGeneratePHP_CRUD(tables);
                k.ShowDialog();
                k.Dispose();
            }

        }

        private void toolStripSelectALL_Click(object sender, EventArgs e)
        {
            check_uncheck_all(true);
        }


        private void toolStripDeselectALL_Click(object sender, EventArgs e)
        {
            check_uncheck_all(false);
        }

        private void check_uncheck_all(bool val)
        {
            TreeModel x = (TreeModel)TR.Model;
            foreach (Node item in x.Nodes)
            {
                treeItem y = (treeItem)item;
                y.IsChecked = val;
                y.nodeCheck = val;


                foreach (var item2 in item.Nodes)
                    (item2 as treeItem).nodeCheck = val;

            }

            TR.Refresh();
        }

        private void toolStripMenuItem2mysqli_Click(object sender, EventArgs e)
        {
            //kai omws
            TreeNodeAdv item = TR.SelectedNode;
            //pai3e
            //to ka8e node einai ena class item treeitem custom diko moy
            List<treeItem> rows = new List<treeItem>();
            foreach (var ea in item.Children)
            {
                rows.Add((treeItem)ea.Tag);
            }

            frmGeneratePHP frmP = new frmGeneratePHP(rows, (TR.SelectedNode.Tag as treeItem).nodeText);
            frmP.ShowDialog();

            Console.WriteLine(item);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            List<treeItem2> tables = new List<treeItem2>();
            treeItem2 table = null;
            treeItem2fields f = null;

            bool add_table = false;
            TreeModel x = (TreeModel)TR.Model;

            //TreeModel TRmodel = new TreeModel();

            foreach (Node item in x.Nodes)
            {
                add_table = false;
                //table
                treeItem y = (treeItem)item;

                table = new treeItem2(y.nodeText);

                foreach (Node item2 in item.Nodes)
                {
                    //fields

                    treeItem t = (treeItem)item2;

                    if (t.nodeCheck)
                    {
                        add_table = true;
                        table.table_fields.Add(new treeItem2fields(t.nodeText, t.fieldType, t.fieldSize, t.imageIndex == 2 ? true : false, t.allowNull));
                    }
                    //TRmodel.Nodes.Add(t);


                    //                    Console.WriteLine(y.nodeText + "." + t.nodeText);
                }



                if (add_table)
                    tables.Add(table);


            }

            if (tables.Count > 0)
            {
                frmJScustom d = new frmJScustom(tables);
                d.ShowDialog();
                d.Dispose();

                //frmGeneratePHP_CRUD k = new frmGeneratePHP_CRUD(tables);
                //k.ShowDialog();
                //k.Dispose();
            }

        }

        private void toolStripSQLServerTunnel_Click(object sender, EventArgs e)
        {
            frmSQLServerTunnelServerConnection frmS = new frmSQLServerTunnelServerConnection();
            DialogResult s = frmS.ShowDialog();
            frmS.Dispose();

            if (s == System.Windows.Forms.DialogResult.OK)
            {
                General.DB = new SQLServerTunnel(General.Connections.Count - 1, imageList1);

                Connect(true);
            }
        }

        private void DG_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            //https://stackoverflow.com/a/32105983/1320686 - fixes "Sum of the columns' FillWeight values cannot exceed 65535"
            e.Column.FillWeight = 10; 
        }



    }
}
