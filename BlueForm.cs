using System.Windows.Forms;
using System.Drawing;

namespace DBManager
{
    public class BlueForm : Form
    {
        public BlueForm()
        {
            // Makes sure the form repaints when it was resized
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Getting the graphics object
            Graphics g = pevent.Graphics;

            // Creating the rectangle for the gradient
            Rectangle rBackground = new Rectangle(0, 0, this.Width, this.Height);

            Color Color1 = Color.FromArgb(191, 219, 255);
            Color Color2 = Color.FromArgb(164, 195, 234);
            // Creating the lineargradient
            System.Drawing.Drawing2D.LinearGradientBrush bBackground = new System.Drawing.Drawing2D.LinearGradientBrush(rBackground, Color1, Color2, System.Drawing.Drawing2D.LinearGradientMode.Vertical);

            // Draw the gradient onto the form
            g.FillRectangle(bBackground, rBackground);

            // Disposing of the resources held by the brush
            bBackground.Dispose();
        }

        public void TextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x1')
            {
                ((TextBox)sender).SelectAll();
                e.Handled = true;
            }
        }
    }
}
