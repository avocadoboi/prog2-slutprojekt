using System;

namespace tetris_backend {

/*
    A grid of tetris cells with fixed dimensions.
*/
public class TetrisBoard : IGrid<TetrisCell> {
    private TetrisCell[,] _cells;
    public TetrisCell[,] Cells { 
        get => _cells;
        set {
            if (_cells == null ||
                value.GetLength(0) == _cells.GetLength(0) &&
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
        Cells = new TetrisCell[size.X, size.Y];
    public TetrisBoard(TetrisBoard other) => 
        Cells = other.Cells.Clone() as TetrisCell[,];
}

interface IBoardListener {
    void HandleScored(BasicScoreGainData data);
    void HandleGameOver();
    void HandleGameStart();
}

/*
    Manages the main tetris game logic and sends signals about 
    scoring, game over etc.
*/
class BoardLogic {
    private IBoardListener _listener;
    
    private TetrisBoard _board;
    private TetrisBoard _dynamic_board;

    private Tetromino _next_tetromino = Tetrominoes.GetRandom();
    private Tetromino _active_tetromino = Tetrominoes.GetRandom();
    private Vec2i _tetromino_position;

    public TetrisBoard Board => _dynamic_board;

    public void Step() {
        _tetromino_position.Y += 1;
        _dynamic_board = new TetrisBoard(_board);
        _dynamic_board.Draw(_active_tetromino, _tetromino_position);        
    }

    public BoardLogic(Vec2i size, IBoardListener listener) => 
        (_board, _listener) = (new TetrisBoard(size), listener);
}

}
