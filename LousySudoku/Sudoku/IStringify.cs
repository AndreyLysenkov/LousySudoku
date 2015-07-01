using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public interface IStringify
    {

        string Stringify();

        IStringify Unstringify(string value);

    }

    /// <summary>
    /// Содержит методы, созданные для использования в методах IStringify;
    /// </summary>
    public class Stringify_Help
    {

        public class Literal_Format
        {
            public const string Code = "{0}";

            public const string Begin_Default = "&#";

            public const string End_Default = ";";

            public string Begin;

            public string End;

            public Literal_Format(string begin = Begin_Default, string end = End_Default)
            {
                this.Begin = begin == null ? Begin_Default : begin;
                this.End = end == null ? End_Default : end;
            }

            public override string ToString()
            {
                return Begin + Code + End;
            }

        }

        public static string Literalize(string value, Literal_Format literal_format)
        {
            string result = "";
            for (int i = 0; i < value.Length; i++)
            {
                result += String.Format(literal_format.ToString(), (int)(value[i]));
            }
            return result;
        }

        public static string Literal(string value, List<string> toReplace, Literal_Format literal_format)
        {
            List<string> replace_reque = new List<string> { literal_format.ToString() }; ///NNBB;
            replace_reque.AddRange(toReplace); ///NNBB; Check repeats;
            string output = value;
            for (int i = 0; i < replace_reque.Count; i++) ///NNBB; May cause troubles;
            {
                output = output.Replace(replace_reque[i], Literalize(replace_reque[i], new Literal_Format()));
            }
            return output;
        }

        public static double GetNumber(string value, ref bool success)
        {
            success = false;
            double result = 0;
            success = (double.TryParse(value, out result));
            return result;
        }

        public static double GetNumber(string value, double default_value = 0)
        {
            bool success = false;
            double result = GetNumber(value, ref success);
            return (success) ? result : 0.0;
        }

        public static string Unliteral(string value, List<string> wasReplace, Literal_Format literal_format)
        {
            List<string> replace_reque = new List<string> { literal_format.ToString() }; ///NNBB;
            replace_reque.AddRange(wasReplace); ///NNBB; Check repeats;
            string result = value; ///"";
            for (int i = replace_reque.Count - 1; i >= 0; i--) ///NNBB; May cause troubles;
            {
                result = result.Replace(Literalize(replace_reque[i], new Literal_Format()), replace_reque[i]);
                Console.WriteLine(" replacing back {0} - {1}", i, result);
            }
            return result;
        }
    
    }
}