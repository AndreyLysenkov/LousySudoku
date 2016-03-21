using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LousySudoku.UnitTest
{
    [TestClass]
    public static class Generator
    {
        [TestMethod]
        public static void Sudoku_Standart_9x9()
        {
            Sudoku sudoku = new Sudoku();
            sudoku.LoadXml(Alist.Xml.Transform.FileToElement("standart_9x9.xml"));
            sudoku.BlockType.Add(new BlockType());
            sudoku.Initialize();
            LousySudoku.Generator generator 
                = new LousySudoku.Generator(sudoku, 2000, 1);
            generator.Generate();
            if (!sudoku.IsCompleted())
                Assert.Fail("Didn't generate sudoku");
            else
                Debug.Print.Sudoku2D(sudoku);

            sudoku.Clear();

            //for (int k = 0; ; k++)
            //{
            //    bool success = true;
            //    for (int i = 0; (i < sudoku.Block.Count) && success; i++)
            //    {
            //        success = sudoku.Block[i].Generate();
            //    }
            //    if (success)
            //        success = sudoku.IsCompleted();

            //    if (success)
            //    {
            //        Console.WriteLine("Success");
            //        Debug.Print.Sudoku2D(sudoku);
            //        return;
            //    }
            //    else
            //    {
            //        sudoku.Clear();
            //    }

            //    if ((k > 0) && (k % 10000 == 0))
            //    {
            //        Assert.Inconclusive("To much attempts to build sudoku");
            //    }
            //}

        }
    }
}
