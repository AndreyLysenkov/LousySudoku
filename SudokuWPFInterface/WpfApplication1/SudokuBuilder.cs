using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku;

namespace WpfApplication1
{
    public static class SudokuBuilder
    {
        public static Sudoku.Sudoku GetStandart9(int[,] numbs)
        {


            Number.Position[,] block = new Number.Position[9 + 9 , 9];

            ///Добавление блоков (горизонтальные линии);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    block[i, j] = new Number.Position(i, j);
                }
            }

            ///Добавление блоков (вертикальные линии);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    block[9 + i, j] = new Number.Position(j, i);
                }
            }

            return Sudoku.Debug.TestSudoku1();
        }
    }
}
