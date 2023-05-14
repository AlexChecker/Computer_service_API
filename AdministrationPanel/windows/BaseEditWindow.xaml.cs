using AdministrationPanel.Models;
using AdministrationPanel.utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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
        private int page = 0;
        dynamic d;
        
        public BaseEditWindow()
        {
            InitializeComponent();
        }

        public async void Window_Loaded<T>(T tabl)
        {
            //data_grid.AutoGenerateColumns = false;
            data_grid.ItemsSource = null;
            data_grid.ItemsSource = await Utils.requestTable(tabl, table,page);
            d = tabl;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (data_grid.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите запись для изменения!");
                return;
            }
            var changed = data_grid.SelectedItem;
            switch (table)
            {
                case "Acquisitions":
                    Utils.updateTable(changed, table, (changed as Acquisition).AcqId.ToString()  );
                    break;
                case "Clients":
                    Utils.updateTable(changed, table, (changed as Client).Login );

                    break;
                case "Components":
                    Utils.updateTable(changed, table, (changed as Component).ArticleNum);

                    break;
                case "ComponentTypes":
                    Utils.updateTable(changed, table, (changed as ComponentType).TypeId.ToString());

                    break;
                case "ComponentUsages":
                    Utils.updateTable(changed, table, (changed as ComponentUsage).UsageId.ToString());

                    break;
                case "Departments":
                    Utils.updateTable(changed, table, (changed as Department).DepId.ToString());

                    break;
                case "Employees":
                    Utils.updateTable(changed, table, (changed as Employee).ServiceId);

                    break;
                case "Issues":
                    Utils.updateTable(changed, table, (changed as Issue).IssId.ToString());

                    break;
                case "IssueStatus":
                    Utils.updateTable(changed, table, (changed as IssueStatus).StatusId.ToString());

                    break;
                case "Orders":
                    Utils.updateTable(changed, table, (changed as Order).Id.ToString());

                    break;
                case "OrderHistories":
                    Utils.updateTable(changed, table, (changed as OrderHistory).HistId.ToString());

                    break;
                case "OrderStatus":
                    Utils.updateTable(changed, table, (changed as OrderStatus).StatusId.ToString());

                    break;
                case "OrderTypes":
                    Utils.updateTable(changed, table, (changed as OrderType).Type);

                    break;
                case "Reviews":
                    Utils.updateTable(changed, table, (changed as Review).RevId.ToString());

                    break;
                case "Vacancies":
                    Utils.updateTable(changed, table, (changed as Vacancy).VacId.ToString());

                    break;
            }
            Window_Loaded(d);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewEntryWindow add = new NewEntryWindow();
            add.tableName = table;
            add.Show();
            add.initEdit(d);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var itemForDelete = data_grid.SelectedItem;

            switch (table)
            {
                case "Acquisitions":
                    Utils.deleteEntry((itemForDelete as Acquisition).AcqId.ToString(), table);
                    break;
                case "Clients":
                    Utils.deleteEntry((itemForDelete as Client).Login, table);

                    break;
                case "Components":
                    Utils.deleteEntry((itemForDelete as Component).ArticleNum, table);

                    break;
                case "ComponentTypes":
                    Utils.deleteEntry((itemForDelete as ComponentType).TypeId.ToString(), table);

                    break;
                case "ComponentUsages":
                    Utils.deleteEntry((itemForDelete as ComponentUsage).UsageId.ToString(), table);

                    break;
                case "Departments":
                    Utils.deleteEntry((itemForDelete as Department).DepId.ToString(), table);

                    break;
                case "Employees":
                    Utils.deleteEntry((itemForDelete as Employee).ServiceId, table);

                    break;
                case "Issues":
                    Utils.deleteEntry((itemForDelete as Issue).IssId.ToString(), table);

                    break;
                case "IssueStatus":
                    Utils.deleteEntry((itemForDelete as IssueStatus).StatusId.ToString(), table);

                    break;
                case "Orders":
                    Utils.deleteEntry((itemForDelete as Order).Id.ToString(), table);

                    break;
                case "OrderHistories":
                    Utils.deleteEntry((itemForDelete as OrderHistory).HistId.ToString(), table);

                    break;
                case "OrderStatus":
                    Utils.deleteEntry((itemForDelete as OrderStatus).StatusId.ToString(), table);

                    break;
                case "OrderTypes":
                    Utils.deleteEntry((itemForDelete as OrderType).Type.ToString(), table);
                    break;
                case "Reviews":
                    Utils.deleteEntry((itemForDelete as Review).RevId.ToString(), table);

                    break;
                case "Vacancies":
                    Utils.deleteEntry((itemForDelete as Vacancy).VacId.ToString(), table);

                    break;
            }
            Window_Loaded(d);
        }

        private void data_grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Utils.loadWindowState(this);
            if (table == "Employees" || table == "Clients") deleteMany.IsEnabled = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Utils.saveWindowState(Title, this.Left, this.Top);

        }

        private void page_forward_Click(object sender, RoutedEventArgs e)
        {
            page++;
            pageLabel.Content = page.ToString();
            Window_Loaded(d);

        }

        private void page_back_Click(object sender, RoutedEventArgs e)
        {
            page--;
            if (page < 0) page = 0;
            pageLabel.Content = page.ToString();
            Window_Loaded(d);

        }

        private void deleteMany_Click(object sender, RoutedEventArgs e)
        {
            var deleteItems = data_grid.SelectedItems;
            if (deleteItems.Count == 0) return;
            List<string> strs = new List<string>();
            switch (table)
            {
                case "Employees":
                    foreach (var emp in deleteItems)
                    {
                        strs.Add((emp as Employee).Login);
                    }
                    break;
                case "Clients":
                    foreach (var emp in deleteItems)
                    {
                        strs.Add((emp as Client).Login);
                    }
                    break;
            }

            Utils.deleteMultiple(strs.ToArray(), table);

            Window_Loaded(d);
        }
    }
}
