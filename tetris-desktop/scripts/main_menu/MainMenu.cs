using Godot;
using System;

public class MainMenu : VBoxContainer
{
	public override void _Ready()
	{
		GetNode("highscores_container/button").Connect("pressed", this, nameof(_HandleHighscoresButtonPress));
		GetNode("play_container/button").Connect("pressed", this, nameof(_HandlePlayButtonPress));
	}
	private void _HandlePlayButtonPress()
	{
		GetTree().ChangeScene("res://scenes/game.tscn");
	}
	private void _HandleHighscoresButtonPress()
	{
		GetTree().ChangeScene("res://scenes/highscores.tscn");
	}
}
