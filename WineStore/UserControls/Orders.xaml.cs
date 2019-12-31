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
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        public Orders(My_account my_account)
        {
            InitializeComponent();
            SelectAllOrders();
                    
        }

        private void SelectAllOrders()
        {
            string sqlExpression = "sp_SelectALLOrders";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                List<string> orders = new List<string> { };
                if (reader.HasRows) // если есть данные
                {
                   
                    TextBlock textBlock = new TextBlock();
                    MainList.Items.Clear();
                    while (reader.Read()) // построчно считываем данные
                    {
                        DateTime Date = reader.GetDateTime(0);
                        Int32 ProductCount = reader.GetInt32(1);
                        Int32 Price = reader.GetInt32(2);
                        orders.Add(" Date and time of order: " + Date.ToString() + ", quantity of goods ordered: " + ProductCount.ToString() + ", the entire order: £" + Price.ToString() + "  ");
                        textBlock.Text = "  " + Date.ToString() + "  " + ProductCount.ToString() + "  " + Price.ToString() + "  ";


                    }
                }
                else
                {
                    MessageBox.Show("No data yet");
                }

                MainList.ItemsSource = orders;


                reader.Close();


            }
        }
    }
}
