using CarWash.Entities;
using CarWash.Tools;
using CarWash.Tools.Models;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Word = Microsoft.Office.Interop.Word;

namespace CarWash.Tools.ReportsGeneration
{
    internal static class GenerationsOfReports
    {
        #region Check
        public static async System.Threading.Tasks.Task DoACheckAsync(OrderModel order,
            FrameworkElement ControlToDisable,
            FrameworkElement ControlToVisibleOff,
            FrameworkElement ControlToVisibleOn)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "Microsoft Word Document (*.docx)|*.docx";
            if (sv.ShowDialog() == true)
            {
                ControlToDisable.IsEnabled = false;
                ControlToVisibleOff.Visibility = Visibility.Collapsed;
                ControlToVisibleOn.Visibility = Visibility.Visible;
                await System.Threading.Tasks.Task.Run(() => GenerateCheck(order, sv.FileName));
                ControlToVisibleOff.Visibility = Visibility.Visible;
                ControlToVisibleOn.Visibility = Visibility.Collapsed;
                MessageBox.Show("Генерация чека завершена!");
                ControlToDisable.IsEnabled = true;
            }
        }
        private static void GenerateCheck(OrderModel order, string FilePath)
        {
            try
            {
                Word.Application App = new Word.Application();
                App.Visible = false;
                Word.Document document;
                try
                {
                    document = App.Documents.Open(Directories.Check());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Шаблонный файл для генерации чека не найден!");
                    App.Quit();
                    return;
                }

                ChangeWordsInCheck(order, document);
                ChangeTableInCheck(order, document);

                document.SaveAs2(FileName: FilePath);
                document.Close();
                App.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void ChangeTableInCheck(OrderModel order, Document document)
        {
            Table table = document.Tables[1];
            int CountOfServices = order.Services.Count;
            for (int i = 1; i <= CountOfServices; i++)
                table.Rows.Add();
            int index = 0;
            foreach (Row row in table.Rows)
            {
                if (row.Index == 1)
                    continue;
                foreach (Cell cell in row.Cells)
                {
                    if (cell.ColumnIndex == 2)
                        cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                    _ = cell.ColumnIndex == 1 ? cell.Range.Text = $"{order.Services[index].ServiceName}" :
                        cell.Range.Text = $"{order.Services[index].ServicePrice}";
                    cell.Range.Font.Reset();
                    cell.Range.Font.Size = 14;
                }
                index++;
            }
            ToolsForGeneration.AutoSizeColumn(table, 1);
        }
        private static void ChangeWordsInCheck(OrderModel order, Word.Document document)
        {
            ToolsForGeneration.ReplaceWord("{IdOrder}", $"{order.Id}", document);
            ToolsForGeneration.ReplaceWordWithUnderline("{Client}", $"{order.Client}", document);
            ToolsForGeneration.ReplaceWordWithUnderline("{ClientPhone}", $"{order.ClientPhone}", document);
            ToolsForGeneration.ReplaceWordWithUnderline("{ClientCar}", $"{order.Car}", document);
            ToolsForGeneration.ReplaceWord("{TotalPrice}", $"{order.TotalPriceOfOrder} руб.", document);
            ToolsForGeneration.ReplaceWord("{DateTimeOfOrder}", $"{order.DateTimeOfOrder:g}", document);
            ToolsForGeneration.ReplaceWord("{CurrentDateTime}", $"{DateTime.Now:g}", document);
            ToolsForGeneration.ReplaceWord("{EmployeeName}", $"{order.Employee}", document);
        }
        #endregion

        #region WorkReport
        public static async System.Threading.Tasks.Task DoAWorkReportAsync(WorkReportModel Data, //Sales
            FrameworkElement ControlToDisable,
            FrameworkElement ControlToVisible)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "Microsoft Word Document (*.docx)|*.docx";
            if (sv.ShowDialog() == true)
            {
                ControlToDisable.IsEnabled = false;
                ControlToVisible.Visibility = Visibility.Visible;
                await System.Threading.Tasks.Task.Run(() => GenerateWorkReport(Data, sv.FileName));
                MessageBox.Show("Генерация отчёта завершена!");
                ControlToDisable.IsEnabled = true;
                ControlToVisible.Visibility = Visibility.Hidden;
            }
        }
        private static void GenerateWorkReport(WorkReportModel data, string fileName)
        {
            try
            {
                Word.Application App = new Word.Application();
                App.Visible = false;

                Word.Document document;
                try
                {
                    document = App.Documents.Open(Directories.WorkReport());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Шаблонный файл для генерации отчёта по работам не найден!");
                    App.Quit();
                    return;
                }

                ChangeFieldsWorkReport(data, document);

                DoATableWorkReport(data, document);

                document.SaveAs2(FileName: fileName);
                document.Close();
                App.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void DoATableWorkReport(WorkReportModel data, Document document)
        {
            Table table = document.Tables[1];
            int CountOfServices = data.Orders.Count;
            for (int i = 1; i <= CountOfServices; i++)
                table.Rows.Add();
            int index = 0;
            foreach (Row row in table.Rows)
            {
                if (row.Index == 1)
                    continue;
                foreach (Cell cell in row.Cells)
                {
                    if (cell.ColumnIndex == 6)
                        cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                    _ = cell.ColumnIndex == 1 ? cell.Range.Text = data.Orders[index].Client :
                        cell.ColumnIndex == 2 ? cell.Range.Text = data.Orders[index].Car :
                        cell.ColumnIndex == 3 ? cell.Range.Text = data.Orders[index].Employee :
                        cell.ColumnIndex == 4 ? cell.Range.Text = string.Join(", ", data.Orders[index].Services.Select(s => s.ServiceName)) :
                        cell.ColumnIndex == 5 ? cell.Range.Text = $"{data.Orders[index].DateTimeOfOrder:g}" :
                        cell.Range.Text = $"{data.Orders[index].TotalPriceOfOrder:N2}";
                }
                index++;
            }
            ToolsForGeneration.AutoSizeColumn(table, 4);
        }
        private static void ChangeFieldsWorkReport(WorkReportModel data, Document document)
        {
            ToolsForGeneration.ReplaceWord("{AllTotalCost}", $"{data.TotalCostOfReport:N2} руб.", document);
            ToolsForGeneration.ReplaceWord("{StartDate}", $"{data.StartDateOfReport:d}", document);
            ToolsForGeneration.ReplaceWord("{EndDate}", $"{data.EndDateOfReport:d}", document);
            ToolsForGeneration.ReplaceWord("{CurrentDate}", $"{DateTime.Now:g}", document);
        }
        #endregion
        #region EmployeeReport
        public static async System.Threading.Tasks.Task DoAEmployeeReportAsync(EmployeeReportModel Data,
            FrameworkElement ControlToDisable,
            FrameworkElement ControlToVisible)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "Microsoft Word Document (*.docx)|*.docx";
            if (sv.ShowDialog() == true)
            {
                ControlToDisable.IsEnabled = false;
                ControlToVisible.Visibility = Visibility.Visible;
                await System.Threading.Tasks.Task.Run(() => GenerateEmployeeReport(Data, sv.FileName));
                MessageBox.Show("Генерация отчёта завершена!");
                ControlToDisable.IsEnabled = true;
                ControlToVisible.Visibility = Visibility.Hidden;
            }
        }
        private static void GenerateEmployeeReport(EmployeeReportModel Data, string filePath)
        {
            try
            {
                Word.Application App = new Word.Application();
                App.Visible = false;

                Word.Document document;
                try
                {
                    document = App.Documents.Open(Directories.EmployeeReport());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Шаблонный файл для генерации отчёта по сотрудникам не найден!");
                    App.Quit();
                    return;
                }

                ChangeFieldsEmployeeReport(Data, document);

                DoATableEmployeeReport(Data, document);

                document.SaveAs2(FileName: filePath);
                document.Close();
                App.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void DoATableEmployeeReport(EmployeeReportModel data, Document document)
        {
            Table table = document.Tables[1];
            int CountOfWorks = data.Orders.Count;
            for (int i = 1; i <= CountOfWorks; i++)
                table.Rows.Add();
            int index = 0;
            foreach (Row row in table.Rows)
            {
                if (row.Index == 1)
                    continue;
                foreach (Cell cell in row.Cells)
                {
                    if (cell.ColumnIndex == 5)
                        cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                    _ = cell.ColumnIndex == 1 ? cell.Range.Text = $"{data.Orders[index].Id}" :
                        cell.ColumnIndex == 2 ? cell.Range.Text = $"{data.Orders[index].Car}" :
                        cell.ColumnIndex == 3 ? cell.Range.Text = $"{data.Orders[index].DateTimeOfOrder:g}" :
                        cell.ColumnIndex == 4 ? cell.Range.Text = string.Join('\n', data.Orders[index].Services.Select(s => s.ServiceName)) :
                        cell.Range.Text = $"{data.Orders[index].TotalPriceOfOrder:N2}";
                }
                index++;
            }
            ToolsForGeneration.AutoSizeColumn(table, 4);
        }
        private static void ChangeFieldsEmployeeReport(EmployeeReportModel data, Document document)
        {
            ToolsForGeneration.ReplaceWordWithBold("{EmployeeId}", $"{data.Id}", document);
            ToolsForGeneration.ReplaceWordWithBold("{EmployeeName}", $"{data.Employee}", document);
            ToolsForGeneration.ReplaceWord("{AllTotalCost}", $"{data.TotalCostOfWorks} руб.", document);
            ToolsForGeneration.ReplaceWord("{StartDate}", $"{data.StartDateOfReport:d}", document);
            ToolsForGeneration.ReplaceWord("{EndDate}", $"{data.EndDateOfReport:d}", document);
            ToolsForGeneration.ReplaceWord("{CurrentDate}", $"{DateTime.Now:g}", document);
        }
        #endregion
    }
}