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

	/*
		This is the main API that Tetris front-end implementations interact with.
		Its only purpose is to provide a minimal user interface to the StateLogic and BoardLogic classes.
		Calls are forwarded to those classes.
	*/
	public class TetrisGame
	{
		private StateLogic _stateLogic;
		private BoardLogic _boardLogic;

		public List<PlayerScore> ScoreList => _stateLogic.ScoreList;
		public CurrentScore CurrentScore => _stateLogic.CurrentScore;
		public int CurrentLevel => _stateLogic.Level;

		/*
			This method can be thought of as a game controller port.
			Pass an action as a TetrisInput enumeration to give player input to the game.
			This method causes observers to be notified of game board changes, for instance.
			Nothing happens if the current game is over. Call Restart to restart the game.
		*/
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
		/*
			Moves the active tetromino one step downwards due to "gravity" and handles any tetromino placement and scoring that may lead to.
			Nothing happens if the current game is over. Call Restart to restart the game.
		*/
		public void Step()
		{
			if (_stateLogic.GameState == GameState.Over)
			{
				return;
			}

			_boardLogic.Step();
		}
		
		/*
			Resets all game state and starts a new game, keeping the same board size and score store.
		*/
		public void Restart()
		{
			_boardLogic.Restart();
			_stateLogic.Restart();
		}

		/*
			Saves the current player score to the score store with the nickname playerName.
		*/
		public void SaveScore(string playerName)
		{
			_stateLogic.SaveScore(playerName);
		}
		/*
			Removes all player scores from the score store.
		*/
		public void RemoveAllScores()
		{
			_stateLogic.RemoveAllScores();
		}

		/*
			Enables an object to be updated of changes in game state.
			Attaching an observer is necessary for implementing a Tetris front-end since that is, for example, 
			how updates to the game board are made accessible.
			Remember to call RemoveStateObserver if you want your observer to die before this tetris game dies.
		*/
		public void AddStateObserver(ITetrisStateObserver observer)
		{
			_stateLogic.AddObserver(observer);
		}
		/*
			Removes a game state observer from the list of observers to notify.
		*/
		public void RemoveStateObserver(ITetrisStateObserver observer)
		{
			_stateLogic.RemoveObserver(observer);
		}

		/*
			boardSize.X is the number of columns on the Tetris board and boardSize.Y is the row count.

			The score store is responsible for loading and saving player scores between instances/sessions.
			The library provides the implementation PlayerScoreFile, but it's a possible customization point.
			Some front end might want to store the player score in a database on the internet and thus use another score store. 
		*/
		public TetrisGame(Vec2i boardSize, IScoreStore<PlayerScore> scoreStore)
		{
			_stateLogic = new StateLogic(scoreStore);
			_boardLogic = new BoardLogic(boardSize, _stateLogic);
		}
	}
}
