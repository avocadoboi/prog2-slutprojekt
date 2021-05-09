using Godot;
using System;

public class main_menu : VBoxContainer {
    public override void _Ready() {
        GetNode("highscores_container/button").Connect("pressed", this, nameof(_HandleHighscoresButtonPress));
    }
    public void _HandleHighscoresButtonPress() {
        GetTree().ChangeScene("res://scenes/highscores.tscn");
    }
}
