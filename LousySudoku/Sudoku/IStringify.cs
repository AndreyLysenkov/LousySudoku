using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    public interface IStringify
    {

        string Stringify();

        IStringify Unstringify(string value);

    }

    /// <summary>
    /// Содержит методы, созданные для использования в методах IStringify;
    /// </summary>
    public class Stringify_Help
    {

        
    }
}