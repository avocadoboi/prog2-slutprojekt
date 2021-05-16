using Godot;
using System;
using TetrisBackend;

public class GameContainer : HBoxContainer, ITetrisStateObserver
{
	public override void _Ready()
	{
		BackendInstance.Game.Restart();
		BackendInstance.Game.AddStateObserver(this);
	}
	public override void _ExitTree()
	{
		BackendInstance.Game.RemoveStateObserver(this);
	}

	void ITetrisStateObserver.HandleGameOver()
	{
		GetTree().ChangeScene("res://scenes/game_over.tscn");
	}

	private static float _StepIntervalFromLevel(int level)
	{
		return TetrisLevel.CalculateFramesPerCell(level)/60f;
	}

	float _stepInterval = _StepIntervalFromLevel(0);

	void ITetrisStateObserver.HandleLevelUp(int newLevel)
	{
		_stepInterval = _StepIntervalFromLevel(newLevel);
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
    }

	float timeSinceLastStep;

	public override void _Process(float delta)
	{
		timeSinceLastStep += delta;
		if (timeSinceLastStep > _stepInterval)
		{
			timeSinceLastStep -= _stepInterval;

			BackendInstance.Game.Step();
		}
	}
}
