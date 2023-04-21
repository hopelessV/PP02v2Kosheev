using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PP02v2
{
    public partial class ProductInBasket : UserControl
    {
        data_base data = new data_base();
        Form form1;
        int summ;
        public ProductInBasket(Form form, string header_text, string description, string manufacturer, string price, string discount, string photo)
        {
            InitializeComponent();
            this.form1 = form;
            label1.Text = header_text;
            label2.Text = description;
            label3.Text = manufacturer;
            label4.Text = price;
            label5.Text = "Скидка: " + discount + "%"; ;
            Image img = (Bitmap)Properties.Resources.ResourceManager.GetObject(photo.Substring(0, photo.Length - 4));

            pictureBox1.Image = img;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                product_class.Description = label2.Text;
                data.OpenCon();
                string order = $@"delete from Корзина where ОписаниеТовара = '{product_class.Description}'";
                SqlCommand del_order = new SqlCommand(order, data.GetCon());
                del_order.ExecuteNonQuery();
                data.CloseCon();
                MessageBox.Show("Товар успешно удалён из корзины", "Сообщение", MessageBoxButtons.OK);
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            product_class.Header_text = label1.Text;
            new Order().Show();
            //data.OpenCon();
            //string order_set = $@"insert into Заказы ([Состав заказа], [Дата заказа], [Дата доставки], [Пункт выдачи], [ФИО клиента], [Код для получения], [Статус заказа])
            //                      values ((select Артикул from Товар where Наименование = (select ID from [Тип_товара] where ТипыТоваров = '{product_class.Header_text }')),
            //                              {DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortDateString()}, )";
        }
    }
}
