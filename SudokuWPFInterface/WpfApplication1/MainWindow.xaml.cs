using System;
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
namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<MyTextBox> textBox = new List<MyTextBox>() { }; //коллекция коробок

        List<int> AdmissibleValues = new List<int> { }; // Содерждит колекцию допустимых значений


        public MainWindow()
        {
            InitializeComponent();
        }

        public void ColumnAdd(int column)
        {
            for (int i = 0; i < column; i++)
            {
                ColumnDefinition columnd = new ColumnDefinition();
                sudokuGrid.ColumnDefinitions.Add(columnd);
            }
        }

        public void RowAdd(int row)
        {
            for (int i = 0; i < row; i++) // деление сетки на строки и столбцы
            {
                RowDefinition rowd = new RowDefinition();
                sudokuGrid.RowDefinitions.Add(rowd);
            }
        }

        private MyTextBox TextBoxAdd(int row, int column)
        {
            MyTextBox tb = new MyTextBox();
            tb.row = row; tb.column = column;
            tb.Name = "_" + Convert.ToString(row) + "_" + Convert.ToString(column);
            tb.TextAlignment = TextAlignment.Center;
            tb.FontSize = 30;
            tb.BorderBrush = new SolidColorBrush(Colors.Black);
            tb.Margin = new Thickness(5, 5, 5, 5);
            tb.Text = "";
            tb.TextChanged += textChangedEventHandler;
            tb.LostFocus += lostFocus;
            sudokuGrid.Children.Add(tb);
            Grid.SetColumn(tb, column);
            Grid.SetRow(tb, row);
            return tb;
        }

        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            MyTextBox myTextBox = (MyTextBox)sender;
            for (int i = 0; i < myTextBox.Text.Length; i++)
            {
                if (myTextBox.Text[i] < '1' || myTextBox.Text[i] > '9')
                {
                    myTextBox.Text = myTextBox.PastText;
                    return;
                }

            }


        }

        private void lostFocus(object sender, RoutedEventArgs e)
        {
            MyTextBox myTextBox = (MyTextBox)sender;

            for(int i = 0; i < myTextBox.Text.Length; i++)
            {
                if (myTextBox.Text[i] < '1' || myTextBox.Text[i] > '9')
                {
                    myTextBox.Text = myTextBox.PastText;
                    return;
                }
            }
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
        }

        private void _9x9_Click(object sender, RoutedEventArgs e)
        {

            sudokuGrid.Children.Clear();
            sudokuGrid.ColumnDefinitions.Clear();
            sudokuGrid.RowDefinitions.Clear();
            
            ColumnAdd(9);
            RowAdd(9);

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
                    textBox.Add(TextBoxAdd(i, j));
                }
            }
            ///////////////////////////////////////////////////////////









        }

        private void _16x16_Click(object sender, RoutedEventArgs e)
        {
            sudokuGrid.Children.Clear();
            sudokuGrid.ColumnDefinitions.Clear();
            sudokuGrid.RowDefinitions.Clear();

            ColumnAdd(16);
            RowAdd(16);

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
                    textBox.Add(TextBoxAdd(i, j));
                }
            }
            ///////////////////////////////////////////////////////////////////////










        }

    }
 }
