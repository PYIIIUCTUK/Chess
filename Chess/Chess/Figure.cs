using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public abstract class Figure
    {
        public int Ind { get; set; }
        public int Size { get; set; }

        public Figure(int ind, int size)
        {
            Ind = ind;
            Size = size;
        }
        public abstract void Draw(int x, int y, int size, int offsetX, int offsetY, Brush brush, PaintEventArgs e);
    }
}
