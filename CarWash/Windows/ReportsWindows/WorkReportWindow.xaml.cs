using CarWash.Tools;
using CarWash.Tools.ReportsGeneration;
using System;
using System.Windows;
using System.Windows.Threading;

namespace CarWash.Windows.ReportsWindows
{
    public partial class WorkReportWindow : Window
    {
        DispatcherTimer timer;
        public WorkReportWindow()
        {
            InitializeComponent();

            SettingTimer();
        }
        private void SettingTimer()
        {
            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += (sender, e) => ChangeGenerationLoadingText();
            timer.Start();
        }
        private void ChangeGenerationLoadingText()
        {
            if (GenerationLoadingTB.Text == "Отчёт генерируется...")
                GenerationLoadingTB.Text = "Отчёт генерируется.";
            if (GenerationLoadingTB.Text == "Отчёт генерируется.")
                GenerationLoadingTB.Text = "Отчёт генерируется..";
            if (GenerationLoadingTB.Text == "Отчёт генерируется..")
                GenerationLoadingTB.Text = "Отчёт генерируется...";
        }
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!StartDateOfReport.SelectedDate.HasValue)
            {
                MessageBox.Show("Необходимо выбрать начальную дату диапазона выполненных работ!");
                return;
            }
            if (!EndDateOfReport.SelectedDate.HasValue)
            {
                MessageBox.Show("Необходимо выбрать конечную дату диапазона выполненных работ!");
                return;
            }
            if (EndDateOfReport.SelectedDate.Value < StartDateOfReport.SelectedDate.Value)
            {
                MessageBox.Show("Начальная дата генерации не может быть больше даты окончания генерации!");
                return;
            }

            DateTime StartDate = new DateTime(StartDateOfReport.SelectedDate.Value.Year,
                StartDateOfReport.SelectedDate.Value.Month, StartDateOfReport.SelectedDate.Value.Day,
                0, 0, 0);

            DateTime EndDate = new DateTime(EndDateOfReport.SelectedDate.Value.Year,
                EndDateOfReport.SelectedDate.Value.Month, EndDateOfReport.SelectedDate.Value.Day,
                23, 59, 59);

            _ = GenerationsOfReports
                .DoAWorkReportAsync(new Tools.Models.WorkReportModel(DbUtils.
                    GetOrderModelsByDates(StartDate,
                        EndDate,
                        out decimal TotalCostOfOrder),
                    TotalCostOfOrder,
                    StartDate,
                    EndDate),
                MainGrid,
                LoadingGrid);
        }

    }
}
