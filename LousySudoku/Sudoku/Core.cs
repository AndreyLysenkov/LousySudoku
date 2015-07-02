using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    public class Sudoku : IStringify
    {

        public class Size
        {
            public int Length
            {
                get;
                private set;
            }

            public int Height
            {
                get;
                private set;
            }

            public Size(int x, int y)
            {
                Length = x;
                Height = y;
            }
        }

        /// <summary>
        /// Содержит поля и методы для представления судоку на экране
        /// </summary>
        public class Visual
        {

            int[,] mask;
            int[,] number;

            public Size size;

            public Visual (Sudoku sudoku)
            {
                this.size = sudoku.size;
                this.Load(sudoku);
            }

            private void Load(Sudoku sudoku, int default_mask_value = 0)
            {
                ///1st. Обнулить две матрицы;
                mask = new int[size.Length, size.Height];
                number = new int[size.Length, size.Height];
                for (int i = 0; i < size.Length; i++ )
                {
                    for (int j = 0; j < size.Height; j++)
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

        Number[] block;

        public Size size;

        public Sudoku()
        {
            number = new Number[0];
            block = new Number[0];
            size = new Size(9, 9);
        }
        
        string IStringify.Stringify()
        {
            return null;
        }

        IStringify IStringify.Unstringify(string value)
        {
            return null;
        }

        public bool Modify(int new_value)
        {



            return false;
        }

    }
}
