using Godot;
using System;

public class main_menu : VBoxContainer {
    public override void _Ready() {
        GetNode("highscores_container/button").Connect("pressed", this, nameof(_HandleHighscoresButtonPress));
        GetNode("play_container/button").Connect("pressed", this, nameof(_HandlePlayButtonPress));
    }
	public void _HandlePlayButtonPress() {
		GetTree().ChangeScene("res://scenes/game.tscn");
	}
    public void _HandleHighscoresButtonPress() {
        GetTree().ChangeScene("res://scenes/highscores.tscn");
    }
}
