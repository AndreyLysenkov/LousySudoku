using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            sudoku.OnCompleted += DoOnCompleted;
            do
            {
                Debug.ShowSudoku(sudoku, sudoku.Size.X);
                Console.Write("Enter position x, y, value: ");
                string[] values = (Console.ReadLine()).Split(new char[] {' '}, 3);
                sudoku.ChangeNumber(new Number.Position(Convert.ToInt32(values[0]), Convert.ToInt32(values[1])), Convert.ToInt32(values[2]));
            } while (!completed);
            Console.WriteLine("Congrats! Completed! \n Press Enter");
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            Console.Write("Enter secret code, to run debug. Or press enter: ");
            string s = Console.ReadLine();
            if (s != "2713")
            {
                Run();
                return;
            }
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

            ///Debug.TestGeneration();

            Debug.TestSudokuFiles();

            Console.ReadLine();
        }
    }
}
