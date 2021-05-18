using System;

namespace TetrisBackend
{
	/*
		Handles functionality related to tetris levels.
		An instance holds a level that can be updated by passing the total lines scored.
	*/
	public struct TetrisLevel
	{
		public int Level { get; private set; }

		/*
			Returns true if the level increased.
		*/
		public bool Update(int totalLinesScored)
		{
			if (totalLinesScored >= Level*10)
			{
				++Level;
				return true;
			}
			return false;
			
			// This below is what I derived from the table on the website.
			// I thought it was progressing too slowly, so I changed it to the above. 
			// The game on https://tetris.com/play-tetris increases the level every 10 lines too.
 
			// https://tetris.fandom.com/wiki/Tetris_(NES,_Nintendo)
			// https://www.desmos.com/calculator/knpxxvajv5?lang=sv-SE
			// if (Level < 9 && totalLinesScored >= 5*(Level + 1)*(Level + 2) ||
			// 	Level >= 9 && totalLinesScored >= 100*Level - 350)
			// {
			// 	++Level;
			// 	return true;
			// }
			// return false;
		}
		public void Reset()
		{
			Level = 0;
			Update(0);
		}

		public TetrisLevel(int startLevel) => Level = startLevel;

		/*
			In Tetris, the level changes the fall speed.
			This static method calculates the time in seconds between every cell step downwards for a certain tetris level.
		*/
		public static float CalculateSecondsPerCell(int level)
		{
			// https://tetris.fandom.com/wiki/Tetris_(NES,_Nintendo)
			return level switch {
				<0 => throw new ArgumentOutOfRangeException(nameof(level), "Negative tetris levels are impossible!"),
				>=0 and <=8 => 48 - level*5,
				9 => 6,
				>=10 and <=12 => 5,
				>=13 and <=15 => 4,
				>=16 and <=18 => 3,
				>=19 and <=28 => 2,
				>=29 => 1,
			}/60f;
		}
	}
}