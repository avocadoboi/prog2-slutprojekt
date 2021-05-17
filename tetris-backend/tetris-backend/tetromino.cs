using System.Linq;
using System;

namespace TetrisBackend
{
	public enum TetrisCell
	{
		Empty, I, J, L, O, S, T, Z, Wall
	}

	/*
		A tetromino is a square grid of tetris cells that can be constructed 
		conveniently from a grid of cell flags and a cell type.
	*/
	public class Tetromino : SquareGrid<TetrisCell>
	{
		public TetrisCell Type { get; private set; }

		/*
			Initializes the tetromino by replacing zeros in cellFlags
			with empty tetris cells and ones with the provided tetris cell type.
		*/
		public Tetromino(TetrisCell type, int[,] cellFlags)
		{
			Type = type;
			Cells = Utils.CreateRectangularArray<TetrisCell>(new Vec2i(cellFlags.GetLength(1), cellFlags.GetLength(0)));

			foreach (var pos in Cells.Indices2D())
			{
				Cells[pos.Y][pos.X] = cellFlags[pos.Y, pos.X] == 0 ? TetrisCell.Empty : type;
			}
		}
		/*
			Copy constructor.
		*/
		public Tetromino(Tetromino other) =>
			(Type, Cells) = (other.Type, other.Cells.Select(row => row.ToArray()).ToArray());
	}

	static class Tetrominoes
	{
		public static Tetromino GetRandom()
		{
			return tetrominoes[new Random().Next(tetrominoes.Length)];
		}

		public static Tetromino[] tetrominoes = 
		{
			new Tetromino(TetrisCell.I, new int[,] {
				{0, 0, 0, 0},
				{1, 1, 1, 1},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
			}),
			new Tetromino(TetrisCell.J, new int[,] {
				{1, 0, 0},
				{1, 1, 1},
				{0, 0, 0},
			}),
			new Tetromino(TetrisCell.L, new int[,] {
				{0, 0, 1},
				{1, 1, 1},
				{0, 0, 0},
			}),
			new Tetromino(TetrisCell.O, new int[,] {
				{1, 1},
				{1, 1},
			}),
			new Tetromino(TetrisCell.S, new int[,] {
				{0, 1, 1},
				{1, 1, 0},
				{0, 0, 0},
			}),
			new Tetromino(TetrisCell.T, new int[,] {
				{0, 1, 0},
				{1, 1, 1},
				{0, 0, 0},
			}),
			new Tetromino(TetrisCell.Z, new int[,] {
				{1, 1, 0},
				{0, 1, 1},
				{0, 0, 0},
			})
		};
	}
}
