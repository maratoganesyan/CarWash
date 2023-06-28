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
    /// Interaction logic for CarsAddAndChange.xaml
    /// </summary>
    public partial class CarsAddAndChange : Window
    {
        bool ChangeMode;

        int id;


        private void FillComboBoxes()
        {
            ModelComboBox.ItemsSource = DbUtils.db.Models.ToList();
            ModelComboBox.DisplayMemberPath = "ModelName";
            MarkComboBox.ItemsSource = DbUtils.db.Mark.ToList();
            MarkComboBox.DisplayMemberPath = "MarkName";
            BodyComboBox.ItemsSource = DbUtils.db.Body.ToList();
            BodyComboBox.DisplayMemberPath = "BodyName";
            ColorComboBox.ItemsSource = DbUtils.db.Color.ToList();
            ColorComboBox.DisplayMemberPath = "ColorName";
            CLientComboBox.ItemsSource = DbUtils.db.Clients.ToList();
        }

        public void AddComboBoxes(Car car)
        {
            MarkComboBox.SelectedItem = car.IdModelNavigation.IdMarkNavigation;
            ModelComboBox.SelectedItem = car.IdModelNavigation;
            BodyComboBox.SelectedItem = car.IdBodyNavigation;
            ColorComboBox.SelectedItem = car.IdColorNavigation;
            if(car.ClientsCars.Count != 0)
            {
                CLientComboBox.SelectedItem = car.ClientsCars.First().IdClientNavigation;
            }

            HeightBox.Text = car.Height.ToString(); 
            WidthBox.Text = car.Width.ToString(); 
            LengthBox.Text = car.Length.ToString();
            StateNumberBox.Text = car.StateNumber;
            DescriptionTextBox.AppendText(car.Description);
        }

        public void SetDefaultValue()
        {
            MarkComboBox.SelectedIndex = 0;
            ModelComboBox.SelectedIndex = 0;
            BodyComboBox.SelectedIndex = 0;
            ColorComboBox.SelectedIndex = 0;
            CLientComboBox.SelectedIndex = 0;
        }
        public CarsAddAndChange(Car? car)
        {
            InitializeComponent();
            FillComboBoxes();
            if(car == null)
            {
                ThisWindow.Title = "Создание нового авто в системе";
                ChangeMode = false;
                SetDefaultValue();
            }
            else
            {
                ThisWindow.Title = "Изменение существующего авто в системе";
                ChangeMode = true;
                AddComboBoxes(car);
                id = car.IdCar;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Car car;
                Clients client = (CLientComboBox.SelectedItem as Clients);
                if (ChangeMode)
                {
                    car = DbUtils.db.Car.FirstOrDefault(p => p.IdCar == id);

                }
                else
                {
                    car = new Car();
                    
                }
                car.IdModel = (ModelComboBox.SelectedItem as Models).IdModel;
                car.IdBody = (BodyComboBox.SelectedItem as Body).IdBody;
                car.IdColor = (ColorComboBox.SelectedItem as Entities.Color).IdColor;
                car.StateNumber = StateNumberBox.Text;
                car.Width = int.Parse(WidthBox.Text);
                car.Height = int.Parse(HeightBox.Text);
                car.Length = int.Parse(LengthBox.Text);
                TextRange textRange = new TextRange(DescriptionTextBox.Document.ContentStart,
                                                   DescriptionTextBox.Document.ContentEnd);
                car.Description = textRange.Text.Replace("\r\n", string.Empty);

                if(!ChangeMode)
                {
                    DbUtils.db.Car.Add(car);
                }
                
                DbUtils.db.SaveChanges();
                if(!ChangeMode)
                {
                    ClientsCars cc = new ClientsCars();
                    cc.IdClient = client.IdClient;
                    cc.IdCar = DbUtils.db.Car.OrderBy(cc => cc.IdCar).Last().IdCar;
                    DbUtils.db.ClientsCars.Add(cc);
                }
                else
                {
                    ClientsCars cc = car.ClientsCars.Single();
                    cc.IdClient = client.IdClient;
                    cc.IdCar = id;
                    DbUtils.db.ClientsCars.Add(cc);
                }
                DbUtils.db.SaveChanges();
                MessageBox.Show("Данные успешно сохранены");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Данные введены неверно");
            }
        }

        private void MarkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mark mark = MarkComboBox.SelectedItem as Mark;
            ModelComboBox.ItemsSource = DbUtils.db.Models.Where(m => m.IdMark == mark.IdMark).ToList();

            ModelComboBox.DisplayMemberPath = "ModelName";
            ModelComboBox.SelectedIndex = 0;
        }
    }
}
