using AdministrationPanel.Models;
using AdministrationPanel.utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
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

namespace AdministrationPanel.windows
{
    /// <summary>
    /// Логика взаимодействия для BaseEditWindow.xaml
    /// </summary>
    public partial class BaseEditWindow : Window
    {
        public string table = "";
        Type type;
        
        
        public BaseEditWindow()
        {
            InitializeComponent();
        }

        public async void Window_Loaded<T>(T tabl)
        {
            //data_grid.AutoGenerateColumns = false;
            data_grid.ItemsSource = await Utils.requestTable(tabl, table);
            type = typeof(T);
        }

        private void data_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (data_grid.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Выберите запись для изменения!");
            //    return;
            //}
            //
            ////DataRowView view = (DataRowView)data_grid.SelectedItem;
            //int index = data_grid.CurrentCell.Column.DisplayIndex;
            //
            ////string cellvalue = view.Row.ItemArray[index].ToString();
            ////data_grid.Columns[index];
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
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
