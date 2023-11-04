using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Client _curClient = new Client();
        int old = 0;
        public AddEditPage(Client selectedClient)
        {
            InitializeComponent();
            if(selectedClient != null)
            {
                _curClient = selectedClient;
                old = 1;
            }
            DataContext = _curClient;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder err = new StringBuilder();
                if (string.IsNullOrWhiteSpace(_curClient.Phone) && string.IsNullOrWhiteSpace(_curClient.Email))
                    err.AppendLine("Укажите Телефон или Email");
                if (err.Length > 0)
                {
                    MessageBox.Show(err.ToString());
                    return;
                }
                if (old == 0)
                {
                    var lastid = nedvizhdbEntities.GetContext().Clients.ToList().Last().Id;
                    int nid = lastid + 1;
                    _curClient.Id = nid;
                }
                if (old == 1)
                {
                    Client client = nedvizhdbEntities.GetContext().Clients
                        .Where(o => o.Id == _curClient.Id)
                        .FirstOrDefault();
                    nedvizhdbEntities.GetContext().Clients.Remove(client);
                    nedvizhdbEntities.GetContext().SaveChanges();
                }
                nedvizhdbEntities.GetContext().Clients.Add(_curClient);
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
