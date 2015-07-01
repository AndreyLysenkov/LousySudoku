using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{

    public class Number : IStringify
    {

        public class Position
        {

            int x;

            int y;

            public Position()
            {
                x = 0;

                y = 0;
            }

        }

        enum NumberType
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

    }
}
