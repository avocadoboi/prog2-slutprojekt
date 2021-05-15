using System.Collections.Generic;
using Xunit;
using tetris_backend;

namespace tetris_backend_tests {

public class BoardTests {
    [Fact]
    public void BoardDrawing() {
        var game = new TetrisGame(new Vec2i(10, 20), new PlayerScoreFile("helo.txt"));
        
        var board = new TetrisBoard(new Vec2i(6, 12));
        board.Draw(new Tetromino(TetrisCell.L, new int[,] {
            {0, 0, 1},
            {1, 1, 1},
            {0, 0, 0},
        }), new Vec2i(2, 5));
        board.Draw(new Tetromino(TetrisCell.T, new int[,] {
            {0, 1, 0},
            {1, 1, 1},
            {0, 0, 0},
        }), new Vec2i(1, 4));

        // I'm sorry
        var expected_board = new TetrisCell[12][] {
            new TetrisCell[6]{0, 0,            0,            0,            0,            0},
            new TetrisCell[6]{0, 0,            0,            0,            0,            0},
            new TetrisCell[6]{0, 0,            0,            0,            0,            0},
            new TetrisCell[6]{0, 0,            0,            0,            0,            0},
            new TetrisCell[6]{0, 0,            TetrisCell.T, 0,            0,            0},
            new TetrisCell[6]{0, TetrisCell.T, TetrisCell.T, TetrisCell.T, TetrisCell.L, 0},
            new TetrisCell[6]{0, 0,            TetrisCell.L, TetrisCell.L, TetrisCell.L, 0},
            new TetrisCell[6]{0, 0,            0,            0,            0,            0},
            new TetrisCell[6]{0, 0,            0,            0,            0,            0},
            new TetrisCell[6]{0, 0,            0,            0,            0,            0},
            new TetrisCell[6]{0, 0,            0,            0,            0,            0},
            new TetrisCell[6]{0, 0,            0,            0,            0,            0},
            // new TetrisCell[6]{0, 0, 0, 0, 0,            0,            0,            0, 0, 0, 0, 0},
            // new TetrisCell[6]{0, 0, 0, 0, 0,            TetrisCell.T, 0,            0, 0, 0, 0, 0},
            // new TetrisCell[6]{0, 0, 0, 0, TetrisCell.T, TetrisCell.T, TetrisCell.L, 0, 0, 0, 0, 0},
            // new TetrisCell[6]{0, 0, 0, 0, 0,            TetrisCell.T, TetrisCell.L, 0, 0, 0, 0, 0},
            // new TetrisCell[6]{0, 0, 0, 0, 0,            TetrisCell.L, TetrisCell.L, 0, 0, 0, 0, 0},
            // new TetrisCell[6]{0, 0, 0, 0, 0,            0,            0,            0, 0, 0, 0, 0},
        };
        Assert.Equal(expected_board, board.Cells);
    }
}

} // namespace tetris_backend_tests