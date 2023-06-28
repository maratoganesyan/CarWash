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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CarWash.Entities;
using CarWash.Windows;

namespace CarWash.Windows;
public partial class Autorization : Window
{
    int AnswerForChecking;
    public Autorization()
    {
        InitializeComponent();
        var temp = DbUtils.db.Employee.ToList();
    }

    public void GenerateContentDialog()
    {
        Number1.Text = new Random().Next(0, 99).ToString();
        Number2.Text = new Random().Next(0, 99).ToString();
        AnswerForChecking = int.Parse(Number1.Text) + int.Parse(Number2.Text);
        Checking.ShowAsync();
    }

    public void UserValidation()
    {
        Employee? employee = DbUtils.db.Employee
                            .SingleOrDefault(u => u.Login == LoginTextBox.Text && u.Password == PasswordTextBox.Password);
        if(employee == null)
        {
            MessageBox.Show("Логин или пароль введены неверно");
        }
        else
        {
            EmployeeWindow window = new EmployeeWindow(employee);
            window.Show();
            this.Close();
        }
    }

    private void AutorizationButton_Click(object sender, RoutedEventArgs e)
    {
        GenerateContentDialog();
    }

    private void GetAnswerButton_Click(object sender, RoutedEventArgs e)
    {
        if(AnswerTextBox.Text == AnswerForChecking.ToString())
        {
            UserValidation();
        }
        else
        {
            MessageBox.Show("Введенное значение неверно");
        }
        AnswerTextBox.Text = string.Empty;
        Checking.Hide();
    }
}
