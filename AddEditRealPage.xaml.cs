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
    /// Логика взаимодействия для AddEditRealPage.xaml
    /// </summary>
    public partial class AddEditRealPage : Page
    {
        private Realty _curReal = new Realty();
        int old = 0;
        public AddEditRealPage(Realty selectedReal)
        {
            InitializeComponent();
            if (selectedReal != null)
            {
                _curReal = selectedReal;
                old = 1;
            }
            DataContext = _curReal;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder err = new StringBuilder();

            if (old == 0)
            {
                var lastid = nedvizhdbEntities.GetContext().Realties.ToList().Last().Id;
                int nid = lastid + 1;
                _curReal.Id = nid;
                _curReal.Coordinate_latitude = 0;
                _curReal.Coordinate_longitude = 0;
            }
            if (old == 1)
            {
                Realty real = nedvizhdbEntities.GetContext().Realties
                    .Where(o => o.Id == _curReal.Id)
                    .FirstOrDefault();
                nedvizhdbEntities.GetContext().Realties.Remove(real);
                nedvizhdbEntities.GetContext().SaveChanges();
            }


            nedvizhdbEntities.GetContext().Realties.Add(_curReal);

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
