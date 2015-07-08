using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
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

        /// <summary>
        /// Наибольшее значение, которое может быть записано в ячейке
        /// </summary>
        public int MaxValue
        {
            get;
            private set;
        }

        public delegate void SudokuEvent (Sudoku sudoku);

        public event SudokuEvent OnCompleted;

        public event SudokuEvent OnFilled;

        /*
         * Конструкторы
         */

        public Sudoku(Number.Position size, int[,] value, Number.NumberType[,] mask, Number.Position[][] block, int maxValue)
        {
            if (size == null)
                return;
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

            this.MaxValue = maxValue;

            this.Size = size;
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

        /// <summary>
        /// Возвращает судоку ввиде матрицы чисел и матрицы их типов;
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="mask"></param>
        public void GetGrid(ref int[,] numbers, ref int[,] mask, ref bool[,] rightness)
        {
            numbers = new int[this.Size.X, this.Size.Y];
            mask = new int[this.Size.X, this.Size.Y];
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    Number number = this.GetNumber(new Number.Position(i, j));
                    numbers[i, j] = number.Value;
                    mask[i, j] = (int)number.Type;
                    rightness[i, j] = number.IsRight();
                }
            }
        }

        /// <summary>
        /// Возвращает все ли в судоку числа заполнены правильно
        /// </summary>
        /// <returns></returns>
        public bool IsRight()
        {
            for (int i = 0; i < this.Number.Length; i++)
            {
                if (!this.Number[i].IsRight())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Возвращает все ли поля заполнены в судоку
        /// </summary>
        /// <returns></returns>
        public bool IsFilled()
        {
            for (int i = 0; i < this.Number.Length; i++)
            {
                if (this.Number[i].Type == LousySudoku.Number.NumberType.Empty)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Возвращает заполненно все судоку полностью и правильно
        /// </summary>
        /// <returns></returns>
        public bool IsCompleted()
        {
            return this.IsFilled() && this.IsRight();
        }

        /// <summary>
        /// Очищает все ячейки судоку
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < this.Number.Length; i++)
            {
                this.Number[i].Clear();
            }
        }

        public void MixNumbers()
        {
            Generator generator = new Generator(this);
            this.Number = generator.MixItems(this.Number);
        }

        public int GetNumberCount()
        {
            int result = 0;
            foreach (Number number in this.Number)
            {
                if (number.IsExist)
                {
                    result++;
                }
            }
            return result;
        }

        public void DeleteNumbers(int count)
        {
            this.MixNumbers();
            for (int i = 0; i < count; i++)
            {
                this.Number[i].Clear();
            }
        }

        /*
         * Переопределенные методы и методы интерфейсов
         */

        string IStringify.Stringify(List<char> separator)
        {
            string result = "";
            char deviderLevel4 = Stringify_Help.GetSeparator(separator);
            char devider = Stringify_Help.GetSeparator(separator);
            result += ((IStringify)(this.Size)).Stringify(separator) + devider;
            int[,] value = new int[this.Size.X, this.Size.Y];
            int[,] mask = new int[this.Size.X, this.Size.Y];
            bool[,] rightness = new bool[this.Size.X, this.Size.Y];
            this.GetGrid(ref value, ref mask, ref rightness);
            char deviderLevel1 = Stringify_Help.GetSeparator(separator);
            char deviderLevel2 = Stringify_Help.GetSeparator(separator);
            char deviderLevel3 = Stringify_Help.GetSeparator(separator);
            for (int i = 0; i < this.Size.X; i++)
            {
                for (int j = 0; j < this.Size.Y; j++)
                {
                    result += value[i, j].ToString() + deviderLevel1 + mask[i, j].ToString() + deviderLevel2;
                }
                result += deviderLevel3;
            }
            result += deviderLevel4 + Stringify_Help.ArrayToString(this.Block, separator);
            result += deviderLevel4 + this.MaxValue.ToString();
            return result;
        }

        private static Number[] GetNumbers(Number.Position size, int[,] number, Number.NumberType[,] mask)
        {
            Number[] result = new Number[size.X * size.Y];
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    result[i * size.X + j] = new Number(mask[i, j], new Number.Position(i, j), number[i, j]);
                }
            }
            return result;
        }

        IStringify IStringify.Unstringify(string value, List<char> separator)
        {
            char deviderLevel4 = Stringify_Help.GetSeparator(separator);
            string[] result4 = value.Split(new char[] { deviderLevel4 }, 3);
            char devider = Stringify_Help.GetSeparator(separator);
            string[] result = result4[0].Split(new char[] { devider }, 2);
            Number.Position size = (Number.Position)((IStringify)(new Number.Position(0, 0))).Unstringify(result[0], separator);
            char deviderLevel1 = Stringify_Help.GetSeparator(separator);
            char deviderLevel2 = Stringify_Help.GetSeparator(separator);
            char deviderLevel3 = Stringify_Help.GetSeparator(separator);
            int[,] number = new int[size.X, size.Y];
            Number.NumberType[,] mask = new Number.NumberType[size.X, size.Y];
            string[] resultLevel3 = result[1].Split(new char[] { deviderLevel3 }, size.X + 1);
            for (int i = 0; i < size.X; i++)
            {
                string[] resultLevel2 = resultLevel3[i].Split(new char[] { deviderLevel2 }, size.Y + 1);
                for (int j = 0; j < size.Y; j++)
                {
                    string[] resultLevel1 = resultLevel2[j].Split(new char[] { deviderLevel1 }, 2);
                    number[i, j] = Convert.ToInt32(resultLevel1[0]);
                    mask[i, j] = (Number.NumberType)(Convert.ToInt32(resultLevel1[1]));
                }
            }
            Number[] numberNumber = GetNumbers(size, number, mask);
            IStringify[] temp = Stringify_Help.ArrayFromString(new Block(null), result4[1], separator);
            Block[] block = new Block[temp.Length];
            Number.Position[][] blockPosition = new Number.Position[temp.Length][];
            for (int i = 0; i < temp.Length; i++)
            {
                block[i] = (Block)temp[i];
                blockPosition[i] = block[i].GetPositions();
            }
            int maxValue = Convert.ToInt32(result4[2]);
            return new Sudoku(size, number, mask, blockPosition, maxValue);
        }

    }

}