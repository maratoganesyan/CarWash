using CarWash.Entities;
using CarWash.Tools;
using CarWash.Windows.ReportsWindows;
using Microsoft.Data.SqlClient;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarWash.Windows.ReportsWindows;

namespace CarWash.Windows
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {

        string TableName;

        bool IsDirectRequest;

        NavigationViewItem PreviewItem;

        bool NavigationDisplayModeSmall;

        int IdUserRole;

        readonly string ConnectionString;

        private void FillDirector()
        {
            Car.Visibility = Visibility.Collapsed;
            Order.Visibility = Visibility.Collapsed;
            spr.Visibility = Visibility.Collapsed;
        }

        private void FillWacher()
        {
            spr.Visibility = Visibility.Collapsed;
            Employee.Visibility = Visibility.Collapsed;
            WorkerReportButton.Visibility = Visibility.Collapsed;
            WorkReportButton.Visibility = Visibility.Collapsed;
        }
        public EmployeeWindow(Employee employee)
        {
            InitializeComponent();
            try
            {
                ConnectionString = File.ReadAllText(Directories.ConnectionString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных! Проверьте подключение!");
                Application.Current.Shutdown();
            }
            NavigationDisplayModeSmall = true;
            if (employee.IdRole == DbUtils.db.Role.Single(r => r.RoleName == "Мойщик").IdRole)
                FillWacher();
            if (employee.IdRole == DbUtils.db.Role.Single(r => r.RoleName == "Директор").IdRole)
                FillDirector();
            IdUserRole = employee.IdRole;
            EmployeeData.Text = employee.Surname + " " + employee.Name + " (" + employee.IdRoleNavigation.RoleName + ")";
        }

        private void Spr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = sender as NavigationViewItem;
                TableName = item.Name;
                if (TableName == "Models")
                {
                    MainDataGrid.ItemsSource = DbUtils.db.Models.Select(m => new
                    {
                        m.IdModel,
                        m.ModelName,
                        m.IdMarkNavigation.MarkName
                    }).ToList();
                    IsDirectRequest = false;
                }
                else
                {
                    using (var connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        string cmd = $"SELECT * FROM {TableName}";

                        SqlCommand command = new SqlCommand(cmd, connection);
                        command.ExecuteNonQuery();
                        SqlDataAdapter dataAdp = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        dataAdp.Fill(dt);
                        MainDataGrid.ItemsSource = dt.DefaultView;
                        connection.Close();
                    }
                    IsDirectRequest = true;
                }
                PreviewItem = item;

                SearchTextBox.IsEnabled = true;
                foreach (var col in MainDataGrid.Columns)
                {
                    col.Header = Translator.Translate(col.Header.ToString());
                }

                MainDataGrid.Columns[0].MaxWidth = 0;
            }
            catch
            {
                MessageBox.Show("error");
            }
        }

        private void Clients_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainDataGrid.ItemsSource = DbUtils.db.Clients.Select(c => new
            {
                c.IdClient,
                c.Surname,
                c.Name,
                c.Patronymic,
                c.PhoneNumber,
                c.IdGenderNavigation.GenderName,
                Cars = string.Join(Environment.NewLine, c.ClientsCars.Select(cc => cc.IdCarNavigation.IdModelNavigation.IdMarkNavigation.MarkName + " " +
                                                                             cc.IdCarNavigation.IdModelNavigation.ModelName + " " +
                                                                             cc.IdCarNavigation.IdColorNavigation.ColorName + " " +
                                                                             cc.IdCarNavigation.IdBodyNavigation.BodyName).ToList())
            }).ToList();

            PreviewItem = sender as NavigationViewItem;

            SearchTextBox.IsEnabled = true;

            foreach (var col in MainDataGrid.Columns)
            {
                col.Header = Translator.Translate(col.Header.ToString());
            }

            MainDataGrid.Columns[0].MaxWidth = 0;

            IsDirectRequest = false;
        }

        private void Employee_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainDataGrid.ItemsSource = DbUtils.db.Employee.Select(e => new
            {
                e.IdEmployee,
                e.Surname,
                e.Name,
                e.Patronymic,
                e.Email,
                e.PhoneNumber,
                e.Login,
                e.Password,
                e.IdGenderNavigation.GenderName,
                e.IdRoleNavigation.RoleName
            }).ToList();

            PreviewItem = sender as NavigationViewItem;

            SearchTextBox.IsEnabled = true;

            foreach (var col in MainDataGrid.Columns)
            {
                col.Header = Translator.Translate(col.Header.ToString());
            }

            MainDataGrid.Columns[0].MaxWidth = 0;

            IsDirectRequest = false;
        }

        private void Order_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainDataGrid.ItemsSource = DbUtils.db.Order.Select(o => new
            {
                o.IdOrder,
                Client = $"{o.IdC.IdClientNavigation.Surname} {o.IdC.IdClientNavigation.Name} {o.IdC.IdClientNavigation.Patronymic} {o.IdC.IdClientNavigation.PhoneNumber}",
                Employee = $"{o.IdEmployeeNavigation.Surname} {o.IdEmployeeNavigation.Name} {o.IdEmployeeNavigation.PhoneNumber}",

                Car = $"{o.IdC.IdCarNavigation.IdModelNavigation.IdMarkNavigation.MarkName}" +
                $" {o.IdC.IdCarNavigation.IdModelNavigation.ModelName} {o.IdC.IdCarNavigation.IdColorNavigation.ColorName} {o.IdC.IdCarNavigation.IdBodyNavigation.BodyName}",

                Services = string.Join(Environment.NewLine, o.ServicesInOrder.Select(s => s.IdServiceNavigation.ServiceName + " " + s.IdServiceNavigation.ServicePrice + " рублей").ToList()),

                o.IdOrderStatusNavigation.OrderStatusName,
                DateTimeOfOrder = $"{o.DateTimeOfOrder:D}",
                o.TotalPriceOfOrder
            }).ToList();

            PreviewItem = sender as NavigationViewItem;

            SearchTextBox.IsEnabled = true;

            foreach (var col in MainDataGrid.Columns)
            {
                col.Header = Translator.Translate(col.Header.ToString());
            }

            MainDataGrid.Columns[0].MaxWidth = 0;

            IsDirectRequest = false;
        }

        private void Car_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainDataGrid.ItemsSource = DbUtils.db.Car.Select(c => new
            {
                c.IdCar,
                c.IdModelNavigation.IdMarkNavigation.MarkName,
                c.IdModelNavigation.ModelName,
                c.IdColorNavigation.ColorName,
                c.IdBodyNavigation.BodyName,
                c.Description,
                c.Width,
                c.Length,
                c.Height,
                Client = c.ClientsCars.Single().IdClientNavigation.Surname + " " + c.ClientsCars.Single().IdClientNavigation.Name + " "
                         + c.ClientsCars.Single().IdClientNavigation.Patronymic + " " + c.ClientsCars.Single().IdClientNavigation.PhoneNumber
            }).ToList();

            PreviewItem = sender as NavigationViewItem;

            SearchTextBox.IsEnabled = true;

            foreach (var col in MainDataGrid.Columns)
            {
                col.Header = Translator.Translate(col.Header.ToString());
            }

            MainDataGrid.Columns[0].MaxWidth = 0;

            IsDirectRequest = false;
        }

        private void MainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отредактировать данную запись?", "Предупреждение",
                                                   MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (PreviewItem == null)
                    {
                        MessageBox.Show("Сущность для манипулирования данными не выбрана!");
                        return;
                    }
                    try
                    {
                        switch (PreviewItem.Name)
                        {
                            case "Car":
                                {
                                    var cellInfo = MainDataGrid.SelectedCells[0];
                                    int content = Convert.ToInt16((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);
                                    Car car = DbUtils.db.Car.Single(c => c.IdCar == content);
                                    CarsAddAndChange window = new CarsAddAndChange(car);
                                    window.ShowDialog();
                                    break;
                                }
                            case "Employee":
                                {
                                    var cellInfo = MainDataGrid.SelectedCells[0];
                                    int content = Convert.ToInt16((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);
                                    Employee employee = DbUtils.db.Employee.Single(c => c.IdEmployee == content);
                                    EmployeeAddAndChange window = new EmployeeAddAndChange(employee);
                                    window.ShowDialog();
                                    break;
                                }
                            case "Clients":
                                {
                                    var cellInfo = MainDataGrid.SelectedCells[0];
                                    int content = Convert.ToInt16((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);
                                    Clients client = DbUtils.db.Clients.Single(c => c.IdClient == content);
                                    ClientsAddAndChange window = new ClientsAddAndChange(client);
                                    window.ShowDialog();
                                    break;
                                }
                            case "Order":
                                {
                                    var cellInfo = MainDataGrid.SelectedCells[0];
                                    int content = Convert.ToInt16((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);
                                    Order order = DbUtils.db.Order.Single(o => o.IdOrder == content);
                                    OrderWindow window = new OrderWindow(order);
                                    window.ShowDialog();
                                    break;
                                }
                            default:
                                {
                                    var cellInfo = MainDataGrid.SelectedCells[0];
                                    int content = Convert.ToInt16((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);
                                    SprAddAndChange window = new SprAddAndChange(PreviewItem.Name, true, content);
                                    window.ShowDialog();
                                    break;
                                }
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("error");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("error");
                }
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PreviewItem == null)
                {
                    MessageBox.Show("Сущность для манипулирования данными не выбрана!");
                    return;
                }
                switch (PreviewItem.Name)
                {

                    case "Car":
                        {
                            CarsAddAndChange window = new CarsAddAndChange(null);
                            window.ShowDialog();
                            break;
                        }
                    case "Employee":
                        {
                            EmployeeAddAndChange window = new EmployeeAddAndChange(null);
                            window.ShowDialog();
                            break;
                        }
                    case "Clients":
                        {
                            ClientsAddAndChange window = new ClientsAddAndChange(null);
                            window.ShowDialog();
                            break;
                        }
                    case "Order":
                        {
                            OrderWindow window = new OrderWindow(null);
                            window.ShowDialog();
                            break;
                        }
                    default:
                        {
                            SprAddAndChange window = new SprAddAndChange(PreviewItem.Name, false, null);
                            window.ShowDialog();
                            break;
                        }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("error");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewItem == null)
            {
                MessageBox.Show("Сущность для манипулирования данными не выбрана!");
                return;
            }
            MouseButtonEventArgs arg = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            arg.RoutedEvent = NavigationViewItem.MouseDownEvent;
            PreviewItem.RaiseEvent(arg);
        }

        private void WorkReportButton_Click(object sender, RoutedEventArgs e)
        {
            WorkReportWindow window = new WorkReportWindow();
            window.Show();
        }

        private void WorkerReportButton_Click(object sender, RoutedEventArgs e)
        {
            EmoloyeeReportWindow window = new EmoloyeeReportWindow();
            window.Show();
        }

        private void CheckGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            CheckGenerationWindow window = new CheckGenerationWindow();
            window.Show();
        }

        private void SearchTextBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            try
            {
                ResetButton_Click(null, null);
                var TextForSearch = SearchTextBox.Text;

                List<object> DataFromTable = MainDataGrid.Items.OfType<object>().ToList();
                var MainList = new List<object>();
                foreach (var item in DataFromTable)
                {
                    string ItemInString;
                    Regex regex = new Regex(@"\{([^\}]*)\}");
                    List<string> Values = new();

                    if (IsDirectRequest)
                    {
                        ItemInString = "{ " + string.Join(", ", (item as DataRowView).Row.ItemArray) + " }";
                        string Inside = regex.Match(ItemInString).Groups[1].Value;
                        List<string> Splitted = Inside.Split(", ").ToList();
                        Splitted.RemoveAt(0);
                        Values = Splitted.Select(s => s.Trim()).ToList();
                    }
                    else
                    {
                        ItemInString = item.ToString();
                        Regex regex2 = new Regex(@"([\s\S]*), [a-zA-Z]*");
                        string Inside = regex.Match(ItemInString).Groups[1].Value;
                        List<string> Splitted = Inside.Split('=').ToList();
                        Splitted.RemoveAt(0);
                        foreach (var str in Splitted)
                        {
                            if (str == Splitted[Splitted.Count - 1])
                                Values.Add(str.Trim());
                            Values.Add(regex2.Match(str).Groups[1].Value.Trim());
                        }
                        Values.RemoveAt(0);
                        Values.RemoveAt(Values.Count - 1);
                    }
                    if (Values.Any(p => p.Contains(TextForSearch)))
                    {
                        MainList.Add(item);
                    }
                }
                MainDataGrid.ItemsSource = null;
                MainDataGrid.ItemsSource = MainList;
                foreach (var col in MainDataGrid.Columns)
                {
                    col.Header = Translator.Translate(col.Header.ToString());
                }
                MainDataGrid.Columns[0].MaxWidth = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Поиск не дал результатов");
            }
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Autorization window = new Autorization();
            window.Show();
            this.Close();
        }
    }
}
