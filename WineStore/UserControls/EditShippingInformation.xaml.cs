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
  
    public partial class EditShippingInformation : UserControl
    {
        My_account My_account { get; set; }
        ShippingInformation Info { get; set; }
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public EditShippingInformation(My_account my_account, ShippingInformation info)
        {
            InitializeComponent();
            My_account = my_account;
            Info = info;
            FirstName.Text = Info.FirstName.Text;
            LastName.Text = Info.LastName.Text;
            StreetAddress.Text = Info.StreetAddress.Text;
            City.Text = Info.City.Text;
            PhoneNumber.Text = Info.PhoneNumber.Text;
        }

        private void BtnSaveShInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UpdateShippingInformation()){
                    MessageBox.Show("data changed successfully");
                    My_account.StackMain.Children.Clear();
                    My_account.StackMain.Children.Add(new UserControls.ShippingInformation(My_account));
                }
                
               
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool UpdateShippingInformation()
        {
            string sqlExpression = "sp_UpdateShippingInformation";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;


                SqlParameter FlagParam = new SqlParameter
                {
                    ParameterName = "@Flag",
                    Value = Info.Flag
                };

                command.Parameters.Add(FlagParam);

                if (FirstName.Text =="")
                {
                    MessageBox.Show("specify the First name parameter");
                    return false;
                }
                SqlParameter FirstNameParam = new SqlParameter
                {
                    ParameterName = "@FirstName",
                    Value = FirstName.Text
                };

                command.Parameters.Add(FirstNameParam);


                if (LastName.Text =="")
                {
                    MessageBox.Show("specify the Last name parameter");
                    return false;
                }
                SqlParameter LastNameParam = new SqlParameter
                {
                    ParameterName = "@LastName",
                    Value = LastName.Text
                };

                command.Parameters.Add(LastNameParam);


                if (StreetAddress.Text =="")
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

                command.ExecuteNonQuery();

                return true;



            }
        }
    }
}
