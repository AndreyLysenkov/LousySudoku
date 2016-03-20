using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku.Debug.Runner
{

    public static class Program
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
                Debug.Common.ShowSudoku(sudoku, 0);
                Console.Write("Enter position x, y, value: ");
                string[] values = (Console.ReadLine()).Split(new char[] { ' ' }, 3);
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

        public static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch testTime
                = new System.Diagnostics.Stopwatch();
            testTime.Start();
            LousySudoku.UnitTest.Common.Run(5);
            testTime.Stop();
            Console.WriteLine("Time elapsed: {0}", testTime.Elapsed);

            Console.ReadKey();
            return;

            int size = 9;
            List<Number> number = new List<Number> { };
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    number.Add
                        (new Number(NumberType.Empty, new Position(i, j)));
                }
            }

            //Debug.Print.Position(new Position(17, 5));

            //Debug.Print.Number(number);

            List<BlockType> blockType = new List<BlockType> { };
            BlockType standart = new BlockType();
            //standart.SetChecker(LousySudoku.Method.CheckMethod_Standart);
            //standart.SetGenerator
            //    (LousySudoku.Method.GenerateMethod_Standart);
            blockType.Add(standart);

            List<Block> block = new List<Block> { };

            Sudoku sudoku = new Sudoku(blockType, block, number, size);

            for (int col = 0; col < size; col++)
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

            for (int i = 0; i < size; i += 3)
            {
                for (int l = 0; l < size; l += 3)
                {
                    Block b = new Block(sudoku, standart);
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            b.AddChild(sudoku.GetNumber(
                                new Position(i + j, k + l)));
                        }
                    }
                    block.Add(b);
                }
            }

            //sudoku.Block.ForEach(new Action<Block>(Debug.Print.Block));
            
            Alist.Xml.Transform.ElementToFile
                (sudoku.UnloadXml(), "standart_12x12.xml");

            //LousySudoku.Constant.rand = new Random();

            //foreach (Number numb in sudoku.Number)
            //{
            //    Console.Write("{0}; ", numb.parent.Count);
            //}
            //Console.WriteLine();

            //sudoku.Block.ForEach(x => Console.Write("{0}, ", x.Child.Count));

            Generator g = new Generator(sudoku, 1000000, 1);

            System.Diagnostics.Stopwatch s0 
                = new System.Diagnostics.Stopwatch();
            s0.Start();
            Console.WriteLine(g.Generate());
            s0.Stop();
            Console.WriteLine(s0.Elapsed);


            Console.WriteLine(g.AttemptsRemain);
            Debug.Print.Sudoku2D(sudoku);
            sudoku.Clear();

            //bool isContinue = true;
            //for (int i = 0; isContinue; i++)
            //{
            //    bool success = true;

                //Block generate
                //for (int j = 0; (j < sudoku.Block.Count) && (success); j++)
                //{
                //    success = sudoku.Block[j].Generate();
                //}

                //Number generate
//                for (int j = 0; (j < sudoku.Number.Count) && success; j++)
//                {
//                    success 
//                        = LousySudoku.Method.Generate(sudoku.Number[j]);
//#if DEBUG
//                    if (!success)
//                        ;
//#endif
//                }

//                //List<int> digit = new List<int>
//                //    { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
//                //bool toDo = true;
//                //for (int k = 0; (k < 9) && toDo; k++)
//                //{
//                //    int index = rand.Next(digit.Count - 1);
//                //    int newnumb = digit[index];
//                //    numb.Modify(newnumb);
//                //    digit.Remove(newnumb);
//                //    if (numb.IsBlockRight())
//                //    {
//                //        toDo = false;
//                //    }
//                //}
//                //if (toDo)
//                //    success = false;

//                ///End of attempt
//                if (!sudoku.Block.All(x => x.IsRight()))
//                    success = false;
//                //success = sudoku.IsRight();

//                if (i % 1000 == 0)
//                {
//                    Console.WriteLine("Attempt #{0}", i);
//                    Debug.Print.Sudoku2D(sudoku);
//                    //LousySudoku.Constant.rand = new Random();
//                }

//                if (success)
//                {
//                    Console.WriteLine("Stopped at {0}", i);
//                    Debug.Print.Sudoku2D(sudoku);
//                    isContinue = false;
//                }
//                else
//                {
//                    //sudoku = new Sudoku(blockType, block, number, 9);
//                    sudoku.Clear();
//                }
//            }

            Debug.Common.ShowSudoku(sudoku, size);

            Console.ReadKey();
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

            Debug.Common.TestSudokuFiles();

            Console.ReadLine();
        }
    }

}