using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alist;

namespace LousySudoku
{

    /// <summary>
    /// Описывает позицию числа в судоку
    /// </summary>
    public class Position : IXmlize
    {

        public List<int> Coordinates
        {
            get;
            private set;
        }

        /// <summary>
        /// Номер столбца
        /// </summary>
        public int X
        {
            get
            {
                return this.GetCoordinate(dimention: 0);
            }
            private set
            {
                this.SetCoordinate(dimention: 0, newValue: value);
            }
        }

        /// <summary>
        /// Номер строки
        /// </summary>
        public int Y
        {
            get
            {
                return this.GetCoordinate(dimention: 1);
            }
            private set
            {
                this.SetCoordinate(dimention: 1, newValue: value);
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
            private set
            {
                this.SetCoordinate(dimention: 2, newValue: value);
            }
        }

        /// <summary>
        /// Создает объект позиции, с заданными координатами
        /// </summary>
        /// <param name="coordinates"></param>
        public Position(params int[] coordinates)
        {
            this.Coordinates = new List<int> { };
            if (coordinates != null)
                this.Coordinates.AddRange(coordinates);
        }

        protected void SetCoordinate(int dimention, int newValue)
        {
            while (dimention > this.Coordinates.Count)
            {
                this.Coordinates.Add(0);
            }
            this.Coordinates[dimention] = newValue;
        }

        protected int GetCoordinate(int dimention)
        {
            if (dimention > this.Coordinates.Count)
                return 0;
            return this.Coordinates[dimention];
        }

        /// <summary>
        /// Возвращает одинаковы ли координаты у этого объекта с position
        /// </summary>
        /// <param name="position"></param>
        /// <returns>равен ли текущий объект данному по значению</returns>
        public bool IsSame(Position position)
        {
            if (this.Coordinates.Count == position.Coordinates.Count)
            {
                for(int i = 0; i < this.Coordinates.Count; i++)
                {
                    if (this.Coordinates[i] != position.Coordinates[i])
                        return false;
                }
                return true;

            }
            return false;
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
            this.Coordinates = tmp.ConvertAll<int>(Convert.ToInt32);
            return true;
        }

        public System.Xml.Linq.XElement UnloadXml()
        {
            List<string> value 
                = this.Coordinates.ConvertAll<string>(Convert.ToString);
            string valueStr = Method.ArrayToString<string>(value, ";");
            Alist.Xml.Tag tag = new Alist.Xml.Tag(
                name: this.NameXml,
                value: valueStr);
            return tag.UnloadXml();
        }

    }

}