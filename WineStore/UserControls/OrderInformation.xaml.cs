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
  
    public partial class OrderInformation : UserControl
    {

        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        Basket Basket1 { get; set; }
        Int32 Count { get; set; }
        Int32 Summ { get; set; }
        public OrderInformation(Basket basket, Int32 count, Int32 summ)
        {
            Summ = summ;
            Count = count;
            Basket1 = basket;
            InitializeComponent();
            try
            {
                LoadShippingInformation();
                LoadPaymentInformation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadPaymentInformation()
        {
            string sqlExpression = "sp_SelectPaymentInformation";

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

                        Int32 cardnumber = reader.GetInt32(0);
                        String expirationdate = reader.GetString(1);
                        Int32 cvc = reader.GetInt32(2);



                        CardNumber.Text = cardnumber.ToString();
                        ExpirationDate.Text = expirationdate;
                        CVC.Text = cvc.ToString();




                    }
                }
                else
                {
                   
                    MessageBox.Show("No data yet, please fill in all fields.");


                }


                reader.Close();


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


                        Fname.Text = firstname;
                        Lname.Text = lastname;
                        StreetAddress.Text = streetaddress;
                        City.Text = city;
                        PhoneNumber.Text = phonenumber;



                    }
                }
                else
                {
                  
                    MessageBox.Show("No data yet, please fill in all fields");


                }


                reader.Close();


            }
        }

        private void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (OrderAdd(Count, Summ))
                {

                    MessageBox.Show("Order successfully added!");
                    Basket1.MainWindow1.GridMain.Children.Clear();
                    Basket1.MainWindow1.GridMain.Children.Add(new UserControls.Basket(Basket1.MainWindow1));

                }
                else
                {
                    MessageBox.Show("Order not added");

                }





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private bool OrderAdd(Int32 count, Int32 price)
        {

            string sqlExpression = "sp_CreateOrder";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter countParam = new SqlParameter
                {
                    ParameterName = "@count",
                    Value = count
                };

                command.Parameters.Add(countParam);

                SqlParameter priceParam = new SqlParameter
                {
                    ParameterName = "@price",
                    Value = price
                };

                command.Parameters.Add(priceParam);

                if (Fname.Text == "")
                {
                    MessageBox.Show("specify the First name parameter");
                    return false;
                }
                SqlParameter FirstNameParam = new SqlParameter
                {
                    ParameterName = "@FirstName",
                    Value = Fname.Text
                };

                command.Parameters.Add(FirstNameParam);


                if (Lname.Text == "")
                {
                    MessageBox.Show("specify the Last name parameter");
                    return false;
                }
                SqlParameter LastNameParam = new SqlParameter
                {
                    ParameterName = "@LastName",
                    Value = Lname.Text
                };

                command.Parameters.Add(LastNameParam);


                if (StreetAddress.Text == "")
                {
                    MessageBox.Show("specify the Street Address parameter");
                    return false;
                }
                SqlParameter StreetAddressParam = new SqlParameter
                {
                    ParameterName = "@StreetAddress",
                    Value = StreetAddress.Text
                };

                command.Parameters.Add(StreetAddressParam);

                if (City.Text == "")
                {
                    MessageBox.Show("specify the City parameter");
                    return false;
                }
                SqlParameter CityParam = new SqlParameter
                {
                    ParameterName = "@City",
                    Value = City.Text
                };

                command.Parameters.Add(CityParam);

                if (PhoneNumber.Text == "")
                {
                    MessageBox.Show("specify the Phone Number parameter");
                    return false;
                }
                SqlParameter PhoneNumberParam = new SqlParameter
                {
                    ParameterName = "@PhoneNumber",
                    Value = PhoneNumber.Text
                };

                command.Parameters.Add(PhoneNumberParam);

                if (CardNumber.Text == "")
                {
                    MessageBox.Show("specify the Card number parameter");
                    return false;
                }
                SqlParameter CardNumberParam = new SqlParameter
                {
                    ParameterName = "@CardNumber",
                    Value = Int32.Parse(CardNumber.Text)
                };

                command.Parameters.Add(CardNumberParam);


                if (ExpirationDate.Text == "")
                {
                    MessageBox.Show("specify the Expiration Date parameter");
                    return false;
                }
                SqlParameter ExpirationDateParam = new SqlParameter
                {
                    ParameterName = "@ExpirationDate",
                    Value = ExpirationDate.Text.ToString()
                };

                command.Parameters.Add(ExpirationDateParam);


                if (CVC.Text == "")
                {
                    MessageBox.Show("specify the CVC parameter");
                    return false;
                }
                SqlParameter CVCParam = new SqlParameter
                {
                    ParameterName = "@CVC",
                    Value = Int32.Parse(CVC.Text)
                };

                command.Parameters.Add(CVCParam);


                var result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
        }
    }
}
