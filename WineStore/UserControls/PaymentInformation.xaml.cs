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
    /// Логика взаимодействия для PaymentInformation.xaml
    /// </summary>
    public partial class PaymentInformation : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        My_account My_account { get; set; }
        public Int32 Flag { get; set; }

        public PaymentInformation(My_account my_account)
        {
            InitializeComponent();
            My_account = my_account;
            Flag = 1;
            try
            {
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
                    Flag = 0;
                    MessageBox.Show("No data yet, please fill in all fields. Billing information will not be saved until basic information is provided.");


                }


                reader.Close();


            }
        }

        private void AlterPInfo_Click(object sender, RoutedEventArgs e)
        {
            My_account.StackMain.Children.Clear();
            My_account.StackMain.Children.Add(new UserControls.EditPaymentInformation(My_account, this));
        }
    }
}
