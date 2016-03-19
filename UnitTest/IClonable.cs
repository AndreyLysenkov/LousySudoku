using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LousySudoku.UnitTest
{

    [TestClass]
    public static class IClonable
    {

        [TestMethod]
        public static void Test_Position(
                int attempt = 100,
                int maxDimention = 3,
                int maxCoordinate = 30
            )
        {
            Random random = new Random();
            for (int i = 0; i < attempt; i++)
            {
                int dimention = random.Next(maxDimention + 1);
                int[] coordinate = new int[dimention];
                for (int j = 0; j < dimention; j++)
                {
                    coordinate[j] = random.Next(maxCoordinate + 1);
                }
                Position position = new Position(coordinate);
                Position clone = (Position)position.Clone();
                clone.Equals(position);
                position.Equals(clone);
            }
        }
    }
}
