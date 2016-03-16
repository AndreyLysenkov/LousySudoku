using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using Alist;
using Alist.Tree;
using Alist.Assembly;
using Alist.Error;

namespace LousySudoku
{

    /// <summary>
    /// Defindes the type of block.
    /// Contains methods for generating and checking 
    /// (is all numbers set correct) block
    /// </summary>
    public class BlockType
        : IXmlize, IActivatable,
        IEquatable<BlockType>, IEquatable<Adress>
    {

        public Adress Id
        {
            get;
            private set;
        }
        
        public IExternalMethod Checker
        {
            get;
            private set;
        }

        public IExternalMethod Generator
        {
            get;
            private set;
        }

        public BlockType(
            Adress id,
            IExternalMethod checker, 
            IExternalMethod generator)
        {
            this.Id = id;
            this.Checker = checker;
            this.Generator = generator;
        }

        public BlockType(
            string id,
            IExternalMethod checker,
            IExternalMethod generator)
            : this(
                  id: new Adress(id),
                  checker: checker,
                  generator: generator)
        {   }

        public BlockType()
            : this("", null, null)
        {   }

        public bool IsInitialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Запускает внешний метод. Указывется ссылка на блок и два параметра, передающиеся в метод
        /// Возвращает то, что внешний метод и возвращает, obviously
        /// </summary>
        /// <param name="block"></param>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public int[] RunCheck(Block block, int[] value, bool[] mask)
        {
            if (this.Checker == null)
            {
                throw new ApplicationException("temporary exception");
                return null;
            }
            return (int[])(this.Checker.Run(null, new object[2] { (object)value, (object)mask }));
        }
        
        public void RunGenerator()
        {
            // NNBB; todo;
            return;
        }

        public MultyException Initialize()
        {
            MultyException error = new MultyException();
            if (!this.IsInitialized)
            {
                if ((this.Checker != null) && (this.Generator != null))
                {
                    error += this.Checker.Initialize();
                    error += this.Generator.Initialize();
                    this.IsInitialized =
                        this.Checker.IsInitialized
                        && this.Generator.IsInitialized;
                }
                else
                {
                    error += Constant.Exception.BlockTypeNotSet;
                }
            }
            else
            {
                error += Alist.Constant.Exception.RepeatInitialization;
            }
            return error;
        }

        public MultyException Finilize()
        {
            MultyException error = new MultyException();
            if (this.IsInitialized)
            {
                error += this.Checker.Finilize();
                error += this.Generator.Finilize();
                this.IsInitialized = false;
            }
            else
            {
                error += Alist.Constant.Exception.RepeatFinalization;
            }
            return error;
        }

        public string NameXml
        {
            get { return Constant.Xml.BlockType.Tag; }
        }

        public bool LoadXml(XElement element)
        {
            Alist.Xml.Tag tag = new Alist.Xml.Tag();
            tag.LoadXml(element);
            if (this.Checker == null)
                this.Checker = (new ExternalMethod(null));
            if (this.Generator == null)
                this.Generator = (new ExternalMethod(null));
            List<XElement> method = tag.GetChildren(this.Checker.NameXml);
            method.AddRange(tag.GetChildren(this.Generator.NameXml));
            for (int i = 0; i < method.Count; i++)
            {
                Alist.Xml.Tag methodTag = new Alist.Xml.Tag();
                methodTag.LoadXml(method[i]);
                string attribute = methodTag.GetAttribute
                    (Constant.Xml.BlockType.MethodAttribute);
                if (attribute.ToLower() 
                    == Constant.Xml.BlockType.MethodAttributeChecker)
                {
                    this.Checker.LoadXml(method[i]);
                }
                if (attribute.ToLower()
                        == Constant.Xml.BlockType.MethodAttributeGenerator)
                {
                    this.Generator.LoadXml(method[i]);
                }
            }
            this.Id = new Adress
                (tag.GetAttribute(Constant.Xml.BlockType.IdAttribute));
            return true;
        }

        public XElement UnloadXml()
        {
            XElement checkerXml = this.Checker.UnloadXml();
            checkerXml.SetAttributeValue(
                Constant.Xml.BlockType.MethodAttribute,
                Constant.Xml.BlockType.MethodAttributeChecker
                );
            XElement generatorXml = this.Checker.UnloadXml();
            generatorXml.SetAttributeValue(
                Constant.Xml.BlockType.MethodAttribute,
                Constant.Xml.BlockType.MethodAttributeGenerator
                );
            Alist.Xml.Tag tag = new Alist.Xml.Tag(
                name: this.NameXml,
                value: null,
                child: new List<XElement> { checkerXml, generatorXml }
                );
            XElement result = tag.UnloadXml();
            result.SetAttributeValue(
                Constant.Xml.BlockType.IdAttribute,
                this.Id.ToString()
                );
            return result;
        }
        
        public bool Equals(Adress id)
        {
            return this.Id == id;
        }
        
        public bool Equals(BlockType other)
        {
            return this.Equals(other.Id);
        }

    }

}