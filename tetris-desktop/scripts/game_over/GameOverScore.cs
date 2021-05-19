using Godot;
using System;

/*
	Shows the player score at the game over screen after a game.
*/
public class GameOverScore : Label
{
	public override void _Ready()
	{
		var score = BackendInstance.Game.CurrentScore;
		Text = $"Score: {score.Points}\nLines: {score.Lines}\nLevel: {BackendInstance.Game.CurrentLevel}";
	}
}
