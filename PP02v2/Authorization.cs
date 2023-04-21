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
    public partial class Authorization : Form
    {
        data_base data = new data_base();
        public Authorization()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxLogin.Text != "" || textBoxPassword.Text != "")
                {
                    data.OpenCon();
                    string User = $@"select * from Пользователи where Логин = '{textBoxLogin.Text}' and Пароль = '{textBoxPassword.Text}'";
                    SqlDataReader read_user = new SqlCommand(User, data.GetCon()).ExecuteReader();
                    while (read_user.Read())
                    {
                        info.ID = Convert.ToInt32(read_user.GetValue(0));
                        info.fio = Convert.ToString(read_user.GetValue(1));
                        info.login = Convert.ToString(read_user.GetValue(2));
                        info.pass = Convert.ToString(read_user.GetValue(3));
                        info.role = Convert.ToInt32(read_user.GetValue(4));
                    }
                    read_user.Close();
                    data.CloseCon();

                    if (textBoxLogin.Text == info.login && textBoxPassword.Text == info.pass)
                    {
                        if (info.role == 1) { new Manager().Show(); Hide(); MessageBox.Show($"Добро пожаловать в магазин {info.fio}", "Сообщение", MessageBoxButtons.OK); }
                        else if (info.role == 2) { new User().Show(); Hide(); }
                        else { new Admin().Show(); Hide(); }
                    }
                    else
                    {
                        MessageBox.Show("Неверные данные прфиля ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else 
                { 
                    new User().Show(); Hide();
                    //MessageBox.Show("Заполните поля для авторизации", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Authorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
