using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PP02v2
{
    public partial class Basket : Form
    {
        public Basket()
        {
            InitializeComponent();
        }
        int countStr;
        private void Basket_Load(object sender, EventArgs e)
        {
            FillBascet();
        }
        private void FillBascet()
        {
            try
            {
                panel2.Controls.Clear();

                string[] header_text = data_base.ExitColumn($@"Корзина join [Тип_товара] on [Тип_товара].ID = Корзина.НаименованиеТовара where IDПользователя = {info.ID}", "[Тип_товара].ТипыТоваров");

                string[] description = data_base.ExitColumn($@"Корзина where IDПользователя = {info.ID}", "ОписаниеТовара");

                string[] manufacturer = data_base.ExitColumn($@"Корзина join Производитель on Производитель.ID = Корзина.Производитель where IDПользователя = {info.ID}", "Производитель.Производители");

                string[] price = data_base.ExitColumn($@"Корзина where IDПользователя = {info.ID}", "Цена");

                string[] discount = data_base.ExitColumn($@"Корзина where IDПользователя = {info.ID}", "Скидка");

                string[] photo = data_base.ExitColumn($@"Корзина where IDПользователя = {info.ID}", "Изображение");

                for (int i = 0; i < header_text.Length; i++)
                {
                    ProductInBasket services = new ProductInBasket(this, header_text[i], description[i], manufacturer[i], price[i], discount[i], photo[i]);
                    services.Dock = DockStyle.Top;
                    if (discount[i] != "0")
                    {
                        services.BackColor = Color.LightGreen;
                    }
                    panel2.Controls.Add(services);
                }

                countStr = header_text.Length;
                label5.Text = "Страниц : " + header_text.Length + " из " + countStr;
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FillBascet();
        }
    }
}
