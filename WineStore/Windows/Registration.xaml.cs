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

namespace WineStore.Windows
{

    public partial class Registration : Window
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public Registration()
        {
            InitializeComponent();
        }

        private void formLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            login login = new login();
            login.Show();
            Close();
        }

        

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

            try
            {


                if (txbLogin.Text != String.Empty && txbPassword1.Password != String.Empty && txbPassword2.Password != String.Empty)
                {
                    if (txbPassword1.Password == txbPassword2.Password)
                    {
                        
                        if (UserСreate(txbLogin.Text, txbPassword1.Password))
                        {
                            MessageBox.Show("User successfully added!");
                            login login = new login();
                            login.Show();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("User not created");
                            txbLogin.Text = "";
                            txbPassword1.Password = "";
                            txbPassword2.Password = "";
                        }


                    }
                    else
                    {
                        MessageBox.Show("Passwords do not match!");
                        txbLogin.Text = "";
                        txbPassword1.Password = "";
                        txbPassword2.Password = "";
                    }

                }
                else
                {
                    MessageBox.Show("Fill in all the fields!");
                    txbLogin.Text = "";
                    txbPassword1.Password = "";
                    txbPassword2.Password = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private bool UserСreate(string login, string password)
        {
            if (!CheckUser(login, password))
            {
                

                string sqlExpression = "sp_InsertUser";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter loginParam = new SqlParameter
                    {
                        ParameterName = "@login",
                        Value = login
                    };

                    command.Parameters.Add(loginParam);

                    SqlParameter passwordParam = new SqlParameter
                    {
                        ParameterName = "@password",
                        Value = password
                    };

                    command.Parameters.Add(passwordParam);

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
            else
            {
                MessageBox.Show("This user already exists");
                txbLogin.Text = "";
                txbPassword1.Password = "";
                txbPassword2.Password = "";
                return false;
            }
        }

        private bool CheckUser(string login, string password)
        {

           

            string sqlExpression = "sp_UserVerification";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (login == "admin" || password == "admin")
                {
                    return true;
                }
                SqlParameter loginParam = new SqlParameter
                {
                    ParameterName = "@login",
                    Value = login
                };

                command.Parameters.Add(loginParam);

                SqlParameter passwordParam = new SqlParameter
                {
                    ParameterName = "@password",
                    Value = password
                };

                command.Parameters.Add(passwordParam);

                SqlParameter resultParam = new SqlParameter
                {
                    ParameterName = "@result",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(resultParam);

                command.ExecuteNonQuery();


                if ((int)command.Parameters["@result"].Value == 1) return true;
                else return false;


            }
        }

    
       
    }
}