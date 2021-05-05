using System;

namespace tetris_backend {
    
public enum TetrisInput {
    Left,
    Right,
    RotateLeft,
    RotateRight,
    SwapTetromino,
    SkipFall,
    Restart
}

public enum TetrisCell {
    Empty, I, J, L, O, S, T, Z
}

public class TetrisBoard : IGrid<TetrisCell> {
    private TetrisCell[,] _cells;
    public TetrisCell[,] Cells { 
        get => _cells;
        set {
            if (value.GetLength(0) == _cells.GetLength(0) &&
                value.GetLength(1) == _cells.GetLength(1))
            {
                _cells = value;
            }
            else {
                throw new ArgumentException("A tetris board was assigned a grid with wrong dimensions!");
            }
        }
    }

    public TetrisBoard(Vec2i size) =>
        Cells = new TetrisCell[size.x, size.y];
    public TetrisBoard(TetrisBoard other) => 
        Cells = other.Cells.Clone() as TetrisCell[,];
}

public class TetrisGame {
    private TetrisBoard _board;
    private Tetromino _active_tetromino;

    public TetrisBoard Board {
        get; set;
    }
    

    // TetrisGame()
}

} // namespace tetris_backend
