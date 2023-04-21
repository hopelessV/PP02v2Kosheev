using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP02v2
{
    internal class dataGrid_selected_product
    {
        //номер товара
        public static int ID {  get; set; }
        //Артикул товара
        public static string Article { get; set; }
        //Название товара
        public static string Name { get; set; }
        //Ед. измерения
        public static string ed_izm { get; set; }
        //Стоимость товара
        public static int price { get; set; }
        //Максимальная скидка
        public static int total_sales { get; set; }
        //Производитель товара
        public static string manufacturer { get; set; }
        //Поставщик
        public static string provider { get; set; }
        //Категория товара
        public static string category_product { get; set; }
        //Действующая скидка
        public static int current_discount { get; set; }
        //Склад
        public static int warehouse { get; set; }
        //Описание
        public static string description { get; set; }
        //Изображение товара
        public static string image { get; set;}
    }
}
