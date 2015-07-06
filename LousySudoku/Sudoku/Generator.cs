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

        private bool FillSudoku(Sudoku sudoku)
        {
            bool success = true;
            

            return success;
        }

    }
}
