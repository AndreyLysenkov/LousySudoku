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
    public static class Stringify_Help
    {

        public static string ArrayToString<item>(item[] array, string separator = ".") 
        {
            string result;
            result = array.Length.ToString() + separator;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i].ToString() + separator;
            }
            return result;
        }
        
    }
}