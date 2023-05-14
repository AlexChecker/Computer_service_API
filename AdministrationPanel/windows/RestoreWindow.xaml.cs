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
    /// Логика взаимодействия для RestoreWindow.xaml
    /// </summary>
    public partial class RestoreWindow : Window
    {

        string table;
        dynamic d;
        int page = 0;
        public RestoreWindow(string table, dynamic d)
        {
            InitializeComponent();
            this.table = table;
            this.d = d;
        }

        private void restoreMultiple_Click(object sender, RoutedEventArgs e)
        {
            var restrable = data_grid.SelectedItems;
            List<string> ids = new List<string>();
            switch (table)
            {
                case "Employees":
                    foreach (var emp in restrable)
                    {
                        ids.Add((emp as Employee).Login);
                    }
                    break;
                case "Clients":
                    foreach (var emp in restrable)
                    {
                        ids.Add((emp as Client).Login);
                    }

                    break;
            }
            //This is just POST method, it's just named incorrectly
            Utils.createEntry(ids, $"{table}/multiple/restore");
            updateTable();
        }

        private void restoreOne_Click(object sender, RoutedEventArgs e)
        {
            var sel = data_grid.SelectedItem;
            switch (table)
            {
                case "Employees":
                    Utils.createEntry("", $"{table}/restore?login={(sel as Employee).Login}");
                    break;
                case "Clients":
                    Utils.createEntry("", $"{table}/restore?login={(sel as Client).Login}");

                    break;
            }
            updateTable();
        }

        async void updateTable()
        {
            data_grid.ItemsSource = null;
            data_grid.ItemsSource = await Utils.requestTable(d, table, page,true);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updateTable();
        }

        private void previousPage_Click(object sender, RoutedEventArgs e)
        {
            page--;
            if (page < 0) page = 0;
            updateTable();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            page++;
            updateTable();
        }
    }
}
