using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Alist;
using Alist.Error;
using Alist.Xml;
using Alist.Assembly;

namespace LousySudoku
{

    /// <summary>
    /// Describes Sudoku. 
    /// Can be used for template for sudoku generation
    /// </summary>
    public class Sudoku
        : 
        IXmlsaver, 
        IActivatable,
        ICloneable
    {

        /// <summary>
        /// Contains all sudoku cells
        /// </summary>
        public List<Number> Number
        {
            get;
            private set;
        }

        /// <summary>
        /// Contains all sudoku blocks
        /// </summary>
        public List<Block> Block
        {
            get;
            private set;
        }

        /// <summary>
        /// Types of block
        /// </summary>
        public List<BlockType> BlockType
        {
            get;
            private set;
        }

        /// <summary>
        /// Max value of any cell in sudoku.
        /// Any cell of sudoku must have value in range [1..MaxValue]
        /// </summary>
        public int MaxValue
        {
            get;
            private set;
        }

        public Position Size
        {
            get { return this.CalculateSize(); }
        }
        
        private int emptyCell;

        public delegate void Generator(Sudoku sudoku);

        private IExternalMethod generator_external;

        private Generator generator;

        /// <summary>
        /// Delegate for sudoku events OnCompleted and OnFilled
        /// </summary>
        /// <param name="sudoku"></param>
        public delegate void SudokuEvent (Sudoku sudoku);

        /// <summary>
        /// Delegate of sudoku event onChanging
        /// </summary>
        /// <param name="sudoku"></param>
        /// <param name="number"></param>
        /// <param name="oldValue"></param>
        public delegate void SudokuChangeNumberEvent(
            Sudoku sudoku, 
            Number number,
            int oldValue);

        /// <summary>
        /// Calls on clearing sudoku, i.e. deleting all values
        /// </summary>
        public event SudokuEvent onClear;

        /// <summary>
        /// Calls if all cells filled with value
        /// and all block are right
        /// </summary>
        public event SudokuEvent onCompleted;

        /// <summary>
        /// Calls if all cells filled with value
        /// </summary>
        public event SudokuEvent onFilled;

        /// <summary>
        /// Trigger event on changing number
        /// </summary>
        public event SudokuChangeNumberEvent onChanging;

        public Sudoku(
            List<BlockType> blockType, 
            List<Block> block, 
            List<Number> cell, 
            int maxValue)
        {
            if (blockType == null)
                blockType = new List<BlockType> { };
            if (block == null)
                block = new List<Block> { };
            if (cell == null)
                cell = new List<Number> { };
            if (maxValue < 0)
                maxValue = 0;

            this.BlockType = blockType;
            this.Block = block;
            this.Number = cell;
            this.MaxValue = maxValue;

            this.onClear += (x => x.Nothing());
            this.onCompleted += (x => x.Nothing());
            this.onFilled += (x => x.Nothing());
            this.onChanging += ((x, y, z) => x.Nothing());
            // ??
            this.Number.Sort();
        }

        public Sudoku()
            : this(blockType: null, block: null, cell: null, maxValue: 0)
        {   }

        public bool SetGenerator(Generator method)
        {
            if (this.generator == null)
            {
                this.generator = method;
                return true;
            }
            return false;
        }

        public bool SetGenerator(IExternalMethod method)
        {
            if (this.generator_external == null)
            {
                this.generator_external = method;
                return true;
            }
            return false;
        }

        private static void Generate_External(Sudoku sudoku)
        {
            sudoku.generator_external.Run(null, new object[] { sudoku });
        }

        private Position CalculateSize()
        {
            Position result = new Position();
            foreach(Number cell in this.Number)
            {
                Position coordinate = cell.Coordinate;
                for (int i = 0; i < coordinate.Dimention; i++)
                {
                    int newOrdinate 
                        = coordinate.GetCoordinate(dimention: i);
                    if (newOrdinate > result.GetCoordinate(dimention: i))
                    {
                        result.SetCoordinate(
                            dimention: i, 
                            newValue: newOrdinate);
                    }   
                }
            }
            return result;
        }

        /// <summary>
        /// Returns number by it's position
        /// If number do not exist return cell with NumberType.Unexists
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Number GetNumber(Position position)
        {
            Number result 
                = this.Number.Find(x => x.Equals(position));
            if (result == null)
                return new Number(NumberType.Unexists, position);
            return result;
        }

        private void CalculateEmptyCell_Refresh()
        {
            this.emptyCell = this.Number.Where(x => !x.HasValue).Count();
        }

        private void CalculateEmptyCell_OneChange(NumberType was, NumberType became)
        {
            if ((was == NumberType.Empty) && (became == NumberType.Modify))
                this.emptyCell--;
            if ((was == NumberType.Modify) && (became == NumberType.Empty))
                this.emptyCell++;
            EmptyCell_Check();
        }

        private void EmptyCell_Check()
        {
            if (this.emptyCell == 0)
            {
                this.onFilled(this);
                if (this.IsRight())
                    this.onCompleted(this);
            }
            
        }

        private bool GetField_IncList(
            ref List<int> coordinate, 
            Position maxPosition,
            List<bool> mask)
        {
            for (int i = 0; i < coordinate.Count; i++)
            {
                if ((mask[i]) && (coordinate[i] < maxPosition.GetCoordinate(i)))
                {
                    coordinate[i]++;
                    return true;
                }
            }
            return false;
        }

        private List<bool> GetField_DimentionMask
            (ref List<int> coordinate, int[] dimention)
        {
            int length = Math.Max(coordinate.Count, dimention.Max());
            for(int i = coordinate.Count; i < length; i++)
            {
                coordinate.Add(0);
            }
            List<bool> result = new List<bool>(length);
            for (int i = 0; i < dimention.Length; i++)
            {
                result[dimention[i]] = true;
                coordinate[dimention[i]] = 0;
            }
            return result;
        }

        private Position GetField_CalculateFieldSize(List<bool> mask)
        {
            List<int> coordinate = new List<int> { };

            return new Position(coordinate);
        }

        public Number[] GetField(ref Position fieldSize, Position position, params int[] dimention)
        {
            List<Number> result = new List<Number> { };
            if (dimention == null)
                return result.ToArray();
            Position size = this.Size;

            // First coordinate of field == (0;0;0 ... 0);
            List<int> coordinate = position.Coordinate;
            List<bool> mask 
                = GetField_DimentionMask(ref coordinate, dimention);

            bool isContinue = true;
            for (;isContinue;)
            {
                Number number = this.GetNumber(new Position(coordinate));


                isContinue = GetField_IncList(ref coordinate, size, mask);
            }
            return result.ToArray();
        }

        public Number[] GetField1D(Position position, int dimention)
        {
            Position fieldSize = new Position();
            return (Number[])this.GetField
                (ref fieldSize, position, dimention);
        }

        public Number[,] GetField2D
            (Position position, int dimention1, int dimention2)
        {
            Position fieldSize = new Position();
            Number[] preResult = GetField
                (ref fieldSize, position, dimention1, dimention2);
            int length1 = fieldSize.GetCoordinate(0);
            int length2 = fieldSize.GetCoordinate(1);
            Number[,] result = new Number[length1, length2];
            for (int i = 0, k = 0; i < length1; i++, k++)
            {
                for (int j = 0; j < length2; j++,  k++)
                {
                    result[i, j] = preResult[k];
                }
            }
            return result;
        }

        /// <summary>
        /// Change value in cell
        /// </summary>
        /// <param name="position"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetNumber(Position position, int value)
        {
            //return this.GetNumber(position).Modify(value);
            Number cell = this.GetNumber(position);
            NumberType was = cell.Type;
            int oldValue = cell.Value;
            bool success = cell.Modify(value);
            this.CalculateEmptyCell_OneChange(was, cell.Type);
            this.onChanging(this, cell, oldValue);
            return success;
        }

        public bool ClearNumber(Position position)
        {
            return this.GetNumber(position).Clear();
        }

        public void GetWrong
            (ref List<Number> cellResult, ref List<Block> blockResult)
        {

            cellResult = new List<Number> { };
            blockResult = new List<Block> { };
            foreach (Block block in this.Block)
            {
                List<Number> newCheck = block.Check().ToList();
                if (newCheck.Count > 0)
                {
                    blockResult.Add(block);
                    cellResult.Union(newCheck);
                    // ?? that ever for
                    cellResult.Sort();
                }
            }
        }

        public List<Number> GetWrongNumber()
        {
            List<Number> result = new List<Number> { };
            List<Block> tmp = new List<Block> { };
            this.GetWrong(ref result, ref tmp);
            return result;
        }

        public List<Block> GetWrongBlock()
        {
            List<Number> tmp = new List<Number> { };
            List<Block> result = new List<Block> { };
            this.GetWrong(ref tmp, ref result);
            return result;
        }

        /// <summary>
        /// Return if all sudoku cells are right
        /// </summary>
        /// <returns></returns>
        public bool IsRight()
        {
            return this.Block.All(x => x.IsRight());
        }

        /// <summary>
        /// Return if all sudoku cells are filled
        /// </summary>
        /// <returns></returns>
        public bool IsFilled()
        {
            return this.Number.All(x => x.HasValue || !x.IsExist);
        }

        /// <summary>
        /// Return if all sudoku cells are filled and right
        /// </summary>
        /// <returns></returns>
        public bool IsCompleted()
        {
            return this.IsFilled() && this.IsRight();
        }

        public BlockType GetBlockType(Alist.Tree.Adress Id)
        {
            return this.BlockType.Find(x => x.Id == Id);
        }

        /// <summary>
        /// Clears all modify cells of sudoku
        /// </summary>
        public void Clear()
        {
            this.onClear(this);
            this.Number.ForEach(x => x.Clear());
            this.CalculateEmptyCell_Refresh();
        }

        /// <summary>
        /// Deletes all values from sudoku
        /// </summary>
        public void Delete()
        {
            this.Number.ForEach(x => x.Delete());
        }

        public bool IsInitialized
        {
            get;
            private set;
        }

        public MultyException Initialize()
        {
            MultyException error = new MultyException();
            if (!this.IsInitialized)
            {
                this.BlockType.ForEach(x => error += x.Initialize());
                this.Block.ForEach(x => error += x.Initialize());
                if (this.generator_external != null)
                {
                    error += this.generator_external.Initialize();
                    if (generator_external.IsInitialized)
                        this.generator = Sudoku.Generate_External;
                }
                if (this.generator == null)
                {
                    //error += "";
                    //NNBB; todo;
                }
            }
            else
            {
                error += Alist.Constant.Exception.RepeatInitialization;
            }
            return error;
        }

        public MultyException Finilize()
        {
            MultyException error = new MultyException();
            if (this.IsInitialized)
            {
                this.Block.ForEach(x => x.Finilize());
                this.BlockType.ForEach(x => x.Finilize());
                this.Number = null;
                this.Block = null;
                this.BlockType = null;
            }
            else
            {
                error += Alist.Constant.Exception.RepeatFinalization;
            }
            return error;
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
            List<XElement> child = tag.GetChildren(Constant.Xml.BlockTag);
            for (int i = 0; i < child.Count; i++)
            {
                Block newBlock = new Block(this);
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
            return true;
        }

        protected bool LoadXml_MethodSection(XElement element)
        {
            Tag tag = new Tag();
            tag.LoadXml(element);
            List<XElement> generator 
                = tag.GetChildren(Alist.Constant.Xml.Assembly.Tag);
            if (generator.Count > 1)
            {
                this.generator_external = new ExternalMethod(null);
                generator.ForEach(x => this.generator_external.LoadXml(x));
            }
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
                    maxValueXml.UnloadXml()
                }
                );
            return result.UnloadXml();
        }

        protected XElement UnloadXml_MethodSection()
        {
            Tag result = new Tag(
                name: Constant.Xml.Sudoku.MethodSection,
                value: null,
                child: new List<XElement>{ }
                );
            if (this.generator_external != null)
                result.Child.Add(this.generator_external.UnloadXml());
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

        public object Clone()
        {
            List<Number> numb = Method.Clone(this.Number).ToList();
            List<Block> block = Method.Clone(this.Block).ToList();
            // NNBB; fix; on change order in list<block>, may be incorrect coping
            Sudoku result = new Sudoku(
                blockType: this.BlockType,
                block: block,
                cell: numb,
                maxValue: this.MaxValue
                );
            result.Block.ForEach(x => x.SetParent(result));
            for (int i = 0; i < this.Block.Count; i++)
            {
                List<Position> blockCell
                    = this.Block[i].Child.ConvertAll<Position>
                        (x => x.Coordinate);
                block[i].AddChildren(blockCell);
            }
            return result;
        }

        /// <summary>
        /// ??
        /// NNBB; tmp; fix;
        /// </summary>
        private void Nothing()
        {
            return;
        }

    }

}