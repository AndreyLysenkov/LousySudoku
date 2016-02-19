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

        /// <summary>
        /// Сохраняет данный объект в строку
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        string Stringify(List<char> separator);

        /// <summary>
        /// Возвращает восстановленный объект из строки
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        IStringify Unstringify(string value, List<char> separator);

    }

    /// <summary>
    /// Содержит методы, созданные для использования в методах IStringify;
    /// </summary>
    public static class Stringify_Help
    {

        public const char SeparatorDefault = ' ';

        /// <summary>
        /// Список разделителей по умолчанию
        /// Для доступа использовать вместе с методом CopyList
        /// </summary>
        public static List<char> SeparatorListDefault = new List<char> { '_', '*', ';', '+', '-', '=', '&', '(', ')', '{', '}', '\n' };

        /// <summary>
        /// Возвращает следующий разделитель из списка, удаляя его
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
        /// Преобразует массив в строку
        /// </summary>
        /// <param name="array"></param>
        /// <param name="separator"></param>
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
        /// Копирует коллекцию
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