using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Tools
{
    internal static class Translator
    {
        public static string Translate(string name)
        {
            switch (name)
            {
                case "ServiceName":
                    {
                        return "Услуга";
                    }
                case "ServiceDescription":
                    {
                        return "Описание";
                    }
                case "ServicePrice":
                    {
                        return "Цена за услугу (руб.)";
                    }
                case "OrderStatusName":
                    {
                        return "Статус заказа (руб.)";
                    }
                case "DateTimeOfOrder":
                    {
                        return "Дата и время заказа";
                    }
                case "TotalPrice":
                    {
                        return "Сумма (руб.)";
                    }
                case "IdOrderStatus":
                    {
                        return "Статус заказа";
                    }
                case "Surname":
                    {
                        return "Фамилия";
                    }
                case "Name":
                    {
                        return "Имя";
                    }
                case "DateOfBirth":
                    {
                        return "Дата рождения";
                    }
                case "PhoneNumber":
                    {
                        return "Номер телефона";
                    }
                case "Email":
                    {
                        return "Электронная почта";
                    }
                case "Login":
                    {
                        return "Логин";
                    }
                case "Password":
                    {
                        return "Пароль";
                    }
                case "RoleName":
                    {
                        return "Роль";
                    }
                case "GenderName":
                    {
                        return "Пол";
                    }
                case "BodyName":
                    {
                        return "Кузов";
                    }
                case "ColorName":
                    {
                        return "Цвет";
                    }
                case "ColorDescription":
                    {
                        return "Описание цвета";
                    }
                case "HEX":
                    {
                        return "HEX-код";
                    }
                case "RudderSideName":
                    {
                        return "Сторона руля";
                    }
                case "Quantity":
                    {
                        return "Количество (шт.)";
                    }
                case "SupplyPrice":
                    {
                        return "Общая цена автомобилей в поставке (руб.)";
                    }
                case "Address":
                    {
                        return "Адрес";
                    }
                case "DateTimeOfSupply":
                    {
                        return "Дата и время поставки";
                    }
                case "TotalPriceOfSupply":
                    {
                        return "Общая стоимость поставки (руб.)";
                    }
                case "CityName":
                    {
                        return "Город";
                    }
                case "StatusName":
                    {
                        return "Статус поставки";
                    }
                case "DriveTypeName":
                    {
                        return "Привод";
                    }
                case "EngineTypeName":
                    {
                        return "Тип двигателя";
                    }
                case "ConfigurationName":
                    {
                        return "Комплектация";
                    }
                case "CountryName":
                    {
                        return "Страна производства";
                    }
                case "ModelName":
                    {
                        return "Модель";
                    }
                case "TransmissionName":
                    {
                        return "Коробка передач";
                    }
                case "YearValue":
                    {
                        return "Год";
                    }
                case "EngineName":
                    {
                        return "Двигатель";
                    }
                case "CarStatusName":
                    {
                        return "Статус автомобиля";
                    }
                case "ClientPrice":
                    {
                        return "Цена (руб.)";
                    }
                case "Description":
                    {
                        return "Описание";
                    }
                case "Power":
                    {
                        return "Мощность";
                    }
                case "EngineCapacity":
                    {
                        return "Объём двигателя";
                    }
                case "TankCapacity":
                    {
                        return "Объём бака";
                    }
                case "IdCar":
                    {
                        return "Номер автомобиля";
                    }
                case "IdSupplier":
                    {
                        return "Номер поставщика";
                    }
                case "Client":
                    {
                        return "Клиент";
                    }
                case "Manager":
                    {
                        return "Менеджер";
                    }
                case "Car":
                    {
                        return "Машина";
                    }
                case "ServicesInOrder":
                    {
                        return "Услуги в заказе";
                    }
                case "Supplier":
                    {
                        return "Поставщик";
                    }
                case "Goods":
                    {
                        return "Товары в поставке";
                    }
                case "RuddeSideName":
                    {
                        return "Сторона руля";
                    }
                case "IdYear":
                    {
                        return "Год производства";
                    }
                case "Price":
                    {
                        return "Стоимость (руб.)";
                    }
                case "Year":
                    {
                        return "Год";
                    }
                case "MarkName":
                    {
                        return "Марка автомобиля";
                    }
                case "StartDateOfContract":
                    {
                        return "Дата начала контракта";
                    }
                case "EndDateOfContract":
                    {
                        return "Дата конца контракта";
                    }
                case "CurrentRemains":
                    {
                        return "Нынешниый остаток";
                    }
                case "ContractStatusName":
                    {
                        return "Статус лизинга";
                    }
                case "IncreaseName":
                    {
                        return "Название удорожание";
                    }
                case "IncreaseValue":
                    {
                        return "Математическая форма удорожания";
                    }
                case "Width":
                    {
                        return "Ширина (в мм)";
                    }
                case "Length":
                    {
                        return "Длина (в мм)";
                    }
                case "Height":
                    {
                        return "Высота (в мм)";
                    }
                case "Employee":
                    {
                        return "Сотрудник";
                    }
                case "Services":
                    {
                        return "Услуги";
                    }
                case "TotalPriceOfOrder":
                    {
                        return "Стоимость заказа";
                    }
                case "Patronymic":
                    {
                        return "Отчество";
                    }
                case "Cars":
                    {
                        return "Автомобили клиента";
                    }
            }
            return $"{name}";
        }
    }
}
