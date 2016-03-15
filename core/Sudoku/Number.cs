using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alist;

namespace LousySudoku
{

    /// <summary>
    /// Описывает число в судоку
    /// </summary>
    public class Number : IXmlize
    {

        /// <summary>
        /// Описывает состояния ячейки судоку
        ///     Unexists - нет значения, нельзя изменить;
        ///     Empty - нет значения, можно изменить;
        ///     Constant - есть значение, нельзя изменить;
        ///     Modify - есть значение, можно изменить;
        /// </summary>
        public enum NumberType
        {
            Unexists,
            Empty,
            Modify,
            Constant
        }

        /// <summary>
        /// Содержит ссылки на блоки, которым принадлежит ячейка
        /// </summary>
        private Block[] parents;

        /// <summary>
        /// Значение ячейки
        /// </summary>
        private int value;

        /// <summary>
        /// Содержит инфо о типе ячейки
        /// </summary>
        public NumberType Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает значение,  записанное в ячейке.
        /// Если значения нет, возвращает 0
        /// </summary>
        public int Value
        {
            get
            {
                return this.value;
            }
            private set
            {
                this.value = value;
                this.UpdateTypeAccordingValue();
            }
        }

        /// <summary>
        /// Отображает позицию ячейки на поле
        /// </summary>
        public Position Coordinates
        {
            get;
            private set;
        }

        /// <summary>
        /// Показывает записанно ли значение в ячейку
        /// </summary>
        public bool HasValue
        {
            get
            {
                switch (this.Type)
                {
                    case NumberType.Unexists:
                    case NumberType.Empty:
                        return false;

                    case NumberType.Modify:
                    case NumberType.Constant:
                        return true;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Показывает может ли пользователь менять значение в ячейке
        /// </summary>
        public bool IsModified
        {
            get
            {
                switch (this.Type)
                {
                    case NumberType.Unexists:
                    case NumberType.Constant:
                        return false;

                    case NumberType.Modify:
                    case NumberType.Empty:
                        return true;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Возвращает, должна ли данная ячейка иметь значение/уже имеет его или данная ячейка не заполняется
        /// Возвращает false, если данная ячейка не должна заполняться (иметь значение)
        /// </summary>
        public bool IsExist
        {
            get
            {
                return !(this.Type == NumberType.Unexists);
            }
        }

        /// <summary>
        /// Создает объект класса по его типу, позиции, значению
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <param name="value"></param>
        public Number(NumberType type, Position position, int value = 0)
        {
            this.Value = value;
            this.Type = (NumberType)type;
            this.Coordinates = position;
            this.parents = new Block[0];
        }

        /// <summary>
        /// Возвращает правильно ли записанно число в ячейке
        /// </summary>
        /// <returns>правильно ли записанно число в ячейке</returns>
        public bool IsRight()
        {
            for (int i = 0; i < parents.Length; i++ )
            {
                Number[] wrongNumber = parents[i].Check();
                for (int j = 0; j < wrongNumber.Length; j++)
                {
                    if (this.IsSame(wrongNumber[j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Возвращает совпадают ли данные координаты с координатами этой ячейки
        /// </summary>
        /// <param name="position">данные координаты</param>
        /// <returns>совпадают ли данные координаты с координатами этой ячейки</returns>
        public bool IsSame(Position position)
        {
            return this.Coordinates.IsSame(position);
        }

        /// <summary>
        /// Возвращает совпадают ли данное число с this
        /// Совпадение чисел происходит, если их позиции совпадают
        /// </summary>
        /// <param name="number">данное число</param>
        /// <returns>совпадают ли данное число с этим</returns>
        public bool IsSame(Number number)
        {
            return this.IsSame(number.Coordinates);
        }

        /// <summary>
        /// Ставит тип ячейки согласно заданному значению
        /// </summary>
        private void UpdateTypeAccordingValue()
        {
            switch(this.Value)
            {
                case 0 :
                    this.Type = NumberType.Empty;
                    break;
                default :
                    this.Type = NumberType.Modify;
                    break;
            }
        }

        /// <summary>
        /// Изменяет, если возможно, значение ячейки
        /// Возвращает успех операции
        /// </summary>
        /// <param name="new_value">новое значение ячейки</param>
        /// <returns></returns>
        public bool Modify(int new_value)
        {
            if (new_value == 0)
            {
                return this.Clear();
            }

            if (IsModified)
            {
                this.Value = new_value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Удаляет какое либо значение из ячейки, если возможно
        /// Возвращает успех операции
        /// </summary>
        /// <returns></returns>
        public bool Clear()
        {
            if (this.IsModified)
            {
                this.Value = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавляет ссылку на блок, которому принадлежит ячейка
        /// Возвращает успех операции
        /// </summary>
        /// <param name="new_parent">новый родительский блок</param>
        public bool AddParent(Block newParent)
        {
            Array.Resize(
                ref this.parents, 
                this.parents.Length + 1
            );
            this.parents[this.parents.Length - 1] = newParent;
            return true;
        }

        public string NameXml
        {
            get { return Constant.Xml.NumberTag; }
        }

        public bool LoadXml(System.Xml.Linq.XElement element)
        {
            Alist.Xml.Tag tag = new Alist.Xml.Tag();
            tag.LoadXml(element);
            this.Coordinates = new Position();
            this.Coordinates.LoadXml
                (tag.GetChild(this.Coordinates.NameXml));
            string type = tag.GetAttribute
                (Constant.Xml.NumberTypeAttribute, 
                NumberType.Unexists.ToString());
            type = Alist.Method.ToLowerFirstUpper(type);
            NumberType numberType = NumberType.Unexists;
            bool success = NumberType.TryParse(type, out numberType);
            this.Type = success ? numberType : NumberType.Unexists;
            return true;
        }

        public System.Xml.Linq.XElement UnloadXml()
        {
            System.Xml.Linq.XElement position 
                = this.Coordinates.UnloadXml();
            Alist.Xml.Tag tag = new Alist.Xml.Tag(
                name: this.NameXml,
                value: this.Value.ToString(),
                attribute: new Dictionary<string, string>
                {
                    {
                        Constant.Xml.NumberTypeAttribute,
                        this.Type.ToString()
                    }
                },
                child: new List<System.Xml.Linq.XElement>
                {
                    position
                }
                );
            return tag.UnloadXml();
        }

    }

}