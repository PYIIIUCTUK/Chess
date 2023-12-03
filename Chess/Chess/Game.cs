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
    public partial class Game : Form
    {
        Menu menu;

        int S, W, H;
        int offsetX;
        int offsetY;

        Player curPlayer;
        Cell curCell = null;
        List<Cell> freeCells = new List<Cell>();

        List<Player> players = new List<Player>();
        List<List<Cell>> map = new List<List<Cell>>();

        public Game(Menu Menu, int w, int h, List<Player> Players, List<List<Cell>> Map)
        {
            InitializeComponent();

            menu = Menu;
            W = w;
            H = h;

            players = Players;
            map = Map;
        }

        private void Game_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.Sizable;

            S = Math.Min((ClientSize.Height - 50) / H, ClientSize.Width / W);
            offsetX = (ClientSize.Width - S * W) / 2;
            offsetY = ((ClientSize.Height) - S * H);

            curPlayer = players[0];
        }
        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            menu.Show();
        }

        private void Game_Paint(object sender, PaintEventArgs e)
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
                                  S, -6, 7, players[map[i][j].Figure.Ind - 1].Brush, e);
                    }
                }
            }

            for (int i = 0; i < freeCells.Count; i++)
            {
                e.Graphics.FillEllipse(Brushes.DarkRed, freeCells[i].X * S + offsetX + S / 3,
                                       freeCells[i].Y * S + offsetY + S / 3, S / 3, S / 3);
            }
            if (curCell != null)
            {
                Pen tmpPen = new Pen(Brushes.DarkBlue, 5);
                e.Graphics.DrawEllipse(tmpPen, curCell.X * S + offsetX,
                                       curCell.Y * S + offsetY, S, S);
            }

            if (curPlayer.Ind == 1)
            {
                e.Graphics.DrawString($"Ход Белых", new Font("Times New Roman", 36, FontStyle.Bold),
                      Brushes.Black, new PointF((W / 2 - 2) * S + offsetX + S / 2, 0));
            }
            else
            {
                e.Graphics.DrawString($"Ход Черных", new Font("Times New Roman", 36, FontStyle.Bold),
                   Brushes.Black, new PointF((W / 2 - 2) * S + offsetX + S / 4, 0));
            }
        }

        private bool IsWithinBoard(int x, int y)
        {
            return (x >= 0 && x < W && y >= 0 && y < H);
        }
        private void Game_MouseClick(object sender, MouseEventArgs e)
        {
            int x = (e.X - offsetX);
            int y = (e.Y - offsetY);

            x /= S;
            y /= S;
            if (!IsWithinBoard(x, y)) { return; }

            bool isEnd = false;

            if (e.Button == MouseButtons.Left)
            {
                if (map[y][x].Figure == null || map[y][x].Figure.Ind != curPlayer.Ind)
                {
                    curCell = null;
                    freeCells.Clear();
                }
                else
                {
                    freeCells.Clear();
                    curCell = map[y][x];
                    FindFreeCells();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (CheckClickFree(x, y))
                {
                    if (map[y][x].Figure != null)
                    {
                        if ((map[y][x].Figure as King) != null)
                        {
                            isEnd = true;
                        }
                        players[2 - curCell.Figure.Ind].CountFigure--;
                    }

                    if ((curCell.Figure as Pawn) != null)
                    {
                        if ((curCell.Figure as Pawn).FirstStep)
                        {
                            (curCell.Figure as Pawn).FirstStep = false;
                            if (Math.Abs(curCell.Y - y) == 2)
                            {
                                int koefY_1 = -1;
                                if (y - curCell.Y > 0)
                                {
                                    koefY_1 = 1;
                                }

                                curPlayer.DeadCell.Add(map[curCell.Y + koefY_1][curCell.X]);
                                curPlayer.DeadCell.Add(map[y][x]);
                            }
                        }

                        if (players[2 - curPlayer.Ind].DeadCell.Count > 0 &&
                            players[2 - curPlayer.Ind].DeadCell[0].X == x &&
                            players[2 - curPlayer.Ind].DeadCell[0].Y == y)
                        {
                            players[2 - curPlayer.Ind].DeadCell[1].Figure = null;
                        }
                        else if (curPlayer.Ind == 1 && y == 0)
                        {
                            GameChooseFigure gameChooseFigure = new GameChooseFigure(curPlayer,
                                curCell.Figure.Size, offsetY);
                            if (gameChooseFigure.ShowDialog() == DialogResult.Yes)
                            {
                                curCell.Figure = gameChooseFigure.curFigure;
                            }
                        }
                        else if (curPlayer.Ind == 2 && y == H - 1)
                        {
                            GameChooseFigure gameChooseFigure = new GameChooseFigure(curPlayer,
                                curCell.Figure.Size, offsetY);
                            if (gameChooseFigure.ShowDialog() == DialogResult.Yes)
                            {
                                curCell.Figure = gameChooseFigure.curFigure;
                            }
                        }
                    }
                    else if ((curCell.Figure as King) != null)
                    {
                        if ((curCell.Figure as King).FirstStep)
                        {
                            (curCell.Figure as King).FirstStep = false;

                            if (Math.Abs(x - curCell.X) == 2)
                            {
                                if (x - curCell.X < 0)
                                {
                                    map[curCell.Y][3].Figure = map[curCell.Y][0].Figure;
                                    map[curCell.Y][0].Figure = null;
                                }
                                else
                                {
                                    map[curCell.Y][5].Figure = map[curCell.Y][W - 1].Figure;
                                    map[curCell.Y][W - 1].Figure = null;
                                }
                            }
                        }
                    }
                    else if ((curCell.Figure as Rook) != null)
                    {
                        if ((curCell.Figure as Rook).FirstStep)
                        {
                            (curCell.Figure as Rook).FirstStep = false;
                        }
                    }

                    map[y][x].Figure = curCell.Figure;
                    map[curCell.Y][curCell.X].Figure = null;

                    ChangeTurn();
                }
            }

            Invalidate();
            if (isEnd || CheckWin())
            {
                if (curPlayer.Ind == 1)
                {
                    MessageBox.Show("Победил Черный!!!");
                }
                else
                {
                    MessageBox.Show("Победил Белый!!!");
                }

                Close();
            }
        }

        void ChangeTurn()
        {
            curPlayer = players[2 - curPlayer.Ind];
            curPlayer.DeadCell.Clear();

            freeCells.Clear();
            curCell = null;
        }
        bool CheckWin()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (curPlayer.CountFigure <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        bool CheckClickFree(int x, int y)
        {
            for (int i = 0; i < freeCells.Count; i++)
            {
                if (x == freeCells[i].X && y == freeCells[i].Y)
                {
                    return true;
                }
            }
            return false;
        }

        void FindFreeCells()
        {
            switch (curCell.Figure)
            {
                case Pawn figure:
                {
                    if (curPlayer.Ind == 1)
                    {
                        if (map[curCell.Y - 1][curCell.X].Figure == null)
                        {
                            freeCells.Add(map[curCell.Y - 1][curCell.X]);

                            if (figure.FirstStep && map[curCell.Y - 2][curCell.X].Figure == null)
                            {
                                freeCells.Add(map[curCell.Y - 2][curCell.X]);
                            }
                        }

                        FindFreePawn(curCell.X - 1, curCell.Y - 1);
                        FindFreePawn(curCell.X + 1, curCell.Y - 1);
                    }
                    else
                    {
                        if (map[curCell.Y + 1][curCell.X].Figure == null)
                        {
                            freeCells.Add(map[curCell.Y + 1][curCell.X]);

                            if (figure.FirstStep && map[curCell.Y + 2][curCell.X].Figure == null)
                            {
                                freeCells.Add(map[curCell.Y + 2][curCell.X]);
                            }
                        }

                        FindFreePawn(curCell.X - 1, curCell.Y + 1);
                        FindFreePawn(curCell.X + 1, curCell.Y + 1);
                    }
                    break;
                }
                case Rook figure:
                {
                    FindFreeRook(curCell.X, curCell.Y, 0, -1);
                    FindFreeRook(curCell.X, curCell.Y, 0, 1);

                    FindFreeRook(curCell.X, curCell.Y, -1, 0);
                    FindFreeRook(curCell.X, curCell.Y, 1, 0);
                    break;
                }
                case Knight figure:
                {
                    FindFreeKnight(curCell.X, curCell.Y, 1, 2);
                    FindFreeKnight(curCell.X, curCell.Y, 1, -2);

                    FindFreeKnight(curCell.X, curCell.Y, 2, 1);
                    FindFreeKnight(curCell.X, curCell.Y, -2, 1);
                    break;
                }
                case Bishop figure:
                {
                    FindFreeRook(curCell.X, curCell.Y, -1, -1);
                    FindFreeRook(curCell.X, curCell.Y, 1, -1);

                    FindFreeRook(curCell.X, curCell.Y, 1, 1);
                    FindFreeRook(curCell.X, curCell.Y, -1, 1);
                    break;
                }
                case Queen figure:
                {
                    FindFreeRook(curCell.X, curCell.Y, 0, -1);
                    FindFreeRook(curCell.X, curCell.Y, 0, 1);

                    FindFreeRook(curCell.X, curCell.Y, -1, 0);
                    FindFreeRook(curCell.X, curCell.Y, 1, 0);

                    FindFreeRook(curCell.X, curCell.Y, -1, -1);
                    FindFreeRook(curCell.X, curCell.Y, 1, -1);

                    FindFreeRook(curCell.X, curCell.Y, 1, 1);
                    FindFreeRook(curCell.X, curCell.Y, -1, 1);

                    break;
                }
                case King figure:
                {
                    FindFreeKing(curCell.X, curCell.Y);

                    if (figure.FirstStep)
                    {
                        FindFreeCastling(curCell.X, curCell.Y, -1);
                        FindFreeCastling(curCell.X, curCell.Y, 1);
                    }
                    break;
                }
            }
        }
        void FindFreePawn(int x, int y)
        {
            if (IsWithinBoard(x, y))
            {
                if ((map[y][x].Figure != null && map[y][x].Figure.Ind != curPlayer.Ind) ||
                    (players[2 - curPlayer.Ind].DeadCell.Count > 0 &&
                     players[2 - curPlayer.Ind].DeadCell[0].X == x &&
                     players[2 - curPlayer.Ind].DeadCell[0].Y == y))
                {
                    freeCells.Add(map[y][x]);
                }
            }
        }
        void FindFreeRook(int x, int y, int koefX, int koefY)
        {
            int kX = koefX;
            int kY = koefY;
            while (IsWithinBoard(x + kX, y + kY))
            {
                if (map[y + kY][x + kX].Figure == null)
                {
                    freeCells.Add(map[y + kY][x + kX]);
                    kY += koefY;
                    kX += koefX;
                    continue;
                }
                else if (map[y + kY][x + kX].Figure.Ind != curPlayer.Ind)
                {
                    freeCells.Add(map[y + kY][x + kX]);
                    return;
                }
                return;
            }
        }
        void FindFreeKnight(int x, int y, int koefX, int koefY)
        {
            if (IsWithinBoard(x - koefX, y + koefY))
            {
                if (map[y + koefY][x - koefX].Figure == null ||
                    map[y + koefY][x - koefX].Figure.Ind != curPlayer.Ind)
                {
                    freeCells.Add(map[y + koefY][x - koefX]);
                }
            }

            if (IsWithinBoard(x + koefX, y - koefY))
            {
                if (map[y - koefY][x + koefX].Figure == null ||
                    map[y - koefY][x + koefX].Figure.Ind != curPlayer.Ind)
                {
                    freeCells.Add(map[y - koefY][x + koefX]);
                }
            }
        }
        void FindFreeKing(int x, int y)
        {
            for (int i = y-1; i <= y+1; i++)
            {
                for (int j = x-1; j <= x+1; j++)
                {
                    if (IsWithinBoard(j, i))
                    {
                        if (map[i][j].Figure == null ||
                            map[i][j].Figure.Ind != curPlayer.Ind)
                        {
                            freeCells.Add(map[i][j]);
                        }
                    }
                }
            }
        }

        void FindFreeCastling(int x, int y, int koefX)
        {
            int kX = koefX;
            while (IsWithinBoard(x + kX, y))
            {
                if (map[y][x + kX].Figure == null)
                {
                    kX += koefX;
                    continue;
                }
                else if (map[y][x + kX].Figure.Ind == curPlayer.Ind)
                {
                    if ((map[y][x + kX].Figure as Rook) != null &&
                        (map[y][x + kX].Figure as Rook).FirstStep)
                    {
                        freeCells.Add(map[y][x + koefX * 2]);
                        return;
                    }
                }
                return;
            }
            return;
        }
    }
}
