using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    public class Number
    {

        enum NumberType
        {
            none,
            constant,
            modify
        }

        int value;

        NumberType type;

        Block[] parents;



    }
}
