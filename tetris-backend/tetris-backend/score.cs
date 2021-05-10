using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tetris_backend {

public interface IScoreStore<T> {
	void SaveScores(IEnumerable<T> scores);
	List<T> LoadScores();
}

/*
	A list of scores that automatically handles saving and loading to/from some score store.
*/
public class ScoreList<T> 
	where T: IComparable<T>
{
	private IScoreStore<T> _store;
	private List<T> _scores;
	public List<T> Scores => _scores;

	public void AddScore(T score) {
		// Our list is sorted form greatest to smallest, so we use a reverse
		// comparer utility class.
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
			return 1; // Everything is larger than null according to MSDN.
		}
		
		var score = other as PlayerScore?;

		// CompareTo should throw if the type is invalid, according to MSDN.
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

/*
	Represents a score file, loads and saves of PlayerScores.
*/
public class PlayerScoreFile : IScoreStore<PlayerScore> 
{
	private string _file_path;
	public string FilePath => _file_path;
	
	/*
		I could not get any kind of JSON serialization working together with Godot...
		So I'm just writing my own simple csv-type serialization. It might be a little 
		faster, not that it matters.
	*/
	
	/*
		Saves a collection of player scores to a file so that they can be loaded 
		again with LoadScores. If the file doesn't exist, it is created.
	*/
	public void SaveScores(IEnumerable<PlayerScore> scores) {
		var content = "";
		foreach (var score in scores) {
			content += $"{score.Name}\t{score.Score}\n";
		}
		File.WriteAllText(_file_path, content);
	}
	/*
		Returns a list of player scores saved in the file by SaveScores.
		If the file doesn't exist, an empty list is returned.
		Throws FileFormatException if the file exists and has an invalid format.
	*/
	public List<PlayerScore> LoadScores() {
		if (File.Exists(_file_path)) 
		{
			return File.ReadAllText(_file_path).Split('\n')
				// The file ends with a \n, so the last element will be empty. 
				// This is safer than doing [..^1]. We'll simply allow but ignore empty lines in our file format.
				.Where((s) => s.Length != 0) 
				.Select(_ScoreFromLine)
				.ToList();
		}
		return new List<PlayerScore>();
	}
	/*
		Parses a line in a score save file and returns a player score.
	*/
	private static PlayerScore _ScoreFromLine(string line) 
	{
		var index = line.LastIndexOf('\t');
		int score;
		if (index == -1 || !int.TryParse(line[(index+1)..], out score)) 
		{
			throw new FileFormatException("Score file had an invalid format.");
		}
		else return new PlayerScore{
			Name = line[0..index],
			Score = score,
		};
	}

	public PlayerScoreFile(string file_name) =>
		_file_path = file_name;
}

} // namespace tetris_backend