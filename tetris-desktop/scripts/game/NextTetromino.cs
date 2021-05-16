using Godot;
using System;
using TetrisBackend;

public class NextTetromino : Panel, ITetrisStateObserver
{
	public override void _Ready()
	{
		BackendInstance.Game.AddStateObserver(this);
	}
	public override void _ExitTree()
	{
		BackendInstance.Game.RemoveStateObserver(this);
	}

    public override void _Notification(int what)
    {
        base._Notification(what);

		// Keeps the panel square.
		if (what == NotificationResized && RectSize.x != RectMinSize.y)
		{
			RectMinSize = new Vector2(0, RectSize.x);
		}
    }

	Tetromino _tetromino;

	void ITetrisStateObserver.HandleNewTetromino(Tetromino newTetromino, Tetromino nextTetromino)
	{
		_tetromino = nextTetromino;
		Update();        
	}

	public override void _Draw()
	{
		if (_tetromino == null)
		{
			return;
		}
		var cellWidth = RectSize.x/_tetromino.Width;
		BoardUtils.DrawTetrisGrid(this, _tetromino, cellWidth);
	}
}
