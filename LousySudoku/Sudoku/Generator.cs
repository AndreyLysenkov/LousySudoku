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
                cell.Modify(ReturnRandomFromArray(number));
            } while (cell.IsRight() || (number.Count != 0));
            return cell.IsRight();
        }

            return success;
        }

    }
}
