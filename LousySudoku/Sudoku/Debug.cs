using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public class Debug
    {
        public static string TestString()
        {
            return "This is the test string prove you didn't mess up... yet, %developername%";
        }

        public static void TestMessage()
        {
            Console.Write("This just for... reasons unknown");
        }

        public static void TestSudoku1()
        {
            int[,] value = new int[9, 9];
            Number.NumberType[,] mask = new Number.NumberType[9, 9];
            Number.Position[,] block = new Number.Position[9, 9];
            Sudoku sudoku = new Sudoku(value, mask, block);

        }
    }
}
