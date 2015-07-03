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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для NewGame.xaml
    /// </summary>
    public partial class NewGame : Window
    {
        public MainWindow parent = new MainWindow();
        public NewGame()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (ComboBoxSize.Text == "")
                MessageBox.Show("Вы не указали размер!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                switch (ComboBoxSize.Text)
                {
                    case "9x9":
                        parent.CreateGrid_9x9();
                        break;
                    case "16x16":
                        parent.CreateGrid_16x16();
                        break;

                }
                this.Close();
            }
        }

    }
}
