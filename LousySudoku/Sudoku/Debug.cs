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
                    Console.Write("{0:00}  ", sudoku.GetNumber(new Number.Position(i, j)).Value);
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
            return coordinates.X.ToString() + ";" + coordinates.Y.ToString();
        }

        public static string NumberToString(Number number)
        {
            return number.Value.ToString() + "|" + number.Type.ToString() + "|" + CoordinateToString(number.Coordinates);
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

        public static Sudoku GetStandart12(int[,] numbs)
        {


            Number.Position[][] block = new Number.Position[12 + 12 + 12][];

            ///Добавление блоков (горизонтальные линии);
            for (int i = 0; i < 12; i++)
            {
                block[i] = new Number.Position[12];
                for (int j = 0; j < 12; j++)
                {
                    block[i][j] = new Number.Position(i, j);
                }
            }

            ///Добавление блоков (вертикальные линии);
            for (int i = 0; i < 12; i++)
            {
                block[12 + i] = new Number.Position[12];
                for (int j = 0; j < 12; j++)
                {
                    block[12 + i][j] = new Number.Position(j, i);
                }
            }

            ///Добавление блоков 4x4;
            int blockIndex = 12 + 12;
            for (int i = 0; i < 12; i += 3)
            {
                for (int j = 0; j < 12; j += 4, blockIndex++)
                {
                    block[blockIndex] = new Number.Position[12];
                    int cellIndex = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 4; l++, cellIndex++)
                        {
                            block[blockIndex][cellIndex] = new Number.Position(i + k, j + l);
                        }
                    }
                }
            }

            int[,] value = new int[12, 12];
            //////////////
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    value[i, j] = 0;
                }
            }
            ////////////////

            Number.NumberType[,] mask = new Number.NumberType[12, 12];
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    mask[i, j] = Number.NumberType.Empty;
                }
            }

            return new Sudoku(new Number.Position(12, 12), value, mask, block, 12);
        }

        public static Sudoku GetStandart25(int[,] numbs)
        {


            Number.Position[][] block = new Number.Position[25 + 25 + 25][];

            ///Добавление блоков (горизонтальные линии);
            for (int i = 0; i < 25; i++)
            {
                block[i] = new Number.Position[25];
                for (int j = 0; j < 25; j++)
                {
                    block[i][j] = new Number.Position(i, j);
                }
            }

            ///Добавление блоков (вертикальные линии);
            for (int i = 0; i < 25; i++)
            {
                block[25 + i] = new Number.Position[25];
                for (int j = 0; j < 25; j++)
                {
                    block[25 + i][j] = new Number.Position(j, i);
                }
            }

            ///Добавление блоков 4x4;
            int blockIndex = 25 + 25;
            for (int i = 0; i < 25; i += 5)
            {
                for (int j = 0; j < 25; j += 5, blockIndex++)
                {
                    block[blockIndex] = new Number.Position[25];
                    int cellIndex = 0;
                    for (int k = 0; k < 5; k++)
                    {
                        for (int l = 0; l < 5; l++, cellIndex++)
                        {
                            block[blockIndex][cellIndex] = new Number.Position(i + k, j + l);
                        }
                    }
                }
            }

            int[,] value = new int[25, 25];
            //////////////
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    value[i, j] = 0;
                }
            }
            ////////////////

            Number.NumberType[,] mask = new Number.NumberType[25, 25];
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    mask[i, j] = Number.NumberType.Empty;
                }
            }

            return new Sudoku(new Number.Position(25, 25), value, mask, block, 25);
        }

        public static void TestGeneration()
        {
            int[,] numbs = new int[25, 25];
            for (int i = 0; i < 25; i++)
                for (int j = 0; j < 25; j++)
                    numbs[i, j] = 0;

            Sudoku sudoku9 = GetStandart9(numbs);
            Stopwatch time9 = new Stopwatch();
            Generator generator9 = new Generator(sudoku9, fillness: 1);
            Console.WriteLine(generator9.AttemptsRemain);
            time9.Start();
            Console.WriteLine(generator9.Generate());
            time9.Stop();
            ShowSudoku(sudoku9, 9);
            string Sudoku = ((IStringify)(sudoku9)).Stringify(Stringify_Help.CopyList(Stringify_Help.SeparatorListDefault));
            Console.WriteLine(Sudoku);
            Sudoku sudoku0 = (Sudoku)((IStringify)(sudoku9)).Unstringify(Sudoku, Stringify_Help.CopyList(Stringify_Help.SeparatorListDefault));
            Console.WriteLine("Go");
            ShowSudoku(sudoku0, 9);
            PrintBlocks(sudoku0);
            Sudoku sudoku16 = GetStandart16(numbs);
            Stopwatch time16 = new Stopwatch();
            Generator generator16 = new Generator(sudoku16, attemptsNumber: 0, fillness: 1);
            ///sudoku16.MixNumbers();
            //Console.WriteLine("Sudoku. Разбор полетов и template: \n {0}", SudokuCoordinates(sudoku16));
            Console.WriteLine(generator16.AttemptsRemain);
            time16.Start();
            Console.WriteLine(generator16.Generate());
            time16.Stop();
            ShowSudoku(sudoku16, 16);

            Console.WriteLine("\n \n {0}x9x{2} against {1}x16x{3}", time9.Elapsed, time16.Elapsed, generator9.AttemptsRemain, generator16.AttemptsRemain);
        }

        public static void TryLoadDll(string filename = "data\\block\\standart.dll", string methodname = "Debug_DllMethod")
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

        public static string BlockCoordinates(Block block)
        {
            string result = "";
            for (int i = 0; i < block.Children.Length; i++)
            {
                result += "    " + CoordinateToString(block.Children[i].Coordinates);
            }
            return result;
        }

        public static string SudokuCoordinates(Sudoku sudoku)
        {
            string result = "";
            for(int i = 0; i < sudoku.Block.Length; i++)
            {
                result += String.Format("Block #{0} : {1} \n", i, BlockCoordinates(sudoku.Block[i]));
            }
            return result;
        }

        public static void SudokuEventTestSubscriber1(Sudoku sudoku)
        {
            Console.WriteLine("{0} congrats, completed", 5);
        }

        public static void SudokuEventTestSubscriber2(Sudoku sudoku)
        {
            Console.WriteLine("{0} congrats, completed", 0);
        }

        public static Sudoku SudokuEventTest(Sudoku sudoku)
        {
            Generator generator = new Generator(sudoku, fillness: 1);
            generator.Generate();
            sudoku.OnCompleted += SudokuEventTestSubscriber1;
            sudoku.OnFilled += SudokuEventTestSubscriber2;
            sudoku.ChangeNumber(new Number.Position(0, 0), sudoku.GetNumber(new Number.Position(0, 0)).Value);
            return sudoku;
        }

        public static void TestSudokuFile(string file = "data\\templates\\standart.txt", int attempt = 0)
        {
            Console.WriteLine("File: {0}", file);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Sudoku sudoku = Interface.LoadSudoku(file);
            bool success = (new Generator(sudoku, attemptsNumber: attempt, fillness: 1)).Generate();
            Debug.ShowSudoku(sudoku, sudoku.Size.X);
            timer.Stop();
            Console.WriteLine("Generated in {0}ms with {1}", timer.ElapsedMilliseconds, success);
        }

        public static void TestSudokuFiles()
        {
            TestSudokuFile("data\\templates\\standart.txt");
            TestSudokuFile("data\\templates\\standart_x.txt", 1);
            TestSudokuFile("data\\templates\\standart_window.txt", 1);
            TestSudokuFile("data\\templates\\standart_window_x.txt", 1);
            TestSudokuFile("data\\templates\\standart_puzzle.txt", 10);
            TestSudokuFile("data\\templates\\standart_point.txt", 10);
            TestSudokuFile("data\\templates\\standart_12.txt", 1);
            TestSudokuFile("data\\templates\\standart_16.txt", 1);
            //Interface.SaveSudoku("data\\templates\\standart_16.txt", GetStandart16(null));
            //Interface.SaveSudoku("data\\templates\\standart_12.txt", GetStandart12(null));
            Console.ReadLine();
        }
    }

}