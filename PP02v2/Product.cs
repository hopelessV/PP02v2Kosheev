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
    public partial class Product : UserControl
    {
        data_base data = new data_base();
        Form form1;
        int summ;
        /// <summary/>
        /// <param name="form">форма</param>
        /// <param name="header_text">заголовок</param>
        /// <param name="description">Описание</param>
        /// <param name="manufacturer">производитель</param>
        /// <param name="price">цена</param>
        /// <param name="discount">скидка</param>
        /// <param name="photo">фото</param>
        public Product(Form form, string header_text, string description, string manufacturer, string price, string discount, string photo)
        {
            InitializeComponent();
            this.form1 = form;
            label1.Text = header_text;
            label2.Text = description;
            label3.Text = manufacturer;
            label4.Text = price;
            label5.Text = "Скидка: "+discount+"%";
            Image img = (Bitmap)Properties.Resources.ResourceManager.GetObject(photo.Substring(0,photo.Length-4));

            pictureBox1.Image = img;
            product_class.Discount = discount;
            product_class.Image = photo;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                product_class.Header_text = label1.Text;
                product_class.Description = label2.Text;
                product_class.Manufacturer = label3.Text;
                product_class.Price = label4.Text;
                data.OpenCon();
                string order = $@"insert into Корзина (IDПользователя, НаименованиеТовара, ОписаниеТовара, Производитель, Цена, Скидка, Изображение) values 
                                  ((select ID from Пользователи where ФИО = '{info.fio}'),
                                   (select ID from Тип_товара where ТипыТоваров = '{product_class.Header_text}'), '{product_class.Description}',
                                   (select ID from Производитель where Производители = '{product_class.Manufacturer}'), '{product_class.Price}', '{int.Parse(product_class.Discount)}', (select Изображение from Товар where Описание = '{product_class.Description}'))";
                SqlCommand add_order = new SqlCommand(order, data.GetCon());
                add_order.ExecuteNonQuery();
                data.CloseCon();
                MessageBox.Show("Товар успешно добавлен в корзину", "Сообщение", MessageBoxButtons.OK);

                pictureBox2.Visible = true;
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new Basket().Show();
        }
    }
}
