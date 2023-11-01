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
    /// Логика взаимодействия для SupplyPage.xaml
    /// </summary>
    public partial class SupplyPage : Page
    {
        public SupplyPage()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditSupplyPage(null));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var supForRemoving = DGridSupply.SelectedItems.Cast<Supply>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {supForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    nedvizhdbEntities.GetContext().Supplies.RemoveRange(supForRemoving);
                    nedvizhdbEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                    DGridSupply.ItemsSource = nedvizhdbEntities.GetContext().Supplies.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditSupplyPage((sender as Button).DataContext as Supply));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                nedvizhdbEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridSupply.ItemsSource = nedvizhdbEntities.GetContext().Supplies.ToList();
            }
        }
    }
}
