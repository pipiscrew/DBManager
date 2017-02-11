using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DBManager
{
    public partial class frmGenerateDesigner : BlueForm
    {
        private Dictionary<string, string> fields;
        public frmGenerateDesigner()
        {
            InitializeComponent();
        }

        public frmGenerateDesigner(Dictionary<string, string> fields)
        {
            InitializeComponent();

            this.fields = fields;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbLabel.Text.Length == 0 || cmbTextbox.Text.Length == 0 || cmbDateTime.Text.Length == 0)
            {
                MessageBox.Show("Please enter templates!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int res = 0;

            if (textBox3.Text.Length == 0)
            {
                MessageBox.Show("Please enter a TABINDEX!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {

                int.TryParse(textBox3.Text, out res);
            }

            string preview="";

            if (textBox1.Text.Length > 0)
            {
            //    MessageBox.Show("Please enter a file!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //else
            //{
                //if (cmbParent.Text.Length == 0)
                //{
                //    MessageBox.Show("Please enter a parent!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                
                preview = File.ReadAllText(textBox1.Text);

                foreach (DataGridViewRow item in DG.Rows)
                {
                    
                    if (preview.IndexOf("lbl" + item.Cells[0].Value.ToString() + ";") > -1)
                    {
                        MessageBox.Show("lbl" + item.Cells[0].Value.ToString() + "\r\n\r\nAlready exists @ form designer.\r\n\r\nOperation aborted!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (preview.IndexOf("txt" + item.Cells[0].Value.ToString() + ";") > -1)
                    {
                        MessageBox.Show("txt" + item.Cells[0].Value.ToString() + "\r\n\r\nAlready exists @ form designer\r\n\r\nOperation aborted!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                button6.Enabled = true;
            }

            ////cmbParent.Items.Clear();

            ////Regex reg;
            ////MatchCollection match;

            ////reg = new Regex("this.(.*?).Controls");
            ////match = reg.Matches(preview);

            ////if (match != null)
            ////{
            ////    foreach (Match item in match)
            ////    {
            ////        if (!cmbParent.Items.Contains(item.Groups[1].Value))
            ////            cmbParent.Items.Add(item.Groups[1].Value);
            ////    }
               
            ////}

            List<string> Init = new List<string>();
            List<string> Parent = new List<string>();
            List<string> Properties = new List<string>();
            List<string> Declare = new List<string>();

            string InitItems = "";
            string ParentItems = "";
            string PropertiesItems = "";
            string DeclareItems = "";


            string InitItemsLBLtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbLabel.Text + "\\Init.txt");
            string ParentItemsLBLtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbLabel.Text + "\\Parent.txt");
            string PropertiesItemsLBLtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbLabel.Text + "\\Properties.txt");
            string DeclareItemsLBLtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbLabel.Text + "\\Declare.txt");

            string InitItemsTXTtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbTextbox.Text + "\\Init.txt");
            string ParentItemsTXTtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbTextbox.Text + "\\Parent.txt");
            string PropertiesItemsTXTtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbTextbox.Text + "\\Properties.txt");
            string DeclareItemsTXTtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbTextbox.Text + "\\Declare.txt");

            string InitItemsDTPtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbDateTime.Text + "\\Init.txt");
            string ParentItemsDTPtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbDateTime.Text + "\\Parent.txt");
            string PropertiesItemsDTPtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbDateTime.Text + "\\Properties.txt");
            string DeclareItemsDTPtemplate = File.ReadAllText(Application.StartupPath + "\\FormControls\\" + cmbDateTime.Text + "\\Declare.txt");

            int tabIndex = res;
            int top = 0;


            for (int i = 0; i < DG.Rows.Count; i++)
            {
                InitItems = InitItemsLBLtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString()) + "\r\n";
                ParentItems = ParentItemsLBLtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString()).Replace("{parent}", cmbParent.Text) + "\r\n";
                PropertiesItems = PropertiesItemsLBLtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString()).Replace("{top}", top.ToString()).Replace("{tabindex}", tabIndex.ToString()) + "\r\n";
                DeclareItems = DeclareItemsLBLtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString()) + "\r\n";

                if (DG.Rows[i].Cells[2].Value.ToString() == "textBox")
                {
                    InitItems += InitItemsTXTtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString());
                    ParentItems += ParentItemsTXTtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString()).Replace("{parent}", cmbParent.Text);

                    tabIndex += 1;

                    PropertiesItems += PropertiesItemsTXTtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString()).Replace("{top}", top.ToString()).Replace("{tabindex}",
                                                                                                    tabIndex.ToString()).Replace("{maxlength}", DG.Rows[i].Cells[1].Value.ToString());
                    DeclareItems += DeclareItemsTXTtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString());
                }
                else if (DG.Rows[i].Cells[2].Value.ToString() == "DateTime")
                {
                    InitItems += InitItemsDTPtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString());
                    ParentItems += ParentItemsDTPtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString()).Replace("{parent}", cmbParent.Text);

                    tabIndex += 1;

                    PropertiesItems += PropertiesItemsDTPtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString()).Replace("{top}", top.ToString()).Replace("{tabindex}",
                                                                                                    tabIndex.ToString()).Replace("{maxlength}", DG.Rows[i].Cells[1].Value.ToString());
                    DeclareItems += DeclareItemsDTPtemplate.Replace("{field}", DG.Rows[i].Cells[0].Value.ToString());
                }


                tabIndex += 1;




                Init.Add(InitItems);
                Parent.Add(ParentItems);
                Properties.Add(PropertiesItems);
                Declare.Add(DeclareItems);

                top += 28;
                tabIndex += 1;
            }

            txtInit.Lines = Init.ToArray();
            txtParent.Lines = Parent.ToArray();
            txtProperties.Lines = Properties.ToArray();
            txtDeclare.Lines = Declare.ToArray();

        }

        private void AddtoGrid(string fieldName, string size)
        {
            if (fieldName.ToLower().Contains("date") || fieldName.ToLower().Contains("dtp"))
                DG.Rows.Add(fieldName, size, "DateTime");
            else
                DG.Rows.Add(fieldName, size, "textBox");
        }

        private void frmGenerateDesigner_Load(object sender, EventArgs e)
        {
            if (fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in fields)
                {
                    if (kvp.Value.ToString().Length == 10)
                       AddtoGrid(kvp.Key.ToString(),"1");
                    else
                        AddtoGrid(kvp.Key.ToString(), kvp.Value.ToString());
                }
            }

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath + "\\FormControls");
            foreach (System.IO.DirectoryInfo f in dir.GetDirectories("*Label"))
            {
                cmbLabel.Items.Add(f.Name);
            }

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(Application.StartupPath + "\\FormControls");
            foreach (System.IO.DirectoryInfo f in dir2.GetDirectories("*Textbox"))
            {
                cmbTextbox.Items.Add(f.Name);
            }

            System.IO.DirectoryInfo dir3 = new System.IO.DirectoryInfo(Application.StartupPath + "\\FormControls");
            foreach (System.IO.DirectoryInfo f in dir2.GetDirectories("*DateTime"))
            {
                cmbDateTime.Items.Add(f.Name);
            }


            System.IO.DirectoryInfo dir4 = new System.IO.DirectoryInfo(Application.StartupPath + "\\FormControls");
            foreach (System.IO.DirectoryInfo f in dir2.GetDirectories("*Combo"))
            {
                cmbCombo.Items.Add(f.Name);
            }

            cmbLabel.SelectedIndex = cmbTextbox.SelectedIndex = cmbDateTime.SelectedIndex = 0;

            txtInit.KeyPress += new KeyPressEventHandler(this.TextBox_KeyPress);
            txtParent.KeyPress += new KeyPressEventHandler(this.TextBox_KeyPress);
            txtProperties.KeyPress += new KeyPressEventHandler(this.TextBox_KeyPress);
            txtDeclare.KeyPress += new KeyPressEventHandler(this.TextBox_KeyPress);

        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard(txtInit.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard(txtParent.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard(txtProperties.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            General.Copy2Clipboard(txtDeclare.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text + ".bak"))
            {
                MessageBox.Show(textBox1.Text + ".bak\r\n\r\nAlready exists!.", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (cmbParent.Text.Length ==0)
            {
                MessageBox.Show("Please select parent!!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBox1.Text.Length == 0)
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "VS form designer (Designer.cs)|*.Designer.cs|All Files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox1.Text = openFileDialog1.FileName;
                else
                {
                    textBox1.Text = "";
                    return;
                }
            }



            //Console.WriteLine(openFileDialog1.FileName);//Do what you want here
            CultureInfo culture = new CultureInfo("el-GR");
            int codePage = culture.TextInfo.ANSICodePage;
            Encoding GreekEncoding = Encoding.GetEncoding(codePage);

            string DES = File.ReadAllText(textBox1.Text, GreekEncoding);
            string InitSearch = "private void InitializeComponent()\r\n        {";
            string PropertiesSearch = "this.SuspendLayout();";
            string ParentSearch = cmbParent.Text + "\r\n            //";
            string OUT = "";



            int Init = DES.IndexOf(InitSearch);
            if (Init > 0)
                OUT = DES.Insert(Init + InitSearch.Length, "\r\n" + txtInit.Text);
            else
            {
                MessageBox.Show("ERROR - Couldnt find Init in Designer!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }



            int Properties = OUT.IndexOf(PropertiesSearch);
            if (Properties > 0)
                OUT = OUT.Insert(Properties + PropertiesSearch.Length, "\r\n" + txtProperties.Text);
            else
            {
                MessageBox.Show("ERROR - Couldnt find Properties in Designer!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            int Parent = OUT.IndexOf(cmbParent.Text, Properties);
            if (Parent < Properties)
            {
                MessageBox.Show("ERROR - Conflict parent cant be before Properties in Designer!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                OUT = OUT.Insert(Parent + ParentSearch.Length, "\r\n" + txtParent.Text);
            }



            int Declare = OUT.LastIndexOf(";");
            if (Declare > 0)
                OUT = OUT.Insert(Declare + 1, "\r\n" + txtDeclare.Text);
            else
            {
                MessageBox.Show("ERROR - Couldnt find Properties in Designer!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }



            File.Copy(textBox1.Text, textBox1.Text + ".bak");

            File.WriteAllText(textBox1.Text, OUT);

            MessageBox.Show("Written! Please switch to VS now!", General.apTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList[0].ToLower().EndsWith(".designer.cs"))
            {
                textBox1.Text = FileList[0];

                string preview = File.ReadAllText(textBox1.Text);
                cmbParent.Items.Clear();

                Regex reg;
                MatchCollection match;

                reg = new Regex("this.(.*?).Controls");
                match = reg.Matches(preview);

                if (match != null)
                {
                    foreach (Match item in match)
                    {
                        if (!cmbParent.Items.Contains(item.Groups[1].Value))
                            cmbParent.Items.Add(item.Groups[1].Value);
                    }
                }

                cmbParent.DroppedDown = true;
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void lst_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //string userInput = lst.SelectedItems[0].Text;
            //if (General.InputBox(General.apTitle, "enter new field name :", ref  userInput) == System.Windows.Forms.DialogResult.OK)
            //    lst.SelectedItems[0].Text = userInput;
        }

        private void DG_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DG.BeginEdit(false);
            if (e.ColumnIndex == 2)// the combobox column index
            {
                if (this.DG.EditingControl != null
                    && this.DG.EditingControl is ComboBox)
                {
                    ComboBox cmb = this.DG.EditingControl as ComboBox;
                    cmb.DroppedDown = true;
                }
            }
        }
    }
}
