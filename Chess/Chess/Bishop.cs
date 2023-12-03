using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    class Bishop : Figure
    {
        static Font font;
        static string sumbol = "\u265D";

        public Bishop(int ind, int size) : base(ind, size)
        {
            font = new Font("Arial", size / 1.5f, FontStyle.Regular);
        }

        public override void Draw(int x, int y, int size, int offsetX, int offsetY, Brush brush, PaintEventArgs e)
        {
            x += offsetX;
            y += offsetY;
            e.Graphics.DrawString(sumbol, font, brush, x, y);
        }
    }
}
