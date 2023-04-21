using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace PP02v2
{
    public partial class Admin : Form
    {
        data_base data = new data_base();
        string path = @"C:\Users\Hopeless\Desktop\IT\PP02v2\PP02v2\Resources\";
        private static string photo_name;
        public Admin()
        {
            InitializeComponent();
            FillDataGridView1();
            FillDataGridView2();
            FillComboBox();
            ComboBox();
        }
        private void ComboBox()
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
        }
        private void FillDataGridView1()
        {
            try
            {
                data.OpenCon();
                string product = $@"select [IDТовара], [Артикул], 'Название' = [Тип_товара].[ТипыТоваров], [Стоимость], [Производитель].[Производители]
                                    [Поставщик], 'Категория' = [КатегоряТоваров].[КатегорииТоваров], [Действующая скидка], [Кол-во на складе], [Описание], [Изображение]
                                    from [Товар]
                                    join [Тип_товара] on [Тип_товара].ID = [Товар].[Наименование]
                                    join [Производитель] on [Производитель].ID = [Товар].[Производитель]
                                    join [КатегоряТоваров] on [КатегоряТоваров].ID = [Товар].[Категория товара]";
                SqlDataAdapter select_product = new SqlDataAdapter(product, data.GetCon());
                DataSet set_product = new DataSet();
                select_product.Fill(set_product);
                dataGridView1.DataSource = set_product.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                data.CloseCon();
                
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void FillDataGridView2()
        {
            try
            {
                data.OpenCon();
                string order = $@"select [Номер заказа], [Состав заказа], [Дата заказа], [Дата доставки], [Пункт выдачи], [ФИО клиента], [Код для получения], [Статус заказа] from Заказы";
                SqlDataAdapter select_order = new SqlDataAdapter(order, data.GetCon());
                DataSet set_order = new DataSet();
                select_order.Fill(set_order);
                dataGridView2.DataSource = set_order.Tables[0];
                dataGridView2.Columns[0].Visible = false;
                data.CloseCon();
            }
            catch (Exception ex) {  MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void FillComboBox()
        {
            try
            {
                data.OpenCon();
                string name_product = $@"select ТипыТоваров from [Тип_товара]";
                SqlDataReader read_new_product = new SqlCommand(name_product, data.GetCon()).ExecuteReader();
                while (read_new_product.Read())
                {
                    comboBox1.Items.Add(read_new_product.GetString(0));
                    comboBox4.Items.Add(read_new_product.GetString(0));
                }
                read_new_product.Close();

                string manufacturer = $@"select Производители from Производитель";
                SqlDataReader read_manufacturer = new SqlCommand(manufacturer, data.GetCon()).ExecuteReader();
                while (read_manufacturer.Read())
                {
                    comboBox2.Items.Add(read_manufacturer.GetString(0));
                }
                read_manufacturer.Close();

                string provider = $@"select Поставщик from Товар";
                SqlDataReader read_provider = new SqlCommand(provider, data.GetCon()).ExecuteReader();
                while (read_provider.Read())
                {
                    comboBox3.Items.Add(read_provider.GetString(0));
                }
                read_provider.Close();

                string Point_of_issue = $@"select Улица from [Пункт_выдачи]";
                SqlDataReader read_Point_of_issue = new SqlCommand(Point_of_issue, data.GetCon()).ExecuteReader();
                while (read_Point_of_issue.Read())
                {
                    comboBox6.Items.Add(read_Point_of_issue.GetString(0));
                }
                read_Point_of_issue.Close();

                string order_status = $@"select [Статус заказа] from Заказы";
                SqlDataReader read_order_status = new SqlCommand(order_status, data.GetCon()).ExecuteReader();
                while(read_order_status.Read())
                {
                    comboBox5.Items.Add(read_order_status.GetString(0));
                }
                read_order_status.Close();
                data.CloseCon();
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            #region[class]
            dataGrid_selected_product.ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            dataGrid_selected_product.Article = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            dataGrid_selected_product.Name = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            dataGrid_selected_product.ed_izm = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            //dataGrid_selected_product.price = Convert.ToInt32(dataGridView1.CurrentRow.Cells[4].Value.ToString());
            dataGrid_selected_product.total_sales = Convert.ToInt32(dataGridView1.CurrentRow.Cells[5].Value.ToString());
            dataGrid_selected_product.manufacturer = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            dataGrid_selected_product.provider = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            dataGrid_selected_product.category_product = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            dataGrid_selected_product.current_discount = Convert.ToInt32(dataGridView1.CurrentRow.Cells[9].Value.ToString());
            dataGrid_selected_product.warehouse = Convert.ToInt32(dataGridView1.CurrentRow.Cells[10].Value.ToString());
            dataGrid_selected_product.description = dataGridView1.CurrentRow.Cells[11].Value.ToString();
            dataGrid_selected_product.image = dataGridView1.CurrentRow.Cells[12].Value.ToString();
            #endregion
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            //textBox3.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            comboBox3.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            comboBox4.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            textBox6.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();
            textBox8.Text = dataGridView1.CurrentRow.Cells[11].Value.ToString();
            pictureBox2.ImageLocation = path + dataGridView1.CurrentRow.Cells[12].Value.ToString();
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            #region[class]
            dataGrid_selected_order.ID = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value.ToString());
            dataGrid_selected_order.order_composition = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            dataGrid_selected_order.order_date = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            dataGrid_selected_order.delivery_date = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            dataGrid_selected_order.Point_of_issue = dataGridView2.CurrentRow.Cells[4].Value.ToString();
            dataGrid_selected_order.FIO_client = dataGridView2.CurrentRow.Cells[5].Value.ToString();
            dataGrid_selected_order.Code_to_get = dataGridView2.CurrentRow.Cells[6].Value.ToString();
            dataGrid_selected_order.order_status = dataGridView2.CurrentRow.Cells[7].Value.ToString();
            #endregion
            textBox5.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            comboBox6.Text = dataGridView2.CurrentRow.Cells[4].Value.ToString();
            textBox10.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();
            textBox9.Text = dataGridView2.CurrentRow.Cells[6].Value.ToString();
            comboBox5.Text = dataGridView2.CurrentRow.Cells[7].Value.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                data.OpenCon();
                string new_product = $@"insert into Товар (Артикул, Наименование, [Единица измерения], Стоимость, [Размер максимально возможной скидки], Производитель,
                                                       Поставщик, [Категория товара], [Действующая скидка], [Кол-во на складе], Описание, Изображение) values 
                                                      ('{textBox1.Text}', (select ID from [Тип_товара] where ТипыТоваров = '{comboBox1.Text}'), '{textBox2.Text}', {int.Parse(textBox3.Text)},
                                                       {int.Parse(textBox4.Text)}, (select ID from Производитель where Производители = '{comboBox2.Text}'), '{comboBox3.Text}',
                                                       (select ID from [Тип_товара] where ТипыТоваров = '{comboBox4.Text}'), {int.Parse(textBox6.Text)}, {int.Parse(textBox7.Text)}, '{textBox8.Text}', '{photo_name}')";
                SqlCommand add_new_product = new SqlCommand(new_product, data.GetCon());
                add_new_product.ExecuteNonQuery();
                data.CloseCon();
                FillDataGridView1();
                FillDataGridView2();
                MessageBox.Show("Товар был успешно добавлен", "Сообщение", MessageBoxButtons.OK);
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                data.OpenCon();
                string product = $@"update Товар set Артикул = '{textBox1.Text}', Наименование = (select ID from [Тип_товара] where ТипыТоваров = '{comboBox1.Text}'), [Единица измерения] = '{textBox2.Text}',
                                Стоимость = {int.Parse(textBox3.Text)}, [Размер максимально возможной скидки] = {int.Parse(textBox4.Text)}, Производитель = (select ID from Производитель where Производители = '{comboBox2.Text}'),
                                Поставщик = '{comboBox3.Text}', [Категория товара] = (select ID from [Тип_товара] where ТипыТоваров = '{comboBox4.Text}'), [Действующая скидка] = {int.Parse(textBox6.Text)},
                                [Кол-во на складе] = {int.Parse(textBox7.Text)}, Описание = '{textBox8.Text}', Изображение = '{photo_name}' where IDТовара = {dataGrid_selected_product.ID}";
                SqlCommand update_product = new SqlCommand(product, data.GetCon());
                update_product.ExecuteNonQuery();
                data.CloseCon();
                FillDataGridView1();
                FillDataGridView2();
                MessageBox.Show("Данные о товаре были успешно обновлены", "Сообщение", MessageBoxButtons.OK);
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                data.OpenCon();
                string product = $@"delete from Товар where ID = {dataGrid_selected_product.ID}";
                SqlCommand delete_product = new SqlCommand(product, data.GetCon());
                delete_product.ExecuteNonQuery();
                data.CloseCon();
                FillDataGridView1();
                FillDataGridView2();
                MessageBox.Show("Данные о товаре были успешно удалены", "Сообщение", MessageBoxButtons.OK);
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private Bitmap bmp;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.BMP, *.JPG, " + "*.GIF, *.PNG)|*.bmp; *.jpg; *.gif; *.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Image image = Image.FromFile(dialog.FileName);
                bmp = new Bitmap(image);
                pictureBox2.Image = bmp;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                data.OpenCon();
                string order = $@"update Заказы set [Состав заказа] = '{textBox5.Text}', [Пункт выдачи] = (select ID from [Пункт_выдачи] where Улица = '{comboBox6.Text}'), [ФИО клиента] = '{textBox10.Text}',
                                  [Код для получения] = {int.Parse(textBox9.Text)}, [Статус заказа] = '{comboBox5.Text}' where [Номер заказа] = {dataGrid_selected_order.ID}";
                SqlCommand update_order = new SqlCommand(order, data.GetCon());
                update_order.ExecuteNonQuery();
                data.CloseCon();
                FillDataGridView1();
                FillDataGridView2();
                MessageBox.Show("Данные о заказе были успешно изменены", "Сообщение", MessageBoxButtons.OK);
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Admin_FormClosing(object sender, FormClosingEventArgs e)
        {
            new Authorization().Show(); Hide();
        }
    }
}
