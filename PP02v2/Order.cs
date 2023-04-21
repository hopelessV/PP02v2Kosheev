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
    public partial class Order : Form
    {
        data_base data = new data_base();
        public Order()
        {
            InitializeComponent();
            FillComboBox();
        }
        private void Order_Load(object sender, EventArgs e)
        {
            
             textBox1.Text = product_class.Header_text;
             textBox2.Text = info.fio;
             Random rnd = new Random();
             textBox3.Text = rnd.Next(1000, 9999).ToString();
            
        }
        private void FillComboBox()
        {
            data.OpenCon();
            string point_of_issue = $@"select Улица from Пункт_выдачи";
            SqlDataReader read_point_of_issue = new SqlCommand(point_of_issue, data.GetCon()).ExecuteReader();
            while (read_point_of_issue.Read())
            {
                comboBox1.Items.Add(read_point_of_issue.GetString(0));
            }
            read_point_of_issue.Close();
            data.CloseCon();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                data.OpenCon();
                string new_ordaer = $@"insert into Заказы ([Состав заказа], [Дата заказа], [Дата доставки], [Пункт выдачи], [ФИО клиента], [Код для получения], [Статус заказа])
                                  values (select ID from [Тип_товара] where ТипыТоваров = '{textBox1.Text}'), {DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortDateString()}, 
                                         (select [ПочтовыйИндекс] from [Пункт_выдачи] where Улица = '{comboBox1.Text}'), '{textBox2.Text}', {int.Parse(textBox3.Text)}, 'Новый')";
                SqlCommand Add_new_order = new SqlCommand(new_ordaer, data.GetCon());
                Add_new_order.ExecuteNonQuery();
                MessageBox.Show("Заказ был успешно создан", "Сообщение", MessageBoxButtons.OK);
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
