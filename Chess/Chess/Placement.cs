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
    public partial class Placement : Form
    {
        Menu menu;
        bool Auto;

        int S, W, H;
        int offsetX;
        int offsetY;

        Player curPlayer;
        Figure curFigure = null;
        int typeFigure = -1;
        int countFigure = 0;
        Point mouse = new Point(-1, -1);

        List<Player> players = new List<Player>();
        List<List<Cell>> map = new List<List<Cell>>();

        public Placement(Menu Menu, int w, int h, bool auto = true)
        {
            InitializeComponent();

            menu = Menu;
            Auto = auto;
            W = w;
            H = h;
        }

        private void Placement_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.Sizable;

            S = Math.Min((ClientSize.Height - 50) / (H + 1), ClientSize.Width / W);
            offsetX = (ClientSize.Width - S * W) / 2;
            offsetY = ((ClientSize.Height) - S * (H + 1));

            for (int i = 0; i < H; i++)
            {
                List<Cell> line = new List<Cell>();
                for (int j = 0; j < W; j++)
                {
                    line.Add(new Cell(j, i));
                }
                map.Add(line);
            }

            players.Add(new Player(1, Brushes.White, Pens.White, W * 2));
            players.Add(new Player(2, Brushes.Black, Pens.Black, W * 2));

            curPlayer = players[0];

            if (Auto)
            {
                AutoPlacement();
            }
        }
        private void Placement_FormClosed(object sender, FormClosedEventArgs e)
        {
            Game game = new Game(menu, W, H, players, map);
            game.Show();
        }

        private void Placement_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Brushes.Black, 3);

            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if ((j + i) % 2 == 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Gray, j * S + offsetX, i * S + offsetY, S, S);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.DarkSlateGray, j * S + offsetX, i * S + offsetY, S, S);
                    }
                    e.Graphics.DrawRectangle(pen, j * S + offsetX, i * S + offsetY, S, S);

                    if (map[i][j].Figure != null)
                    {
                        map[i][j].Figure.Draw(j * S + offsetX, i * S + offsetY,
                                  S, -11, 5, players[map[i][j].Figure.Ind - 1].Brush, e);
                    }
                }
            }

            for (int j = 1; j < 7; j++)
            {
                e.Graphics.FillRectangle(Brushes.DarkGray, j * S + offsetX, H * S + offsetY, S, S);
                e.Graphics.DrawRectangle(pen, j * S + offsetX, H * S + offsetY, S, S);
                switch (j)
                {
                    case 1:
                    {
                        Pawn figure = new Pawn(curPlayer.Ind, S);
                        figure.Draw(j * S + offsetX, H * S + offsetY, S, -11, 5,
                                  curPlayer.Brush, e);
                        break;
                    }
                    case 2:
                    {
                        Bishop figure = new Bishop(curPlayer.Ind, S);
                        figure.Draw(j * S + offsetX, H * S + offsetY, S, -11, 5,
                                  curPlayer.Brush, e);
                        break;
                    }
                    case 3:
                    {
                        Knight figure = new Knight(curPlayer.Ind, S);
                        figure.Draw(j * S + offsetX, H * S + offsetY, S, -11, 5,
                                  curPlayer.Brush, e);
                        break;
                    }
                    case 4:
                    {
                        Rook figure = new Rook(curPlayer.Ind, S);
                        figure.Draw(j * S + offsetX, H * S + offsetY, S, -11, 5,
                                  curPlayer.Brush, e);
                        break;
                    }
                    case 5:
                    {
                        Queen figure = new Queen(curPlayer.Ind, S);
                        figure.Draw(j * S + offsetX, H * S + offsetY, S, -11, 5,
                                  curPlayer.Brush, e);
                        break;
                    }
                    case 6:
                    {
                        King figure = new King(curPlayer.Ind, S);
                        figure.Draw(j * S + offsetX, H * S + offsetY, S, -11, 5,
                                  curPlayer.Brush, e);
                        break;
                    }
                }
            }
            if (typeFigure != -1)
            {
                Pen pBlue = new Pen(Brushes.DarkBlue, 5);
                e.Graphics.DrawEllipse(pBlue, typeFigure * S + offsetX, H * S + offsetY, S, S);
            }


            if (mouse.X >= 0 && mouse.Y >= 0 && curFigure != null)
            {
                curFigure.Draw(mouse.X * S + offsetX, mouse.Y * S + offsetY, S, -11, 5,
                               curPlayer.Brush, e);
            }

            if (curPlayer.Ind == 1)
            {
                e.Graphics.DrawString($"Ход Белых", new Font("Times New Roman", 36, FontStyle.Bold),
                      Brushes.Black, new PointF((W / 2 - 2) * S + offsetX, 0));
            }
            else
            {
                e.Graphics.DrawString($"Ход Черных", new Font("Times New Roman", 36, FontStyle.Bold),
                   Brushes.Black, new PointF((W / 2 - 2) * S + offsetX, 0));
            }
        }

        private void Placement_MouseClick(object sender, MouseEventArgs e)
        {
            int x = (e.X - offsetX);
            int y = (e.Y - offsetY);
            if (x < 0 || x >= W * S || y < 0 || y >= (H + 1) * S) { return; }

            x /= S;
            y /= S;

            if (y == H && x >= 1 && x < 7)
            {
                switch (x)
                {
                    case 1:
                    {
                        curFigure = new Pawn(curPlayer.Ind, S);
                        typeFigure = 1;
                        break;
                    }
                    case 2:
                    {
                        curFigure = new Bishop(curPlayer.Ind, S);
                        typeFigure = 2;
                        break;
                    }
                    case 3:
                    {
                        curFigure = new Knight(curPlayer.Ind, S);
                        typeFigure = 3;
                        break;
                    }
                    case 4:
                    {
                        curFigure = new Rook(curPlayer.Ind, S);
                        typeFigure = 4;
                        break;
                    }
                    case 5:
                    {
                        curFigure = new Queen(curPlayer.Ind, S);
                        typeFigure = 5;
                        break;
                    }
                    case 6:
                    {
                        curFigure = new King(curPlayer.Ind, S);
                        typeFigure = 6;
                        break;
                    }
                }
            }
            else if ((curPlayer.Ind == 1 && x >= 0 && x < W && y >= H - 2 && y < H) ||
                    (curPlayer.Ind == 2 && x >= 0 && x < W && y >= 0 && y < 2))
            {
                if (curFigure != null && map[y][x].Figure == null)
                {
                    map[y][x].Figure = Copy();
                    countFigure++;
                }
            }

            if (countFigure == curPlayer.CountFigure)
            {
                ChangeTurn();
            }
            Invalidate();
        }

        private void Placement_MouseMove(object sender, MouseEventArgs e)
        {
            int x = (e.X - offsetX);
            int y = (e.Y - offsetY);

            if (x == mouse.X && y == mouse.Y) { return; }

            if (x < 0 || x >= W * S || y < 0 || y >= H * S)
            { mouse.X = -1; mouse.Y = -1; Invalidate(); return; }

            x /= S;
            y /= S;

            mouse.X = x;
            mouse.Y = y;
            Invalidate();
        }

        private Figure Copy()
        {
            switch (typeFigure)
            {
                case 1:
                {
                    return new Pawn(curPlayer.Ind, S);
                }
                case 2:
                {
                    return new Bishop(curPlayer.Ind, S);
                }
                case 3:
                {
                    return new Knight(curPlayer.Ind, S);
                }
                case 4:
                {
                    return new Rook(curPlayer.Ind, S);
                }
                case 5:
                {
                    return new Queen(curPlayer.Ind, S);
                }
                case 6:
                {
                    return new King(curPlayer.Ind, S);
                }
            }
            return null;
        }

        private void ChangeTurn()
        {
            if (curFigure.Ind == 2)
            {
                Close();
            }

            curPlayer = players[1];
            countFigure = 0;
            curFigure = null;
            typeFigure = -1;
        }

        private void AutoPlacement()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    if (i == 1)
                    {
                        map[i][j].Figure = new Pawn(2, S);
                        map[H - 1 - i][j].Figure = new Pawn(1, S);
                    }
                    else
                    {
                        if (j == 0 || j == W - 1)
                        {
                            map[i][j].Figure = new Rook(2, S);
                            map[H - 1 - i][j].Figure = new Rook(1, S);
                        }
                        else if (j == 1 || j == W - 2)
                        {
                            map[i][j].Figure = new Knight(2, S);
                            map[H - 1 - i][j].Figure = new Knight(1, S);
                        }
                        else if (j == 2 || j == W - 3)
                        {
                            map[i][j].Figure = new Bishop(2, S);
                            map[H - 1 - i][j].Figure = new Bishop(1, S);
                        }
                        else if (j == 3)
                        {
                            map[i][j].Figure = new Queen(2, S);
                            map[H - 1 - i][j].Figure = new Queen(1, S);
                        }
                        else
                        {
                            map[i][j].Figure = new King(2, S);
                            map[H - 1 - i][j].Figure = new King(1, S);
                        }
                    }
                }
            }
            Close();
        }
    }
}
