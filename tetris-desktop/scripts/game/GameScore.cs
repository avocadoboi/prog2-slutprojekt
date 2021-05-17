using Godot;
using System;
using TetrisBackend;

public class GameScore : Label, ITetrisStateObserver
{
    public override void _Ready()
    {
		BackendInstance.Game.AddStateObserver(this);
    }
	public override void _ExitTree()
	{
		BackendInstance.Game.RemoveStateObserver(this);
	}

	void ITetrisStateObserver.HandleGameStart()
	{
		(this as ITetrisStateObserver).HandleScored(BackendInstance.Game.CurrentScore);
	}
    void ITetrisStateObserver.HandleScored(CurrentScore newScore)
	{
		Text = $"Score: {newScore.Points}\nLines: {newScore.Lines}\nLevel: {BackendInstance.Game.CurrentLevel}";
	}
}
