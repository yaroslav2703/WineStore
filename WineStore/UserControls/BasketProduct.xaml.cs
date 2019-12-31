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
    /// Логика взаимодействия для BasketProduct.xaml
    /// </summary>
    public partial class BasketProduct : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        String Product_code{ get; set; }
         String Title { get; set; }
         Int32 Volume { get; set; }
         Decimal Price { get; set; }
         String Foto { get; set; }
        Basket Basket1 { get; set; }

    
        public BasketProduct(Basket basket, String product_code, String title, Int32 volume, Decimal price,  String foto)
        {
            InitializeComponent();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@foto, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            ImageView.Source = bi;
            TitleView.Text = title;
            CodeView.Text = product_code;
            VolumeView.Text = volume.ToString() + "cl";
            PriceView.Text = "£" + price.ToString();
            Product_code = product_code;
            Basket1 = basket;
          
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RemoveWineFromBasket();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Basket1.MainWindow1.GridMain.Children.Clear();
            Basket1.MainWindow1.GridMain.Children.Add(new UserControls.Basket(Basket1.MainWindow1));
        }

        private void RemoveWineFromBasket()
        {
            string sqlExpression = "sp_RemoveWineFromBasket";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter CodeParam = new SqlParameter
                {
                    ParameterName = "@Code",
                    Value = Product_code
                };

                command.Parameters.Add(CodeParam);

                command.ExecuteNonQuery();



            }
        }
    }
}
