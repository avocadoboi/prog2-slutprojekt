using Godot;
using System.Collections.Generic;
using TetrisBackend;

/*
	Loads and holds the different tetromino textures.
*/
public class TetrominoTextures : Node
{
	private Dictionary<TetrisCell, Texture> _textures = new Dictionary<TetrisCell, Texture> 
	{
		{ TetrisCell.I, _LoadCell("I.jpg") },
		{ TetrisCell.J, _LoadCell("J.jpg") },
		{ TetrisCell.L, _LoadCell("L.jpg") },
		{ TetrisCell.O, _LoadCell("O.jpg") },
		{ TetrisCell.S, _LoadCell("S.jpg") },
		{ TetrisCell.T, _LoadCell("T.jpg") },
		{ TetrisCell.Z, _LoadCell("Z.jpg") },
	};

	public Dictionary<TetrisCell, Texture> Textures => _textures;

	private static Texture _LoadCell(string fileName) {
		var texture = GD.Load<Texture>("res://assets/images/" + fileName);
		texture.Flags = (uint)(Texture.FlagsEnum.AnisotropicFilter | Texture.FlagsEnum.Filter | Texture.FlagsEnum.Mipmaps);
		return texture;
	}
}
