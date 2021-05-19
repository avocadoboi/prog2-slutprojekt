using TetrisBackend;

/*
	Shows the next tetromino in a game.
*/
public class NextTetromino : TetrominoDisplay
{
	protected override Tetromino _GetDisplayTetromino(TetrominoUpdate tetrominoes)
	{
		return tetrominoes.Next;
	}
}
