using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alist;

namespace LousySudoku
{

    // NNBB; todo;
    public class BlockType
        : IXmlize, IEquatable<BlockType>, IEquatable<Alist.Tree.Adress>
    {




        // NNBB; todo;





        public string NameXml
        {
            // NNBB; todo;
            get { return null; }
        }

        public bool LoadXml(System.Xml.Linq.XElement element)
        {
            // NNBB; todo;
            return false;
        }

        public System.Xml.Linq.XElement UnloadXml()
        {
            // NNBB; todo;
            return null;
        }

        public bool Equals(BlockType other)
        {
            // NNBB; todo;
            return false;
        }

        public bool Equals(Alist.Tree.Adress id)
        {
            // NNBB; todo;
            return false;
        }

    }

}