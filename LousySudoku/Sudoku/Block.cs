using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public class Block : IStringify
    {

        Number[] children;

        public Block(Number[] children)
        {
            this.children = children;
        }

        private int[] GetValues()
        {
            int[] result = new int[children.Length];
            for (int i = 0; i < children.Length; i++)
            {
                result[i] = children[i].Value;
            }
            return result;
        }

        private bool[] GetValuesMask()
        {
            bool[] result = new bool[children.Length];
            for (int i = 0; i < children.Length; i++)
            {
                result[i] = children[i].HasValue;
            }
            return result;
        }

        protected virtual int[] Check(int[] value, bool[] mask)
        {
            int[] result = new int[0];
            for (int i = 0; i < value.Length; i++)
            {
                for (int j = i + 1; (j < value.Length) && (mask[i]); j++)
                {
                    if ((mask[j]) && (value[i] == value[j]))
                    {
                        Array.Resize(ref result, result.Length + 2);
                        result[result.Length - 1] = i;
                        result[result.Length - 2] = j;
                    }
                }
            }
            return result;
        }

        public int[] Check()
        {
            return Check(this.GetValues(), this.GetValuesMask());
        }

        string IStringify.Stringify()
        {
            return null;
        }

        IStringify IStringify.Unstringify(string value)
        {
            return null;
        }

        public bool IsRight()
        {
            return (this.Check().Length == 0);
        }
    
    }
}
