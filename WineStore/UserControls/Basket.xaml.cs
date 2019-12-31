using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Shapes;

namespace WineStore.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Deliveries.xaml
    /// </summary>
    public partial class Basket : UserControl
    {

        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

     public MainWindow MainWindow1 { get; set; }

        Int32 count = 0;
        Decimal summ = 0;
        public Basket(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow1 = mainWindow;
            SelectAllWinesToBasket();
          
        }

        public void SelectAllWinesToBasket()
        {
            string sqlExpression = "sp_SelectALLWinesToBasket";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    btnCheckOut.IsEnabled = true;
                    ListofWines.Children.Clear();
                    while (reader.Read()) // построчно считываем данные
                    {
                        count++;
                        String product_code = reader.GetString(0);
                        String title = reader.GetString(1);
                        Int32 volume = reader.GetInt32(2);
                        Decimal price = reader.GetDecimal(3);
                        summ += price;
                        //Int32 available = reader.GetInt32(6);
                        String foto = reader.GetString(4);

                        ListofWines.Children.Add(new UserControls.BasketProduct(this, product_code, title, volume, price, foto));


                    }
                }
                else
                {
                    MessageBox.Show("Cart is empty");
                    btnCheckOut.IsEnabled = false;
                }


                reader.Close();


            }
        }

        private void BtnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(Main);
            StackMain.Children.Add(new UserControls.OrderInformation(this, count,(int)summ));
        }
    }
}
