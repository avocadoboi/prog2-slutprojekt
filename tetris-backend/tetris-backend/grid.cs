using System;
using System.Linq;

namespace TetrisBackend
{
	public interface IGrid<T>
	{
		// I changed from a 2D array to a jagged array because it turned out that rows are very important in Tetris.
		// It made the algorithms simpler. The drawback is that the array is assumed to be rectangular.
		// Different rows could technically be given different lengths.
		// The Cells array is row-first, meaning the first index specifies the row (Y-axis) and the second index specifies column (X-axis).
		T[][] Cells { get; }
	}

	public enum Direction1D
	{
		Left, Right
	}

	/*
		A grid that holds the invariant of being square.
		It also provides functionality that is specific to square grids.
	*/
	public class SquareGrid<T> : IGrid<T>
	{
		private T[][] _cells;

		public T[][] Cells
		{
			get => _cells;
			set
			{
				// The array is jagged, so different lines could have different lengths,
				// but it's not worth checking every row so we're assuming it's at least rectangular.
				if (value.Length > 0 && value.Length == value[0].Length)
				{
					_cells = value;
				}
				else
				{
					throw new ArgumentException("A square grid was assigned a non-square array.");
				}
			}
		}
		public int Width => _cells.Length;

		/*
			Rotates the square grid 90 degrees clockwise or counter-clockwise.
			Direction1D.Left is counter-clockwise and Direction1D.Right is clockwise.
		*/
		public void Rotate(Direction1D direction)
		{
			// Iterate each position within the top-left quadrant of the square grid.
			// If the width is odd then we have to include the middle column.
			foreach (var quadrantCellPosition in Utils.Range2D(new Vec2i(Width / 2 + (Width & 1), Width / 2)))
			{
				var (x, y) = (quadrantCellPosition.X, quadrantCellPosition.Y);

				// Below, a single cell in each quadrant is moved to its new rotated position.
				// (x, y) is the quadrant-relative position of the cell to be rotated.

				// The first cell that is replaced.
				var first = _cells[y][x];
				
				if (direction == Direction1D.Right)
				{
					_cells[y            ][x            ] = _cells[Width - 1 - x][y            ];
					_cells[Width - 1 - x][y            ] = _cells[Width - 1 - y][Width - 1 - x];
					_cells[Width - 1 - y][Width - 1 - x] = _cells[x            ][Width - 1 - y];
					_cells[x            ][Width - 1 - y] = first;
				}
				else
				{
					_cells[y            ][x            ] = _cells[x            ][Width - 1 - y];
					_cells[x            ][Width - 1 - y] = _cells[Width - 1 - y][Width - 1 - x];
					_cells[Width - 1 - y][Width - 1 - x] = _cells[Width - 1 - x][y            ];
					_cells[Width - 1 - x][y            ] = first;
				}
			}
		}
	}

	public static class GridExtensions
	{
		/*
			"draws" a grid of tetris cells onto this grid at a certain position (defined by the top-left corner of the grid to draw).
			This means that non-empty cells in "toDraw" are set on this grid at the respective coordinates, offset by "position".
			Cells that happen to be placed outside the bounds of the grid are ignored.
		*/
		public static void Draw(this IGrid<TetrisCell> board, IGrid<TetrisCell> toDraw, Vec2i position)
		{
			foreach (var (x, y) in toDraw.Cells.Indices2D())
			{
				if (toDraw.Cells[y][x] != TetrisCell.Empty &&
					new Vec2i(x + position.X, y + position.Y).GetIsWithin(board.Cells.GetSize()))
				{
					board.Cells[y + position.Y][x + position.X] = toDraw.Cells[y][x];
				}
			}
		}
	}
}
