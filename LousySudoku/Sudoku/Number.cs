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

            public Position(int x, int y)
            {
                this.X = x;

                this.Y = y;
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

        public bool IsModified
        {
            get
            {
                switch (this.type)
                {
                    case NumberType.Unexists:
                    case NumberType.Constant:
                        return false;

                    case NumberType.Modify:
                    case NumberType.Empty:
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

        public bool IsRight()
        {
            bool result = true;
            for (int i = 0; i < parents.Length; i++ )
            {
                Number[] wrong_number = parents[i].Check();
                for (int j = 0; j < wrong_number.Length; j++)
                {
                    if (this.IsSame(wrong_number[j]))
                    {
                        return false;
                    }
                }
            }
            return result;
        }

        public bool IsSame(Number number)
        {
            return this.position.IsSame(number.position);
        }

        public bool Modify(int new_value)
        {
            if (new_value == 0)
                return this.Clear();
            if (IsModified)
                this.value = new_value;
            return this.IsModified;
        }

        private bool Clear()
        {
            if (this.IsModified)
                this.value = 0;
            return this.IsModified;
        }

    }
}
