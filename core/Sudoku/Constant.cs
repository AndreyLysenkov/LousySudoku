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

        public static IEnumerable<Type> Clone<Type>(List<Type> list)
            where Type : ICloneable
        {
            if (list == null)
                return null;
            return list.ConvertAll<Type>(
                new Converter<Type, Type>(
                    x => (Type)x.Clone()
                )
            );
        }

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
            Random rand = new Random();
            List<int> digit = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            bool isContinue = true;
            for (; (digit.Count != 0) && isContinue ; )
            {
                int index = rand.Next(digit.Count - 1);
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
            for (int i = 0; i < block.Child.Count; i++)
            {
                if (!Generate(block.Child[i]))
                    return false;
            }
            // NNBB; todo;
            return block.IsRight();
        }

    }

}