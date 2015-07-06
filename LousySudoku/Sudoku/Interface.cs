using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{

    /// <summary>
    /// Предоставляет методы для пользовательского интерфейса
    /// </summary>
    public static class Interface
    {

        public static Sudoku CreateSudoku(Number.Position size, int[,] value, Number.NumberType[,] mask, Number.Position[][] block, int maxValue)
        {
            return new Sudoku(size, value, mask, block, maxValue);
        }

        /// <summary>
        /// Изменяет число в ячейке судоку
        /// Возвращает успех операции
        /// </summary>
        /// <param name="sudoku">судоку</param>
        /// <param name="number_position">позиция ячейки</param>
        /// <param name="value">новое значение</param>
        /// <returns></returns>
        public static bool ChangeNumber(Sudoku sudoku, Number.Position number_position, int value)
        {
            return sudoku.ChangeNumber(number_position, value);
        }

        /// <summary>
        /// Возвращает ячейку судоку по заданной позиции
        /// </summary>
        /// <param name="sudoku">судоку</param>
        /// <param name="position">позиция ячейки</param>
        /// <returns></returns>
        public static Number GetNumber(Sudoku sudoku, Number.Position position)
        {
            return sudoku.GetNumber(position);
        }

        /// <summary>
        /// Возвращает отобращение судоку
        /// </summary>
        /// <param name="sudoku">ссылка на экземпляр судоку</param>
        /// <param name="number">матрица чисел</param>
        /// <param name="mask">матрица маски чисел</param>
        /// <param name="rightness">матрица правильности чисел</param>
        public static void GetGrid(Sudoku sudoku, ref int[,] number, ref int[,] mask, ref bool[,] rightness)
        {
            number = new int[sudoku.Size.X, sudoku.Size.Y];
            mask = new int[sudoku.Size.X, sudoku.Size.Y];
            rightness = new bool[sudoku.Size.X, sudoku.Size.Y];
            for (int i = 0; i < sudoku.Size.X; i++)
            {
                for (int j = 0; j < sudoku.Size.Y; j++)
                {
                    Number theNumber = sudoku.GetNumber(new Number.Position(i, j));
                    number[i, j] = theNumber.Value;
                    mask[i, j] = (int)theNumber.Type;
                    rightness[i, j] = theNumber.IsRight();
                }
            }
        }

        public static class SudokuBuilder
        {

            private static Number.Position[] GetSquareBlock(Number.Position start_position, Number.Position size)
            {
                Number.Position[] result = new Number.Position[size.X * size.Y];
                for (int i = 0; i < size.X; i++ )
                {
                    for (int j = 0; j < size.Y; j++)
                    {
                        result[i * size.Y + j] = new Number.Position(start_position.X + i, start_position.Y + j);

                    }
                }
                return result;
            }

            private static Number.Position[][] GetAllSquareBlock(Number.Position sudoku_size, Number.Position block_size)
            {
                Number.Position[][] result = new Number.Position[0][];
                for (int i = 0, k = 0; i < sudoku_size.X; i += block_size.X, k++)
                {
                    for (int j = 0; j < sudoku_size.Y; j += block_size.Y, k++)
                    {
                        result[k] = GetSquareBlock(new Number.Position(i, j), block_size);
                    }
                }
                return result;
            }

            private static Number.Position[] GetLineBlock(Number.Position start_position, int length, bool isHorizontal)
            {
                Number.Position[] result = new Number.Position[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = new Number.Position(
                        start_position.X + ((isHorizontal) ? i : 0),
                        start_position.Y + ((isHorizontal) ? 0 : i)
                        );
                }
                return result;
            }

            private static Number.Position[][] GetAllLineBlock(Number.Position size, bool isHorizontal)
            {
                int length = (isHorizontal) ? size.X : size.Y;
                int block_count = (isHorizontal) ? size.Y : size.X;
                Number.Position[][] result = new Number.Position[block_count][];
                for (int i = 0; i < block_count; i++)
                {
                    result[i] = 
                        GetLineBlock(
                            new Number.Position(
                                (isHorizontal) ? 0 : i,
                                (isHorizontal) ? i : 0
                            ), 
                            length, 
                            isHorizontal
                        );
                }
                return result;
            }

            private static void CombineArrays_WriteArray<item>(ref item[] array, int start_position, item[] array_a)
            {
                for (int i = 0; i < array_a.Length; i++)
                {
                    array[start_position + i] = array_a[i];
                }
            }

            private static item[] CombineArrays<item>(item[] array_a, item[] array_b)
            {
                item[] result = new item[array_a.Length + array_b.Length];
                CombineArrays_WriteArray(ref result, 0, array_a);
                CombineArrays_WriteArray(ref result, array_a.Length, array_b);
                return result;
            }

            private static Number.Position[][] GetAllStandartBlock(Number.Position sudoku_size, Number.Position block_size)
            {
                Number.Position[][] result_Horisontal = GetAllLineBlock(sudoku_size, true);
                Number.Position[][] result_Vertical = GetAllLineBlock(sudoku_size, false);
                Number.Position[][] result_Square = GetAllSquareBlock(sudoku_size, block_size);
                return 
                    CombineArrays(
                        CombineArrays(result_Horisontal, result_Vertical),
                        result_Square
                    );
            }

            private static Sudoku GetStandart(int[,] numbs, Number.NumberType[,] mask, int sudoku_size, int block_size)
            {
                Number.Position[][] block = GetAllStandartBlock(new Number.Position(sudoku_size, sudoku_size), new Number.Position(block_size, block_size));
                return new Sudoku(new Number.Position(sudoku_size, sudoku_size), numbs, mask, block, sudoku_size);
            }

            public static Sudoku GetStandart9(int[,] numbs)
            {

                Number.Position[][] block = new Number.Position[9 + 9 + 9][];

                ///Добавление блоков (горизонтальные линии);
                for (int i = 0; i < 9; i++)
                {
                    block[i] = new Number.Position[9];
                    for (int j = 0; j < 9; j++)
                    {
                        block[i][j] = new Number.Position(i, j);
                    }
                }

                ///Добавление блоков (вертикальные линии);
                for (int i = 0; i < 9; i++)
                {
                    block[9 + i] = new Number.Position[9];
                    for (int j = 0; j < 9; j++)
                    {
                        block[9 + i][j] = new Number.Position(j, i);
                    }
                }

                ///Добавление квадратных блоков;
                for (int i = 0; i < 9; i += 3)
                {
                    for (int j = 0; j < 9; j += 3)
                    {

                    }
                }

                Number.NumberType[,] mask = new Number.NumberType[9, 9];
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        mask[i, j] = (numbs[i, j] == 0) ? Number.NumberType.Modify : Number.NumberType.Constant;
                    }
                }

                return new Sudoku(new Number.Position(9, 9), numbs, mask, block, 9);
            }

            public static Sudoku GetStandart16(int[,] numbs)
            {

                Number.Position[][] block = new Number.Position[16 + 16][];

                ///Добавление блоков (горизонтальные линии);
                for (int i = 0; i < 16; i++)
                {
                    block[i] = new Number.Position[16];
                    for (int j = 0; j < 16; j++)
                    {
                        block[i][j] = new Number.Position(i, j);
                    }
                }

                ///Добавление блоков (вертикальные линии);
                for (int i = 0; i < 16; i++)
                {
                    block[16 + i] = new Number.Position[16];
                    for (int j = 0; j < 16; j++)
                    {
                        block[16 + i][j] = new Number.Position(j, i);
                    }
                }

                int[,] value = new int[16, 16];
                //////////////
                Random rand = new Random();
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        value[i, j] = rand.Next(16);
                    }
                }
                ////////////////

                Number.NumberType[,] mask = new Number.NumberType[16, 16];
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        mask[i, j] = Number.NumberType.Modify;
                    }
                }

                return new Sudoku(new Number.Position(16, 16), value, mask, block, 16);
            }

        }
    
    }

}