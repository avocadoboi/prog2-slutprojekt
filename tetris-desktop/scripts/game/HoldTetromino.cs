using TetrisBackend;

/*
	Shows the current hold tetromino in a game.
*/
public class HoldTetromino : TetrominoDisplay
{
	protected override Tetromino _GetDisplayTetromino(TetrominoUpdate tetrominoes)
	{
		return tetrominoes.Hold;
	}
}
