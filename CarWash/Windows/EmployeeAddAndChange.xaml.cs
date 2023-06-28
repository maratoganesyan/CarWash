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
using CarWash.Entities;

namespace CarWash.Windows
{
    /// <summary>
    /// Interaction logic for EmployeeAddAndChange.xaml
    /// </summary>
    public partial class EmployeeAddAndChange : Window
    {
        bool ChangeMode;

        int id;

        public void FillComboBoxes()
        {
            RoleComboBox.ItemsSource = DbUtils.db.Role.ToList();
            GenderComboBox.ItemsSource = DbUtils.db.Gender.ToList();
        }

        public EmployeeAddAndChange(Employee? employee)
        {
            InitializeComponent();
            FillComboBoxes();
            if (employee == null)
            {
                ThisWindow.Title = "Создание нового сотрудника";
                ChangeMode = false;
                RoleComboBox.SelectedIndex = 1;
                GenderComboBox.SelectedIndex = 1;
            }
            else
            {
                ThisWindow.Title = "Изменение существующего сотрудника";
                ChangeMode = true;
                id = employee.IdEmployee;
                SurnameTextBox.Text = employee.Surname;
                NameTextBox.Text = employee.Name;
                PatronymicTextBox.Text = employee.Patronymic;
                PhoneNumberBox.Text = employee.PhoneNumber;
                EmailTextBox.Text = employee.Email;
                LoginTextBox.Text = employee.Login;
                PasswordTextBox.Text = employee.Password;
                RoleComboBox.SelectedItem = employee.IdRoleNavigation;
                GenderComboBox.SelectedItem = employee.IdGenderNavigation;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateAll())
                    return;
                Employee employee;
                if (ChangeMode)
                    employee = DbUtils.db.Employee.Single(e => e.IdEmployee == id);
                else
                    employee = new Employee();

                employee.Surname = SurnameTextBox.Text;
                employee.Name = NameTextBox.Text;
                employee.Patronymic = PatronymicTextBox.Text;
                employee.PhoneNumber = PhoneNumberBox.Text;
                employee.Email = EmailTextBox.Text;
                employee.Login = LoginTextBox.Text;
                employee.Password = PasswordTextBox.Text;
                employee.IdRole = (RoleComboBox.SelectedItem as Role).IdRole;
                employee.IdGender = (GenderComboBox.SelectedItem as Gender).IdGender;

                if (!ChangeMode)
                    DbUtils.db.Employee.Add(employee);
                DbUtils.db.SaveChanges();
                MessageBox.Show("Данные успешно сохранены");
                this.Close();

            }
            catch
            {
                MessageBox.Show("Ошибка сохранения данных");
            }
        }

        #region Validation
        private bool ValidateAll()
        {
            if (SurnameTextBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести фамилию сотрудника!");
                return false;
            }
            if (NameTextBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести имя сотрудника!");
                return false;
            }
            if (PatronymicTextBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести отчество сотрудника!");
                return false;
            }
            if (PhoneNumberBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести номер телефона сотрудника!");
                return false;
            }
            if (EmailTextBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести электронную почту сотрудника!");
                return false;
            }
            if (LoginTextBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести логин сотрудника!");
                return false;
            }
            if (PasswordTextBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести пароль сотрудника!");
                return false;
            }
            if (RoleComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо выбрать должность сотрудника!");
                return false;
            }
            if (GenderComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо выбрать пол сотрудника!");
                return false;
            }
            return true;
        }
        #endregion
    }
}
