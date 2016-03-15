using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alist;

namespace LousySudoku
{

    /// <summary>
    /// Definds number (cell) of sudoku it's position, value, 
    /// can or cannot user change it's value
    /// </summary>
    public class Number
        : IXmlize, ICloneable, IComparable<Number>,
        IEquatable<Number>, IEquatable<Position>
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
        /// Describes blocks, wich one current cell belongs to.
        /// Every block should add here link to itself 
        /// throught this.AddParent(Block)
        /// </summary>
        private List<Block> parent;

        /// <summary>
        /// Number of current cell. It's relevant 
        /// if this.Type set as Modify or Constant
        /// </summary>
        private int value;

        /// <summary>
        /// Current cell status. Can't be changed.
        /// (Except, Empty on Modify, or Modify on Empty)
        /// </summary>
        public NumberType Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns value storing in current cell. It's relevant 
        /// if this.Type set as Modify or Constant
        /// </summary>
        public int Value
        {
            get
            {
                return this.value;
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
                    case NumberType.Unexists:
                    case NumberType.Empty:
                        return false;

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
        public bool IsModified
        {
            get
            {
                switch (this.Type)
                {
                    case NumberType.Unexists:
                    case NumberType.Constant:
                        return false;

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
                return !(this.Type == NumberType.Unexists);
            }
        }

        /// <summary>
        /// Creates new cell with specified fields
        /// </summary>
        /// <param name="type"></param>
        /// <param name="coordinate"></param>
        /// <param name="value"></param>
        public Number(NumberType type, Position coordinate, int value = 0)
        {
            this.value = value;
            this.Type = type;
            this.Coordinate = coordinate;
            this.parent = new List<Block> { };
            this.UpdateValueByType();
            this.UpdateTypeByValue();
        }

        /// <summary>
        /// If type indicates, that record have no value, sets value to 0.
        /// Recomended to use only in constructor.
        /// </summary>
        protected void UpdateValueByType()
        {
            switch(this.Type)
            {
                case NumberType.Empty:
                case NumberType.Unexists:
                    {
                        this.value = 0;
                        break;
                    }
            }
        }

        /// <summary>
        /// Sets type of cell according value:
        /// if type says cell has value, but value is 0,
        /// then change it's type on type with no value.
        /// And other way round.
        /// Recomended to use only in constructor.
        /// </summary>
        protected void UpdateTypeByValue()
        {
            switch (this.value)
            {
                case 0:
                    {
                        if (this.IsModified)
                            this.Type = NumberType.Empty;
                        else
                            this.Type = NumberType.Unexists;
                        break;
                    }
                default:
                    {
                        if (this.IsModified)
                            this.Type = NumberType.Modify;
                        else
                            this.Type = NumberType.Constant;
                        break;
                    }
            }
        }

        /// <summary>
        /// Calculates if cell's value is right.
        /// i.e. goes throught all blocks and check if they are right.
        /// </summary>
        /// <returns></returns>
        public bool IsRight()
        {
            for (int i = 0; i < parent.Count; i++ )
            {
                Number[] wrongNumber = parent[i].Check();
                for (int j = 0; j < wrongNumber.Length; j++)
                {
                    if (this.Equals(wrongNumber[j]))
                    {
                        return false;
                    }
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
            if (IsModified)
            {
                this.value = new_value;
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
            if (this.IsModified)
            {
                this.value = 0;
                return true;
            }
            return false;
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
                this.AddParent(newParent);
            }
        }

        public string NameXml
        {
            get { return Constant.Xml.NumberTag; }
        }

        public bool LoadXml(System.Xml.Linq.XElement element)
        {
            Alist.Xml.Tag tag = new Alist.Xml.Tag();
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
            return true;
        }

        public System.Xml.Linq.XElement UnloadXml()
        {
            System.Xml.Linq.XElement position 
                = this.Coordinate.UnloadXml();
            Alist.Xml.Tag tag = new Alist.Xml.Tag(
                name: this.NameXml,
                value: this.Value.ToString(),
                attribute: new Dictionary<string, string>
                {
                    {
                        Constant.Xml.NumberTypeAttribute,
                        this.Type.ToString()
                    }
                },
                child: new List<System.Xml.Linq.XElement>
                {
                    position
                }
                );
            return tag.UnloadXml();
        }

        public object Clone()
        {
            return new Number(
                type: this.Type, 
                coordinate: (Position)this.Coordinate.Clone(), 
                value: this.Value);
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