﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    public class Number : IStringify
    {

        public class Position
        {

            public int X
            {
                private set;
                get;
            }

            public int Y
            {
                private set;
                get;
            }

            public Position()
            {
                X = 0;

                Y = 0;
            }

            public bool IsSame(Position position)
            {
                return ((position.X == this.X) && (position.Y == this.Y));
            }

        }

        public enum NumberType
        {
            Unexists,
            Empty,
            Constant,
            Modify
        }

        int value;

        public NumberType type;

        Block[] parents;

        public Position position;

        public int Value
        {
            get { return this.value; }
        }

        public bool HasValue
        {
            get
            {
                switch (this.type)
                {
                    case NumberType.Unexists:
                    case NumberType.Empty:
                        return false;

                    case NumberType.Modify:
                    case NumberType.Constant:
                        return true;

                    default: return false;
                }
            }
        }

        string IStringify.Stringify()
        {


            return null;
        }

        IStringify IStringify.Unstringify(string value)
        {
            return null;
        }

        bool IsRight()
        {
            return false;
        }

    }
}
