using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{

    public static class Constant
    {

        public static Alist.Core Core;

        public static class Xml
        {

            public static class BlockType
            {

                public const string MethodAttribute = "method";

                public const string MethodAttributeChecker
                    = "checker";

                public const string MethodAttributeGenerator
                    = "generator";

                public const string IdAttribute = "id";

                public const string Tag = "type";

            }

            public static class Sudoku
            {

                public const string Tag = "sudoku";

                public const string NumberSection = "numberpart";

                public const string BlockSection = "blockpart";

                public const string InfoSection = "infopart";

                public const string MethodSection = "methodpart";

                public const string MaxValue = "maxvalue";

            }

            public const string PositionTag = "position";

            public const string NumberTag = "number";

            public const string NumberValueTag = "value";

            public const string NumberTypeAttribute = "type";

            public const string BlockTag = "block";

            public const string BlockNumberTag = "number";

        }

        public static class Exception
        {

            public const string BlockTypeCheckerNotSet
                = "BlockType's checker is not set. Set as default";

            public const string BlockSudokuNotSet
                = "There aren't any link to sudoku in block";
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

        public static List<Type> Clone<Type>
            (IEnumerable<Type> list)
        where Type : struct
        {
            if (list == null)
                return null;
            List<Type> result = new List<Type> { };
            for (int i = 0; i < list.Count<Type>(); i++)
            {
                result.Add(list.ElementAt(i));
            }
            return result;
        }

        public static void EqualizeLength<Type>(
            ref List<Type> list1,
            ref List<Type> list2,
            Type value = default(Type))
        {
            int length = Math.Max(list1.Count, list2.Count);
            for (int i = list1.Count; i < length; i++)
            {
                list1.Add(value);
            }
            for (int i = list2.Count; i < length; i++)
            {
                list2.Add(value);
            }
        }

        public static int[] CheckMethod_Standart
            (Block block, int[] value, bool[] mask)
        {
            int[] result = new int[0];
            for (int i = 0; i < value.Length; i++)
            {
                for (int j = i + 1; (j < value.Length) && (mask[i]); j++)
                {
                    if ((mask[j]) && (value[i] == value[j]))
                    {
                        Array.Resize(ref result, result.Length + 2);
                        result[result.Length - 1] = i;
                        result[result.Length - 2] = j;
                    }
                }
            }
            return result;
        }

        public static bool GenerateMethod_Standart
            (Block block, int[] value, bool[] mask)
        {
            List<Number> cell = block.Child;
            Random rand = new Random();
            for (int i = 0; i < cell.Count; i++)
            {
                if (cell[i].CanModify)
                {
                    List<int> ints = new List<int>
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    bool isContinue = true;
                    for (int k = 0; isContinue && (k < 9); k++)
                    {
                        int newnumb = ints[rand.Next(ints.Count - 1)];
                        cell[i].Modify(newnumb);
                        ints.Remove(newnumb);
                        if (cell[i].IsBlockRight())
                        {
                            isContinue = false;
                        }
                    }
                    if (isContinue = true)
                        return false;
                }
            }
            // NNBB; todo;
            return true;
        }

    }

}