﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Xml.Linq;
using LousySudoku;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<MyTextBox> textBox = new List<MyTextBox>() { }; //коллекция коробок

        List<int> AdmissibleValues = new List<int> { }; // Содерждит колекцию допустимых значений

        Sudoku sudoku = SudokuDebug.GetStandart16(null);

        double complex = 0.27;

        string currenType;

        public MainWindow()
        {
            InitializeComponent();

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private MyTextBox TextBox_9_9Add(int row, int column)
        {
            MyTextBox tb = new MyTextBox();
            tb.row = row; tb.column = column;
            tb.Name = "_" + Convert.ToString(row) + "_" + Convert.ToString(column);
            tb.TextAlignment = TextAlignment.Center;
            tb.FontSize = 45;
            tb.Background = new SolidColorBrush(Colors.Transparent);
            tb.BorderBrush = new SolidColorBrush(Colors.Black);
            tb.BorderThickness = new Thickness(2);

            if ((column + 1) % 3 == 0)
            {
                tb.Margin = new Thickness(0, 0, 10, 0);
            }
            else if ((row + 1) % 3 == 0)
                tb.Margin = new Thickness(0, 0, 0, 10);
            if ((column + 1) % 3 == 0 && ((row + 1) % 3 == 0))
                tb.Margin = new Thickness(0, 0, 10, 10);

            tb.Text = "";
            tb.TextChanged += textChangedEventHandler;
            tb.LostFocus += lostFocus;
            sudokuGrid.Children.Add(tb);

            Grid.SetColumn(tb, column);
            Grid.SetRow(tb, row);

            tb.Text = Interface.GetNumber(sudoku, new Number.Position(row, column)).Value.ToString();


            ///Console.WriteLine(sudoku.ReturnNumberByPosition(new Number.Position(column, row)).Value);

            return tb;
        }

        private MyTextBox TextBox_16_16Add(int row, int column)
        {
            MyTextBox tb = new MyTextBox();
            tb.row = row; tb.column = column;
            tb.Name = "_" + Convert.ToString(row) + "_" + Convert.ToString(column);
            tb.TextAlignment = TextAlignment.Center;
            tb.FontSize = 25;
            tb.Background = new SolidColorBrush(Colors.Transparent);
            tb.BorderBrush = new SolidColorBrush(Colors.Black);
            tb.BorderThickness = new Thickness(2);

            if ((column + 1) % 4 == 0)
            {
                tb.Margin = new Thickness(0, 0, 10, 0);
            }
            else if ((row + 1) % 4 == 0)
                tb.Margin = new Thickness(0, 0, 0, 10);
            if ((column + 1) % 4 == 0 && ((row + 1) % 4 == 0))
                tb.Margin = new Thickness(0, 0, 10, 10);

            tb.Text = "";
            tb.TextChanged += textChangedEventHandler;
            tb.LostFocus += lostFocus;
            sudokuGrid.Children.Add(tb);

            Grid.SetColumn(tb, column);
            Grid.SetRow(tb, row);

            tb.Text = Interface.GetNumber(sudoku, new Number.Position(row, column)).Value.ToString();

            return tb;
        }

        private void UpdateRightness()
        {
            foreach (MyTextBox tb in textBox)
            {
                if (tb.Text != "")
                {
                    if (Interface.GetNumber(sudoku, new Number.Position(tb.row, tb.column)).IsRight())
                    {
                        tb.Background = new SolidColorBrush(Colors.Transparent);
                    }
                    else
                    {
                        tb.Background = new SolidColorBrush(Colors.Red);
                    }
                }
                else
                    tb.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            MyTextBox myTextBox = (MyTextBox)sender;

            int valueText = 0;

            bool success;

            switch (currenType)
            {
                case "9x9":
                    for (int i = 0; i < myTextBox.Text.Length; i++)
                    {
                        if (myTextBox.Text[i] < '1' || myTextBox.Text[i] > '9')
                        {
                            myTextBox.Text = myTextBox.PastText;
                            return;
                        }

                    }
                    ///////////////



                    if (myTextBox.Text != "")
                        valueText = Convert.ToInt32(myTextBox.Text);


                    success = Interface.ChangeNumber(sudoku, new Number.Position(myTextBox.row, myTextBox.column), valueText);

                    UpdateRightness();
                    break;
                case "16x16":
                    for (int i = 0; i < myTextBox.Text.Length; i++)
                    {
                        if (myTextBox.Text[i] < '1' || myTextBox.Text[i] > '9')
                        {
                            myTextBox.Text = myTextBox.PastText;
                            return;
                        }

                    }
                    ///////////////


                    if (myTextBox.Text != "")
                        valueText = Convert.ToInt32(myTextBox.Text);


                    success = sudoku.ChangeNumber(new Number.Position(myTextBox.row, myTextBox.column), valueText);

                    UpdateRightness();
                    break;
            }
        }

        private void lostFocus(object sender, RoutedEventArgs e)
        {
            MyTextBox myTextBox = (MyTextBox)sender;

            if (myTextBox.Text != "")
            {
                bool IsAdmissibleValue = true;
                foreach (int number in AdmissibleValues)
                {
                    if (Convert.ToInt32(myTextBox.Text) != number)
                        IsAdmissibleValue = false;
                    else
                    {
                        IsAdmissibleValue = true;
                        break;
                    }
                }
                if (!IsAdmissibleValue)
                {
                    MessageBox.Show("Введено недопустимое число", "Внимание", MessageBoxButton.OK);
                    myTextBox.Text = myTextBox.PastText;
                    return;
                }

            }

            myTextBox.PastText = myTextBox.Text;
            /////////////////////////////////////////


            UpdateRightness();

            LousySudoku.Debug.ShowSudoku(sudoku, 9);
            LousySudoku.Debug.ShowSudokuRightness(sudoku, 9);
        }

        public void CreateGrid_9x9(bool generate = true)
        {
            if (generate)
                sudoku = SudokuDebug.GetStandart9((new Debug("9x9")).matrix);
            currenType = "9x9";


            mainWindow.Height = 720 + 2 * 10;
            mainWindow.Width = 745 + 2 * 10;

            sudokuGrid.Children.Clear();
            sudokuGrid.Columns = 9;
            sudokuGrid.Rows = 9;

            if (AdmissibleValues.Count != 0)
                AdmissibleValues.Clear();

            for (int i = 1; i < 10; i++)
                AdmissibleValues.Add(i);

            foreach (TextBox tb in textBox)
                sudokuGrid.Children.Remove(tb);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    textBox.Add(TextBox_9_9Add(i, j));
                }
            }

            UpdateRightness();

        }

        public void CreateGrid_16x16(bool generate = true)
        {
            if (generate)
                sudoku = SudokuDebug.GetStandart16((new Debug("16x16")).matrix);
            currenType = "16x16";

            mainWindow.Height = 800 + 2 * 10;
            mainWindow.Width = 825 + 2 * 10;

            sudokuGrid.Children.Clear();

            sudokuGrid.Rows = 16;
            sudokuGrid.Columns = 16;

            if (AdmissibleValues.Count != 0)
                AdmissibleValues.Clear();

            for (int i = 0; i < 16; i++)
                AdmissibleValues.Add(i);

            foreach (TextBox tb in textBox)
                sudokuGrid.Children.Remove(tb);

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    textBox.Add(TextBox_16_16Add(i, j));
                }
            }

            UpdateRightness();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame newGameWindow = new NewGame();
            newGameWindow.parent = this;
            newGameWindow.Owner = this;
            newGameWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            newGameWindow.Show();
        }

        public void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "*";
            dlg.DefaultExt = ".txt";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                sudoku = Interface.LoadSudoku(filename);
            }

            if (sudoku.Size.X == 9)
                CreateGrid_9x9(false);
            else
                CreateGrid_16x16(false);
        }

        public void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "sudoku";
            dlg.DefaultExt = ".txt";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                Interface.SaveSudoku(filename, sudoku);
            }
        }

    }

}