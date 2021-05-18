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
					throw new ArgumentException("A tetris board was assigned a grid with wrong dimensions!", nameof(value));
				}
			}
		}
		public Vec2i Size { get; }

		/*
			Searches for full lines on the board and returns their indices.
		*/
		public int[] FindFullLines()
		{
			return _cells.Select((row, i) => i).Where(i => _cells[i].All(x => x != TetrisCell.Empty)).ToArray();
		}

		/*
			Removes all full tetris lines, packing the remaining lines downwards.
		*/
		public void ClearFullLines()
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

		/*
			Returns true if any non-empty cell in the given tetromino either overlaps with a non-empty cell in the board or is outside the bounds of
			the board.
		*/
		public bool IntersectsTetromino(Tetromino tetromino, Vec2i offset) 
		{
			return tetromino.Cells.Indices2D().Any(pos =>
				tetromino.Cells[pos.Y][pos.X] != TetrisCell.Empty && 
				// If any non-empty cell within the tetromino is outside the bounds of the board then it intersects.
				(!new Vec2i(pos.X + offset.X, pos.Y + offset.Y).GetIsWithin(Size) ||
				// If two non-empty cells overlap then the tetromino intersects the board.
				Cells[pos.Y + offset.Y][pos.X + offset.X] != TetrisCell.Empty));
		}

		/*
			Calculates the Y position (row index of the top edge of the tetromino) where the tetromino would drop on the board due to falling down
			from the start position "offset". 

			An InvalidOperationException is thrown if the tetromino is empty.
		*/
		public int CalculateDropPosition(Tetromino tetromino, Vec2i offset)
		{
			// Note that this will throw an exception if no drop position is found.
			// It is intended that the program crashes in that case because that would indicate a bug.
			// A drop position should always be found, since IntersectsTetromino returns true if any non-empty cell in the tetromino is outside the
			// bounds of the board - which it definitely is at Size.Y + tetromino.Width unless the tetromino is empty.
			return Enumerable.Range(offset.Y + 1, Size.Y + tetromino.Width).First(y => IntersectsTetromino(tetromino, new Vec2i(offset.X, y))) - 1;
		}

		public TetrisBoard(Vec2i size) =>
			(Cells, Size) = (Utils.CreateRectangularArray<TetrisCell>(size), size);

		public TetrisBoard(TetrisBoard other) =>
			(Cells, Size) = (other.Cells.Select(row => row.ToArray()).ToArray(), other.Size);
	}

	public readonly struct TetrominoUpdate
	{
		public Tetromino Active { get; }
		public Tetromino Next { get; }
		public Tetromino Hold { get; }

		public TetrominoUpdate(Tetromino active, Tetromino next, Tetromino hold) =>
			(Active, Next, Hold) = (active, next, hold);
	}

	/*
		Gets notified of events from the tetris game board.
	*/
	interface IBoardObserver
	{
		void HandleLinesCleared(int[] rowIndices);
		void HandleGameOver();
		void HandleTetrominoUpdated(TetrominoUpdate newTetrominoes);
		void HandleBoardUpdated(TetrisBoard newBoard);
	}

	/*
		Manages the main tetris game logic and sends signals about scoring, game over etc.
	*/
	class BoardLogic
	{
		private IBoardObserver _observer;

		private TetrisBoard _board;

		private Tetromino _nextTetromino = Tetrominoes.GetRandom();
		private Tetromino _holdTetromino;
		private bool _hasSwappedHold; // Only one swap allowed per new tetromino.

		private Tetromino _activeTetromino;
		private Vec2i _tetrominoPosition;

		/*
			Starts the game 
		*/
		public void Restart()
		{
			_board = new TetrisBoard(_board.Size);
			_holdTetromino = null;
			_nextTetromino = Tetrominoes.GetRandom();
			_SpawnTetromino();
			_UpdateBoard();
		}

		/*
			Draws the active tetromino onto a copy of the static board and sends the composite board to the observer.
		*/
		private void _UpdateBoard()
		{
			var board = new TetrisBoard(_board);
			board.Draw(_activeTetromino, _tetrominoPosition);
			_observer.HandleBoardUpdated(board);
		}

		/*
			Spawns the next tetromino and selects a new next tetromino. 
			This method can cause a game over if it failed to place it at the top.
		*/
		private void _SpawnTetromino()
		{
			_activeTetromino = _nextTetromino;
			_nextTetromino = Tetrominoes.GetRandom();
			_RespawnTetromino();
		}

		/*
			Places the tetromino at a new start position at the top and sends a game over signal if it failed to place it there.
		*/
		private void _RespawnTetromino()
		{
			_tetrominoPosition = new Vec2i(new Random().Next(_board.Size.X - _activeTetromino.Width), 0);

			if (_board.IntersectsTetromino(_activeTetromino, _tetrominoPosition))
			{
				_observer.HandleGameOver();
			}
			else
			{
				_observer.HandleTetrominoUpdated(new TetrominoUpdate(_activeTetromino, _nextTetromino, _holdTetromino));
			}
		}

		/*
			Draws the active tetromino onto the static tetris board at its current position, handles scoring and spawns the next tetromino.
		*/
		private void _PlaceTetromino()
		{
			_hasSwappedHold = false;

			_board.Draw(_activeTetromino, _tetrominoPosition);

			var fullRowIndices = _board.FindFullLines();
			if (fullRowIndices.Length != 0)
			{
				_board.ClearFullLines();

				_observer.HandleLinesCleared(fullRowIndices);
			}

			_SpawnTetromino();
		}

		/*
			Moves the active tetromino in some way and reverts the move if it caused a collision with the board.
			"forwards" is the action that moves the tetromino and "backwards" is the inverse action that resets it to its previous state.
			Returns true if it succeeded and false if "backwards" was invoked.
		*/
		private bool _MoveWithoutIntersection(Action forwards, Action backwards)
		{
			forwards();
			if (_board.IntersectsTetromino(_activeTetromino, _tetrominoPosition))
			{
				backwards();
				return false;
			}
			return true;
		}

		/*
			Moves the active tetromino one step downwards and places it if it hits something.
			Sends an updated board to the observer.
		*/
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
			if (_hasSwappedHold)
			{
				return;
			}
			
			_hasSwappedHold = true;

			if (_holdTetromino == null)
			{
				// If there's no hold tetromino then we take a new tetromino and put the active one in the hold slot.
				_holdTetromino = _activeTetromino;
				_SpawnTetromino();
			}
			else
			{
				// If there's already a hold tetromino then we swap it with the active one.
				(_holdTetromino, _activeTetromino) = (_activeTetromino, _holdTetromino);
				_RespawnTetromino();
			}
			_observer.HandleTetrominoUpdated(new TetrominoUpdate(_activeTetromino, _nextTetromino, _holdTetromino));
			_UpdateBoard();
		}

		public void Drop()
		{
			_tetrominoPosition.Y = _board.CalculateDropPosition(_activeTetromino, _tetrominoPosition);
			_PlaceTetromino();
			_UpdateBoard();
		}

		public BoardLogic(Vec2i size, IBoardObserver listener)
		{
			_board = new TetrisBoard(size);
			_observer = listener;
			_SpawnTetromino();
		}
	}
}
