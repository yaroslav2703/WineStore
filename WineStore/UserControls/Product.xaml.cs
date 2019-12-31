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
  
    public partial class Product : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        Int32 ID { get; set; }
        String Product_code { get; set; }
        String Title { get; set; }
        Int32 Volume { get; set; }
        Decimal Price { get; set; }
        Int32 Vintage { get; set; }
        Int32 Available { get; set; }
        String Type { get; set; }
        String Country { get; set; }
        String Category { get; set; }
        String Foto { get; set; }
        public Product(Wines wines, Int32 id, String product_code, String title, Int32 volume, Decimal price, Int32 vintage, Int32 available, String type, String country, String category, String foto)
        {
            InitializeComponent();
            ID = id;
            Product_code = product_code;
            Title = title;
            Volume = volume;
            Price = price;
            Vintage = vintage;
            Available = available;
            Type = type;
            Country = country;
            Category = category;
            Foto = foto;
            TitleView.Text = Title;
            BitmapImage bi = new BitmapImage();        
            bi.BeginInit();
            bi.UriSource = new Uri(@Foto, UriKind.RelativeOrAbsolute);
            bi.EndInit();         
            ImageView.Source = bi;
            VolumeView.Text = Volume + " cl";
            AvailableView.Text = "Available: " + Available;
            PriceView.Text = "£" + Price;
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            try
            {


               

                        if (ProductAdd(ID))
                        {
                            MessageBox.Show("Product successfully added!");
                           
                        }
                        else
                        {
                            MessageBox.Show("Product not added");
                           
                        }


                  

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ProductAdd(Int32 id)
        {

            string sqlExpression = "sp_AddWinesToBasket";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id
                };

                command.Parameters.Add(idParam);

              
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