using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public static class Debug
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
            Console.WriteLine(" / / / / / / AL");
        }

        public static void ShowSudokuRightness(Sudoku sudoku, int size)
        {
            Console.WriteLine(" - - - - - - - AL");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write("{0}   ", sudoku.ReturnNumberByPosition(new Number.Position(i, j)).IsRight());
                }
                Console.WriteLine();
            }
            Console.WriteLine(" / / / / / / AL");
        }

        public static Sudoku TestSudoku1()
        {
            Console.WriteLine("Starting TestSudoku1()");

            int[,] value = new int[9, 9];
            //////////////
            Random rand = new Random();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    value[i, j] = rand.Next(9);
                }
            }
            ////////////////
            
            Number.NumberType[,] mask = new Number.NumberType[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    mask[i, j] = Number.NumberType.Modify;
                }
            }

            Number.Position[][] block = new Number.Position[9][];
            for (int i = 0; i < 9; i++)
            {
                block[i] = new Number.Position[9];
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    block[i][j] = new Number.Position(i , j);
                }
            }
            Console.WriteLine("temp vars initialized");

            Console.WriteLine("Call constructor Sufoku");
            Sudoku sudoku = new Sudoku(new Number.Position(9, 9), value, mask, block);

            Console.WriteLine("Call method Debug.ShowSudoku");
            ShowSudoku(sudoku, 9);

            Console.WriteLine("Return sudoku. Method TestSudoku1() ends here");
            return sudoku;
        }

        public static string CoordinateToString(Number.Position coordinates)
        {
            return coordinates.X + ";" + coordinates.Y;
        }

        public static string NumberToString(Number number)
        {
            return number.Value.ToString() + "|" + number.type + "|" + CoordinateToString(number.position);
        }

        public static string BlockToString(Block block)
        {
            string result = "BLOCK: ";
            for (int i = 0; i < block.children.Length; i++)
            {
                result += " ++ " + NumberToString(block.children[i]);
            }
            return result;
        }

        public static string PrintBlocks(Sudoku sudoku)
        {
            string result = "Printing.. ";
            for (int i = 0; i < sudoku.block.Length; i++)
            {
                result += "\n" + "#" + i.ToString() + " " + BlockToString(sudoku.block[i]);
            }
            return result;
        }

    }

}