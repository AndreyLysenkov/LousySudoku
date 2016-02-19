using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Debug
    {
        public int[,] matrix;

        public Debug(string type)
        {
            switch (type)
            {
                case "9x9":
                    {
                        matrix = new int[9, 9];
                        Random rand = new Random();
                        for (int i = 0; i < 9; i++)
                        {
                            for (int j = 0; j < 9; j++)
                            {
                                matrix[i, j] = rand.Next(9);
                            }
                        }
                        break;
                    }
                case "16x16":
                    {
                        matrix = new int[16, 16];
                        Random rand = new Random();
                        for (int i = 0; i < 16; i++)
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                matrix[i, j] = rand.Next(16);
                            }
                        }
                        break;
                    }
            }
       }

    }
}
