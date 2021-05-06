using System;

namespace tetris_backend {

public interface IGrid<T> {
    T[,] Cells { get; }
}

public enum Direction1D {
    Left, Right
}

/*
    A grid that holds the invariant of being square.
    It also provides functionality that is specific to square grids.
*/
public class SquareGrid<T> : IGrid<T> {
    private T[,] _cells;

    public T[,] Cells {
        get => _cells;
        set {
            if (value.GetLength(0) == value.GetLength(1)) {
                _cells = value;
            }
            else {
                throw new ArgumentException("A square grid was assigned a non-square array.");
            }
        }
    }
    public int Width => _cells.GetLength(0);

    /*
        Rotates the square grid 90 degrees clockwise or counter-clockwise.
        Direction1D.Left is counter-clockwise and Direction1D.Right is clockwise.
    */
    public void Rotate(Direction1D direction) {
        foreach (var (x, y) in Utils.Range2D(Width/2 + (Width & 1), Width/2)) {
            var first = _cells[x, y];
            if (direction == Direction1D.Right) {
                _cells[x, y] = _cells[y, Width - 1 - x];
                _cells[y, Width - 1 - x] = _cells[Width - 1 - x, Width - 1 - y];
                _cells[Width - 1 - x, Width - 1 - y] = _cells[Width - 1 - y, x];
                _cells[Width - 1 - y, x] = first;
            }
            else {
                _cells[x, y] = _cells[Width - 1 - y, x];
                _cells[Width - 1 - y, x] = _cells[Width - 1 - x, Width - 1 - y];
                _cells[Width - 1 - x, Width - 1 - y] = _cells[y, Width - 1 - x];
                _cells[y, Width - 1 - x] = first;
            }
        }
    }
}

public static class GridExtensions {
    public static void Draw(this IGrid<TetrisCell> board, IGrid<TetrisCell> to_draw, Vec2i position) 
    {
        foreach (var (x, y) in to_draw.Cells.Indices()) {
            if (to_draw.Cells[x, y] != TetrisCell.Empty) {
                board.Cells[x + position.X, y + position.Y] = to_draw.Cells[x, y];
            }
        }
    }
}

} // namespace tetris_backend
