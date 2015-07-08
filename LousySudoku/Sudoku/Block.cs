﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LousySudoku
{

    /// <summary>
    /// Описывает блок чисел
    /// Блок чисел - это массив чисел, подчиняющийся одному правилу (нет повторяющихся чисел)
    /// </summary>
    public class Block : IStringify
    {

        public class BlockType : IStringify
        {
            public string MethodName
            {
                get;
                private set;
            }

            public string AssembleyPath
            {
                get;
                private set;
            }

            public static string ParametrDefault
            {
                get
                {
                    return ExternalCheck.MethodNameDefault + ' ' + ExternalCheck.FileNameDefault;
                }
            }

            private BlockType(string assembleyPath = ExternalCheck.FileNameDefault, string methodName = ExternalCheck.MethodNameDefault)
            {
                this.AssembleyPath = assembleyPath;
                this.MethodName = methodName;
            }

            public BlockType(string parametr, char separator = ' ')
                : this(StringToParametrs(parametr, separator)[1], StringToParametrs(parametr, separator)[0])
            { }

            private static string[] StringToParametrs(string parametr, char separator)
            {
                return parametr.Split(new char[1] { separator }, 2);
            }

            string IStringify.Stringify(List<char> separator)
            {
                char devider = Stringify_Help.GetSeparator(separator);
                return this.MethodName + devider + this.AssembleyPath;
            }

            IStringify IStringify.Unstringify(string value, List<char> separator)
            {
                char devider = Stringify_Help.GetSeparator(separator);
                string[] parametr = value.Split(new char[] {devider}, 2);
                return new BlockType(parametr[1], parametr[0]);
            }

        }

        public class ExternalCheck
        {

            public const string FileNameDefault = "data\\block\\standart.dll";

            public const string MethodNameDefault = "Check";

            MethodInfo method;

            public ExternalCheck(string filename = FileNameDefault, string methodname = MethodNameDefault)
            {
                this.Load(filename, methodname);
            }

            public ExternalCheck(BlockType type)
                : this(type.AssembleyPath, type.MethodName)
            { }

            public int[] Run(Block block, int[] value, bool[] mask)
            {
                if (method == null)
                {
                    Console.WriteLine("Debug.... Not founded Check Method to call");
                    return block.CheckOnDefault(value, mask);
                }
                return (int[])(this.method.Invoke(null, new object[2] { (object)value, (object)mask }));
            }

            private MethodInfo[][] GetAssembleyMethods(Assembly assembly)
            {
                Type[] type = assembly.GetTypes();
                int length = type.Length;
                MethodInfo[][] result = new MethodInfo[length][];
                for (int i = 0; i < length; i++)
                {
                    result[i] = type[i].GetMethods();
                }
                return result;
            }

            private void Load(string filename, string methodname)
            {
                try
                {
                    bool IsFounded = false;
                    Assembly assembly = Assembly.LoadFrom(filename);
                    MethodInfo[][] method = this.GetAssembleyMethods(assembly);
                    for (int i = 0; (i < method.Length) && !IsFounded; i++)
                    {
                        for (int j = 0; (j < method[i].Length) && !IsFounded; j++)
                        {
                            if (method[i][j].Name == methodname)
                            {
                                IsFounded = true;
                                this.method = method[i][j];
                            }
                        }
                    }
                    if (!IsFounded)
                    {
                        this.method = null;
                    }
                }
                catch(Exception e)
                {
                    this.method = null;
                }
            }
        }

        /*
         * Свойства
         */

        /// <summary>
        /// Содержит ссылки на ячейки, принадлежащие блоку
        /// </summary>
        public Number[] Children
        {
            get;
            private set;
        }

        public ExternalCheck CheckMethod
        {
            get;
            private set;
        }

        private BlockType blockType;

        /*
         * Конструкторы
         */

        public Block(Number[] children, BlockType type)
        {
            this.Children = children;
            this.AddReference();
            this.blockType = type;
            this.CheckMethod = new ExternalCheck(type);
        }

        public Block(Number[] children, string BlockTypeParametrs)
        : this(children, new BlockType(BlockTypeParametrs))
        { }

        /// <summary>
        /// Создает блок  заданными "детьми" - ячейками, принадлежащие данному блоку
        /// </summary>
        /// <param name="children"></param>
        public Block(Number[] children)
            : this(children, new BlockType(BlockType.ParametrDefault))
        { }

        /*
         * Методы
         */

        /// <summary>
        /// Вощвращает правильность заполнености блока по методу Check()
        /// </summary>
        /// <returns></returns>
        public bool IsRight()
        {
            return (this.Check().Length == 0);
        }

        /// <summary>
        /// Добавляет всем числам блока ссылку на самого себя
        /// </summary>
        public void AddReference()
        {
            if (this.Children == null)
                return;

            for (int i = 0; i < this.Children.Length; i++)
            {
                this.Children[i].AddParent(this);
            }
        }

        /// <summary>
        /// Возвращает все числа, которые не соответствуют правилу блока
        /// </summary>
        /// <returns></returns>
        public Number[] Check()
        {
            int[] result_indexes = this.Check(this.GetValues(), this.GetValuesMask());
            Number[] result = new Number[result_indexes.Length];

            for (int i = 0; i < result_indexes.Length; i++)
            {
                result[i] = this.Children[result_indexes[i]];
            }
            return result;
        }

        /// <summary>
        /// Возвращает все числа блока ввиде массива чисел
        /// </summary>
        /// <returns></returns>
        private int[] GetValues()
        {
            int[] result = new int[this.Children.Length];
            for (int i = 0; i < this.Children.Length; i++)
            {
                result[i] = this.Children[i].Value;
            }
            return result;
        }

        /// <summary>
        /// Возвращает boolean массив, указывающий содержит ли соответствующая ячейка значение в свойстве Children
        /// </summary>
        /// <returns></returns>
        private bool[] GetValuesMask()
        {
            bool[] result = new bool[this.Children.Length];
            for (int i = 0; i < this.Children.Length; i++)
            {
                result[i] = this.Children[i].HasValue;
            }
            return result;
        }

        public Number.Position[] GetPositions()
        {
            Number.Position[] result = new Number.Position[this.Children.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = this.Children[i].Coordinates;
            }
            return result;
        }

        private static Number[] GetNumbers(Number.Position[] array)
        {
            Number[] result = new Number[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = new Number(Number.NumberType.Empty, array[i]);
            }
            return result;
        }

        /*
         * Переопределенные методы и методы интерфейсов
         */

        private int[] Check(int[] value, bool[] mask)
        {
            return this.CheckMethod.Run(this, value, mask);
        }

        private int[] CheckOnDefault(int[] value, bool[] mask)
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

        string IStringify.Stringify(List<char> separator)
        {
            char devider = Stringify_Help.GetSeparator(separator);
            string result = "";

            result += ((IStringify)(this.blockType)).Stringify(separator) + devider;

            result += Stringify_Help.ArrayToString(this.GetPositions(), separator);

            return result;
        }

        IStringify IStringify.Unstringify(string value, List<char> separator)
        {
            string[] result = value.Split(new char[] { Stringify_Help.GetSeparator(separator) });

            BlockType blockType = (BlockType)((IStringify)(this.blockType)).Unstringify(result[0], separator);

            IStringify[] temp = (Stringify_Help.ArrayFromString(new Number.Position(0,0), result[1], separator));
            Number.Position[] number = new Number.Position[temp.Length];
            for (int i = 0; i < temp.Length;  i++)
            {
                number[i] = (Number.Position)temp[i];
            }

            return new Block(Block.GetNumbers(number), blockType);
        }

    }

}