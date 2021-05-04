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
    public TetrisCell[,] Cells { get; private set; }

    public TetrisBoard(int width, int height) =>
        Cells = new TetrisCell[width, height];
    public TetrisBoard(TetrisBoard other) => 
        Cells = other.Cells.Clone() as TetrisCell[,];
}

// public class Game {
    // private 
// }

} // namespace tetris_backend
