using System.Collections.Generic;

namespace tetris_backend {

public enum GameState {
    Running,
    Over,
}

class StateLogic : IBoardListener {
    private BasicScoreKeeper _score_keeper = new BasicScoreKeeper();
    public CurrentScore Score => _score_keeper.Score;
    
    private ScoreList<PlayerScore> _score_list;
    public List<PlayerScore> ScoreList => _score_list.Scores;

    private GameState _state = GameState.Running;
    public GameState State => _state;

    public void SaveScore(string player_name) {
        _score_list.AddScore(new PlayerScore{Name = player_name, Score = _score_keeper.Score.Points});
    }
    public void HandleScored(BasicScoreGainData data) {
        _score_keeper.GainScore(data);
    }
    public void HandleGameOver() {
        _state = GameState.Over;
    }
    public void HandleGameStart() {
        _state = GameState.Running;
    }
}

}