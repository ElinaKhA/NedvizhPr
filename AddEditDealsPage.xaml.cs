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
    /// Логика взаимодействия для AddEditDealsPage.xaml
    /// </summary>
    public partial class AddEditDealsPage : Page
    {
        private Deal _curDeal = new Deal();
        int old = 0;
        public AddEditDealsPage(Deal selectedDeal)
        {
            InitializeComponent();
            if (selectedDeal != null)
            {
                _curDeal = selectedDeal;
                old = 1;
            }
            DataContext = _curDeal;
            cbDem.ItemsSource = nedvizhdbEntities.GetContext().Demands.ToList();
            cbSup.ItemsSource = nedvizhdbEntities.GetContext().Supplies.ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                StringBuilder err = new StringBuilder();
                if (cbDem.SelectedIndex == -1)
                    err.AppendLine("Укажите Потребность");
                if (cbSup.SelectedIndex == -1)
                    err.AppendLine("Укажите Предложение");
                if (err.Length > 0)
                {
                    MessageBox.Show(err.ToString());
                    return;
                }
            if (old == 1)
            {
                Deal d = nedvizhdbEntities.GetContext().Deals
                    .Where(o => o.Id == _curDeal.Id)
                    .FirstOrDefault();
                nedvizhdbEntities.GetContext().Deals.Remove(d);
                nedvizhdbEntities.GetContext().SaveChanges();
                old = 0;
            }

                int idD = _curDeal.DemandId;
                //int idD = Convert.ToInt32(cbDem.SelectedValue);
                bool existsD = nedvizhdbEntities.GetContext().Deals.Any(d => d.DemandId == idD);
                int idS = _curDeal.SupplyId;

            //  int idS = Convert.ToInt32(cbSup.SelectedValue);
                bool existsS = nedvizhdbEntities.GetContext().Deals.Any(d => d.SupplyId == idS);
                if (existsD)
                {
                    MessageBox.Show($"Данная потребность уже удовлетворена, т.к. участвует в сделке, поэтому не может быть применена");
                }
                else if (existsS)
                {
                    MessageBox.Show($"Данное предложение уже удовлетворено, т.к. участвует в сделке, поэтому не может быть применено");
                }
                else
                {
                    try
                    {
                        nedvizhdbEntities.GetContext().Deals.Add(_curDeal);
                        nedvizhdbEntities.GetContext().SaveChanges();
                        MessageBox.Show("Информация сохранена");
                        Manager.MainFrame.GoBack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            //}
            //catch { MessageBox.Show("Ошибка сервера"); }
        }
    }
}
