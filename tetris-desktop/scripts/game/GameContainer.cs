using Godot;
using System;
using TetrisBackend;

public class GameContainer : HBoxContainer, ITetrisStateObserver
{
	private float _stepInterval = TetrisLevel.CalculateSecondsPerCell(0);
	private bool _isSpeedActivated = false;
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

		if (@event.IsActionPressed("move_left"))
		{
			BackendInstance.Game.GiveInput(TetrisInput.Left);
		}
		else if (@event.IsActionPressed("move_right"))
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
		}
		else if (@event.IsActionPressed("hold_tetromino"))
		{
			BackendInstance.Game.GiveInput(TetrisInput.HoldTetromino);
		}
		else if (@event.IsActionPressed("speed_fall"))
		{
			_ActivateSpeedFall();
		}
		else if (@event.IsActionReleased("speed_fall"))
		{
			_DeactivateSpeedFall();
		}
    }

	void _ActivateSpeedFall()
	{
		if (!_isSpeedActivated)
		{
			_isSpeedActivated = true;
			_timeSinceLastStep = 0f;
		}
	}
	void _DeactivateSpeedFall()
	{
		if (_isSpeedActivated)
		{
			_isSpeedActivated = false;
			// _timeSinceLastStep = 0f;
		}
	}

	void ITetrisStateObserver.HandleGameOver()
	{
		GetTree().ChangeScene("res://scenes/game_over.tscn");
	}
	void ITetrisStateObserver.HandleTetrominoUpdated(TetrominoUpdate newTetrominoes)
	{
		_DeactivateSpeedFall();
	}

	void ITetrisStateObserver.HandleLevelUp(int newLevel)
	{
		_stepInterval = TetrisLevel.CalculateSecondsPerCell(newLevel);
	}

	public override void _Process(float delta)
	{
		var actualInterval = _isSpeedActivated ? 1f/30f : _stepInterval;
		
		_timeSinceLastStep += delta;
		while (_timeSinceLastStep > actualInterval)
		{
			_timeSinceLastStep -= actualInterval;

			BackendInstance.Game.Step();
		}
	}
}
