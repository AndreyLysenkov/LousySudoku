using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{

    /// <summary>
    /// Object IStringify can be stored as string i.e. saved as string (Stringify) and restored from string (Unstringify)
    /// </summary>
    public interface IStringify
    {

        /// <summary>
        /// Saves object as string
        /// </summary>
        /// <param name="separator">Defines list of char, these chars separate values of current object</param>
        /// <returns>object as string</returns>
        string Stringify(List<char> separator);

        /// <summary>
        /// Restores object from string
        /// </summary>
        /// <param name="value">String with object </param>
        /// <param name="separator">Defines list of char, these chars separate values of output object in input string</param>
        /// <returns></returns>
        IStringify Unstringify(string value, List<char> separator);

    }

    /// <summary>
    /// Methods, which may help in realization of IStringify interface
    /// </summary>
    public static class Stringify_Help
    {

        /// <summary>
        /// It's... complicated
        /// </summary>
        public const char SeparatorDefault = ' ';

        /// <summary>
        /// Default list for separator
        /// NB! Using only with method CopyList
        /// </summary>
        public static List<char> SeparatorListDefault = new List<char> { '_', '*', ';', '+', '-', '=', '&', '(', ')', '{', '}', '\n' };

        /// <summary>
        /// Returns next separator for level according to current list of separators
        /// Yeah, it's complicated... too
        /// Or as I prefer it's stupid
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Saves array of IStringify elements as string
        /// </summary>
        /// <param name="array"></param>
        /// <param name="separator">These chars separate values of output object in input string</param>
        /// <returns></returns>
        public static string ArrayToString(IStringify[] array, List<char> separator)
        {
            List<char> deviderList = CopyList(separator);
            string result;
            char devider = Stringify_Help.GetSeparator(deviderList);
            result = array.Length.ToString() + devider;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i].Stringify(CopyList(deviderList));
                if (i != array.Length - 1)
                    result += devider;
            }
            return result;
        }

        /// <summary>
        /// Copies list of objects
        /// </summary>
        /// <typeparam name="item"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<item> CopyList<item>(List<item> list)
        {
            List<item> result = new List<item> { };
            foreach (item x in list)
            {
                result.Add(x);
            }
            return result;
        }

        /// <summary>
        /// Восстанавливает массив из строки
        /// </summary>
        /// <param name="example"></param>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static IStringify[] ArrayFromString(IStringify example, string value, List<char> separator)
        {
            List<char> deviderList = CopyList(separator);
            char devider = GetSeparator(deviderList);
            string[] resultString = value.Split(new char[] {devider}, 2);
            int length = Convert.ToInt32(resultString[0]);
            string[] arrayString = resultString[1].Split(new char[] { devider }, length);
            IStringify[] result = new IStringify[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ((IStringify)(example)).Unstringify(arrayString[i], CopyList(deviderList));
            }
            return result;
        }

    }

}