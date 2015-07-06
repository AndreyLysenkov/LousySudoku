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
            do
            {
                Console.WriteLine("0: {0};{1}, type: {2}, value: {3}", cell.Coordinates.X, cell.Coordinates.Y, cell.Type, cell.Value); ///NNBB Debug;
                cell.Modify(ReturnRandomFromArray(number));
                Console.WriteLine("1: {0};{1}, type: {2}, value: {3}", cell.Coordinates.X, cell.Coordinates.Y, cell.Type, cell.Value); ///NNBB Debug;
            } while (!cell.IsRight() && (number.Count != 0));
            return cell.IsRight();
        }

        private bool FillSudokuOneAttempt(Sudoku sudoku)
        {
            for (int i = 0; i < sudoku.Number.Length; i++)
            {
                bool success = FillCell(sudoku.Number[i], sudoku.MaxValue);
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
                bool success = FillSudokuOneAttempt(sudoku);
                if (success)
                {
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
