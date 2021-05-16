using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TetrisBackend;

public static class BoardUtils
{
	/*
		Draws a grid of tetris cells onto any canvas node, with a given cell width.
	*/
	public static void DrawTetrisGrid(CanvasItem node, IGrid<TetrisCell> grid, float cellWidth)
	{
		var textures = node.GetNode<TetrominoTextures>("/root/gui/tetromino_textures").Textures;
		
		foreach (var pos in Utils.Range2D(grid.Cells.GetSize()))
		{
			if (grid.Cells[pos.Y][pos.X] != TetrisCell.Empty)
			{
				node.DrawTextureRect(
					textures[grid.Cells[pos.Y][pos.X]], 
					new Rect2(pos.X*cellWidth, pos.Y*cellWidth, cellWidth, cellWidth), 
					false
				);
			}
		}
	}
}

public class Board : Panel, ITetrisStateObserver
{
	private float _cellWidth;

	public override void _Ready()
	{
		BackendInstance.Game.AddStateObserver(this);
		
		_cellWidth = RectSize.y / Constants.boardSize.Y;
		RectMinSize = new Vector2(_cellWidth * Constants.boardSize.X, _cellWidth * Constants.boardSize.Y);
	}
	public override void _ExitTree()
	{
		BackendInstance.Game.RemoveStateObserver(this);
	}

	TetrisBoard _board;

	void ITetrisStateObserver.HandleBoardUpdated(TetrisBoard newBoard)
	{
		_board = newBoard;
		Update();
	}

	public override void _Draw()
	{
		if (_board == null)
		{
			return;
		}

		BoardUtils.DrawTetrisGrid(this, _board, _cellWidth);
	}
}
