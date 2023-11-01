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
            StringBuilder err = new StringBuilder();

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
    }
}
