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
    /// Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public About(MainWindow mainWindow)
        {
            InitializeComponent();
            SelectAllFaqs();
        }

        private void SelectAllFaqs()
        {
            string sqlExpression = "sp_SelectAllFaqs";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    StackMain.Children.Clear();
                    while (reader.Read()) // построчно считываем данные
                    {
                        
                        String question = reader.GetString(0);
                        String answer = reader.GetString(1);

                        TextBlock textQuestion = new TextBlock();
                        textQuestion.Text = question;
                        textQuestion.Margin = new Thickness(5, 10, 5, 5);
                        textQuestion.TextWrapping = TextWrapping.WrapWithOverflow;
                        textQuestion.FontStyle = FontStyles.Italic;
                        textQuestion.FontSize = 20;

                        TextBlock textAnswer = new TextBlock();
                        textAnswer.Text = answer;
                        textAnswer.Margin = new Thickness(10, 10, 5, 20);
                        textAnswer.TextWrapping = TextWrapping.WrapWithOverflow;
                        textAnswer.FontSize = 15;

                        StackMain.Children.Add(textQuestion);
                        StackMain.Children.Add(textAnswer);


                    }
                }
                else
                {
                    StackMain.Children.Clear();
                    TextBlock text = new TextBlock();
                    text.Text = "Nothing found on request";
                    text.TextAlignment = TextAlignment.Center;
                    text.FontSize = 20;

                    StackMain.Children.Add(text);
                }


                reader.Close();


            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string sqlExpression = "sp_SelectFromFaqs";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (Request.Text == "")
                {
                    return;
                }
                SqlParameter TextParam = new SqlParameter
                {
                    ParameterName = "@Text",
                    Value = Request.Text.ToString()
                };

                   command.Parameters.Add(TextParam);

                   SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    StackMain.Children.Clear();
                    while (reader.Read()) // построчно считываем данные
                    {

                        String question = reader.GetString(0);
                        String answer = reader.GetString(1);

                        TextBlock textQuestion = new TextBlock();
                        textQuestion.Text = question;
                        textQuestion.Margin = new Thickness(5, 10, 5, 5);
                        textQuestion.TextWrapping = TextWrapping.WrapWithOverflow;
                        textQuestion.FontStyle = FontStyles.Italic;
                        textQuestion.FontSize = 20;

                        TextBlock textAnswer = new TextBlock();
                        textAnswer.Text = answer;
                        textAnswer.Margin = new Thickness(10, 10, 5, 20);
                        textAnswer.TextWrapping = TextWrapping.WrapWithOverflow;
                        textAnswer.FontSize = 15;

                        StackMain.Children.Add(textQuestion);
                        StackMain.Children.Add(textAnswer);


                    }
                }
                else
                {

                    StackMain.Children.Clear();
                    TextBlock text = new TextBlock();
                    text.Text = "Nothing found on request";
                    text.TextAlignment = TextAlignment.Center;
                    text.FontSize = 20;

                    StackMain.Children.Add(text);
                }


                reader.Close();


            }
        }
    }
}
