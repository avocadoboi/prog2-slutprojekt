using Godot;
using System;
// using tetris_backend;
using System.Collections.Generic;
using Newtonsoft.Json;

public static class Constants {
    public const string score_file = "scores";
}

public readonly struct PlayerScore {
    public string Name { get; init; }
	public int Score { get; init; }
}

public class highscore_list : ItemList {
    public override void _Ready() {
        var serialized = JsonConvert.SerializeObject(new PlayerScore{Name = "Snigel", Score = 70});

        // var scores = new PlayerScore[] {
		// 	new PlayerScore{Name = "Palettblad", Score = 70},
		// 	new PlayerScore{Name = "Snudin", Score = 8},
		// 	new PlayerScore{Name = "Satan", Score = 6},
		// 	new PlayerScore{Name = "Bjärn", Score = 3},
		// 	new PlayerScore{Name = "Snigel", Score = -4},
		// };
        // System.IO.File.WriteAllText("hello.txt", "hello!");

        // var list = _file.LoadScores();
        // foreach ((int i, PlayerScore item) in list.Enumerate()) {
        //     AddItem(i.ToString(), null, false);
        //     AddItem(item.Name, null, false);
        //     AddItem(item.Score.ToString(), null, false);
        // }
        // _file.SaveScores(new List<PlayerScore> {
		// 	new PlayerScore{Name = "Palettblad", Score = 70},
		// 	new PlayerScore{Name = "Snudin", Score = 8},
		// 	new PlayerScore{Name = "Satan", Score = 6},
		// 	new PlayerScore{Name = "Bjärn", Score = 3},
		// 	new PlayerScore{Name = "Snigel", Score = -4},
		// });
        // AddItem("1.", null, false);
        // AddItem("satan", null, false);
        // AddItem("83", null, false);
        // AddItem("2.", null, false);
        // AddItem("Göran", null, false);
        // AddItem("24", null, false);
    }
}
