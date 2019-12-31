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

namespace WineStore.Admin_page_elements
{
    /// <summary>
    /// Логика взаимодействия для AddWinesAvailable.xaml
    /// </summary>
    public partial class AddWinesAvailable : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        ForAdmin forAdmin;
        public AddWinesAvailable(ForAdmin forAdmin)
        {
            InitializeComponent();
            this.forAdmin = forAdmin;
        }

        private void AlterWine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AlterWinesSP( CodeWines.Text, Int32.Parse(PriceWines.Text), Int32.Parse(AvailableWines.Text)))
                {
                    MessageBox.Show("Wine altered successfully");
                    ForAdmin forAdminN = new ForAdmin();
                    forAdminN.Show();
                    forAdmin.Close();



                }
                else
                {
                    MessageBox.Show("No wine altered");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool AlterWinesSP(string Code, int Price, int Available)
        {
            string sqlExpression = "sp_AlterWines";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (Code == null)
                {
                    MessageBox.Show("specify the Product code parameter");
                    return false;
                }
                SqlParameter CodeParam = new SqlParameter
                {
                    ParameterName = "@Code",
                    Value = Code
                };

                command.Parameters.Add(CodeParam);

                if (Price == 0 && Available == 0)
                {
                    
                    MessageBox.Show("specify the parameters");                     
                    return false;
                }

               

                SqlParameter PriceParam = new SqlParameter
                {
                    ParameterName = "@Price",
                    Value = Price
                };

                command.Parameters.Add(PriceParam);

               
                SqlParameter AvailableParam = new SqlParameter
                {
                    ParameterName = "@Available",
                    Value = Available
                };

                command.Parameters.Add(AvailableParam);

                command.ExecuteNonQuery();

             
                    return true;
              

            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            forAdmin.GridMain.Children.Clear();
            forAdmin.GridMain.Children.Add(forAdmin.start);
        }
    }
}
