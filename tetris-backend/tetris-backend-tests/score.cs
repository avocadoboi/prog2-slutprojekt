using System.Collections.Generic;
using System.IO;
using Xunit;
using TetrisBackend;

namespace TetrisBackendTests
{
	public class ScoreTests
	{
		public const string FILE_NAME = "test_scores";

		[Fact]
		public void PlayerScoreList()
		{
			if (File.Exists(FILE_NAME))
			{
				File.Delete(FILE_NAME);
			}

			var list = new ScoreList<PlayerScore>(new PlayerScoreFile(FILE_NAME));
			list.AddScore(new PlayerScore("Bjärn", 3));
			list.AddScore(new PlayerScore("Snudin", 8));
			list.AddScore(new PlayerScore("Satan", 6));
			list.AddScore(new PlayerScore("Snigel", -4));
			list.AddScore(new PlayerScore("Palettblad", 70));

			var expectedList = new List<PlayerScore> {
				new PlayerScore("Palettblad", 70),
				new PlayerScore("Snudin", 8),
				new PlayerScore("Satan", 6),
				new PlayerScore("Bjärn", 3),
				new PlayerScore("Snigel", -4),
			};
			Assert.Equal(expectedList, list.Scores);

			var newList = new ScoreList<PlayerScore>(new PlayerScoreFile(FILE_NAME));
			Assert.Equal(expectedList, newList.Scores);
		}
	}
}