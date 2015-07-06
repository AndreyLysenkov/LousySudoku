using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{
    public class Generator
    {

        private Random random;

        public Generator()
        {
            this.random = new Random();
        }

        private int ReturnRandomFromArray(List<int> number)
        {
            if (number.Count == 0)
            {
                return -1;
            }
            int index = random.Next(number.Count);
            int result = number[index];
            number.RemoveAt(index);
            return result;
        }

        private bool FillCell(Number cell, int maxValue)
        {
            List<int> number = new List<int> {};
            for (int i = 1; i <= maxValue; i++)
            {
                number.Add(i);
            }
            Console.WriteLine("Starting number right value searching"); ///NNBB! Debug;
            do
            {
                cell.Modify(ReturnRandomFromArray(number));
            } while (cell.IsRight() || (number.Count != 0));
                Console.WriteLine("Number random length: {0}, success: {1}", number.Count, cell.IsRight()); ///NNBB! Debug;
            return cell.IsRight();
        }

        private bool FillSudokuOneAttempt(Sudoku sudoku)
        {
            for (int i = 0; i < sudoku.Number.Length; i++)
            {
                Console.WriteLine("Starting fill cell #{0}", i); ///NNBB! Debug;
                bool success = FillCell(sudoku.Number[i], sudoku.MaxValue);
                Console.WriteLine("Filled cell #{0} with {1}", i, success); ///NNBB! Debug;
                if (!success)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FillSudoku(Sudoku sudoku, int attempts = 100)
        {
            for (int i = 0; i < attempts; i++)
            {
                Console.WriteLine("Starting attempt #{0}", i); ///NNBB! Debug;
                bool success = FillSudokuOneAttempt(sudoku);
                Console.WriteLine("Ended attempt #{0} with {1}", i, success); ///NNBB! Debug;
                if (success)
                {
                    Console.WriteLine("Total #{0} attempts", i); ///NNBB! Debug;
                    return true;
                }
                else
                {
                    sudoku.Clear();
                }
            }
            return false;
        }

    }
}
