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
            for (int i = 0; i < children.Length - 1; i++)
            {
                for (int j = i + 1; j < children.Length; j++)
                {
                    if (children[i].value == children[j].value)
                    {
                        Array.Resize(ref result, result.Length + 2);
                        result[result.Length - 1] = i;
                        result[result.Length - 2] = j;
                    }
                }
            }
            return result;
        }

    }
}
