using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    public class Sudoku
    {

        public class Size
        {
            int Length
            {
                public get;
                private set;
            }

            int Height
            {
                public get;
                private set;
            }

            public Size(int x, int y)
            {
                Length = x;
                Height = y;
            }
        }

        Number[] number;

        Number[] block;

        public Sudoku()
        {
            number = new Number[0];
            block = new Number[0];
        }


    }
}
