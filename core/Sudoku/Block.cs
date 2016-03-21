using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Linq;
using Alist;
using Alist.Tree;
using Alist.Error;
using Alist.Xml;

namespace LousySudoku
{

    /// <summary>
    /// Описывает блок чисел
    /// Блок чисел - это массив чисел, подчиняющийся одному правилу (нет повторяющихся чисел)
    /// </summary>
    public class Block
        : IXmlsaver, IActivatable,
        ICloneable
    {
        
        private List<Number> child;

        /// <summary>
        /// Luke, I am your Father.
        /// Well, it's not funny, I know...
        /// I don't fance starWars anyway...
        /// Haven't seen even that moment...
        /// </summary>
        public Sudoku Father
        {
            get;
            private set;
        }

        /// <summary>
        /// Cells, that belongs to block
        /// </summary>
        public List<Number> Child
        {
            get
            {
                return this.child.AsReadOnly().ToList();
            }
            private set
            {
                this.DeleteChild();
                this.AddChildren(value);
            }
        }

        /// <summary>
        /// Id of blockType.
        /// Uses for correct xml parsing.
        /// </summary>
        public Adress TypeId
        {
            get;
            private set;
        }

        private BlockType blockType;
        
        public Block(Sudoku sudoku, BlockType type, List<Number> children)
        {
            this.child = new List<Number> { };
            this.Child = children;
            this.blockType = type;
            this.Father = sudoku;
        }
        
        public Block(Sudoku sudoku, BlockType type)
            : this(sudoku, type, new List<Number> { })
        {   }
        
        public Block(Sudoku sudoku, Adress typeId, List<Number> children)
        {
            this.child = new List<Number> { };
            this.Child = children;
            this.TypeId = typeId;
            this.Father = sudoku;
            this.blockType = null;
        }

        public Block(Sudoku sudoku, Adress typeId)
            : this(sudoku, typeId, null)
        {   }

        public Block(Sudoku sudoku)
            : this(sudoku, new Adress(Constant.BlockTypeIdDefault))
        {   }

        private void GetValuesMask(ref int[] value, ref bool[] mask)
        {
            value = new int[this.child.Count];
            mask = new bool[this.child.Count];
            for (int i = 0; i < this.child.Count; i++)
            {
                mask[i] = this.child[i].HasValue;
                value[i] = this.child[i].Value;
            }
        }
        
        /// <summary>
        /// Calls check method of block.
        /// BlockType contains that method
        /// </summary>
        /// <returns></returns>
        public Number[] Check()
        {
            return this.blockType.Checker(this);
        }

        public void SetParent(Sudoku sudoku)
        {
            if (this.Father == null)
                this.Father = sudoku;
        }

        public bool Generate()
        {
            return this.blockType.Generator(this);
        }

        /// <summary>
        /// Delete all links to child cells
        /// </summary>
        private void DeleteChild()
        {
            this.child = new List<Number> { };
        }

        /// <summary>
        /// Indicates if block has number with specified coordinate
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public bool HasChild(Position coordinate)
        {
            if (coordinate == null)
                return false;
            foreach (Number number in this.child)
            {
                if (number.Coordinate == coordinate)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Indicates if block has specified number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool HasChild(Number number)
        {
            if (number == null)
                return false;
            return this.HasChild(number.Coordinate);
        }

        /// <summary>
        /// Add specified number as block child.
        /// Add itself as parent to number
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(Number child)
        {
            if (child == null)
                return;
            if (!this.HasChild(child))
            {
                this.child.Add(child);
                child.AddParent(this);
            }
        }

        public void AddChildren(IEnumerable<Number> child)
        {
            if (child == null)
                return;
            foreach (Number number in child)
            {
                this.AddChild(number);
            }
        }

        public void AddChildren(List<Position> childCoordinate)
        {
            this.Child = childCoordinate.ConvertAll<Number>
                (x => this.Father.GetNumber(x));
        }

        /// <summary>
        /// Check if all numbers set right in block
        /// </summary>
        /// <returns></returns>
        public bool IsRight()
        {
            return (this.Check().Length == 0);
        }

        /// <summary>
        /// Return coordinates of all children cells
        /// </summary>
        /// <returns></returns>
        public List<Position> GetCoordinates()
        {
            List<Position> result = new List<Position> { };
            foreach(Number number in this.child)
            {
                result.Add(number.Coordinate);
            }
            return result;
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
                if (this.Father == null)
                {
                    error += Constant.Exception.BlockSudokuNotSet;
                }
                else
                {
                    if (this.blockType == null)
                    {
                        try
                        {
                            this.blockType
                                = this.Father.GetBlockType(this.TypeId);
                            this.IsInitialized = true;
                        }
                        catch (ApplicationException exception)
                        {
                            error += exception;
                            this.IsInitialized = false;
                        }
                    }
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
                this.blockType = null;
                this.child = null;
            }
            else
            {
                error += Alist.Constant.Exception.RepeatFinalization;
            }
            return error;
        }

        public XElement ElementXml
        {
            get;
            private set;
        }

        public string NameXml
        {
            get { return Constant.Xml.BlockTag; }
        }

        public bool LoadXml(XElement element)
        {
            Tag tag = new Tag();
            tag.LoadXml(element);
            List<XElement> position 
                = tag.GetChildren(Constant.Xml.PositionTag);
            foreach(XElement elem in position)
            {
                Position coordinate = new Position();
                coordinate.LoadXml(elem);
                Number newChild = this.Father.GetNumber(coordinate);
                this.AddChild(newChild);
            }
            this.TypeId = new Adress(tag.GetAttribute
                (Constant.Xml.BlockType.IdAttribute, Constant.BlockTypeIdDefault));
            return true;
        }

        public XElement UnloadXml()
        {
            List<Number> child = this.child;
            List<XElement> childXml 
                = new List<XElement> { };
            foreach(Number cell in child)
            {
                childXml.Add(cell.Coordinate.UnloadXml());
            }
            Tag tag = new Tag(
                name: this.NameXml,
                value: null,
                child: childXml,
                attribute: null
                );
            return tag.UnloadXml();
        }

        public object Clone()
        {
            Block clone = new Block(sudoku: null);
            clone.TypeId = this.TypeId;
            clone.blockType = this.blockType;
            clone.IsInitialized = this.IsInitialized;
            clone.child = new List<Number> { };
            return clone;
        }

    }

}