using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    /// <summary>
    /// Описывает число в судоку
    /// </summary>
    public class Number : IStringify
    {

        /*
         * Классы и перечисления 
         */

        /// <summary>
        /// Описывает позицию числа в судоку
        /// </summary>
        public class Position
        {

            /// <summary>
            /// Номер столбца
            /// </summary>
            public int X
            {
                private set;
                get;
            }

            /// <summary>
            /// Номер строки
            /// </summary>
            public int Y
            {
                private set;
                get;
            }

            /// <summary>
            /// Создает объект позиции, с заданными координатами
            /// </summary>
            /// <param name="x">номер столбца</param>
            /// <param name="y">номер строки</param>
            public Position(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            /// <summary>
            /// Возвращает одинаковы ли координаты у этого объекта с position
            /// </summary>
            /// <param name="position"></param>
            /// <returns>равен ли текущий объект данному по значению</returns>
            public bool IsSame(Position position)
            {
                return ((position.X == this.X) && (position.Y == this.Y));
            }

        }

        /// <summary>
        /// Описывает состояния ячейки судоку
        ///     Unexists - нет значения, нельзя изменить;
        ///     Empty - нет значения, можно изменить;
        ///     Constant - есть значение, нельзя изменить;
        ///     Modify - есть значение, можно изменить;
        /// </summary>
        public enum NumberType
        {
            Unexists,
            Empty,
            Modify,
            Constant
        }

        /*
         * Поля класса
         */

        /// <summary>
        /// Сообщает о типе ячейки
        /// </summary>
        NumberType type;

        /// <summary>
        /// Хранит число, записанное в ячейку
        /// </summary>
        int value;

        /// <summary>
        /// Содержит ссылки на блоки, которым принадлежит ячейка
        /// </summary>
        Block[] parents;

        /// <summary>
        /// Отображает позицию ячейки на поле
        /// </summary>
        Position position;

        /*
         * Свойства
         */

        /// <summary>
        /// Возвращает значение,  записанное в ячейке.
        /// Если значения нет, возвращает 0
        /// </summary>
        public int Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Показывает записанно ли значение в ячейку
        /// </summary>
        public bool HasValue
        {
            get
            {
                switch (this.type)
                {
                    case NumberType.Unexists:
                    case NumberType.Empty:
                        return false;

                    case NumberType.Modify:
                    case NumberType.Constant:
                        return true;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Показывает может ли пользователь менять значение в ячейке
        /// </summary>
        public bool IsModified
        {
            get
            {
                switch (this.type)
                {
                    case NumberType.Unexists:
                    case NumberType.Constant:
                        return false;

                    case NumberType.Modify:
                    case NumberType.Empty:
                        return true;

                    default:
                        return false;
                }
            }
        }

        /*
         * Конструкторы
         */

        public Number(NumberType type, Position position, int value = 0)
        {
            this.type = type;
            this.position = position;
            this.value = value;
            this.parents = new Block[0];
        }

        /*
         * Методы
         */

        /// <summary>
        /// Возвращает правильно ли записанно число в ячейке
        /// </summary>
        /// <returns>правильно ли записанно число в ячейке</returns>
        public bool IsRight()
        {
            for (int i = 0; i < parents.Length; i++ )
            {
                Number[] wrongNumber = parents[i].Check();
                for (int j = 0; j < wrongNumber.Length; j++)
                {
                    if (this.IsSame(wrongNumber[j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Возвращает совпадают ли данные координаты с координатами этой ячейки
        /// </summary>
        /// <param name="position">данные координаты</param>
        /// <returns>совпадают ли данные координаты с координатами этой ячейки</returns>
        public bool IsSame(Position position)
        {
            return this.position.IsSame(position);
        }

        /// <summary>
        /// Возвращает совпадают ли данное число с this
        /// Совпадение чисел происходит, если их позиции совпадают
        /// </summary>
        /// <param name="number">данное число</param>
        /// <returns>совпадают ли данное число с этим</returns>
        public bool IsSame(Number number)
        {
            return this.IsSame(number.position);
        }

        /// <summary>
        /// Изменяет, если возможно, значение ячейки
        /// Возвращает успех операции
        /// </summary>
        /// <param name="new_value">новое значение ячейки</param>
        /// <returns></returns>
        public bool Modify(int new_value)
        {
            if (new_value == 0)
            {
                return this.Clear();
            }

            if (IsModified)
            {
                this.value = new_value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Удаляет какое либо значение из ячейки, если возможно
        /// Возвращает успех операции
        /// </summary>
        /// <returns></returns>
        private bool Clear()
        {
            if (this.IsModified)
            {
                this.value = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавляет ссылку на блок, которому принадлежит ячейка
        /// Возвращает успех операции
        /// </summary>
        /// <param name="new_parent">новый родительский блок</param>
        public bool AddParent(Block new_parent)
        {
            Array.Resize(
                ref this.parents, 
                this.parents.Length + 1
            );
            this.parents[this.parents.Length - 1] = new_parent;
            return true;
        }

        /*
         * Переопределенные методы и методы интерфейсов
         */

        string IStringify.Stringify()
        {
            return null;
        }

        IStringify IStringify.Unstringify(string value)
        {
            return null;
        }

    }
}
