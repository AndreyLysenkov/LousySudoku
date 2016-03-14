using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alist;

namespace LousySudoku
{

    /// <summary>
    /// Описывает поле судоку и взаимосвязи ячеек поля
    /// </summary>
    public class Sudoku : IXmlize
    {

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
        public Position Size
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

        /// <summary>
        /// Делегат метода, вызывающегося при зоплнении или завершении судоку
        /// </summary>
        /// <param name="sudoku"></param>
        public delegate void SudokuEvent (Sudoku sudoku);

        /// <summary>
        /// Вызывается при завершеннии судоку
        /// Судоку заполнено полностью и правильно
        /// </summary>
        public event SudokuEvent OnCompleted;

        /// <summary>
        /// Вызывается при полном заполнении судоку
        /// </summary>
        public event SudokuEvent OnFilled;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <param name="block"></param>
        /// <param name="maxValue"></param>
        public Sudoku(Position size, int[,] value, Number.NumberType[,] mask, Position[][] block, int maxValue)
        {
            this.OnFilled = EmptySudokuEventHandler;
            this.OnCompleted = EmptySudokuEventHandler;

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
                            new Position(i, j),
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
                ///this.Block[i].AddReference();
            }

            this.MaxValue = maxValue;

            this.Size = size;
        }

        /// <summary>
        /// Возвращает число по его позиции
        /// Если такого числа не существует, возвращает пустую ссылку null
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Number GetNumber(Position position)
        {
            for (int i = 0; i < this.Number.Length; i++)
            {
                if (this.Number[i].IsSame(position))
                {
                    return this.Number[i];
                }
            }
            return new Number(LousySudoku.Number.NumberType.Unexists, position);
        }

        /// <summary>
        /// Изменяет значение числа в ячейке 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ChangeNumber(Position position, int value)
        {
            bool success = this.GetNumber(position).Modify(value);
            if (success)
            {
                this.IsCompleted();
            }
            return success;
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
            rightness = new bool[this.Size.X, this.Size.Y];
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    Number number = this.GetNumber(new Position(i, j));
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
            OnFilled(this);
            return true;
        }

        /// <summary>
        /// Возвращает заполненно все судоку полностью и правильно
        /// </summary>
        /// <returns></returns>
        public bool IsCompleted()
        {
            bool result = this.IsFilled() && this.IsRight();
            if (result)
            {
                OnCompleted(this);
            }
            return result;
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

        /// <summary>
        /// Перемешивает ссылки на ячейки в судоку в массиве
        /// Не меняет позиции
        /// </summary>
        public void MixNumbers()
        {
            Generator generator = new Generator(this);
            this.Number = generator.MixItems(this.Number);
        }

        /// <summary>
        /// Перемешивает ссылки на блоки в судоку в массиве
        /// Не меняет позиции
        /// </summary>
        public void MixBlocks()
        {
            Generator generator = new Generator(this);
            this.Block = generator.MixItems(this.Block);
        }

        /// <summary>
        /// Посчитывает сколько ячеек должно содержать или содержит числа
        /// </summary>
        /// <returns></returns>
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

        public int GetNumberCount0()
        {
            int result = 0;
            foreach (Number number in this.Number)
            {
                if (number.HasValue)
                {
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// Удаляет из судоку указанное количество ячеек
        /// </summary>
        /// <param name="count"></param>
        public void DeleteNumbers(int count)
        {
            this.MixNumbers();
            for (int i = 0; i < count; i++)
            {
                this.Number[i].Clear();
            }
        }

        /// <summary>
        /// Метод вызывающийся при заполнении и завершении судоку
        /// </summary>
        /// <param name="sudoku"></param>
        private void EmptySudokuEventHandler(Sudoku sudoku)
        {  }

        /// <summary>
        /// Returns a copy of object;
        /// </summary>
        /// <returns></returns>
        public Sudoku Copy()
        {
            return null;
                //(Sudoku)((IStringify)(new Sudoku(null, null, null, null, 0))).Unstringify(
                //    ((IStringify)this).Stringify(
                //        Stringify_Help.CopyList(Stringify_Help.SeparatorListDefault)
                //    ),
                //    Stringify_Help.CopyList(Stringify_Help.SeparatorListDefault)
                //);
        }
        
        /// <summary>
        /// Возвращает массив ячеек по размеру матрицы, матрице значений и маске ячеек
        /// </summary>
        /// <param name="size"></param>
        /// <param name="number"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        private static Number[] GetNumbers(Position size, int[,] number, Number.NumberType[,] mask)
        {
            Number[] result = new Number[size.X * size.Y];
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    result[i * size.X + j] = new Number(mask[i, j], new Position(i, j), number[i, j]);
                }
            }
            return result;
        }

        public string NameXml
        {
            get;
        }

        public bool LoadXml(System.Xml.Linq.XElement element)
        {

            return false;
        }

        public System.Xml.Linq.XElement UnloadXml()
        {

            return null;
        }

    }

}