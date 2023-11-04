using DuoVia.FuzzyStrings;
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
    /// Логика взаимодействия для RealtyPage.xaml
    /// </summary>
    public partial class RealtyPage : Page
    {
        public RealtyPage()
        {
            InitializeComponent();
            DGridRealty.ItemsSource = nedvizhdbEntities.GetContext().Realties.ToList();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditRealPage(null));
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditRealPage((sender as Button).DataContext as Realty));
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var realForRemoving = DGridRealty.SelectedItems.Cast<Realty>().Select(real => real.Id).ToList();
            var dbContext = nedvizhdbEntities.GetContext();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {realForRemoving.Count()} элементов?", "Внимание",
              MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var idsNotRemovable = realForRemoving
                    .Where(id => dbContext.Supplies.Any(s => s.RealtyId == id))
                    .ToList();
                    var idsToRemove = realForRemoving.Except(idsNotRemovable).ToList();
                    foreach (var id in idsToRemove)
                    {
                        var reToRemove = dbContext.Realties.Find(id);
                        dbContext.Realties.Remove(reToRemove);
                        MessageBox.Show("Данные удалены");
                    }

                    if (idsNotRemovable.Any())
                    {
                        MessageBox.Show($"Следующие элементы содержатся в таблице Supplies (Предложения) и не могут быть удалены: " +
                            $"{string.Join(", ", idsNotRemovable)}");
                    }
                    dbContext.SaveChanges();
                    DGridRealty.ItemsSource = dbContext.Realties.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                nedvizhdbEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridRealty.ItemsSource = nedvizhdbEntities.GetContext().Realties.ToList();
            }
        }
        private void findTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = findTb.Text;
            var allReal = nedvizhdbEntities.GetContext().Realties.ToList();
            var foundReal = allReal.Where(ag =>
            searchText.LevenshteinDistance(ag.Address_City) <= 3 ||
            searchText.LevenshteinDistance(ag.Address_Street) <= 3).ToList();
            DGridRealty.ItemsSource = foundReal.ToList();
        }
    }
}
