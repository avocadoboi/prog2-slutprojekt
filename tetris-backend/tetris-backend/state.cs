using System.Collections.Generic;

namespace tetris_backend
{

	public enum GameState
	{
		Running,
		Over,
	}

	public interface ITetrisStateObserver
	{
		void HandleNewTetromino(Tetromino new_tetromino, Tetromino next_tetromino) { }
		void HandleRowsCleared(int[] row_indices) { }
		void HandleScored(CurrentScore new_score) { }
		void HandleGameOver() { }
		void HandleGameStart() { }
	}

	class StateLogic : IBoardObserver
	{
		private BasicScoreKeeper _score_keeper = new BasicScoreKeeper();
		// public CurrentScore Score => _score_keeper.Score;

		private ScoreList<PlayerScore> _score_list;
		// public List<PlayerScore> ScoreList => _score_list.Scores;

		// private GameState _state = GameState.Running;
		// public GameState State => _state;

		private List<ITetrisStateObserver> _observers = new List<ITetrisStateObserver>();

		public void AddObserver(ITetrisStateObserver observer)
		{
			_observers.Add(observer);
		}

		public void SaveScore(string player_name)
		{
			_score_list.AddScore(new PlayerScore(player_name, _score_keeper.Score.Points));
		}
		void IBoardObserver.HandleRowsCleared(int[] indices)
		{
			// For now, I'm not implementing game levels.
			_score_keeper.GainScore(new BasicScoreGainData(indices.Length, 1));

			foreach (var observer in _observers)
			{
				observer.HandleRowsCleared(indices);
				observer.HandleScored(_score_keeper.Score);
			}
		}
		void IBoardObserver.HandleNewTetromino(Tetromino new_tetromino, Tetromino next_tetromino)
		{
			foreach (var observer in _observers)
			{
				observer.HandleNewTetromino(new_tetromino, next_tetromino);
			}
		}
		void IBoardObserver.HandleGameOver()
		{
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

		public StateLogic(IScoreStore<PlayerScore> score_store) =>
			_score_list = new ScoreList<PlayerScore>(score_store);
	}

}