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
    /// Логика взаимодействия для AddEditAgentsPage.xaml
    /// </summary>
    public partial class AddEditAgentsPage : Page
    {
        private Agent _curAgent = new Agent();
        int old = 0;

        public AddEditAgentsPage(Agent selectedAgent)
        {
            InitializeComponent();
            if (selectedAgent != null)
            {
                _curAgent = selectedAgent;
                old = 1;
            }
            DataContext = _curAgent;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder err = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_curAgent.FirstName))
                err.AppendLine("Укажите Фамилию");
            if (string.IsNullOrWhiteSpace(_curAgent.MiddleName))
                err.AppendLine("Укажите Имя");
            if (string.IsNullOrWhiteSpace(_curAgent.LastName))
                err.AppendLine("Укажите Отчество");
            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString());
                return;
            }

            if (int.TryParse(_curAgent.DealShare.ToString(), out int dealShareValue) && (dealShareValue >= 0 && dealShareValue <= 100) || string.IsNullOrWhiteSpace(_curAgent.DealShare.ToString()))
            {
                if (old == 0)
                {
                    var lastid = nedvizhdbEntities.GetContext().Agents.ToList().Last().Id;
                    int nid = lastid + 1;
                    _curAgent.Id = nid;
                }
                if (old == 1)
                {
                    Agent ag = nedvizhdbEntities.GetContext().Agents
                        .Where(o => o.Id == _curAgent.Id)
                        .FirstOrDefault();
                    nedvizhdbEntities.GetContext().Agents.Remove(ag);
                    nedvizhdbEntities.GetContext().SaveChanges();
                    if (string.IsNullOrWhiteSpace(_curAgent.DealShare.ToString()))
                    {
                        _curAgent.DealShare = null;
                    }
                }
                nedvizhdbEntities.GetContext().Agents.Add(_curAgent);
                try
                {
                    nedvizhdbEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                err.AppendLine("Процент от сделки должен быть от 0 до 100");
                MessageBox.Show(err.ToString());
            }
        }
    }

}