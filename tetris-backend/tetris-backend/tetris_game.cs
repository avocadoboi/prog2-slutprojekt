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

public enum CellType {
    Empty, I, J, L, O, S, T, Z
}

}
