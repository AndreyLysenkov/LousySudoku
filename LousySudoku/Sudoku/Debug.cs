﻿using System;
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

        public static void ShowSudoku(Sudoku sudoku, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(sudoku.ReturnNumberByPosition(new Number.Position(i, j)).Value);
                }
                Console.WriteLine();
            }
        }

        public static void TestSudoku1()
        {
            Console.WriteLine("Starting TestSudoku1()");

            int[,] value = new int[9, 9];
            Number.NumberType[,] mask = new Number.NumberType[9, 9];
            Number.Position[][] block = new Number.Position[9][];
            for (int i = 0; i < 9; i++)
            {
                block[i] = new Number.Position[9];
            }
            Console.WriteLine("temp vars initialized");

            Console.WriteLine("Call constructor Sufoku");
            Sudoku sudoku = new Sudoku(new Number.Position(9, 9), value, mask, block);

            Console.WriteLine("Call method Debug.ShowSudoku");
            ShowSudoku(sudoku, 9);
        }
    }
}
