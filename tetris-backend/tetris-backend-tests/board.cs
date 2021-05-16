using System.Collections.Generic;
using Xunit;
using TetrisBackend;

namespace TetrisBackendTests
{
    public class BoardTests
    {
        [Fact]
        public void BoardDrawing()
        {
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
			};
            Assert.Equal(expected_board, board.Cells);
        }
        [Fact]
        public void ClearRows()
        {
            var board = new TetrisBoard(new Vec2i(3, 10));
			board.Cells = new TetrisCell[10][] {
                new TetrisCell[3]{TetrisCell.Z, TetrisCell.Z, 0           },
                new TetrisCell[3]{TetrisCell.J, TetrisCell.Z, TetrisCell.Z},
                new TetrisCell[3]{TetrisCell.J, TetrisCell.J, TetrisCell.J},
                new TetrisCell[3]{TetrisCell.L, TetrisCell.L, TetrisCell.L},
                new TetrisCell[3]{TetrisCell.L, 0,            TetrisCell.Z},
                new TetrisCell[3]{0,            TetrisCell.Z, TetrisCell.Z},
                new TetrisCell[3]{TetrisCell.J, TetrisCell.Z, 0           },
                new TetrisCell[3]{TetrisCell.J, TetrisCell.J, TetrisCell.J},
                new TetrisCell[3]{TetrisCell.T, TetrisCell.T, TetrisCell.T},
                new TetrisCell[3]{0,            TetrisCell.T, 0           },
			};

			var expectedBoard = new TetrisCell[10][] {
				new TetrisCell[3]{0,            0,            0           },
				new TetrisCell[3]{0,            0,            0           },
				new TetrisCell[3]{0,            0,            0           },
				new TetrisCell[3]{0,            0,            0           },
				new TetrisCell[3]{0,            0,            0           },
                new TetrisCell[3]{TetrisCell.Z, TetrisCell.Z, 0           },
                new TetrisCell[3]{TetrisCell.L, 0,            TetrisCell.Z},
                new TetrisCell[3]{0,            TetrisCell.Z, TetrisCell.Z},
                new TetrisCell[3]{TetrisCell.J, TetrisCell.Z, 0           },
                new TetrisCell[3]{0,            TetrisCell.T, 0           },
			};

			var fullRows = board.FindFullRows();
			var expectedFullRows = new int[]{1, 2, 3, 7, 8};
			Assert.Equal(expectedFullRows, fullRows);
			
			board.ClearFullRows();
			Assert.Equal(expectedBoard, board.Cells);
        }
    }
}