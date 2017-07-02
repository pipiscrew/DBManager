using System;
using System.Data;
using System.Windows.Forms;

namespace EFClassGenerator
{
    public partial class frmLogin : Form
    {
        private MsSql sql { get; set; }
        public frmLogin()
        {
            InitializeComponent();
            this.CenterToScreen();
            SettingLoad();
        }

        private void SettingLoad()
        {
            txtServer.Text = Properties.Settings.Default.Server;
            chIntegratedSecurity.Checked = Properties.Settings.Default.IntegratedSecurity;
            txtUsername.Text = Properties.Settings.Default.User;
            txtPassword.Text = Properties.Settings.Default.Pass;
            coCatalog.Text = Properties.Settings.Default.Catalog;
        }
        private void SaveSettings()
        {
            Properties.Settings.Default.Server = txtServer.Text;
            Properties.Settings.Default.User = txtUsername.Text;
            Properties.Settings.Default.Pass = txtPassword.Text;
            Properties.Settings.Default.Catalog = coCatalog.Text;
            Properties.Settings.Default.IntegratedSecurity = chIntegratedSecurity.Checked;
            Properties.Settings.Default.Save();

        }
        private void SetMSSql()
        {
            if (chIntegratedSecurity.Checked)
            {
                sql = new MsSql(txtServer.Text);
            }
            else
            {
                sql = new MsSql(txtServer.Text, txtUsername.Text, txtPassword.Text);
            }

        }
        private void FillCatalogList()
        {
            DataTable dt = new DataTable();
            dt = sql.GetCatalogList();
            coCatalog.DataSource = dt;
            coCatalog.DisplayMember = "name";
            coCatalog.Enabled = true;
            cmdGetCatalog.Enabled = true;
            coCatalog.Text = Properties.Settings.Default.Catalog;
        }


        private void cmdNext_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                SetMSSql();
                sql.Catalog = coCatalog.Text;
                sql.CheckConnection();
                SaveSettings();
                this.Hide();
                Form frm = new frmMain();
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Cursor = Cursors.Default;

        }
        private void cmdTestConnection_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                SetMSSql();
                FillCatalogList();
                SaveSettings();
                cmdSave.Enabled = true;
            }
            catch (Exception ex)
            {
                cmdSave.Enabled = false;
                coCatalog.Enabled = false;
                cmdGetCatalog.Enabled = false;
                MessageBox.Show(ex.Message,"SQL server connection issue");
            }

            this.Cursor = Cursors.Default;

        }
        private void OnclickWindowsAuthentication(object sender, EventArgs e)
        {
            txtPassword.Enabled = (!chIntegratedSecurity.Checked);
            txtUsername.Enabled = (!chIntegratedSecurity.Checked);
        }
        private void cmdGetCatalog_Click(object sender, EventArgs e)
        {
            FillCatalogList();
        }
        private void cmdExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
