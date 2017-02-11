using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

internal sealed class ErrorReporter : Form
{
    private static string ErrorDirectory = ".\\Errors";
    private readonly string ErrorPath;
    private Button btnDontSend;
    private CheckBox chkRestart;
    private ColumnHeader colDescription;
    private ColumnHeader colValue;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private Label labelInfo;
    private ListView listException;
    private ListView listGeneral;
    private Panel panelError;
    private Panel panelMain;
    private PictureBox picErrorLogo;
    private TabPage tabException;
    private TabPage tabGeneral;
    private TabControl tabMain;
    private TabPage tabStackTrace;
    private TextBox txtStackTrace;

    public ErrorReporter()
    {
        this.InitializeComponent();
        base.Icon = SystemIcons.Error;
        this.picErrorLogo.Image = SystemIcons.Error.ToBitmap();
        this.labelInfo.Text = string.Format(this.labelInfo.Text, Application.ProductName);
        this.Text = string.Format("{0} Exception Handler", Application.ProductName);
        this.chkRestart.Text = string.Format(this.chkRestart.Text, Application.ProductName);
        base.TopMost = true;
        base.TopLevel = true;
        this.ErrorPath = string.Format("{0}\\{1:yyyy}_{1:MM}_{1:dd}_{1:HH}{1:mm}{1:ss}.log", ErrorReporter.ErrorDirectory, DateTime.Now);
        this.UpdateGeneralData();
    }
    public ErrorReporter(Exception e)
        : this()
    {
        this.UpdateExceptionData(e);
        this.UpdateStackTrace(e);
    }
    private void UpdateGeneralData()
    {
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"Current Date/Time", 
			DateTime.Now.ToString()
		}));
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"Process Started", 
			Process.GetCurrentProcess().StartTime.ToString()
		}));
        try
        {
            FileInfo fileInfo = new FileInfo(Application.ExecutablePath);
            this.listGeneral.Items.Add(new ListViewItem(new string[]
			{
				"Last Modified", 
				fileInfo.LastWriteTime.ToString()
			}));
        }
        catch
        {
        }
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"Operating System", 
			Environment.OSVersion.VersionString
		}));
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"Language", 
			Application.CurrentInputLanguage.LayoutName
		}));
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"System Uptime", 
			TimeSpan.FromMilliseconds((double)Environment.TickCount).ToString()
		}));
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"Program Uptime", 
			Process.GetCurrentProcess().TotalProcessorTime.ToString()
		}));
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"Process ID", 
			Process.GetCurrentProcess().Id.ToString()
		}));
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"Thread Count", 
			Process.GetCurrentProcess().Threads.Count.ToString()
		}));
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"Thread ID", 
			Thread.CurrentThread.ManagedThreadId.ToString()
		}));
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"Executable Path", 
			Application.ExecutablePath
		}));
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"Product Version", 
			Application.ProductVersion
		}));
        this.listGeneral.Items.Add(new ListViewItem(new string[]
		{
			"CLR Version", 
			Environment.Version.ToString()
		}));
        this.listGeneral.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
        this.listGeneral.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
        this.listGeneral.Columns[0].Width += 20;
    }
    private void UpdateExceptionData(Exception e)
    {
        if (e == null)
        {
            return;
        }
        this.RecursiveUpdateExceptionData(e, 0);
        this.listException.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
        this.listException.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
        this.listException.Columns[0].Width += 20;
    }
    private void RecursiveUpdateExceptionData(Exception e, int count)
    {
        this.listException.Items.Add(new ListViewItem(new string[]
		{
			string.Format("{0} {1}", (count == 0) ? "Exception" : "Inner Exception", (count == 0) ? "" : ("#" + count)), 
			e.GetType().ToString()
		}));
        if (!string.IsNullOrEmpty(e.Message))
        {
            this.listException.Items.Add(new ListViewItem(new string[]
			{
				"   Message", 
				e.Message
			}));
        }
        if (!string.IsNullOrEmpty(e.Source))
        {
            this.listException.Items.Add(new ListViewItem(new string[]
			{
				"   Source", 
				e.Source
			}));
        }
        if (!string.IsNullOrEmpty(e.HelpLink))
        {
            this.listException.Items.Add(new ListViewItem(new string[]
			{
				"   Help Link", 
				e.HelpLink
			}));
        }
        if (e.TargetSite != null)
        {
            this.listException.Items.Add(new ListViewItem(new string[]
			{
				"   Target Site", 
				e.TargetSite.ToString()
			}));
        }
        if (e.Data.Count > 0)
        {
            foreach (DictionaryEntry dictionaryEntry in e.Data)
            {
                this.listException.Items.Add(new ListViewItem(new string[]
				{
					"   Data", 
					string.Format("Key: {0} | Value: {1}", dictionaryEntry.Key, dictionaryEntry.Value)
				}));
            }
        }
        if (e.InnerException != null)
        {
            this.RecursiveUpdateExceptionData(e.InnerException, ++count);
        }
    }
    private void UpdateStackTrace(Exception e)
    {
        this.txtStackTrace.Text = string.Format("Exception Stack Trace\r\n\r\n{0}\r\n\r\nEnvironment Stack Strace\r\n\r\n{1}", e.StackTrace, Environment.StackTrace);
    }
    public bool CreateErrorLog()
    {
        if (!Directory.Exists(ErrorReporter.ErrorDirectory))
        {
            Directory.CreateDirectory(ErrorReporter.ErrorDirectory);
        }
        if (File.Exists(this.ErrorPath))
        {
            return true;
        }
        try
        {
            using (StreamWriter streamWriter = new StreamWriter(this.ErrorPath, false))
            {
                for (int i = 0; i < this.listGeneral.Items.Count; i++)
                {
                    streamWriter.WriteLine(string.Format("{0}: {1}", this.listGeneral.Items[i].SubItems[0].Text, this.listGeneral.Items[i].SubItems[1].Text));
                }
                streamWriter.WriteLine();
                for (int j = 0; j < this.listException.Items.Count; j++)
                {
                    streamWriter.WriteLine(string.Format("{0}: {1}", this.listException.Items[j].SubItems[0].Text, this.listException.Items[j].SubItems[1].Text));
                }
                streamWriter.WriteLine();
                streamWriter.WriteLine(this.txtStackTrace.Text);
            }
        }
        catch
        {
            return false;
        }
        return File.Exists(this.ErrorPath);
    }
    private void btnDontSend_Click(object sender, EventArgs e)
    {
        CreateErrorLog();
        if (this.chkRestart.Checked)
        {
            Application.Restart();
            Environment.Exit(1);
        }

        Environment.Exit(1);
    }
    private void ErrorReporter_FormClosed(object sender, FormClosedEventArgs e)
    {
        if (this.chkRestart.Checked)
        {
            Application.Restart();
            Environment.Exit(1);
        }
        Environment.Exit(1);
    }
    private void InitializeComponent()
    {
        this.panelError = new Panel();
        this.labelInfo = new Label();
        this.picErrorLogo = new PictureBox();
        this.panelMain = new Panel();
        this.tabMain = new TabControl();
        this.tabGeneral = new TabPage();
        this.tabException = new TabPage();
        this.tabStackTrace = new TabPage();
        this.listGeneral = new ListView();
        this.colDescription = new ColumnHeader();
        this.colValue = new ColumnHeader();
        this.listException = new ListView();
        this.columnHeader1 = new ColumnHeader();
        this.columnHeader2 = new ColumnHeader();
        this.txtStackTrace = new TextBox();
        this.btnDontSend = new Button();
        this.chkRestart = new CheckBox();
        this.panelError.SuspendLayout();
        ((ISupportInitialize)this.picErrorLogo).BeginInit();
        this.panelMain.SuspendLayout();
        this.tabMain.SuspendLayout();
        this.tabGeneral.SuspendLayout();
        this.tabException.SuspendLayout();
        this.tabStackTrace.SuspendLayout();
        base.SuspendLayout();
        this.panelError.BackColor = Color.White;
        this.panelError.BorderStyle = BorderStyle.FixedSingle;
        this.panelError.Controls.Add(this.labelInfo);
        this.panelError.Controls.Add(this.picErrorLogo);
        this.panelError.Location = new Point(0, 0);
        this.panelError.Name = "panelError";
        this.panelError.Size = new Size(395, 61);
        this.panelError.TabIndex = 0;
        this.labelInfo.Font = new Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
        this.labelInfo.ForeColor = Color.Black;
        this.labelInfo.Location = new Point(67, 6);
        this.labelInfo.Name = "labelInfo";
        this.labelInfo.Size = new Size(315, 48);
        this.labelInfo.TabIndex = 1;
        this.labelInfo.Text = "An exception occured in a component of {0}. We have created an error report that you can send us. This report will be confidential and anonymous.";
        this.picErrorLogo.Location = new Point(12, 6);
        this.picErrorLogo.Name = "picErrorLogo";
        this.picErrorLogo.Size = new Size(48, 48);
        this.picErrorLogo.SizeMode = PictureBoxSizeMode.CenterImage;
        this.picErrorLogo.TabIndex = 1;
        this.picErrorLogo.TabStop = false;
        this.panelMain.BorderStyle = BorderStyle.Fixed3D;
        this.panelMain.Controls.Add(this.tabMain);
        this.panelMain.Location = new Point(-2, 60);
        this.panelMain.Name = "panelMain";
        this.panelMain.Size = new Size(397, 280);
        this.panelMain.TabIndex = 1;
        this.tabMain.Controls.Add(this.tabGeneral);
        this.tabMain.Controls.Add(this.tabException);
        this.tabMain.Controls.Add(this.tabStackTrace);
        this.tabMain.Dock = DockStyle.Fill;
        this.tabMain.Location = new Point(0, 0);
        this.tabMain.Name = "tabMain";
        this.tabMain.SelectedIndex = 0;
        this.tabMain.Size = new Size(393, 276);
        this.tabMain.TabIndex = 0;
        this.tabGeneral.Controls.Add(this.listGeneral);
        this.tabGeneral.Location = new Point(4, 22);
        this.tabGeneral.Name = "tabGeneral";
        this.tabGeneral.Padding = new Padding(3);
        this.tabGeneral.Size = new Size(385, 250);
        this.tabGeneral.TabIndex = 0;
        this.tabGeneral.Text = "General";
        this.tabGeneral.UseVisualStyleBackColor = true;
        this.tabException.Controls.Add(this.listException);
        this.tabException.Location = new Point(4, 22);
        this.tabException.Name = "tabException";
        this.tabException.Padding = new Padding(3);
        this.tabException.Size = new Size(385, 250);
        this.tabException.TabIndex = 1;
        this.tabException.Text = "Exception";
        this.tabException.UseVisualStyleBackColor = true;
        this.tabStackTrace.Controls.Add(this.txtStackTrace);
        this.tabStackTrace.Location = new Point(4, 22);
        this.tabStackTrace.Name = "tabStackTrace";
        this.tabStackTrace.Size = new Size(385, 250);
        this.tabStackTrace.TabIndex = 2;
        this.tabStackTrace.Text = "Stack Trace";
        this.tabStackTrace.UseVisualStyleBackColor = true;
        this.listGeneral.Columns.AddRange(new ColumnHeader[]
		{
			this.colDescription, 
			this.colValue
		});
        this.listGeneral.Dock = DockStyle.Fill;
        this.listGeneral.GridLines = true;
        this.listGeneral.Location = new Point(3, 3);
        this.listGeneral.Name = "listGeneral";
        this.listGeneral.Size = new Size(379, 244);
        this.listGeneral.TabIndex = 0;
        this.listGeneral.UseCompatibleStateImageBehavior = false;
        this.listGeneral.View = View.Details;
        this.colDescription.Text = "Description";
        this.colValue.Text = "Value";
        this.listException.Columns.AddRange(new ColumnHeader[]
		{
			this.columnHeader1, 
			this.columnHeader2
		});
        this.listException.Dock = DockStyle.Fill;
        this.listException.GridLines = true;
        this.listException.Location = new Point(3, 3);
        this.listException.Name = "listException";
        this.listException.Size = new Size(379, 244);
        this.listException.TabIndex = 1;
        this.listException.UseCompatibleStateImageBehavior = false;
        this.listException.View = View.Details;
        this.columnHeader1.Text = "Description";
        this.columnHeader1.Width = 101;
        this.columnHeader2.Text = "Value";
        this.columnHeader2.Width = 95;
        this.txtStackTrace.Dock = DockStyle.Fill;
        this.txtStackTrace.Location = new Point(0, 0);
        this.txtStackTrace.Multiline = true;
        this.txtStackTrace.Name = "txtStackTrace";
        this.txtStackTrace.ReadOnly = true;
        this.txtStackTrace.ScrollBars = ScrollBars.Both;
        this.txtStackTrace.Size = new Size(385, 250);
        this.txtStackTrace.TabIndex = 0;
        this.btnDontSend.Location = new Point(307, 343);
        this.btnDontSend.Name = "btnDontSend";
        this.btnDontSend.Size = new Size(75, 23);
        this.btnDontSend.TabIndex = 2;
        this.btnDontSend.Text = "Close";
        this.btnDontSend.UseVisualStyleBackColor = true;
        this.btnDontSend.Click += new EventHandler(this.btnDontSend_Click);
        this.chkRestart.AutoSize = true;
        this.chkRestart.Location = new Point(12, 347);
        this.chkRestart.Name = "chkRestart";
        this.chkRestart.Size = new Size(81, 17);
        this.chkRestart.TabIndex = 4;
        this.chkRestart.Text = "Restart {0}";
        this.chkRestart.UseVisualStyleBackColor = true;
        base.ClientSize = new Size(394, 372);
        base.Controls.Add(this.chkRestart);
        base.Controls.Add(this.btnDontSend);
        base.Controls.Add(this.panelMain);
        base.Controls.Add(this.panelError);
        this.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
        base.FormBorderStyle = FormBorderStyle.FixedDialog;
        base.MaximizeBox = false;
        base.Name = "ErrorReporter";
        base.SizeGripStyle = SizeGripStyle.Hide;
        base.StartPosition = FormStartPosition.CenterScreen;
        base.FormClosed += new FormClosedEventHandler(this.ErrorReporter_FormClosed);
        this.panelError.ResumeLayout(false);
        ((ISupportInitialize)this.picErrorLogo).EndInit();
        this.panelMain.ResumeLayout(false);
        this.tabMain.ResumeLayout(false);
        this.tabGeneral.ResumeLayout(false);
        this.tabException.ResumeLayout(false);
        this.tabStackTrace.ResumeLayout(false);
        this.tabStackTrace.PerformLayout();
        base.ResumeLayout(false);
        base.PerformLayout();
    }
}