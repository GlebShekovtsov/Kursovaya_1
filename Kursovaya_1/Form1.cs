using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya_1
{
    public partial class Form1 : Form
    {
        const int t = 60;
        int zadumano = 0;
        int ostalos = 60;
        int nomer_popitki = 0;
        DBconnection connect = new DBconnection();
        RecordClass record = new RecordClass();

        public Form1()
        {
            InitializeComponent();
            showRecInfo();
            guna2TextBox2.Focus();
            guna2TextBox1.Enabled = false;
            guna2Button1.Enabled = false;
            guna2Button2.Enabled = false;
            label2.Text = "";
            toolStripStatusLabel1.Text = "У вас осталось: " + Convert.ToString(t) + " сек";
            toolStripStatusLabel2.Text = " Попыток: 0 ";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ostalos--;
            guna2ProgressBar1.Value--;
            toolStripStatusLabel1.Text = "У вас осталось: " + Convert.ToString(ostalos) + " сек";
            if (ostalos == 0)
            {
                timer1.Enabled = false;
                guna2TextBox1.Enabled = false;
                guna2ProgressBar1.Enabled = false;
                label2.Text = "Увы, время истекло...";
            }
        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
              if (e.KeyChar.Equals((char)13))
            {
                try
                {
                    listBox1.Items.Add(guna2TextBox1.Text);
                    if (Convert.ToInt16(guna2TextBox1.Text) == zadumano)
                    {
                        timer1.Enabled = false;
                        guna2TextBox1.Enabled = false;
                        label2.Text = "Вы угадали!, задумывалось число " + Convert.ToString(zadumano);
                        guna2Button1.Enabled = true;
                    };
                    if (Convert.ToInt16(guna2TextBox1.Text) > zadumano) label2.Text = "Задуманное число меньше";
                    if (Convert.ToInt16(guna2TextBox1.Text) < zadumano) label2.Text = "Задуманное число больше";
                }
                catch { label2.Text = "Некорректные входные данные!"; }
                nomer_popitki++;
                toolStripStatusLabel2.Text = " Попыток: " + Convert.ToString(nomer_popitki);
                guna2TextBox1.Text = "";
                guna2TextBox1.Focus();
            }
        }
        bool verify()
        {
            if ((guna2TextBox2.Text == ""))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void showRecInfo()
        {
            guna2DataGridView1.DataSource = record.getreclist();
            DataGridViewColumn Column = new DataGridViewColumn();
            Column = guna2DataGridView1.Columns[2];
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string name = guna2TextBox2.Text;
            string tryes = Convert.ToString(nomer_popitki);


            if (verify())
            {
                try
                {

                    if (record.insertrec(name, tryes))
                    {
                        MessageBox.Show("Новые данные успешно добавлены", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Restart();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Введите своё имя для записи рекорда", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox2.TextLength <= 2)
            {
                guna2Button2.Enabled = false;
            }
            else
            {
                guna2Button2.Enabled = true;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            guna2TextBox1.Enabled = true;
            timer1.Enabled = true;
            timer1.Interval = 1000;
            guna2ProgressBar1.Maximum = t;
            guna2ProgressBar1.Value = t;
            Random n = new Random();
            zadumano = n.Next(100);
        }
    }
}
