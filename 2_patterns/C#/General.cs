using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

static class General
{
	#region " CLIPBOARD OPERATIONS"

	public static void Copy2Clipboard(string val)
	{
		try
		{
			Clipboard.Clear();
			Clipboard.SetDataObject(val, true);
		}
		catch (Exception e)
		{
			MessageBox.Show(e.Message, Application.ProductName);
		}
	}

	public static string GetFromClipboard()
	{
		try
		{
			return Clipboard.GetText().Trim();
		}
		catch (Exception e)
		{
			MessageBox.Show(e.Message, Application.ProductName);
			return "";
		}
	}

	#endregion


	internal static void PointFile(string filepath)
	{
		Process objProcess = default(System.Diagnostics.Process);
		try
		{
			objProcess = new System.Diagnostics.Process();
			objProcess.StartInfo.FileName = "explorer.exe";
			objProcess.StartInfo.Arguments = "/select, " + filepath;
			objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			objProcess.Start();

			objProcess.Close();
		}
		catch
		{
		}
	}

	internal static DialogResult Mes(string descr, MessageBoxIcon icon = MessageBoxIcon.Information, MessageBoxButtons butt = MessageBoxButtons.OK)
	{
		if (descr.Length > 0)
			return MessageBox.Show(descr, Application.ProductName, butt, icon);
		else
			return DialogResult.OK;

	}
	
	public static string GetOrDefault(string keyName, string defaultValue)
	{//application_name.exe.config
		string value = ConfigurationManager.AppSettings[keyName];

		if (value != null)
			return value;
		else
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings.Add(keyName, defaultValue);
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}

		return defaultValue;
	}
	
	public static string GetOrDefault2(string keyName, string defaultValue)
	{//Settings.settings
		var settings = Properties.Settings.Default;

		if (settings.Properties[keyName] != null)
			return settings.Properties[keyName].DefaultValue.ToString();
		
		else
		{
			settings.Properties.Add(new SettingsProperty(keyName)
			{
				DefaultValue = defaultValue,
				IsReadOnly = false
			});

			settings.Reload();
			settings.Save(); //is not writing the value to file, is only for current session
			return defaultValue;
		}
	}

	internal static DataTable ImportDelimitedFile(string filename, string delimiter, bool first_is_column_names)
	{

		DataTable dt = new DataTable();


		using (StreamReader file = new StreamReader(filename))
		{
			//read the first line
			string line = file.ReadLine();

			if (line == null)
				return null;

			//split the first line to create columns to datatable!
			string[] columns = Regex.Split(line, delimiter); //line.Split(Convert.ToChar(delimiter));// Regex.Split(line, "|");

			for (int i = 0; i < columns.Count(); i++)
			{
				if (first_is_column_names)
					dt.Columns.Add(columns[i].Replace("\"", ""));
				else
					dt.Columns.Add("no" + i.ToString());
			}

			if (!first_is_column_names)
			{
				//rewind reader to start!
				file.DiscardBufferedData();
				file.BaseStream.Seek(0, SeekOrigin.Begin);
				file.BaseStream.Position = 0;
			}

			while ((line = file.ReadLine()) != null)
			{
				if (line.Trim().Length > 0)
				{
					line = line.Replace("\"", "");
					string[] rows = Regex.Split(line, delimiter); // line.Split(Convert.ToChar(delimiter));//Regex.Split(line, delimiter);
					dt.Rows.Add(rows);

				}
			}
		}

		return dt;
	}


	internal static void addDataTable2Grid(string tableName, DataTable dT, DataGridView dg)
	{
		//Column header style
		DataGridViewCellStyle columnHeader = new DataGridViewCellStyle();
		FontFamily family = new FontFamily("Calibri");
		Font font = new Font(family, 10f, FontStyle.Bold);

		columnHeader.BackColor = Color.FromArgb(0, 112, 192);
		columnHeader.ForeColor = Color.White;
		columnHeader.Font = font;

		DataGridViewCellStyle cellNull = new DataGridViewCellStyle();
		cellNull.ForeColor = Color.FromArgb(220, 20, 60);
		//

		dg.Rows.Add();
		dg.Rows.Add();
		dg.Rows[dg.Rows.Count - 1].Cells[0].Value = tableName;
		dg.Rows[dg.Rows.Count - 1].Cells[1].Value = dT.Rows.Count;

		//column names
		dg.Rows.Add();

		int cellIndex = 1;

		foreach (DataColumn item in dT.Columns)
		{
			dg.Rows[dg.Rows.Count - 1].Cells[cellIndex].Value = item.Caption;
			dg.Rows[dg.Rows.Count - 1].Cells[cellIndex].Style = columnHeader;
			cellIndex += 1;
		}

		//rows
		foreach (DataRow row in dT.Rows)
		{
			cellIndex = 1;
			dg.Rows.Add();

			foreach (DataColumn item in dT.Columns)
			{
				if (row[item] is DBNull)
				{
					dg.Rows[dg.Rows.Count - 1].Cells[cellIndex].Style = cellNull;
					dg.Rows[dg.Rows.Count - 1].Cells[cellIndex].Value = "NULL";
				}
				else
					dg.Rows[dg.Rows.Count - 1].Cells[cellIndex].Value = row[item].ToString();

				cellIndex += 1;
			}

		}

		/*
		must add the columns manually at form load, like :
			for (int i = 0; i < 25; i++)
			{
				dg.Columns.Add("Col" + i, "Col" + i);
			}
		
		for clear the grid on the form use :
			dg.DataSource = null;
			dg.Rows.Clear();
		 
		at designer : 
			this.dg.AllowUserToAddRows = false;
			this.dg.AllowUserToDeleteRows = false;
			this.dg.AllowUserToResizeRows = false;
			this.dg.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dg.ColumnHeadersVisible = false;
			this.dg.RowHeadersVisible = false;
			this.dg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dg.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
		 */
	}

	public static DataTable xml2datable()
	{  //https://stackoverflow.com/a/7801673
	   //get all records belong to EMEA, region field - xml2datable().AsEnumerable().Where(x => x.Field<string>("Region").Equals("EMEA")).ToList();
	   //https://www.pipiscrew.com/2017/01/convert-excel-sheet-to-xml-elements-or-xml-attributes/
		StringReader theReader = new StringReader(@"B:\countries.xml");
		DataSet theDataSet = new DataSet();
		theDataSet.ReadXml(theReader.ReadToEnd());

		theReader.Dispose();

		return theDataSet.Tables[0];
	}
	
        public static string ExtractHtmlInnerText(string htmlText)
        {
            //Match any Html tag (opening or closing tags) 
            // followed by any successive whitespaces
            //consider the Html text as a single line

            Regex regex = new Regex("(<.*?>\\s*)+", RegexOptions.Singleline);

            string resultText = regex.Replace(htmlText, " ").Trim();

            return resultText;
        }

        internal void ExportDG2XML(DataGridView dg,  string filepath)
        {  //the file extension MUST be .xls

            //false = overwrite the file if exists
            using (var sw = new StreamWriter(filepath, false, System.Text.Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();

                //xls header
                sb.AppendLine("<?xml version='1.0'?>");
                sb.AppendLine("<?mso-application progid='Excel.Sheet'?>");
                sb.AppendLine("<s:Workbook xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:s=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                sb.AppendLine("  <s:Worksheet s:Name=\"export\">");
                sb.AppendLine("    <s:Table>");

                //columns row [start]
                sb.AppendLine("      <s:Row>");

                string cellTemplate = "        <s:Cell><s:Data s:Type=\"String\">{0}</s:Data></s:Cell>";
                foreach (DataGridViewColumn col in dg.Columns)
                {
                    sb.AppendLine(string.Format(cellTemplate, col.Name));
                }
                sb.AppendLine("      </s:Row>");
                //columns row [end]

                //rows [start]
                foreach (DataGridViewRow r in dg.Rows)
                {

                    sb.AppendLine("      <s:Row>");

                    foreach (DataGridViewCell c in r.Cells)
                    {
                        sb.AppendLine(string.Format(cellTemplate, c.Value.ToStrinX()));
                    }

                    sb.Append("      </s:Row>");

                    //write each row to StreamWriter (write to memory)
                    if (sb.Length > 0)
                    {
                        sw.WriteLine(sb.ToString());
                        sb.Length = 0;                  //empty stringbuilder
                    }
                }
                //rows [end]

                sb.AppendLine("    </s:Table>");
                sb.AppendLine("  </s:Worksheet>");
                sb.AppendLine("</s:Workbook>");

                if (sb.Length > 0)
                { //StreamWriter
                    sw.WriteLine(sb.ToString());
                    sb.Length = 0;
                }
            }
        }

        internal void ExportDG2CSV(DataGridView dg, string delimiter, string filepath)
        {   //src - https://github.com/unvell/ReoGrid/blob/1bc1bf59432e2dadc5d23c1ab48c3ffb866ffbfc/ReoGrid/Core/CSV.cs
            // EXCEL default delimiter ;

            //false = overwrite the file if exists
            using (var sw = new StreamWriter(filepath, false, System.Text.Encoding.Default))
            {
                StringBuilder sb = new StringBuilder();

                foreach (DataGridViewColumn col in dg.Columns)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(delimiter);
                    }
                    sb.Append(col.Name);
                }

                if (sb.Length > 0)
                {
                    sw.WriteLine(sb.ToString());    //streamWriter - write to memory
                    sb.Length = 0;                  //empty stringbuilder
                }

                foreach (DataGridViewRow r in dg.Rows)
                {   // rows [start]

                    foreach (DataGridViewCell c in r.Cells)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(delimiter);
                        }

                        string cell = c.Value.ToStrinX();

                        if (string.IsNullOrEmpty(cell))
                            continue;

                        bool quota = false;

                        if (cell.IndexOf(delimiter) >= 0 || cell.IndexOf('"') >= 0
                                    || cell.StartsWith(" ") || cell.EndsWith(" "))
                        {
                            quota = true;
                        }

                        if (quota)
                        {
                            sb.Append('"');
                            sb.Append(cell.Replace("\"", "\"\""));
                            sb.Append('"');
                        }
                        else
                        {
                            sb.Append(cell);
                        }
                    }

                    //write each row to StreamWriter (write to memory)
                    if (sb.Length > 0)
                    {
                        sw.WriteLine(sb.ToString());    
                        sb.Length = 0;                  //empty stringbuilder
                    }

                } // rows [end]

            } //write the file
        }
	/*  Knowledge Base
		Cursor = System.Windows.Forms.Cursors.WaitCursor;
		Cursor = System.Windows.Forms.Cursors.Default;
		-
		string.Format("{0}{1}{0}", "'", string.Join("','", IEnumerable<string> var));
		-
		CTRL click - Control.ModifierKeys == Keys.Control
		-
		Validate list if contains int - ids.Any(x => x.ToLong() == 0)
		Count nulls - .Where(r => string.IsNullOrEmpty(r.Cells["X"].Value)).Count()).ToInt()
		-
		Split enter - q.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		-
		new string[] { "99", "18", "11", "31", "14" }.Contains(resp.Body)
	 */

}
