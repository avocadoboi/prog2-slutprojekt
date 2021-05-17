using System.Collections.Generic;
using System;

namespace TetrisBackend
{
	public enum GameState
	{
		Running,
		Over,
	}

	/*
		Receives signals about changes in the game state that would be useful for a front-end implementation. 
		It is meant to be implemented by the front-end program that uses this class library.
	*/
	public interface ITetrisStateObserver
	{
		void HandleTetrominoUpdated(TetrominoUpdate newTetrominoes) { }
		void HandleLinesCleared(int[] rowIndices) { }
		void HandleScored(CurrentScore newScore) { }
		void HandleLevelUp(int newLevel) { }
		void HandleGameOver() { }
		void HandleGameStart() { }
		void HandleBoardUpdated(TetrisBoard newBoard) { }
	}

	/*
		Manages the game state; things like score keeping, level keeping and running/game over states.
		It receives signals through the IBoardObserver interface from the tetris board logic about things that happen in the game. 
		It uses these callbacks to update the game state and send forward events to the user interface through the ITetrisStateObserver interface.
	*/
	class StateLogic : IBoardObserver
	{
		private BasicScoreKeeper _scoreKeeper = new BasicScoreKeeper();
		public CurrentScore CurrentScore => _scoreKeeper.Score;

		private TetrisLevel _level;
		public int Level => _level.Level;

		private ScoreList<PlayerScore> _scoreList;
		public List<PlayerScore> ScoreList => _scoreList.Scores;

		public GameState GameState { get; private set; }

		public void Restart()
		{
			GameState = GameState.Running;
			_scoreKeeper = new BasicScoreKeeper();
			_level.Reset();

			foreach (var observer in _observers)
			{
				observer.HandleGameStart();
			}
		}
		/*
			Saves the current player score to the score store with the nickname playerName.
		*/
		public void SaveScore(string playerName)
		{
			_scoreList.AddScore(new PlayerScore(playerName, _scoreKeeper.Score.Points));
		}
		/*
			Removes all player scores from the score store.
		*/
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
			if (_level.Update(CurrentScore.Lines))
			{
				foreach (var observer in _observers)
				{
					observer.HandleLevelUp(Level);
				}
			}
		}
		
		void IBoardObserver.HandleLinesCleared(int[] indices)
		{
			_scoreKeeper.GainScore(new BasicScoreGainData(indices.Length, Level));
			_UpdateLevel();

			foreach (var observer in _observers)
			{
				observer.HandleLinesCleared(indices);
				observer.HandleScored(_scoreKeeper.Score);
			}
		}
		
		void IBoardObserver.HandleTetrominoUpdated(TetrominoUpdate newTetrominoes)
		{
			foreach (var observer in _observers)
			{
				observer.HandleTetrominoUpdated(newTetrominoes);
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
		void IBoardObserver.HandleBoardUpdated(TetrisBoard newBoard)
		{
			foreach (var observer in _observers)
			{
				observer.HandleBoardUpdated(newBoard);
			}
		}

		public StateLogic(IScoreStore<PlayerScore> scoreStore) {
			_level.Reset();
			_scoreList = new ScoreList<PlayerScore>(scoreStore);
		}
	}
}