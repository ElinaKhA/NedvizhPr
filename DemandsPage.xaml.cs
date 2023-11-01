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
    /// Логика взаимодействия для DemandsPage.xaml
    /// </summary>
    public partial class DemandsPage : Page
    {
        public DemandsPage()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                nedvizhdbEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridDemands.ItemsSource = nedvizhdbEntities.GetContext().Demands.ToList();
            }
        }
    }
}
