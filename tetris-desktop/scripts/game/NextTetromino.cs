using TetrisBackend;

public class NextTetromino : TetrominoDisplay
{
	protected override Tetromino _GetDisplayTetromino(TetrominoUpdate tetrominoes)
	{
		return tetrominoes.Next;
	}
}
