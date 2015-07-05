﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public static class Interface
    {

        public static Sudoku CreateSudoku()
        {
            return null;
        }

        public static bool ChangeNumber(Sudoku sudoku, Number.Position number_position, int value)
        {
            return sudoku.ChangeNumber(number_position, value);
        }

        public static class SudokuBuilder
        {

            public static Sudoku GetStandart9(int[,] numbs)
            {

                Number.Position[][] block = new Number.Position[9 + 9][];

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

                return new Sudoku(new Number.Position(9, 9), value, mask, block);
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

                return new Sudoku(new Number.Position(16, 16), value, mask, block);
            }


        }
    }

}