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

        /// <summary>
        /// Method, checking block of numbers. 
        /// Returns indexes in value array, wich do not fit
        /// </summary>
        /// <param name="block">Ref to block, calling method</param>
        /// <param name="value">Setted number in block</param>
        /// <param name="mask">Each element indicates is element with same
        /// index in value array has got value</param>
        /// <returns></returns>
        public delegate int[] CheckMethod
            (Block block, int[] value, bool[] mask);

        /// <summary>
        /// Method fills missing number in value array.
        /// Return success.
        /// </summary>
        /// <param name="block">Ref to block, calling method</param>
        /// <param name="value">Setted number in block</param>
        /// <param name="mask">Each element indicates is element with same
        /// index in value array has got value</param>
        /// <returns></returns>
        public delegate bool GenerateMethod
            (Block block, int[] value, bool[] mask);

        /// <summary>
        /// Identificator of block types.
        /// Uses for correct loading from xml file.
        /// </summary>
        public Adress Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Method, that check block on wrong number 
        /// and returns wrong numbers' indexes
        /// </summary>
        public CheckMethod Checker
        {
            get;
            private set;
        }

        /// <summary>
        /// Method, that fill block with numbers
        /// and returns if successed
        /// </summary>
        public GenerateMethod Generator
        {
            get;
            private set;
        }

        private IExternalMethod checkerExternal;

        private IExternalMethod generatorExternal;

        public BlockType(Adress id)
        {
            this.Id = id;
            this.Checker = null;
            this.Generator = null;
            this.generatorExternal = null;
            this.checkerExternal = null;
        }

        public BlockType()
            : this(new Adress(Alist.Constant.Undefinded))
        {   }

        public bool SetGenerator(GenerateMethod generator)
        {
            if (this.Generator != null)
                return false;
            this.Generator = generator;
            return true;
        }

        public bool SetGenerator(IExternalMethod external)
        {
            if (this.Generator != null)
                return false;
            this.generatorExternal = external;
            this.Generator = this.ExternalGenerate;
            return true;
        }

        public bool SetChecker(CheckMethod checker)
        {
            if (this.Checker != null)
                return false;
            this.Checker = checker;
            return true;
        }

        public bool SetCheker(IExternalMethod external)
        {
            if (this.Checker != null)
                return false;
            this.checkerExternal = external;
            this.Checker = this.ExternalCheck;
            return true;
        }

        /// <summary>
        /// Runs check method from external assambly.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public int[] ExternalCheck(Block block, int[] value, bool[] mask)
        {
            return (int[])(this.checkerExternal.Run(
                caller: null,
                parameter: new object[3] { this, value, mask }));
        }

        /// <summary>
        /// Runs generate method from external assambly.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public bool ExternalGenerate(Block block, int[] value, bool[] mask)
        {
            return (bool)(this.generatorExternal.Run(
                caller: null,
                parameter: new object[3] { this, value, mask }));
        }

        public bool IsInitialized
        {
            get;
            private set;
        }

        public MultyException Initialize()
        {
            MultyException error = new MultyException();
            if (!this.IsInitialized)
            {
                if ((this.Checker == null)
                    && (this.checkerExternal == null))
                {
                    error += Constant.Exception.BlockTypeCheckerNotSet;
                    this.Checker = Method.CheckMethod_Standart;
                }
                if ((this.Generator == null)
                    && (this.generatorExternal == null))
                {
                    this.Generator = Method.GenerateMethod_Standart;
                }

                bool success = true;
                if (this.checkerExternal != null)
                {
                    error += this.checkerExternal.Initialize();
                    success = success 
                        ? this.checkerExternal.IsInitialized
                        : success;
                }
                if (this.generatorExternal != null)
                {
                    error += this.generatorExternal.Initialize();
                    success = success
                        ? this.generatorExternal.IsInitialized
                        : success;
                }
                this.IsInitialized = success;
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
                if (this.checkerExternal != null)
                    error += this.checkerExternal.Finilize();
                if (this.generatorExternal != null)
                    error += this.generatorExternal.Finilize();
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
            if (this.checkerExternal == null)
                this.checkerExternal = (new ExternalMethod(null));
            if (this.generatorExternal == null)
                this.generatorExternal = (new ExternalMethod(null));
            List<XElement> method = tag.GetChildren
                (this.checkerExternal.NameXml);
            method.AddRange(tag.GetChildren
                (this.generatorExternal.NameXml));
            for (int i = 0; i < method.Count; i++)
            {
                Alist.Xml.Tag methodTag = new Alist.Xml.Tag();
                methodTag.LoadXml(method[i]);
                string attribute = methodTag.GetAttribute
                    (Constant.Xml.BlockType.MethodAttribute);
                if (attribute.ToLower() 
                    == Constant.Xml.BlockType.MethodAttributeChecker)
                {
                    this.checkerExternal.LoadXml(method[i]);
                }
                if (attribute.ToLower()
                        == Constant.Xml.BlockType.MethodAttributeGenerator)
                {
                    this.generatorExternal.LoadXml(method[i]);
                }
            }
            this.Id = new Adress
                (tag.GetAttribute(Constant.Xml.BlockType.IdAttribute));
            return true;
        }

        public XElement UnloadXml()
        {
            XElement checkerXml = this.checkerExternal.UnloadXml();
            checkerXml.SetAttributeValue(
                Constant.Xml.BlockType.MethodAttribute,
                Constant.Xml.BlockType.MethodAttributeChecker
                );
            XElement generatorXml = this.generatorExternal.UnloadXml();
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