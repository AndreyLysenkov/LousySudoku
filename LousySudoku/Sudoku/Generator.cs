using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{
    public class Generator
    {

        private Random random;

        public int AttemptsRemain
        {
            get;
            private set;
        }

        public Generator(int attemptsCount = 10000)
        {
            this.random = new Random();
            this.AttemptsRemain = attemptsCount;
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
                cell.Modify(ReturnRandomFromArray(number));
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

        public bool FillSudoku(Sudoku sudoku)
        {
            for (; this.AttemptsRemain > 0; this.AttemptsRemain--)
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
