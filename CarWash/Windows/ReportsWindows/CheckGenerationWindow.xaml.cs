using CarWash.Tools.Models;
using CarWash.Tools.ReportsGeneration;
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

namespace CarWash.Windows.ReportsWindows
{
    /// <summary>
    /// Interaction logic for CheckGenerationWindow.xaml
    /// </summary>
    public partial class CheckGenerationWindow : Window
    {
        public CheckGenerationWindow()
        {
            InitializeComponent();

            OrdersComboBox.ItemsSource = DbUtils
                .db
                .Order
                .Where(o => o.IdOrderStatusNavigation.OrderStatusName == "Выполнен")
                .ToList()
                .Select(o => new OrderModel(o));

        }
        private void CheckGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Сущность для генерации чека не выбрана!");
                return;
            }
            _ = GenerationsOfReports
                .DoACheckAsync(OrdersComboBox.SelectedItem as OrderModel,
                    MainGrid,
                    LogoLeftSide,
                    LoadingLeftSide);
        }

    }
}
