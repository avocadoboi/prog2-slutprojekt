using Godot;
using System;
using tetris_backend;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Constants {
    public const string score_file = "scores";
}

public class highscore_list : ItemList {
    private PlayerScoreFile _file = new PlayerScoreFile(Constants.score_file);
    
    public override void _Ready() {
        var list = _file.LoadScores();
        foreach ((int i, PlayerScore item) in list.Enumerate()) {
            AddItem(i.ToString(), null, false);
            AddItem(item.Name, null, false);
            AddItem(item.Score.ToString(), null, false);
        }

        GetNode("../../buttons/back").Connect("pressed", this, nameof(_HandleBackPressed));
    }

    private void _HandleBackPressed() {
        GetTree().ChangeScene("res://scenes/main_menu.tscn");
    }
}
