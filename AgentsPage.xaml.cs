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
using static NedvizhPr.ClientsPage;

namespace NedvizhPr
{
    /// <summary>
    /// Логика взаимодействия для AgentsPage.xaml
    /// </summary>
    public partial class AgentsPage : Page
    {
        public AgentsPage()
        {
            InitializeComponent();
        }
        public class AgentData
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string DealShare { get; set; }
            public int Demand { get; set; }
            public int Supply { get; set; }
        }


        public List<AgentData> GetAllAgentsData(nedvizhdbEntities nedvizhdbEntities)
        {
            var agents = nedvizhdbEntities.Agents.ToList();
            var demands = nedvizhdbEntities.Demands.ToList();
            var supplies = nedvizhdbEntities.Supplies.ToList();

            var allAgents = agents.Select(ag => new AgentData
            {
                Id = ag.Id,
                FirstName = ag.FirstName,
                MiddleName = ag.MiddleName,
                LastName = ag.LastName,
                DealShare = ag.DealShare.ToString(),
                Demand = demands
                    .Where(d => d.AgentId == ag.Id)
                    .Select(d => d.Id)
                    .FirstOrDefault(),
                Supply = supplies
                    .Where(s => s.AgentId == ag.Id)
                    .Select(s => s.Id)
                    .FirstOrDefault()
            }).ToList();

            return allAgents;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedAgent = DGridAgents.SelectedItem as AgentData;
            int agentId = selectedAgent.Id;
            Agent agent = nedvizhdbEntities.GetContext().Agents.FirstOrDefault(c => c.Id == agentId); ;
            Manager.MainFrame.Navigate(new AddEditAgentsPage(agent as Agent));
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditAgentsPage(null));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var ids = DGridAgents.SelectedItems.Cast<AgentData>().ToList().Select(agent => agent.Id).ToList();
            var agentsToRemove = nedvizhdbEntities.GetContext().Agents.Where(agent => ids.Contains(agent.Id)).ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {agentsToRemove.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    nedvizhdbEntities.GetContext().Agents.RemoveRange(agentsToRemove);
                    nedvizhdbEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                    DGridAgents.ItemsSource = GetAllAgentsData(nedvizhdbEntities.GetContext()).ToList();
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
                DGridAgents.ItemsSource = GetAllAgentsData(nedvizhdbEntities.GetContext()).ToList();
            }
        }
        private void findTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = findTb.Text;
            var allAgents =GetAllAgentsData(nedvizhdbEntities.GetContext()).ToList();
            var foundAgents = allAgents.Where(ag =>
            searchText.LevenshteinDistance(ag.FirstName) <= 3 ||
            searchText.LevenshteinDistance(ag.MiddleName) <= 3 ||
            searchText.LevenshteinDistance(ag.LastName) <= 3).ToList();
            DGridAgents.ItemsSource = foundAgents.ToList();
        }
    }
}
