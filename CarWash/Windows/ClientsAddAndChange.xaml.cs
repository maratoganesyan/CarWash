using CarWash.Entities;
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
using CarWash.Tools;

namespace CarWash.Windows
{
    /// <summary>
    /// Interaction logic for ClientsAddAndChange.xaml
    /// </summary>
    public partial class ClientsAddAndChange : Window
    {
        bool ChangeMode;

        int id;

        List<Car> cars;

        public void FillClient(Clients? clients)
        {
            id = clients.IdClient;
            SurnameTextBox.Text = clients.Surname;
            NameTextBox.Text = clients.Name;
            PatronymicTextBox.Text = clients.Patronymic;
            PhoneNumberTextBox.Text = clients.PhoneNumber;
            GenderComboBox.SelectedItem = clients.IdGenderNavigation;
            CarsControl.ItemsSource = cars;
        }
        public ClientsAddAndChange(Clients? client)
        {
            InitializeComponent();
            if (client == null)
            {
                ThisWindow.Title = "Создание нового клиента";
                ChangeMode = false;
                GenderComboBox.ItemsSource = DbUtils.db.Gender.ToList();
                GenderComboBox.SelectedIndex = 0;
                Grid.SetColumn(ClientData, 0);
                Grid.SetColumnSpan(ClientData, 2);
                CarsGrid.Visibility = Visibility.Collapsed;
                this.Width = 300;
            }
            else
            {
                ThisWindow.Title = "Изменение существующего клиента";
                ChangeMode = true;
                cars = client.ClientsCars.Select(cc => cc.IdCarNavigation).ToList();
                GenderComboBox.ItemsSource = DbUtils.db.Gender.ToList();
                FillClient(client);

            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateAll())
                {
                    return;
                }
                Clients client;
                if (ChangeMode)
                    client = DbUtils.db.Clients.Single(c => c.IdClient == id);
                else
                    client = new Clients();
                client.Surname = SurnameTextBox.Text;
                client.Name = NameTextBox.Text;
                client.Patronymic = PatronymicTextBox.Text;
                client.PhoneNumber = PhoneNumberTextBox.Text;
                client.IdGender = (GenderComboBox.SelectedItem as Gender).IdGender;
                if (!ChangeMode)
                {
                    DbUtils.db.Add(client);
                }
                DbUtils.db.SaveChanges();
                MessageBox.Show("Данные успешно сохранены");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        #region Validation
        private bool ValidateAll()
        {
            if (SurnameTextBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести фамилию клиента!");
                return false;
            }
            if (NameTextBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести имя клиента!");
                return false;
            }
            if (PatronymicTextBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести отчество клиента!");
                return false;
            }
            if (PhoneNumberTextBox.Text == "")
            {
                MessageBox.Show("Необходимо ввести номер телефона клиента!");
                return false;
            }
            if (GenderComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо выбрать пол клиента");
                return false;
            }
            return true;

        }
        #endregion
    }
}
