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

namespace NedvizhPr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClientsBtn_Click(object sender, RoutedEventArgs e)
        {
            HeadLb.Content = "Работа с клиентами";
            MainFrame.Navigate(new ClientsPage());
        }

        private void AgentsBtn_Click(object sender, RoutedEventArgs e)
        {
            HeadLb.Content = "Работа с риэлторами";
            MainFrame.Navigate(new AgentsPage());
        }

    }
}
