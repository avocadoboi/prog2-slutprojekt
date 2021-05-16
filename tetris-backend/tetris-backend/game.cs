using System;
using System.Collections.Generic;

namespace TetrisBackend
{
	public enum TetrisInput
	{
		Left,
		Right,
		RotateLeft,
		RotateRight,
		HoldTetromino,
		Drop,
	}

	public class TetrisGame
	{
		private StateLogic _stateLogic;
		private BoardLogic _boardLogic;

		public List<PlayerScore> ScoreList => _stateLogic.ScoreList;
		public CurrentScore CurrentScore => _stateLogic.CurrentScore;
		public int CurrentLevel => _stateLogic.Level;

		public void GiveInput(TetrisInput input)
		{
			if (_stateLogic.GameState == GameState.Over)
			{
				return;
			}
			
			switch (input)
			{
			case TetrisInput.Left:
				_boardLogic.MoveLeft();
				break;
			case TetrisInput.Right:
				_boardLogic.MoveRight();
				break;
			case TetrisInput.RotateLeft:
				_boardLogic.RotateLeft();
				break;
			case TetrisInput.RotateRight:
				_boardLogic.RotateRight();
				break;
			case TetrisInput.Drop:
				_boardLogic.Drop();
				break;
			case TetrisInput.HoldTetromino:
				_boardLogic.HoldTetromino();
				break;
			}
		}
		public void Step()
		{
			if (_stateLogic.GameState == GameState.Over)
			{
				return;
			}

			_boardLogic.Step();
		}
		
		public void Restart()
		{
			_boardLogic.Reset();
			_stateLogic.Reset();
		}

		public void SaveScore(string playerName)
		{
			_stateLogic.SaveScore(playerName);
		}
		public void RemoveAllScores()
		{
			_stateLogic.RemoveAllScores();
		}

		public void AddStateObserver(ITetrisStateObserver observer)
		{
			_stateLogic.AddObserver(observer);
		}
		public void RemoveStateObserver(ITetrisStateObserver observer)
		{
			_stateLogic.RemoveObserver(observer);
		}

		public TetrisGame(Vec2i boardSize, IScoreStore<PlayerScore> scoreStore)
		{
			_stateLogic = new StateLogic(scoreStore);
			_boardLogic = new BoardLogic(boardSize, _stateLogic);
		}
	}
}
