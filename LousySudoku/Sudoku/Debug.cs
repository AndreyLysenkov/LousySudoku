using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

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
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    value[i, j] = 0;
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

        public static Sudoku GetStandart16(int[,] numbs)
        {


            Number.Position[][] block = new Number.Position[16 + 16 + 16][];

            ///Добавление блоков (горизонтальные линии);
            for (int i = 0; i < 16; i++)
            {
                block[i] = new Number.Position[16];
                for (int j = 0; j < 16; j++)
                {
                    block[i][j] = new Number.Position(i, j);
                }
            }

            ///Добавление блоков (вертикальные линии);
            for (int i = 0; i < 16; i++)
            {
                block[16 + i] = new Number.Position[16];
                for (int j = 0; j < 16; j++)
                {
                    block[16 + i][j] = new Number.Position(j, i);
                }
            }

            ///Добавление блоков 4x4;
            int blockIndex = 16 + 16;
            for (int i = 0; i < 16; i += 4)
            {
                for (int j = 0; j < 16; j += 4, blockIndex++)
                {
                    block[blockIndex] = new Number.Position[16];
                    int cellIndex = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 0; l < 4; l++, cellIndex++)
                        {
                            block[blockIndex][cellIndex] = new Number.Position(i + k, j + l);
                        }
                    }
                }
            }

            int[,] value = new int[16, 16];
            //////////////
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    value[i, j] = 0;
                }
            }
            ////////////////

            Number.NumberType[,] mask = new Number.NumberType[16, 16];
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    mask[i, j] = Number.NumberType.Empty;
                }
            }

            return new Sudoku(new Number.Position(16, 16), value, mask, block, 16);
        }

        public static void TestGeneration()
        {
            int[,] numbs = new int[25, 25];
            for (int i = 0; i < 25; i++)
                for (int j = 0; j < 25; j++)
                    numbs[i, j] = 0;

            Sudoku sudoku9 = GetStandart9(numbs);
            Stopwatch time9 = new Stopwatch();
            Generator generator9 = new Generator(sudoku9, 100);
            time9.Start();
            ///Console.WriteLine(generator9.FillSudoku());
            time9.Stop();
            ShowSudoku(sudoku9, 9);

            Sudoku sudoku16 = GetStandart16(numbs);
            Stopwatch time16 = new Stopwatch();
            time16.Start();
            Console.WriteLine(generator16.FillSudokuOneAttempt());
            time16.Stop();
            ShowSudoku(sudoku16, 16);

            Console.WriteLine("\n \n {0}x9x{2} against {1}x16x{3}", time9.Elapsed, time16.Elapsed, generator9.AttemptsRemain, generator16.AttemptsRemain);
        }

        public static void TryLoadDll(string filename = "block.dll", string methodname = "Debug_DllMethod")
        {
            Assembly block_dll = Assembly.LoadFrom(filename);
            Type[] type = block_dll.GetTypes();
            foreach (Type item in type)
            {
                Console.WriteLine("Extracted: {0}", item.Name);
                MethodInfo[] methods = item.GetMethods();
                foreach (MethodInfo method in methods)
                {
                    Console.WriteLine("   Include: {0}", method.Name);
                    ParameterInfo[] parametr = method.GetParameters();
                    ///bool isCorrect = (parametr.Length >= 2) && (parametr[0].);
                    if (method.Name == methodname)
                    {
                        Console.WriteLine("--------------------------------");
                        method.Invoke(null, new object[0]);
                    }
                }
            }
            
        }

    }

}