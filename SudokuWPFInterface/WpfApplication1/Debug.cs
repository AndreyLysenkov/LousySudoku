using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Debug
    {
        public int[,] matrix = new int[9, 9];

        public Debug()
        {
            Random rand = new Random();
            for(int i = 0; i<9; i++)
            {
                for(int j = 0; j<9; j++)
                {
                    matrix[i, j] = rand.Next(9);
                }
            }
        }

    }
}
