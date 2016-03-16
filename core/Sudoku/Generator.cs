using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{

    /// <summary>
    /// Содержит методы для генерации судоку
    /// </summary>
    public class Generator
    {

        /// <summary>
        /// Коэффициент попыток по умолчанию
        /// </summary>
        public const int AttemptsNumberDefault = 200;

        /// <summary>
        /// Заполненость судоку по умолчанию
        /// </summary>
        public const double FillnessDefault = 0.27;

        /// <summary>
        /// Рандомайзер
        /// </summary>
        private Random random;

        /// <summary>
        /// Ссылка на объект, в который будет генерироваться судоку
        /// </summary>
        private Sudoku sudoku;

        /// <summary>
        /// Коэффициент попыток. Чем он больше, тем дольше генератор будет пытаться сгенерировать судоку
        /// В зависимости от этого коэффициента будет разное количество попыток для разных судоку
        /// </summary>
        public int AttemptsRemain
        {
            get;
            private set;
        }

        /// <summary>
        /// Коэффициент заполнености судоку
        /// Значение от 0 до 1, где 0 - ни одного числа, 0,2 - 20% чисел заполнены, 1 - все числа заполнены
        /// </summary>
        public double Fillness
        {
            get;
            private set;
        }

        private int AttemptsCounter = 0;

        private void AttemptWaisted()
        {
            this.AttemptsRemain--;
            this.AttemptsCounter++;
            if ((this.AttemptsCounter % 1000) == 0)
                Console.WriteLine("Did: {0} attempts", this.AttemptsCounter);
        }

        /// <summary>
        /// Создает объект, задавая шаблон судоку, коэффициент попыток и заполненость
        /// </summary>
        /// <param name="sudoku"></param>
        /// <param name="attemptsNumber"></param>
        /// <param name="fillness"></param>
        public Generator(Sudoku sudoku, int attemptsNumber = AttemptsNumberDefault, double fillness = FillnessDefault)
        {
            this.random = new Random();
            this.sudoku = sudoku;
            ///NNBB temp Debug;
            //((IStringify)(this.sudoku)).Unstringify((((IStringify)(this.sudoku)).Stringify(Stringify_Help.CopyList(Stringify_Help.SeparatorListDefault))), Stringify_Help.CopyList(Stringify_Help.SeparatorListDefault));
            this.AttemptsRemain = this.GetAttempts(attemptsNumber);
            this.Fillness = fillness;
        }

        /// <summary>
        /// Возвращает случайное число из колекции, удаляя его
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int ReturnRandomFromArray(List<int> number)
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
                cell.Modify(ReturnRandomFromArray(number));
            } while (!cell.IsRight() && (number.Count != 0));
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
                bool success = FillCell(sudokuToFill.Number[i], sudokuToFill.MaxValue);
                if (!success)
                {
                    return false;
                }
            }
            return true;
        }

        private bool FillSudokuBlock(Block block)
        {
            for (int i = 0; i < block.Children.Count; i++)
            {
                Number child = block.Children[i];
                if (!child.HasValue)
                {
                    bool success = FillCell(block.Children[i], sudoku.MaxValue);
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
            bool IsForever = (this.AttemptsRemain == 0);
            for (; (this.AttemptsRemain > 0) || IsForever; )
            {
                ///bool success = this.FillSudokuOneAttempt();
                bool success = this.FillSudokuOneAttemptBlock();
                if (success)
                {
                    return true;
                }
                else
                {
                    if (this.AttemptsRemain != 0)
                        this.sudoku.Clear();
                }
            }
            return false;
        }
        
        private bool CompleteSudoku()
        {
            bool IsForever = (this.AttemptsRemain == 0);
            for (; (this.AttemptsRemain > 0) || IsForever; )
            {
                Sudoku sudokuToFill = this.sudoku.Copy();
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
            int numberCount = this.sudoku.GetNumberCount();
            int numberStay = (int)(numberCount * this.Fillness);
            return numberCount - numberStay;
        }

        /// <summary>
        /// Генерирует судоку, согласно параметрам, заложенным в объект
        /// </summary>
        /// <returns></returns>
        public bool Generate()
        {
            bool success = this.FillSudoku();
            int numberDelete = this.CountNumbersToDelete();
            this.sudoku.DeleteNumbers(numberDelete);
            return success;
        }

        /// <summary>
        /// Возвращает колличество попыток, которые будет делать программа, чтобы заполнить судоку
        /// </summary>
        /// <param name="attemptsNumber"></param>
        /// <returns></returns>
        public int GetAttempts(int attemptsNumber = AttemptsNumberDefault)
        {
            return sudoku.Size.X * sudoku.Size.Y * attemptsNumber;
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
                int fillIndex = ReturnRandomFromArray(index);
                result[j] = array[fillIndex];
            }
            
            return result;
        }

    }
}
