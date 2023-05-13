using AdministrationPanel.Models;
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
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tablePath = "", windowName;
            Type tabletype = null;
            BaseEditWindow win = new BaseEditWindow();
            
            Button b = (Button)sender;
            windowName = $"Редактировать {b.Content}";
            switch (b.Content)
            {
                case "Закупки":
                    tablePath = "Acquisitions";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new Acquisition());
                    break;
                case "Клиенты":
                    tablePath = "Clients";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new Client());
                    break;
                case "Компоненты":
                    tablePath = "Components";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new Component());
                    break;
                case "Типы компонентов":
                    tablePath = "ComponentTypes";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new ComponentType());
                    break;
                case "Использование компонентов":
                    tablePath = "ComponentUsages";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new ComponentUsage());
                    break;
                case "Отделы":
                    tablePath = "Departments";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new Department());
                    break;
                case "Сотрудники":
                    tablePath = "Employees";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new Employee());
                    break;
                case "Проблемы":
                    tablePath = "Issues";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new Issue());
                    break;
                case "Статусы проблем":
                    tablePath = "IssueStatus";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new IssueStatus());
                    break;
                case "Заказы":
                    tablePath = "Orders";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new Order());
                    break;
                case "История заказов":
                    tablePath = "OrderHistories";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new OrderHistory());
                    break;
                case "Статусы заказа":
                    tablePath = "OrderStatus";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new OrderStatus());
                    break;
                case "Типы заказов":
                    tablePath = "OrderTypes";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new OrderType());
                    break;
                case "Отзывы":
                    tablePath = "Reviews";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new Review());
                    break;
                case "Вакансии":
                    tablePath = "Vacancies";
                    win.Title = windowName;
                    win.table = tablePath;
                    win.Show();
                    win.Window_Loaded(new Vacancy());
                    break;
            }
            

        }
    }
}
