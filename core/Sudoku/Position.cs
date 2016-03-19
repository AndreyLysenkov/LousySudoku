using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alist;

namespace LousySudoku
{

    /// <summary>
    /// Definds position of number in sudoku.
    /// May be used as unique id
    /// </summary>
    public class Position
        : IXmlize, 
        ICloneable,
        IComparable<Position>,
        IEquatable<Position>
    {

        protected List<int> coordinates;

        /// <summary>
        /// Coordinates of position (x;y;z) etc.
        /// </summary>
        public List<int> Coordinates
        {
            get
            {
                return this.coordinates.AsReadOnly().ToList();
            }
        }

        /// <summary>
        /// Displays how many dimentions uses
        /// current coordinate
        /// </summary>
        public int Dimention
        {
            get { return this.coordinates.Count; }
        }

        /// <summary>
        /// Return length of positon.
        /// i.e. square root of summ of square coordinates.
        /// sqrt(x*x + y*y + z*z);
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(this.coordinates.Sum(x => x * x));
            }
        }
        
        /// <summary>
        /// Column number
        /// </summary>
        public int X
        {
            get
            {
                return this.GetCoordinate(dimention: 0);
            }
        }

        /// <summary>
        /// Row number
        /// </summary>
        public int Y
        {
            get
            {
                return this.GetCoordinate(dimention: 1);
            }
        }

        /// <summary>
        /// Number of layer for 3D sudoku
        /// </summary>
        public int Z
        {
            get
            {
                return this.GetCoordinate(dimention: 2);
            }
        }

        /// <summary>
        /// Create new position with specified coordinates
        /// </summary>
        /// <param name="coordinates"></param>
        public Position(params int[] coordinates)
        {
            this.coordinates = new List<int> { };
            if (coordinates != null)
                this.coordinates.AddRange(coordinates);
        }

        public void SetCoordinate(int dimention, int newValue)
        {
            while (dimention >= this.coordinates.Count)
            {
                this.coordinates.Add(0);
            }
            this.coordinates[dimention] = newValue;
        }

        /// <summary>
        /// Return value of coordinate in that dimension.
        /// For x dimention = 0, y - 1, z - 2, etc.
        /// </summary>
        /// <param name="dimention"></param>
        /// <returns></returns>
        public int GetCoordinate(int dimention)
        {
            if (dimention >= this.Coordinates.Count)
                return 0;
            return this.Coordinates[dimention];
        }

        public static bool operator <= (Position obj1, Position obj2)
        {
            int length = Math.Max 
                (obj1.coordinates.Count, obj2.coordinates.Count);
            for (int i = 0; i < length; i++)
            {
                if (!(obj1.GetCoordinate(i) <= obj2.GetCoordinate(i)))
                    return false;
            }
            return true;
        }

        public static bool operator >= (Position obj1, Position obj2)
        {
            return obj2 <= obj1;
        }

        public bool IsZero()
        {
            return this.coordinates.All(x => x == 0);
        }

        public string NameXml
        {
            get { return Constant.Xml.PositionTag; }
        }

        public bool LoadXml(System.Xml.Linq.XElement element)
        {
            Alist.Xml.Tag tag = new Alist.Xml.Tag();
            tag.LoadXml(element);
            string value = tag.Value;
            List<string> tmp = value.Split(';').ToList();
            this.coordinates = tmp.ConvertAll<int>(Convert.ToInt32);
            return true;
        }

        public System.Xml.Linq.XElement UnloadXml()
        {
            List<string> value 
                = this.coordinates.ConvertAll<string>(Convert.ToString);
            string valueStr = Method.ArrayToString<string>(value, ";");
            Alist.Xml.Tag tag = new Alist.Xml.Tag(
                name: this.NameXml,
                value: valueStr);
            return tag.UnloadXml();
        }

        /// <summary>
        /// Clone position
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Position
                (Method.Clone<int>
                    ((IEnumerable<int>)this.coordinates).ToArray());
        }

        /// <summary>
        /// Returns if this and other positions has same coordinates.
        /// If positions has different dimentions, 
        /// then missing coordinates of dimentions will be consider as 0.
        /// Example, {5;6;0} is equal {5;6}
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Position other)
        {
            int length = Math.Max(this.Dimention, other.Dimention);
            for (int i = 0; i < length; i++)
            {
                if (this.GetCoordinate(i) != other.GetCoordinate(i))
                    return false;
            }
            return true;
        }

        public int CompareTo(Position other)
        {
            int result = 0;
            double differ = this.Length - other.Length;
            result = Convert.ToInt32(differ);
            if ((result == 0) && (!this.Equals(other)))
            {
                int dimention = Math.Max(this.Dimention, other.Dimention);
                for(int i = 0; (i < dimention) && (result == 0); i++)
                {
                    result = this.GetCoordinate(i) - other.GetCoordinate(i);
                }
            }
            return result;
        }

    }

}