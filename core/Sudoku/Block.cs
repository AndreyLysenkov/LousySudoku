using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Alist;

namespace LousySudoku
{

    /// <summary>
    /// Описывает блок чисел
    /// Блок чисел - это массив чисел, подчиняющийся одному правилу (нет повторяющихся чисел)
    /// </summary>
    public class Block : IXmlize
    {

        /// <summary>
        /// Тип блока
        /// Содержит информацию о методе, проверяющем правильность блока
        /// </summary>
        public class BlockType : IXmlize
        {

            /// <summary>
            /// Имя метода
            /// </summary>
            public string MethodName
            {
                get;
                private set;
            }

            /// <summary>
            /// Путь к сборке с методом
            /// </summary>
            public string AssembleyPath
            {
                get;
                private set;
            }

            /// <summary>
            /// Параметры типа блока по умолчанию
            /// </summary>
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

            /// <summary>
            /// Создает тип блока по параметру и разделяющему знаку
            /// В строке параметр через разделяющий знак написанны имя метода и путь к сборке
            /// </summary>
            /// <param name="parametr">имяМетода + разделяющийЗнак + путьКсборке</param>
            /// <param name="separator">разделяющий знак</param>
            public BlockType(string parametr, char separator = ' ')
                : this(StringToParametrs(parametr, separator)[1], StringToParametrs(parametr, separator)[0])
            { }

            /// <summary>
            /// Возвращает имя метода и имя сборки в массиве строк из строки параметра
            /// </summary>
            /// <param name="parametr">строка параметра</param>
            /// <param name="separator">разделяющий знак</param>
            /// <returns></returns>
            private static string[] StringToParametrs(string parametr, char separator)
            {
                return parametr.Split(new char[1] { separator }, 2);
            }

            public string NameXml
            {
                get;
            }

            public bool LoadXml(System.Xml.Linq.XElement element)
            {

                return false;
            }

            public System.Xml.Linq.XElement UnloadXml()
            {

                return null;
            }

        }

        /// <summary>
        /// Класс для загрузки методов из внешних сборок
        /// </summary>
        public class ExternalCheck
        {

            /// <summary>
            /// Имя сборки по умолчанию
            /// </summary>
            public const string FileNameDefault = "data\\block\\standart.dll";

            /// <summary>
            /// Имя метода по умолчанию
            /// </summary>
            public const string MethodNameDefault = "Check";

            /// <summary>
            /// Содержит иныормацию о внешнем методе
            /// </summary>
            private MethodInfo method;

            /// <summary>
            /// Загружает метод из внешней сборки и делает его доступным для данного объекта
            /// </summary>
            /// <param name="filename">путь сборки</param>
            /// <param name="methodname">имя метода</param>
            public ExternalCheck(string filename = FileNameDefault, string methodname = MethodNameDefault)
            {
                this.Load(filename, methodname);
            }

            /// <summary>
            /// Загружает метод описанный в BlockType, делая его доступным через данный объект
            /// </summary>
            /// <param name="type"></param>
            public ExternalCheck(BlockType type)
                : this(type.AssembleyPath, type.MethodName)
            { }

            /// <summary>
            /// Запускает внешний метод. Указывется ссылка на блок и два параметра, передающиеся в метод
            /// Возвращает то, что внешний метод и возвращает, obviously
            /// </summary>
            /// <param name="block"></param>
            /// <param name="value"></param>
            /// <param name="mask"></param>
            /// <returns></returns>
            public int[] Run(Block block, int[] value, bool[] mask)
            {
                if (method == null)
                {
                    Console.WriteLine("Debug.... Not founded Check Method to call");
                    return block.CheckOnDefault(value, mask);
                }
                return (int[])(this.method.Invoke(null, new object[2] { (object)value, (object)mask }));
            }

            /// <summary>
            /// Возвращат информацию о всех методах в данной сборке
            /// </summary>
            /// <param name="assembly"></param>
            /// <returns></returns>
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

            /// <summary>
            /// Загружает указанный метод из внешней сборки
            /// Если загрузить не удалось, будет использоваться стандартный метод (уникальность чисел)
            /// </summary>
            /// <param name="filename"></param>
            /// <param name="methodname"></param>
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

        /// <summary>
        /// Содержит ссылки на ячейки, принадлежащие блоку
        /// </summary>
        public List<Number> Children
        {
            get;
            private set;
        }

        /// <summary>
        /// Информация о внешнем методе провери правильности блока
        /// </summary>
        public ExternalCheck CheckMethod
        {
            get;
            private set;
        }

        /// <summary>
        /// Содержит информацию, откуда получил метод проверки блока
        /// </summary>
        private BlockType blockType;

        /// <summary>
        /// Создаёт новый экземпляр объекта по ссылкам на ячейкам, принадлежащим блоку и информации о методе проверки блока
        /// </summary>
        /// <param name="children"></param>
        /// <param name="type"></param>
        public Block(Number[] children, BlockType type)
        {
            this.Children = children.ToList();
            this.AddReference();
            this.blockType = type;
            this.CheckMethod = new ExternalCheck(type);
        }

        /// <summary>
        /// Создаёт новый экземпляр объекта по ссылкам на ячейкам, принадлежащим блоку и параметре информации о методе проверки блока
        /// </summary>
        /// <param name="children"></param>
        /// <param name="BlockTypeParametrs"></param>
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

            for (int i = 0; i < this.Children.Count; i++)
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
            int[] result = new int[this.Children.Count];
            for (int i = 0; i < this.Children.Count; i++)
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
            bool[] result = new bool[this.Children.Count];
            for (int i = 0; i < this.Children.Count; i++)
            {
                result[i] = this.Children[i].HasValue;
            }
            return result;
        }

        /// <summary>
        /// Возвращает массив позиций чисел, принадлежащих данному блоку
        /// </summary>
        /// <returns></returns>
        public Position[] GetPositions()
        {
            Position[] result = new Position[this.Children.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = this.Children[i].Coordinates;
            }
            return result;
        }

        /// <summary>
        /// Возвращает массив ячеек по массиву позиций
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static Number[] GetNumbers(Position[] array)
        {
            Number[] result = new Number[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = new Number(Number.NumberType.Empty, array[i]);
            }
            return result;
        }

        /// <summary>
        /// Проверка правильности блока
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        private int[] Check(int[] value, bool[] mask)
        {
            return this.CheckMethod.Run(this, value, mask);
        }

        /// <summary>
        /// Стандартная проверка блока на правильность, активирующаяся, когда внешний метод не удалось загрузить
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
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

        public string NameXml
        {
            get { return Constant.Xml.BlockTag; }
        }

        public bool LoadXml(System.Xml.Linq.XElement element)
        {
            Alist.Xml.Tag tag = new Alist.Xml.Tag();
            tag.LoadXml(element);
            Alist.Xml.Tag number = new Alist.Xml.Tag();
            number.LoadXml(tag.GetChild(Constant.Xml.BlockNumberTag));
            List<System.Xml.Linq.XElement> position 
                = number.GetChildren(Constant.Xml.PositionTag);
            // NNBB!; ToDo; number parsing;
            return true;
        }

        public System.Xml.Linq.XElement UnloadXml()
        {
            List<Number> child = this.Children.ToList();
            List<System.Xml.Linq.XElement> childXml 
                = new List<System.Xml.Linq.XElement> { };
            foreach(Number cell in child)
            {
                childXml.Add(cell.Coordinates.UnloadXml());
            }
            System.Xml.Linq.XElement number 
                = new System.Xml.Linq.XElement(Constant.Xml.BlockNumberTag);
            Alist.Xml.Tag tag = new Alist.Xml.Tag(
                name: this.NameXml,
                value: null,
                child: new List<System.Xml.Linq.XElement>
                {
                    number
                },
                attribute: null
                );
            return tag.UnloadXml();
        }

    }

}