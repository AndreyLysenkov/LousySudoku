using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public class Block
    {

        Number[] children;

        public Block()
        {
            children = null;
        }

        public virtual int[] Check()
        {
            int[] result = new int[0];

            return result;
        }

    }
}
