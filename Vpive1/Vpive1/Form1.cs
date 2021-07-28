using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Vpive1
{
    public partial class Form1 : Form
    {
        public int k=0;
        public Form1()
        {
            InitializeComponent();
        }


        private void yt_Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" && textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Пустые поля!", "Ошибка");
            }
            else
            {
                string query = "SELECT * FROM Users WHERE Login=@log AND Password=@pass AND id_User=1";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@log", textBox1.Text);
                cmd.Parameters.AddWithValue("@pass", textBox2.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Form2 form2 = new Form2(1);
                    form2.Show();
                    this.Hide();
                    cmd.Dispose();
                    reader.Close();
                    conn.Close();
                    k = 1;
                }
                else
                {
                    string query1 = "SELECT * FROM Users WHERE Login=@log AND Password=@pass AND id_User>1";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("@log", textBox1.Text);
                    cmd1.Parameters.AddWithValue("@pass", textBox2.Text);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    SQLiteDataReader reader1 = cmd1.ExecuteReader();
                    if (reader1.Read())
                    {
                        Form2 form2 = new Form2(2);
                        form2.Show();
                        this.Hide();
                        cmd1.Dispose();
                        cmd.Dispose();
                        reader1.Close();
                        reader.Close();
                        conn.Close();
                        k = 2;
                    }
                    else
                    {
                        cmd1.Dispose();
                        cmd.Dispose();
                        reader1.Close();
                        reader.Close();
                        conn.Close();
                        MessageBox.Show("Неверный логин/пароль!", "Ошибка");
                    }
                }
            }
        }

        private void yt_Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void yt_Button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button1_Click(sender, e);
            }
        }

        private void yt_Button2_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
                e.Handled = true;
            else return;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
                e.Handled = true;
            else return;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = (char)0;
            }
            else
            {
                textBox2.PasswordChar = (char)42;
            }
        }
    }
}
