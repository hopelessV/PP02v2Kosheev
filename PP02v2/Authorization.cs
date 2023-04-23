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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = System.Windows.Forms.Application;
using Image = System.Drawing.Image;

namespace PP02v2
{
    public partial class Authorization : Form
    {
        data_base data = new data_base();
        public static string text = String.Empty;
        int attemp = 2;
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
                        attemp--;
                        if (attemp == 1)
                        {
                            pictureBox2.Visible = true;
                            textBoxCapcha.Visible = true;
                        }
                    }
                }
                else 
                { 
                    new User().Show(); Hide();
                }
            }
            catch(Exception ex) { MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Authorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBoxPassword.UseSystemPasswordChar = false;
            }
            else
            {
                textBoxPassword.UseSystemPasswordChar = true;
            }
        }
        private void GenerateCapcha_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
            textBoxCapcha.Text = "";
        }
        public Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);

            //Вычислим позицию текста
            int Xpos = 10;
            int Ypos = 10;

            //Добавим различные цвета ддя текста
            Brush[] colors = {
            Brushes.Black,
            Brushes.Red,
            Brushes.RoyalBlue,
            Brushes.Green,
            Brushes.Yellow,
            Brushes.White,
            Brushes.Tomato,
            Brushes.Sienna,
            Brushes.Pink };

            //Добавим различные цвета линий
            Pen[] colorpens = {
            Pens.Black,
            Pens.Red,
            Pens.RoyalBlue,
            Pens.Green,
            Pens.Yellow,
            Pens.White,
            Pens.Tomato,
            Pens.Sienna,
            Pens.Pink };

            //Делаем случайный стиль текста
            FontStyle[] fontstyle = {
            FontStyle.Bold,
            FontStyle.Italic,
            FontStyle.Regular,
            FontStyle.Strikeout,
            FontStyle.Underline};

            //Добавим различные углы поворота текста
            Int16[] rotate = { 1, -1, 2, -2, 3, -3, 4, -4, 5, -5, 6, -6 };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage((Image)result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Делаем случайный угол поворота текста
            g.RotateTransform(rnd.Next(rotate.Length));

            //Генерируем текст
            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";

            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            //Нарисуем сгенирируемый текст
            g.DrawString(text, new Font("Arial", 25, fontstyle[rnd.Next(fontstyle.Length)]), colors[rnd.Next(colors.Length)], new PointF(Xpos, Ypos));

            //Добавим немного помех
            //Линии из углов
            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
            new Point(0, 0),
            new Point(Width - 1, Height - 1));
            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
            new Point(0, Height - 1),
            new Point(Width - 1, 0));

            //Белые точки
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }
    }
}
