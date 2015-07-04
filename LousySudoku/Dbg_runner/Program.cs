using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sudoku;

namespace Dbg_runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Sudoku.Sudoku sudoku = Debug.TestSudoku1();

            Console.WriteLine("Change number");
            sudoku.ChangeNumber(new Number.Position(2, 1), 9);

            Console.WriteLine("Print sudoku");
            Debug.ShowSudoku(sudoku, 9);

            Console.ReadLine();
        }
    }
}
