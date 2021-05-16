using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisBackend
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
			return _cells.Select((row, i) => i).Where(i => _cells[i].All(x => x != TetrisCell.Empty)).ToArray();
		}

		/*
			Removes all full tetris rows, packing the remaining rows downwards.
		*/
		public void ClearFullRows()
		{
			var size = Size;
			
			// The index of the row to copy from.
			var copyRow = size.Y - 1;

			for (var row = copyRow; row >= 0; --row, --copyRow)
			{
				// Can't copy from a row that doesn't exist, so we fill with empty cells.
				if (copyRow < 0)
				{
					Array.Fill(_cells[row], TetrisCell.Empty);
				}
				else 
				{
					// Move the row index to copy from up one step if it is a full row.
					// This effectively removes it.
					while (_cells[copyRow].All(x => x != TetrisCell.Empty))
					{
						--copyRow;
					}
					// If it is the same row then the copy is a no-op.
					if (copyRow != row)
					{
						Array.Copy(_cells[copyRow], _cells[row], size.X);
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

	/*
		Gets notified of events from the tetris game board.
	*/
	interface IBoardObserver
	{
		void HandleRowsCleared(int[] rowIndices);
		void HandleGameOver();
		void HandleGameStart();
		void HandleNewTetromino(Tetromino newTetromino, Tetromino nextTetromino);
		void HandleBoardUpdated(TetrisBoard newBoard);
	}

	/*
		Manages the main tetris game logic and sends signals about 
		scoring, game over etc.
	*/
	class BoardLogic
	{
		private IBoardObserver _observer;

		private TetrisBoard _board;

		private Tetromino _nextTetromino = Tetrominoes.GetRandom();
		private Tetromino _holdTetromino;

		private Tetromino _activeTetromino;
		private Vec2i _tetrominoPosition;

		private void _CheckGameStarted()
		{
			if (_activeTetromino == null)
			{
				_SpawnTetromino();
				_observer.HandleGameStart();
			}
		}

		private bool _MoveWithoutIntersection(Action forwards, Action backwards)
		{
			_CheckGameStarted();

			forwards();
			if (_board.IntersectsTetromino(_activeTetromino, _tetrominoPosition))
			{
				backwards();
				return false;
			}
			return true;
		}

		private void _PlaceTetromino()
		{
			_board.Draw(_activeTetromino, _tetrominoPosition);

			var fullRowIndices = _board.FindFullRows();
			if (fullRowIndices.Length != 0)
			{
				_board.ClearFullRows();

				_observer.HandleRowsCleared(fullRowIndices);
			}

			_SpawnTetromino();
		}

		private void _SpawnTetromino()
		{
			_activeTetromino = _nextTetromino;
			_nextTetromino = Tetrominoes.GetRandom();
			_RespawnTetromino();
		}
		private void _RespawnTetromino()
		{
			_tetrominoPosition = new Vec2i(new Random().Next(_board.Size.X - _activeTetromino.Width), 0);

			if (_board.IntersectsTetromino(_activeTetromino, _tetrominoPosition))
			{
				_observer.HandleGameOver();
			}
			else
			{
				_observer.HandleNewTetromino(_activeTetromino, _nextTetromino);
			}
		}

		public void Reset()
		{
			_board = new TetrisBoard(_board.Size);
			_activeTetromino = null;
		}

		private void _UpdateBoard()
		{
			var board = new TetrisBoard(_board);
			board.Draw(_activeTetromino, _tetrominoPosition);
			_observer.HandleBoardUpdated(board);
		}

		public void Step()
		{
			_MoveWithoutIntersection(
				() => _tetrominoPosition.Y += 1, 
				() => {
					_tetrominoPosition.Y -= 1;
					_PlaceTetromino();
				});

			_UpdateBoard();
		}

		public void MoveLeft()
		{
			if (_MoveWithoutIntersection(() => --_tetrominoPosition.X, () => ++_tetrominoPosition.X))
			{
				_UpdateBoard();
			}
		}
		public void MoveRight()
		{
			if (_MoveWithoutIntersection(() => ++_tetrominoPosition.X, () => --_tetrominoPosition.X))
			{
				_UpdateBoard();
			}
		}
		public void RotateLeft()
		{
			if (_MoveWithoutIntersection(() => _activeTetromino.Rotate(Direction1D.Left), 
				() => _activeTetromino.Rotate(Direction1D.Right)))
			{
				_UpdateBoard();
			}
		}
		public void RotateRight()
		{
			if (_MoveWithoutIntersection(() => _activeTetromino.Rotate(Direction1D.Right), 
				() => _activeTetromino.Rotate(Direction1D.Left)))
			{
				_UpdateBoard();
			}
		}

		public void HoldTetromino()
		{
			if (_holdTetromino == null)
			{
				_holdTetromino = _activeTetromino;
				_SpawnTetromino();
			}
			else
			{
				(_holdTetromino, _activeTetromino) = (_activeTetromino, _holdTetromino);
				_RespawnTetromino();
			}
		}

		public void Drop()
		{

		}

		public BoardLogic(Vec2i size, IBoardObserver listener)
		{
			_board = new TetrisBoard(size);
			_observer = listener;
		}
	}
}
