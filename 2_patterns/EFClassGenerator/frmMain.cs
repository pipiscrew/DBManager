using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace EFClassGenerator
{
    public partial class frmMain : Form
    {
        public MsSql sql { get; set; }
        public CodeGenerator CodeGen { get; set; }
        public frmMain()
        {
            try
            {
                InitializeComponent();
                if (Properties.Settings.Default.IntegratedSecurity)
                {
                    sql = new MsSql(Properties.Settings.Default.Server);
                }
                else
                {
                    sql = new MsSql(Properties.Settings.Default.Server, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                }
                SettingLoad();
                cmdReloadTables_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Generate(CodeGenerator.enumTemplateFile template)
        {
            if (CheckInput())
            {
                SettingSave();
                CodeGen = new CodeGenerator();
                CodeGen.Namespace = txtNamespace.Text.Trim();
                CodeGen.Context = txtEFContext.Text.Trim();
                CodeGen.TableName = coTabellen.Text.Trim();
                CodeGen.IndexColumn = coPKColumn.Text.Trim();
                CodeGen.OrderColumn = coOrderColumn.Text.Trim();

                //Check columns
                foreach (DataRowView item in coPKColumn.Items)
                {
                    if (item.Row[0].ToString() == "DELETED") CodeGen.useDELETED = true;
                    if (item.Row[0].ToString() == "UPDATED") CodeGen.useUPDATED = true;
                    if (item.Row[0].ToString() == "CREATED") CodeGen.useCREATED = true;
                }

                //Get type of PK column
                object col = sql.GetColumnDataType(coTabellen.Text.Trim(), coPKColumn.Text.Trim());
                if (col != null) CodeGen.IndexColumnDataType = col.ToString();

                //Generate code
                txtOut.Text = CodeGen.Generate(template);
                txtOut.Focus();
                txtOut.SelectionLength = 0;

            }

        }
        private void ReloadTables()
        {
            coTabellen.DisplayMember = "TABLE_NAME";
            if (optTables.Checked)
            {
                coTabellen.DataSource = sql.GetTableList();
            }
            else
            {
                coTabellen.DataSource = sql.GetViewList();
            }
            if (coTabellen.Items.Count > 0) coTabellen.SelectedIndex = 0;

        }
        private void ReloadColumns()
        {
            coPKColumn.DisplayMember = "column_name";
            coPKColumn.DataSource = sql.GetColumnList(coTabellen.Text);
            coOrderColumn.DisplayMember = "column_name";
            coOrderColumn.DataSource = sql.GetColumnList(coTabellen.Text);

        }
        private void SettingLoad()
        {
            txtEFContext.Text = Properties.Settings.Default.EFContext;
            txtNamespace.Text = Properties.Settings.Default.Namespace;
        }
        private void SettingSave()
        {
            Properties.Settings.Default.EFContext = txtEFContext.Text.Trim();
            Properties.Settings.Default.Namespace = txtNamespace.Text.Trim();
            Properties.Settings.Default.Save();

        }
        private bool CheckInput()
        {
            if (txtNamespace.Text.Trim() == "" || txtEFContext.Text.Trim() == "")
            {
                MessageBox.Show("You need to enter the Namespace and EF Context of your destination project", "Input missing");
                return false;
            }
            else
            {
                return true;
            }
        }


        private void cmdGenerateClass_Click(object sender, EventArgs e)
        {
            if (optTables.Checked)
            {
                Generate(CodeGenerator.enumTemplateFile.Table);
            }
            else
            {
                Generate(CodeGenerator.enumTemplateFile.View);
            }
        }
        private void cmdGenerateException_Click(object sender, EventArgs e)
        {
            Generate(CodeGenerator.enumTemplateFile.Exception);
        }
        private void cmdGenerateSelectList_Click(object sender, EventArgs e)
        {
            Generate(CodeGenerator.enumTemplateFile.SelList);
        }
        private void cmdReloadTables_Click(object sender, EventArgs e)
        {
            sql.Catalog = Properties.Settings.Default.Catalog;
            ReloadTables();
            ReloadColumns();
        }
        private void coTabellen_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadColumns();
        }
        private void cmdSaveToFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = CodeGen.SaveFileName;

            saveFileDialog1.DefaultExt = "cs";
            saveFileDialog1.Filter = "Visual C# Source files (*.cs)|*.cs|All files (*.*)|*.*";
            saveFileDialog1.OverwritePrompt = true;

            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, txtOut.Text.Trim());
            }
        }
        private void optTables_Click(object sender, EventArgs e)
        {
            cmdReloadTables_Click(null, null);
        }
        private void optViews_Click(object sender, EventArgs e)
        {
            cmdReloadTables_Click(null, null);
        }
        private void cmdCopyClip_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtOut.Text.Trim());
        }
        private void cmdTemplateOpen_Click(object sender, EventArgs e)
        {

            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "Template files (tpl*.txt)|tpl*.txt|All files (*.*)|*.*";

            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                Process p = new Process();
                p.StartInfo.FileName = openFileDialog1.FileName;
                p.Start();

            }

        }
    }
}
