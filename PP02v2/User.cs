using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace PP02v2
{
    public partial class User : Form
    {
        string discount = "";
        string[] _disc = new string[2];
        public User()
        {
            InitializeComponent();
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
        int countStr;
        private void User_Load(object sender, EventArgs e)
        {
            try
            {
                string[] header_text = data_base.ExitColumn("Товар join [Тип_товара] on [Тип_товара].ID = Товар.Наименование", "[Тип_товара].ТипыТоваров");

                string[] description = data_base.ExitColumn("Товар", "Описание");

                string[] manufacturer = data_base.ExitColumn("Товар join Производитель on Производитель.ID = Товар.Производитель", "Производитель.Производители");

                string[] price = data_base.ExitColumn("Товар", "Стоимость");

                string[] discount = data_base.ExitColumn("Товар", "[Действующая скидка]");

                string[] photo = data_base.ExitColumn("Товар", "Изображение");

                for (int i = 0; i < header_text.Length; i++)
                {
                    Product services = new Product(this, header_text[i], description[i], manufacturer[i], price[i], discount[i], photo[i]);
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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            easypeasy(textBox1, comboBox1, comboBox2);
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

                label5.Text = header_text.Length + " из " + countStr;
                return;
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            easypeasy(textBox1, comboBox1, comboBox2);
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            easypeasy(textBox1, comboBox1, comboBox2);
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
        }

        private void comboBox1_MouseDown(object sender, MouseEventArgs e)
        {
            comboBox1.Text = "";
        }

        private void comboBox2_MouseDown(object sender, MouseEventArgs e)
        {
            comboBox2.Text = "";
        }

        private void User_FormClosing(object sender, FormClosingEventArgs e)
        {
            new Authorization().Show(); Hide();
        }
    }
}
