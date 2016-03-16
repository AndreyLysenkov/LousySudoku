using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alist;

namespace LousySudoku
{

    public interface IXmlsaver
        : IXmlize
    {

        System.Xml.Linq.XElement ElementXml
        {
            get;
        }

    }

}