using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TetrisBackend
{
	/*
		Something that can store score data between sessions.
	*/
	public interface IScoreStore<T>
	{
		void SaveScores(IEnumerable<T> scores);
		List<T> LoadScores();
	}

	/*
		A list of scores that automatically handles saving and loading to/from some score store.
	*/
	public class ScoreList<T>
		where T : IComparable<T>
	{
		private IScoreStore<T> _store;

		private List<T> _scores;

		public List<T> Scores => _scores;

		public void AddScore(T score)
		{
			// Our list is sorted form greatest to smallest, so we use a reverse
			// comparer utility class.
			var index = _scores.BinarySearch(score, new ReverseComparer<T>());

			// If the exact score wasn't found in the list of previous scores,
			// then the bitwise complement of the score gives the index of
			// the first score smaller than the given score.
			if (index < 0)
			{
				index = ~index;
			}
			_scores.Insert(index, score);
			_store.SaveScores(_scores);
		}

		public void RemoveAll()
		{
			_scores = new List<T>();
			_store.SaveScores(_scores);
		}

		public ScoreList(IScoreStore<T> store) =>
			(_store, _scores) = (store, store.LoadScores());
	}

	/*
		Holds a player nickname and a score value.
	*/
	public readonly struct PlayerScore :
		IComparable<PlayerScore>,
		IComparable
	{
		public string Name { get; }
		public int Score { get; }

		public int CompareTo(PlayerScore other)
		{
			return Score.CompareTo(other.Score);
		}
		public int CompareTo(object other)
		{
			if (other == null)
			{
				return 1; // Everything is larger than null according to MSDN.
			}

			var score = other as PlayerScore?;

			// CompareTo should throw if the type is invalid, according to MSDN.
			if (score == null)
			{
				throw new ArgumentException("The object is not a PlayerScore.");
			}
			else
			{
				return CompareTo(score.Value);
			}
		}

		public static bool operator <(PlayerScore left, PlayerScore right)
		{
			return left.CompareTo(right) < 0;
		}
		public static bool operator <=(PlayerScore left, PlayerScore right)
		{
			return left.CompareTo(right) <= 0;
		}
		public static bool operator >(PlayerScore left, PlayerScore right)
		{
			return left.CompareTo(right) > 0;
		}
		public static bool operator >=(PlayerScore left, PlayerScore right)
		{
			return left.CompareTo(right) >= 0;
		}
		
		public PlayerScore(string name, int score) =>
			(Name, Score) = (name, score);
	}

	/*
		Represents a score file, loads and saves of PlayerScores.
	*/
	public class PlayerScoreFile : IScoreStore<PlayerScore>
	{
		public string FilePath { get; }

		/*
			I could not get any kind of JSON serialization working together with 
			Godot at first, so I wrote my own simple csv-type serialization. 
			I fixed the issue with Godot, but I'm keeping this.
		*/

		/*
			Saves a collection of player scores to a file so that they can be loaded 
			again with LoadScores. If the file doesn't exist, it is created.
		*/
		public void SaveScores(IEnumerable<PlayerScore> scores)
		{
			var content = "";
			foreach (var score in scores)
			{
				content += $"{score.Name}\t{score.Score}\n";
			}
			File.WriteAllText(FilePath, content);
		}
		/*
			Returns a list of player scores saved in the file by SaveScores.
			If the file doesn't exist, an empty list is returned.
			Thlines FileFormatException if the file exists and has an invalid format.
		*/
		public List<PlayerScore> LoadScores()
		{
			if (File.Exists(FilePath))
			{
				return File.ReadAllText(FilePath).Split('\n')
					// The file ends with a \n, so the last element will be empty. 
					// This is safer than doing [..^1]. We'll simply allow but 
					// ignore empty lines in our file format.
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
			if (index == -1 || !int.TryParse(line[(index + 1)..], out score))
			{
				throw new FileFormatException("Score file had an invalid format.");
			}
			return new PlayerScore(line[0..index], score);
		}

		public PlayerScoreFile(string fileName) =>
			FilePath = fileName;
	}
}
