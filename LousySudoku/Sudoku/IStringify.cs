using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public interface IStringify
    {

        string Stringify();

        static IStringify Unstringify(string value);

    }
}
