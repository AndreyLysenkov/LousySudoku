using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{
    public class Generator
    {

        public const int AttemptsNumberDefault = 200;

        private Random random;

        private Sudoku sudoku;

        public int AttemptsRemain;

        public Generator(Sudoku sudoku, int attemptsNumber = AttemptsNumberDefault)
        {
            this.random = new Random();
            this.sudoku = sudoku;
            this.AttemptsRemain = this.GetAttempts(attemptsNumber);
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
            if (!cell.IsRight())
            {
                ///Console.WriteLine(" position {0};{1} ", cell.Coordinates.X, cell.Coordinates.Y);
            }
            return cell.IsRight();
        }

        public bool FillSudokuOneAttempt()
        {
            this.AttemptsRemain--;
            for (int i = 0; i < sudoku.Number.Length; i++)
            {
                bool success = FillCell(sudoku.Number[i], sudoku.MaxValue);
                if (!success)
                {
                    ///Debug.ShowSudoku(this.sudoku, this.sudoku.Size.X);
                    return false;
                }
            }
            return true;
        }

        public bool FillSudoku()
        {
            for (; this.AttemptsRemain > 0; )
            {
                bool success = this.FillSudokuOneAttempt();
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

        public int GetAttempts(int attemptsNumber = AttemptsNumberDefault)
        {
            return sudoku.Size.X * sudoku.Size.Y * attemptsNumber;
        }

        public item[] MixItems<item>(item[] array)
        {
            int length = array.Length;
            item[] result = new item[length];
            List<int> index = new List<int> { };
            for (int i = 0; i < length; i++)
            {
                index.Add(i);
            }

            for (int j = 0; j < length; j++ )
            {
                int fillIndex = ReturnRandomFromArray(index);
                result[j] = array[fillIndex];
            }
            
            return result;
        }

    }
}
