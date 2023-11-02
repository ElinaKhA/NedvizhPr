using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NedvizhPr
{
    /// <summary>
    /// Логика взаимодействия для AddEditSupplyPage.xaml
    /// </summary>
    public partial class AddEditSupplyPage : Page
    {
        private Supply _curSup = new Supply();
        int old = 0;
        public AddEditSupplyPage(Supply selectedSup)
        {
            InitializeComponent();
            if (selectedSup != null)
            {
                _curSup = selectedSup;
                old = 1;
            }
            DataContext = _curSup;
            cbClients.ItemsSource = nedvizhdbEntities.GetContext().Clients.ToList();
            cbAgents.ItemsSource = nedvizhdbEntities.GetContext().Agents.ToList();
            cbReal.ItemsSource = nedvizhdbEntities.GetContext().Realties.ToList();
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
                if (cbReal.SelectedIndex == -1)
                    err.AppendLine("Укажите недвижимость");
                if (string.IsNullOrEmpty(_curSup.Price.ToString()))
                    err.AppendLine("Укажите цену");
                if (_curSup.Price < 0 || tbPrice.Text.All(char.IsDigit) == false)
                    err.AppendLine("Цена должна быть положительным числом");
                if (err.Length > 0)
                {
                    MessageBox.Show(err.ToString());
                    return;
                }
                if (old == 0)
                {
                    var lastid = nedvizhdbEntities.GetContext().Supplies.ToList().Last().Id;
                    int nid = lastid + 1;
                    _curSup.Id = nid;
                }
                if (old == 1)
                {
                    Supply sup = nedvizhdbEntities.GetContext().Supplies
                        .Where(o => o.Id == _curSup.Id)
                        .FirstOrDefault();
                    nedvizhdbEntities.GetContext().Supplies.Remove(sup);
                    nedvizhdbEntities.GetContext().SaveChanges();
                }

                nedvizhdbEntities.GetContext().Supplies.Add(_curSup);
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
