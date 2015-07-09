using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LousySudoku;

namespace Dbg_runner
{
    class Program
    {
        static void Main(string[] args)
        {
            //LousySudoku.Sudoku sudoku = Debug.TestSudoku1();

            //Console.WriteLine("Change number");
            //sudoku.ChangeNumber(new Number.Position(2, 1), 9);

            //Console.WriteLine("Print sudoku");
            //Debug.ShowSudoku(sudoku, 9);

            ///Debug.TryLoadDll();

            //Console.ReadLine();

            //Debug.TryLoadDll("Sudoku.dll");

            //Console.ReadLine();

            ///Debug.ShowSudoku(Debug.SudokuEventTest(Debug.GetStandart9(null)), 9);
            //Sudoku sudoku = Debug.GetStandart25(null);
            //(new Generator(sudoku, 10, 1)).Generate();
            //Debug.ShowSudoku(sudoku, 25);

            Debug.TestSudokuFiles();

            Debug.TestGeneration();

            Console.ReadLine();
        }
    }
}
