using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{

    /// <summary>
    /// Предоставляет методы для записи объекта класса в строку и для восстановления объекта из строки
    /// </summary>
    public interface IStringify
    {

        string Stringify(List<char> separator);

        IStringify Unstringify(string value, List<char> separator);

    }

    /// <summary>
    /// Содержит методы, созданные для использования в методах IStringify;
    /// </summary>
    public static class Stringify_Help
    {

        public static string ArrayToString(IStringify[] array, string separator = ".") 
        public const char SeparatorDefault = ' ';

        public static List<char> SeparatorListDefault = new List<char> { '_', '.', ';', '+', '-', '=', '&' };

        public static char GetSeparator(List<char> separator)
        {
            char result;

            if (separator.Count == 0)
            {
                result = SeparatorDefault;
            }
            else
            {
                result = separator[0];
            }

            if (separator.Count > 1)
            {
                separator.RemoveAt(0);
            }

            return result;
        }

        {
            string result;
            result = array.Length.ToString() + separator;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i].Stringify() + separator;
            }
            return result;
        }
        
    }

}