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
    /// Логика взаимодействия для EditPaymentInformation.xaml
    /// </summary>
    public partial class EditPaymentInformation : UserControl
    {
        My_account My_account { get; set; }
        PaymentInformation Info { get; set; }
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public EditPaymentInformation(My_account my_account, PaymentInformation info)
        {
            InitializeComponent();
            My_account = my_account;
            Info = info;
            CardNumber.Text = Info.CardNumber.Text;
            ExpirationDate.Text = Info.ExpirationDate.Text;
            CVC.Text = Info.CVC.Text;
           
        }

        private void BtnSaveShInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UpdatePaymentInformation())
                {
                    MessageBox.Show("data changed successfully");
                    My_account.StackMain.Children.Clear();
                    My_account.StackMain.Children.Add(new UserControls.PaymentInformation(My_account));
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool UpdatePaymentInformation()
        {
            string sqlExpression = "sp_UpdatePaymentInformation";

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

                

                command.ExecuteNonQuery();

                return true;



            }
        }
    }
}
