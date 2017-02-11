using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBManager.DBASES;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.Web.Script.Serialization;

namespace DBManager
{
   public static class General
    {
        public static string apTitle = "PipisCrew.DBManager v" + Application.ProductVersion;

        public static IdbType DB;

        [Serializable]
        public enum dbTypes
        {
            SQLSERVER = 0x01,
            Access = 0x02,
            SQLite = 0x03,
            MySQL = 0x04,
            MySQLtunnel = 0x05,
        }

        public static List<dbConnection> Connections;

        //used only by mySQLTunnel! @ copy rows from other dbase-31/8/14->made 2 all!
        public static int activeConnection = -1;


        public static DialogResult Mes(string descr, MessageBoxIcon icon = MessageBoxIcon.Information, MessageBoxButtons butt = MessageBoxButtons.OK)
        {
            if (descr.Length > 0)
                return MessageBox.Show(descr, General.apTitle, butt, icon);
            else
                return DialogResult.OK;

        }

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
                MessageBox.Show(e.Message, General.apTitle);
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
                MessageBox.Show(e.Message, General.apTitle);
                return "";
            }
        }

        #endregion

        #region " Serialize / De List2File "

        public static bool SerializeList2File()
        {
            try
            {
                TextWriter textWriter = new StreamWriter(Application.StartupPath + "\\connections.cfg");
                XmlSerializer serializer = new XmlSerializer(typeof(List<dbConnection>));
                serializer.Serialize(textWriter, Connections);
                textWriter.Close();
                textWriter.Dispose();

                return true;
            }
            catch
            {
                return false; 
            }
        }

        public static bool DeSerializeFile2List()
        {
            try
            {
                Connections = null;
                TextReader textReader = new StreamReader(Application.StartupPath + "\\connections.cfg");
                XmlSerializer deserializer = new XmlSerializer(typeof(List<dbConnection>));
                Connections = (List<dbConnection>)deserializer.Deserialize(textReader);

                textReader.Close();
                textReader.Dispose();

                return true;
            }
            catch { return false; }
            finally
            {
                if (Connections == null)
                    Connections = new List<dbConnection>();
            }
        }

        #endregion

        public static string ReadASCIIfromResources(string filename)
        {
            byte[] Buffer;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename))
            {
                Buffer = new byte[stream.Length];
                stream.Read(Buffer, 0, Buffer.Length);
            }

            return Encoding.ASCII.GetString(Buffer);
        }

        #region  InputBox

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new BlueForm();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            label.BackColor = Color.Transparent;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        #endregion 

        public static string SafeJSON(string sIn)
        {
            StringBuilder sbOut = new StringBuilder(sIn.Length);
            foreach (char ch in sIn)
            {
                if (Char.IsControl(ch) || ch == '\'')
                {
                    int ich = (int)ch;
                    sbOut.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                else if (ch == '\"' || ch == '\\' || ch == '/')
                {
                    sbOut.Append('\\');
                }
                sbOut.Append(ch);
            }
            return sbOut.ToString();
        }

        public static string ToJSON(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static string SliceSTR(string STR, string STR1, string STR2, int StartIndex)
        {
            try
            {

                int i1 = STR.IndexOf(STR1, StartIndex);
                if (i1 < 0) return ""; else i1 += 1;

                int i2 = STR.IndexOf(STR2, i1 + 1);
                if (i2 < 0) return "";

                return STR.Substring(i1, i2 - i1).Trim();
            }
            catch
            {
                return "";
            }
        }
    }
}
