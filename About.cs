using System.Windows.Forms;

namespace DBManager
{
    internal partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://pipiscrew.com/");
        }
    }
}
