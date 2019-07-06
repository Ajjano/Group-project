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

namespace ConsultApp.ForAdmin
{
    /// <summary>
    /// Логика взаимодействия для AdminAccount.xaml
    /// </summary>
    public partial class AdminAccount : Page
    {
        public AdminAccount()
        {
            InitializeComponent();
        }

        private void BtnRoom_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            GridCursor.Margin = new Thickness(10 + (150 * index), 0, 0, 0);

            switch (index)
            {
                case 0:
                    NewdataGrid.Children.Add(new Rooms());
                    break;
                case 1:
                    NewdataGrid.Children.Add(new Teachers());
                    break;
                default:
                    break;
            }
        }

        private void BtnConsult_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnTeachers_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
