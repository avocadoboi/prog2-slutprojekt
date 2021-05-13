using System.Linq;
using System;

namespace tetris_backend {

public enum TetrisCell {
    Empty, I, J, L, O, S, T, Z
}

/*
    A tetromino is a square grid of tetris cells that can be constructed 
    conveniently from a grid of cell flags and a cell type.
*/
class Tetromino : SquareGrid<TetrisCell> {
    public TetrisCell Type { get; private set; }

    /*
        Initializes the tetromino by transposing cell_flags and replacing zeros 
        with empty tetris cells and ones with the provided tetris cell type.
    */
    public Tetromino(TetrisCell type, int[,] cell_flags) {
        Type = type;
        Cells = new TetrisCell[cell_flags.GetLength(0), cell_flags.GetLength(1)];

        foreach (var (x, y) in cell_flags.Indices()) {
            // Here we transpose the grid, because the declaration of a 2D array is 
            // column-first but written as rows in the code (see the Tetrominoes class).
            Cells[y, x] = cell_flags[x, y] == 0 ? TetrisCell.Empty : type;
        }
    }
    /*
        Copy constructor.
    */
    public Tetromino(Tetromino other) =>
        (Type, Cells) = (other.Type, other.Cells.Clone() as TetrisCell[,]);
}

static class Tetrominoes {
    public static Tetromino GetRandom() {
        return tetrominoes[new Random().Next(tetrominoes.Length)];
    }
    
    public static Tetromino[] tetrominoes = {
        new Tetromino(TetrisCell.I, new int[,] {
            {0, 0, 0, 0},
            {1, 1, 1, 1},
            {0, 0, 0, 0},
            {0, 0, 0, 0},
        }),
        new Tetromino(TetrisCell.J, new int[,] {
            {1, 0, 0},
            {1, 1, 1},
            {0, 0, 0},
        }),
        new Tetromino(TetrisCell.L, new int[,] {
            {0, 0, 1},
            {1, 1, 1},
            {0, 0, 0},
        }),
        new Tetromino(TetrisCell.O, new int[,] {
            {1, 1},
            {1, 1},
        }),
        new Tetromino(TetrisCell.S, new int[,] {
            {0, 1, 1},
            {1, 1, 0},
            {0, 0, 0},
        }),
        new Tetromino(TetrisCell.T, new int[,] {
            {0, 1, 0},
            {1, 1, 1},
            {0, 0, 0},
        }),
        new Tetromino(TetrisCell.Z, new int[,] {
            {1, 1, 0},
            {0, 1, 1},
            {0, 0, 0},
        })
    };
}

}
