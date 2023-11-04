using DuoVia.FuzzyStrings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
        }
        public class ClientData
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int Demand { get; set; }
            public int Supply { get; set; }
        }
        public List<ClientData> GetAllClientsData(nedvizhdbEntities nedvizhdbEntities)
        {
            var clients = nedvizhdbEntities.Clients.ToList();
            var demands = nedvizhdbEntities.Demands.ToList();
            var supplies = nedvizhdbEntities.Supplies.ToList();

            var allClients = clients.Select(cl => new ClientData
            {
                Id = cl.Id,
                FirstName = cl.FirstName,
                MiddleName = cl.MiddleName,
                LastName = cl.LastName,
                Phone = cl.Phone,
                Email = cl.Email,
                Demand = demands
                    .Where(d => d.ClientId == cl.Id)
                    .Select(d => d.Id)
                    .FirstOrDefault(),
                Supply = supplies
                    .Where(s => s.ClientId == cl.Id)
                    .Select(s => s.Id)
                    .FirstOrDefault()
            }).ToList();
            return allClients;
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedClient = DGridClients.SelectedItem as ClientData;
            int clientId = selectedClient.Id;
            Client client = nedvizhdbEntities.GetContext().Clients.FirstOrDefault(c => c.Id == clientId); ;
            Manager.MainFrame.Navigate(new AddEditPage(client as Client));
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var ids = DGridClients.SelectedItems.Cast<ClientData>().Select(client => client.Id).ToList();
            var clientsToRemove = nedvizhdbEntities.GetContext().Clients.Where(client => ids.Contains(client.Id)).ToList();
            var dbContext = nedvizhdbEntities.GetContext();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {clientsToRemove.Count()} элементов?", "Внимание", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var idsDNotRemovable = ids
                    .Where(id => dbContext.Demands.Any(d => d.ClientId == id))
                    .ToList();
                    var idsSNotRemovable = ids
                    .Where(id => dbContext.Supplies.Any(deal => deal.ClientId == id))
                    .ToList();
                    var idsToRemove = ids.Except(idsDNotRemovable).Except(idsSNotRemovable).ToList();
                    foreach (var id in idsToRemove)
                    {
                        var suplToRemove = dbContext.Clients.Find(id);
                        dbContext.Clients.Remove(suplToRemove);
                        dbContext.SaveChanges();
                        MessageBox.Show("Данные удалены");
                    }

                    if (idsDNotRemovable.Any())
                    {
                        MessageBox.Show($"Следующие элементы содержатся в таблице Demands (Потребности) и не могут быть удалены: {string.Join(", ", idsDNotRemovable)}");
                    }
                    if (idsSNotRemovable.Any())
                    {
                        MessageBox.Show($"Следующие элементы содержатся в таблице Supplies (Предложения) и не могут быть удалены: {string.Join(", ", idsSNotRemovable)}");
                    }
                    DGridClients.ItemsSource = GetAllClientsData(nedvizhdbEntities.GetContext()).ToList();
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
                DGridClients.ItemsSource = GetAllClientsData(nedvizhdbEntities.GetContext()).ToList();
            }
        }
        private void findTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = findTb.Text;
            var allClients = GetAllClientsData(nedvizhdbEntities.GetContext()).ToList();
            var foundClients = allClients.Where(client =>
            searchText.LevenshteinDistance(client.FirstName) <= 3 ||
            searchText.LevenshteinDistance(client.MiddleName) <= 3 ||
            searchText.LevenshteinDistance(client.LastName) <= 3).ToList();
            DGridClients.ItemsSource = foundClients.ToList();
        }
    }
}
