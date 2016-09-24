using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectPrefire
{
    class Drawer
    {

        Graphics gfx { get; set; }
        Brush brushRed { get; set; }
        Brush brushBlue { get; set; }
        Font smallFont = new Font("Terminal", 7f, FontStyle.Regular);



        public Drawer(Control canvas)
        {

            gfx = canvas.CreateGraphics();
            //Pen redPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            brushBlue = Brushes.Blue;
            brushRed = Brushes.Red;
 
        }
        
        public void drawCircle(float x, float y, Color color)
        {

            SolidBrush brush = new SolidBrush(color);

            gfx.FillEllipse(brush, (int)x - 3 , (int)y- 3, 6, 6);
            brush.Dispose();
        }

        public void drawLine(float x1, float y1, float x2, float y2, Color color)
        {

            Pen pen = new Pen(color);
            gfx.DrawLine(pen, x1, y1, x2, y2);
            pen.Dispose();
        }

        public void drawString(string text, int x, int y, Color color)
        {
            SolidBrush brush = new SolidBrush(color);
            SolidBrush brushBlack = new SolidBrush(Color.Black);
            gfx.DrawString(text, smallFont, brushBlack, new Point(x - 1, y - 1));
            gfx.DrawString(text, smallFont, brush, new Point(x, y));
            brush.Dispose();
            brushBlack.Dispose();
        }

        public void drawRectangle(int x, int y, int w, int h, Color color)
        {
            drawLine(x, y, x + w, y, color);
            drawLine(x + w, y, x + w, y + h, color);
            drawLine(x + w, y + h, x, y + h, color);
            drawLine(x, y + h, x, y, color);
            
        }
        public void clearCanvas()
        {
            gfx.Clear(Color.Empty);
        }
    }
}
