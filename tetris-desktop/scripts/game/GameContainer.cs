using Godot;
using System;
using TetrisBackend;

/*
	Manages game input and updates the game.
*/
public class GameContainer : HBoxContainer, ITetrisStateObserver
{
	private float _stepInterval = TetrisLevel.CalculateSecondsPerCell(0);
	private bool _isSoftDropActivated = false;
	private float _timeSinceLastStep;
	
	public override void _Ready()
	{
		BackendInstance.Game.AddStateObserver(this);
		BackendInstance.Game.Restart();
	}
	public override void _ExitTree()
	{
		BackendInstance.Game.RemoveStateObserver(this);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		base._UnhandledInput(@event);

		if (@event.IsActionPressed("move_left", true))
		{
			BackendInstance.Game.GiveInput(TetrisInput.Left);
		}
		else if (@event.IsActionPressed("move_right", true))
		{
			BackendInstance.Game.GiveInput(TetrisInput.Right);
		}
		else if (@event.IsActionPressed("rotate_right"))
		{
			BackendInstance.Game.GiveInput(TetrisInput.RotateRight);
		}
		else if (@event.IsActionPressed("rotate_left"))
		{
			BackendInstance.Game.GiveInput(TetrisInput.RotateLeft);
		}
		else if (@event.IsActionPressed("drop_tetromino"))
		{
			BackendInstance.Game.GiveInput(TetrisInput.Drop);
			GetNode<AudioStreamPlayer>("../bonk").Play();
		}
		else if (@event.IsActionPressed("hold_tetromino"))
		{
			BackendInstance.Game.GiveInput(TetrisInput.HoldTetromino);
		}
		else if (@event.IsActionPressed("soft_drop"))
		{
			_ActivateSoftDrop();
		}
		else if (@event.IsActionReleased("soft_drop"))
		{
			_DeactivateSoftDrop();
		}
	}

	void _ActivateSoftDrop()
	{
		if (!_isSoftDropActivated)
		{
			_isSoftDropActivated = true;
			_timeSinceLastStep = 0f;
		}
	}
	void _DeactivateSoftDrop()
	{
		if (_isSoftDropActivated)
		{
			_isSoftDropActivated = false;
			// _timeSinceLastStep = 0f;
		}
	}

	void ITetrisStateObserver.HandleGameOver()
	{
		GetTree().ChangeScene("res://scenes/game_over.tscn");
	}
	void ITetrisStateObserver.HandleTetrominoUpdated(TetrominoUpdate newTetrominoes)
	{
		_DeactivateSoftDrop();
	}

	void ITetrisStateObserver.HandleLevelUp(int newLevel)
	{
		_stepInterval = TetrisLevel.CalculateSecondsPerCell(newLevel);
	}

	private float backgroundHue = 0f;

	private void _UpdateBackgroundHue()
	{
		VisualServer.SetDefaultClearColor(Color.FromHsv(backgroundHue, 0.4f, 1f));
		backgroundHue += 0.0001f*(float)Math.Pow(BackendInstance.Game.CurrentLevel, 2);
	}

	public override void _Process(float delta)
	{
		// The interval in seconds between two game steps.
		// It is faster if soft drop is activated (unless the game has progressed very far).
		var actualInterval = _isSoftDropActivated ? Math.Min(Constants.softDropInterval, _stepInterval) : _stepInterval;
		
		_timeSinceLastStep += delta;
		while (_timeSinceLastStep > actualInterval)
		{
			_timeSinceLastStep -= actualInterval;

			BackendInstance.Game.Step();
		}

		_UpdateBackgroundHue();
	}
}
