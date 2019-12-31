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

namespace WineStore
{
    /// <summary>
    /// Логика взаимодействия для ForAdmin.xaml
    /// </summary>
    public partial class ForAdmin : Window
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        public ForAdmin()
        {
            InitializeComponent();
            
        }
        private void exitAdmin_Click(object sender, RoutedEventArgs e)
        {
            Windows.login login = new Windows.login();
            login.Show();
            Close();
        }

        private void ButtonA_Click(object sender, RoutedEventArgs e)
        {       
           GridMain.Children.Clear();
           GridMain.Children.Add(new Admin_page_elements.AddWines(this));
        }

        private void btnWinesAAvail_Click(object sender, RoutedEventArgs e)
        {
          GridMain.Children.Clear();
          GridMain.Children.Add(new Admin_page_elements.AddWinesAvailable(this));
        }

        private void btnWinesAXMLImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AddWineFromXML())
                {
                    MessageBox.Show("Wine added successfully");
                    ForAdmin forAdminN = new ForAdmin();
                    forAdminN.Show();
                    this.Close();



                }
                else
                {
                    MessageBox.Show("No wine added");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wine with this product code is already in the store catalog");
            }
        }

        private void btnWinesAXMLExport_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            GridMain.Children.Add(new Admin_page_elements.Export(this));
        }

        private bool AddWineFromXML()
        {
            string sqlExpression = "sp_ImportWineFromXML";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

         
                var result = command.ExecuteNonQuery();

                if (result == 1)
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
