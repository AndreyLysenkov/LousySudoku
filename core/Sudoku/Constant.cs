using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{

    public static class Constant
    {

        public static Alist.Core Core;

        public static Random rand;

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

        public static bool Generate(Number numb)
        {
            if ((numb.HasValue) || (!numb.CanModify))
                return true;
            List<int> digit = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            bool isContinue = true;
            for (; (digit.Count != 0) && isContinue ; )
            {
                int index = Constant.rand.Next(digit.Count - 1);
                numb.Modify(digit[index]);
                digit.RemoveAt(index);
                if (numb.IsBlockRight())
                    isContinue = false;
            }
            return !isContinue;
        }

        public static int[] CheckMethod_Standart
            (Block block, int[] value, bool[] mask)
        {
            // tmp;
            /*
            List<int> result = value.ToList();
            result.Select(x => x != 0);
            if (result.Distinct().Count() != result.Count)
                return new int[1] { 0 };
            return new int[0] { };
            // */
            //for (int i = 0; i < value.Length; i++)
            //{
            //    for (int j = i + 1; (j < value.Length) && (mask[i]); j++)
            //    {
            //        if ((mask[j]) && (value[i] == value[j]))
            //        {
            //            result.Add(i);
            //            result.Add(j);
            //        }
            //    }
            //}
            List<int> result = new List<int> { };
            List<int> tmp = value.ToList();
            // Do not ask
            List<bool> resultMask 
                = tmp.Select
                    (x => 
                        (x != 0)
                        && (tmp.FindAll(y => y == x).ToList().Count > 1))
                            .ToList();
            for (int i = 0; i < resultMask.Count; i++)
                if (resultMask[i])
                    result.Add(i);
            return result.ToArray();
        }

        public static bool GenerateMethod_Standart
            (Block block, int[] value, bool[] mask)
        {
#if DEBUG
            int count9 = 0;
#endif
            List<Number> cell = block.Child;
            Random rand = new Random();
                //Constant.rand;
                //new Random();
            for (int i = 0; i < cell.Count; i++)
            {
                Number numb = cell[i];
                if ((numb.CanModify) && (!numb.HasValue))
                {
                    List<int> ints = new List<int>
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    bool isContinue = true;
                    for (int k = 0; isContinue && (ints.Count != 0); k++)
                    {
                        int index = rand.Next(ints.Count);
                        int newnumb = ints[index];
                        numb.Modify(newnumb);
                        ints.RemoveAt(index);
                        if (numb.IsBlockRight())
                            isContinue = false;
                    }
                    if (isContinue)
                        return false;
                }
#if DEBUG
                if (cell[i].Value == 9)
                    count9++;
                if (count9 > 2)
                    ;
#endif
            }
            // NNBB; todo;
            return block.IsRight();
        }

    }

}