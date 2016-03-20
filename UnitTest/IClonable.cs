using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LousySudoku.UnitTest
{
    [TestClass]
    public static class Common
    {

        [TestMethod]
        public static void Run(int deep = 0)
        {





            if (deep >= 4)
            {
                Interface.ICLonable_Position();
            }



        }




    }


    [TestClass]
    public static class Interface
    {

        

        [TestMethod]
        public static void ICLonable_Position(
                int attempt = 100,
                int maxDimention = 3,
                int maxCoordinate = 30
            )
        {

            // Standart situation
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

                // Check;
                int length = Math.Max(position.Dimention, clone.Dimention);
                for (int k = 0; k < length; k++)
                    Assert.AreEqual
                        (position.Coordinate[k], clone.Coordinate[k]);

            }

            // Unique situation
            Position obj = new Position();
            Position obj_clone = (Position)obj.Clone();
            int length0 = Math.Max(obj.Dimention, obj_clone.Dimention);
            for (int k = 0; k < length0; k++)
                Assert.AreEqual
                    (obj.Coordinate[k], obj_clone.Coordinate[k]);

            Position obj1 = new Position(new List<int> { });
            Position obj_clone1 = (Position)obj.Clone();
            int length1 = Math.Max(obj1.Dimention, obj_clone1.Dimention);
            for (int k = 0; k < length1; k++)
                Assert.AreEqual
                    (obj1.Coordinate[k], obj_clone1.Coordinate[k]);

        }



    }

}