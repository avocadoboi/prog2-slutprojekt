using Godot;
using TetrisBackend;

public static class Constants
{
	public const string scoreFile = "scores";
	
	// In seconds.
	public const float softDropInterval = 1f/40f;
	
	public static readonly Vec2i boardSize = new Vec2i(10, 20);
}
