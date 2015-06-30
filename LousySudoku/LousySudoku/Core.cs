using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LousySudoku
{
    public class DBG
    {

        public static string Change(string s)
        {
            return "checked " + s;
        }

        public static void Write(string s)
        {
            Console.WriteLine("out " + s);
        }

    }
}
