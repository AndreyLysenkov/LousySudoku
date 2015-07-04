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

            Console.ReadLine();
        }
    }
}
