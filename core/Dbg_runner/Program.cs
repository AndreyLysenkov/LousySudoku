using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alist;
using LousySudoku;

namespace Dbg_runner
{
    class Program
    {

        public static bool completed = false;

        public static void DoOnCompleted(Sudoku sudoku)
        {
            completed = true;
        }

        public static void Run()
        {
            Console.Write("Enter name: ");
            string filename = "data\\templates\\" + Console.ReadLine() + ".txt";
            Console.Write("Enter fillenes [0.0 - 1.0]: ");
            double fillness = Convert.ToDouble(Console.ReadLine());
            Sudoku sudoku = Interface.GenerateFromTemplate(filename, fillness);
            sudoku.onCompleted += DoOnCompleted;
            do
            {
                Debug.ShowSudoku(sudoku, 0);
                Console.Write("Enter position x, y, value: ");
                string[] values = (Console.ReadLine()).Split(new char[] {' '}, 3);
                if (values[0] == "AL")
                {
                    (new Generator(sudoku, 0)).Complete();
                }
                else
                {
                    sudoku.SetNumber(new Position(Convert.ToInt32(values[0]), Convert.ToInt32(values[1])), Convert.ToInt32(values[2]));
                }
            } while (!completed);
            Console.WriteLine("Congrats! Completed! \n Press Enter");
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            List<Number> number = new List<Number> { };
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    number.Add
                        (new Number(NumberType.Empty, new Position(i, j)));
                }
            }

            List<BlockType> blockType = new List<BlockType> { };
            BlockType standart = new BlockType();
            standart.SetChecker(LousySudoku.Method.CheckMethod_Standart);
            standart.SetGenerator(LousySudoku.Method.GenerateMethod_Standart);
            blockType.Add(standart);

            List<Block> block = new List<Block> { };

            Sudoku sudoku = new Sudoku(blockType, block, number, 9);

            for (int col = 0; col < 9; col++)
            {
                block.Add(new Block(
                    sudoku, 
                    standart,
                    number.FindAll(x => x.Coordinate.GetCoordinate(0) == col)
                    ));
                block.Add(new Block(
                    sudoku, 
                    standart,
                    number.FindAll(x => x.Coordinate.GetCoordinate(1) == col)
                    ));
            }

            for (int i = 0; i < 9; i += 3)
            {
                for (int l = 0; l < 9; l += 3)
                {
                    Block b = new Block(sudoku, standart);
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            b.AddChild(sudoku.GetNumber(new Position(i + j, k + l)));
                        }

                    }
                    block.Add(b);
                }
            }

            Alist.Xml.Transform.ElementToFile(sudoku.UnloadXml(), "standart_9x9.xml");

            for (int i = 0; ; i++)
            {
                bool success = true;
                for (int j = 0; j < sudoku.Block.Count || success; j++)
                {
                    Block b = sudoku.Block[j];
                    success = b.Generate();
                }
                if (i % 1000 == 0)
                    Console.WriteLine("Attempt #{0}", i);
                if (success)
                {
                    Console.WriteLine("Stopped at {0}", i);
                    break;
                }
            }
            
            return; //Tmp;

            Console.Write("Enter secret code, to run debug. Or press enter: ");
            string s = Console.ReadLine();
            if (s != "2713")
            {
                Run();
                return;
            }

            //LousySudoku.Sudoku sudoku = Debug.TestSudoku1();

            //Console.WriteLine("Change number");
            //sudoku.ChangeNumber(new Position(2, 1), 9);

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

            ///Debug.TestGeneration();

            Debug.TestSudokuFiles();

            Console.ReadLine();
        }
    }

}
