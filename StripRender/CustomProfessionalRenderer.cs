using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DBManager.StripRender
{



    internal class CustomProfessionalRenderer : ToolStripProfessionalRenderer
    {

        public CustomProfessionalRenderer(ProfessionalColorTable colorTable)
            : base(colorTable)
        {
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Enabled)
            {
                LinearGradientBrush brush = null;
                if (e.Item.IsOnDropDown)
                {
                    if (e.Item.IsOnDropDown && e.Item.Selected)
                    {
                        Rectangle rect = new Rectangle(4, 2, e.Item.Width - 7, e.Item.Height - 3);
                        brush = new LinearGradientBrush(rect, this.ColorTable.MenuItemSelectedGradientBegin, this.ColorTable.MenuItemSelectedGradientEnd, LinearGradientMode.Vertical);
                        e.Graphics.FillRectangle(brush, rect);
                        CustomProfessionalHelper.smethod_1(e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width + 1, rect.Height + 1, 6, this.ColorTable.MenuItemBorder);
                    }
                }
                else if (!((ToolStripMenuItem)e.Item).DropDown.Visible)
                {
                    if (e.Item.Selected)
                    {
                        Rectangle rectangle2 = new Rectangle(2, 2, e.Item.Width - 4, e.Item.Height - 4);
                        brush = new LinearGradientBrush(rectangle2, this.ColorTable.MenuItemSelectedGradientBegin, this.ColorTable.MenuItemSelectedGradientEnd, LinearGradientMode.Vertical);
                        e.Graphics.FillRectangle(brush, rectangle2);
                        CustomProfessionalHelper.smethod_1(e.Graphics, rectangle2.Left - 1, rectangle2.Top - 1, rectangle2.Width + 1, rectangle2.Height + 1, 4, this.ColorTable.MenuItemBorder);
                        CustomProfessionalHelper.smethod_1(e.Graphics, rectangle2.Left - 2, rectangle2.Top - 2, rectangle2.Width + 3, rectangle2.Height + 3, 4, Color.White);
                    }
                }
                else
                {
                    Rectangle rectangle = new Rectangle(2, 2, e.Item.Width - 4, e.Item.Height - 4);
                    brush = new LinearGradientBrush(rectangle, this.ColorTable.MenuItemPressedGradientBegin, this.ColorTable.MenuItemPressedGradientEnd, LinearGradientMode.Vertical);
                    e.Graphics.FillRectangle(brush, rectangle);
                    CustomProfessionalHelper.smethod_1(e.Graphics, rectangle.Left - 1, rectangle.Top - 1, rectangle.Width + 1, rectangle.Height + 1, 4, this.ColorTable.MenuItemBorder);
                    CustomProfessionalHelper.smethod_1(e.Graphics, rectangle.Left - 2, rectangle.Top - 2, rectangle.Width + 3, rectangle.Height + 3, 4, Color.White);
                }
                if (brush != null)
                {
                    brush.Dispose();
                }
            }
        }


    }

}
