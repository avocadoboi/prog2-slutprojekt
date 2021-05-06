using System;
using System.Collections.Generic;

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

public enum GameState {
    Running,
    Over,
}   

public readonly struct CurrentScore {
    public int Points { get; init; }
    public int Rows { get; init; }
}

public struct ScoreKeeper {
    private int _points;
    private int _rows;

    public CurrentScore Score => new CurrentScore{Points = _points, Rows = _rows};
    
    public void ClearRows(int count, int level) {
        _rows += count;
        
        // According to https://tetris.fandom.com/wiki/Scoring.
        _points += count switch {
            0 => 0,
            1 => 40,
            2 => 100,
            3 => 300,
            4 => 1200,
            _ => throw new ArgumentOutOfRangeException(nameof(count), $"Only 1-4 rows can be cleared at once in Tetris.")
        }*(level + 1);
    }
}

public interface IGameListener {
    // void Scored()
}

public class TetrisAPI {
    private ScoreKeeper _score_keeper;
    private ScoreList<PlayerScore> _score_list;
    
    private TetrisBoard _board;
    private TetrisBoard _dynamic_board;

    private Tetromino _active_tetromino;
    private Vec2i _tetromino_position;

    private GameState _state = GameState.Running;
    
    public TetrisBoard Board => _dynamic_board;
    public GameState State => _state;
    public List<PlayerScore> ScoreList => _score_list.Scores;
    public CurrentScore Score => _score_keeper.Score;
    
    public void GiveInput(TetrisInput input) {

    }
    public void Step() {
        _tetromino_position.Y += 1;
        _dynamic_board = new TetrisBoard(_board);
        _dynamic_board.Draw(_active_tetromino, _tetromino_position);        
    }
    public void SaveScore(string player_name) {

    }

}

} // namespace tetris_backend
