using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    public class Sudoku : IStringify
    {

        /// <summary>
        /// Содержит поля и методы для представления судоку на экране
        /// </summary>
        public class Visual
        {

            int[,] mask;
            int[,] number;

            public Number.Position size;

            public Visual (Sudoku sudoku)
            {
                this.size = sudoku.Size;
                this.Load(sudoku);
            }

            private void Load(Sudoku sudoku, int default_mask_value = 0)
            {
                ///1st. Обнулить две матрицы;
                mask = new int[size.X, size.Y];
                number = new int[size.X, size.Y];
                for (int i = 0; i < size.Y; i++ )
                {
                    for (int j = 0; j < size.X; j++)
                    {
                        mask[i, j] = default_mask_value;
                        number[i, j] = 0;
                    }
                }

                ///Transfering itself;
                for (int i = 0; i < sudoku.number.Length; i++)
                {
                    Number numb = sudoku.number[i];
                    mask[numb.position.X, numb.position.Y] = (int)numb.type; ///Fix in need
                    if (numb.HasValue)
                        number[numb.position.X, numb.position.Y] = sudoku.number[i].Value;
                }
            }

        }

        Number[] number;

        Block[] block;

        public Number.Position Size;

        public Sudoku(Number.Position size, int[,] value, Number.NumberType[,] mask, Number.Position[,] block)
        {
            size = new Number.Position(value.Length, (int)(value.LongLength / value.Length));

            number = new Number[value.LongLength];
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    number[i * size.Y + j] = new Number(mask[i, j], new Number.Position(i, j), value[i, j]);
                }
            }

            this.block = new Block[block.Length];
            for (int i = 0; i < block.Length; i++)
            {
                Number[] children = new Number[(int)(block.LongLength / block.Length)];
                for (int j = 0; j < (int)(block.LongLength / block.Length); j++)
                {
                    children[j] = this.ReturnNumberByPosition(block[i, j]);
                }
                this.block[i] = new Block(children);
            }
        }
        
        string IStringify.Stringify()
        {
            return null;
        }

        IStringify IStringify.Unstringify(string value)
        {
            return null;
        }

        public Number ReturnNumberByPosition(Number.Position position)
        {
            for (int i = 0; i < number.Length; i++)
            {
                if (number[i].IsSame(position))
                {
                    return number[i];
                }
            }
            return null;
        }

        public bool ChangeNumber(Number.Position position, int value)
        {
            Number temp = this.ReturnNumberByPosition(position);
            return temp.Modify(value);
        }

    }
}
