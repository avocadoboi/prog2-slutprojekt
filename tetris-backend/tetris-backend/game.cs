using System;
using System.Collections.Generic;

namespace tetris_backend
{

    public enum TetrisInput
    {
        Left,
        Right,
        RotateLeft,
        RotateRight,
        SwapTetromino,
        SkipFall,
        Restart
    }

    public class TetrisGame
    {
        private StateLogic _state_logic;
        private BoardLogic _board_logic;

        public TetrisBoard Board => _board_logic.Board;
        // public GameState State => _state_logic.State;
        // public List<PlayerScore> ScoreList => _state_logic.ScoreList;
        // public CurrentScore Score => _state_logic.Score;

        public void GiveInput(TetrisInput input)
        {
            switch (input)
            {
                case TetrisInput.Restart:
                    _board_logic.Restart();
                    break;
                    // case TetrisInput.
            }
        }
        public void Step()
        {
            _board_logic.Step();
        }
        public void SaveScore(string player_name)
        {
            _state_logic.SaveScore(player_name);
        }

        public void AddObserver(ITetrisStateObserver observer)
        {
            _state_logic.AddObserver(observer);
        }

        public TetrisGame(Vec2i board_size, IScoreStore<PlayerScore> score_store)
        {
            _state_logic = new StateLogic(score_store);
            _board_logic = new BoardLogic(board_size, _state_logic);
        }
    }

}
