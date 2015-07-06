using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
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
            Console.WriteLine(" - - - - - - - AL");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write("{0}   ", sudoku.GetNumber(new Number.Position(i, j)).Value);
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
                    Console.Write("{0}   ", (sudoku.GetNumber(new Number.Position(i, j)).IsRight()) ? 1 : 0);
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
            Sudoku sudoku = new Sudoku(new Number.Position(9, 9), value, mask, block, 9);

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
            return number.Value.ToString() + "|" + number.Type + "|" + CoordinateToString(number.Coordinates);
        }

        public static string BlockToString(Block block)
        {
            string result = "BLOCK: ";
            for (int i = 0; i < block.Children.Length; i++)
            {
                result += " ++ " + NumberToString(block.Children[i]);
            }
            return result;
        }

        public static string PrintBlocks(Sudoku sudoku)
        {
            string result = "Printing.. ";
            for (int i = 0; i < sudoku.Block.Length; i++)
            {
                result += "\n" + "#" + i.ToString() + " " + BlockToString(sudoku.Block[i]);
            }
            return result;
        }

        public static Sudoku GetStandart9(int[,] numbs)
        {


            Number.Position[][] block = new Number.Position[9 + 9 + 9][];

            ///Добавление блоков (горизонтальные линии);
            for (int i = 0; i < 9; i++)
            {
                block[i] = new Number.Position[9];
                for (int j = 0; j < 9; j++)
                {
                    block[i][j] = new Number.Position(i, j);
                }
            }

            ///Добавление блоков (вертикальные линии);
            for (int i = 0; i < 9; i++)
            {
                block[9 + i] = new Number.Position[9];
                for (int j = 0; j < 9; j++)
                {
                    block[9 + i][j] = new Number.Position(j, i);
                }
            }

            ///Добавление блоков 3x3;
            int blockIndex = 9 + 9;
            for (int i = 0; i < 9; i += 3)
            {
                for (int j = 0; j < 9; j += 3, blockIndex++)
                {
                    block[blockIndex] = new Number.Position[9];
                    int cellIndex = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++, cellIndex++)
                        {
                            block[blockIndex][cellIndex] = new Number.Position(i + k, j + l);
                        }
                    }
                }
            }

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
                    mask[i, j] = Number.NumberType.Empty;
                }
            }

            return new Sudoku(new Number.Position(9, 9), value, mask, block, 9);
        }
    }

}