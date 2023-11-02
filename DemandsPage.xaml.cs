using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            Manager.MainFrame.Navigate(new AddEditDemandPage(null));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var demForRemoving = DGridDemands.SelectedItems.Cast<Demand>().Select(demand => demand.Id).ToList();
            var dbContext = nedvizhdbEntities.GetContext();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {demForRemoving.Count()} элементов?", "Внимание",
              MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                var idsNotRemovable = demForRemoving
                .Where(id => dbContext.Deals.Any(deal => deal.DemandId == id))
                .ToList();
                var idsToRemove = demForRemoving.Except(idsNotRemovable).ToList();
                foreach (var id in idsToRemove)
                {
                    var demandToRemove = dbContext.Demands.Find(id);
                    dbContext.Demands.Remove(demandToRemove);
                    MessageBox.Show("Данные удалены");
                }
               
                if (idsNotRemovable.Any())
                    {
                        MessageBox.Show($"Следующие элементы содержатся в таблице Deals и не могут быть удалены: {string.Join(", ", idsNotRemovable)}");
                    }
                dbContext.SaveChanges();
                
                DGridDemands.ItemsSource = dbContext.Demands.ToList();
                }
                catch (Exception ex)
                {
                MessageBox.Show(ex.Message.ToString());
                }
            }            
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditDemandPage((sender as Button).DataContext as Demand));
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
