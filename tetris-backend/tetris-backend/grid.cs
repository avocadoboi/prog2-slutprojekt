using System;
using System.Linq;

namespace tetris_backend
{
    public interface IGrid<T>
    {
        T[][] Cells { get; }
    }

    public enum Direction1D
    {
        Left, Right
    }

    /*
        A grid that holds the invariant of being square.
        It also provides functionality that is specific to square grids.
    */
    public class SquareGrid<T> : IGrid<T>
    {
        private T[][] _cells;

        public T[][] Cells
        {
            get => _cells;
            set
            {
                // The array is jagged, so different rows could have different lengths,
                // but it's not worth checking every row so we're assuming it's rectangular.
                if (value.Length > 0 && value.Length == value[0].Length)
                {
                    _cells = value;
                }
                else
                {
                    throw new ArgumentException("A square grid was assigned a non-square array.");
                }
            }
        }
        public int Width => _cells.Length;

        /*
            Rotates the square grid 90 degrees clockwise or counter-clockwise.
            Direction1D.Left is counter-clockwise and Direction1D.Right is clockwise.
        */
        public void Rotate(Direction1D direction)
        {
            foreach (var pos in Utils.Range2D(new Vec2i(Width / 2 + (Width & 1), Width / 2)))
            {
                var (x, y) = (pos.X, pos.Y);
                var first = _cells[y][x];
                if (direction == Direction1D.Right)
                {
                    _cells[y][x] = _cells[Width - 1 - x][y];
                    _cells[Width - 1 - x][y] = _cells[Width - 1 - y][Width - 1 - x];
                    _cells[Width - 1 - y][Width - 1 - x] = _cells[x][Width - 1 - y];
                    _cells[x][Width - 1 - y] = first;
                }
                else
                {
                    _cells[y][x] = _cells[x][Width - 1 - y];
                    _cells[x][Width - 1 - y] = _cells[Width - 1 - y][Width - 1 - x];
                    _cells[Width - 1 - y][Width - 1 - x] = _cells[Width - 1 - x][y];
                    _cells[Width - 1 - x][y] = first;
                }
            }
        }
    }

    public static class GridExtensions
    {
        public static void Draw(this IGrid<TetrisCell> board, IGrid<TetrisCell> to_draw, Vec2i position)
        {
            foreach (var pos in to_draw.Cells.Indices())
            {
                if (to_draw.Cells[pos.Y][pos.X] != TetrisCell.Empty &&
                    new Vec2i(pos.X + position.X, pos.Y + position.Y).GetIsWithin(board.Cells.GetSize()))
                {
                    board.Cells[pos.Y + position.Y][pos.X + position.X] = to_draw.Cells[pos.Y][pos.X];
                }
            }
        }

        // public static bool OverlapsWith(this IGrid<TetrisCell> board, IGrid<TetrisCell> other, Vec2i offset)
        // {
        //     return other.Cells.Indices().Any(pos =>
        //         new Vec2i(pos.X + offset.X, pos.Y + offset.Y).GetIsWithin(board.Cells.GetSize()) &&
        //         other.Cells[pos.Y][pos.X] != TetrisCell.Empty &&
        //         board.Cells[pos.Y + offset.Y][pos.X + offset.X] != TetrisCell.Empty);
        // }
    }

}
