using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    public class Sudoku : IStringify
    {

        public class Size
        {
            public int Length
            {
                get;
                private set;
            }

            public int Height
            {
                get;
                private set;
            }

            public Size(int x, int y)
            {
                Length = x;
                Height = y;
            }
        }

        /// <summary>
        /// Содержит поля и методы для представления судоку на экране
        /// </summary>
        public class Visual
        {

            int[,] mask;
            int[,] number;

            public Size size;

            public Visual (Sudoku sudoku)
            {


            }

        }

        Number[] number;

        Number[] block;

        public Size size;

        public Sudoku()
        {
            number = new Number[0];
            block = new Number[0];
            size = new Size(9, 9);
        }
        
        string IStringify.Stringify()
        {
            return null;
        }

        IStringify IStringify.Unstringify(string value)
        {
            return null;
        }

    }
}
