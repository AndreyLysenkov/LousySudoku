using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public static class Interface
    {

        public static Sudoku CreateSudoku()
        {
            return null;
        }

        public static bool ChangeNumber(Sudoku sudoku, Number.Position number_position, int value)
        {
            return sudoku.ChangeNumber(number_position, value);
        }

    }
}
