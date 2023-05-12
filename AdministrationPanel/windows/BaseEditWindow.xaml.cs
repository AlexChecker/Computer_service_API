using AdministrationPanel.Models;
using AdministrationPanel.utils;
using System;
using System.Collections.Generic;
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

namespace AdministrationPanel.windows
{
    /// <summary>
    /// Логика взаимодействия для BaseEditWindow.xaml
    /// </summary>
    public partial class BaseEditWindow : Window
    {
        public string table = "";
        
        
        public BaseEditWindow()
        {
            InitializeComponent();
        }

        public async void Window_Loaded<T>(T tabl)
        {
            //data_grid.AutoGenerateColumns = false;
            data_grid.ItemsSource = await Utils.requestTable(tabl, table);
        }
    }
}
