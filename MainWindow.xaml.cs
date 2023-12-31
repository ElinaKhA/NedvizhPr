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
            Manager.MainFrame = MainFrame;
        }

        private void ClientsBtn_Click(object sender, RoutedEventArgs e)
        {
            HeadLb.Content = "Работа с клиентами";
            Manager.MainFrame.Navigate(new ClientsPage());
        }

        private void AgentsBtn_Click(object sender, RoutedEventArgs e)
        {
            HeadLb.Content = "Работа с риэлторами";
            Manager.MainFrame.Navigate(new AgentsPage());
        }

        private void RealtyBtn_Click(object sender, RoutedEventArgs e)
        {
            HeadLb.Content = "Работа с недвижимостью";
            Manager.MainFrame.Navigate(new RealtyPage());
        }

        private void SupplyBtn_Click(object sender, RoutedEventArgs e)
        {
            HeadLb.Content = "Работа с предложениями";
            Manager.MainFrame.Navigate(new SupplyPage());
        }

        private void DemandsBtn_Click(object sender, RoutedEventArgs e)
        {
            HeadLb.Content = "Работа с потребностями";
            Manager.MainFrame.Navigate(new DemandsPage());
        }

        private void DealsBtn_Click(object sender, RoutedEventArgs e)
        {
            HeadLb.Content = "Работа со сделками";
            Manager.MainFrame.Navigate(new DealsPage());
        }
    }
}
