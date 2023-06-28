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
using ModernWpf.Controls.Primitives;
using CarWash.Tools;
using CarWash.Entities;

namespace CarWash.Windows
{
    
    public partial class SprAddAndChange : Window
    {
        bool ChangeMode = false;

        string TableName;

        int id;

        public void ChangeVisibilityOrTranslate()
        {
            foreach (Control item in MainStackPanel.Children)
            {
                if ((item is not Button) && ControlHelper.GetHeader(item).ToString() == "")
                {
                    item.Visibility = Visibility.Collapsed;
                }
                else if (item is not Button)
                {
                    ControlHelper.SetHeader(item, Translator.Translate(ControlHelper.GetHeader(item).ToString()));
                }
            }
        }

        public void SelectTable(string Table)
        {
            switch (Table)
            {
                case "AdditionalServices":
                    {
                        ControlHelper.SetHeader(First, "ServiceName");
                        ControlHelper.SetHeader(Second, "ServiceDescription");
                        ControlHelper.SetHeader(Third, "ServicePrice");
                        if (ChangeMode)
                        {
                            First.Text = DbUtils.db.AdditionalServices.Single(p => p.IdService == id).ServiceName;
                            Second.Text = DbUtils.db.AdditionalServices.Single(p => p.IdService == id).ServiceDescription;
                            Third.Text = DbUtils.db.AdditionalServices.Single(p => p.IdService == id).ServicePrice.ToString();
                        }
                        break;
                    }
                case "Body":
                    {
                        ControlHelper.SetHeader(First, "BodyName");
                        if (ChangeMode)
                        {
                            First.Text = DbUtils.db.Body.Single(p => p.IdBody == id).BodyName;
                        }
                        break;
                    }
                case "Color":
                    {
                        ControlHelper.SetHeader(First, "ColorName");
                        ControlHelper.SetHeader(Second, "ColorDescription");
                        ControlHelper.SetHeader(Third, "HEX");
                        if (ChangeMode)
                        {
                            First.Text = DbUtils.db.Color.Single(p => p.IdColor == id).ColorName;
                            Second.Text = DbUtils.db.Color.Single(p => p.IdColor == id).ColorDescription;
                            Third.Text = DbUtils.db.Color.Single(p => p.IdColor == id).Hex;
                        }
                        break;
                    }
                case "Gender":
                    {
                        ControlHelper.SetHeader(First, "GenderName");
                        if (ChangeMode)
                        {
                            First.Text = DbUtils.db.Gender.Single(p => p.IdGender == id).GenderName;
                        }
                        break;
                    }
                case "Mark":
                    {
                        ControlHelper.SetHeader(First, "MarkName");
                        if (ChangeMode)
                        {
                            First.Text = DbUtils.db.Mark.Single(p => p.IdMark == id).MarkName;
                        }
                        break;
                    }
                case "Models":
                    {
                        ControlHelper.SetHeader(First, "ModelName");
                        ControlHelper.SetHeader(SelectBox, "MarkName");
                        SelectBox.ItemsSource = DbUtils.db.Mark.ToList();
                        SelectBox.DisplayMemberPath = "MarkName";
                        if (ChangeMode)
                        {
                            Models models = DbUtils.db.Models.Single(p => p.IdModel == id);
                            First.Text = models.ModelName;
                            SelectBox.SelectedItem = DbUtils.db.Mark.Single(p => p.IdMark == models.IdMark);
                        }
                        break;
                    }
                case "Role":
                    {
                        ControlHelper.SetHeader(First, "RoleName");
                        if (ChangeMode)
                        {
                            First.Text = DbUtils.db.Role.Single(p => p.IdRole == id).RoleName;
                        }
                        break;
                    }
                case "OrderStatus":
                    {
                        ControlHelper.SetHeader(First, "OrderStatusName");
                        if (ChangeMode)
                        {
                            First.Text = DbUtils.db.OrderStatus.Single(p => p.IdOrderStatus == id).OrderStatusName;
                        }
                        break;
                    }
                default:
                    {
                        MessageBox.Show("error");
                        break;
                    }
            }
            ChangeVisibilityOrTranslate();
        }
        public SprAddAndChange(string Table, bool IsChangeMode, int? id)
        {
            InitializeComponent();
            ChangeMode = IsChangeMode;
            this.Title = "Создание записи справочника";
            if (ChangeMode)
            {
                this.id = id.Value;
                this.Title = "Изменение записи справочника";
            }
            TableName = Table;
            SelectTable(Table);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            switch (TableName)
            {
                case "AdditionalServices":
                    {
                        AdditionalServices service;
                        if (ChangeMode)
                            service = DbUtils.db.AdditionalServices.Single(p => p.IdService == id);
                        else
                            service = new AdditionalServices();

                        service.ServiceName = First.Text;
                        service.ServiceDescription = Second.Text;
                        service.ServicePrice = decimal.Parse(Third.Text);

                        if (!ChangeMode)
                            DbUtils.db.AdditionalServices.Add(service);

                        DbUtils.db.SaveChanges();
                        break;
                    }
                case "Body":
                    {
                        Body body;
                        if (ChangeMode)
                            body = DbUtils.db.Body.Single(p => p.IdBody == id);
                        else
                            body = new Body();

                        body.BodyName = First.Text;

                        if (!ChangeMode)
                            DbUtils.db.Body.Add(body);

                        DbUtils.db.SaveChanges();
                        break;
                    }
                case "Color":
                    {
                        Entities.Color Color;
                        if (ChangeMode)
                            Color = DbUtils.db.Color.Single(p => p.IdColor == id);
                        else
                            Color = new Entities.Color();

                        Color.ColorName = First.Text;
                        Color.ColorDescription = Second.Text;
                        Color.Hex = Third.Text;
                        if (!ChangeMode)
                            DbUtils.db.Color.Add(Color);

                        DbUtils.db.SaveChanges();
                        break;
                    }
                case "Gender":
                    {
                        Gender Gender;
                        if (ChangeMode)
                            Gender = DbUtils.db.Gender.Single(p => p.IdGender == id);
                        else
                            Gender = new Gender();

                        Gender.GenderName = First.Text;

                        if (!ChangeMode)
                            DbUtils.db.Gender.Add(Gender);

                        DbUtils.db.SaveChanges();
                        break;
                    }
                case "Mark":
                    {
                        Mark Marks;
                        if (ChangeMode)
                            Marks = DbUtils.db.Mark.Single(p => p.IdMark == id);
                        else
                            Marks = new Mark();

                        Marks.MarkName = First.Text;

                        if (!ChangeMode)
                            DbUtils.db.Mark.Add(Marks);

                        DbUtils.db.SaveChanges();
                        break;
                    }
                case "Models":
                    {
                        Models model;
                        if (ChangeMode)
                            model = DbUtils.db.Models.Single(p => p.IdModel == id);
                        else
                            model = new Models();

                        model.ModelName = First.Text;
                        model.IdMark = (SelectBox.SelectedItem as Mark).IdMark;

                        if (!ChangeMode)
                            DbUtils.db.Models.Add(model);

                        DbUtils.db.SaveChanges();
                        break;
                    }
                case "Role":
                    {
                        Role Role;
                        if (ChangeMode)
                            Role = DbUtils.db.Role.Single(p => p.IdRole == id);
                        else
                            Role = new Role();

                        Role.RoleName = First.Text;

                        if (!ChangeMode)
                            DbUtils.db.Role.Add(Role);

                        DbUtils.db.SaveChanges();
                        break;
                    }
                case "OrderStatus":
                    {
                        OrderStatus status;
                        if (ChangeMode)
                            status = DbUtils.db.OrderStatus.Single(p => p.IdOrderStatus == id);
                        else
                            status = new OrderStatus();

                        status.OrderStatusName = First.Text;

                        if (!ChangeMode)
                            DbUtils.db.OrderStatus.Add(status);

                        DbUtils.db.SaveChanges();
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Ошибка");
                        break;
                    }
            }
            MessageBox.Show("Данные сохранены");
            this.Close();
        }
    }
}
