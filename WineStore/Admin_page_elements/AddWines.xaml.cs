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

namespace WineStore.Admin_page_elements
{
   
    public partial class AddWines : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        ForAdmin forAdmin;
        public AddWines(ForAdmin forAdmin)
        {
            InitializeComponent();
            this.forAdmin = forAdmin;
        }

        private void AddWine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(AddWinesSP(codeWines.Text, nameWines.Text, categoryWines.Text, typeWines.Text, countryWines.Text, Int32.Parse(VintageWines.Text), Int32.Parse(PriceWines.Text), Double.Parse(VolumeWines.Text), Int32.Parse(AvailableWines.Text), FotoWines.Text))
                {
                    MessageBox.Show("Wine added successfully");
                    ForAdmin forAdminN = new ForAdmin();
                    forAdminN.Show();
                    forAdmin.Close();
                   

                    
                }
                else
                {
                    MessageBox.Show("No wine added");                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool AddWinesSP(string Code, string Name, string Category, string Type, string Country, int Vintage, int Price, double Volume, int Available, string Foto)
        {
            string sqlExpression = "sp_AddWines";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (Code == null)
                {
                    MessageBox.Show("specify the Product code parameter");
                    return false;
                }
                SqlParameter CodeParam = new SqlParameter
                {
                    ParameterName = "@Code",
                    Value = Code
                };

                command.Parameters.Add(CodeParam);

                if (Name == null)
                {
                    MessageBox.Show("specify the Product name parameter");
                    return false;
                }
                SqlParameter NameParam = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = Name
                };

                command.Parameters.Add(NameParam);

                if (Category == null)
                {
                    MessageBox.Show("specify the Category parameter");
                    return false;
                }
                SqlParameter CategoryParam = new SqlParameter
                {
                    ParameterName = "@Category",
                    Value = Category
                };

                command.Parameters.Add(CategoryParam);


                if (Type == null)
                {
                    MessageBox.Show("specify the Type parameter");
                    return false;
                }
                SqlParameter TypeParam = new SqlParameter
                {
                    ParameterName = "@Type",
                    Value = Type
                };

                command.Parameters.Add(TypeParam);


                if (Country == null)
                {
                    MessageBox.Show("specify the Country parameter");
                    return false;
                }
                SqlParameter CountryParam = new SqlParameter
                {
                    ParameterName = "@Country",
                    Value = Country
                };

                command.Parameters.Add(CountryParam);


                if (Vintage == 0)
                {
                    MessageBox.Show("specify the Vintage parameter");
                    return false;
                }
                SqlParameter VintageParam = new SqlParameter
                {
                    ParameterName = "@Vintage",
                    Value = Vintage
                };

                command.Parameters.Add(VintageParam);


                if (Price == 0)
                {
                    MessageBox.Show("specify the Price parameter");
                    return false;
                }
                SqlParameter PriceParam = new SqlParameter
                {
                    ParameterName = "@Price",
                    Value = Price
                };

                command.Parameters.Add(PriceParam);

                if (Volume == 0)
                {
                    MessageBox.Show("specify the Volume parameter");
                    return false;
                }
                SqlParameter VolumeParam = new SqlParameter
                {
                    ParameterName = "@Volume",
                    Value = Volume
                };

                command.Parameters.Add(VolumeParam);

                if (Available == 0)
                {
                    MessageBox.Show("specify the Available parameter");
                    return false;
                }
                SqlParameter AvailableParam = new SqlParameter
                {
                    ParameterName = "@Available",
                    Value = Available
                };

                command.Parameters.Add(AvailableParam);

                if (Foto == null)
                {
                    Foto = "/Images/default_drink_wine_red_teaser.png";
                  
                }
                SqlParameter FotoParam = new SqlParameter
                {
                    ParameterName = "@Foto",
                    Value = Foto
                };

                command.Parameters.Add(FotoParam);


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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            forAdmin.GridMain.Children.Clear();
           forAdmin.GridMain.Children.Add(forAdmin.start);
        }
    }
}
