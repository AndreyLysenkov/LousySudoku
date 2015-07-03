using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    /// <summary>
    /// Описывает блок чисел
    /// Блок чисел - это массив чисел, подчиняющийся одному правилу (нет повторяющихся чисел)
    /// </summary>
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

        public Number[] Check()
        {
            int[] result_indexes = Check(this.GetValues(), this.GetValuesMask());
            Number[] result = new Number[result_indexes.Length];
            for (int i = 0; i < result_indexes.Length; i++)
            {
                result[i] = children[result_indexes[i]];
            }
            return result;
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
