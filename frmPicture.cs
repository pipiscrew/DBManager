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
    public partial class frmPicture : Form
    {
        public frmPicture(Bitmap pic)
        {
            InitializeComponent();
            pictureBox1.Image = pic;

            this.Height = this.pictureBox1.Height + 50;
            this.Width = this.pictureBox1.Width + 6;

            this.CenterToParent();

        }
    }
}
