using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.JScript;

namespace DBManager
{
    public partial class frmJScustom : BlueForm
    {
        string tbls = null;
        public frmJScustom(List<treeItem2> tbl)
        {
            InitializeComponent();

            //convert 'c# listbox' items to json and store it to 'q javascript variable'
            textBox2.Text = "var tbls = " + General.ToJSON(tbl) + ";";

            textEditorControl1.Document.HighlightingStrategy = ICSharpCode.TextEditor.Document.HighlightingStrategyFactory.CreateHighlightingStrategy("JavaScript");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            textBox1.Text = "";

            string script = textBox2.Text + textEditorControl1.Text;
            try
            {
                Object Result = Microsoft.JScript.Eval.JScriptEvaluate(script, Microsoft.JScript.Vsa.VsaEngine.CreateEngine());

                if (Result.GetType().Name == "ConcatString" || Result.GetType().Name == "String")
                {
                    textBox1.Text = Result.ToString();
                }
                else if (Result.GetType().Name == "ArrayObject")
                {
                    ArrayObject obj = Result as ArrayObject;

                    for (int i = 0; i < int.Parse(obj.length.ToString()); i++)
                    {
                        textBox1.Text += obj[i];

                    }
                }
                else
                {
                    MessageBox.Show("Type is " + Result.GetType().Name);
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message + " \r\n\r\n***" + ex.StackTrace;
            }
        }
    }
}
