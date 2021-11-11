using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DBManager.DBASES;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using System.Reflection;
using System.Web.Script.Serialization;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.BZip2;

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
            SQLSERVERtunnel = 0x06,
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

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        public static string DecompressStringBZIP(string compressedText)
              {
                  //src - http://community.sharpdevelop.net/forums/t/11005.aspx
                  Byte[] bytes = Convert.FromBase64String(compressedText);
                  MemoryStream memStream = new MemoryStream(bytes);
                  BZip2InputStream gzipStream = new BZip2InputStream(memStream);
                  byte[] data = new byte[2048];
                  char[] chars = new char[2048]; // must be at least as big as 'data'
                  Decoder decoder = Encoding.UTF8.GetDecoder();
                  StringBuilder sb = new StringBuilder();

                  while (true)
                  {
                      int size = gzipStream.Read(data, 0, data.Length);
                      if (size == 0)
                          break;
                      int n = decoder.GetChars(data, 0, size, chars, 0);
                      sb.Append(chars, 0, n);
                  }
                  return sb.ToString();

        }

        public static string DecompressStringGZIP(string compressedText)
        {
            // src - https://stackoverflow.com/a/4080983
            byte[] gZipBuffer = Convert.FromBase64String(compressedText); 

            using (var mem = new MemoryStream())
            {
                mem.Write(new byte[] { 0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, 8);
                mem.Write(gZipBuffer, 0, gZipBuffer.Length);

                mem.Position = 0;

                using (var gzip = new GZipStream(mem, CompressionMode.Decompress))
                using (var reader = new StreamReader(gzip))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string Capitalize(this string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }

        public static int ToInt(this object value)
        {
            int number = 0;

            if (value != null)
                int.TryParse(value.ToString(), out number);

            return number;
        }

        public static string ToStrinX(this object value)
        {
            string retvalue = "";

            if (value != null)
                retvalue = value.ToString();

            return retvalue;
        }

        public static bool ToBool(this object value)
        {
            bool result = false;
            if (value != null)
                bool.TryParse(value.ToString(), out result);

            return result;
        }
    }
}
