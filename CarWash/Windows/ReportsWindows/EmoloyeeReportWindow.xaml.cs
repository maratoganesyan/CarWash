using CarWash.Entities;
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
    /// Interaction logic for EmoloyeeReportWindow.xaml
    /// </summary>
    public partial class EmoloyeeReportWindow : Window
    {
        public EmoloyeeReportWindow()
        {
            InitializeComponent();

            EmployeeCB.ItemsSource = DbUtils.db.Employee.Where(e => e.IdRoleNavigation.RoleName == "Мойщик").ToList();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeCB.SelectedIndex == -1)
            {
                MessageBox.Show("Сотрудник для генерации отчёта не выбран!");
                return;
            }
            Employee emp = EmployeeCB.SelectedItem as Employee;
            DateTime StartDate;
            DateTime EndDate;
            if (StartDateOfReport.SelectedDate.HasValue && EndDateOfReport.SelectedDate.HasValue)
            {
                StartDate = new DateTime(StartDateOfReport.SelectedDate.Value.Year, StartDateOfReport.SelectedDate.Value.Month,
                    StartDateOfReport.SelectedDate.Value.Day, 0, 0, 0);
                EndDate = new DateTime(EndDateOfReport.SelectedDate.Value.Year, EndDateOfReport.SelectedDate.Value.Month,
                    EndDateOfReport.SelectedDate.Value.Day, 23, 59, 59);
            }
            else
                (StartDate, EndDate) = DbUtils.GetTheDeadlineDates(emp);
            _ = GenerationsOfReports
                .DoAEmployeeReportAsync(new Tools.Models.EmployeeReportModel(emp,
                                        DbUtils.GetEmployeeOrderByDate(emp, StartDate, EndDate),
                                        StartDate,
                                        EndDate),
                MainGrid,
                LoadingGrid);
        }

    }
}
