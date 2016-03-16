using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Alist;
using Alist.Tree;
using Alist.Error;

namespace LousySudoku
{

    /// <summary>
    /// Описывает блок чисел
    /// Блок чисел - это массив чисел, подчиняющийся одному правилу (нет повторяющихся чисел)
    /// </summary>
    public class Block
        : IXmlize, IActivatable, IEquatable<Block>
    {

        /// <summary>
        /// Luke, I am your Father.
        /// Well, it's not funny, I know...
        /// I don't fance starWars anyway...
        /// Didn't see even that moment...
        /// </summary>
        public Sudoku Father
        {
            get;
            private set;
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
        /// Содержит информацию, откуда получил метод проверки блока
        /// </summary>
        public Adress TypeId
        {
            get;
            private set;
        }

        private BlockType type;

        /// <summary>
        /// Создаёт новый экземпляр объекта по ссылкам на ячейкам, принадлежащим блоку и информации о методе проверки блока
        /// </summary>
        /// <param name="children"></param>
        /// <param name="type"></param>
        public Block(Number[] children, Adress typeId)
        {
            this.Children = children.ToList();
            this.AddReference();
            this.TypeId = typeId;
        }

        /// <summary>
        /// Создаёт новый экземпляр объекта по ссылкам на ячейкам, принадлежащим блоку и параметре информации о методе проверки блока
        /// </summary>
        /// <param name="children"></param>
        /// <param name="BlockTypeParametrs"></param>
        public Block(Number[] children, string BlockTypeParametrs)
        : this(children, new Adress(new List<string> { }))
        { }

        /// <summary>
        /// Создает блок  заданными "детьми" - ячейками, принадлежащие данному блоку
        /// </summary>
        /// <param name="children"></param>
        public Block(Number[] children)
            : this(children, new Adress(new List<string> { }))
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
                result[i] = this.Children[i].Coordinate;
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
            return null;
                //this.CheckMethod.Run(this, value, mask);
                // NNBB; todo;
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

        public bool IsInitialized
        {
            // NNBB; todo;
            get { return false; }
        }

        public MultyException Initialize()
        {
            // NNBB; todo;
            return null;
        }

        public MultyException Finilize()
        {
            // NNBB; todo;
            return null;
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
                childXml.Add(cell.Coordinate.UnloadXml());
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

        public bool Equals(Block other)
        {
            // NNBB; todo;
            return false;
        }

    }

}