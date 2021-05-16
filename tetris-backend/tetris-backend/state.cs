using System.Collections.Generic;
using System;

namespace TetrisBackend
{
	public enum GameState
	{
		Running,
		Over,
	}

	public struct TetrisLevel
	{
		public int Level { get; private set; }

		/*
			Returns true if the level increased.
		*/
		public bool Update(int totalLinesScored)
		{
			// https://tetris.fandom.com/wiki/Tetris_(NES,_Nintendo)
			// https://www.desmos.com/calculator/knpxxvajv5?lang=sv-SE
			if (Level < 9 && totalLinesScored > 5*(Level + 1)*(Level + 2) ||
				Level >= 9 && totalLinesScored > 100*Level - 350)
			{
				++Level;
				return true;
			}
			return false;
		}
		public void Reset()
		{
			Level = 0;
		}

		public static int CalculateFramesPerCell(int level)
		{
			// https://tetris.fandom.com/wiki/Tetris_(NES,_Nintendo)
			return level switch {
				<0 => throw new ArgumentOutOfRangeException(nameof(level), "Negative tetris levels are impossible!"),
				>=0 and <=8 => 48 - level*5,
				9 => 6,
				>=10 and <=12 => 5,
				>=13 and <=15 => 4,
				>=16 and <=18 => 3,
				>=19 and <=28 => 2,
				>=29 => 1,
			};
		}

		public TetrisLevel(int startLevel) => Level = startLevel;
	}

	public interface ITetrisStateObserver
	{
		void HandleNewTetromino(Tetromino newTetromino, Tetromino nextTetromino) { }
		void HandleRowsCleared(int[] rowIndices) { }
		void HandleScored(CurrentScore newScore) { }
		void HandleLevelUp(int newLevel) { }
		void HandleGameOver() { }
		void HandleGameStart() { }
		void HandleBoardUpdated(TetrisBoard newBoard) { }
	}

	class StateLogic : IBoardObserver
	{
		private BasicScoreKeeper _scoreKeeper = new BasicScoreKeeper();
		public CurrentScore CurrentScore => _scoreKeeper.Score;

		private TetrisLevel _level;
		public int Level => _level.Level;

		private ScoreList<PlayerScore> _scoreList;
		public List<PlayerScore> ScoreList => _scoreList.Scores;

		public GameState GameState { get; private set; }

		public void Reset()
		{
			GameState = GameState.Running;
			_scoreKeeper = new BasicScoreKeeper();
			_level.Reset();
		}
		public void SaveScore(string playerName)
		{
			_scoreList.AddScore(new PlayerScore(playerName, _scoreKeeper.Score.Points));
		}
		public void RemoveAllScores()
		{
			_scoreList.RemoveAll();
		}

		private List<ITetrisStateObserver> _observers = new List<ITetrisStateObserver>();

		public void AddObserver(ITetrisStateObserver observer)
		{
			if (!_observers.Contains(observer))
			{
				_observers.Add(observer);
			}
		}
		public void RemoveObserver(ITetrisStateObserver observer)
		{
			_observers.Remove(observer);
		}
		
		private void _UpdateLevel()
		{
			if (_level.Update(CurrentScore.Rows))
			{
				foreach (var observer in _observers)
				{
					observer.HandleLevelUp(Level);
				}
			}
		}
		
		void IBoardObserver.HandleRowsCleared(int[] indices)
		{
			_scoreKeeper.GainScore(new BasicScoreGainData(indices.Length, Level));

			foreach (var observer in _observers)
			{
				observer.HandleRowsCleared(indices);
				observer.HandleScored(_scoreKeeper.Score);
			}

			_UpdateLevel();
		}
		
		void IBoardObserver.HandleNewTetromino(Tetromino newTetromino, Tetromino nextTetromino)
		{
			foreach (var observer in _observers)
			{
				observer.HandleNewTetromino(newTetromino, nextTetromino);
			}
		}
		void IBoardObserver.HandleGameOver()
		{
			GameState = GameState.Over;
			foreach (var observer in _observers)
			{
				observer.HandleGameOver();
			}
		}
		void IBoardObserver.HandleGameStart()
		{
			foreach (var observer in _observers)
			{
				observer.HandleGameStart();
			}
		}
		void IBoardObserver.HandleBoardUpdated(TetrisBoard newBoard)
		{
			foreach (var observer in _observers)
			{
				observer.HandleBoardUpdated(newBoard);
			}
		}

		public StateLogic(IScoreStore<PlayerScore> scoreStore) =>
			_scoreList = new ScoreList<PlayerScore>(scoreStore);
	}
}