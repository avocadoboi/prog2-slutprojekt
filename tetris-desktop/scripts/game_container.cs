using Godot;
using System;
using tetris_backend;

public class game_container : HBoxContainer, ITetrisStateObserver
{
	private TetrisGame _game = new TetrisGame(Constants.board_size, new PlayerScoreFile(Constants.score_file));

	public TetrisGame Game => _game;

	public override void _Ready()
	{
		_game.AddObserver(this);
	}

	float time_since_last_step;

	public override void _Process(float delta)
	{
		time_since_last_step += delta;
		if (time_since_last_step > Constants.tetris_step_interval)
		{
			_game.Step();
			GetNode<board>("board").Update();
			time_since_last_step -= Constants.tetris_step_interval;
		}
	}
}
