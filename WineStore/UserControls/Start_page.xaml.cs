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
using System.Drawing;

namespace WineStore.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Start_page.xaml
    /// </summary>
    public partial class Start_page : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public Start_page(MainWindow mainWindow)
        {
            InitializeComponent();
            StartSelect();
        }

        private void StartSelect()
        {

            string sqlExpression = "sp_SelectShopInfo";

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

                        String address = reader.GetString(0);
                        String telefon_number = reader.GetString(1);
                        String email = reader.GetString(2);

                        TextBlock textAddress = new TextBlock();
                        textAddress.Text = address;
                        textAddress.TextAlignment = TextAlignment.Center;
                        textAddress.Margin = new Thickness(5, 10, 5, 5);
                        textAddress.TextWrapping = TextWrapping.WrapWithOverflow;
                        textAddress.FontStyle = FontStyles.Italic;
                        textAddress.FontSize = 15;

                        TextBlock textTelefon = new TextBlock();
                        textTelefon.Text = telefon_number;
                        textTelefon.TextAlignment = TextAlignment.Center;
                        textTelefon.Margin = new Thickness(5, 10, 5, 5);
                        textTelefon.TextWrapping = TextWrapping.WrapWithOverflow;
                        textTelefon.FontSize = 15;

                        TextBlock textEmail = new TextBlock();
                        textEmail.Text = email;
                        textEmail.TextAlignment = TextAlignment.Center;
                        textEmail.Margin = new Thickness(5, 10, 5, 5);
                        textEmail.TextWrapping = TextWrapping.WrapWithOverflow;
                        textEmail.FontSize = 15;

                        MainStack.Children.Add(textAddress);
                        MainStack.Children.Add(textTelefon);
                        MainStack.Children.Add(textEmail);


                    }
                }
                else
                {
                    MainStack.Children.Clear();
                    TextBlock text = new TextBlock();
                    text.Text = "Nothing found on request";
                    text.TextAlignment = TextAlignment.Center;
                    text.FontSize = 20;

                    MainStack.Children.Add(text);
                }


                reader.Close();


            }
        }
    }
}
