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

        public const char SeparatorDefault = ' ';

        public static List<char> SeparatorListDefault = new List<char> { '_', '*', ';', '+', '-', '=', '&', '(', ')', '{', '}', '\n' };

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

        public static string ArrayToString(IStringify[] array, List<char> separator)
        {
            string result;
            char devider = Stringify_Help.GetSeparator(separator);
            result = array.Length.ToString() + devider;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i].Stringify(separator) + devider;
            }
            return result;
        }

        public static IStringify[] ArrayFromString(string value, List<char> separator)
        {
            char devider = GetSeparator(separator);
            string[] resultString = value.Split(new char[] {devider}, 2);
            int length = Convert.ToInt32(resultString[0]);
            string[] arrayString = value.Split(new char[] { devider }, length);
            IStringify[] result = new IStringify[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = result[i].Unstringify(arrayString[i], separator);
            }
            return result;
        }

    }

}