using Godot;
using System;
using TetrisBackend;

public class GameOver : VBoxContainer
{
	public override void _Ready()
	{
		GetNode("retry_container/button").Connect("pressed", this, nameof(_HandleRetryPress));
		GetNode("main_menu_container/button").Connect("pressed", this, nameof(_HandleMainMenuPress));
	}

	private void _HandleRetryPress()
	{
		_SaveScore();
		GetTree().ChangeScene("res://scenes/game.tscn");
	}
	private void _HandleMainMenuPress()
	{
		_SaveScore();
		GetTree().ChangeScene("res://scenes/main_menu.tscn");
	}

	private void _SaveScore()
	{
		var nickname = GetNode<LineEdit>("nickname").Text;
		if (nickname.Length != 0)
		{
			BackendInstance.Game.SaveScore(nickname);
		}
	}
}
