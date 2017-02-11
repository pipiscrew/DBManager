using System.Drawing;
using System.Windows.Forms;

namespace DBManager.StripRender
{
    internal class CustomProfessionalColors : ProfessionalColorTable
    {
        
         // Fields
    public Color color_0;
    public Color color_1;
    public Color color_10;
    public Color color_11;
    public Color color_12;
    public Color color_13;
    public Color color_14;
    public Color color_15;
    public Color color_16;
    public Color color_17;
    public Color color_18;
    public Color color_19;
    public Color color_2;
    public Color color_20;
    public Color color_21;
    public Color color_22;
    public Color color_23;
    public Color color_24;
    public Color color_3;
    public Color color_4;
    public Color color_5;
    public Color color_6;
    public Color color_7;
    public Color color_8;
    public Color color_9;

    // Methods
    public CustomProfessionalColors()
    {
        this.color_0 = base.GripDark;
        this.color_1 = base.GripLight;
        this.color_2 = base.ToolStripBorder;
        this.color_3 = base.ToolStripGradientBegin;
        this.color_4 = base.ToolStripGradientMiddle;
        this.color_5 = base.ToolStripGradientEnd;
        this.color_6 = base.SeparatorDark;
        this.color_7 = base.SeparatorLight;
        this.color_8 = base.OverflowButtonGradientMiddle;
        this.color_9 = base.OverflowButtonGradientBegin;
        this.color_10 = base.OverflowButtonGradientEnd;
        this.color_11 = base.ToolStripPanelGradientBegin;
        this.color_12 = base.ToolStripPanelGradientEnd;
        this.color_13 = base.ToolStripDropDownBackground;
        this.color_14 = base.MenuItemBorder;
        this.color_15 = base.MenuItemSelectedGradientBegin;
        this.color_16 = base.MenuItemSelectedGradientEnd;
        this.color_17 = base.MenuBorder;
        this.color_18 = base.ImageMarginGradientBegin;
        this.color_19 = base.ImageMarginGradientMiddle;
        this.color_20 = base.ImageMarginGradientEnd;
        this.color_21 = base.MenuStripGradientBegin;
        this.color_22 = base.MenuStripGradientEnd;
        this.color_23 = base.ButtonSelectedBorder;
        this.color_24 = base.ButtonSelectedHighlightBorder;
    }

    // Properties
    public override Color ButtonSelectedBorder
    {
        get
        {
            return this.color_23;
        }
    }

    public override Color ButtonSelectedHighlightBorder
    {
        get
        {
            return this.color_24;
        }
    }

    public override Color GripDark
    {
        get
        {
            return this.color_0;
        }
    }

    public override Color GripLight
    {
        get
        {
            return this.color_1;
        }
    }

    public override Color ImageMarginGradientBegin
    {
        get
        {
            return this.color_18;
        }
    }

    public override Color ImageMarginGradientEnd
    {
        get
        {
            return this.color_20;
        }
    }

    public override Color ImageMarginGradientMiddle
    {
        get
        {
            return this.color_19;
        }
    }

    public override Color MenuBorder
    {
        get
        {
            return this.color_17;
        }
    }

    public override Color MenuItemBorder
    {
        get
        {
            return this.color_14;
        }
    }

    public override Color MenuItemSelectedGradientBegin
    {
        get
        {
            return this.color_15;
        }
    }

    public override Color MenuItemSelectedGradientEnd
    {
        get
        {
            return this.color_16;
        }
    }

    public override Color MenuStripGradientBegin
    {
        get
        {
            return this.color_21;
        }
    }

    public override Color MenuStripGradientEnd
    {
        get
        {
            return this.color_22;
        }
    }

    public override Color OverflowButtonGradientBegin
    {
        get
        {
            return this.color_9;
        }
    }

    public override Color OverflowButtonGradientEnd
    {
        get
        {
            return this.color_10;
        }
    }

    public override Color OverflowButtonGradientMiddle
    {
        get
        {
            return this.color_8;
        }
    }

    public override Color SeparatorDark
    {
        get
        {
            return this.color_6;
        }
    }

    public override Color SeparatorLight
    {
        get
        {
            return this.color_7;
        }
    }

    public override Color ToolStripBorder
    {
        get
        {
            return this.color_2;
        }
    }

    public override Color ToolStripDropDownBackground
    {
        get
        {
            return this.color_13;
        }
    }

    public override Color ToolStripGradientBegin
    {
        get
        {
            return this.color_3;
        }
    }

    public override Color ToolStripGradientEnd
    {
        get
        {
            return this.color_5;
        }
    }

    public override Color ToolStripGradientMiddle
    {
        get
        {
            return this.color_4;
        }
    }

    public override Color ToolStripPanelGradientBegin
    {
        get
        {
            return this.color_11;
        }
    }

    public override Color ToolStripPanelGradientEnd
    {
        get
        {
            return this.color_12;
        }
    }
}

}