using System;
using System.Collections.Generic;
using System.Linq;

namespace tetris_backend
{

	/*
		A grid of tetris cells with fixed dimensions.
	*/
	public class TetrisBoard : IGrid<TetrisCell>
	{
		private TetrisCell[][] _cells;
		public TetrisCell[][] Cells
		{
			get => _cells;
			set
			{
				if (_cells == null ||
					value.Length == Size.Y && 
					value.GetWidth() == Size.X)
				{
					_cells = value;
				}
				else
				{
					throw new ArgumentException("A tetris board was assigned a grid with wrong dimensions!");
				}
			}
		}
		public Vec2i Size { get; }
		// public Vec2i Size { get; init; }

		/*
			Searches for full rows on the board and returns their indices.
		*/
		public int[] FindFullRows()
		{
			return _cells.Where((row, i) => row.All(x => x != TetrisCell.Empty)).Select((row, i) => i).ToArray();
		}

		/*
			Removes all full tetris rows, packing the remaining rows downwards.
		*/
		public void ClearFullRows()
		{
			var size = Size;
			
			// The index of the row to copy from.
			var copy_row = size.Y - 1;

			for (var row = copy_row; row >= 0; --row, --copy_row)
			{
				// Can't copy from a row that doesn't exist, so we fill with empty cells.
				if (copy_row < 0)
				{
					Array.Fill(_cells[row], TetrisCell.Empty);
				}
				else 
				{
					// Move the row to copy from up one step if it is a full row.
					// This effectively removes it.
					if (_cells[copy_row].All(x => x != TetrisCell.Empty))
					{
						--copy_row;
					}
					// If it is the same row then the copy is a no-op.
					if (copy_row != row)
					{
						Array.Copy(_cells[copy_row], _cells[row], size.X);
					}
				}
			}
		}

		public bool IntersectsTetromino(Tetromino tetromino, Vec2i offset) 
		{
			return tetromino.Cells.Indices().Any(pos =>
                tetromino.Cells[pos.Y][pos.X] != TetrisCell.Empty && 
				// If any non-empty cell within the tetromino is outside the bounds of the board then it intersects.
                (!new Vec2i(pos.X + offset.X, pos.Y + offset.Y).GetIsWithin(Size) ||
				// If two non-empty cells overlap then the tetromino intersects the board.
                Cells[pos.Y + offset.Y][pos.X + offset.X] != TetrisCell.Empty));
		}

		public TetrisBoard(Vec2i size) =>
			(Cells, Size) = (Utils.CreateRectangularArray<TetrisCell>(size), size);

		public TetrisBoard(TetrisBoard other) =>
			(Cells, Size) = (other.Cells.Select(row => row.ToArray()).ToArray(), other.Size);
	}

	interface IBoardObserver
	{
		void HandleRowsCleared(int[] row_indices);
		void HandleGameOver();
		void HandleGameStart();
		void HandleNewTetromino(Tetromino new_tetromino, Tetromino next_tetromino);
	}

	/*
		Manages the main tetris game logic and sends signals about 
		scoring, game over etc.
	*/
	class BoardLogic
	{
		private IBoardObserver _observer;

		private TetrisBoard _board;
		private TetrisBoard _dynamic_board;

		private Tetromino _next_tetromino = Tetrominoes.GetRandom();
		private Tetromino _active_tetromino;
		private Vec2i _tetromino_position;

		public TetrisBoard Board => _dynamic_board;

		public void Step()
		{
			_tetromino_position.Y += 1;

			if (_board.IntersectsTetromino(_active_tetromino, _tetromino_position))
			{
				_tetromino_position.Y -= 1;

				_PlaceTetromino();
			}

			_dynamic_board = new TetrisBoard(_board);
			_dynamic_board.Draw(_active_tetromino, _tetromino_position);
		}

		private void _PlaceTetromino()
		{
			_board.Draw(_active_tetromino, _tetromino_position);

			var full_row_indices = _board.FindFullRows();
			if (full_row_indices.Length != 0)
			{
				_board.ClearFullRows();

				_observer.HandleRowsCleared(full_row_indices);
			}

			_SpawnTetromino();
		}

		private void _SpawnTetromino()
		{
			_active_tetromino = _next_tetromino;
			_next_tetromino = Tetrominoes.GetRandom();
			_tetromino_position = new Vec2i(new Random().Next(_board.Size.X - _active_tetromino.Width), 0);
			_observer.HandleNewTetromino(_active_tetromino, _next_tetromino);
		}

		public void Restart()
		{
			_board = new TetrisBoard(_board.Size);
			_observer.HandleGameStart();
		}

		public BoardLogic(Vec2i size, IBoardObserver listener)
		{
			_board = new TetrisBoard(size);
			_dynamic_board = new TetrisBoard(size);
			_observer = listener;

			_SpawnTetromino();
			_observer.HandleGameStart();
		}
	}

}
