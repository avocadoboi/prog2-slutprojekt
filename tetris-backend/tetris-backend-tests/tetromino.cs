using System.Collections.Generic;
using Xunit;
using tetris_backend;

namespace tetris_backend_tests {

public class TetrominoTests
{
    [Fact]
    public void TetrominoInitialization()
    {
        // Transposed to [x, y] coordinates.
        var tetromino_i_0 = new TetrisCell[,] {
            {0, TetrisCell.I, 0, 0},
            {0, TetrisCell.I, 0, 0},
            {0, TetrisCell.I, 0, 0},
            {0, TetrisCell.I, 0, 0},
        };
        var tetromino_i_1 = new Tetromino(TetrisCell.I, new int[,] {
            {0, 0, 0, 0},
            {1, 1, 1, 1},
            {0, 0, 0, 0},
            {0, 0, 0, 0},
        });
        Assert.Equal(tetromino_i_0, tetromino_i_1.Cells);
    }

    public static IEnumerable<object[]> rotation_data = new Tetromino[][] {
        new Tetromino[] {
            new Tetromino(TetrisCell.Z, new int[,] {
                {1, 1, 0},
                {0, 1, 1},
                {0, 0, 0},
            }),
            new Tetromino(TetrisCell.Z, new int[,] {
                {0, 1, 0},
                {1, 1, 0},
                {1, 0, 0},
            }),
            new Tetromino(TetrisCell.Z, new int[,] {
                {0, 0, 1},
                {0, 1, 1},
                {0, 1, 0},
            })
        },
        new Tetromino[] {
            new Tetromino(TetrisCell.I, new int[,] {
                {0, 0, 0, 0},
                {1, 1, 1, 1},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            }),
            new Tetromino(TetrisCell.I, new int[,] {
                {0, 1, 0, 0},
                {0, 1, 0, 0},
                {0, 1, 0, 0},
                {0, 1, 0, 0},
            }),
            new Tetromino(TetrisCell.I, new int[,] {
                {0, 0, 1, 0},
                {0, 0, 1, 0},
                {0, 0, 1, 0},
                {0, 0, 1, 0},
            })
        }
    };

    [Theory]
    [MemberData(nameof(rotation_data))]
    public void TetrominoRotation(Tetromino original, Tetromino left_expected, Tetromino right_expected)
    {
        var left_actual = new Tetromino(original);
        left_actual.Rotate(Direction1D.Left);
        
        Assert.Equal(left_expected.Cells, left_actual.Cells);

        var right_actual = new Tetromino(original);
        right_actual.Rotate(Direction1D.Right);

        Assert.Equal(right_expected.Cells, right_actual.Cells);
    }
}

} // namespace tetris_backend_tests
