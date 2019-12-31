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

namespace WineStore.UserControls
{
    /// <summary>
    /// Логика взаимодействия для My_account.xaml
    /// </summary>
    public partial class My_account : UserControl
    {
        public My_account(MainWindow mainWindow)
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

           


            switch (index)
            {
                case 0:
                    StackMain.Children.Clear();
                    StackMain.Children.Add(new UserControls.ShippingInformation(this));
                    break;
                case 1:
                    StackMain.Children.Clear();
                    StackMain.Children.Add(new UserControls.PaymentInformation(this));
                    break;
                case 2:
                    StackMain.Children.Clear();
                    StackMain.Children.Add(new UserControls.Orders(this));
                    break;
               

            }
        }
    }
}
