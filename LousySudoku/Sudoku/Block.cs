using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{

    /// <summary>
    /// Описывает блок чисел
    /// Блок чисел - это массив чисел, подчиняющийся одному правилу (нет повторяющихся чисел)
    /// </summary>
    public class Block : IStringify
    {

        /*
         * Свойства
         */

        /// <summary>
        /// Содержит ссылки на ячейки, принадлежащие блоку
        /// </summary>
        public Number[] Children
        {
            get;
            private set;
        }

        /*
         * Конструкторы
         */

        /// <summary>
        /// Создает блок  заданными "детьми" - ячейками, принадлежащие данному блоку
        /// </summary>
        /// <param name="children"></param>
        public Block(Number[] children)
        {
            this.Children = children;
        }

        /*
         * Методы
         */

        /// <summary>
        /// Вощвращает правильность заполнености блока по методу Check()
        /// </summary>
        /// <returns></returns>
        public bool IsRight()
        {
            return (this.Check().Length == 0);
        }

        /// <summary>
        /// Добавляет всем числам блока ссылку на самого себя
        /// </summary>
        public void AddReference()
        {
            for (int i = 0; i < this.Children.Length; i++)
            {
                this.Children[i].AddParent(this);
            }
        }

        /// <summary>
        /// Возвращает все числа, которые не соответствуют правилу блока
        /// </summary>
        /// <returns></returns>
        public Number[] Check()
        {
            int[] result_indexes = this.Check(this.GetValues(), this.GetValuesMask());
            Number[] result = new Number[result_indexes.Length];

            for (int i = 0; i < result_indexes.Length; i++)
            {
                result[i] = this.Children[result_indexes[i]];
            }
            return result;
        }

        /// <summary>
        /// Возвращает все числа блока ввиде массива чисел
        /// </summary>
        /// <returns></returns>
        private int[] GetValues()
        {
            int[] result = new int[this.Children.Length];
            for (int i = 0; i < this.Children.Length; i++)
            {
                result[i] = this.Children[i].Value;
            }
            return result;
        }

        /// <summary>
        /// Возвращает boolean массив, указывающий содержит ли соответствующая ячейка значение в свойстве Children
        /// </summary>
        /// <returns></returns>
        private bool[] GetValuesMask()
        {
            bool[] result = new bool[this.Children.Length];
            for (int i = 0; i < this.Children.Length; i++)
            {
                result[i] = this.Children[i].HasValue;
            }
            return result;
        }

        /*
         * Переопределенные методы и методы интерфейсов
         */

        protected virtual int[] Check(int[] value, bool[] mask)
        {
            int[] result = new int[0];
            for (int i = 0; i < value.Length; i++)
            {
                for (int j = i + 1; (j < value.Length) && (mask[i]); j++)
                {
                    if ((mask[j]) && (value[i] == value[j]))
                    {
                        Array.Resize(ref result, result.Length + 2);
                        result[result.Length - 1] = i;
                        result[result.Length - 2] = j;
                    }
                }
            }
            return result;
        }

        string IStringify.Stringify()
        {
            string result = "";
            for (int i = 0; i < this.Children.Length; i++)
            {
                result += ((IStringify)(this.Children[i].Coordinates)).Stringify() + "|";
            }
            return result;
        }

        IStringify IStringify.Unstringify(string value)
        {
            string[] result = value.Split(new char[] {'|'});
            Block block = new Block(new Number[0]);
            block.Children = new Number[result.Length];
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].Length != 0)
                {
                    block.Children[i] =
                        new Number(
                            0, 
                            (Number.Position)((IStringify)(new Number.Position(0, 0))).Unstringify(result[i]),
                            0
                        );
                }
            }
            return block;
        }

    }

}