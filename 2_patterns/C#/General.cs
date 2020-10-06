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

        public static string ExtractHtmlInnerText(string htmlText)
        {
            //Match any Html tag (opening or closing tags) 
            // followed by any successive whitespaces
            //consider the Html text as a single line

            Regex regex = new Regex("(<.*?>\\s*)+", RegexOptions.Singleline);

            string resultText = regex.Replace(htmlText, " ").Trim();

            return resultText;
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
