using Godot;
using System;
using TetrisBackend;
using System.Collections.Generic;

public class HighscoreList : GridContainer
{
	public override void _Ready()
	{
		_LoadScoreTable();

		GetNode("/root/container/buttons/back").Connect("pressed", this, nameof(_GoBack));
		GetNode("/root/container/buttons/reset_all").Connect("pressed", this, nameof(_ResetAll));
	}

	private void _LoadScoreTable()
	{
		var scores = BackendInstance.Game.ScoreList;
		if (scores.Count == 0)
		{
			_AddNoScoresInfo();
		}
		else
		{
			_AddScores(scores);
		}
	}

	private void _AddScores(List<PlayerScore> scores)
	{
		foreach ((int i, PlayerScore item) in scores.Enumerate())
		{
			AddChild(new Label() { Text = $"{i+1}." });
			AddChild(new Label() { Text = item.Name });
			AddChild(new Label() { Text = item.Score.ToString() });
		}
	}
	private void _AddNoScoresInfo()
	{
		var label = new Label() { Text = "No scores!" };
		label.AddColorOverride("font_color", new Color(0, 0, 0, 0.5f));
		AddChild(label);
	}

	private void _GoBack()
	{
		GetTree().ChangeScene("res://scenes/main_menu.tscn");
	}
	private void _ResetAll()
	{
		BackendInstance.Game.RemoveAllScores();

		foreach (Node child in GetChildren())
		{
			child.QueueFree();
		}
		_AddNoScoresInfo();
	}
}
