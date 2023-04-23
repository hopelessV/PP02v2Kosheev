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
    public partial class Manager : Form
    {
        data_base data = new data_base();
        int countStr;
        string[] _disc = new string[2];
        public Manager()
        {
            InitializeComponent();
            FillBascet();
            FillDataGrid();
        }
        private void Division(string text)
        {
            if (text == "Все скидки" || text == "")
            {
                _disc[0] = "0";
                _disc[1] = "100";
                return;
            }
            _disc = text.Split('-');
        }
        private void FillDataGrid()
        {
            data.OpenCon();
            string order = $@"select [Номер заказа], [Состав заказа], [Дата заказа], [Дата доставки], 'Пункт выдачи' = [Пункт_выдачи].Улица, [ФИО клиента], [Код для получения], [Статус заказа] from Заказы
                                  join Пункт_выдачи as t on t.ID = Заказы.[Пункт выдачи]";
            SqlDataAdapter select_order = new SqlDataAdapter(order, data.GetCon());
            DataSet set_order = new DataSet();
            select_order.Fill(set_order);
            dataGridView1.DataSource = set_order.Tables[0];
            data.CloseCon();
        }
        private void FillBascet()
        {
            try
            {
                panel3.Controls.Clear();

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
                    panel3.Controls.Add(services);
                }

                countStr = header_text.Length;
                label6.Text = "Страниц : " + header_text.Length + " из " + countStr;
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void easypeasy(TextBox nameFilter, ComboBox discountFilter, ComboBox priceFilter)
        {
            try
            {
                panel2.Controls.Clear();
                Division(discountFilter.Text);
                string order;

                if (priceFilter.Text == "") order = "order by Стоимость desc";
                else order = "order by Стоимость asc";

                string query = $@"join [Тип_товара] as t on t.ID = Товар.Наименование
                           where t.ТипыТоваров  like '{nameFilter.Text}%' and [Действующая скидка]>={_disc[0]} and [Действующая скидка]<{_disc[1]} {order}";

                string[] header_text = data_base.ExitColumn($@"Товар {query}", "t.ТипыТоваров");

                string[] description = data_base.ExitColumn($@"Товар {query}", "Описание");

                string[] manufacturer = data_base.ExitColumn($@"Товар join Производитель on Производитель.ID = Товар.Производитель {query}", "Производитель.Производители");

                string[] price = data_base.ExitColumn($@"Товар {query}", "Стоимость");

                string[] discount = data_base.ExitColumn($@"Товар {query}", "[Действующая скидка]");

                string[] photo = data_base.ExitColumn($@"Товар {query}", "Изображение");

                for (int i = 0; i < header_text.Length; i++)
                {
                    Product services = new Product(this, header_text[i], description[i], manufacturer[i], price[i], discount[i], photo[i]);
                    services.Dock = DockStyle.Top;
                    if (discount[i] != 0.ToString())
                    {
                        services.BackColor = Color.LightGreen;
                    }
                    panel2.Controls.Add(services);
                }

                countStr = header_text.Length;
                label5.Text = header_text.Length + " из " + countStr;
                return;
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            easypeasy(textBox1, comboBox1, comboBox2);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            easypeasy(textBox1, comboBox1, comboBox2);
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            easypeasy(textBox1, comboBox1, comboBox2);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FillBascet();
        }
        private void Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            new Authorization().Show(); Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            data.OpenCon();
            string order = $@"update Заказы set [Состав заказа] = '{textBox2.Text}', [Пункт выдачи] = (select ID from [Пункт_выдачи] where Улица = '{comboBox3.Text}'), [ФИО клиента] = '{textBox4.Text}',
                              [Код для получения] = {int.Parse(textBox3.Text)}, [Статус заказа] = '{comboBox4.Text}' where [Номер заказа] = {dataGrid_selected_order.ID}";
            data.CloseCon();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            #region[class]
            dataGrid_selected_order.ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            dataGrid_selected_order.order_composition = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            dataGrid_selected_order.order_date = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            dataGrid_selected_order.delivery_date = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            dataGrid_selected_order.Point_of_issue = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            dataGrid_selected_order.FIO_client = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            dataGrid_selected_order.Code_to_get = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            dataGrid_selected_order.order_status = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            #endregion
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            comboBox3.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            comboBox4.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
        }
    }
}
