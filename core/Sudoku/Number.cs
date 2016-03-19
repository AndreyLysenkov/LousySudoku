using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Alist.Xml;

namespace LousySudoku
{

    /// <summary>
    /// Describes status of cell. Status can't be change 
    /// (except Empty may be changed on Modify, and other way round)
    /// </summary>
    public enum NumberType
    {
        /// <summary>
        /// No number can be stored here.
        /// i.e. don't have value, can't be changed by user
        /// </summary>
        Unexists,

        /// <summary>
        /// No number yet is storing here.
        /// i.e. don't have value, can be changed by user
        /// </summary>
        Empty,

        /// <summary>
        /// Storing number, seted by user.
        /// i.e. have value, can be changed by user
        /// </summary>
        Modify,

        /// <summary>
        /// Storing number, seted by program
        /// i.e. have value, can't be changed by user
        /// </summary>
        Constant
    }
    
    /// <summary>
    /// Definds number (cell) of sudoku it's position, value, 
    /// can or cannot user change it's value
    /// </summary>
    public class Number
        : IXmlsaver, ICloneable, IComparable<Number>,
        IEquatable<Number>, IEquatable<Position>
    {

        /// <summary>
        /// Describes blocks, wich one current cell belongs to.
        /// Every block should add here link to itself 
        /// throught this.AddParent(Block)
        /// </summary>
        public List<Block> parent;

        /// <summary>
        /// Current cell status. Can't be changed.
        /// (Except, Empty on Modify, or Modify on Empty)
        /// </summary>
        public NumberType Type
        {
            get;
            private set;
        }

        private int value;

        /// <summary>
        /// Returns value storing in current cell. It's relevant 
        /// if this.Type set as Modify or Constant
        /// </summary>
        public int Value
        {
            get
            {
                if (this.HasValue)
                    return this.value;
                else
                    return 0;
            }
            private set
            {
                if (this.HasValue)
                    this.value = value;
            }
        }

        /// <summary>
        /// Contains position of cell on sudoku field.
        /// Also, uses as cell id, so can't be changed,
        /// and one sudoku can't have two or more sells with same coordinate
        /// </summary>
        public Position Coordinate
        {
            get;
            private set;
        }

        /// <summary>
        /// Indicates if cell contains value.
        /// It is if cell's type is Modify or Constant
        /// </summary>
        public bool HasValue
        {
            get
            {
                switch (this.Type)
                {
                    case NumberType.Modify:
                    case NumberType.Constant:
                        return true;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Indicates if user may modify cell's value.
        /// User may if cell's type is Empty or Modify.
        /// </summary>
        public bool CanModify
        {
            get
            {
                switch (this.Type)
                {
                    case NumberType.Modify:
                    case NumberType.Empty:
                        return true;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Indicates if current cell has or can have value.
        /// It can't if it's type is Unexists.
        /// Applies for non rectangle sudoku.
        /// </summary>
        public bool IsExist
        {
            get
            {
                return (this.Type != NumberType.Unexists);
            }
        }

        /// <summary>
        /// Creates new cell with specified fields
        /// </summary>
        /// <param name="type"></param>
        /// <param name="coordinate"></param>
        /// <param name="value"></param>
        public Number(
            NumberType type, 
            Position coordinate)
        {
            this.value = 0;
            this.Type = type;
            this.Coordinate = coordinate;
            this.parent = new List<Block> { };
        }

        public Number()
            : this(NumberType.Unexists, null)
        {   }

        /// <summary>
        /// Return false if at least one of the parent blocks are wrong
        /// </summary>
        /// <returns></returns>
        public bool IsBlockRight()
        {
            foreach (Block block in this.parent)
            {
                if (!block.IsRight())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Return false if at least one of parent block 
        /// consider current cell as wrong
        /// </summary>
        /// <returns></returns>
        public bool IsRight()
        {
            foreach (Block block in this.parent)
            {
                List<Number> wrong = block.Check();
                foreach (Number number in wrong)
                {
                    if (number == this)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Changes value in cell.
        /// Returns if change possible.
        /// Uses for user changes only
        /// </summary>
        /// <param name="new_value"></param>
        /// <returns></returns>
        public bool Modify(int new_value)
        {
            if (new_value == 0)
            {
                return this.Clear();
            }
            if (this.CanModify)
            {
                this.value = new_value;
                this.Type = NumberType.Modify;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes value of cell.
        /// Return if it is possible.
        /// Uses for user changes only
        /// </summary>
        /// <returns></returns>
        public bool Clear()
        {
            if (this.CanModify)
            {
                this.value = 0;
                this.Type = NumberType.Empty;
                return true;
            }
            return false;
        }

        public void Delete()
        {
            this.value = 0;
            this.Type = NumberType.Empty;
        }

        /// <summary>
        /// Add new block as cell's owner
        /// Return if parent set true, or false if already has that parent
        /// </summary>
        /// <param name="new_parent">новый родительский блок</param>
        public void AddParent(Block newParent)
        {
            if (!this.parent.Any(newParent.Equals))
            {
                this.parent.Add(newParent);
            }
        }

        public XElement ElementXml
        {
            get;
            private set;
        }

        public string NameXml
        {
            get { return Constant.Xml.NumberTag; }
        }

        public bool LoadXml(XElement element)
        {
            this.ElementXml = element;
            Tag tag = new Tag();
            tag.LoadXml(element);
            this.Coordinate = new Position();
            this.Coordinate.LoadXml
                (tag.GetChild(this.Coordinate.NameXml));
            string type = tag.GetAttribute
                (Constant.Xml.NumberTypeAttribute, 
                NumberType.Unexists.ToString());
            type = Alist.Method.ToLowerFirstUpper(type);
            NumberType numberType = NumberType.Unexists;
            bool success = NumberType.TryParse(type, out numberType);
            this.Type = success ? numberType : NumberType.Unexists;
            if (this.HasValue)
            {
                XElement number = tag.GetChild(Constant.Xml.NumberValueTag);
                bool successed = int.TryParse(number.Value, out this.value);
                if (!successed)
                {
                    this.value = 0;
                    if (this.CanModify)
                    {
                        this.Type = NumberType.Empty;
                    }
                    else
                    {
                        this.Type = NumberType.Unexists;
                    }
                }
            }
            return true;
        }

        public XElement UnloadXml()
        {
            XElement position 
                = this.Coordinate.UnloadXml();
            XElement value
                = new XElement(Constant.Xml.NumberValueTag);
            value.Value = this.value.ToString();
            List<XElement> child = new List<XElement> { };
            if (this.HasValue)
                child.Add(value);
            child.Add(position);
            Tag tag = new Tag(
                name: this.NameXml,
                value: null,
                attribute: new Dictionary<string, string>
                {
                    {
                        Constant.Xml.NumberTypeAttribute,
                        this.Type.ToString()
                    }
                },
                child: child,
                element: this.ElementXml
                );
            return tag.UnloadXml();
        }

        public object Clone()
        {
            Number clone = new Number(
                type: this.Type, 
                coordinate: (Position)this.Coordinate.Clone());
            if (this.HasValue)
                clone.Value = this.Value;
            return clone;
        }

        public bool Equals(Number other)
        {
            return this.Equals(other.Coordinate);
        }

        public bool Equals(Position other)
        {
            return this.Coordinate.Equals(other);
        }

        public int CompareTo(Number other)
        {
            return this.Coordinate.CompareTo(other.Coordinate);
        }

    }

}