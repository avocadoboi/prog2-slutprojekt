using Godot;
using TetrisBackend;

public abstract class TetrominoDisplay : Panel, ITetrisStateObserver
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

	private Tetromino _tetromino;

	/*
		Returns the tetromino to display in the panel control.
	*/
	protected abstract Tetromino _GetDisplayTetromino(TetrominoUpdate update);

	void ITetrisStateObserver.HandleTetrominoUpdated(TetrominoUpdate newTetrominoes)
	{
		var displayTetromino = _GetDisplayTetromino(newTetrominoes);
		if (displayTetromino != _tetromino)
		{
			_tetromino = displayTetromino;
			Update();        
		}
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