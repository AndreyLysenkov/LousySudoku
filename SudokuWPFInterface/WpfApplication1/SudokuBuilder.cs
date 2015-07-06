using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku;

namespace WpfApplication1
{
    public static class SudokuDebug
    {
        public static Sudoku.Sudoku GetStandart9(int[,] numbs)
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

            ///Добавление блоков 3x3;
            int blockIndex = 9 + 9;
            for (int i = 0; i < 9; i += 3)
            {
                for (int j = 0; j < 9; j += 3, blockIndex++)
                {
                    block[blockIndex] = new Number.Position[9];
                    int cellIndex = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++, cellIndex++)
                        {
                            block[blockIndex][cellIndex] = new Number.Position(i + k, j + l);
                        }
                    }
                }
            }

            int[,] value = new int[9, 9];
            //////////////
            Random rand = new Random();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    value[i, j] = rand.Next(9);
                }
            }
            ////////////////

            Number.NumberType[,] mask = new Number.NumberType[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    mask[i, j] = Number.NumberType.Modify;
                }
            }

            return new Sudoku.Sudoku(new Number.Position(9, 9), value, mask, block);
        }

        public static Sudoku.Sudoku GetStandart16(int[,] numbs)
        {


            Number.Position[][] block = new Number.Position[16 + 16 + 16][];

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

            ///Добавление блоков 4x4;
            int blockIndex = 16 + 16;
            for (int i = 0; i < 16; i += 4)
            {
                for (int j = 0; j < 16; j += 4, blockIndex++)
                {
                    block[blockIndex] = new Number.Position[16];
                    int cellIndex = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 0; l < 4; l++, cellIndex++)
                        {
                            block[blockIndex][cellIndex] = new Number.Position(i + k, j + l);
                        }
                    }
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

            return new Sudoku.Sudoku(new Number.Position(16, 16), value, mask, block);
        }
    }
}
