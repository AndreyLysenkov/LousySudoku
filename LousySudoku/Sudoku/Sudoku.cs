using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    /// <summary>
    /// Описывает поле судоку и взаимосвязи ячеек поля
    /// </summary>
    public class Sudoku : IStringify
    {

        /*
         * Свойства
         */

        /// <summary>
        /// Содержит все числа судоку
        /// </summary>
        public Number[] Number
        {
            get;
            private set;
        }

        /// <summary>
        /// Содержит все блоки судоку
        /// </summary>
        public Block[] Block
        {
            get;
            private set;
        }

        /// <summary>
        /// Размер судоку
        /// </summary>
        public Number.Position Size
        {
            get;
            private set;
        }

        /*
         * Конструкторы
         */

        public Sudoku(Number.Position size, int[,] value, Number.NumberType[,] mask, Number.Position[][] block)
        {
            ///Заполнение всех чисел
            this.Number = new Number[size.X * size.Y];
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    this.Number[i * size.Y + j] = 
                        new Number(
                            mask[i, j], 
                            new Number.Position(i, j), 
                            value[i, j]
                        );
                }
            }

            ///Заполнение всех блоков
            this.Block = new Block[block.Length];
            for (int i = 0; i < block.Length; i++)
            {
                Number[] children = new Number[block[i].Length];
                for (int j = 0; j < block[i].Length; j++)
                {
                    children[j] = this.GetNumber(block[i][j]);
                }
                this.Block[i] = new Block(children);
                this.Block[i].AddReference();
            }
        }

        /*
         * Методы
         */

        /// <summary>
        /// Возвращает число по его позиции
        /// Если такого числа не существует, возвращает пустую ссылку null
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Number GetNumber(Number.Position position)
        {
            for (int i = 0; i < this.Number.Length; i++)
            {
                if (this.Number[i].IsSame(position))
                {
                    return this.Number[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Изменяет значение числа в ячейке 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ChangeNumber(Number.Position position, int value)
        {
            return this.GetNumber(position).Modify(value);
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