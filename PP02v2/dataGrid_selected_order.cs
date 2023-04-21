using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP02v2
{
    internal class dataGrid_selected_order
    {
        //номер заказа
        public static int ID { get; set; }
        //Состав заказа
        public static string order_composition { get; set; }
        //Дата заказа
        public static string order_date { get; set; }
        //Дата доставки
        public static string delivery_date { get; set; }
        //Пункт выдачи
        public static string Point_of_issue { get; set; }
        //ФИО клиента
        public static string FIO_client { get; set; }
        //Код для получения
        public static string Code_to_get { get; set; }
        //Статус заказа
        public static string order_status { get; set; }
    }
}
