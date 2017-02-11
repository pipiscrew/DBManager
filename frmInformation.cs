using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBManager
{
    public partial class frmInformation : BlueForm
    {
        public frmInformation(string val)
        {
            InitializeComponent();

            textBox1.Text = val;
        }
    }
}
