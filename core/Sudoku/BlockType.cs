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
        public delegate Number[] CheckMethod(Block block);

        /// <summary>
        /// Method fills missing number in value array.
        /// Return success.
        /// </summary>
        /// <param name="block">Ref to block, calling method</param>
        /// <param name="value">Setted number in block</param>
        /// <param name="mask">Each element indicates is element with same
        /// index in value array has got value</param>
        /// <returns></returns>
        public delegate bool GenerateMethod(Block block);

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

        public BlockType(string id = Constant.BlockTypeIdDefault)
            : this(new Adress(id))
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
        private Number[] ExternalCheck(Block block)
        {
            return (Number[])(this.checkerExternal.Run(
                caller: null,
                parameter: new object[] { this }));
        }

        /// <summary>
        /// Runs generate method from external assambly.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        private bool ExternalGenerate(Block block)
        {
            return (bool)(this.generatorExternal.Run(
                caller: null,
                parameter: new object[] { this }));
        }
        
        public static bool Generate(Number numb, int maxValue)
        {
            if ((numb.HasValue) || (!numb.CanModify))
                return true;

            Random rand = new Random();
            List<int> digit = new List<int> { };
            for (int i = 0; i < maxValue; i++)
                digit.Add(i);

            bool isContinue = true;
            for (; (digit.Count != 0) && isContinue;)
            {
                int index = rand.Next(digit.Count);
                numb.Modify(digit[index]);
                digit.RemoveAt(index);
                if (numb.IsBlockRight())
                    isContinue = false;
            }
            return !isContinue;
        }

        private static Number[] CheckMethod_Standart(Block block)
        {
            // OMG do not ask;
            // NNBB; todo; fix;
            return block.Child.FindAll(
                number =>
                    (number.HasValue)
                    && (block.Child.FindAll(
                        number2 =>
                            (number2.HasValue)
                            && (number2.Value == number.Value)
                        ).Count > 1)
                ).ToArray();
            //Position[]
            //ConvertAll(
            //    number =>
            //        number.HasValue ?
            //            (block.Child.FindAll
            //                (number2 => 
            //                    number2.HasValue 
            //                    && number2.Value == number.Value
            //                ).Count > 1 ?
            //                    number.Coordinate  
            //                    : null)
            //            : null
            //    ).ToArray();
        }

        private static bool GenerateMethod_Standart(Block block)
        {
            //block.Child.All(Generate);
            int maxvalue = block.Father.MaxValue;
            for (int i = 0; i < block.Child.Count; i++)
            {
                if (!Generate(block.Child[i], maxvalue))
                    return false;
            }
            // NNBB; todo;
            return block.IsRight();
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
                    this.Checker = CheckMethod_Standart;
                }
                if ((this.Generator == null)
                    && (this.generatorExternal == null))
                {
                    this.Generator = GenerateMethod_Standart;
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