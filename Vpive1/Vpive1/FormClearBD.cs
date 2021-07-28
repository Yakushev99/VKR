using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vpive1
{
    public partial class FormClearBD : Form
    {
        public FormClearBD()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Очистить выбранные таблицы?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (textBox2.Text.Trim() == "")
                {
                    MessageBox.Show("Пустое поле!", "Ошибка");
                }
                else
                {
                    string query = "SELECT * FROM Users WHERE  Password=@pass AND id_User=1";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@pass", textBox2.Text);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (checkBox3.Checked)
                        {
                            string query1 = "DELETE FROM RealisationOnNomenclatures";
                            SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                            cmd1.ExecuteNonQuery();
                            cmd1.Dispose();

                            string query2 = "DELETE FROM Realisations";
                            SQLiteCommand cmd3 = new SQLiteCommand(query2, conn);
                            cmd3.ExecuteNonQuery();
                            cmd3.Dispose();

                            string query3 = "DELETE FROM Payments";
                            SQLiteCommand cmd5 = new SQLiteCommand(query3, conn);
                            cmd5.ExecuteNonQuery();
                            cmd5.Dispose();
                        }
                        if (checkBox4.Checked)
                        {
                            string query1 = "DELETE FROM InvoiceTables WHERE Type=3 AND id_InvoiceTable>1";
                            SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                            cmd1.ExecuteNonQuery();
                            cmd1.Dispose();
                        }
                        if (checkBox5.Checked)
                        {
                            string query1 = "DELETE FROM InvoiceTables WHERE Type < 3 AND id_InvoiceTable>1";
                            SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                            cmd1.ExecuteNonQuery();
                            cmd1.Dispose();

                            string query2 = "DELETE FROM InvoiceHeaders";
                            SQLiteCommand cmd3 = new SQLiteCommand(query2, conn);
                            cmd3.ExecuteNonQuery();
                            cmd3.Dispose();
                        }
                        if (checkBox6.Checked)
                        {
                            string query1 = "DELETE FROM Clients WHERE id_Client>1";
                            SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                            cmd1.ExecuteNonQuery();
                            cmd1.Dispose();
                        }
                        if (checkBox7.Checked)
                        {
                            string query1 = "DELETE FROM InvoiceTables WHERE Type=3 AND id_InvoiceTable>1";
                            SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                            cmd1.ExecuteNonQuery();
                            cmd1.Dispose();

                            string query2 = "DELETE FROM Nomenclatures WHERE id_Nomenclature < 52 OR id_Nomenclature > 52";
                            SQLiteCommand cmd3 = new SQLiteCommand(query2, conn);
                            cmd3.ExecuteNonQuery();
                            cmd3.Dispose();
                        }
                        if (checkBox8.Checked)
                        {
                            string query1 = "DELETE FROM Providers";
                            SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                            cmd1.ExecuteNonQuery();
                            cmd1.Dispose();
                        }
                        if (checkBox9.Checked)
                        {
                            string query1 = "DELETE FROM Employees";
                            SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                            cmd1.ExecuteNonQuery();
                            cmd1.Dispose();
                        }
                        this.Hide();
                        cmd.Dispose();
                        reader.Close();
                        conn.Close();
                    }
                    else
                        MessageBox.Show("Неверный пароль!");
                }
            }
            else if (result == DialogResult.No)
            {
                return;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox4.Checked = true;
                checkBox5.Checked = true;
                checkBox6.Checked = true;
                checkBox7.Checked = true;
                checkBox8.Checked = true;
                checkBox9.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;
                checkBox9.Checked = false;
            }
        }
    }
}
