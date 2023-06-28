using CarWash.Entities;
using CarWash.Tools;
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

namespace CarWash.Windows
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private List<AdditionalServices> ServicesBackUp;
        private List<AdditionalServices> ActualServices;
        private bool IsItChange;
        private readonly Order OrderBackUp;
        public OrderWindow(Order? order)
        {
            InitializeComponent();

            IsItChange = order == null ? false : true;
            OrderBackUp = order ?? null;

            AssignServicesLists(order);
            ConfigureComboBoxes(order);
            ConfigureFields(order);

        }
        private void AssignServicesLists(Order? order)
        {
            ServicesBackUp = order == null ? new List<AdditionalServices>() :
                order.ServicesInOrder.Select(s => s.IdServiceNavigation).ToList();
            ActualServices = order == null ? new List<AdditionalServices>() :
                order.ServicesInOrder.Select(s => s.IdServiceNavigation).ToList();
        }
        private void ConfigureFields(Order? order)
        {
            ThisWindow.Title = "Создание нового заказа";
            if (order != null)
            {
                ThisWindow.Title = "Изменение существующего заказа";
                UpdateServicesDataGrid();
                OrderDatePicker.Text = order.DateTimeOfOrder.Date.ToString();
                HourOrderPicker.Text = $"{order.DateTimeOfOrder:HH}";
                MinutesOrderPicker.Text = $"{order.DateTimeOfOrder:mm}";
            }
        }
        private void ConfigureComboBoxes(Order? order)
        {
            AddServiceComboBox.ItemsSource = DbUtils.db.AdditionalServices.ToList();
            UpdateReplacementServiceComboBox();
            EmployeeComboBox.ItemsSource = DbUtils.db.Employee.Where(e => e.IdRoleNavigation.RoleName == "Мойщик").ToList();
            ClientComboBox.ItemsSource = DbUtils.db.Clients.ToList();
            StatusComboBox.ItemsSource = DbUtils.db.OrderStatus.ToList();
            if (order != null)
            {
                UpdateReplaceableServiceComboBox();
                EmployeeComboBox.SelectedItem = order.IdEmployeeNavigation;
                ClientComboBox.SelectedItem = order.IdC.IdClientNavigation;
                ClientCarComboBox.SelectedItem = order.IdC.IdCarNavigation;
                StatusComboBox.SelectedItem = order.IdOrderStatusNavigation;
            }
        }
        private void UpdateServicesDataGrid()
        {
            AdditationalServicesDataGrid.ItemsSource = null;
            AdditationalServicesDataGrid.ItemsSource = ActualServices.Select(s => new
            {
                ServiceName = s.ServiceName,
                ServicePrice = $"{s.ServicePrice:N2}",
                ServiceDescription = s.ServiceDescription
            });
        }
        private void UpdateReplaceableServiceComboBox()
        {
            ReplaceableServiceComboBox.ItemsSource = null;
            ReplaceableServiceComboBox.ItemsSource = ActualServices;
        }
        private void UpdateReplacementServiceComboBox()
        {
            ReplacementServiceComboBox.ItemsSource = null;
            ReplacementServiceComboBox.ItemsSource = DbUtils
                .db
                .AdditionalServices
                .ToList()
                .Where(s => ActualServices.All(ac => ac != s))
                .ToList();
        }
        private void UpdateAllControls()
        {
            UpdateServicesDataGrid();
            UpdateReplaceableServiceComboBox();
            UpdateReplacementServiceComboBox();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAll())
                return;
            Order order;
            if (IsItChange)
                order = OrderBackUp;
            else
                order = new Order();
            ClientsCars cc = DbUtils
                .db
                .ClientsCars
                .Single(cars => cars.IdClientNavigation == (ClientComboBox.SelectedItem as Clients) &&
                cars.IdCarNavigation == (ClientCarComboBox.SelectedItem as Car));
            order.IdClient = cc.IdClient;
            order.IdCar = cc.IdCar;
            order.IdEmployee = (EmployeeComboBox.SelectedItem as Employee).IdEmployee;
            order.DateTimeOfOrder = new DateTime(OrderDatePicker.SelectedDate.Value.Year,
                OrderDatePicker.SelectedDate.Value.Month, OrderDatePicker.SelectedDate.Value.Day,
                int.Parse(HourOrderPicker.Text), int.Parse(MinutesOrderPicker.Text), 0);
            order.TotalPriceOfOrder = ActualServices.Select(s => s.ServicePrice).Sum();
            order.IdOrderStatus = (StatusComboBox.SelectedItem as OrderStatus).IdOrderStatus;
            if (!IsItChange)
                DbUtils.db.Order.Add(order);
            DbUtils.db.SaveChanges();
            int IdOrder = !IsItChange ? DbUtils.db.Order.OrderBy(o => o.IdOrder).Last().IdOrder : OrderBackUp.IdOrder;
            UpdateServicesInOrder(IdOrder);
            MessageBox.Show("Заказ успешно сохранён!");
            Close();
        }
        private void UpdateServicesInOrder(int idOrder)
        {
            if (!IsItChange)
            {
                foreach (AdditionalServices service in ActualServices)
                {
                    DbUtils
                        .db
                        .ServicesInOrder
                        .Add(new ServicesInOrder() { IdOrder = idOrder, IdService = service.IdService });
                    DbUtils.db.SaveChanges();
                }
            }
            else
            {
                DeleteAllServices(idOrder);

                foreach (AdditionalServices service in ActualServices)
                {
                    DbUtils
                        .db
                        .ServicesInOrder
                        .Add(new ServicesInOrder()
                        {
                            IdOrder = idOrder,
                            IdService = service.IdService
                        });
                }
                DbUtils.db.SaveChanges();
            }
        }
        private void DeleteAllServices(int idOrder)
        {
            foreach (ServicesInOrder service in DbUtils
                .db
                .ServicesInOrder
                .Where(s => s.IdOrder == idOrder)
                .ToList())
            {
                DbUtils.db.ServicesInOrder.Remove(service);
            }
            DbUtils.db.SaveChanges();
        }
        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddServiceComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Услуа для добавления не выбрана!");
                return;
            }
            AdditionalServices service = AddServiceComboBox.SelectedItem as AdditionalServices;
            if (ActualServices.Any(s => s == service))
            {
                MessageBox.Show("Данная услуга уже добавлена в список услуг заказа!");
                return;
            }
            ActualServices.Add(service);
            UpdateAllControls();
        }
        private void ReplaceServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReplaceableServiceComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Заменяемая услуга не выбрана!");
                return;
            }
            if (ReplacementServiceComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Заменяющая услуга не выбрана!");
                return;
            }
            AdditionalServices replaceable = ReplaceableServiceComboBox.SelectedItem as AdditionalServices;
            AdditionalServices replacement = ReplacementServiceComboBox.SelectedItem as AdditionalServices;
            int index = ActualServices.IndexOf(replaceable);
            ActualServices[index] = replacement;
            UpdateAllControls();
        }
        private void ClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Клиент заказа не выбран!");
                return;
            }
            Clients c = ClientComboBox.SelectedItem as Clients;
            ClientCarComboBox.ItemsSource = c.ClientsCars.Select(cars => cars.IdCarNavigation).ToList();
        }
        #region Validation
        private bool ValidateAll() => ValidateRightSide();
        private bool ValidateRightSide()
        {
            if (EmployeeComboBox.SelectedIndex == -1)
                ReturnFalseWithError("Сотруник заказа не выбран!");
            if (ClientComboBox.SelectedIndex == -1)
                ReturnFalseWithError("Клиент заказа не выбран!");
            if (ClientCarComboBox.SelectedIndex == -1)
                ReturnFalseWithError("Автомобиль заказа не выбран!");
            if (!OrderDatePicker.SelectedDate.HasValue)
                ReturnFalseWithError("Дата заказа не выбрана!");
            if (!ValidateHourTB())
                return false;
            if (!ValidateMinuteTB())
                return false;
            if (StatusComboBox.SelectedIndex == -1)
                ReturnFalseWithError("Статус заказа не выбран!");
            return true;
        }
        private bool ValidateHourTB()
        {
            if (!int.TryParse(HourOrderPicker.Text, out int Hour))
            {
                MessageBox.Show("Введённое число часов должно представлять собой целое значение!");
                return false;
            }
            if (((0 <= Hour) && (Hour <= 23)) == false)
            {
                MessageBox.Show("Введённое число часов не подходит под общепринятый диапазон доступных значений!");
                return false;
            }
            return true;
        }
        private bool ValidateMinuteTB()
        {
            if (!int.TryParse(MinutesOrderPicker.Text, out int Minutes))
            {
                MessageBox.Show("Введённое число минут должно представлять собой целое значение!");
                return false;
            }
            if (((0 <= Minutes) && (Minutes <= 59)) == false)
            {
                MessageBox.Show("Введённое число минут не подходит под общепринятый диапазон доступных значений!");
                return false;
            }
            return true;
        }

        private bool ReturnFalseWithError(string s)
        {
            MessageBox.Show(s);
            return false;
        }
        #endregion
        private void AdditationalServicesDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            foreach (var col in AdditationalServicesDataGrid.Columns)
            {
                col.Header = Translator.Translate(col.Header.ToString());
            }
        }
    }
}
