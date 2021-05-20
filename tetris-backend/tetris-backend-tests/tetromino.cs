using System.Collections.Generic;
using Xunit;
using TetrisBackend;

namespace TetrisBackendTests 
{
	public class TetrominoTests
	{
		[Fact]
		public void TetrominoInitialization()
		{
			// Transposed to [x, y] coordinates.
			var tetromino_i_0 = new TetrisCell[][] {
				new TetrisCell[]{0, 0, 0, 0},
				new TetrisCell[]{TetrisCell.I, TetrisCell.I, TetrisCell.I, TetrisCell.I},
				new TetrisCell[]{0, 0, 0, 0},
				new TetrisCell[]{0, 0, 0, 0},
			};
			var tetromino_i_1 = new Tetromino(TetrisCell.I, new int[,] {
				{0, 0, 0, 0},
				{1, 1, 1, 1},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
			});
			Assert.Equal(tetromino_i_0, tetromino_i_1.Cells);
		}

		public static IEnumerable<object[]> rotationData = new Tetromino[][] {
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
		[MemberData(nameof(rotationData))]
		public void TetrominoRotation(Tetromino original, Tetromino leftExpected, Tetromino rightExpected)
		{
			var leftActual = new Tetromino(original);
			leftActual.Rotate(Direction1D.Left);
			
			Assert.Equal(leftExpected.Cells, leftActual.Cells);

			var rightActual = new Tetromino(original);
			rightActual.Rotate(Direction1D.Right);

			Assert.Equal(rightExpected.Cells, rightActual.Cells);
		}
	}
}
