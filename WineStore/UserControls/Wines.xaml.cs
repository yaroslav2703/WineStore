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
    /// Логика взаимодействия для Wines.xaml
    /// </summary>
    public partial class Wines : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        string Category=null;
        string Type=null;
        string Country=null;
        int Vintage=0;
        int Price=0;
        int PriceMin=0;
        int PriceMax=0;
        double Volume=0;
        string foto = null;
        public Wines(MainWindow mainWindow)
        {
            InitializeComponent();
            SelectAllWines();
        }

        private void SelectAllWines()
        {
            string sqlExpression = "sp_SelectALLWines";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    ListofWines.Children.Clear();
                    while (reader.Read()) // построчно считываем данные
                    {
                        Int32 id = reader.GetInt32(0);
                        String product_code = reader.GetString(1);
                        String title = reader.GetString(2);
                        Int32 valua = reader.GetInt32(3);
                        Decimal price = reader.GetDecimal(4);
                        Int32 vintage = reader.GetInt32(5);                    
                        Int32 available = reader.GetInt32(6);
                        String type = reader.GetString(7);
                        String country = reader.GetString(8);
                        String category = reader.GetString(9);
                        String foto = reader.GetString(10);

                        ListofWines.Children.Add(new UserControls.Product(this,id, product_code, title, valua, price, vintage, available, type, country, category, foto));
                    
                      
                    }
                }
                else
                {
                    MessageBox.Show("No such data");
                }


                reader.Close();

              
            }
        }






        private void CategoryWines_Checked(object sender, RoutedEventArgs e)
        {
           RadioButton selectedRadio = (RadioButton)e.Source;
           Category = selectedRadio.Content.ToString();
        }

        private void TypeWines_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton selectedRadio = (RadioButton)e.Source;
            Type = selectedRadio.Content.ToString();
        }

        private void CountryWines_Checked(object sender, RoutedEventArgs e)
        {

            RadioButton selectedRadio = (RadioButton)e.Source;
            Country = selectedRadio.Content.ToString();
        }

        private void VintageWines_Checked(object sender, RoutedEventArgs e)
        {

            RadioButton selectedRadio = (RadioButton)e.Source;
            Vintage =  Int32.Parse(selectedRadio.Content.ToString());
        }

        private void PriceWines_Checked(object sender, RoutedEventArgs e)
        {

            RadioButton selectedRadio = (RadioButton)e.Source;
            string PriceChecked = selectedRadio.Content.ToString();
            switch (PriceChecked)
            {
                case "Under £30":
                    PriceMin = 0;
                    PriceMax = 30;
                    break;
                case "£30 - £100":
                    PriceMin = 30;
                    PriceMax = 100;
                    break;             
                case "£100 - £250":
                    PriceMin = 100;
                    PriceMax = 250;
                    break;
                case "£250 - £500":
                    PriceMin = 250;
                    PriceMax = 500;
                    break;
                case "£500 - £1000":
                    PriceMin = 500;
                    PriceMax = 1000;
                    break;
                case "Over £1000":
                    PriceMin = 1000;
                    PriceMax = int.MaxValue;
                    break;             
            }
           
        }

        private void VolumeWines_Checked(object sender, RoutedEventArgs e)
        {

            RadioButton selectedRadio = (RadioButton)e.Source;
            string VolumeAll = selectedRadio.Content.ToString();
            string cl = " cl";
            int indexOfSubstring = VolumeAll.IndexOf(cl);
            VolumeAll = VolumeAll.Substring(0, indexOfSubstring);         
            Volume = Double.Parse(VolumeAll);
        }




        private void BtnSelectWines_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectWines(TitleWines.Text ,Category, Type, Country, Vintage, PriceMin, PriceMax, Volume);

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private void SelectWines(string Title, string Category, string Type, string Country, int Vintage, int PriceMin, int PriceMax, double Volume)
        {
            string sqlExpression = "sp_SelectWines";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (Title == "" || Title == null)
                {
                    MessageBox.Show("input the Title");
                    return;
                }
                SqlParameter TitleParam = new SqlParameter
                {
                    ParameterName = "@Title",
                    Value = Title
                };

                command.Parameters.Add(TitleParam);

                if (Category == null)
                {
                    MessageBox.Show("specify the Category parameter");
                    return;
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
                    return;
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
                    return;
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
                    return;
                }
                SqlParameter VintageParam = new SqlParameter
                {
                    ParameterName = "@Vintage",
                    Value = Vintage
                };

                command.Parameters.Add(VintageParam);


                if (PriceMin == 0)
                {
                    MessageBox.Show("specify the Price parameter");
                    return;
                }
                SqlParameter PriceMinParam = new SqlParameter
                {
                    ParameterName = "@PriceMin",
                    Value = PriceMin
                };

                command.Parameters.Add(PriceMinParam);


                if (PriceMax == 0)
                {
                    MessageBox.Show("specify the Price parameter");
                    return;
                }
                SqlParameter PriceMaxParam = new SqlParameter
                {
                    ParameterName = "@PriceMax",
                    Value = PriceMax
                };

                command.Parameters.Add(PriceMaxParam);


                if (Volume == 0)
                {
                    MessageBox.Show("specify the Volume parameter");
                    return;
                }
                SqlParameter VolumeParam = new SqlParameter
                {
                    ParameterName = "@Volume",
                    Value = Volume
                };

                command.Parameters.Add(VolumeParam);


                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    ListofWines.Children.Clear();
                    while (reader.Read()) // построчно считываем данные
                    {
                        Int32 id = reader.GetInt32(0);
                        String product_code = reader.GetString(1);
                        String title = reader.GetString(2);
                        Int32 valua = reader.GetInt32(3);
                        Decimal price = reader.GetDecimal(4);
                        Int32 vintage = reader.GetInt32(5);
                        Int32 available = reader.GetInt32(6);
                        String type = reader.GetString(7);
                        String country = reader.GetString(8);
                        String category = reader.GetString(9);
                        String foto = reader.GetString(10);

                        ListofWines.Children.Add(new UserControls.Product(this, id, product_code, title, valua, price, vintage, available, type, country, category, foto));


                    }
                }
                else
                {
                    MessageBox.Show("No such data");
                }


                reader.Close();




            }
        }








        private void ComboBox_Selected(object sender, SelectionChangedEventArgs e)
        {
           
         
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;         
            TextBlock Item = (TextBlock)selectedItem.Content;
            if (Item.Text.ToString() != "Click to sort by:")
            {
                
                string SelectedSort = Item.Text.ToString();
                switch (SelectedSort)
                {
                    case "Default sort":
                        SelectAllWines();
                        break;
                    case "Lowest price":
                        SelectAllWinesSortedPrice(1);
                        break;
                    case "Highest price":
                        SelectAllWinesSortedPrice(0);
                        break;
                    case "A-Z":
                        SelectAllWinesSortedName(1);
                        break;
                    case "Z-A":
                        SelectAllWinesSortedName(0);
                        break;
                    case "Vintage(old to new)":
                        SelectAllWinesSortedVintage(1);
                        break;
                    case "Vintage(new to old)":
                        SelectAllWinesSortedVintage(0);
                        break;
                }
            }
          

          


        }

        private void SelectAllWinesSortedPrice(Int32 priceSort)
        {
            string sqlExpression = "sp_SelectAllWinesSortedPrice";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter PriceSort = new SqlParameter
                {
                    ParameterName = "@PriceSort",
                    Value = priceSort
                };

                command.Parameters.Add(PriceSort);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    ListofWines.Children.Clear();
                    while (reader.Read()) // построчно считываем данные
                    {
                        Int32 id = reader.GetInt32(0);
                        String product_code = reader.GetString(1);
                        String title = reader.GetString(2);
                        Int32 valua = reader.GetInt32(3);
                        Decimal price = reader.GetDecimal(4);
                        Int32 vintage = reader.GetInt32(5);
                        Int32 available = reader.GetInt32(6);
                        String type = reader.GetString(7);
                        String country = reader.GetString(8);
                        String category = reader.GetString(9);
                        String foto = reader.GetString(10);

                        ListofWines.Children.Add(new UserControls.Product(this, id, product_code, title, valua, price, vintage, available, type, country, category, foto));


                    }
                }
                else
                {
                    MessageBox.Show("Таких данных нет");
                }


                reader.Close();


            }
        }

        private void SelectAllWinesSortedName(Int32 nameSort)
        {
            string sqlExpression = "sp_SelectAllWinesSortedName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter NameSort = new SqlParameter
                {
                    ParameterName = "@NameSort",
                    Value = nameSort
                };

                command.Parameters.Add(NameSort);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    ListofWines.Children.Clear();
                    while (reader.Read()) // построчно считываем данные
                    {
                        Int32 id = reader.GetInt32(0);
                        String product_code = reader.GetString(1);
                        String title = reader.GetString(2);
                        Int32 valua = reader.GetInt32(3);
                        Decimal price = reader.GetDecimal(4);
                        Int32 vintage = reader.GetInt32(5);
                        Int32 available = reader.GetInt32(6);
                        String type = reader.GetString(7);
                        String country = reader.GetString(8);
                        String category = reader.GetString(9);
                        String foto = reader.GetString(10);

                        ListofWines.Children.Add(new UserControls.Product(this, id, product_code, title, valua, price, vintage, available, type, country, category, foto));


                    }
                }
                else
                {
                    MessageBox.Show("Таких данных нет");
                }


                reader.Close();


            }
        }

        private void SelectAllWinesSortedVintage(Int32 vintageSort)
        {
            string sqlExpression = "sp_SelectAllWinesSortedVintage";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter VintageSort = new SqlParameter
                {
                    ParameterName = "@VintageSort",
                    Value = vintageSort
                };

                command.Parameters.Add(VintageSort);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    ListofWines.Children.Clear();
                    while (reader.Read()) // построчно считываем данные
                    {
                        Int32 id = reader.GetInt32(0);
                        String product_code = reader.GetString(1);
                        String title = reader.GetString(2);
                        Int32 valua = reader.GetInt32(3);
                        Decimal price = reader.GetDecimal(4);
                        Int32 vintage = reader.GetInt32(5);
                        Int32 available = reader.GetInt32(6);
                        String type = reader.GetString(7);
                        String country = reader.GetString(8);
                        String category = reader.GetString(9);
                        String foto = reader.GetString(10);

                        ListofWines.Children.Add(new UserControls.Product(this, id, product_code, title, valua, price, vintage, available, type, country, category, foto));


                    }
                }
                else
                {
                    MessageBox.Show("Таких данных нет");
                }


                reader.Close();


            }
        }



    }
}
