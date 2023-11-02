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
    /// Логика взаимодействия для AddEditDemandPage.xaml
    /// </summary>
    public partial class AddEditDemandPage : Page
    {
        private Demand _curDem = new Demand();
        int old = 0;
        public AddEditDemandPage(Demand selectedDem)
        {
            InitializeComponent();
            if (selectedDem != null)
            {
                _curDem = selectedDem;
                old = 1;
            }
            DataContext = _curDem;
            cbClients.ItemsSource = nedvizhdbEntities.GetContext().Clients.ToList();
            cbAgents.ItemsSource = nedvizhdbEntities.GetContext().Agents.ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder err = new StringBuilder();
                if (cbClients.SelectedIndex == -1)
                    err.AppendLine("Укажите клиента");
                if (cbAgents.SelectedIndex == -1)
                    err.AppendLine("Укажите риэлтора");
                if (tb1.Text.All(char.IsDigit) == false || tb2.Text.All(char.IsDigit) == false || 
                    tb3.Text.All(char.IsDigit) == false || tb4.Text.All(char.IsDigit) == false || 
                    tb5.Text.All(char.IsDigit) == false || tb6.Text.All(char.IsDigit) == false ||
                    tb7.Text.All(char.IsDigit) == false || tb8.Text.All(char.IsDigit) == false || 
                    tb9.Text.All(char.IsDigit) == false || tb10.Text.All(char.IsDigit) == false || 
                    tb11.Text.All(char.IsDigit) == false || tb12.Text.All(char.IsDigit) == false)
                    err.AppendLine("Все поля, кроме адреса должны быть числом");
                if (err.Length > 0)
                {
                    MessageBox.Show(err.ToString());
                    return;
                }
                if (old == 0)
                {
                    var lastid = nedvizhdbEntities.GetContext().Demands.ToList().Last().Id;
                    int nid = lastid + 1;
                    _curDem.Id = nid;
                }
                if (old == 1)
                {
                    Demand dem = nedvizhdbEntities.GetContext().Demands
                        .Where(o => o.Id == _curDem.Id)
                        .FirstOrDefault();
                    nedvizhdbEntities.GetContext().Demands.Remove(dem);
                    nedvizhdbEntities.GetContext().SaveChanges();
                }

                nedvizhdbEntities.GetContext().Demands.Add(_curDem);

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
            catch { MessageBox.Show("Ошибка сервера"); }
            
        }
    }
}
