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
            var supForRemoving = DGridSupply.SelectedItems.Cast<Supply>().Select(sup => sup.Id).ToList();
            var dbContext = nedvizhdbEntities.GetContext();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {supForRemoving.Count()} элементов?", "Внимание",
              MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var idsNotRemovable = supForRemoving
                    .Where(id => dbContext.Deals.Any(deal => deal.SupplyId == id))
                    .ToList();
                    var idsToRemove = supForRemoving.Except(idsNotRemovable).ToList();
                    foreach (var id in idsToRemove)
                    {
                        var suplToRemove = dbContext.Supplies.Find(id);
                        dbContext.Supplies.Remove(suplToRemove);
                        MessageBox.Show("Данные удалены");
                    }

                    if (idsNotRemovable.Any())
                    {
                        MessageBox.Show($"Следующие элементы содержатся в таблице Deals и не могут быть удалены: {string.Join(", ", idsNotRemovable)}");
                    }
                    dbContext.SaveChanges();

                    DGridSupply.ItemsSource = dbContext.Supplies.ToList();
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
