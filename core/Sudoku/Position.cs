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
            get;
            private set;
        }

        /// <summary>
        /// Номер строки
        /// </summary>
        public int Y
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of layer for 3D sudoku
        /// </summary>
        public int Z
        {
            get;
            private set;
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
}
