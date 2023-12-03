using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Player
    {
        public int Ind { get; set; }
        public Brush Brush { get; set; }
        public Pen Pen { get; set; }

        public int CountFigure { get; set; }
        public List<Cell> DeadCell { get; set; } = new List<Cell>();

        public Player(int Ind, Brush Brush, Pen Pen, int count)
        {
            this.Ind = Ind;
            this.Brush = Brush;
            this.Pen = Pen;

            CountFigure = count;
        }
    }
}
