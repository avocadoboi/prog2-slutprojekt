using System;

namespace TetrisBackend
{
    /*
		Holds the current game score.
	*/
	public readonly struct CurrentScore
	{
		public int Points { get; }
		public int Lines { get; }

		public CurrentScore(int points, int lines) =>
			(Points, Lines) = (points, lines);
	}

	interface IScoreKeeper<_ScoreGainData>
	{
		CurrentScore Score { get; }

		/*
			Calculates a score gain from some data structure containing
			information about the game state necessary to calculate score.
			For example, this method would need a different data structure
			if it were able to calculate T-spin points than if it only 
			calculated score based on level and lines cleared.
		*/
		void GainScore(_ScoreGainData scoreGainData);
	}

	readonly struct BasicScoreGainData
	{
		public int LinesCleared { get; }
		public int GameLevel { get; }

		public BasicScoreGainData(int linesCleared, int gameLevel) =>
			(LinesCleared, GameLevel) = (linesCleared, gameLevel);
	}

	/*
		Keeps track of points and lines scored in a game.
	*/
	struct BasicScoreKeeper : IScoreKeeper<BasicScoreGainData>
	{
		private int _points;
		private int _lines;

		public CurrentScore Score => new CurrentScore(_points, _lines);

		/*
			Increases the score as a function of the number of lines cleared 
			and the game level, according to https://tetris.fandom.com/wiki/Scoring
		*/
		public void GainScore(BasicScoreGainData data)
		{
			_lines += data.LinesCleared;

			_points += data.LinesCleared switch
			{
				0 => 0,
				1 => 40,
				2 => 100,
				3 => 300,
				4 => 1200,
				_ => throw new ArgumentOutOfRangeException(nameof(data.LinesCleared), $"Only 1-4 lines can be cleared at once in Tetris."),
			} * (data.GameLevel + 1);
		}
	}
}