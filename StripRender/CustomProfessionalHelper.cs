using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DBManager.StripRender
{
    internal sealed class CustomProfessionalHelper
        {
            // Methods
            public static void smethod_0(Graphics graphics_0, int int_0, int int_1, int int_2, int int_3, int int_4, Color color_0)
            {
                using (SolidBrush brush = new SolidBrush(color_0))
                {
                    RectangleF rect = new RectangleF((float)int_0, (float)int_1, (float)int_2, (float)int_3);
                    graphics_0.FillRectangle(brush, rect);
                }
                smethod_1(graphics_0, int_0, int_1, int_2, int_3, int_4, Color.FromArgb(color_0.R - 15, color_0.G - 15, color_0.B - 15));
            }

            public static void smethod_1(Graphics graphics_0, int int_0, int int_1, int int_2, int int_3, int int_4, Color color_0)
            {
                using (Pen pen = new Pen(color_0))
                {
                    graphics_0.SmoothingMode = SmoothingMode.HighQuality;
                    RectangleF ef = new RectangleF((float)int_0, (float)int_1, (float)int_2, (float)int_3);
                    SizeF size = new SizeF((float)int_4, (float)int_4);
                    RectangleF rect = new RectangleF(ef.Location, size);
                    graphics_0.DrawArc(pen, rect, 180f, 90f);
                    graphics_0.DrawLine(pen, int_0 + ((int)Math.Round((double)(((double)int_4) / 2.0))), int_1, (int_0 + int_2) - ((int)Math.Round((double)(((double)int_4) / 2.0))), int_1);
                    rect.X = ef.Right - int_4;
                    graphics_0.DrawArc(pen, rect, 270f, 90f);
                    graphics_0.DrawLine(pen, (int)(int_0 + int_2), (int)(int_1 + ((int)Math.Round((double)(((double)int_4) / 2.0)))), (int)(int_0 + int_2), (int)((int_1 + int_3) - ((int)Math.Round((double)(((double)int_4) / 2.0)))));
                    rect.Y = ef.Bottom - int_4;
                    graphics_0.DrawArc(pen, rect, 0f, 90f);
                    graphics_0.DrawLine(pen, (int)(int_0 + ((int)Math.Round((double)(((double)int_4) / 2.0)))), (int)(int_1 + int_3), (int)((int_0 + int_2) - ((int)Math.Round((double)(((double)int_4) / 2.0)))), (int)(int_1 + int_3));
                    rect.X = ef.Left;
                    graphics_0.DrawArc(pen, rect, 90f, 90f);
                    graphics_0.DrawLine(pen, int_0, int_1 + ((int)Math.Round((double)(((double)int_4) / 2.0))), int_0, (int_1 + int_3) - ((int)Math.Round((double)(((double)int_4) / 2.0))));
                }
            }

            internal static Color smethod_9(Color color_0, Color color_1, int int_8)
            {
                Color color = Color.FromArgb(255, color_0);
                float num=0;
                byte b=0;
                float num2=0;
                float num4=0;
                float num5=0;
                float num6=0;
                float num7=0;
                float num8=0;
                float num9=0;
                byte green = 0;
                byte b2=0;
                if ((uint)num + (uint)b > 4294967295u)
                {
                    if (((uint)num2 & 0u) != 0u)
                    {
                        goto IL_13A;
                    }
                }
                else
                {
                //IL_27:
                    Color color2 = Color.FromArgb(255, color_1);
                    float num3 = (float)color.R;
                    num4 = (float)color.G;
                    do
                    {
                        num5 = (float)color.B;
                        num6 = (float)color2.R;
                        num7 = (float)color2.G;
                        num8 = (float)color2.B;
                    }
                    while ((uint)num9 > 4294967295u);
                    if ((uint)num9 - (uint)num6 <= 4294967295u)
                    {
                        num = num3 * (float)int_8 / 255f + num6 * (float)((double)checked(255 - int_8) / 255.0);
                        if ((uint)num2 - (uint)num3 > 4294967295u)
                        {
                            goto IL_E1;
                        }
                        b2 = checked((byte)Math.Round((double)num));
                    }
                }
                num9 = num4 * (float)int_8 / 255f + num7 * (float)((double)checked(255 - int_8) / 255.0);
            IL_E1:
                if ((uint)num6 - (uint)b2 < 0u)
                {
                   // goto IL_27;
                }
                
                green=checked((byte)Math.Round((double)num9));
            IL_FB:
                if (((uint)num & 0u) == 0u)
                {
                    goto IL_104;
                }
            IL_104:
                num2 = num5 * (float)int_8 / 255f + num8 * (float)((double)checked(255 - int_8) / 255.0);
                b = checked((byte)Math.Round((double)num2));
                if (((uint)num4 & 0u) != 0u)
                {
                    goto IL_147;
                }
            IL_13A:
                if ((uint)num5 - (uint)num5 > 4294967295u)
                {
                    goto IL_FB;
                }
            IL_147:
                return Color.FromArgb(255, (int)b2, (int)green, (int)b);
            }


 

        }


    }
