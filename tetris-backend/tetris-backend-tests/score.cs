using System.Collections.Generic;
using System.IO;
using Xunit;
using tetris_backend;

namespace tetris_backend_tests {

public class ScoreTests
{
	public const string FILE_NAME = "test_scores";
	
	[Fact]
	public void PlayerScoreList() {
		if (File.Exists(FILE_NAME)) {
			File.Delete(FILE_NAME);
		}
		
		var list = new ScoreList<PlayerScore>(new PlayerScoreFile(FILE_NAME));
		list.AddScore(new PlayerScore{Name = "Bjärn", Score = 3});
		list.AddScore(new PlayerScore{Name = "Snudin", Score = 8});
		list.AddScore(new PlayerScore{Name = "Satan", Score = 6});
		list.AddScore(new PlayerScore{Name = "Snigel", Score = -4});
		list.AddScore(new PlayerScore{Name = "Palettblad", Score = 70});

		var expected_list = new List<PlayerScore> {
			new PlayerScore{Name = "Palettblad", Score = 70},
			new PlayerScore{Name = "Snudin", Score = 8},
			new PlayerScore{Name = "Satan", Score = 6},
			new PlayerScore{Name = "Bjärn", Score = 3},
			new PlayerScore{Name = "Snigel", Score = -4},
		};
		Assert.Equal(expected_list, list.Scores);

		var new_list = new ScoreList<PlayerScore>(new PlayerScoreFile(FILE_NAME));
		Assert.Equal(expected_list, new_list.Scores);
	}
}

} // namespace tetris_backend_tests