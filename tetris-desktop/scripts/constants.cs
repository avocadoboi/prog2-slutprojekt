using Godot;
using tetris_backend;

public static class Constants
{
	public const string score_file = "scores";
	public static readonly Vec2i board_size = new Vec2i(10, 20);
	public const float tetris_step_interval = 0.3f; // Seconds
}
