using AdministrationPanel.Models;
using AdministrationPanel.utils;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.CodeDom;
using System.Collections.Generic;
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
    /// Логика взаимодействия для NewEntryWindow.xaml
    /// </summary>
    public partial class NewEntryWindow : Window
    {
        public string tableName = "";
        public List<FrameworkElement> inputs = new List<FrameworkElement>();
        public List<string> types = new List<string>();
        dynamic d;
       
        public NewEntryWindow()
        {
            InitializeComponent();
        }

        public void initEdit<T>(T obj)
        {
            d = obj;
            //var variablesList = obj.GetType().GetFields().Select(field => field.Name).ToList();
            var varlist = obj.GetType().GetProperties();
            foreach (var prop in varlist)
            {
                Label l = new Label();
                l.Height = 30;
                l.Width = 260;
                l.Content = prop.Name;
                if ((prop.Name.Contains("id") || prop.Name.Contains("Id")) && (prop.PropertyType.ToString() == "System.Nullable`1[System.Int32]")) continue;
                Grid.Children.Add(l);
                switch (prop.PropertyType.ToString())
                {
                    case "System.Nullable`1[System.String]":
                    case "System.String":
                        TextBox str = new TextBox();
                        str.Width = 260;
                        str.Height = 25;
                        Grid.Children.Add(str);
                        inputs.Add(str);
                        types.Add(prop.PropertyType.ToString());
                        break;
                    case "System.Nullable`1[System.Int32]":
                    case "System.Int32":
                        TextBox intt = new TextBox();
                        intt.Width = 260;
                        intt.Height = 25;
                        Grid.Children.Add(intt);
                        inputs.Add(intt);
                        types.Add(prop.PropertyType.ToString());

                        break;
                    case "System.Nullable`1[System.Boolean]":
                    case "System.Boolean":
                        TextBox bol = new TextBox();
                        bol.Width = 260;
                        bol.Height = 25;
                        Grid.Children.Add(bol);
                        inputs.Add(bol);
                        types.Add(prop.PropertyType.ToString());

                        break;
                    case "System.Nullable`1[System.Double]":
                    case "System.Double":
                        TextBox dou = new TextBox();
                        dou.Width = 260;
                        dou.Height = 25;
                        Grid.Children.Add(dou);
                        inputs.Add(dou);
                        types.Add(prop.PropertyType.ToString());

                        break;
                    case "System.Nullable`1[System.DateTime]":
                    case "System.DateTime":
                        DatePicker dat = new DatePicker();
                        //TextBox dat = new TextBox();
                        dat.Width = 260;
                        dat.Height = 25;
                        Grid.Children.Add(dat);
                        inputs.Add(dat);
                        types.Add(prop.PropertyType.ToString());

                        break;
                }

            }
        }

        private readonly string salt = "972db1d5-5b7f-43f6-ae66-a610e71c78af";

        private string saltPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 512 / 8
                ));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (tableName)
            {
                case "Acquisitions":
                    Acquisition acq = new Acquisition();
                    acq.Component = (inputs.ToArray()[0] as TextBox).Text;
                    double v;
                    if (!Double.TryParse((inputs.ToArray()[1] as TextBox).Text, out v))
                    {
                        return;
                    }
                    acq.Price = v;
                    int i;
                    if (!int.TryParse((inputs.ToArray()[2] as TextBox).Text, out i))
                    {
                        return;
                    }
                    acq.Amount = i;
                    Utils.createEntry(acq, tableName);
                    break;
                case "Clients":
                    Client client = new Client();
                    client.Login = (inputs.ToArray()[0] as TextBox).Text;
                    client.Password =saltPassword( (inputs.ToArray()[1] as TextBox).Text);
                    client.Deleted = ((inputs.ToArray()[2] as TextBox).Text.ToLower() == "false")?false:true;
                    Utils.createEntry(client, tableName);
                    break;
                case "Components":
                    Component component = new Component();
                    component.ArticleNum = (inputs.ToArray()[0] as TextBox).Text;
                    int type;
                    if (!int.TryParse((inputs.ToArray()[1] as TextBox).Text, out type))
                    {
                        return;
                    }
                    component.Type = type;
                    int price;
                    if (!int.TryParse((inputs.ToArray()[2] as TextBox).Text, out price))
                    {
                        return;
                    }
                    Utils.createEntry(component, tableName);
                    break;
                case "ComponentTypes":
                    ComponentType type1 = new ComponentType();
                    type1.TypeName = (inputs.ToArray()[0] as TextBox).Text;
                    Utils.createEntry(type1, tableName);
                    break;
                case "ComponentUsages":
                    ComponentUsage usage = new ComponentUsage();
                    usage.Component = (inputs.ToArray()[0] as TextBox).Text;
                    int ord;
                    if (!int.TryParse((inputs.ToArray()[1] as TextBox).Text, out ord))
                    {
                        return;
                    }
                    usage.Order = ord;
                    Utils.createEntry(usage, tableName);
                    break;
                case "Departments":
                    Department dep = new Department();
                    dep.DepName = (inputs.ToArray()[0] as TextBox).Text;
                    Utils.createEntry(dep, tableName);

                    break;
                case "Employees":
                    Employee emp = new Employee();
                    emp.ServiceId = (inputs.ToArray()[0] as TextBox).Text;
                    emp.FirstName = (inputs.ToArray()[1] as TextBox).Text;
                    emp.SecondName = (inputs.ToArray()[2] as TextBox).Text;
                    emp.Login = (inputs.ToArray()[3] as TextBox).Text;
                    int depa;
                    if (!int.TryParse((inputs.ToArray()[4] as TextBox).Text, out depa))
                    {
                        return;
                    }
                    emp.Department = depa;
                    emp.Deleted = ((inputs.ToArray()[5] as TextBox).Text.ToLower() == "false") ? false : true;
                    Utils.createEntry(emp, tableName);
                    break;
                case "Issues":
                    Issue iss = new Issue();
                    int stat;
                    if (!int.TryParse((inputs.ToArray()[0] as TextBox).Text, out stat))
                    {

                        return;
                    }
                    iss.IssStatus = stat;
                    iss.IssAuthor = (inputs.ToArray()[1] as TextBox).Text;
                    iss.IssAssistant = (inputs.ToArray()[2] as TextBox).Text;
                    Utils.createEntry(iss, tableName);
                    break;
                case "IssueStatus":
                    MessageBox.Show("Данная таблица не подлежит изменению");
                    break;
                case "Orders":
                    Order order = new Order();
                    order.Type = (inputs.ToArray()[0] as TextBox).Text;
                    order.Client = (inputs.ToArray()[1] as TextBox).Text;
                    order.Employee = (inputs.ToArray()[2] as TextBox).Text;
                    double pr;
                    if (!Double.TryParse((inputs.ToArray()[3] as TextBox).Text, out pr))
                    { 
                        return;
                    }
                    order.Price = pr;
                    int statu;
                    if (!int.TryParse((inputs.ToArray()[4] as TextBox).Text, out statu))
                    {
                        return;
                    }
                    order.Status = statu;
                    Utils.createEntry(order, tableName);
                    break;
                case "OrderHistories":
                    OrderHistory ordHist = new OrderHistory();
                    int hio;
                    if (!int.TryParse((inputs.ToArray()[0] as TextBox).Text, out hio))
                    {
                        return;
                    }
                    ordHist.HistOrder = hio;
                    ordHist.HistClient = (inputs.ToArray()[1] as TextBox).Text;
                    ordHist.HistDate = (inputs.ToArray()[2] as DatePicker).DisplayDate;
                    Utils.createEntry(ordHist, tableName);
                    break;
                case "OrderStatus":
                    MessageBox.Show("Таблица не подлежит редактированию");
                    break;
                case "OrderTypes":
                    MessageBox.Show("Таблица не подлежит редактированию");

                    break;
                case "Reviews":
                    Review review = new Review();
                    review.RevText = (inputs.ToArray()[0] as TextBox).Text;
                    review.RevAuthor = (inputs.ToArray()[1] as TextBox).Text;
                    Utils.createEntry(review, tableName);
                    break;
                case "Vacancies":
                    Vacancy vac = new Vacancy();
                    int d;
                    if (!int.TryParse((inputs.ToArray()[0] as TextBox).Text, out d))
                    {
                        return;
                    }
                    vac.VacDepartment = d;
                    double sal;
                    if (!Double.TryParse((inputs.ToArray()[1] as TextBox).Text, out sal))
                    {
                        return;
                    }
                    vac.VacSalary = sal;
                    vac.VacComment = (inputs.ToArray()[2] as TextBox).Text;
                    Utils.createEntry(vac, tableName);
                    break;
            }
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utils.loadWindowState(this);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Utils.saveWindowState(Title, this.Left, this.Top);

        }
    }
}
