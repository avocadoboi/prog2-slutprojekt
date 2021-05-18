using Godot;
using System;

public class GameOverScore : Label
{
	public override void _Ready()
	{
		var score = BackendInstance.Game.CurrentScore;
		Text = $"Score: {score.Points}\nLines: {score.Lines}\nLevel: {BackendInstance.Game.CurrentLevel}";
	}
}
