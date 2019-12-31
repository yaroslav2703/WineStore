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
    /// Логика взаимодействия для ShippingInformation.xaml
    /// </summary>
    public partial class ShippingInformation : UserControl
    {
        My_account My_account { get; set; }
      public  Int32 Flag { get; set; }

        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ShippingInformation(My_account my_account)
        {
            InitializeComponent();
            My_account = my_account;
            Flag = 1;
            try
            {
                LoadShippingInformation();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadShippingInformation()
        {
            string sqlExpression = "sp_SelectShippingInformation";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    
                    while (reader.Read()) // построчно считываем данные
                    {
                      
                        String firstname = reader.GetString(0);
                        String lastname = reader.GetString(1);
                        String streetaddress = reader.GetString(2);
                        String city = reader.GetString(3);
                        String phonenumber = reader.GetString(4);

                       
                        FirstName.Text = firstname;
                        LastName.Text = lastname;
                        StreetAddress.Text = streetaddress;
                        City.Text = city;
                        PhoneNumber.Text = phonenumber;
                     


                    }
                }
                else
                {
                    Flag = 0;
                    MessageBox.Show("No data yet, please fill in all fields");
                   
                    
                }


                reader.Close();


            }
        }

        private void AlterSInfo_Click(object sender, RoutedEventArgs e)
        {
            My_account.StackMain.Children.Clear();
            My_account.StackMain.Children.Add(new UserControls.EditShippingInformation(My_account, this));
        }
    }
}
