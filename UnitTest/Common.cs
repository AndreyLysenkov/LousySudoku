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

            if (deep >= 5)
            {
                Generator.Sudoku_Standart_9x9();
            }

        }




    }
}
