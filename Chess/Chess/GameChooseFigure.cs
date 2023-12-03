using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class GameChooseFigure : Form
    {
        Player curPlayer = null;
        public Figure curFigure { get; set; } = null;
        Point curPoint = new Point(-1, -1);

        int S;
        int W = 4;
        int offsetY;

        public GameChooseFigure(Player player, int s, int OffsetY)
        {
            InitializeComponent();

            S = s;
            offsetY = OffsetY;
            curPlayer = player;
        }

        private void GameChooseFigure_Shown(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.Sizable;

            ClientSize = new Size(S * 4 + 2, S + offsetY + 2);
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void GameChooseFigure_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (curFigure == null)
            {
                e.Cancel = true;
            }
            else
            {
                DialogResult = DialogResult.Yes;
            }
        }

        private void GameChooseFigure_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString($"Выбор фигуры", new Font("Times New Roman", 30, FontStyle.Bold),
                      Brushes.Black, new PointF(0, 0));

            Pen pBlack = new Pen(Brushes.Black, 3);
            for (int j = 0; j < W; j++)
            {
                e.Graphics.FillRectangle(Brushes.DarkGray, j * S, offsetY, S, S);
                e.Graphics.DrawRectangle(pBlack, j * S, offsetY, S, S);

                switch (j)
                {
                    case 0:
                    {
                        Bishop figure = new Bishop(curPlayer.Ind, S);
                        figure.Draw(j * S, offsetY, S, -11, 5,
                                    curPlayer.Brush, e);
                        break;
                    }
                    case 1:
                    {
                        Knight figure = new Knight(curPlayer.Ind, S);
                        figure.Draw(j * S, offsetY, S, -11, 5,
                                    curPlayer.Brush, e);
                        break;
                    }
                    case 2:
                    {
                        Rook figure = new Rook(curPlayer.Ind, S);
                        figure.Draw(j * S, offsetY, S, -11, 5,
                                    curPlayer.Brush, e);
                        break;
                    }
                    case 3:
                    {
                        Queen figure = new Queen(curPlayer.Ind, S);
                        figure.Draw(j * S, offsetY, S, -11, 5,
                                     curPlayer.Brush, e);
                        break;
                    }
                }
            }

            if (curFigure != null)
            {
                Pen pBlue = new Pen(Brushes.DarkBlue, 5);
                e.Graphics.DrawEllipse(pBlue, curPoint.X * S, offsetY, S, S);
            }
        }

        private void GameChooseFigure_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = (e.X);
                int y = (e.Y - offsetY);
                if (x < 0 || x >= W * S || y < 0 || y >= offsetY) { return; }

                x /= S;

                switch (x)
                {
                    case 0:
                    {
                        curFigure = new Bishop(curPlayer.Ind, S);
                        break;
                    }
                    case 1:
                    {
                        curFigure = new Knight(curPlayer.Ind, S);
                        break;
                    }
                    case 2:
                    {
                        curFigure = new Rook(curPlayer.Ind, S);
                        break;
                    }
                    case 3:
                    {
                        curFigure = new Queen(curPlayer.Ind, S);
                        break;
                    }
                }
                curPoint.X = x;
                Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                Close();
            }
        }
    }
}
