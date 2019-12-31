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
using System.Xml;

namespace WineStore.Admin_page_elements
{
    /// <summary>
    /// Логика взаимодействия для Export.xaml
    /// </summary>
    public partial class Export : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        ForAdmin forAdmin;
        public Export(ForAdmin forAdmin)
        {
            InitializeComponent();
            this.forAdmin = forAdmin;
            try
            {
                string XMLcode = XMLExport();
                XMLcode = XMLcode.Replace("<WineData>", "<WineData>\n");
                XMLcode = XMLcode.Replace("</WineData>", "</WineData>\n");
                XMLcode = XMLcode.Replace("<Wine>", "<Wine>\n");
                XMLcode = XMLcode.Replace("</Wine>", "</Wine>\n");
                XMLcode = XMLcode.Replace("</ID>", "</ID>\n");
                XMLcode = XMLcode.Replace("</PRODUCT_CODE>", "</PRODUCT_CODE>\n");
                XMLcode = XMLcode.Replace("</TITLE>", "</TITLE>\n");
                XMLcode = XMLcode.Replace("</VOLUME>", "</VOLUME>\n");
                XMLcode = XMLcode.Replace("</PRICE>", "</PRICE>\n");
                XMLcode = XMLcode.Replace("</VINTAGE>", "</VINTAGE>\n");
                XMLcode = XMLcode.Replace("</AVAILABLE>", "</AVAILABLE>\n");
                XMLcode = XMLcode.Replace("</TYPE>", "</TYPE>\n");
                XMLcode = XMLcode.Replace("</COUNTRY>", "</COUNTRY>\n");
                XMLcode = XMLcode.Replace("</COTEGORY>", "</COTEGORY>\n");
                XMLcode = XMLcode.Replace("</FOTO>", "</FOTO>\n");
                XMLView.Text = XMLcode;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string XMLExport()
        {
            string sqlExpression = "sp_ExportWineToXML";
            string XML;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;


                SqlParameter xmlmParam = new SqlParameter
                {
                    ParameterName = "@xmlm",
                    SqlDbType = SqlDbType.Xml,
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(xmlmParam);

                command.ExecuteNonQuery();


                XML = xmlmParam.Value.ToString();
              
            }
            return XML;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            forAdmin.GridMain.Children.Clear();
            forAdmin.GridMain.Children.Add(forAdmin.start);
        }
    }
}
