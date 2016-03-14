using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{

    public static class Constant
    {




        public static class Xml
        {

            public const string PositionTag = "position";



        }

    }

    public static class Method
    {

        public static string ArrayToString<Type>
            (List<Type> list, string separator)
        {
            string result = "";
            for (int i = 0; i < list.Count; i++)
            {
                result += list[i].ToString();
                if (i != list.Count - 1)
                    result += separator;
            }
            return result;
        }


    }

}
