using AdministrationPanel.utils;
using AdministrationPanel.windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdministrationPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var httpClient = new HttpClient())
            {
                var builder = new UriBuilder("https://localhost");
                builder.Port = 7253;
                builder.Path = "/api/EmployeeRegister/login";
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["login"] = loginBox.Text;
                query["password"] = passwordBox.Password;
                builder.Query = query.ToString();

                using (var response = await httpClient.GetAsync(builder.ToString()))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Utils.tokens = JsonConvert.DeserializeObject<Utils.Tokens>(apiResponse);
                        AdminPanel panel = new AdminPanel();
                        panel.Show();
                    }

                }
            }
        }
    }
}
