using TetrisBackend;

/*
	Holds a global tetris game backend instance.
*/
public static class BackendInstance
{
	public static TetrisGame Game { get; } = new TetrisGame(Constants.boardSize, new PlayerScoreFile(Constants.scoreFile));
}
