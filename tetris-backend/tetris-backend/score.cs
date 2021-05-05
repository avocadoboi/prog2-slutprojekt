using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace tetris_backend {

public interface IScoreStore<T> {
	void SaveScores(IEnumerable<T> scores);
	List<T> LoadScores();
}

public class ScoreList<T> 
	where T: IComparable<T>
{
	private IScoreStore<T> _store;
	private List<T> _scores;
	public List<T> Scores => _scores;

	public void AddScore(T score) {
		var index = _scores.BinarySearch(score, new ReverseComparer<T>());
		// If the exact score wasn't found in the list of previous scores,
		// then the bitwise complement of the score gives the index of
		// the first score smaller than the given score.
		if (index < 0) {
			index = ~index;
		}
		_scores.Insert(index, score);
		_store.SaveScores(_scores);
	}

	public ScoreList(IScoreStore<T> store) =>
		(_store, _scores) = (store, store.LoadScores());
}

public readonly struct PlayerScore : 
	IComparable<PlayerScore>, 
	IComparable
{
	public string Name { get; init; }
	public int Score { get; init; }

	public int CompareTo(PlayerScore other) {
		return Score.CompareTo(other.Score);
	}
	public int CompareTo(object other) {
		if (other == null) {
			return 1;
		}
		var score = other as PlayerScore?;
		if (score == null) {
			throw new ArgumentException("The object is not a PlayerScore.");
		}
		else {
			return CompareTo(score.Value);
		}
	}
	
	public static bool operator<(PlayerScore left, PlayerScore right) {
		return left.CompareTo(right) < 0;
	}
	public static bool operator<=(PlayerScore left, PlayerScore right) {
		return left.CompareTo(right) <= 0;
	}
	public static bool operator>(PlayerScore left, PlayerScore right) {
		return left.CompareTo(right) > 0;
	}
	public static bool operator>=(PlayerScore left, PlayerScore right) {
		return left.CompareTo(right) >= 0;
	}
}

public class PlayerScoreFile : IScoreStore<PlayerScore> {
	string _file_path;
	string FilePath => _file_path;
	
	public void SaveScores(IEnumerable<PlayerScore> scores) {
		File.WriteAllBytes(_file_path, JsonSerializer.SerializeToUtf8Bytes(scores));
	}
	public List<PlayerScore> LoadScores() {
		if (File.Exists(_file_path)) {
			return JsonSerializer.Deserialize<List<PlayerScore>>(File.ReadAllText(_file_path));
		}
		return new List<PlayerScore>();
	}

	public PlayerScoreFile(string file_name) =>
		_file_path = file_name;
}

} // namespace tetris_backend