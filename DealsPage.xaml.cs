using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для DealsPage.xaml
    /// </summary>
    public partial class DealsPage : Page
    {
        public DealsPage()
        {
            InitializeComponent();
        }
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditDealsPage(null));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var ids = DGridDeals.SelectedItems.Cast<Deal>().Select(d => d.Id).ToList();
            var supForRemoving = nedvizhdbEntities.GetContext().Deals.Where(c => ids.Contains(c.Id)).ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {supForRemoving.Count()} элементов?", "Внимание",
              MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    nedvizhdbEntities.GetContext().Deals.RemoveRange(supForRemoving);
                    nedvizhdbEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                    DGridDeals.ItemsSource = nedvizhdbEntities.GetContext().Deals.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditDealsPage((sender as Button).DataContext as Deal));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                nedvizhdbEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridDeals.ItemsSource = nedvizhdbEntities.GetContext().Deals.ToList();
            }
        }
    }
}
