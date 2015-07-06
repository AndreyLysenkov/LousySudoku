using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public class Generator
    {

        private Random random;

        public Generator()
        {
            this.random = new Random();
        }

        private int ReturnRandomFromArray(int[] number)
        {
            if (number.Length == 0)
            {
                return -1;
            }
            return random.Next(number.Length);
        }

    }
}
