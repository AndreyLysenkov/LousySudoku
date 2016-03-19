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
    /// Describes Sudoku. 
    /// Can be used for template for sudoku generation
    /// </summary>
    public class Sudoku
        : IXmlsaver, IActivatable,
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
        
        // NNBB; todo;
        private int emptyCell;

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
        /// NNBB; todo;
        /// </summary>
        public event SudokuEvent onCompleted;

        /// <summary>
        /// Calls if all cells filled with value
        /// NNBB; todo;
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
            // ??
            this.Number.Sort();
        }
        
        public Sudoku()
            : this(blockType: null, block: null, cell: null, maxValue: 0)
        {   }

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
            List<Number> result 
                = this.Number.Where(x => x.Equals(position)).ToList();
            if (result.Count == 0)
                return new Number(NumberType.Unexists, position);
            return result[0];
        }

        public object GetField(Position position, params int[] dimention)
        {
            List<object> result = new List<object> { };
            if (dimention == null)
                return result.ToArray();
            for (int i = 0; i < dimention.Length; i++)
            {
                List<int> array = new List<int>();

                // NNBB; todo;
            }
            return result.ToArray();
        }

        public int[] GetField_1D(Position position, int dimention)
        {
            return (int[])this.GetField(position, dimention);
        }

        public int[][] GetField_2D
            (Position position, int dimention1, int dimention2)
        {
            return (int[][])this.GetField(position, dimention1, dimention2);
        }

        public int[][][] GetField_3D(
            Position position, 
            int dimention1, 
            int dimention2, 
            int dimention3)
        {
            return (int[][][])this.GetField(
                position, 
                dimention1, 
                dimention2, 
                dimention3);
        }

        /// <summary>
        /// Change value in cell
        /// </summary>
        /// <param name="position"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetNumber(Position position, int value)
        {
            return this.GetNumber(position).Modify(value);
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
                List<Number> newCheck = block.Check();
                if (newCheck.Count > 0)
                    blockResult.Add(block);
                cellResult.Union(newCheck);
                // ??
                cellResult.Sort();
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
            this.Number.ForEach(x => x.Clear());
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
                    maxValueXml.UnloadXml()
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

        public object Clone()
        {
            // NNBB; todo;
            return null;
        }

    }

}