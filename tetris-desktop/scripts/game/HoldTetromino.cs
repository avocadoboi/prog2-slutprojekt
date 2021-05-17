using TetrisBackend;

public class HoldTetromino : TetrominoDisplay
{
	protected override Tetromino _GetDisplayTetromino(TetrominoUpdate tetrominoes)
	{
		return tetrominoes.Hold;
	}
}
