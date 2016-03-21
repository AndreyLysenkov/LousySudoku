using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alist;

namespace LousySudoku
{

    /// <summary>
    /// Class for sudoku generation
    /// </summary>
    public class Generator
        : IXmlize
    {

        /// <summary>
        /// Attempts count on default
        /// </summary>
        public const int AttemptsCountDefault = 2000;

        /// <summary>
        /// Sudoku fillness after generation on default
        /// </summary>
        public const double FillnessDefault = 0.27;

        /// <summary>
        /// Randomizer
        /// </summary>
        private Random random;

        /// <summary>
        /// Sudoku object in wich will be generated sudoku
        /// </summary>
        private Sudoku sudoku;

        private int attemptsRemain;

        /// <summary>
        /// Sudoku fillness after generation.
        /// Value [0..1], where 0,2, for exanple, 
        /// - means 20% of sudoku number are known
        /// </summary>
        public double Fillness
        {
            get;
            private set;
        }

        public int AttemptsCount
        {
            get;
            private set;
        }

        private void AttemptWaisted()
        {
            this.attemptsRemain--;
            this.AttemptsCount++;
#if DEBUG
            if ((this.AttemptsCount % 1000) == 0)
            {
                Console.WriteLine("Did: {0} attempts", this.AttemptsCount);
                Debug.Print.Sudoku2D(this.sudoku);
            }
#endif
        }

        /// <summary>
        /// Create new generator, wich will be generate sudoku
        /// </summary>
        /// <param name="sudoku"></param>
        /// <param name="attemptsNumber"></param>
        /// <param name="fillness"></param>
        public Generator(Sudoku sudoku,
            int attemptsNumber = AttemptsCountDefault, 
            double fillness = FillnessDefault)
        {
            this.random = new Random();
            this.sudoku = sudoku;
            this.attemptsRemain = attemptsNumber;
            this.Fillness = fillness;
        }

        /// <summary>
        /// Возвращает случайное число из колекции, удаляя его
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int ReturnRandomFromArray(ref List<int> number)
        {
            if (number.Count == 0)
            {
                return -1;
            }
            int index = random.Next(number.Count);
            int result = number[index];
            number.RemoveAt(index);
            return result;
        }

        /// <summary>
        /// Пытается заполнить ячейку судоку случайным числом, пока оно не будет правильным
        /// </summary>
        /// <param name="cell">Заполняемая ячейка</param>
        /// <param name="maxValue">Максимально возможное значение</param>
        /// <returns>Успех попытки</returns>
        private bool FillCell(Number cell, int maxValue)
        {
            List<int> number = new List<int> {};
            for (int i = 1; i <= maxValue; i++)
            {
                number.Add(i);
            }
            do
            {
                cell.Modify(ReturnRandomFromArray(ref number));
            } while (!cell.IsBlockRight() && (number.Count != 0));
            return cell.IsRight();
        }

        /// <summary>
        /// Пытается заполнить судоку
        /// </summary>
        /// <returns>Успех попытки</returns>
        private bool FillSudokuOneAttempt(Sudoku sudokuToFill)
        {
            this.AttemptWaisted();
            for (int i = 0; i < sudokuToFill.Number.Count; i++)
            {
                bool success
                    = FillCell(sudokuToFill.Number[i], sudokuToFill.MaxValue);
                if (!success)
                {
                    return false;
                }
            }
            return true;
        }

        private bool FillSudokuBlock(Block block)
        {
            for (int i = 0; i < block.Child.Count; i++)
            {
                Number child = block.Child[i];
                if (!child.HasValue)
                {
                    bool success = FillCell(block.Child[i], sudoku.MaxValue);
                    if (!success)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool FillSudokuOneAttemptBlock()
        {
            this.AttemptWaisted();
            for (int i = 0; i < this.sudoku.Block.Count; i++)
            {
                bool success = FillSudokuBlock(this.sudoku.Block[i]);
                if (!success)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Пытается заполнить судоку столько, сколько указано в AttemptsRemain
        /// </summary>
        /// <returns>Успех генерации судоку</returns>
        private bool FillSudoku()
        {
            bool IsForever = (this.attemptsRemain == 0);
            for (; (this.attemptsRemain > 0) || IsForever; )
            {
                ///bool success = this.FillSudokuOneAttempt();
                bool success = this.FillSudokuOneAttemptBlock();
                if (success)
                {
                    return true;
                }
                else
                {
                    if (this.attemptsRemain != 0)
                        this.sudoku.Clear();
                }
            }
            return false;
        }
        
        private bool CompleteSudoku()
        {
            bool IsForever = (this.attemptsRemain == 0);
            for (; (this.attemptsRemain > 0) || IsForever; )
            {
                Sudoku sudokuToFill = (Sudoku)this.sudoku.Clone();
                bool success = this.FillSudokuOneAttempt(sudokuToFill);
                if (success)
                {
                    this.sudoku = sudokuToFill;
                    return true;
                }
            }
            return false;
        }

        public bool Complete()
        {
            bool success = this.CompleteSudoku();
            return success;
        }

        /// <summary>
        /// Возвращает сколько чисел из судоку надо удалить, чтобы заполненасть равнялась Fillness
        /// </summary>
        /// <returns></returns>
        private int CountNumbersToDelete()
        {
            //int numberCount = this.sudoku.GetNumberCount();
            //int numberStay = (int)(numberCount * this.Fillness);
            //return numberCount - numberStay;
            return 0;
        }

        /// <summary>
        /// Генерирует судоку, согласно параметрам, заложенным в объект
        /// </summary>
        /// <returns></returns>
        public bool Generate()
        {
            bool success = this.FillSudoku();
            int numberDelete = this.CountNumbersToDelete();
            //this.sudoku.DeleteNumbers(numberDelete);
            return success;
        }
        
        /// <summary>
        /// Перемешивает элементы массива
        /// </summary>
        /// <typeparam name="item"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public item[] MixItems<item>(item[] array)
        {
            int length = array.Length;
            item[] result = new item[length];
            List<int> index = new List<int> { };
            for (int i = 0; i < length; i++)
            {
                index.Add(i);
            }

            for (int j = 0; j < length; j++ )
            {
                int fillIndex = ReturnRandomFromArray(ref index);
                result[j] = array[fillIndex];
            }
            
            return result;
        }

        public string NameXml
        {
            // NNBB; todo;
            get;
        }

        public bool LoadXml(System.Xml.Linq.XElement element)
        {
            // NNBB; todo;
            return true;
        }

        public System.Xml.Linq.XElement UnloadXml()
        {
            // NNBB; todo;
            return null;
        }

    }

}