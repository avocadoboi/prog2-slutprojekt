using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using tetris_backend;

public class board : Panel
{
	private TetrisGame _game;
	private float _cell_width;
	
	private Dictionary<TetrisCell, Texture> _textures = new Dictionary<TetrisCell, Texture> 
	{
		{ TetrisCell.I, _CreateCell("I.jpg") },
		{ TetrisCell.J, _CreateCell("J.jpg") },
		{ TetrisCell.L, _CreateCell("L.jpg") },
		{ TetrisCell.O, _CreateCell("O.jpg") },
		{ TetrisCell.S, _CreateCell("S.jpg") },
		{ TetrisCell.T, _CreateCell("T.jpg") },
		{ TetrisCell.Z, _CreateCell("Z.jpg") },
	};

	public override void _Ready()
	{
		_cell_width = RectSize.y / Constants.board_size.Y;
		RectMinSize = new Vector2(_cell_width * Constants.board_size.X, _cell_width * Constants.board_size.Y);
		
		_game = GetNode<game_container>("../").Game;
	}

	private static Texture _CreateCell(string file_name) {
		return GD.Load<Texture>("res://assets/images/" + file_name);
	}

	public override void _Draw()
	{
		if (_game == null) 
		{
			return;
		}
		foreach (var pos in Utils.Range2D(_game.Board.Size))
		{
			if (_game.Board.Cells[pos.Y][pos.X] != TetrisCell.Empty)
			{
				DrawTextureRect(
					_textures[_game.Board.Cells[pos.Y][pos.X]], 
					new Rect2(pos.X*_cell_width, pos.Y*_cell_width, _cell_width, _cell_width), 
					false
				);
			}
		}
	}
}
