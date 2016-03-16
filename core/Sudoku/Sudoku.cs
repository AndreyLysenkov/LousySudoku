using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Alist;
using Alist.Error;
using Alist.Xml;

namespace LousySudoku
{

    /// <summary>
    /// Описывает поле судоку и взаимосвязи ячеек поля
    /// </summary>
    public class Sudoku
        : IXmlsaver, IActivatable
    {

        /// <summary>
        /// Содержит все числа судоку
        /// </summary>
        public List<Number> Number
        {
            get;
            private set;
        }

        /// <summary>
        /// Содержит все блоки судоку
        /// </summary>
        public List<Block> Block
        {
            get;
            private set;
        }

        /// <summary>
        /// Размер судоку
        /// </summary>
        public Position Size
        {
            get;
            private set;
        }

        /// <summary>
        /// Наибольшее значение, которое может быть записано в ячейке
        /// </summary>
        public int MaxValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Делегат метода, вызывающегося при зоплнении или завершении судоку
        /// </summary>
        /// <param name="sudoku"></param>
        public delegate void SudokuEvent (Sudoku sudoku);

        /// <summary>
        /// Вызывается при завершеннии судоку
        /// Судоку заполнено полностью и правильно
        /// </summary>
        public event SudokuEvent OnCompleted;

        /// <summary>
        /// Вызывается при полном заполнении судоку
        /// </summary>
        public event SudokuEvent OnFilled;

        public event SudokuEvent OnChanging;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <param name="block"></param>
        /// <param name="maxValue"></param>
        public Sudoku(Position size, int[,] value, Number.NumberType[,] mask, Position[][] block, int maxValue)
        {
            this.OnFilled = EmptySudokuEventHandler;
            this.OnCompleted = EmptySudokuEventHandler;

            if (size == null)
                return;
            ///Заполнение всех чисел
            this.Number = new Number[size.X * size.Y].ToList();
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    this.Number[i * size.Y + j] =
                        new Number(
                            mask[i, j],
                            new Position(i, j),
                            value[i, j]
                        );
                }
            }

            ///Заполнение всех блоков
            this.Block = new Block[block.Length].ToList();
            for (int i = 0; i < block.Length; i++)
            {
                Number[] children = new Number[block[i].Length];
                for (int j = 0; j < block[i].Length; j++)
                {
                    children[j] = this.GetNumber(block[i][j]);
                }
                this.Block[i] = new Block(children);
                ///this.Block[i].AddReference();
            }

            this.MaxValue = maxValue;

            this.Size = size;
        }

        /// <summary>
        /// Возвращает число по его позиции
        /// Если такого числа не существует, возвращает пустую ссылку null
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Number GetNumber(Position position)
        {
            for (int i = 0; i < this.Number.Count; i++)
            {
                if (this.Number[i].Equals(position))
                {
                    return this.Number[i];
                }
            }
            return new Number(LousySudoku.Number.NumberType.Unexists, position);
        }

        /// <summary>
        /// Изменяет значение числа в ячейке 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ChangeNumber(Position position, int value)
        {
            bool success = this.GetNumber(position).Modify(value);
            if (success)
            {
                this.IsCompleted();
            }
            return success;
        }

        /// <summary>
        /// Возвращает судоку ввиде матрицы чисел и матрицы их типов;
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="mask"></param>
        public void GetGrid(ref int[,] numbers, ref int[,] mask, ref bool[,] rightness)
        {
            numbers = new int[this.Size.X, this.Size.Y];
            mask = new int[this.Size.X, this.Size.Y];
            rightness = new bool[this.Size.X, this.Size.Y];
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    Number number = this.GetNumber(new Position(i, j));
                    numbers[i, j] = number.Value;
                    mask[i, j] = (int)number.Type;
                    rightness[i, j] = number.IsRight();
                }
            }
        }

        /// <summary>
        /// Возвращает все ли в судоку числа заполнены правильно
        /// </summary>
        /// <returns></returns>
        public bool IsRight()
        {
            for (int i = 0; i < this.Number.Count; i++)
            {
                if (!this.Number[i].IsRight())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Возвращает все ли поля заполнены в судоку
        /// </summary>
        /// <returns></returns>
        public bool IsFilled()
        {
            for (int i = 0; i < this.Number.Count; i++)
            {
                if (this.Number[i].Type == LousySudoku.Number.NumberType.Empty)
                {
                    return false;
                }
            }
            OnFilled(this);
            return true;
        }

        /// <summary>
        /// Возвращает заполненно все судоку полностью и правильно
        /// </summary>
        /// <returns></returns>
        public bool IsCompleted()
        {
            bool result = this.IsFilled() && this.IsRight();
            if (result)
            {
                OnCompleted(this);
            }
            return result;
        }

        /// <summary>
        /// Очищает все ячейки судоку
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < this.Number.Count; i++)
            {
                this.Number[i].Clear();
            }
        }

        /// <summary>
        /// Перемешивает ссылки на ячейки в судоку в массиве
        /// Не меняет позиции
        /// </summary>
        public void MixNumbers()
        {
            Generator generator = new Generator(this);
            this.Number
                = generator.MixItems(this.Number.ToArray()).ToList();
        }

        /// <summary>
        /// Перемешивает ссылки на блоки в судоку в массиве
        /// Не меняет позиции
        /// </summary>
        public void MixBlocks()
        {
            Generator generator = new Generator(this);
            this.Block = generator.MixItems(this.Block.ToArray()).ToList();
        }

        /// <summary>
        /// Посчитывает сколько ячеек должно содержать или содержит числа
        /// </summary>
        /// <returns></returns>
        public int GetNumberCount()
        {
            int result = 0;
            foreach (Number number in this.Number)
            {
                if (number.IsExist)
                {
                    result++;
                }
            }
            return result;
        }

        public int GetNumberCount0()
        {
            int result = 0;
            foreach (Number number in this.Number)
            {
                if (number.HasValue)
                {
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// Удаляет из судоку указанное количество ячеек
        /// </summary>
        /// <param name="count"></param>
        public void DeleteNumbers(int count)
        {
            this.MixNumbers();
            for (int i = 0; i < count; i++)
            {
                this.Number[i].Clear();
            }
        }

        /// <summary>
        /// Метод вызывающийся при заполнении и завершении судоку
        /// </summary>
        /// <param name="sudoku"></param>
        private void EmptySudokuEventHandler(Sudoku sudoku)
        {  }

        /// <summary>
        /// Returns a copy of object;
        /// </summary>
        /// <returns></returns>
        public Sudoku Copy()
        {
            return null;
                //(Sudoku)((IStringify)(new Sudoku(null, null, null, null, 0))).Unstringify(
                //    ((IStringify)this).Stringify(
                //        Stringify_Help.CopyList(Stringify_Help.SeparatorListDefault)
                //    ),
                //    Stringify_Help.CopyList(Stringify_Help.SeparatorListDefault)
                //);
        }
        
        /// <summary>
        /// Возвращает массив ячеек по размеру матрицы, матрице значений и маске ячеек
        /// </summary>
        /// <param name="size"></param>
        /// <param name="number"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        private static Number[] GetNumbers(Position size, int[,] number, Number.NumberType[,] mask)
        {
            Number[] result = new Number[size.X * size.Y];
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    result[i * size.X + j] = new Number(mask[i, j], new Position(i, j), number[i, j]);
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

        protected bool LoadXml_NumberSection(XElement element)
        {
            Tag tag = new Tag();
            tag.LoadXml(element);
            List<XElement> child = tag.GetChildren(Constant.Xml.NumberTag);
            for(int i = 0; i < child.Count; i++)
            {
                Number newNumber = new Number(0, null);
                newNumber.LoadXml(child[i]);
                this.Number.Add(newNumber);
            }
            return true;
        }

        protected bool LoadXml_BlockSection(XElement element)
        {
            Tag tag = new Tag();
            tag.LoadXml(element);
            List<XElement> child = tag.GetChildren(Constant.Xml.NumberTag);
            for (int i = 0; i < child.Count; i++)
            {
                Block newBlock = new Block(null);
                newBlock.LoadXml(child[i]);
                this.Block.Add(newBlock);
            }
            return true;
        }

        protected bool LoadXml_InfoSection(XElement element)
        {
            Tag tag = new Tag();
            tag.LoadXml(element);
            Tag maxValueTag = new Tag();
            maxValueTag.LoadXml
                (tag.GetChild(Constant.Xml.Sudoku.MaxValue));
            this.MaxValue = Convert.ToInt32(maxValueTag.Value);
            this.Size = new Position();
            this.Size.LoadXml(tag.GetChild(this.Size.NameXml));
            return true;
        }

        protected bool LoadXml_MethodSection(XElement element)
        {
            // NNBB; todo;
            return true;
        }

        protected static XElement UnloadXml_Section
            (List<IXmlize> list, string nameXml)
        {
            List<XElement> child = new List<XElement>();
            for (int i = 0; i < list.Count; i++)
            {
                child.Add(list[i].UnloadXml());
            }
            XElement result = new XElement
                (nameXml);
            result.Add(child);
            return result;
        }

        protected XElement UnloadXml_NumberSection()
        {
            return Sudoku.UnloadXml_Section(
                this.Number.ToList<IXmlize>(),
                Constant.Xml.Sudoku.NumberSection
                );
        }

        protected XElement UnloadXml_BlockSection()
        {
            return Sudoku.UnloadXml_Section(
                this.Block.ToList<IXmlize>(),
                Constant.Xml.Sudoku.BlockSection
                );
        }

        protected XElement UnloadXml_InfoSection()
        {
            Tag maxValueXml = new Tag(
                   name: Constant.Xml.Sudoku.MaxValue,
                   value: this.MaxValue.ToString()
                   );
            Tag result = new Tag(
                name: Constant.Xml.Sudoku.InfoSection,
                value: null,
                child: new List<XElement>
                {
                    maxValueXml.UnloadXml(),
                    this.Size.UnloadXml()
                }
                );
            return result.UnloadXml();
        }

        protected XElement UnloadXml_MethodSection()
        {
            // NNBB; todo;
            //Tag checkerXml = new Tag(
            //    //element: this.checker.UnloadXml(),
            //    attribute: new Dictionary<string, string>
            //    {
            //        {
            //            Constant.Xml.BlockType.MethodAttribute,
            //            Constant.Xml.BlockType.MethodAttributeChecker
            //        }
            //    }
            //    );
            //Tag generatorXml = new Tag(
            //    //element: this.generator.UnloadXml(),
            //    attribute: new Dictionary<string, string>
            //    {
            //        {
            //            Constant.Xml.BlockType.MethodAttribute,
            //            Constant.Xml.BlockType.MethodAttributeGenerator
            //        }
            //    }
            //    );
            Tag result = new Tag(
                name: Constant.Xml.Sudoku.MethodSection,
                value: null,
                child: new List<XElement>
                {
                    // checkerXml.UnloadXml(),
                    // generatorXml.UnloadXml()
                }
                );
            return result.UnloadXml();
        }

        public XElement ElementXml
        {
            get;
            private set;
        }

        public string NameXml
        {
            get { return Constant.Xml.Sudoku.Tag; }
        }

        public bool LoadXml(XElement element)
        {
            Tag tag = new Tag();
            tag.LoadXml(element);
            this.LoadXml_InfoSection
                (tag.GetChild(Constant.Xml.Sudoku.InfoSection));
            this.LoadXml_MethodSection
                (tag.GetChild(Constant.Xml.Sudoku.MethodSection));
            this.LoadXml_NumberSection
                (tag.GetChild(Constant.Xml.Sudoku.NumberSection));
            this.LoadXml_BlockSection
                (tag.GetChild(Constant.Xml.Sudoku.BlockSection));
            return true;
        }

        public XElement UnloadXml()
        {
            Tag tag = new Tag(
                name: this.NameXml,
                value: null,
                child: new List<XElement>
                {
                    this.UnloadXml_InfoSection(),
                    this.UnloadXml_MethodSection(),
                    this.UnloadXml_BlockSection(),
                    this.UnloadXml_NumberSection()
                }
                );
            return tag.UnloadXml();
        }

    }

}