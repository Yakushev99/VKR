using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Vpive1
{
    public partial class Form2 : Form
    {
        private int c;
        private int s;
        private int p;
        private int n;
        private int ost;
        private int ind;
        private int k;
        private int dat = 0;
        public string email;
        private double summ, CenaNapit = 0;
        double ostatok = 0, kolichestvo = 0;
        int printed = 0;

        public Form2(int k)
        {
            this.k = k;
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        }


            private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox28.Text = Properties.Settings.Default.UvedomNapit.ToString();
            textBox29.Text = Properties.Settings.Default.UvedomSrok.ToString();
            textBox30.Text = Properties.Settings.Default.Email.ToString();
            if (Properties.Settings.Default.SavedSetting1 == 1)
                checkBox10.Checked = true;
            else
                checkBox10.Checked = false;
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker2.Value = DateTime.Now;

            string query4 = "SELECT id_Realisation, Date AS Date From Realisations WHERE (julianday(Date) - julianday('now'))>0.5 ";

            SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn1.Open();
            SQLiteCommand cmd4 = new SQLiteCommand(query4, conn1);
            SQLiteDataAdapter da4 = new SQLiteDataAdapter(cmd4);
            DataTable dt4 = new DataTable();
            da4.Fill(dt4);
            listBox6.DataSource = dt4;
            listBox6.DisplayMember = "Date";
            listBox6.ValueMember = "id_Realisation";

            string query1 = "SELECT ProductGroups.Name, InvoiceTables.id_Nomenclature,IFNULL(ProductGroups.Name, '')||' '|| IFNULL(Nomenclatures.Name, '')" +
                "||' '||IFNULL(Nomenclatures.Articul, '' ) AS Nazvanie, SrokGodnosti, " +
                "ROUND(julianday(SrokGodnosti) - julianday('now')) AS Srok FROM InvoiceTables " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature = Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup" +
                " WHERE Srok < @uvedomsrok AND Srok > -1 AND Type = 3";

            SQLiteCommand cmd1 = new SQLiteCommand(query1, conn1);
            cmd1.Parameters.AddWithValue("@uvedomsrok", Properties.Settings.Default.UvedomSrok);
            SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            listBox1.DataSource = dt1;
            listBox1.DisplayMember = "Nazvanie";
            listBox1.ValueMember = "id_Nomenclature";
            listBox2.DataSource = dt1;
            listBox2.DisplayMember = "Srok";
            listBox2.ValueMember = "id_Nomenclature";

            string query2 = "SELECT ProductGroups.Name, InvoiceTables.id_Nomenclature,IFNULL(ProductGroups.Name, '')||' '|| IFNULL(Nomenclatures.Name, '')" +
    "||' '||IFNULL(Nomenclatures.Articul, '' ) AS Nazvanie, IFNULL(Amount, '')||' '|| IFNULL(InvoiceTables.EdIzm, '') AS Kolichestvo FROM InvoiceTables " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature = Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup" +
    " WHERE ProductGroups.Name = 'Напитки' AND Amount <= @uvedomnapit AND Type = 3";

            SQLiteCommand cmd2 = new SQLiteCommand(query2, conn1);
            cmd2.Parameters.AddWithValue("@uvedomnapit", Properties.Settings.Default.UvedomNapit);
            SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            listBox4.DataSource = dt2;
            listBox4.DisplayMember = "Nazvanie";
            listBox4.ValueMember = "id_Nomenclature";
            listBox5.DataSource = dt2;
            listBox5.DisplayMember = "Kolichestvo";
            listBox5.ValueMember = "id_Nomenclature";
            if (listBox1.Items.Count > 0 | listBox4.Items.Count > 0)
            {
                tabControl1.SelectedIndex = 4;
                MessageBox.Show("Появились новые уведомления!", "Внимание!");

            }
            if (k == 2)
            {
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Remove(tabPage9);
                tabControl1.TabPages.Remove(tabPage8);
                tabControl1.TabPages.Remove(tabPage7);
                tabControl1.TabPages.Remove(tabPage5);
                tabControl1.TabPages.Remove(tabPage6);
                //yt_Button3.Visible = false;
            }
            else
            {
                string query3 = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')||' '||IFNULL(Nomenclatures.Weight, '') AS Наименование," +
        " PriceSale AS [Цена на продажу], Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
        " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до] FROM InvoiceTables " +
        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND id_InvoiceTable > 1 ORDER BY ProductGroups.Name DESC";
                SQLiteCommand cmd3 = new SQLiteCommand(query3, conn1);
                SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                DataTable dt3 = new DataTable();
                da3.Fill(dt3);
                dataGridView7.DataSource = dt3;
                dataGridView7.Columns[0].Visible = false;
                cmd3.Dispose();
                conn1.Close();
                dataGridView7.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView7.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView7.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView7.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView7.Select();
                tbOstatSearch.Clear();
            }
        }


        // Обработка кнопок Клиент
        private void bAddClient_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
            bAddClient.Enabled = false;
            EditClient.Enabled = false;
            DeleteClient.Enabled = false;
            tbClientFirstName.Enabled = true;
            tbClientLastName.Enabled = true;
            tbClientMiddleName.Enabled = true;
            tbClientPhone.Enabled = true;
            tbClientSkidka.Enabled = true;
            SaveClient.Visible = true;
            CancelClient.Visible = true;
            tbSearchClient.ReadOnly = true;

            tbClientFirstName.Clear();
            tbClientLastName.Clear();
            tbClientMiddleName.Clear();
            tbClientPhone.Clear();
            tbClientSkidka.Clear();
            SaveClient.Visible = true;
            CancelClient.Visible = true;
            tbClientFirstName.Focus();
            tbClientSkidka.Text = "5";
            c = 1;
        }

        private void EditClient_Click_1(object sender, EventArgs e)
        {
            bAddClient.Enabled = false;
            EditClient.Enabled = false;
            DeleteClient.Enabled = false;
            tbClientFirstName.Enabled = true;
            tbClientLastName.Enabled = true;
            tbClientMiddleName.Enabled = true;
            tbClientPhone.Enabled = true;
            tbClientSkidka.Enabled = true;
            SaveClient.Visible = true;
            CancelClient.Visible = true;
            tbSearchClient.ReadOnly = true;
            c = 2;
        }

        private void DeleteClient_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Focused == false)
                MessageBox.Show("Не выбрана запись для удаления!", "Ошибка");
            else if (dataGridView1.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Удалить запись?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Clients WHERE id_Client = @idclient";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(tbidclient.Text));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    query = "SELECT id_Client, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон, Skidka AS Скидка FROM Clients WHERE id_Client > 1";
                    SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    dataGridView1.DataSource = dt1;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    cmd.Dispose();
                    conn.Close();
                    if (dataGridView1.Rows.Count == 0)
                    {
                        EditClient.Enabled = false;
                        DeleteClient.Enabled = false;
                    }
                    MessageBox.Show("Удаление прошло успешно!");
                    dataGridView1.Select();
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void SaveClient_Click(object sender, EventArgs e)
        {
            if ((tbClientFirstName.Text == "") | (tbClientLastName.Text == "") | (tbClientPhone.Text == "+7") | (tbClientSkidka.Text == ""))
                MessageBox.Show("Не все поля заполнены!", "Ошибка");
            else if (c == 1)
            {
                string query = "INSERT INTO Clients (FirstName, LastName, MiddleName, Phone, Skidka) VALUES (@firstname, @lastname, @middlename, @phone, @skidka)";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstname", tbClientFirstName.Text);
                cmd.Parameters.AddWithValue("@lastname", tbClientLastName.Text);
                cmd.Parameters.AddWithValue("@middlename", tbClientMiddleName.Text);
                cmd.Parameters.AddWithValue("@phone", tbClientPhone.Text);
                cmd.Parameters.AddWithValue("@skidka", tbClientSkidka.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                cmd.Dispose();
                conn.Close();

                string query1 = "SELECT id_Client, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон, Skidka AS [Скидка (%)] FROM Clients WHERE id_Client > 1";
                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                cmd1.Dispose();
                conn.Close();
                dataGridView1.DataSource = dt1;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Select();
                MessageBox.Show("Новый клиент добавлен!");

                SaveClient.Visible = false;
                CancelClient.Visible = false;
                tbClientFirstName.Enabled = false;
                tbClientLastName.Enabled = false;
                tbClientMiddleName.Enabled = false;
                tbClientPhone.Enabled = false;
                tbClientSkidka.Enabled = false;
                bAddClient.Enabled = true;
                EditClient.Enabled = true;
                DeleteClient.Enabled = true;
                dataGridView1.Enabled = true;
                tbSearchClient.ReadOnly = false;
                dataGridView1.Focus();
            }
            else if (c == 2)
            {
                string query = "UPDATE Clients SET FirstName = @firstname, LastName = @lastname, MiddleName = @middlename, Phone = @phone, Skidka = @skidka WHERE id_Client = @idclient";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(tbidclient.Text));
                cmd.Parameters.AddWithValue("@firstname", tbClientFirstName.Text);
                cmd.Parameters.AddWithValue("@lastname", tbClientLastName.Text);
                cmd.Parameters.AddWithValue("@middlename", tbClientMiddleName.Text);
                cmd.Parameters.AddWithValue("@phone", tbClientPhone.Text);
                cmd.Parameters.AddWithValue("@skidka", tbClientSkidka.Text);
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                cmd.Dispose();

                query = "SELECT id_Client, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон, Skidka AS [Скидка (%)] FROM Clients WHERE id_Client > 1";
                SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                cmd.Dispose();
                conn.Close();
                dataGridView1.DataSource = dt1;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Select();
                MessageBox.Show("Изменения сохранены!");
                dataGridView1.Select();

                SaveClient.Visible = false;
                CancelClient.Visible = false;
                tbClientFirstName.Enabled = false;
                tbClientLastName.Enabled = false;
                tbClientMiddleName.Enabled = false;
                tbClientPhone.Enabled = false;
                tbClientSkidka.Enabled = false;
                bAddClient.Enabled = true;
                EditClient.Enabled = true;
                DeleteClient.Enabled = true;
                tbSearchClient.ReadOnly = false;
            }
        }


        private void CancelClient_Click_1(object sender, EventArgs e)
        {
            SaveClient.Visible = false;
            CancelClient.Visible = false;
            tbClientFirstName.Enabled = false;
            tbClientLastName.Enabled = false;
            tbClientMiddleName.Enabled = false;
            tbClientPhone.Enabled = false;
            tbClientSkidka.Enabled = false;
            bAddClient.Enabled = true;
            EditClient.Enabled = true;
            DeleteClient.Enabled = true;
            dataGridView1.Enabled = true;
            tbSearchClient.ReadOnly = false;
            dataGridView1.Focus();
        }


        // Обработка кнопок Сотрудник
        private void AddSotrud_Click(object sender, EventArgs e)
        {
            dataGridView3.Enabled = false;
            AddSotrud.Enabled = false;
            EditSotrud.Enabled = false;
            DeleteSotrud.Enabled = false;
            tbSotrudFirstName.Enabled = true;
            tbSotrudLastName.Enabled = true;
            tbSotrudMiddleName.Enabled = true;
            tbSotrudPhone.Enabled = true;
            SaveSotrud.Visible = true;
            CancelSotrud.Visible = true;
            tbSotrudSearch.ReadOnly = true;

            tbSotrudFirstName.Clear();
            tbSotrudLastName.Clear();
            tbSotrudMiddleName.Clear();
            tbSotrudPhone.Clear();
            SaveSotrud.Visible = true;
            CancelSotrud.Visible = true;
            tbSotrudFirstName.Focus();
            s = 1;
        }


        private void EditSotrud_Click(object sender, EventArgs e)
        {
            AddSotrud.Enabled = false;
            EditSotrud.Enabled = false;
            DeleteSotrud.Enabled = false;
            tbSotrudFirstName.Enabled = true;
            tbSotrudLastName.Enabled = true;
            tbSotrudMiddleName.Enabled = true;
            tbSotrudPhone.Enabled = true;
            SaveSotrud.Visible = true;
            CancelSotrud.Visible = true;
            tbSotrudSearch.ReadOnly = true;
            s = 2;
        }

        private void SaveSotrud_Click(object sender, EventArgs e)
        {
            if ((tbSotrudFirstName.Text == "") | (tbSotrudLastName.Text == "") | (tbSotrudMiddleName.Text == "") | (tbSotrudPhone.Text == "+7"))
                MessageBox.Show("Не все поля заполнены!", "Ошибка");
            else if (s == 1)
            {
                string query = "INSERT INTO Employees (FirstName, LastName, MiddleName, Phone) VALUES (@firstname, @lastname, @middlename, @phone)";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstname", tbSotrudFirstName.Text);
                cmd.Parameters.AddWithValue("@lastname", tbSotrudLastName.Text);
                cmd.Parameters.AddWithValue("@middlename", tbSotrudMiddleName.Text);
                cmd.Parameters.AddWithValue("@phone", tbSotrudPhone.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView3.DataSource = dt;
                dataGridView3.Refresh();
                cmd.Dispose();

                string query1 = "SELECT id_Employee, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон FROM Employees";
                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                cmd1.Dispose();
                conn.Close();
                dataGridView3.DataSource = dt1;
                dataGridView3.Columns[0].Visible = false;
                dataGridView3.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                MessageBox.Show("Новый сотрудник добавлен!");

                SaveSotrud.Visible = false;
                CancelSotrud.Visible = false;
                tbSotrudFirstName.Enabled = false;
                tbSotrudLastName.Enabled = false;
                tbSotrudMiddleName.Enabled = false;
                tbSotrudPhone.Enabled = false;
                AddSotrud.Enabled = true;
                EditSotrud.Enabled = true;
                DeleteSotrud.Enabled = true;
                dataGridView3.Enabled = true;
                tbSotrudSearch.ReadOnly = false;
                dataGridView3.Focus();
            }
            else if (s == 2)
            {
                string query = "UPDATE Employees SET FirstName = @firstname, LastName = @lastname, MiddleName = @middlename, Phone = @phone WHERE id_Employee = @idemployee";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(tbidsotrud.Text));
                cmd.Parameters.AddWithValue("@firstname", tbSotrudFirstName.Text);
                cmd.Parameters.AddWithValue("@lastname", tbSotrudLastName.Text);
                cmd.Parameters.AddWithValue("@middlename", tbSotrudMiddleName.Text);
                cmd.Parameters.AddWithValue("@phone", tbSotrudPhone.Text);
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView3.DataSource = dt;
                cmd.Dispose();

                query = "SELECT id_Employee, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон FROM Employees";
                SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                cmd.Dispose();
                conn.Close();
                dataGridView3.DataSource = dt1;
                dataGridView3.Columns[0].Visible = false;
                dataGridView3.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                MessageBox.Show("Изменения сохранены!");
                dataGridView3.Select();

                SaveSotrud.Visible = false;
                CancelSotrud.Visible = false;
                tbSotrudFirstName.Enabled = false;
                tbSotrudLastName.Enabled = false;
                tbSotrudMiddleName.Enabled = false;
                tbSotrudPhone.Enabled = false;
                AddSotrud.Enabled = true;
                EditSotrud.Enabled = true;
                DeleteSotrud.Enabled = true;
                tbSotrudSearch.ReadOnly = false;
            }
        }

        private void DeleteSotrud_Click(object sender, EventArgs e)
        {
            if (dataGridView3.Focused == false)
                MessageBox.Show("Не выбрана запись для удаления!", "Ошибка");
            else if (dataGridView3.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Удалить запись?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Employees WHERE id_Employee = @idemployee";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(tbidsotrud.Text));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    query = "SELECT id_Employee, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон FROM Employees";
                    SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    cmd.Dispose();
                    conn.Close();
                    dataGridView3.DataSource = dt1;
                    dataGridView3.Columns[0].Visible = false;
                    dataGridView3.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView3.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView3.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView3.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView3.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    if (dataGridView3.Rows.Count == 0)
                    {
                        EditSotrud.Enabled = false;
                        DeleteSotrud.Enabled = false;
                    }
                    MessageBox.Show("Удаление прошло успешно!");
                    dataGridView3.Select();
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void CancelSotrud_Click(object sender, EventArgs e)
        {
            AddSotrud.Enabled = true;
            EditSotrud.Enabled = true;
            DeleteSotrud.Enabled = true;
            tbSotrudFirstName.Enabled = false;
            tbSotrudLastName.Enabled = false;
            tbSotrudMiddleName.Enabled = false;
            tbSotrudPhone.Enabled = false;
            SaveSotrud.Visible = false;
            CancelSotrud.Visible = false;
            dataGridView3.Enabled = true;
            tbSotrudSearch.ReadOnly = false;
            dataGridView3.Focus();
        }

        // выключаем поля ввода, когда переходим на другую вкладку
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;              // Накладные
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;

            SaveClient.Visible = false;             //Клиент
            CancelClient.Visible = false;
            tbClientFirstName.Enabled = false;
            tbClientLastName.Enabled = false;
            tbClientMiddleName.Enabled = false;
            tbClientPhone.Enabled = false;
            tbClientSkidka.Enabled = false;
            bAddClient.Enabled = true;
            dataGridView1.Enabled = true;
            tbSearchClient.ReadOnly = false;
            //EditClient.Enabled = true;
            //DeleteClient.Enabled = true;

            SaveSotrud.Visible = false;             // Сотрудник
            CancelSotrud.Visible = false;
            tbSotrudFirstName.Enabled = false;
            tbSotrudLastName.Enabled = false;
            tbSotrudMiddleName.Enabled = false;
            tbSotrudPhone.Enabled = false;
            AddSotrud.Enabled = true;
            dataGridView3.Enabled = true;
            tbSotrudSearch.ReadOnly = false;
            //EditSotrud.Enabled = true;
            //DeleteSotrud.Enabled = true;

            SavePostav.Visible = false;             // Контрагент
            CancelPostav.Visible = false;
            tbPostavName.Enabled = false;
            tbPostavAddress.Enabled = false;
            tbPostavPhone.Enabled = false;
            tbPostavDopPhone.Enabled = false;
            tbPostavEmail.Enabled = false;
            AddPostav.Enabled = true;
            dataGridView5.Enabled = true;
            tbPostavSearch.ReadOnly = false;
            // EditPostav.Enabled = true;
            // DeletePostav.Enabled = true;

            SaveNomenclature.Visible = false;             // Номенклатура
            CancelNomenclature.Visible = false;
            tbNomenclatureName.Enabled = false;
            tbNomenclatureArticul.Enabled = false;
            tbNomenclatureMassa.Enabled = false;
            cbEdIzm.Enabled = false;
            cbGroup.Enabled = false;
            AddNomenclature.Enabled = true;
            EditNomenclature.Enabled = true;
            DeleteNomenclature.Enabled = true;
            tbNomenclatureSearch.ReadOnly = false;
            label11.Visible = false;
            comboBox14.Visible = false;
            comboBox14.SelectedIndex = -1;

            comboBox3.Enabled = false;  //Остатки
            textBox2.Enabled = false;
            textBox1.Enabled = false;
            bSaveOstat.Visible = false;
            bCancelOstat.Visible = false;
            bAddOstat.Enabled = true;
            bEditOstat.Enabled = true;
            bDeleteOstat.Enabled = true;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            dataGridView7.Enabled = true;
            tbOstatSearch.ReadOnly = false;

            label66.Visible = false;
            label67.Visible = false;
            textBox23.Visible = false;

            if ((textBox28.Enabled == true) | (textBox29.Enabled == true))
            {
                tabControl1.SelectedIndex = 9;
                MessageBox.Show("Введите данные!");
            }
        }

        //Запрет ввода соответвующих символов в текстбоксы
        private void tbClientFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я') || (e.KeyChar >= 'а' && e.KeyChar <= 'я') || (e.KeyChar == 8) || (e.KeyChar == 45)) return;
            else
                e.Handled = true;
        }

        private void tbClientLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я') || (e.KeyChar >= 'а' && e.KeyChar <= 'я') || (e.KeyChar == 8) || (e.KeyChar == 45)) return;
            else
                e.Handled = true;
        }

        private void tbClientMiddleName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я') || (e.KeyChar >= 'а' && e.KeyChar <= 'я') || (e.KeyChar == 8) || (e.KeyChar == 45)) return;
            else
                e.Handled = true;
        }

        private void tbClientSkidka_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar)) || (e.KeyChar == 8)) return;
            else
                e.Handled = true;
        }

        private void tbSotrudFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я') || (e.KeyChar >= 'а' && e.KeyChar <= 'я') || (e.KeyChar == 8) || (e.KeyChar == 45)) return;
            else
                e.Handled = true;
        }

        private void tbSotrudLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я') || (e.KeyChar >= 'а' && e.KeyChar <= 'я') || (e.KeyChar == 8) || (e.KeyChar == 45)) return;
            else
                e.Handled = true;
        }

        private void tbSotrudMiddleName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я') || (e.KeyChar >= 'а' && e.KeyChar <= 'я') || (e.KeyChar == 8) || (e.KeyChar == 45)) return;
            else
                e.Handled = true;
        }


        private void tbNomenclatureMassa_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        // Поиск по таблице Клиенты
        private void tbSearchClient_TextChanged(object sender, EventArgs e)
        {
            string query = "SELECT id_Client, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон, Skidka AS Скидка FROM Clients WHERE (FirstName LIKE '%" + tbSearchClient.Text +
                "%' or LastName LIKE '%" + tbSearchClient.Text + "%' or MiddleName LIKE '%" + tbSearchClient.Text + "%' or Phone LIKE '%" + tbSearchClient.Text + "%') AND id_Client > 1";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            cmd.Dispose();
            conn.Close();
        }

        // Поиск по таблице Сотрудники
        private void tbSotrudSearch_TextChanged(object sender, EventArgs e)
        {
            string query = "SELECT id_Employee, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон FROM Employees WHERE (FirstName LIKE '%" + tbSotrudSearch.Text +
    "%' or LastName LIKE '%" + tbSotrudSearch.Text + "%' or MiddleName LIKE '%" + tbSotrudSearch.Text + "%' or Phone LIKE '%" + tbSotrudSearch.Text + "%')";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView3.DataSource = dt;
            dataGridView3.Columns[0].Visible = false;
            cmd.Dispose();
            conn.Close();
        }

        // Поиск по таблице Контрагенты
        private void tbPostavSearch_TextChanged(object sender, EventArgs e)
        {
            string query = "SELECT id_Provider, Type AS Тип, Name AS Наименование, Address AS Адрес, Phone AS Телефон, DopPhone AS [Доп телефон], Email, OKPO AS ОКПО, INN AS ИНН FROM Providers" +
                "WHERE (Type LIKE '%" + tbPostavSearch.Text + "%' or Name LIKE '%" + tbPostavSearch.Text +
            "%' or Address LIKE '%" + tbPostavSearch.Text + "%' or Phone LIKE '%" + tbPostavSearch.Text + "%')";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView5.DataSource = dt;
            cmd.Dispose();
            conn.Close();
            dataGridView5.Columns[0].Visible = false;
            dataGridView5.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        // Поиск по таблице Номенклатура
        private void tbNomenclatureSearch_TextChanged(object sender, EventArgs e)
        {
            string query = "SELECT id_Nomenclature, Nomenclatures.id_ProductGroup, ProductGroups.Name AS Группа, Nomenclatures.Name AS Наименование, Articul AS Артикул, " +
    "Weight AS [Масса (нетто)], EdIzm AS [Ед Измерения] FROM Nomenclatures " +
    "LEFT JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup WHERE (ProductGroups.Name LIKE '%" + tbNomenclatureSearch.Text + "%' or Nomenclatures.Name LIKE '%" + tbNomenclatureSearch.Text + "%' " +
    " or Articul LIKE '%" + tbNomenclatureSearch.Text + "%' or Weight LIKE '%" + tbNomenclatureSearch.Text + "%') ";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView6.DataSource = dt;
            dataGridView6.Columns[0].Visible = false;
            dataGridView6.Columns[1].Visible = false;
            cmd.Dispose();
            conn.Close();
        }

        // Поиск по таблице Остатки
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            string query = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Articul, '' )||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
" PriceSale AS [Цена на продажу], Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения],  strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до] FROM InvoiceTables " +
" LEFT JOIN Nomenclatures ON Nomenclatures.id_Nomenclature = InvoiceTables.id_Nomenclature " +
" LEFT JOIN ProductGroups ON ProductGroups.id_ProductGroup = Nomenclatures.id_ProductGroup" +
" WHERE (ProductGroups.Name LIKE '%" + tbOstatSearch.Text + "%' or Nomenclatures.Name LIKE '%" + tbOstatSearch.Text + "%' " +
" or Articul LIKE '%" + tbOstatSearch.Text + "%') AND id_InvoiceTable>1 AND Type =3 ORDER BY ProductGroups.Name DESC";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView7.DataSource = dt;
            dataGridView7.Columns[0].Visible = false;
            cmd.Dispose();
            conn.Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textBox3.Text != "")
            {
                DialogResult result1 = MessageBox.Show("Отменить продажу?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result1 == DialogResult.Yes)
                {
                    int rows = dataGridView8.Rows.Count;
                    for (int i = 0; i <= rows; i++)
                    {
                        yt_Button15_Click(sender, e);
                    }

                    textBox3.Clear();
                    comboBox9.SelectedIndex = -1;
                }
                else if (result1 == DialogResult.No)
                {
                    e.Cancel = true;
                    tabControl1.SelectedIndex = 0;
                    return;
                }
            }
            e.Cancel = true;
            if (DialogResult.Yes == MessageBox.Show("Закрыть приложение?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                e.Cancel = false;
        }


        // Настройки админа
        private void ChangeAdminPass_Click(object sender, EventArgs e)
        {
            Login1.Visible = true;
            Pass1.Visible = true;
            Login2.Visible = true;
            Pass2.Visible = true;
            tbLogin1.Visible = true;
            tbPass1.Visible = true;
            tbLogin2.Visible = true;
            tbPass2.Visible = true;
            OkNewPass.Visible = true;
            CancelNewPass.Visible = true;
            ShowPass1.Visible = true;
            ShowPass2.Visible = true;
        }

        private void ShowPass1_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPass1.Checked)
            {
                tbPass1.PasswordChar = (char)0;
            }
            else
            {
                tbPass1.PasswordChar = (char)42;
            }
        }

        private void ShowPass2_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPass2.Checked)
            {
                tbPass2.PasswordChar = (char)0;
            }
            else
            {
                tbPass2.PasswordChar = (char)42;
            }
        }

        private void CancelNewPass_Click(object sender, EventArgs e)
        {
            Login1.Visible = false;
            Pass1.Visible = false;
            Login2.Visible = false;
            Pass2.Visible = false;
            tbLogin1.Visible = false;
            tbPass1.Visible = false;
            tbLogin2.Visible = false;
            tbPass2.Visible = false;
            OkNewPass.Visible = false;
            CancelNewPass.Visible = false;
            ShowPass1.Visible = false;
            ShowPass2.Visible = false;
        }



        private void AddPostav_Click(object sender, EventArgs e)
        {
            dataGridView5.Enabled = false;
            AddPostav.Enabled = false;
            EditPostav.Enabled = false;
            DeletePostav.Enabled = false;
            cbProviderType.Enabled = true;
            tbPostavName.Enabled = true;
            tbPostavAddress.Enabled = true;
            tbPostavPhone.Enabled = true;
            tbPostavDopPhone.Enabled = true;
            tbPostavEmail.Enabled = true;
            SavePostav.Visible = true;
            CancelPostav.Visible = true;
            tbPostavSearch.ReadOnly = true;
            cbProviderType.SelectedIndex = -1;
            textBox31.Enabled = true;
            textBox32.Enabled = true;

            tbPostavName.Clear();
            tbPostavAddress.Clear();
            tbPostavPhone.Clear();
            tbPostavDopPhone.Clear();
            tbPostavEmail.Clear();
            textBox31.Clear();
            textBox32.Clear();
            cbProviderType.Focus();
            p = 1;
        }

        private void EditPostav_Click(object sender, EventArgs e)
        {
            AddPostav.Enabled = false;
            EditPostav.Enabled = false;
            DeletePostav.Enabled = false;
            tbPostavName.Enabled = true;
            tbPostavAddress.Enabled = true;
            tbPostavPhone.Enabled = true;
            tbPostavDopPhone.Enabled = true;
            tbPostavEmail.Enabled = true;
            SavePostav.Visible = true;
            CancelPostav.Visible = true;
            cbProviderType.Enabled = true;
            tbPostavSearch.ReadOnly = true;
            textBox31.Enabled = true;
            textBox32.Enabled = true;
            p = 2;
        }


        private void DeletePostav_Click(object sender, EventArgs e)
        {
            if (dataGridView5.Focused == false)
                MessageBox.Show("Не выбрана запись для удаления!", "Ошибка");
            else if (dataGridView5.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Удалить запись?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Providers WHERE id_Provider = @idprovider";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idprovider", Convert.ToInt32(tbidprovider.Text));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    query = "SELECT id_Provider, Type AS Тип, Name AS Наименование, Address AS Адрес, Phone AS Телефон, DopPhone AS [Доп телефон], Email, OKPO AS ОКПО, INN AS ИНН FROM Providers";
                    SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    dataGridView5.DataSource = dt1;
                    dataGridView5.Columns[0].Visible = false;
                    cmd.Dispose();
                    dataGridView5.Select();
                    conn.Close();

                    dataGridView5.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView5.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView5.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView5.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView5.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView5.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView5.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView5.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                    if (dataGridView5.Rows.Count == 0)
                    {
                        EditPostav.Enabled = false;
                        DeletePostav.Enabled = false;
                    }
                        MessageBox.Show("Удаление прошло успешно!");
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void CancelPostav_Click(object sender, EventArgs e)
        {
            AddPostav.Enabled = true;
            EditPostav.Enabled = true;
            DeletePostav.Enabled = true;
            cbProviderType.Enabled = false;
            tbPostavName.Enabled = false;
            tbPostavAddress.Enabled = false;
            tbPostavPhone.Enabled = false;
            tbPostavDopPhone.Enabled = false;
            tbPostavEmail.Enabled = false;
            SavePostav.Visible = false;
            CancelPostav.Visible = false;
            dataGridView5.Enabled = true;
            tbPostavSearch.ReadOnly = false;
            textBox31.Enabled = false;
            textBox32.Enabled = false;
            dataGridView5.Focus();
        }


        private void SavePostav_Click(object sender, EventArgs e)
        {
            if ((tbPostavName.Text == "") | (tbPostavAddress.Text == "") | (cbProviderType.SelectedIndex == -1))
                MessageBox.Show("Не все поля заполнены!", "Ошибка");
            else if (p == 1)
            {
                string query = "INSERT INTO Providers (Type, Name, Address, Phone, DopPhone, Email) VALUES (@type, @name, @address, @phone, @dopphone, @email)";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@type", cbProviderType.Text);
                cmd.Parameters.AddWithValue("@name", tbPostavName.Text);
                cmd.Parameters.AddWithValue("@address", tbPostavAddress.Text);
                cmd.Parameters.AddWithValue("@phone", tbPostavPhone.Text);
                cmd.Parameters.AddWithValue("@dopphone", tbPostavDopPhone.Text);
                cmd.Parameters.AddWithValue("@email", tbPostavEmail.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView5.DataSource = dt;
                cmd.Dispose();
                conn.Close();

                string query1 = "SELECT id_Provider, Type AS Тип, Name AS Наименование, Address AS Адрес, Phone AS Телефон, DopPhone AS [Доп телефон], Email, OKPO AS ОКПО, INN AS ИНН FROM Providers";
                SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn1);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                dataGridView5.DataSource = dt1;
                dataGridView5.Columns[0].Visible = false;
                dataGridView5.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                cmd1.Dispose();
                conn1.Close();
                MessageBox.Show("Новый поставщик добавлен!");

                SavePostav.Visible = false;
                CancelPostav.Visible = false;
                tbPostavName.Enabled = false;
                tbPostavAddress.Enabled = false;
                tbPostavPhone.Enabled = false;
                tbPostavDopPhone.Enabled = false;
                tbPostavEmail.Enabled = false;
                cbProviderType.Enabled = false;
                textBox31.Enabled = false;
                textBox32.Enabled = false;
                AddPostav.Enabled = true;
                EditPostav.Enabled = true;
                DeletePostav.Enabled = true;
                dataGridView5.Enabled = true;
                tbPostavSearch.ReadOnly = false;
                dataGridView5.Focus();
            }
            else if (p == 2)
            {
                string query = "UPDATE Providers SET Type=@type, Name = @name, Address = @address, Phone = @phone, DopPhone = @dopphone, Email = @email WHERE id_Provider = @idprovider";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@idprovider", Convert.ToInt32(tbidprovider.Text));
                cmd.Parameters.AddWithValue("@type", cbProviderType.Text);
                cmd.Parameters.AddWithValue("@name", tbPostavName.Text);
                cmd.Parameters.AddWithValue("@address", tbPostavAddress.Text);
                cmd.Parameters.AddWithValue("@phone", tbPostavPhone.Text);
                cmd.Parameters.AddWithValue("@dopphone", tbPostavDopPhone.Text);
                cmd.Parameters.AddWithValue("@email", tbPostavEmail.Text);
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView5.DataSource = dt;
                cmd.Dispose();
                conn.Close();

                query = "SELECT id_Provider, Type AS Тип, Name AS Наименование, Address AS Адрес, Phone AS Телефон, DopPhone AS [Доп телефон], Email, OKPO AS ОКПО, INN AS ИНН FROM Providers";
                conn.Open();
                SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                dataGridView5.DataSource = dt1;
                dataGridView5.Columns[0].Visible = false;
                cmd.Dispose();
                conn.Close();
                MessageBox.Show("Изменения сохранены!");
                dataGridView5.Focus();
                dataGridView5.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;

                SavePostav.Visible = false;
                CancelPostav.Visible = false;
                tbPostavName.Enabled = false;
                tbPostavAddress.Enabled = false;
                tbPostavPhone.Enabled = false;
                tbPostavDopPhone.Enabled = false;
                tbPostavEmail.Enabled = false;
                cbProviderType.Enabled = false;
                textBox31.Enabled = false;
                textBox32.Enabled = false;
                AddPostav.Enabled = true;
                EditPostav.Enabled = true;
                DeletePostav.Enabled = true;
                tbPostavSearch.ReadOnly = false;
            }
        }



        //Добавить накладную
        private void yt_Button1_Click(object sender, EventArgs e)
        {
            FormInvoiceAdd forminvoiceadd = new FormInvoiceAdd();
            forminvoiceadd.Owner = this;
            forminvoiceadd.ShowDialog();
        }

        private void AddNomenclature_Click(object sender, EventArgs e)
        {
            label11.Visible = true;
            comboBox14.Visible = true;
            comboBox14.SelectedIndex = -1;
            dataGridView6.Enabled = false;
            AddNomenclature.Enabled = false;
            EditNomenclature.Enabled = false;
            DeleteNomenclature.Enabled = false;
            cbGroup.Enabled = true;
            tbNomenclatureName.Enabled = true;
            tbNomenclatureArticul.Enabled = true;
            tbNomenclatureMassa.Enabled = true;
            cbEdIzm.Enabled = true;
            SaveNomenclature.Visible = true;
            CancelNomenclature.Visible = true;
            tbNomenclatureSearch.ReadOnly = true;
            cbEdIzm.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbEdIzm.AutoCompleteSource = AutoCompleteSource.ListItems;

            cbGroup.SelectedIndex = -1;
            comboBox1.SelectedIndex = 0;
            tbNomenclatureName.Clear();
            tbNomenclatureArticul.Clear();
            tbNomenclatureMassa.Clear();
            cbEdIzm.SelectedIndex = -1;
            cbGroup.Focus();
            n = 1;
        }

        private void EditNomenclature_Click(object sender, EventArgs e)
        {
            AddNomenclature.Enabled = false;
            EditNomenclature.Enabled = false;
            DeleteNomenclature.Enabled = false;
            cbGroup.Enabled = true;
            tbNomenclatureName.Enabled = true;
            tbNomenclatureArticul.Enabled = true;
            tbNomenclatureMassa.Enabled = true;
            cbEdIzm.Enabled = true;
            SaveNomenclature.Visible = true;
            CancelNomenclature.Visible = true;
            tbNomenclatureSearch.ReadOnly = true;
            n = 2;
        }

        private void DeleteNomenclature_Click(object sender, EventArgs e)
        {
            if (dataGridView6.Focused == false)
                MessageBox.Show("Не выбрана запись для удаления!", "Ошибка");
            else if (dataGridView6.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Удалить запись?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Nomenclatures WHERE id_Nomenclature = @idnomenclature";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(tbidnomenclature.Text));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    query = "SELECT id_Nomenclature, Nomenclatures.id_ProductGroup, ProductGroups.Name AS Группа, Nomenclatures.Name AS Наименование, Articul AS Артикул, " +
                        "Weight AS [Масса (нетто)], EdIzm AS [Ед Измерения] FROM Nomenclatures " +
                        "LEFT JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup " +
                        "WHERE Nomenclatures.Name <> '-' ORDER BY ProductGroups.Name DESC, Nomenclatures.Name";
                    SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    dataGridView6.DataSource = dt1;
                    dataGridView6.Columns[0].Visible = false;
                    dataGridView6.Columns[1].Visible = false;
                    dataGridView6.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    cmd1.Dispose();
                    conn.Close();
                    MessageBox.Show("Удаление прошло успешно!");
                    dataGridView6.Select();
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void CancelNomenclature_Click(object sender, EventArgs e)
        {
            label11.Visible = false;
            comboBox14.Visible = false;
            comboBox14.SelectedIndex = -1;
            SaveNomenclature.Visible = false;
            CancelNomenclature.Visible = false;
            tbNomenclatureName.Enabled = false;
            tbNomenclatureArticul.Enabled = false;
            tbNomenclatureMassa.Enabled = false;
            cbEdIzm.Enabled = false;
            cbGroup.Enabled = false;
            AddNomenclature.Enabled = true;
            EditNomenclature.Enabled = true;
            DeleteNomenclature.Enabled = true;
            dataGridView6.Enabled = true;
            tbNomenclatureSearch.ReadOnly = false;
            cbEdIzm.SelectedIndex = -1;
            dataGridView6.Focus();
        }

        private void SaveNomenclature_Click(object sender, EventArgs e)
        {
            if (tbNomenclatureName.Text == "")
                MessageBox.Show("Не заполнено наименование!", "Ошибка");
            else if (n == 1)
            {
                if (comboBox14.Text.Trim() == "")
                    MessageBox.Show("Введите единицу измерения для продажи!", "Ошибка");
                else
                {
                    string query = "INSERT INTO Nomenclatures (id_ProductGroup, Name, Articul, Weight, EdIzm) VALUES (@idproductgroup, @name, @articul, @weight, @edizm)";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idproductgroup", Convert.ToInt32(comboBox1.Text));
                    cmd.Parameters.AddWithValue("@name", tbNomenclatureName.Text);
                    cmd.Parameters.AddWithValue("@articul", tbNomenclatureArticul.Text);
                    cmd.Parameters.AddWithValue("@weight", tbNomenclatureMassa.Text);
                    cmd.Parameters.AddWithValue("@edizm", cbEdIzm.Text);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView6.DataSource = dt;
                    cmd.Dispose();

                    query = "SELECT MAX(id_Nomenclature) FROM Nomenclatures ";
                    SQLiteCommand cmd4 = new SQLiteCommand(query, conn);
                    Int64 idnomen = (Int64)cmd4.ExecuteScalar();
                    cmd4.Dispose();


                    query = "INSERT INTO InvoiceTables (id_InvoiceHeader, id_Nomenclature, Type, PriceSale, Amount, Edizm, SrokGodnosti, Price, Summ) " +
    "VALUES (@idinvoiceheader, @idnomenclature, @type, @pricesale, @amount, @edizm, @srokgodnosti, @price, @summ)";
                    SQLiteCommand cmd2 = new SQLiteCommand(query, conn);
                    cmd2.Parameters.AddWithValue("@idinvoiceheader", 0);
                    cmd2.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(idnomen));
                    cmd2.Parameters.AddWithValue("@type", 3);
                    cmd2.Parameters.AddWithValue("@pricesale", Convert.ToDouble(0));
                    cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(0));
                    cmd2.Parameters.AddWithValue("@edizm", comboBox14.Text);
                    cmd2.Parameters.AddWithValue("@srokgodnosti", "-");
                    cmd2.Parameters.AddWithValue("@price", 0);
                    cmd2.Parameters.AddWithValue("@summ", 0);
                    SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    dataGridView6.DataSource = dt2;
                    cmd2.Dispose();

                    string query1 = "SELECT id_Nomenclature, Nomenclatures.id_ProductGroup, ProductGroups.Name AS Группа, Nomenclatures.Name AS Наименование, Articul AS Артикул, " +
                            "Weight AS [Масса (нетто)], EdIzm AS [Ед Измерения] FROM Nomenclatures " +
                            "LEFT JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup " +
                            "WHERE Nomenclatures.Name <> '-' ORDER BY ProductGroups.Name DESC, Nomenclatures.Name ";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    dataGridView6.DataSource = dt1;
                    dataGridView6.Columns[0].Visible = false;
                    dataGridView6.Columns[1].Visible = false;
                    dataGridView6.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView6.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    cmd1.Dispose();
                    conn.Close();

                    SaveNomenclature.Visible = false;
                    CancelNomenclature.Visible = false;
                    tbNomenclatureName.Enabled = false;
                    tbNomenclatureArticul.Enabled = false;
                    tbNomenclatureMassa.Enabled = false;
                    cbEdIzm.Enabled = false;
                    cbGroup.Enabled = false;
                    AddNomenclature.Enabled = true;
                    EditNomenclature.Enabled = true;
                    DeleteNomenclature.Enabled = true;
                    dataGridView6.Enabled = true;
                    tbNomenclatureSearch.ReadOnly = false;
                    cbEdIzm.SelectedIndex = -1;
                    dataGridView6.Focus();
                    label11.Visible = false;
                    comboBox14.Visible = false;
                    comboBox14.SelectedIndex = -1;
                }
            }
            else if (n == 2)
            {
                string query = "UPDATE Nomenclatures SET id_ProductGroup = @idproductgroup, Name = @name, Articul = @articul, Weight = @weight, EdIzm = @edizm WHERE id_Nomenclature = @idnomenclature";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(tbidnomenclature.Text));
                cmd.Parameters.AddWithValue("@idproductgroup", Convert.ToInt32(comboBox1.Text));
                cmd.Parameters.AddWithValue("@name", tbNomenclatureName.Text);
                cmd.Parameters.AddWithValue("@articul", tbNomenclatureArticul.Text);
                cmd.Parameters.AddWithValue("@weight", tbNomenclatureMassa.Text);
                cmd.Parameters.AddWithValue("@edizm", cbEdIzm.Text);
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView6.DataSource = dt;
                cmd.Dispose();
                conn.Close();

                query = "SELECT id_Nomenclature, Nomenclatures.id_ProductGroup, ProductGroups.Name AS Группа, Nomenclatures.Name AS Наименование, Articul AS Артикул, " +
                        "Weight AS [Масса (нетто)], EdIzm AS [Ед Измерения] FROM Nomenclatures " +
                        "LEFT JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup " +
                        "WHERE Nomenclatures.Name <> '-' ORDER BY ProductGroups.Name DESC, Nomenclatures.Name";
                conn.Open();
                SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                dataGridView6.DataSource = dt1;
                dataGridView6.Columns[0].Visible = false;
                dataGridView6.Columns[1].Visible = false;
                dataGridView6.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                cmd.Dispose();
                conn.Close();
                MessageBox.Show("Изменения сохранены!");
                dataGridView6.Focus();

                SaveNomenclature.Visible = false;
                CancelNomenclature.Visible = false;
                tbNomenclatureName.Enabled = false;
                tbNomenclatureArticul.Enabled = false;
                tbNomenclatureMassa.Enabled = false;
                cbEdIzm.Enabled = false;
                cbGroup.Enabled = false;
                AddNomenclature.Enabled = true;
                EditNomenclature.Enabled = true;
                DeleteNomenclature.Enabled = true;
                tbNomenclatureSearch.ReadOnly = false;
                cbEdIzm.SelectedIndex = -1;
                label11.Visible = false;
                comboBox14.Visible = false;
            }
        }




        //Соединение с табицами при перемещении по вкладкам
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                dateTimePicker5.Value = DateTime.Now.Date;
                checkBox5.Checked = false;
                checkBox4.Checked = false;
                checkBox6.Checked = false;
                comboBox6.Enabled = false;
                comboBox7.Enabled = false;
                yt_Button4.Enabled = false;
                yt_Button5.Enabled = false;
                yt_Button6.Enabled = false;
                yt_Button7.Enabled = false;
                checkBox5.Enabled = false;
                checkBox6.Enabled = false;
                string query = "SELECT id_Client, FirstName||' '||IFNULL(LastName, '')||' '||IFNULL(MiddleName, '' )||' '||IFNULL(Phone, '' ) AS Name, Skidka FROM Clients " +
    "WHERE id_Client > 0";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comboBox8.DataSource = dt;
                comboBox8.DisplayMember = "Name";
                comboBox8.ValueMember = "id_Client";
                comboBox8.SelectedIndex = 0;
                comboBox8.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox8.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBox10.DataSource = dt;
                comboBox10.DisplayMember = "id_Client";
                comboBox10.ValueMember = "id_Client";
                comboBox18.DataSource = dt;
                comboBox18.DisplayMember = "Skidka";
                comboBox18.ValueMember = "id_Client";
               // textBox26.Text = comboBox18.Text;

                query = "SELECT id_Employee, (LastName||' ' ||FirstName||' '||MiddleName) AS FIO FROM Employees";
                SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                comboBox9.DataSource = dt1;
                comboBox9.DisplayMember = "FIO";
                comboBox9.ValueMember = "id_Employee";
                comboBox9.SelectedIndex = -1;
                comboBox11.DataSource = dt1;
                comboBox11.DisplayMember = "id_Employee";
                comboBox11.ValueMember = "id_Employee";
                cmd1.Dispose();
                conn.Close();

            }
            else if (tabControl1.SelectedIndex == 1 && k == 1)
            {
                string query1 = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
                    " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                    " PriceSale AS [Цена на продажу], Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
                    " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до] FROM InvoiceTables " +
                    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                    " WHERE Type = 3 AND id_InvoiceTable > 1 ORDER BY ProductGroups.Name DESC, Наименование";

                SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn1.Open();
                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn1);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                dataGridView7.DataSource = dt1;
                dataGridView7.Columns[0].Visible = false;
                cmd1.Dispose();
                conn1.Close();
                dataGridView7.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView7.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView7.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView7.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView7.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView7.Select();
                tbOstatSearch.Clear();
            }
            else if (tabControl1.SelectedIndex == 1 && k == 2)
            {
                checkBox1.Checked = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                string query = "SELECT InvoiceHeaders.id_InvoiceHeader, InvoiceTypes.Name AS Тип, Providers.Name AS Контрагент, " +
                    "Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y', Date) AS Дата, Sum || ' руб' AS Сумма FROM InvoiceHeaders " +
                    "JOIN Providers ON InvoiceHeaders.id_Provider = Providers.id_Provider " +
                    "JOIN InvoiceTypes ON InvoiceHeaders.id_InvoiceType = InvoiceTypes.id_InvoiceTypes " +
                    "JOIN Employees ON InvoiceHeaders.id_Employee = Employees.id_Employee";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                dataGridView2.Columns[0].Visible = false;
                cmd.Dispose();
                conn.Close();
                dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView2.Select();
                label71.Text = dataGridView2.Rows.Count.ToString();

                if (dataGridView2.Rows.Count < 2)
                {
                    string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature, ProductGroups.Name AS Группа, " +
                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                        "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения], Rasfasovka AS [Расфасовка (Уп)], " +
                        " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до], Price || ' руб' AS Цена, Summ || ' руб' AS Сумма,  PriceSale || ' руб' AS [Цена на продажу] FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                        " WHERE Type = 1 OR Type = 2";
                    SQLiteCommand cmd3 = new SQLiteCommand(query1, conn);
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView4.DataSource = dt3;
                    dataGridView4.Columns[0].Visible = false;
                    dataGridView4.Columns[1].Visible = false;
                    cmd3.Dispose();
                    conn.Close();

                    dataGridView4.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView4.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView4.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView4.Select();
                }
            }
            else if (tabControl1.SelectedIndex == 2 && k==1)
            {
                checkBox1.Checked = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                string query = "SELECT InvoiceHeaders.id_InvoiceHeader, InvoiceTypes.Name AS Тип, Providers.Name AS Контрагент, " +
                    "Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y', Date) AS Дата, Sum || ' руб' AS Сумма FROM InvoiceHeaders " +
                    "JOIN Providers ON InvoiceHeaders.id_Provider = Providers.id_Provider " +
                    "JOIN InvoiceTypes ON InvoiceHeaders.id_InvoiceType = InvoiceTypes.id_InvoiceTypes " +
                    "JOIN Employees ON InvoiceHeaders.id_Employee = Employees.id_Employee";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                dataGridView2.Columns[0].Visible = false;
                cmd.Dispose();
                conn.Close();
                dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView2.Select();
                label71.Text = dataGridView2.Rows.Count.ToString();

                if (dataGridView2.Rows.Count < 2)
                {
                    string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature, ProductGroups.Name AS Группа, " +
                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                        "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения], Rasfasovka AS [Расфасовка (Уп)], " +
                        " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до], Price || ' руб' AS Цена, Summ || ' руб' AS Сумма,  PriceSale || ' руб' AS [Цена на продажу] FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                        " WHERE Type = 1 OR Type = 2";
                    SQLiteCommand cmd3 = new SQLiteCommand(query1, conn);
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView4.DataSource = dt3;
                    dataGridView4.Columns[0].Visible = false;
                    dataGridView4.Columns[1].Visible = false;
                    cmd3.Dispose();
                    conn.Close();

                    dataGridView4.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView4.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView4.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView4.Select();
                }
            }
            else if (tabControl1.SelectedIndex == 2 && k == 2)
            {
                string query = "SELECT id_Client, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон, Skidka AS [Скидка (%)] FROM Clients WHERE id_Client > 1";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                cmd.Dispose();
                conn.Close();
                dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Select();
                if (dataGridView1.Rows.Count == 0)
                {
                    EditClient.Enabled = false;
                    DeleteClient.Enabled = false;
                }
                else
                {
                    EditClient.Enabled = true;
                    DeleteClient.Enabled = true;
                }
            
            }
            else if (tabControl1.SelectedIndex == 3 && k==1)
            {
                string query = "SELECT id_Client, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон, Skidka AS [Скидка (%)] FROM Clients WHERE id_Client > 1";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                cmd.Dispose();
                conn.Close();
                dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Select();
                if (dataGridView1.Rows.Count == 0)
                {
                    EditClient.Enabled = false;
                    DeleteClient.Enabled = false;
                }
                else
                {
                    EditClient.Enabled = true;
                    DeleteClient.Enabled = true;
                }
            }
            else if (tabControl1.SelectedIndex == 3 && k == 2)
            {
                listBox3.Items.Clear();

                string query3 = "SELECT id_Realisation, strftime('%d.%m.%Y', Date) AS Date From Realisations WHERE (julianday(Date) - julianday('now'))>0.5 ";

                SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn1.Open();
                SQLiteCommand cmd3 = new SQLiteCommand(query3, conn1);
               // cmd3.Parameters.AddWithValue("@uvedomsrok", Properties.Settings.Default.UvedomSrok);
                SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                DataTable dt3 = new DataTable();
                da3.Fill(dt3);
                listBox6.DataSource = dt3;
                listBox6.DisplayMember = "Date";
                listBox6.ValueMember = "id_Realisation";

                string query1 = "SELECT ProductGroups.Name, InvoiceTables.id_Nomenclature,IFNULL(ProductGroups.Name, '')||' '|| IFNULL(Nomenclatures.Name, '')" +
    "||' '||IFNULL(Nomenclatures.Articul, '' )||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie, SrokGodnosti, " +
    "ROUND(julianday(SrokGodnosti) - julianday('now')) AS Srok FROM InvoiceTables " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature = Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup" +
    " WHERE Srok < @uvedomsrok AND Srok > -1 AND Type = 3";

                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn1);
                cmd1.Parameters.AddWithValue("@uvedomsrok", Properties.Settings.Default.UvedomSrok);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                listBox1.DataSource = dt1;
                listBox1.DisplayMember = "Nazvanie";
                listBox1.ValueMember = "id_Nomenclature";
                listBox2.DataSource = dt1;
                listBox2.DisplayMember = "Srok";
                listBox2.ValueMember = "id_Nomenclature";

                string query2 = "SELECT ProductGroups.Name, InvoiceTables.id_Nomenclature,IFNULL(ProductGroups.Name, '')||' '|| IFNULL(Nomenclatures.Name, '')" +
        "||' '||IFNULL(Nomenclatures.Articul, '' )||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie, IFNULL(Amount, '')||' '|| IFNULL(InvoiceTables.EdIzm, '') AS Kolichestvo FROM InvoiceTables " +
        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature = Nomenclatures.id_Nomenclature " +
        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup" +
        " WHERE ProductGroups.Name = 'Напитки' AND Amount <= @uvedomnapit AND Type = 3";

                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn1);
                cmd2.Parameters.AddWithValue("@uvedomnapit", Properties.Settings.Default.UvedomNapit);
                SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                listBox4.DataSource = dt2;
                listBox4.DisplayMember = "Nazvanie";
                listBox4.ValueMember = "id_Nomenclature";
                listBox5.DataSource = dt2;
                listBox5.DisplayMember = "Kolichestvo";
                listBox5.ValueMember = "id_Nomenclature";
                if (listBox1.Items.Count > 0 | listBox4.Items.Count > 0| listBox6.Items.Count > 0)
                {
                    for (int index = 0; index < listBox1.Items.Count; index++)
                    {
                        listBox1.SelectedIndex = index;
                        DataRowView drv = (DataRowView)listBox1.SelectedItem;
                        String valueOfItem = drv["Nazvanie"].ToString();
                        String valueOfItem2 = drv["Srok"].ToString();

                        if (Convert.ToInt32(valueOfItem2) < 1)
                        {
                            listBox3.Items.Add("Закончился срок годности " + valueOfItem);
                        }
                        else if (valueOfItem2 == "1")
                        {
                            listBox3.Items.Add("Остался " + valueOfItem2 + " день до окончания срока годности " + valueOfItem);
                        }
                        else if ((valueOfItem2 == "2") | (valueOfItem2 == "3") | (valueOfItem2 == "4"))
                        {
                            listBox3.Items.Add("Осталось " + valueOfItem2 + " дня до окончания срока годности " + valueOfItem);

                        }
                        else
                        {
                            listBox3.Items.Add("Осталось " + valueOfItem2 + " дней до окончания срока годности " + valueOfItem);
                        }

                    }
                    for (int index = 0; index < listBox4.Items.Count; index++)
                    {
                        listBox4.SelectedIndex = index;
                        DataRowView drv2 = (DataRowView)listBox4.SelectedItem;
                        String valueOfItem3 = drv2["Nazvanie"].ToString();
                        String valueOfItem4 = drv2["Kolichestvo"].ToString();
                        listBox3.Items.Add(valueOfItem3 + "осталось всего " + valueOfItem4 + ", необходима закупка!");
                    }
                    for (int index = 0; index < listBox6.Items.Count; index++)
                    {
                        listBox6.SelectedIndex = index;
                        DataRowView drv3 = (DataRowView)listBox6.SelectedItem;
                        DateTime valueOfItem6 = Convert.ToDateTime(drv3["Date"]);
                        
                        listBox3.Items.Add("Предзаказ на дату " + valueOfItem6.ToShortDateString() + " (состав в списке продаж)!");
                    }
                    cmd1.Dispose();
                    cmd2.Dispose();
                    cmd3.Dispose();
                }
                conn1.Close();
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                listBox3.Items.Clear();

                string query3 = "SELECT id_Realisation, strftime('%d.%m.%Y', Date) AS Date From Realisations WHERE (julianday(Date) - julianday('now'))>0.5 ";

                SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn1.Open();
                SQLiteCommand cmd3 = new SQLiteCommand(query3, conn1);
                SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                DataTable dt3 = new DataTable();
                da3.Fill(dt3);
                listBox6.DataSource = dt3;
                listBox6.DisplayMember = "Date";
                listBox6.ValueMember = "id_Realisation";

                string query1 = "SELECT ProductGroups.Name, InvoiceTables.id_Nomenclature,IFNULL(ProductGroups.Name, '')||' '|| IFNULL(Nomenclatures.Name, '')" +
    "||' '||IFNULL(Nomenclatures.Articul, '' )||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie, SrokGodnosti, " +
    "ROUND(julianday(SrokGodnosti) - julianday('now')) AS Srok FROM InvoiceTables " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature = Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup" +
    " WHERE Srok < @uvedomsrok AND Srok > -1 AND Type = 3";

                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn1);
                cmd1.Parameters.AddWithValue("@uvedomsrok", Properties.Settings.Default.UvedomSrok);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                listBox1.DataSource = dt1;
                listBox1.DisplayMember = "Nazvanie";
                listBox1.ValueMember = "id_Nomenclature";
                listBox2.DataSource = dt1;
                listBox2.DisplayMember = "Srok";
                listBox2.ValueMember = "id_Nomenclature";

                string query2 = "SELECT ProductGroups.Name, InvoiceTables.id_Nomenclature,IFNULL(ProductGroups.Name, '')||' '|| IFNULL(Nomenclatures.Name, '')" +
        "||' '||IFNULL(Nomenclatures.Articul, '' )||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie, IFNULL(Amount, '')||' '|| IFNULL(InvoiceTables.EdIzm, '') AS Kolichestvo FROM InvoiceTables " +
        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature = Nomenclatures.id_Nomenclature " +
        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup" +
        " WHERE ProductGroups.Name = 'Напитки' AND Amount <= @uvedomnapit AND Type = 3";

                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn1);
                cmd2.Parameters.AddWithValue("@uvedomnapit", Properties.Settings.Default.UvedomNapit);
                SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                listBox4.DataSource = dt2;
                listBox4.DisplayMember = "Nazvanie";
                listBox4.ValueMember = "id_Nomenclature";
                listBox5.DataSource = dt2;
                listBox5.DisplayMember = "Kolichestvo";
                listBox5.ValueMember = "id_Nomenclature";
                if (listBox1.Items.Count > 0 | listBox4.Items.Count > 0 | listBox6.Items.Count > 0)
                {
                    for (int index = 0; index < listBox1.Items.Count; index++)
                    {
                        listBox1.SelectedIndex = index;
                        DataRowView drv = (DataRowView)listBox1.SelectedItem;
                        String valueOfItem = drv["Nazvanie"].ToString();
                        String valueOfItem2 = drv["Srok"].ToString();

                        if (Convert.ToInt32(valueOfItem2) < 1)
                        {
                            listBox3.Items.Add("Закончился срок годности " + valueOfItem);
                        }
                        else if (valueOfItem2 == "1")
                        {
                            listBox3.Items.Add("Остался " + valueOfItem2 + " день до окончания срока годности " + valueOfItem);
                        }
                        else if ((valueOfItem2 == "2") | (valueOfItem2 == "3") | (valueOfItem2 == "4"))
                        {
                            listBox3.Items.Add("Осталось " + valueOfItem2 + " дня до окончания срока годности " + valueOfItem);

                        }
                        else
                        {
                            listBox3.Items.Add("Осталось " + valueOfItem2 + " дней до окончания срока годности " + valueOfItem);
                        }

                    }
                    for (int index = 0; index < listBox4.Items.Count; index++)
                    {
                        listBox4.SelectedIndex = index;
                        DataRowView drv2 = (DataRowView)listBox4.SelectedItem;
                        String valueOfItem3 = drv2["Nazvanie"].ToString();
                        String valueOfItem4 = drv2["Kolichestvo"].ToString();
                        listBox3.Items.Add(valueOfItem3 + "осталось всего " + valueOfItem4 + ", необходима закупка!");
                    }

                    for (int index = 0; index < listBox6.Items.Count; index++)
                    {
                        listBox6.SelectedIndex = index;
                        DataRowView drv3 = (DataRowView)listBox6.SelectedItem;
                        DateTime valueOfItem6 = Convert.ToDateTime(drv3["Date"]);

                        listBox3.Items.Add("Предзаказ на дату " + valueOfItem6.ToShortDateString() + " (состав в списке продаж)!");
                    }
                    cmd1.Dispose();
                    cmd2.Dispose();
                    cmd3.Dispose();
                }
                conn1.Close();
            }
            else if (tabControl1.SelectedIndex == 5)
            {
                string query = "SELECT id_ProductGroup, Name FROM ProductGroups";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbGroup.DataSource = dt;
                cbGroup.DisplayMember = "Name";
                cbGroup.ValueMember = "id_ProductGroup";
                cbGroup.SelectedIndex = 0;
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "id_ProductGroup";
                comboBox1.ValueMember = "id_ProductGroup";
                cmd.Dispose();
                conn.Close();

                query = "SELECT id_Nomenclature, Nomenclatures.id_ProductGroup, ProductGroups.Name AS Группа, Nomenclatures.Name AS Наименование, Articul AS Артикул, " +
                    "Weight AS [Масса (нетто)], EdIzm AS [Ед Измерения] FROM Nomenclatures " +
                    "LEFT JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup " +
                    "WHERE Nomenclatures.Name <> '-' ORDER BY ProductGroups.Name DESC, Nomenclatures.Name";
                conn.Open();
                SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                dataGridView6.DataSource = dt1;
                dataGridView6.Columns[0].Visible = false;
                dataGridView6.Columns[1].Visible = false;
                cmd.Dispose();
                conn.Close();
                dataGridView6.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView6.Select();
                if (dataGridView5.Rows.Count == 0)
                {
                    EditNomenclature.Enabled = false;
                    DeleteNomenclature.Enabled = false;
                }
                else
                {
                    EditNomenclature.Enabled = true;
                    DeleteNomenclature.Enabled = true;
                }

            }
            else if (tabControl1.SelectedIndex == 6)
            {
                string query = "SELECT id_Provider, Type AS Тип, Name AS Наименование, Address AS Адрес, Phone AS Телефон, DopPhone AS [Доп телефон], Email, OKPO AS ОКПО, INN AS ИНН FROM Providers";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView5.DataSource = dt;
                dataGridView5.Columns[0].Visible = false;
                cmd.Dispose();
                conn.Close();
                dataGridView5.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView5.Select();
                if (dataGridView5.Rows.Count == 0)
                {
                    EditPostav.Enabled = false;
                    DeletePostav.Enabled = false;
                }
                else
                {
                    EditPostav.Enabled = true;
                    DeletePostav.Enabled = true;
                }
            }
            else if (tabControl1.SelectedIndex == 7)
            {
                string query = "SELECT id_Employee, FirstName AS Имя, LastName AS Фамилия, MiddleName AS Отчество, Phone AS Телефон FROM Employees";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView3.DataSource = dt;
                dataGridView3.Columns[0].Visible = false;
                cmd.Dispose();
                conn.Close();
                dataGridView3.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView3.Select();
                if (dataGridView3.Rows.Count == 0)
                {
                    EditSotrud.Enabled = false;
                    DeleteSotrud.Enabled = false;
                }
                else
                {
                    EditSotrud.Enabled = true;
                    DeleteSotrud.Enabled = true;
                }
            }
        }


        // Перемещаем значения из таблицы в текстбоксы
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            ind = dataGridView1.CurrentRow.Index;
            tbidclient.Text = dataGridView1.Rows[ind].Cells[0].Value.ToString();
            tbClientFirstName.Text = dataGridView1.Rows[ind].Cells[1].Value.ToString();
            tbClientLastName.Text = dataGridView1.Rows[ind].Cells[2].Value.ToString();
            tbClientMiddleName.Text = dataGridView1.Rows[ind].Cells[3].Value.ToString();
            tbClientPhone.Text = dataGridView1.Rows[ind].Cells[4].Value.ToString();
            tbClientSkidka.Text = dataGridView1.Rows[ind].Cells[5].Value.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
            }
            else if (!checkBox1.Checked)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
        }

        private void yt_Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Удалить запись?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int count = dataGridView4.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        yt_Button18_Click(sender, e);
                    }
                    checkBox1.Checked = false;
                    dateTimePicker1.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    string query = "SELECT InvoiceHeaders.id_InvoiceHeader, InvoiceTypes.Name AS Тип, Providers.Name AS Контрагент, " +
                        "Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y', Date) AS Дата, Sum || ' руб' AS Сумма FROM InvoiceHeaders " +
                        "JOIN Providers ON InvoiceHeaders.id_Provider = Providers.id_Provider " +
                        "JOIN InvoiceTypes ON InvoiceHeaders.id_InvoiceType = InvoiceTypes.id_InvoiceTypes " +
                        "JOIN Employees ON InvoiceHeaders.id_Employee = Employees.id_Employee";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                    dataGridView2.Columns[0].Visible = false;
                    cmd.Dispose();
                    conn.Close();
                    dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView2.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView2.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView2.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView2.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Select();
                    label71.Text = dataGridView2.Rows.Count.ToString();

                    if (dataGridView2.Rows.Count < 2)
                    {
                        string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature, ProductGroups.Name AS Группа, " +
                            "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                            "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения], Rasfasovka AS [Расфасовка (уп)]," +
                            " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до], Price|| ' руб' AS Цена, Summ || ' руб' AS Сумма,  PriceSale|| ' руб' AS [Цена на продажу]  FROM InvoiceTables " +
                            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                            " WHERE Type = 1 OR Type = 2";
                        SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                        conn1.Open();
                        SQLiteCommand cmd3 = new SQLiteCommand(query1, conn1);
                        SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                        DataTable dt3 = new DataTable();
                        da3.Fill(dt3);
                        dataGridView4.DataSource = dt3;
                        dataGridView4.Columns[0].Visible = false;
                        cmd3.Dispose();
                        conn1.Close();

                        dataGridView4.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView4.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView4.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView4.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView4.Select();
                    }
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string query2 = "SELECT InvoiceHeaders.id_InvoiceHeader, InvoiceTypes.Name AS Тип, Providers.Name AS [Поставщик], " +
"Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y', Date) AS Дата, Sum || ' руб' AS Сумма FROM InvoiceHeaders " +
"JOIN Providers ON InvoiceHeaders.id_Provider = Providers.id_Provider " +
"JOIN InvoiceTypes ON InvoiceHeaders.id_InvoiceType = InvoiceTypes.id_InvoiceTypes " +
"JOIN Employees ON InvoiceHeaders.id_Employee = Employees.id_Employee " +
" WHERE Date BETWEEN @date1 AND @date2";
            SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn2.Open();
            SQLiteCommand cmd2 = new SQLiteCommand(query2, conn2);
            SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
            cmd2.Parameters.AddWithValue("@date1", dateTimePicker1.Value);
            cmd2.Parameters.AddWithValue("@date2", dateTimePicker2.Value);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;
            dataGridView2.Columns[0].Visible = false;
            cmd2.Dispose();
            conn2.Close();
            dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            label71.Text = dataGridView2.Rows.Count.ToString();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            string query2 = "SELECT InvoiceHeaders.id_InvoiceHeader, InvoiceTypes.Name AS Тип, Providers.Name AS [Поставщик], " +
"Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y', Date) AS Дата, Sum || ' руб' AS Сумма FROM InvoiceHeaders " +
"JOIN Providers ON InvoiceHeaders.id_Provider = Providers.id_Provider " +
"JOIN InvoiceTypes ON InvoiceHeaders.id_InvoiceType = InvoiceTypes.id_InvoiceTypes " +
"JOIN Employees ON InvoiceHeaders.id_Employee = Employees.id_Employee " +
" WHERE Date BETWEEN @date1 AND @date2";
            SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn2.Open();
            SQLiteCommand cmd2 = new SQLiteCommand(query2, conn2);
            SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
            cmd2.Parameters.AddWithValue("@date1", dateTimePicker1.Value);
            cmd2.Parameters.AddWithValue("@date2", dateTimePicker2.Value);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;
            dataGridView2.Columns[0].Visible = false;
            cmd2.Dispose();
            conn2.Close();
            dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            label71.Text = dataGridView2.Rows.Count.ToString();
        }

        private void yt_Button2_Click(object sender, EventArgs e)
        {
            FormInvoiceAddRas form3 = new FormInvoiceAddRas();
            form3.Owner = this;
            form3.ShowDialog();
        }

        private void yt_Button4_Click(object sender, EventArgs e)
        {
            comboBox3.Enabled = false;
            comboBox4.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            bSaveOstat.Visible = true;
            bCancelOstat.Visible = true;
            bAddOstat.Enabled = false;
            bEditOstat.Enabled = false;
            bDeleteOstat.Enabled = false;
            tbOstatSearch.ReadOnly = true;

            if (dateTimePicker4.Value >= DateTime.Now)
            {
                checkBox3.Checked = true;
            }
            else checkBox3.Checked = false;

            dataGridView7.Enabled = false;
            ost = 2;
        }

        private void yt_Button6_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox7.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            bSaveOstat.Visible = false;
            bCancelOstat.Visible = false;
            bAddOstat.Enabled = true;
            bEditOstat.Enabled = true;
            bDeleteOstat.Enabled = true;
            dataGridView7.Enabled = true;
            tbOstatSearch.ReadOnly = false;
            dataGridView7.Focus();

        }

        private void dataGridView7_SelectionChanged(object sender, EventArgs e)
        {
            ind = dataGridView7.CurrentRow.Index;
            tbidOstat.Text = dataGridView7.Rows[ind].Cells[0].Value.ToString();
            comboBox3.Text = dataGridView7.Rows[ind].Cells[2].Value.ToString();
            textBox1.Text = dataGridView7.Rows[ind].Cells[3].Value.ToString();
            textBox2.Text = dataGridView7.Rows[ind].Cells[4].Value.ToString();
            comboBox4.Text = dataGridView7.Rows[ind].Cells[5].Value.ToString();
            if (dataGridView7.Rows[ind].Cells[6].Value.ToString() == "-" | dataGridView7.Rows[ind].Cells[6].Value.ToString() == "")
            {
                dateTimePicker4.Value = DateTime.Now;
            }
            else dateTimePicker4.Value = Convert.ToDateTime(dataGridView7.Rows[ind].Cells[6].Value.ToString());
        }

        private void yt_Button5_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                dateTimePicker4.Value = dateTimePicker3.Value.AddDays(Convert.ToDouble(textBox7.Text));
            }
            if ((comboBox3.Text == ""))
                MessageBox.Show("Введите наименование!", "Ошибка");
            else if ((comboBox4.Text == ""))
                MessageBox.Show("Введите единицу измерения!", "Ошибка");
            else if ((textBox2.Text == ""))
                MessageBox.Show("Введите остаток!", "Ошибка");
            else if ((textBox1.Text == ""))
                MessageBox.Show("Введите цену на продажу!", "Ошибка");
            else if (ost == 1)
            {
                if ((checkBox2.Checked) | (checkBox3.Checked))
                {
                    if ((dateTimePicker4.Value <= DateTime.Today))
                        MessageBox.Show("Срок годности указан неверно!", "Ошибка");
                    else
                    {
                        string query = "INSERT INTO InvoiceTables (id_InvoiceHeader, id_Nomenclature, Type, PriceSale, Amount, EdIzm, SrokGodnosti, Price, Summ) " +
        "VALUES (@idinvoiceheader, @idnomenclature, @type, @pricesale, @amount, @edizm,  @srokgodnosti, @price, @summ)";
                        SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                        conn2.Open();
                        SQLiteCommand cmd2 = new SQLiteCommand(query, conn2);
                        cmd2.Parameters.AddWithValue("@idinvoiceheader", 0);
                        cmd2.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(comboBox5.SelectedValue));
                        cmd2.Parameters.AddWithValue("@type", 3);
                        cmd2.Parameters.AddWithValue("@pricesale", Convert.ToDouble(textBox1.Text));
                        cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox2.Text));
                        cmd2.Parameters.AddWithValue("@edizm", comboBox4.Text);
                        cmd2.Parameters.AddWithValue("@srokgodnosti", dateTimePicker4.Value.Date);
                        cmd2.Parameters.AddWithValue("@price", 0);
                        cmd2.Parameters.AddWithValue("@summ", 0);
                        SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                        DataTable dt2 = new DataTable();
                        da2.Fill(dt2);
                        cmd2.Dispose();

                        string query1 = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
                            " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                            " PriceSale AS [Цена на продажу], Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
                            " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до] FROM InvoiceTables " +
                            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                            " WHERE Type = 3 AND id_InvoiceTable > 1 ORDER BY ProductGroups.Name DESC, Наименование";
                        SQLiteCommand cmd3 = new SQLiteCommand(query1, conn2);
                        SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                        DataTable dt3 = new DataTable();
                        da3.Fill(dt3);
                        dataGridView7.DataSource = dt3;
                        dataGridView7.Columns[0].Visible = false;
                        cmd3.Dispose();
                        conn2.Close();
                        dataGridView7.Enabled = true;

                        MessageBox.Show("Товар успешно добавлен!");
                        comboBox3.Enabled = false;
                        comboBox4.Enabled = false;
                        bSaveOstat.Visible = false;
                        bCancelOstat.Visible = false;
                        checkBox2.Checked = false;
                        checkBox3.Checked = false;
                        bAddOstat.Enabled = true;
                        bEditOstat.Enabled = true;
                        bDeleteOstat.Enabled = true;
                        dateTimePicker3.Enabled = false;
                        dateTimePicker4.Enabled = false;
                        textBox1.Enabled = false;
                        textBox2.Enabled = false;
                        textBox7.Enabled = false;
                        dataGridView7.Enabled = true;
                        tbOstatSearch.ReadOnly = false;

                        dataGridView7.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView7.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView7.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView7.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView7.Focus();

                        textBox1.Text = "0";
                        textBox2.Text = "0";
                        textBox7.Text = "0";
                    }
                }
                else
                {
                    string query = "INSERT INTO InvoiceTables (id_InvoiceHeader, id_Nomenclature, Type, PriceSale, Amount, EdIzm, SrokGodnosti, Price, Summ) " +
    "VALUES (@idinvoiceheader, @idnomenclature, @type, @pricesale, @amount, @edizm,  @srokgodnosti, @price, @summ)";
                    SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn2.Open();
                    SQLiteCommand cmd2 = new SQLiteCommand(query, conn2);
                    cmd2.Parameters.AddWithValue("@idinvoiceheader", 0);
                    cmd2.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(comboBox5.SelectedValue));
                    cmd2.Parameters.AddWithValue("@type", 3);
                    cmd2.Parameters.AddWithValue("@pricesale", Convert.ToDouble(textBox1.Text));
                    cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox2.Text));
                    cmd2.Parameters.AddWithValue("@edizm", comboBox4.Text);
                    cmd2.Parameters.AddWithValue("@srokgodnosti", "-");
                    cmd2.Parameters.AddWithValue("@price", 0);
                    cmd2.Parameters.AddWithValue("@summ", 0);
                    SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    cmd2.Dispose();

                    string query1 = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                        " PriceSale AS [Цена на продажу], Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
                        " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до] FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                        " WHERE Type = 3 AND id_InvoiceTable > 1 ORDER BY ProductGroups.Name DESC, Наименование";
                    SQLiteCommand cmd3 = new SQLiteCommand(query1, conn2);
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView7.DataSource = dt3;
                    dataGridView7.Columns[0].Visible = false;
                    cmd3.Dispose();
                    conn2.Close();
                    dataGridView7.Enabled = true;


                    MessageBox.Show("Товар успешно добавлен!");
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    bSaveOstat.Visible = false;
                    bCancelOstat.Visible = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    bAddOstat.Enabled = true;
                    bEditOstat.Enabled = true;
                    bDeleteOstat.Enabled = true;
                    dateTimePicker3.Enabled = false;
                    dateTimePicker4.Enabled = false;
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox7.Enabled = false;
                    dataGridView7.Enabled = true;
                    tbOstatSearch.ReadOnly = false;

                    dataGridView7.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Focus();

                    textBox1.Text = "0";
                    textBox2.Text = "0";
                    textBox7.Text = "0";
                }
            }
            else if (ost == 2)
            {
                if ((checkBox2.Checked) | (checkBox3.Checked))
                {
                    if ((dateTimePicker4.Value <= DateTime.Today))
                        MessageBox.Show("Срок годности указан неверно!", "Ошибка");
                    else
                    {
                        string query = "UPDATE InvoiceTables SET Amount = @amount, EdIzm=@edizm, PriceSale = @pricesale, SrokGodnosti = @srokgodnosti WHERE id_InvoiceTable = @idinvoicetable";
                        SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                        conn.Open();
                        SQLiteCommand cmd = new SQLiteCommand(query, conn);
                        cmd.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(tbidOstat.Text));
                        cmd.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox2.Text));
                        cmd.Parameters.AddWithValue("@edizm", Convert.ToString(comboBox4.Text));
                        cmd.Parameters.AddWithValue("@pricesale", Convert.ToDouble(textBox1.Text));
                        cmd.Parameters.AddWithValue("@srokgodnosti", dateTimePicker4.Value.Date);
                        cmd.ExecuteNonQuery();
                        SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView7.DataSource = dt;
                        cmd.Dispose();

                        string query1 = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
                            " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                            " PriceSale AS [Цена на продажу], Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
                            " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до] FROM InvoiceTables " +
                            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                            " WHERE Type = 3 AND id_InvoiceTable > 1 ORDER BY ProductGroups.Name DESC, Наименование";

                        SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                        SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);
                        dataGridView7.DataSource = dt1;
                        dataGridView7.Columns[0].Visible = false;
                        cmd1.Dispose();
                        conn.Close();
                        dataGridView7.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView7.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView7.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView7.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView7.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView7.Enabled = true;


                        comboBox3.Enabled = false;
                        comboBox4.Enabled = false;
                        bSaveOstat.Visible = false;
                        bCancelOstat.Visible = false;
                        checkBox2.Checked = false;
                        checkBox3.Checked = false;
                        bAddOstat.Enabled = true;
                        bEditOstat.Enabled = true;
                        bDeleteOstat.Enabled = true;
                        dateTimePicker3.Enabled = false;
                        dateTimePicker4.Enabled = false;
                        textBox1.Enabled = false;
                        textBox2.Enabled = false;
                        textBox7.Enabled = false;
                        dataGridView7.Enabled = true;
                        tbOstatSearch.ReadOnly = false;
                        dataGridView7.Focus();
                    }
                }
                else
                {
                    string query = "UPDATE InvoiceTables SET Amount = @amount, EdIzm=@edizm, PriceSale = @pricesale, SrokGodnosti = @srokgodnosti WHERE id_InvoiceTable = @idinvoicetable";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(tbidOstat.Text));
                    cmd.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox2.Text));
                    cmd.Parameters.AddWithValue("@edizm", Convert.ToString(comboBox4.Text));
                    cmd.Parameters.AddWithValue("@pricesale", Convert.ToDouble(textBox1.Text));
                    cmd.Parameters.AddWithValue("@srokgodnosti", "-");
                    cmd.ExecuteNonQuery();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView7.DataSource = dt;
                    cmd.Dispose();

                    string query1 = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                        " PriceSale AS [Цена на продажу], Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
                        " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до] FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                        " WHERE Type = 3 AND id_InvoiceTable > 1 ORDER BY ProductGroups.Name DESC, Наименование";

                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    dataGridView7.DataSource = dt1;
                    dataGridView7.Columns[0].Visible = false;
                    cmd1.Dispose();
                    conn.Close();
                    dataGridView7.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    bSaveOstat.Visible = false;
                    bCancelOstat.Visible = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    bAddOstat.Enabled = true;
                    bEditOstat.Enabled = true;
                    bDeleteOstat.Enabled = true;
                    dateTimePicker3.Enabled = false;
                    dateTimePicker4.Enabled = false;
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox7.Enabled = false;
                    dataGridView7.Enabled = true;
                    tbOstatSearch.ReadOnly = false;
                    dataGridView7.Focus();
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox3.Checked = false;
                dateTimePicker3.Enabled = true;
                textBox7.Enabled = true;
            }
            if (!checkBox2.Checked)
            {
                dateTimePicker3.Enabled = false;
                textBox7.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox2.Checked = false;
                dateTimePicker4.Enabled = true;
            }
            if (!checkBox3.Checked)
            {
                dateTimePicker4.Enabled = false;
            }
        }

        private void bDeleteOstat_Click(object sender, EventArgs e)
        {
            if (dataGridView7.Focused == false)
                MessageBox.Show("Не выбрана запись для удаления!", "Ошибка");
            else if (dataGridView7.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Удалить запись?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM InvoiceTables WHERE id_InvoiceTable = @idinvoicetable";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(tbidOstat.Text));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    query = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '') AS Наименование," +
                        " PriceSale AS [Цена на продажу], Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
                        " SrokGodnosti AS [Срок годности до]  FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                        " WHERE Type = 3 AND id_InvoiceTable>1 ORDER BY ProductGroups.Name DESC, Наименование";

                    SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    dataGridView7.DataSource = dt1;
                    dataGridView7.Columns[0].Visible = false;
                    cmd1.Dispose();
                    conn.Close();
                    dataGridView7.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView7.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView7.Select();

                    MessageBox.Show("Удаление прошло успешно!");
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                comboBox8.Enabled = true;
                string query = "SELECT id_Client, FirstName||' '||IFNULL(LastName, '')||' '||IFNULL(MiddleName, '' )||' '||IFNULL(Phone, '' ) AS Name, Skidka  FROM Clients " +
                    "WHERE id_Client > 0";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comboBox8.DataSource = dt;
                comboBox8.DisplayMember = "Name";
                comboBox8.ValueMember = "id_Client";
                comboBox8.SelectedIndex = -1;
                comboBox8.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox8.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBox10.DataSource = dt;
                comboBox10.DisplayMember = "id_Client";
                comboBox10.ValueMember = "id_Client";
                comboBox18.DataSource = dt;
                comboBox18.DisplayMember = "Skidka";
                comboBox18.ValueMember = "id_Client";
                textBox26.Visible = true;
                label13.Visible = true;
                button2.Enabled = true;
                button2.Visible = true;
            }
            else
            {
                button1.Visible = false;
                textBox26.Visible = false;
                button2.Visible = false;
                label13.Visible = false;
                button2.Enabled = false;
                comboBox8.Enabled = false;
                comboBox8.SelectedIndex = 0;
                textBox26.Enabled = false;
                textBox11.Text = Convert.ToString(summ);
            }
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox9.SelectedIndex == -1)
            {
                checkBox9.Enabled = false;
                checkBox11.Enabled = false;
                textBox11.Enabled = false;
                checkBox7.Enabled = false;
                checkBox8.Enabled = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;
                yt_Button10.Enabled = false;
                yt_Button4.Enabled = false;
                yt_Button5.Enabled = false;
                yt_Button6.Enabled = false;
                yt_Button7.Enabled = false;
                comboBox6.Enabled = false;
                comboBox2.Enabled = false;
                comboBox15.Enabled = false;
                textBox11.Enabled = false;
                // yt_Button10.Enabled = false;
                yt_Button8.Enabled = false;
                textBox5.Enabled = false;
                checkBox5.Checked = false;
                checkBox5.Enabled = false;
                checkBox6.Checked = false;
                checkBox6.Enabled = false;
                textBox16.Enabled = false;
                textBox27.Enabled = false;

                comboBox7.Enabled = false;
                textBox4.Enabled = false;

                label66.Visible = false;
                label67.Visible = false;
                textBox23.Visible = false;
                textBox4.Text = "0";
            }
            else
            {
                checkBox9.Enabled = true;
                checkBox11.Enabled = true;
                comboBox6.Select();
                comboBox6.Enabled = true;
                textBox11.Enabled = true;
                checkBox7.Enabled = true;
                checkBox8.Enabled = true;
                // yt_Button10.Enabled = true;
                yt_Button8.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = true;
                checkBox9.Checked = false;
                checkBox11.Checked = false;


                comboBox7.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;

                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                //Не выводим товары с нулевым остатком
                if (checkBox10.Checked)
                {
                    string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, ProductGroups.Name, " +
        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
        " strftime('%d.%m.%Y', SrokGodnosti) FROM InvoiceTables " +
        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comboBox6.DataSource = dt;
                    comboBox6.DisplayMember = "Nazvanie";
                    comboBox6.ValueMember = "id_InvoiceTable";
                    comboBox6.SelectedIndex = -1;

                    comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;


                    comboBox12.DataSource = dt;
                    comboBox12.DisplayMember = "id_InvoiceTable";
                    comboBox12.ValueMember = "id_InvoiceTable";
                    comboBox12.SelectedIndex = -1;

                    comboBox2.DataSource = dt;
                    comboBox2.DisplayMember = "Amount";
                    comboBox2.ValueMember = "id_InvoiceTable";
                    comboBox2.SelectedIndex = -1;


                    comboBox15.DataSource = dt;
                    comboBox15.DisplayMember = "PriceSale";
                    comboBox15.ValueMember = "id_InvoiceTable";
                    comboBox15.SelectedIndex = -1;
                    textBox27.Text = comboBox15.Text;

                    comboBox6.Text = "";
                    comboBox2.Text = "";
                    cmd.Dispose();
                }
                else // выводим товары с нулевым остатком
                {
                    string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, ProductGroups.Name, " +
        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
        " strftime('%d.%m.%Y', SrokGodnosti) FROM InvoiceTables " +
        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comboBox6.DataSource = dt;
                    comboBox6.DisplayMember = "Nazvanie";
                    comboBox6.ValueMember = "id_InvoiceTable";
                    comboBox6.SelectedIndex = -1;

                    comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;


                    comboBox12.DataSource = dt;
                    comboBox12.DisplayMember = "id_InvoiceTable";
                    comboBox12.ValueMember = "id_InvoiceTable";
                    comboBox12.SelectedIndex = -1;

                    comboBox2.DataSource = dt;
                    comboBox2.DisplayMember = "Amount";
                    comboBox2.ValueMember = "id_InvoiceTable";
                    comboBox2.SelectedIndex = -1;


                    comboBox15.DataSource = dt;
                    comboBox15.DisplayMember = "PriceSale";
                    comboBox15.ValueMember = "id_InvoiceTable";
                    comboBox15.SelectedIndex = -1;
                    textBox27.Text = comboBox15.Text;

                    comboBox6.Text = "";
                    comboBox2.Text = "";
                    cmd.Dispose();
                }

                //Не выводим товары с нулевым остатком
                if (checkBox10.Checked)
                {
                    string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, ProductGroups.Name," +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y', SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' AND Amount > 0 OR id_InvoiceTable=1 ORDER BY Nazvanie";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    comboBox7.DataSource = dt1;
                    comboBox7.DisplayMember = "Nazvanie";
                    comboBox7.ValueMember = "id_InvoiceTable";
                    comboBox7.SelectedIndex = -1;
                    comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;


                    comboBox13.DataSource = dt1;
                    comboBox13.DisplayMember = "id_InvoiceTable";
                    comboBox13.ValueMember = "id_InvoiceTable";
                    comboBox13.SelectedIndex = -1;

                    comboBox17.DataSource = dt1;
                    comboBox17.DisplayMember = "Amount";
                    comboBox17.ValueMember = "id_InvoiceTable";
                    comboBox17.SelectedIndex = -1;

                    comboBox16.DataSource = dt1;
                    comboBox16.DisplayMember = "PriceSale";
                    comboBox16.ValueMember = "id_InvoiceTable";
                    comboBox16.SelectedIndex = -1;
                    textBox16.Text = comboBox16.Text;

                    comboBox19.DataSource = dt1;
                    comboBox19.DisplayMember = "EdIzm";
                    comboBox19.ValueMember = "id_InvoiceTable";
                    comboBox19.SelectedIndex = -1;

                    cmd1.Dispose();
                }
                else  //выводим товары с нулевым остатком
                {
                    string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, ProductGroups.Name," +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y', SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' ORDER BY Nazvanie";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    comboBox7.DataSource = dt1;
                    comboBox7.DisplayMember = "Nazvanie";
                    comboBox7.ValueMember = "id_InvoiceTable";
                    comboBox7.SelectedIndex = -1;
                    comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;


                    comboBox13.DataSource = dt1;
                    comboBox13.DisplayMember = "id_InvoiceTable";
                    comboBox13.ValueMember = "id_InvoiceTable";
                    comboBox13.SelectedIndex = -1;

                    comboBox17.DataSource = dt1;
                    comboBox17.DisplayMember = "Amount";
                    comboBox17.ValueMember = "id_InvoiceTable";
                    comboBox17.SelectedIndex = -1;

                    comboBox16.DataSource = dt1;
                    comboBox16.DisplayMember = "PriceSale";
                    comboBox16.ValueMember = "id_InvoiceTable";
                    comboBox16.SelectedIndex = -1;
                    textBox16.Text = comboBox16.Text;

                    comboBox19.DataSource = dt1;
                    comboBox19.DisplayMember = "EdIzm";
                    comboBox19.ValueMember = "id_InvoiceTable";
                    comboBox19.SelectedIndex = -1;

                    cmd1.Dispose();
                }
            }

        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar)) || (e.KeyChar == 8)) return;
            else
                e.Handled = true;
        }

        private void comboBox19_TextChanged(object sender, EventArgs e)
        {
            label50.Text = comboBox19.Text;
            label51.Text = comboBox19.Text;
        }

        //Тара 0.5л
        private void yt_Button4_Click_1(object sender, EventArgs e)
        {
            kolichestvo = 0;
            if (dat == 0)
                dateTimePicker5.Value = DateTime.Now;
            string query16 = "SELECT COUNT(id_InvoiceTable) AS count FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" WHERE Nomenclatures.Name LIKE 'Тара 0,5%' ";
            SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn2.Open();
            SQLiteCommand cmd16 = new SQLiteCommand(query16, conn2);
            Int64 count3 = (Int64)cmd16.ExecuteScalar();
            cmd16.Dispose();
            conn2.Close();

            if (count3 == 0)
            {
                MessageBox.Show("Такой тары нет в номенклатуре!");
            }
            else
            {
                if (comboBox6.Text == "" | comboBox2.Text == "" | comboBox15.Text == "" | comboBox6.Text.Trim() == "-")
                    MessageBox.Show("Выберите напиток!");
                else
                {
                    if (textBox27.Text.Trim() == "")
                        MessageBox.Show("Введите цену!");
                    else
                    {
                        if (checkBox5.Checked)
                        {
                            if (textBox17.Text == "" | textBox17.Text == "0")
                                MessageBox.Show("Введите количество напитка!");
                            else
                            {
                                kolichestvo = Convert.ToDouble(textBox18.Text);

                                if (kolichestvo == 0)
                                    return;
                                else
                                {

                                    string query7 = "SELECT id_InvoiceTable, Nomenclatures.id_Nomenclature AS id_Nomenclature, Amount, PriceSale FROM InvoiceTables " +
                    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                    "WHERE Nomenclatures.Name LIKE 'Тара 0,5%'";
                                    SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                    conn1.Open();
                                    SQLiteCommand cmd7 = new SQLiteCommand(query7, conn1);
                                    SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                                    DataTable dt7 = new DataTable();
                                    da7.Fill(dt7);
                                    comboBox23.DataSource = dt7;
                                    comboBox23.DisplayMember = "id_InvoiceTable";
                                    comboBox23.ValueMember = "id_InvoiceTable";

                                    comboBox20.DataSource = dt7;
                                    comboBox20.DisplayMember = "id_Nomenclature";
                                    comboBox20.ValueMember = "id_InvoiceTable";

                                    comboBox21.DataSource = dt7;
                                    comboBox21.DisplayMember = "Amount";
                                    comboBox21.ValueMember = "id_InvoiceTable";

                                    comboBox22.DataSource = dt7;
                                    comboBox22.DisplayMember = "PriceSale";
                                    comboBox22.ValueMember = "id_InvoiceTable";
                                    conn1.Close();

                                    if (textBox3.Text == "")
                                    {
                                        string query = "INSERT INTO Realisations (id_Client, id_Employee, Date) VALUES (@idclient, @idemployee, @date)";
                                        SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                        conn.Open();
                                        SQLiteCommand cmd = new SQLiteCommand(query, conn);
                                        cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                                        cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(comboBox11.SelectedValue));
                                        cmd.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                                        SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                                        DataTable dt = new DataTable();
                                        da.Fill(dt);
                                        cmd.Dispose();

                                        string query1 = "SELECT MAX(id_Realisation) FROM Realisations";
                                        SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                                        Int64 id = (Int64)cmd1.ExecuteScalar();
                                        textBox3.Text = Convert.ToString(id);
                                        cmd1.Dispose();
                                        MessageBox.Show(numericUpDown1.Value.ToString());
                                        string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                                        "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                        SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                                        cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                        cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(numericUpDown1.Value));
                                        cmd2.Parameters.AddWithValue("@price", CenaNapit);
                                        cmd2.Parameters.AddWithValue("@summa", CenaNapit * Convert.ToDouble(numericUpDown1.Value));
                                        SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                                        DataTable dt2 = new DataTable();
                                        da2.Fill(dt2);
                                        cmd2.Dispose();

                                        //MessageBox.Show(numericUpDown1.Value.ToString());


                                        query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                        "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                        SQLiteCommand cmd6 = new SQLiteCommand(query2, conn);
                                        cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd6.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(kolichestvo));
                                        cmd6.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                        cmd6.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text) * kolichestvo);
                                        SQLiteDataAdapter da6 = new SQLiteDataAdapter(cmd6);
                                        DataTable dt6 = new DataTable();
                                        da6.Fill(dt6);
                                        cmd6.Dispose();


                                        string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                        SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                        cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        summ = (Double)cmd4.ExecuteScalar();
                                        textBox11.Text = Convert.ToString(summ);
                                        if (comboBox8.SelectedIndex > 0)
                                        {
                                            textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                        }
                                        cmd4.Dispose();

                                        //Уменьшение остатков напитка
                                        string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                        SQLiteCommand cmd8 = new SQLiteCommand(query5, conn);
                                        cmd8.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                        cmd8.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(numericUpDown1.Value));
                                        cmd8.ExecuteNonQuery();
                                        cmd8.Dispose();

                                        ostatok = ostatok - Convert.ToDouble(numericUpDown1.Value);

                                        //Уменьшение остатков тары
                                        string query9 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                        SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                        cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - kolichestvo);
                                        cmd9.ExecuteNonQuery();
                                        cmd9.Dispose();
                                        conn.Close();

                                        checkBox11.Checked = false;
                                        //MessageBox.Show("Добавлено " + Convert.ToDouble(numericUpDown1.Value) + "л " + comboBox6.Text);
                                    }

                                    else
                                    {
                                        string query15 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                        SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                        conn.Open();
                                        SQLiteCommand cmd15 = new SQLiteCommand(query15, conn);
                                        cmd15.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                        cmd15.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        Int64 count2 = (Int64)cmd15.ExecuteScalar();
                                        cmd15.Dispose();

                                        if (count2 > 0)
                                        {
                                            string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                            SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                            cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                            cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            Double amount = (Double)cmd10.ExecuteScalar();
                                            textBox10.Text = Convert.ToString(amount);
                                            cmd10.Dispose();

                                            textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + Convert.ToDouble(kolichestvo) / 2);

                                            //пересчет общей суммы
                                            string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                            SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                            cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                            cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            Double summa = (Double)cmd11.ExecuteScalar();
                                            textBox12.Text = Convert.ToString(summa);
                                            cmd11.Dispose();

                                            textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + CenaNapit * (Convert.ToDouble(kolichestvo) / 2));

                                            string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                            SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                            cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                            cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                            cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                            cmd9.ExecuteNonQuery();
                                            cmd9.Dispose();

                                            //скидка клиента
                                            string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                            SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                            cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            summ = (Double)cmd4.ExecuteScalar();
                                            textBox11.Text = Convert.ToString(summ);
                                            if (comboBox8.SelectedIndex > 0)
                                            {
                                                textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                            }
                                            cmd4.Dispose();

                                            //уменьшение количества напитка
                                            string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                            SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                            cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                            cmd6.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(numericUpDown1.Value));
                                            cmd6.ExecuteNonQuery();
                                            cmd6.Dispose();
                                            conn.Close();
                                            checkBox11.Checked = false;

                                            ostatok = ostatok - Convert.ToDouble(numericUpDown1.Value);
                                        }
                                        else
                                        {

                                            //добавление напитка
                                            string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                            "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                            SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                            cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                            cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            cmd14.Parameters.AddWithValue("@amount", Convert.ToDouble(numericUpDown1.Value));
                                            cmd14.Parameters.AddWithValue("@price", CenaNapit);
                                            cmd14.Parameters.AddWithValue("@summa", CenaNapit * Convert.ToDouble(numericUpDown1.Value));
                                            SQLiteDataAdapter da14 = new SQLiteDataAdapter(cmd14);
                                            DataTable dt14 = new DataTable();
                                            da14.Fill(dt14);
                                            cmd14.Dispose();

                                            //скидка клиента
                                            string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                            SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                            cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            summ = (Double)cmd4.ExecuteScalar();
                                            textBox11.Text = Convert.ToString(summ);
                                            if (comboBox8.SelectedIndex > 0)
                                            {
                                                textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                            }
                                            cmd4.Dispose();

                                            //уменьшение количества напитка
                                            string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                            SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                            cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                            cmd6.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(numericUpDown1.Value));
                                            cmd6.ExecuteNonQuery();
                                            cmd6.Dispose();
                                            conn.Close();
                                            checkBox11.Checked = false;

                                            ostatok = ostatok - Convert.ToDouble(numericUpDown1.Value);
                                        }


                                        string query12 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                        conn.Open();
                                        SQLiteCommand cmd12 = new SQLiteCommand(query12, conn);
                                        cmd12.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd12.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        Int64 count = (Int64)cmd12.ExecuteScalar();
                                        cmd12.Dispose();

                                        if (count > 0)
                                        {
                                            string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                            SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                            cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                            cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            Double amount = (Double)cmd10.ExecuteScalar();
                                            textBox10.Text = Convert.ToString(amount);
                                            cmd10.Dispose();

                                            textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + Convert.ToDouble(kolichestvo));

                                            string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                            SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                            cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                            cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            Double summa = (Double)cmd11.ExecuteScalar();
                                            textBox12.Text = Convert.ToString(summa);
                                            cmd11.Dispose();

                                            textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + Convert.ToDouble(comboBox22.Text) * kolichestvo);

                                            string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                            SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                            cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                            cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                            cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                            cmd9.ExecuteNonQuery();
                                            cmd9.Dispose();

                                            string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                            SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                            cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            summ = (Double)cmd4.ExecuteScalar();
                                            textBox11.Text = Convert.ToString(summ);
                                            if (comboBox8.SelectedIndex > 0)
                                            {
                                                textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                            }
                                            cmd4.Dispose();

                                            string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                            SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                            cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                            cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - kolichestvo);
                                            cmd13.ExecuteNonQuery();
                                            cmd13.Dispose();
                                            conn.Close();

                                            checkBox11.Checked = false;
                                            //MessageBox.Show("Добавлено " + Convert.ToDouble(numericUpDown1.Value) + "л " + comboBox6.Text);

                                        }
                                        else
                                        {

                                            string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                            "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                            SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                            cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                            cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            cmd14.Parameters.AddWithValue("@amount", kolichestvo);
                                            cmd14.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                            cmd14.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text) * kolichestvo);
                                            SQLiteDataAdapter da14 = new SQLiteDataAdapter(cmd14);
                                            DataTable dt14 = new DataTable();
                                            da14.Fill(dt14);
                                            cmd14.Dispose();

                                            string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                            SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                            cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                            summ = (Double)cmd4.ExecuteScalar();
                                            textBox11.Text = Convert.ToString(summ);
                                            if (comboBox8.SelectedIndex > 0)
                                            {
                                                textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                            }
                                            cmd4.Dispose();

                                            string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                            SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                            cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                            cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - kolichestvo);
                                            cmd13.ExecuteNonQuery();
                                            cmd13.Dispose();
                                            conn.Close();

                                            checkBox11.Checked = false;
                                            //MessageBox.Show("Добавлено " + Convert.ToDouble(numericUpDown1.Value) + "л " + comboBox6.Text);

                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (comboBox6.Text.Trim() == "" | comboBox2.Text.Trim() == "" | comboBox15.Text.Trim() == "" | comboBox6.Text.Trim() == "-")
                                MessageBox.Show("Выберите напиток!");
                            else
                            {
                                CenaNapit = Convert.ToDouble(textBox27.Text);
                                string query7 = "SELECT id_InvoiceTable, Nomenclatures.id_Nomenclature AS id_Nomenclature, Amount, PriceSale FROM InvoiceTables " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                "WHERE Nomenclatures.Name LIKE 'Тара 0,5%'";
                                SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn1.Open();
                                SQLiteCommand cmd7 = new SQLiteCommand(query7, conn1);
                                SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                                DataTable dt7 = new DataTable();
                                da7.Fill(dt7);
                                comboBox23.DataSource = dt7;
                                comboBox23.DisplayMember = "id_InvoiceTable";
                                comboBox23.ValueMember = "id_InvoiceTable";

                                comboBox20.DataSource = dt7;
                                comboBox20.DisplayMember = "id_Nomenclature";
                                comboBox20.ValueMember = "id_InvoiceTable";

                                comboBox21.DataSource = dt7;
                                comboBox21.DisplayMember = "Amount";
                                comboBox21.ValueMember = "id_InvoiceTable";

                                comboBox22.DataSource = dt7;
                                comboBox22.DisplayMember = "PriceSale";
                                comboBox22.ValueMember = "id_InvoiceTable";
                                conn1.Close();

                                if (Convert.ToDouble(comboBox21.Text) < kolichestvo)
                                {
                                    kolichestvo = Convert.ToDouble(comboBox21.Text);
                                    kolichestvo = Math.Truncate(kolichestvo);
                                }
                                if (textBox3.Text == "")
                                {
                                    string query = "INSERT INTO Realisations (id_Client, id_Employee, Date) VALUES (@idclient, @idemployee, @date)";
                                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                    conn.Open();
                                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                                    cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                                    cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(comboBox11.SelectedValue));
                                    cmd.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);
                                    cmd.Dispose();

                                    string query1 = "SELECT MAX(id_Realisation) FROM Realisations";
                                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                                    Int64 id = (Int64)cmd1.ExecuteScalar();
                                    textBox3.Text = Convert.ToString(id);
                                    cmd1.Dispose();

                                    string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                                    cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(0.5));
                                    cmd2.Parameters.AddWithValue("@price", CenaNapit);
                                    cmd2.Parameters.AddWithValue("@summa", CenaNapit * 0.5);
                                    SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                                    DataTable dt2 = new DataTable();
                                    da2.Fill(dt2);
                                    cmd2.Dispose();


                                    query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query2, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd6.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd6.Parameters.AddWithValue("@amount", 1);
                                    cmd6.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                    cmd6.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text));
                                    SQLiteDataAdapter da6 = new SQLiteDataAdapter(cmd6);
                                    DataTable dt6 = new DataTable();
                                    da6.Fill(dt6);
                                    cmd6.Dispose();


                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex > 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //Уменьшение остатков напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd8 = new SQLiteCommand(query5, conn);
                                    cmd8.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd8.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 0.5);
                                    cmd8.ExecuteNonQuery();
                                    cmd8.Dispose();

                                    //Уменьшение остатков тары
                                    string query9 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    if (checkBox10.Checked)
                                    {
                                        query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                    " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                    " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                    " strftime('%d.%m.%Y', SrokGodnosti) FROM InvoiceTables " +
                    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);
                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    else
                                    {
                                        query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                    " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                    " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                    " strftime('%d.%m.%Y', SrokGodnosti) FROM InvoiceTables " +
                    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);
                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }

                                    string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
                "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб' AS [По цене], Summa || '  руб'  AS Сумма  FROM RealisationOnNomenclatures " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
                " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                    DataTable dt3 = new DataTable();
                                    da3.Fill(dt3);
                                    dataGridView8.DataSource = dt3;
                                    dataGridView8.Columns[0].Visible = false;
                                    dataGridView8.Columns[1].Visible = false;
                                    cmd3.Dispose();
                                    dataGridView8.Select();
                                    conn.Close();

                                    checkBox11.Checked = false;

                                    dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    yt_Button11.Enabled = true;
                                    dataGridView8.Select();
                                }

                                else
                                {
                                    string query12 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                    conn.Open();
                                    SQLiteCommand cmd12 = new SQLiteCommand(query12, conn);
                                    cmd12.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd12.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Int64 count2 = (Int64)cmd12.ExecuteScalar();
                                    cmd12.Dispose();

                                    if (count2 > 0)
                                    {
                                        string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                        SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                        cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                        cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        Double amount = (Double)cmd10.ExecuteScalar();
                                        textBox10.Text = Convert.ToString(amount);
                                        cmd10.Dispose();

                                        textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + 0.5);

                                        //пересчет общей суммы
                                        string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                        SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                        cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                        cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        Double summa = (Double)cmd11.ExecuteScalar();
                                        textBox12.Text = Convert.ToString(summa);
                                        cmd11.Dispose();

                                        textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + CenaNapit * 0.5);

                                        string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                        SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                        cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                        cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                        cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                        cmd9.ExecuteNonQuery();
                                        cmd9.Dispose();

                                        //скидка клиента
                                        string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                        SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                        cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        summ = (Double)cmd4.ExecuteScalar();
                                        textBox11.Text = Convert.ToString(summ);
                                        if (comboBox8.SelectedIndex > 0)
                                        {
                                            textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                        }
                                        cmd4.Dispose();

                                        //уменьшение количества напитка
                                        string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                        SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                        cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                        cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 0.5);
                                        cmd6.ExecuteNonQuery();
                                        cmd6.Dispose();

                                        //уменьшение количества тар
                                        string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                        SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                        cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                        cmd13.ExecuteNonQuery();
                                        cmd13.Dispose();
                                        //MessageBox.Show("Добавлено " + Convert.ToDouble(kolichestvo * 1.5) + "л " + comboBox6.Text + " и " + kolichestvo + " тары по 1.5л");
                                        conn.Close();

                                        checkBox11.Checked = false;
                                    }
                                    else
                                    {

                                        //добавление напитка
                                        string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                        "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                        SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                        cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                        cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        cmd14.Parameters.AddWithValue("@amount", 0.5);
                                        cmd14.Parameters.AddWithValue("@price", CenaNapit);
                                        cmd14.Parameters.AddWithValue("@summa", CenaNapit * 0.5);
                                        SQLiteDataAdapter da14 = new SQLiteDataAdapter(cmd14);
                                        DataTable dt14 = new DataTable();
                                        da14.Fill(dt14);
                                        cmd14.Dispose();

                                        //скидка клиента
                                        string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                        SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                        cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        summ = (Double)cmd4.ExecuteScalar();
                                        textBox11.Text = Convert.ToString(summ);
                                        if (comboBox8.SelectedIndex != 0)
                                        {
                                            textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                        }
                                        cmd4.Dispose();

                                        //уменьшение количества напитка
                                        string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                        SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                        cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                        cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 0.5);
                                        cmd6.ExecuteNonQuery();
                                        cmd6.Dispose();

                                        //уменьшение количества тар
                                        string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                        SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                        cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                        cmd13.ExecuteNonQuery();
                                        cmd13.Dispose();
                                        //MessageBox.Show("Добавлено " + Convert.ToDouble(kolichestvo * 1.5) + "л " + comboBox6.Text + " и " + kolichestvo + " тары по 1.5л");
                                        conn.Close();

                                        checkBox11.Checked = false;

                                    }

                                    string query15 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    conn.Open();
                                    SQLiteCommand cmd15 = new SQLiteCommand(query15, conn);
                                    cmd15.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd15.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Int64 count = (Int64)cmd15.ExecuteScalar();
                                    cmd15.Dispose();

                                    if (count > 0)
                                    {
                                        string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                        SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                        cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        Double amount = (Double)cmd10.ExecuteScalar();
                                        textBox10.Text = Convert.ToString(amount);
                                        cmd10.Dispose();

                                        textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + 1);

                                        //пересчет общей суммы
                                        string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                        SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                        cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        Double summa = (Double)cmd11.ExecuteScalar();
                                        textBox12.Text = Convert.ToString(summa);
                                        cmd11.Dispose();

                                        textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + Convert.ToDouble(comboBox22.Text));

                                        string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                        SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                        cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                        cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                        cmd9.ExecuteNonQuery();
                                        cmd9.Dispose();

                                        //скидка клиента
                                        string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                        SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                        cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        summ = (Double)cmd4.ExecuteScalar();
                                        textBox11.Text = Convert.ToString(summ);
                                        if (comboBox8.SelectedIndex != 0)
                                        {
                                            textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                        }
                                        cmd4.Dispose();

                                        //уменьшение количества напитка
                                        string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                        SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                        cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                        cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 0.5);
                                        cmd6.ExecuteNonQuery();
                                        cmd6.Dispose();

                                        //уменьшение количества тар
                                        string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                        SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                        cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                        cmd13.ExecuteNonQuery();
                                        cmd13.Dispose();

                                        if (checkBox10.Checked)
                                        {
                                            string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                            " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                            " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                            " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                            SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                            SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                            DataTable dt5 = new DataTable();
                                            da5.Fill(dt5);


                                            comboBox6.DataSource = dt5;
                                            comboBox6.DisplayMember = "Nazvanie";
                                            comboBox6.ValueMember = "id_InvoiceTable";
                                            comboBox6.SelectedIndex = -1;
                                            comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                            comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                            comboBox12.DataSource = dt5;
                                            comboBox12.DisplayMember = "id_InvoiceTable";
                                            comboBox12.ValueMember = "id_InvoiceTable";

                                            comboBox2.DataSource = dt5;
                                            comboBox2.DisplayMember = "Amount";
                                            comboBox2.ValueMember = "id_InvoiceTable";
                                            comboBox2.SelectedIndex = -1;

                                            comboBox15.DataSource = dt5;
                                            comboBox15.DisplayMember = "PriceSale";
                                            comboBox15.ValueMember = "id_InvoiceTable";
                                            comboBox15.SelectedIndex = -1;
                                            textBox27.Text = comboBox15.Text;

                                            cmd5.Dispose();
                                        }

                                        else
                                        {
                                            string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                            " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                            " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                            " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                            SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                            SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                            DataTable dt5 = new DataTable();
                                            da5.Fill(dt5);


                                            comboBox6.DataSource = dt5;
                                            comboBox6.DisplayMember = "Nazvanie";
                                            comboBox6.ValueMember = "id_InvoiceTable";
                                            comboBox6.SelectedIndex = -1;
                                            comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                            comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                            comboBox12.DataSource = dt5;
                                            comboBox12.DisplayMember = "id_InvoiceTable";
                                            comboBox12.ValueMember = "id_InvoiceTable";

                                            comboBox2.DataSource = dt5;
                                            comboBox2.DisplayMember = "Amount";
                                            comboBox2.ValueMember = "id_InvoiceTable";
                                            comboBox2.SelectedIndex = -1;

                                            comboBox15.DataSource = dt5;
                                            comboBox15.DisplayMember = "PriceSale";
                                            comboBox15.ValueMember = "id_InvoiceTable";
                                            comboBox15.SelectedIndex = -1;
                                            textBox27.Text = comboBox15.Text;

                                            cmd5.Dispose();
                                        }
                                        string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                            "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
                    "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По цене],  Summa || '  руб' AS Сумма  FROM RealisationOnNomenclatures " +
                    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                    " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
                    " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                        SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                        cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                        DataTable dt3 = new DataTable();
                                        da3.Fill(dt3);
                                        dataGridView8.DataSource = dt3;
                                        dataGridView8.Columns[0].Visible = false;
                                        dataGridView8.Columns[1].Visible = false;
                                        cmd3.Dispose();
                                        dataGridView8.Select();
                                        conn.Close();
                                        checkBox11.Checked = false;

                                        dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                        dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                        dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                        yt_Button11.Enabled = true;
                                        dataGridView8.Select();
                                    }
                                    else
                                    {

                                        //добавление тар
                                        string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                        "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                        SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                        cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        cmd14.Parameters.AddWithValue("@amount", 1);
                                        cmd14.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                        cmd14.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text));
                                        SQLiteDataAdapter da14 = new SQLiteDataAdapter(cmd14);
                                        DataTable dt14 = new DataTable();
                                        da14.Fill(dt14);
                                        cmd14.Dispose();

                                        //скидка клиента
                                        string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                        SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                        cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        summ = (Double)cmd4.ExecuteScalar();
                                        textBox11.Text = Convert.ToString(summ);
                                        if (comboBox8.SelectedIndex != 0)
                                        {
                                            textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                        }
                                        cmd4.Dispose();

                                        //уменьшение количества напитка
                                        string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                        SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                        cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                        cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 0.5);
                                        cmd6.ExecuteNonQuery();
                                        cmd6.Dispose();

                                        //уменьшение количества тар
                                        string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                        SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                        cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                        cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                        cmd13.ExecuteNonQuery();
                                        cmd13.Dispose();

                                        if (checkBox10.Checked)
                                        {
                                            string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                                " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                                " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                                " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                            SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                            SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                            DataTable dt5 = new DataTable();
                                            da5.Fill(dt5);

                                            comboBox6.DataSource = dt5;
                                            comboBox6.DisplayMember = "Nazvanie";
                                            comboBox6.ValueMember = "id_InvoiceTable";
                                            comboBox6.SelectedIndex = -1;
                                            comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                            comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                            comboBox12.DataSource = dt5;
                                            comboBox12.DisplayMember = "id_InvoiceTable";
                                            comboBox12.ValueMember = "id_InvoiceTable";

                                            comboBox2.DataSource = dt5;
                                            comboBox2.DisplayMember = "Amount";
                                            comboBox2.ValueMember = "id_InvoiceTable";
                                            comboBox2.SelectedIndex = -1;

                                            comboBox15.DataSource = dt5;
                                            comboBox15.DisplayMember = "PriceSale";
                                            comboBox15.ValueMember = "id_InvoiceTable";
                                            comboBox15.SelectedIndex = -1;
                                            textBox27.Text = comboBox15.Text;

                                            cmd5.Dispose();
                                        }
                                        else
                                        {
                                            string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                                " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                                " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                                " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                    " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                            SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                            SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                            DataTable dt5 = new DataTable();
                                            da5.Fill(dt5);

                                            comboBox6.DataSource = dt5;
                                            comboBox6.DisplayMember = "Nazvanie";
                                            comboBox6.ValueMember = "id_InvoiceTable";
                                            comboBox6.SelectedIndex = -1;
                                            comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                            comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                            comboBox12.DataSource = dt5;
                                            comboBox12.DisplayMember = "id_InvoiceTable";
                                            comboBox12.ValueMember = "id_InvoiceTable";

                                            comboBox2.DataSource = dt5;
                                            comboBox2.DisplayMember = "Amount";
                                            comboBox2.ValueMember = "id_InvoiceTable";
                                            comboBox2.SelectedIndex = -1;

                                            comboBox15.DataSource = dt5;
                                            comboBox15.DisplayMember = "PriceSale";
                                            comboBox15.ValueMember = "id_InvoiceTable";
                                            comboBox15.SelectedIndex = -1;
                                            textBox27.Text = comboBox15.Text;

                                            cmd5.Dispose();
                                        }

                                        string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                            "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
                    "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По цене], Summa || '  руб'  AS Сумма  FROM RealisationOnNomenclatures " +
                    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                    " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
                    " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                        SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                        cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                        SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                        DataTable dt3 = new DataTable();
                                        da3.Fill(dt3);
                                        dataGridView8.DataSource = dt3;
                                        dataGridView8.Columns[0].Visible = false;
                                        dataGridView8.Columns[1].Visible = false;
                                        cmd3.Dispose();
                                        dataGridView8.Select();
                                        conn.Close();
                                        checkBox11.Checked = false;

                                        dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                        dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                        dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                        yt_Button11.Enabled = true;
                                        dataGridView8.Select();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            checkBox5.Checked = false;
        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex == -1)
            {
                yt_Button16.Enabled = false;
            }
            else
            {
                yt_Button4.Enabled = true;
                yt_Button5.Enabled = true;
                yt_Button6.Enabled = true;
                yt_Button7.Enabled = true;
            }
            checkBox5.Checked = false;
        }

        private void yt_Button8_Click(object sender, EventArgs e)
        {
            if (dat == 0)
                dateTimePicker5.Value = DateTime.Now;

            if (comboBox7.Text.Trim() == "" | comboBox16.Text.Trim() == "" | comboBox17.Text.Trim() == "" | comboBox7.Text.Trim() == "--")
                MessageBox.Show("Выберите товар!");
            else if (textBox4.Text.Trim() == "")
                MessageBox.Show("Укажите количество!");
            else if (textBox16.Text.Trim() == "")
                MessageBox.Show("Укажите цену!");
            else
            {
                if (textBox3.Text == "")
                {
                    string query = "INSERT INTO Realisations (id_Client, id_Employee, Date) VALUES (@idclient, @idemployee, @date)";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                    cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(comboBox11.SelectedValue));
                    cmd.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmd.Dispose();

                    string query1 = "SELECT MAX(id_Realisation) FROM Realisations";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                    Int64 id = (Int64)cmd1.ExecuteScalar();
                    textBox3.Text = Convert.ToString(id);
                    cmd1.Dispose();

                    string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                    cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox4.Text));
                    cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(textBox16.Text));
                    cmd2.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox5.Text));
                    SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    cmd2.Dispose();

                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    summ = (Double)cmd4.ExecuteScalar();
                    textBox11.Text = Convert.ToString(summ);
                    if (comboBox8.SelectedIndex != 0)
                    {
                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                    }
                    cmd4.Dispose();


                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                    SQLiteCommand cmd7 = new SQLiteCommand(query5, conn);
                    cmd7.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                    cmd7.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox17.Text) - Convert.ToDouble(textBox4.Text));
                    cmd7.ExecuteNonQuery();
                    cmd7.Dispose();
                    conn.Close();

                    if (checkBox10.Checked)
                    {
                        query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
    " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
    " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm FROM InvoiceTables " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' AND Amount > 0 OR id_InvoiceTable=1 ORDER BY Nazvanie";
                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                        DataTable dt5 = new DataTable();
                        da5.Fill(dt5);
                        comboBox7.DataSource = dt5;
                        comboBox7.DisplayMember = "Nazvanie";
                        comboBox7.ValueMember = "id_InvoiceTable";
                        comboBox7.SelectedIndex = -1;
                        comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                        comboBox13.DataSource = dt5;
                        comboBox13.DisplayMember = "id_InvoiceTable";
                        comboBox13.ValueMember = "id_InvoiceTable";

                        comboBox17.DataSource = dt5;
                        comboBox17.DisplayMember = "Amount";
                        comboBox17.ValueMember = "id_InvoiceTable";
                        comboBox17.SelectedIndex = -1;

                        comboBox19.DataSource = dt5;
                        comboBox19.DisplayMember = "EdIzm";
                        comboBox19.ValueMember = "id_InvoiceTable";
                        comboBox19.SelectedIndex = -1;

                        comboBox16.DataSource = dt5;
                        comboBox16.DisplayMember = "PriceSale";
                        comboBox16.ValueMember = "id_InvoiceTable";
                        comboBox16.SelectedIndex = -1;
                        textBox16.Text = comboBox16.Text;

                        cmd5.Dispose();
                    }
                    else
                    {
                        query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
    " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
    " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm FROM InvoiceTables " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' ORDER BY Nazvanie";
                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                        DataTable dt5 = new DataTable();
                        da5.Fill(dt5);
                        comboBox7.DataSource = dt5;
                        comboBox7.DisplayMember = "Nazvanie";
                        comboBox7.ValueMember = "id_InvoiceTable";
                        comboBox7.SelectedIndex = -1;
                        comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                        comboBox13.DataSource = dt5;
                        comboBox13.DisplayMember = "id_InvoiceTable";
                        comboBox13.ValueMember = "id_InvoiceTable";

                        comboBox17.DataSource = dt5;
                        comboBox17.DisplayMember = "Amount";
                        comboBox17.ValueMember = "id_InvoiceTable";
                        comboBox17.SelectedIndex = -1;

                        comboBox19.DataSource = dt5;
                        comboBox19.DisplayMember = "EdIzm";
                        comboBox19.ValueMember = "id_InvoiceTable";
                        comboBox19.SelectedIndex = -1;

                        comboBox16.DataSource = dt5;
                        comboBox16.DisplayMember = "PriceSale";
                        comboBox16.ValueMember = "id_InvoiceTable";
                        comboBox16.SelectedIndex = -1;
                        textBox16.Text = comboBox16.Text;

                        cmd5.Dispose();
                    }

                    string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
"  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По цене], Summa || '  руб'  AS Сумма  FROM RealisationOnNomenclatures " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
" WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView8.DataSource = dt3;
                    dataGridView8.Columns[0].Visible = false;
                    dataGridView8.Columns[1].Visible = false;
                    cmd3.Dispose();
                    dataGridView8.Select();
                    conn.Close();
                    checkBox11.Checked = false;

                    dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView8.Select();
                    yt_Button11.Enabled = true;
                    checkBox9.Checked = false;
                }
                else
                {

                    string query12 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation AND Price=@price";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd12 = new SQLiteCommand(query12, conn);
                    cmd12.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                    cmd12.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    cmd12.Parameters.AddWithValue("@price", Convert.ToDouble(textBox16.Text));
                    Int64 count = (Int64)cmd12.ExecuteScalar();
                    cmd12.Dispose();

                    if (count > 0)
                    {
                        string query19 = "SELECT Price AS Price FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation AND Price=@price";
                        SQLiteCommand cmd19 = new SQLiteCommand(query19, conn);
                        cmd19.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                        cmd19.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                        cmd19.Parameters.AddWithValue("@price", Convert.ToDouble(textBox16.Text));
                        Double price = (Double)cmd19.ExecuteScalar();
                        textBox22.Text = Convert.ToString(price);
                        cmd19.Dispose();

                        if (Convert.ToDouble(textBox22.Text) == price)
                        {

                            string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation AND Price=@price";
                            SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                            cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                            cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                            cmd10.Parameters.AddWithValue("@price", price);
                            Double amount = (Double)cmd10.ExecuteScalar();
                            textBox10.Text = Convert.ToString(amount);
                            cmd10.Dispose();

                            textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + (Convert.ToDouble(textBox4.Text)));

                            string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation AND Price=@price";
                            SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                            cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                            cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                            cmd11.Parameters.AddWithValue("@price", price);
                            Double summa = (Double)cmd11.ExecuteScalar();
                            textBox12.Text = Convert.ToString(summa);
                            cmd11.Dispose();

                            textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + Convert.ToDouble(textBox5.Text));

                            string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation AND Price=@price";
                            SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                            cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                            cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                            cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                            cmd9.Parameters.AddWithValue("@price", price);
                            cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                            cmd9.ExecuteNonQuery();
                            cmd9.Dispose();

                            string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                            SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                            cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                            summ = (Double)cmd4.ExecuteScalar();
                            textBox11.Text = Convert.ToString(summ);
                            if (comboBox8.SelectedIndex != 0)
                            {
                                textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                            }
                            cmd4.Dispose();


                            string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                            SQLiteCommand cmd7 = new SQLiteCommand(query5, conn);
                            cmd7.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                            cmd7.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox17.Text) - Convert.ToDouble(textBox4.Text));
                            cmd7.ExecuteNonQuery();
                            cmd7.Dispose();


                            if (checkBox10.Checked)
                            {
                                string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
            " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
            " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
            " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' AND Amount > 0 OR id_InvoiceTable=1 ORDER BY Nazvanie";
                                SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                DataTable dt5 = new DataTable();
                                da5.Fill(dt5);
                                comboBox7.DataSource = dt5;
                                comboBox7.DisplayMember = "Nazvanie";
                                comboBox7.ValueMember = "id_InvoiceTable";
                                comboBox7.SelectedIndex = -1;
                                comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                                comboBox13.DataSource = dt5;
                                comboBox13.DisplayMember = "id_InvoiceTable";
                                comboBox13.ValueMember = "id_InvoiceTable";

                                comboBox17.DataSource = dt5;
                                comboBox17.DisplayMember = "Amount";
                                comboBox17.ValueMember = "id_InvoiceTable";
                                comboBox17.SelectedIndex = -1;

                                comboBox19.DataSource = dt5;
                                comboBox19.DisplayMember = "EdIzm";
                                comboBox19.ValueMember = "id_InvoiceTable";
                                comboBox19.SelectedIndex = -1;

                                comboBox16.DataSource = dt5;
                                comboBox16.DisplayMember = "PriceSale";
                                comboBox16.ValueMember = "id_InvoiceTable";
                                comboBox16.SelectedIndex = -1;
                                textBox16.Text = comboBox16.Text;

                                cmd5.Dispose();
                            }
                            else
                            {
                                string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
            " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
            " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
            " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' ORDER BY Nazvanie";
                                SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                DataTable dt5 = new DataTable();
                                da5.Fill(dt5);
                                comboBox7.DataSource = dt5;
                                comboBox7.DisplayMember = "Nazvanie";
                                comboBox7.ValueMember = "id_InvoiceTable";
                                comboBox7.SelectedIndex = -1;
                                comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                                comboBox13.DataSource = dt5;
                                comboBox13.DisplayMember = "id_InvoiceTable";
                                comboBox13.ValueMember = "id_InvoiceTable";

                                comboBox17.DataSource = dt5;
                                comboBox17.DisplayMember = "Amount";
                                comboBox17.ValueMember = "id_InvoiceTable";
                                comboBox17.SelectedIndex = -1;

                                comboBox19.DataSource = dt5;
                                comboBox19.DisplayMember = "EdIzm";
                                comboBox19.ValueMember = "id_InvoiceTable";
                                comboBox19.SelectedIndex = -1;

                                comboBox16.DataSource = dt5;
                                comboBox16.DisplayMember = "PriceSale";
                                comboBox16.ValueMember = "id_InvoiceTable";
                                comboBox16.SelectedIndex = -1;
                                textBox16.Text = comboBox16.Text;

                                cmd5.Dispose();
                            }

                            string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
        "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По цене],  Summa || '  руб'  AS Сумма  FROM RealisationOnNomenclatures " +
        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
        " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                            SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                            cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                            SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                            DataTable dt3 = new DataTable();
                            da3.Fill(dt3);
                            dataGridView8.DataSource = dt3;
                            dataGridView8.Columns[0].Visible = false;
                            dataGridView8.Columns[1].Visible = false;
                            cmd3.Dispose();
                            dataGridView8.Select();
                            conn.Close();
                            checkBox11.Checked = false;

                            dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            yt_Button11.Enabled = true;
                            checkBox9.Checked = false;
                        }
                        else
                        {
                            string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
"VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                            SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                            cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                            cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                            cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox4.Text));
                            cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(textBox16.Text));
                            cmd2.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox5.Text));
                            SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                            DataTable dt2 = new DataTable();
                            da2.Fill(dt2);
                            cmd2.Dispose();

                            string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                            SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                            cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                            summ = (Double)cmd4.ExecuteScalar();
                            textBox11.Text = Convert.ToString(summ);
                            if (comboBox8.SelectedIndex != 0)
                            {
                                textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                            }
                            cmd4.Dispose();


                            string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                            SQLiteCommand cmd7 = new SQLiteCommand(query5, conn);
                            cmd7.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                            cmd7.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox17.Text) - Convert.ToDouble(textBox4.Text));
                            cmd7.ExecuteNonQuery();
                            cmd7.Dispose();


                            if (checkBox10.Checked)
                            {
                                string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
            " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
            " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
            " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' AND Amount > 0 OR id_InvoiceTable=1 ORDER BY Nazvanie";
                                SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                DataTable dt5 = new DataTable();
                                da5.Fill(dt5);
                                comboBox7.DataSource = dt5;
                                comboBox7.DisplayMember = "Nazvanie";
                                comboBox7.ValueMember = "id_InvoiceTable";
                                comboBox7.SelectedIndex = -1;
                                comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                                comboBox13.DataSource = dt5;
                                comboBox13.DisplayMember = "id_InvoiceTable";
                                comboBox13.ValueMember = "id_InvoiceTable";

                                comboBox17.DataSource = dt5;
                                comboBox17.DisplayMember = "Amount";
                                comboBox17.ValueMember = "id_InvoiceTable";
                                comboBox17.SelectedIndex = -1;

                                comboBox19.DataSource = dt5;
                                comboBox19.DisplayMember = "EdIzm";
                                comboBox19.ValueMember = "id_InvoiceTable";
                                comboBox19.SelectedIndex = -1;

                                comboBox16.DataSource = dt5;
                                comboBox16.DisplayMember = "PriceSale";
                                comboBox16.ValueMember = "id_InvoiceTable";
                                comboBox16.SelectedIndex = -1;
                                textBox16.Text = comboBox16.Text;

                                cmd5.Dispose();
                            }
                            else
                            {
                                string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
            " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
            " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
            " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' ORDER BY Nazvanie";
                                SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                DataTable dt5 = new DataTable();
                                da5.Fill(dt5);
                                comboBox7.DataSource = dt5;
                                comboBox7.DisplayMember = "Nazvanie";
                                comboBox7.ValueMember = "id_InvoiceTable";
                                comboBox7.SelectedIndex = -1;
                                comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                                comboBox13.DataSource = dt5;
                                comboBox13.DisplayMember = "id_InvoiceTable";
                                comboBox13.ValueMember = "id_InvoiceTable";

                                comboBox17.DataSource = dt5;
                                comboBox17.DisplayMember = "Amount";
                                comboBox17.ValueMember = "id_InvoiceTable";
                                comboBox17.SelectedIndex = -1;

                                comboBox19.DataSource = dt5;
                                comboBox19.DisplayMember = "EdIzm";
                                comboBox19.ValueMember = "id_InvoiceTable";
                                comboBox19.SelectedIndex = -1;

                                comboBox16.DataSource = dt5;
                                comboBox16.DisplayMember = "PriceSale";
                                comboBox16.ValueMember = "id_InvoiceTable";
                                comboBox16.SelectedIndex = -1;
                                textBox16.Text = comboBox16.Text;

                                cmd5.Dispose();
                            }

                            string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
        "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По цене], Summa || '  руб'  AS Сумма  FROM RealisationOnNomenclatures " +
        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
        " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                            SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                            cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                            SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                            DataTable dt3 = new DataTable();
                            da3.Fill(dt3);
                            dataGridView8.DataSource = dt3;
                            dataGridView8.Columns[0].Visible = false;
                            dataGridView8.Columns[1].Visible = false;
                            cmd3.Dispose();
                            dataGridView8.Select();
                            conn.Close();
                            checkBox11.Checked = false;

                            dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            yt_Button11.Enabled = true;
                            checkBox9.Checked = false;
                        }
                    }
                    else
                    {
                        string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
"VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                        SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                        cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                        cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                        cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox4.Text));
                        cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(textBox16.Text));
                        cmd2.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox5.Text));
                        SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                        DataTable dt2 = new DataTable();
                        da2.Fill(dt2);
                        cmd2.Dispose();

                        string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                        SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                        cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                        summ = (Double)cmd4.ExecuteScalar();
                        textBox11.Text = Convert.ToString(summ);
                        if (comboBox8.SelectedIndex != 0)
                        {
                            textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                        }
                        cmd4.Dispose();


                        string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                        SQLiteCommand cmd7 = new SQLiteCommand(query5, conn);
                        cmd7.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox13.Text));
                        cmd7.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox17.Text) - Convert.ToDouble(textBox4.Text));
                        cmd7.ExecuteNonQuery();
                        cmd7.Dispose();


                        if (checkBox10.Checked)
                        {
                            string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
        " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' AND Amount > 0 OR id_InvoiceTable=1 ORDER BY Nazvanie";
                            SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                            SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                            DataTable dt5 = new DataTable();
                            da5.Fill(dt5);
                            comboBox7.DataSource = dt5;
                            comboBox7.DisplayMember = "Nazvanie";
                            comboBox7.ValueMember = "id_InvoiceTable";
                            comboBox7.SelectedIndex = -1;
                            comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                            comboBox13.DataSource = dt5;
                            comboBox13.DisplayMember = "id_InvoiceTable";
                            comboBox13.ValueMember = "id_InvoiceTable";

                            comboBox17.DataSource = dt5;
                            comboBox17.DisplayMember = "Amount";
                            comboBox17.ValueMember = "id_InvoiceTable";
                            comboBox17.SelectedIndex = -1;

                            comboBox19.DataSource = dt5;
                            comboBox19.DisplayMember = "EdIzm";
                            comboBox19.ValueMember = "id_InvoiceTable";
                            comboBox19.SelectedIndex = -1;

                            comboBox16.DataSource = dt5;
                            comboBox16.DisplayMember = "PriceSale";
                            comboBox16.ValueMember = "id_InvoiceTable";
                            comboBox16.SelectedIndex = -1;
                            textBox16.Text = comboBox16.Text;

                            cmd5.Dispose();
                        }
                        else
                        {
                            string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
        " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' ORDER BY Nazvanie";
                            SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                            SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                            DataTable dt5 = new DataTable();
                            da5.Fill(dt5);
                            comboBox7.DataSource = dt5;
                            comboBox7.DisplayMember = "Nazvanie";
                            comboBox7.ValueMember = "id_InvoiceTable";
                            comboBox7.SelectedIndex = -1;
                            comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                            comboBox13.DataSource = dt5;
                            comboBox13.DisplayMember = "id_InvoiceTable";
                            comboBox13.ValueMember = "id_InvoiceTable";

                            comboBox17.DataSource = dt5;
                            comboBox17.DisplayMember = "Amount";
                            comboBox17.ValueMember = "id_InvoiceTable";
                            comboBox17.SelectedIndex = -1;

                            comboBox19.DataSource = dt5;
                            comboBox19.DisplayMember = "EdIzm";
                            comboBox19.ValueMember = "id_InvoiceTable";
                            comboBox19.SelectedIndex = -1;

                            comboBox16.DataSource = dt5;
                            comboBox16.DisplayMember = "PriceSale";
                            comboBox16.ValueMember = "id_InvoiceTable";
                            comboBox16.SelectedIndex = -1;
                            textBox16.Text = comboBox16.Text;

                            cmd5.Dispose();
                        }

                        string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                            "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
    "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По цене], Summa || '  руб' AS Сумма  FROM RealisationOnNomenclatures " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
    " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                        SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                        cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                        SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                        DataTable dt3 = new DataTable();
                        da3.Fill(dt3);
                        dataGridView8.DataSource = dt3;
                        dataGridView8.Columns[0].Visible = false;
                        dataGridView8.Columns[1].Visible = false;
                        cmd3.Dispose();
                        dataGridView8.Select();
                        conn.Close();
                        checkBox11.Checked = false;

                        dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        yt_Button11.Enabled = true;
                        checkBox9.Checked = false;
                    }
                }
            }
            textBox4.Text = "0";
            textBox23.Text = "0";
        }


        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            double n = 0, cena = 0, summa = 0;
            if (textBox16.Text != "" && textBox4.Text != "")
            {
                if (textBox23.Visible == true)
                {
                    n = Convert.ToDouble(textBox4.Text);
                    cena = Convert.ToDouble(textBox16.Text);
                    summa = n * cena;
                    textBox5.Text = summa.ToString();
                    textBox23.Text = Convert.ToString(Convert.ToDouble(textBox4.Text) * 1000);
                }
                n = Convert.ToDouble(textBox4.Text);
                cena = Convert.ToDouble(textBox16.Text);
                summa = n * cena;
                textBox5.Text = summa.ToString();
            }
            else textBox5.Text = "0";
        }

        private void comboBox16_TextChanged(object sender, EventArgs e)
        {
            double summa;
            if (comboBox16.Text != "" && textBox4.Text != "" && comboBox16.Text != "System.Data.DataRowView")
            {
                double n = Convert.ToDouble(textBox4.Text);
                double cena = Convert.ToDouble(comboBox16.Text);
                summa = n * cena;
                textBox5.Text = summa.ToString();
            }
            else textBox5.Text = "0";
        }

        private void yt_Button5_Click_1(object sender, EventArgs e)
        {
            kolichestvo = 0;
            if (dat == 0)
                dateTimePicker5.Value = DateTime.Now;
            string query16 = "SELECT COUNT(id_InvoiceTable) AS count FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" WHERE Nomenclatures.Name = 'Тара 1 л' ";
            SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn2.Open();
            SQLiteCommand cmd16 = new SQLiteCommand(query16, conn2);
            Int64 count3 = (Int64)cmd16.ExecuteScalar();
            cmd16.Dispose();
            conn2.Close();

            if (count3 == 0)
            {
                MessageBox.Show("Такой тары нет в номенклатуре!");
            }
            else
            {
                if (textBox27.Text.Trim() == "")
                    MessageBox.Show("Введите цену!");
                else
                {
                    if (checkBox5.Checked)
                    {
                        kolichestvo = Convert.ToDouble(textBox19.Text);

                        if (kolichestvo == 0)
                            return;
                        else
                        {

                            string query7 = "SELECT id_InvoiceTable, Nomenclatures.id_Nomenclature AS id_Nomenclature, Amount, PriceSale FROM InvoiceTables " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            "WHERE Nomenclatures.Name = 'Тара 1 л'";
                            SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                            conn1.Open();
                            SQLiteCommand cmd7 = new SQLiteCommand(query7, conn1);
                            SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                            DataTable dt7 = new DataTable();
                            da7.Fill(dt7);
                            comboBox23.DataSource = dt7;
                            comboBox23.DisplayMember = "id_InvoiceTable";
                            comboBox23.ValueMember = "id_InvoiceTable";

                            comboBox20.DataSource = dt7;
                            comboBox20.DisplayMember = "id_Nomenclature";
                            comboBox20.ValueMember = "id_InvoiceTable";

                            comboBox21.DataSource = dt7;
                            comboBox21.DisplayMember = "Amount";
                            comboBox21.ValueMember = "id_InvoiceTable";

                            comboBox22.DataSource = dt7;
                            comboBox22.DisplayMember = "PriceSale";
                            comboBox22.ValueMember = "id_InvoiceTable";
                            conn1.Close();


                            if (textBox3.Text == "")
                            {
                                string query = "INSERT INTO Realisations (id_Client, id_Employee, Date) VALUES (@idclient, @idemployee, @date)";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                                cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                                cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(comboBox11.SelectedValue));
                                cmd.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                cmd.Dispose();

                                string query1 = "SELECT MAX(id_Realisation) FROM Realisations";
                                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                                Int64 id = (Int64)cmd1.ExecuteScalar();
                                textBox3.Text = Convert.ToString(id);
                                cmd1.Dispose();

                                string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                                cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(numericUpDown2.Value));
                                cmd2.Parameters.AddWithValue("@price", CenaNapit);
                                cmd2.Parameters.AddWithValue("@summa", CenaNapit * Convert.ToDouble(numericUpDown2.Value));
                                SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                                DataTable dt2 = new DataTable();
                                da2.Fill(dt2);
                                cmd2.Dispose();


                                string query6 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd6 = new SQLiteCommand(query6, conn);
                                cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd6.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(kolichestvo));
                                cmd6.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                cmd6.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text) * kolichestvo);
                                SQLiteDataAdapter da6 = new SQLiteDataAdapter(cmd6);
                                DataTable dt6 = new DataTable();
                                da6.Fill(dt6);
                                cmd6.Dispose();


                                string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                summ = (Double)cmd4.ExecuteScalar();
                                textBox11.Text = Convert.ToString(summ);
                                if (comboBox8.SelectedIndex != 0)
                                {
                                    textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                }
                                cmd4.Dispose();

                                //Уменьшение остатков напитка
                                string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd8 = new SQLiteCommand(query5, conn);
                                cmd8.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd8.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(numericUpDown2.Value));
                                cmd8.ExecuteNonQuery();
                                cmd8.Dispose();

                                ostatok = ostatok - Convert.ToDouble(numericUpDown2.Value);

                                //Уменьшение остатков тары
                                string query9 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - kolichestvo);
                                cmd9.ExecuteNonQuery();
                                cmd9.Dispose();
                                conn.Close();
                                checkBox11.Checked = false;

                            }

                            else
                            {

                                string query15 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd15 = new SQLiteCommand(query15, conn);
                                cmd15.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                cmd15.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                Int64 count2 = (Int64)cmd15.ExecuteScalar();
                                cmd15.Dispose();

                                if (count2 > 0)
                                {
                                    string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                    cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double amount = (Double)cmd10.ExecuteScalar();
                                    textBox10.Text = Convert.ToString(amount);
                                    cmd10.Dispose();

                                    textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + Convert.ToDouble(kolichestvo));

                                    //пересчет общей суммы
                                    string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                    cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double summa = (Double)cmd11.ExecuteScalar();
                                    textBox12.Text = Convert.ToString(summa);
                                    cmd11.Dispose();

                                    textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + CenaNapit * Convert.ToDouble(kolichestvo));

                                    string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                    cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(numericUpDown2.Value));
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();
                                    conn.Close();
                                    checkBox11.Checked = false;
                                    ostatok = ostatok - Convert.ToDouble(numericUpDown2.Value);
                                }
                                else
                                {

                                    //добавление напитка
                                    string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                    cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd14.Parameters.AddWithValue("@amount", Convert.ToDouble(kolichestvo));
                                    cmd14.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                    cmd14.Parameters.AddWithValue("@summa", CenaNapit * Convert.ToDouble(numericUpDown2.Value));
                                    SQLiteDataAdapter da14 = new SQLiteDataAdapter(cmd14);
                                    DataTable dt14 = new DataTable();
                                    da14.Fill(dt14);
                                    cmd14.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(numericUpDown2.Value));
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();
                                    conn.Close();
                                    checkBox11.Checked = false;
                                    ostatok = ostatok - Convert.ToDouble(numericUpDown2.Value);
                                }


                                string query12 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                conn.Open();
                                SQLiteCommand cmd12 = new SQLiteCommand(query12, conn);
                                cmd12.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd12.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                Int64 count = (Int64)cmd12.ExecuteScalar();
                                cmd12.Dispose();

                                if (count > 0)
                                {
                                    string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                    cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double amount = (Double)cmd10.ExecuteScalar();
                                    textBox10.Text = Convert.ToString(amount);
                                    cmd10.Dispose();

                                    textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + kolichestvo);

                                    string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                    cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double summa = (Double)cmd11.ExecuteScalar();
                                    textBox12.Text = Convert.ToString(summa);
                                    cmd11.Dispose();

                                    textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + Convert.ToDouble(comboBox22.Text));

                                    string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                    cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - kolichestvo);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();
                                    conn.Close();
                                    checkBox11.Checked = false;

                                }
                                else
                                {

                                    string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                    cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd14.Parameters.AddWithValue("@amount", Convert.ToDouble(kolichestvo));
                                    cmd14.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                    cmd14.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text) * kolichestvo);
                                    cmd14.ExecuteNonQuery();
                                    cmd14.Dispose();

                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - kolichestvo);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();
                                    conn.Close();
                                    checkBox11.Checked = false;

                                }
                            }
                        }
                    }
                    else
                    {

                        if (comboBox6.Text.Trim() == "" | comboBox2.Text.Trim() == "" | comboBox15.Text.Trim() == "" | comboBox6.Text.Trim() == "-")
                            MessageBox.Show("Выберите напиток!");
                        else
                        {
                            CenaNapit = Convert.ToDouble(textBox27.Text);
                            string query7 = "SELECT id_InvoiceTable, Nomenclatures.id_Nomenclature AS id_Nomenclature, Amount, PriceSale FROM InvoiceTables " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            "WHERE Nomenclatures.Name = 'Тара 1 л'";
                            SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                            conn1.Open();
                            SQLiteCommand cmd7 = new SQLiteCommand(query7, conn1);
                            SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                            DataTable dt7 = new DataTable();
                            da7.Fill(dt7);
                            comboBox23.DataSource = dt7;
                            comboBox23.DisplayMember = "id_InvoiceTable";
                            comboBox23.ValueMember = "id_InvoiceTable";

                            comboBox20.DataSource = dt7;
                            comboBox20.DisplayMember = "id_Nomenclature";
                            comboBox20.ValueMember = "id_InvoiceTable";

                            comboBox21.DataSource = dt7;
                            comboBox21.DisplayMember = "Amount";
                            comboBox21.ValueMember = "id_InvoiceTable";

                            comboBox22.DataSource = dt7;
                            comboBox22.DisplayMember = "PriceSale";
                            comboBox22.ValueMember = "id_InvoiceTable";
                            conn1.Close();

                            if (textBox3.Text == "")
                            {
                                string query = "INSERT INTO Realisations (id_Client, id_Employee, Date) VALUES (@idclient, @idemployee, @date)";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                                cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                                cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(comboBox11.SelectedValue));
                                cmd.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                string query1 = "SELECT MAX(id_Realisation) FROM Realisations";
                                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                                Int64 id = (Int64)cmd1.ExecuteScalar();
                                textBox3.Text = Convert.ToString(id);
                                cmd1.Dispose();

                                string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                                cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(1));
                                cmd2.Parameters.AddWithValue("@price", CenaNapit);
                                cmd2.Parameters.AddWithValue("@summa", CenaNapit * 1);
                                cmd2.ExecuteNonQuery();
                                cmd2.Dispose();


                                query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd6 = new SQLiteCommand(query2, conn);
                                cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd6.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd6.Parameters.AddWithValue("@amount", 1);
                                cmd6.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                cmd6.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text));
                                cmd6.ExecuteNonQuery();
                                cmd6.Dispose();


                                string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                summ = (Double)cmd4.ExecuteScalar();
                                textBox11.Text = Convert.ToString(summ);
                                if (comboBox8.SelectedIndex != 0)
                                {
                                    textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                }
                                cmd4.Dispose();

                                //Уменьшение остатков напитка
                                string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd8 = new SQLiteCommand(query5, conn);
                                cmd8.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd8.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 1);
                                cmd8.ExecuteNonQuery();
                                cmd8.Dispose();

                                //Уменьшение остатков тары
                                string query9 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                cmd9.ExecuteNonQuery();
                                cmd9.Dispose();

                                if (checkBox10.Checked)
                                {
                                    query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                    SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                    DataTable dt5 = new DataTable();
                                    da5.Fill(dt5);
                                    comboBox6.DataSource = dt5;
                                    comboBox6.DisplayMember = "Nazvanie";
                                    comboBox6.ValueMember = "id_InvoiceTable";
                                    comboBox6.SelectedIndex = -1;
                                    comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                    comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                    comboBox12.DataSource = dt5;
                                    comboBox12.DisplayMember = "id_InvoiceTable";
                                    comboBox12.ValueMember = "id_InvoiceTable";

                                    comboBox2.DataSource = dt5;
                                    comboBox2.DisplayMember = "Amount";
                                    comboBox2.ValueMember = "id_InvoiceTable";
                                    comboBox2.SelectedIndex = -1;

                                    comboBox15.DataSource = dt5;
                                    comboBox15.DisplayMember = "PriceSale";
                                    comboBox15.ValueMember = "id_InvoiceTable";
                                    comboBox15.SelectedIndex = -1;
                                    textBox27.Text = comboBox15.Text;

                                    cmd5.Dispose();
                                }
                                else
                                {
                                    query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
           " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                    SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                    DataTable dt5 = new DataTable();
                                    da5.Fill(dt5);
                                    comboBox6.DataSource = dt5;
                                    comboBox6.DisplayMember = "Nazvanie";
                                    comboBox6.ValueMember = "id_InvoiceTable";
                                    comboBox6.SelectedIndex = -1;
                                    comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                    comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                    comboBox12.DataSource = dt5;
                                    comboBox12.DisplayMember = "id_InvoiceTable";
                                    comboBox12.ValueMember = "id_InvoiceTable";

                                    comboBox2.DataSource = dt5;
                                    comboBox2.DisplayMember = "Amount";
                                    comboBox2.ValueMember = "id_InvoiceTable";
                                    comboBox2.SelectedIndex = -1;

                                    comboBox15.DataSource = dt5;
                                    comboBox15.DisplayMember = "PriceSale";
                                    comboBox15.ValueMember = "id_InvoiceTable";
                                    comboBox15.SelectedIndex = -1;
                                    textBox27.Text = comboBox15.Text;

                                    cmd5.Dispose();
                                }

                                string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                    "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
            "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения],  RealisationOnNomenclatures.Price || '  руб'  AS [По цене], Summa || '  руб' AS Сумма  FROM RealisationOnNomenclatures " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
            " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                DataTable dt3 = new DataTable();
                                da3.Fill(dt3);
                                dataGridView8.DataSource = dt3;
                                dataGridView8.Columns[0].Visible = false;
                                dataGridView8.Columns[1].Visible = false;
                                cmd3.Dispose();
                                dataGridView8.Select();
                                conn.Close();
                                checkBox11.Checked = false;

                                dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                yt_Button11.Enabled = true;
                                dataGridView8.Select();
                            }

                            else
                            {
                                string query12 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd12 = new SQLiteCommand(query12, conn);
                                cmd12.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                cmd12.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                Int64 count2 = (Int64)cmd12.ExecuteScalar();
                                cmd12.Dispose();

                                if (count2 > 0)
                                {
                                    string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                    cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double amount = (Double)cmd10.ExecuteScalar();
                                    textBox10.Text = Convert.ToString(amount);
                                    cmd10.Dispose();

                                    textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + 1);

                                    //пересчет общей суммы
                                    string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                    cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double summa = (Double)cmd11.ExecuteScalar();
                                    textBox12.Text = Convert.ToString(summa);
                                    cmd11.Dispose();

                                    textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + CenaNapit);

                                    string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                    cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 1);
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    //уменьшение количества тар
                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();
                                    //MessageBox.Show("Добавлено " + Convert.ToDouble(kolichestvo * 1.5) + "л " + comboBox6.Text + " и " + kolichestvo + " тары по 1.5л");
                                    conn.Close();
                                    checkBox11.Checked = false;
                                }
                                else
                                {

                                    //добавление напитка
                                    string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                    cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd14.Parameters.AddWithValue("@amount", 1);
                                    cmd14.Parameters.AddWithValue("@price", CenaNapit);
                                    cmd14.Parameters.AddWithValue("@summa", CenaNapit);
                                    cmd14.ExecuteNonQuery();
                                    cmd14.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 1);
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    //уменьшение количества тар
                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();
                                    //MessageBox.Show("Добавлено " + Convert.ToDouble(kolichestvo * 1.5) + "л " + comboBox6.Text + " и " + kolichestvo + " тары по 1.5л");
                                    conn.Close();
                                    checkBox11.Checked = false;

                                }

                                string query15 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                conn.Open();
                                SQLiteCommand cmd15 = new SQLiteCommand(query15, conn);
                                cmd15.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd15.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                Int64 count = (Int64)cmd15.ExecuteScalar();
                                cmd15.Dispose();

                                if (count > 0)
                                {
                                    string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                    cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double amount = (Double)cmd10.ExecuteScalar();
                                    textBox10.Text = Convert.ToString(amount);
                                    cmd10.Dispose();

                                    textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + 1);

                                    //пересчет общей суммы
                                    string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                    cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double summa = (Double)cmd11.ExecuteScalar();
                                    textBox12.Text = Convert.ToString(summa);
                                    cmd11.Dispose();

                                    textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + Convert.ToDouble(comboBox22.Text));

                                    string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                    cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 1);
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    //уменьшение количества тар
                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();

                                    if (checkBox10.Checked)
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                        " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);


                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    else
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                        " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);


                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
                "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По цене], Summa || '  руб' AS Сумма  FROM RealisationOnNomenclatures " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
                " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                    DataTable dt3 = new DataTable();
                                    da3.Fill(dt3);
                                    dataGridView8.DataSource = dt3;
                                    dataGridView8.Columns[0].Visible = false;
                                    dataGridView8.Columns[1].Visible = false;
                                    cmd3.Dispose();
                                    dataGridView8.Select();
                                    conn.Close();
                                    checkBox11.Checked = false;

                                    dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    yt_Button11.Enabled = true;
                                    dataGridView8.Select();
                                }
                                else
                                {

                                    //добавление тар
                                    string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                    cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd14.Parameters.AddWithValue("@amount", 1);
                                    cmd14.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                    cmd14.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text));
                                    cmd14.ExecuteNonQuery();
                                    cmd14.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 1);
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    //уменьшение количества тар
                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();

                                    if (checkBox10.Checked)
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                        " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);

                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    else
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                        " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);

                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
                "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По цене], Summa || '  руб' AS Сумма  FROM RealisationOnNomenclatures " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
                " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                    DataTable dt3 = new DataTable();
                                    da3.Fill(dt3);
                                    dataGridView8.DataSource = dt3;
                                    dataGridView8.Columns[0].Visible = false;
                                    dataGridView8.Columns[1].Visible = false;
                                    cmd3.Dispose();
                                    dataGridView8.Select();
                                    conn.Close();
                                    checkBox11.Checked = false;

                                    dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    yt_Button11.Enabled = true;
                                    dataGridView8.Select();
                                }
                            }
                        }
                    }
                }
            }
        }


        private void yt_Button6_Click_1(object sender, EventArgs e)
        {
            kolichestvo = 0;
            if (dat == 0)
                dateTimePicker5.Value = DateTime.Now;
            string query16 = "SELECT COUNT(id_InvoiceTable) AS count FROM InvoiceTables " +
       " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
           " WHERE Nomenclatures.Name LIKE 'Тара 1,5%' ";
            SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn2.Open();
            SQLiteCommand cmd16 = new SQLiteCommand(query16, conn2);
            Int64 count3 = (Int64)cmd16.ExecuteScalar();
            cmd16.Dispose();
            conn2.Close();

            if (count3 == 0)
            {
                MessageBox.Show("Такой тары нет в номенклатуре!");
            }
            else
            {
                if (textBox27.Text.Trim() == "")
                    MessageBox.Show("Введите цену!");
                else
                {
                    if (checkBox5.Checked)
                    {

                        kolichestvo = Convert.ToDouble(textBox20.Text);
                        if (kolichestvo == 0)
                            return;
                        else
                        {

                            string query7 = "SELECT id_InvoiceTable, Nomenclatures.id_Nomenclature AS id_Nomenclature, Amount, PriceSale FROM InvoiceTables " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            "WHERE Nomenclatures.Name LIKE 'Тара 1,5%'";
                            SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                            conn1.Open();
                            SQLiteCommand cmd7 = new SQLiteCommand(query7, conn1);
                            SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                            DataTable dt7 = new DataTable();
                            da7.Fill(dt7);
                            comboBox23.DataSource = dt7;
                            comboBox23.DisplayMember = "id_InvoiceTable";
                            comboBox23.ValueMember = "id_InvoiceTable";

                            comboBox20.DataSource = dt7;
                            comboBox20.DisplayMember = "id_Nomenclature";
                            comboBox20.ValueMember = "id_InvoiceTable";

                            comboBox21.DataSource = dt7;
                            comboBox21.DisplayMember = "Amount";
                            comboBox21.ValueMember = "id_InvoiceTable";

                            comboBox22.DataSource = dt7;
                            comboBox22.DisplayMember = "PriceSale";
                            comboBox22.ValueMember = "id_InvoiceTable";
                            conn1.Close();


                            if (textBox3.Text == "")
                            {
                                string query = "INSERT INTO Realisations (id_Client, id_Employee, Date) VALUES (@idclient, @idemployee, @date)";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                                cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                                cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(comboBox11.SelectedValue));
                                cmd.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                string query1 = "SELECT MAX(id_Realisation) FROM Realisations";
                                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                                Int64 id = (Int64)cmd1.ExecuteScalar();
                                textBox3.Text = Convert.ToString(id);
                                cmd1.Dispose();

                                //Добавление количества напитка
                                string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                                cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(numericUpDown3.Value));
                                cmd2.Parameters.AddWithValue("@price", CenaNapit);
                                cmd2.Parameters.AddWithValue("@summa", CenaNapit * Convert.ToDouble(numericUpDown3.Value));
                                cmd2.ExecuteNonQuery();
                                cmd2.Dispose();

                                //добавление колиечества тар
                                query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd6 = new SQLiteCommand(query2, conn);
                                cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd6.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(kolichestvo));
                                cmd6.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                cmd6.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text) * Convert.ToDouble(kolichestvo));
                                cmd6.ExecuteNonQuery();
                                cmd6.Dispose();


                                string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                summ = (Double)cmd4.ExecuteScalar();
                                textBox11.Text = Convert.ToString(summ);
                                if (comboBox8.SelectedIndex != 0)
                                {
                                    textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                }
                                cmd4.Dispose();

                                //Уменьшение остатков напитка
                                string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd8 = new SQLiteCommand(query5, conn);
                                cmd8.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd8.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(numericUpDown3.Value));
                                cmd8.ExecuteNonQuery();
                                cmd8.Dispose();

                                ostatok = ostatok - Convert.ToDouble(numericUpDown3.Value);

                                //Уменьшение остатков тары
                                string query9 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - Convert.ToDouble(kolichestvo));
                                cmd9.ExecuteNonQuery();
                                cmd9.Dispose();
                                conn.Close();
                                checkBox11.Checked = false;
                                //MessageBox.Show("Добавлено " + Convert.ToDouble(kolichestvo * 1.5) + "л " + comboBox6.Text + " и "+ kolichestvo+ " тары по 1.5л");
                            }
                            else
                            {
                                string query12 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd12 = new SQLiteCommand(query12, conn);
                                cmd12.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                cmd12.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                Int64 count2 = (Int64)cmd12.ExecuteScalar();
                                cmd12.Dispose();

                                if (count2 > 0)
                                {
                                    string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                    cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double amount = (Double)cmd10.ExecuteScalar();
                                    textBox10.Text = Convert.ToString(amount);
                                    cmd10.Dispose();

                                    textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + Convert.ToDouble(numericUpDown3.Value));

                                    //пересчет общей суммы
                                    string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                    cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double summa = (Double)cmd11.ExecuteScalar();
                                    textBox12.Text = Convert.ToString(summa);
                                    cmd11.Dispose();

                                    textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + CenaNapit * Convert.ToDouble(kolichestvo) * 1.5);

                                    string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                    cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(numericUpDown3.Value));
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();
                                    conn.Close();
                                    checkBox11.Checked = false;
                                    ostatok = ostatok - Convert.ToDouble(numericUpDown3.Value);
                                }
                                else
                                {

                                    //добавление напитка
                                    string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                    cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd14.Parameters.AddWithValue("@amount", Convert.ToDouble(numericUpDown3.Value));
                                    cmd14.Parameters.AddWithValue("@price", CenaNapit);
                                    cmd14.Parameters.AddWithValue("@summa", CenaNapit * Convert.ToDouble(numericUpDown3.Value));
                                    cmd14.ExecuteNonQuery();
                                    cmd14.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(numericUpDown3.Value));
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();
                                    conn.Close();
                                    checkBox11.Checked = false;
                                    ostatok = ostatok - Convert.ToDouble(numericUpDown3.Value);

                                }

                                string query15 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                conn.Open();
                                SQLiteCommand cmd15 = new SQLiteCommand(query15, conn);
                                cmd15.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd15.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                Int64 count = (Int64)cmd15.ExecuteScalar();
                                cmd15.Dispose();

                                if (count > 0)
                                {
                                    string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                    cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double amount = (Double)cmd10.ExecuteScalar();
                                    textBox10.Text = Convert.ToString(amount);
                                    cmd10.Dispose();

                                    textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + kolichestvo);

                                    //пересчет общей суммы
                                    string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                    cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double summa = (Double)cmd11.ExecuteScalar();
                                    textBox12.Text = Convert.ToString(summa);
                                    cmd11.Dispose();

                                    textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + Convert.ToDouble(comboBox22.Text) * Convert.ToDouble(kolichestvo));

                                    string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                    cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества тар
                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - Convert.ToDouble(kolichestvo));
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();
                                    conn.Close();
                                    checkBox11.Checked = false;
                                }
                                else
                                {

                                    //добавление тар
                                    string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                    cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd14.Parameters.AddWithValue("@amount", kolichestvo);
                                    cmd14.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                    cmd14.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text) * kolichestvo);
                                    cmd14.ExecuteNonQuery();
                                    cmd14.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества тар
                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - Convert.ToDouble(kolichestvo));
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();
                                    conn.Close();
                                    checkBox11.Checked = false;
                                }
                            }

                        }

                    }
                    else
                    {
                        if (comboBox6.Text.Trim() == "" | comboBox2.Text.Trim() == "" | comboBox15.Text.Trim() == "" | comboBox6.Text.Trim() == "-")
                            MessageBox.Show("Выберите напиток!");
                        else
                        {
                            CenaNapit = Convert.ToDouble(textBox27.Text);
                            string query7 = "SELECT id_InvoiceTable, Nomenclatures.id_Nomenclature AS id_Nomenclature, Amount, PriceSale FROM InvoiceTables " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            "WHERE Nomenclatures.Name LIKE 'Тара 1,5%'";
                            SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                            conn1.Open();
                            SQLiteCommand cmd7 = new SQLiteCommand(query7, conn1);
                            SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                            DataTable dt7 = new DataTable();
                            da7.Fill(dt7);
                            comboBox23.DataSource = dt7;
                            comboBox23.DisplayMember = "id_InvoiceTable";
                            comboBox23.ValueMember = "id_InvoiceTable";

                            comboBox20.DataSource = dt7;
                            comboBox20.DisplayMember = "id_Nomenclature";
                            comboBox20.ValueMember = "id_InvoiceTable";

                            comboBox21.DataSource = dt7;
                            comboBox21.DisplayMember = "Amount";
                            comboBox21.ValueMember = "id_InvoiceTable";

                            comboBox22.DataSource = dt7;
                            comboBox22.DisplayMember = "PriceSale";
                            comboBox22.ValueMember = "id_InvoiceTable";
                            conn1.Close();

                            if (textBox3.Text == "")
                            {
                                string query = "INSERT INTO Realisations (id_Client, id_Employee, Date) VALUES (@idclient, @idemployee, @date)";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                                cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                                cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(comboBox11.SelectedValue));
                                cmd.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                string query1 = "SELECT MAX(id_Realisation) FROM Realisations";
                                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                                Int64 id = (Int64)cmd1.ExecuteScalar();
                                textBox3.Text = Convert.ToString(id);
                                cmd1.Dispose();

                                string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                                cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(1.5));
                                cmd2.Parameters.AddWithValue("@price", CenaNapit);
                                cmd2.Parameters.AddWithValue("@summa", CenaNapit * 1.5);
                                cmd2.ExecuteNonQuery();
                                cmd2.Dispose();


                                query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd6 = new SQLiteCommand(query2, conn);
                                cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd6.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd6.Parameters.AddWithValue("@amount", 1);
                                cmd6.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                cmd6.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text));
                                cmd6.ExecuteNonQuery();
                                cmd6.Dispose();


                                string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                summ = (Double)cmd4.ExecuteScalar();
                                textBox11.Text = Convert.ToString(summ);
                                if (comboBox8.SelectedIndex != 0)
                                {
                                    textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                }
                                cmd4.Dispose();

                                //Уменьшение остатков напитка
                                string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd8 = new SQLiteCommand(query5, conn);
                                cmd8.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd8.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 1.5);
                                cmd8.ExecuteNonQuery();
                                cmd8.Dispose();

                                //Уменьшение остатков тары
                                string query9 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                cmd9.ExecuteNonQuery();
                                cmd9.Dispose();

                                if (checkBox10.Checked)
                                {
                                    query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                    SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                    DataTable dt5 = new DataTable();
                                    da5.Fill(dt5);
                                    comboBox6.DataSource = dt5;
                                    comboBox6.DisplayMember = "Nazvanie";
                                    comboBox6.ValueMember = "id_InvoiceTable";
                                    comboBox6.SelectedIndex = -1;
                                    comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                    comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                    comboBox12.DataSource = dt5;
                                    comboBox12.DisplayMember = "id_InvoiceTable";
                                    comboBox12.ValueMember = "id_InvoiceTable";

                                    comboBox2.DataSource = dt5;
                                    comboBox2.DisplayMember = "Amount";
                                    comboBox2.ValueMember = "id_InvoiceTable";
                                    comboBox2.SelectedIndex = -1;

                                    comboBox15.DataSource = dt5;
                                    comboBox15.DisplayMember = "PriceSale";
                                    comboBox15.ValueMember = "id_InvoiceTable";
                                    comboBox15.SelectedIndex = -1;
                                    textBox27.Text = comboBox15.Text;
                                    cmd5.Dispose();
                                }
                                else
                                {
                                    query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                    SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                    DataTable dt5 = new DataTable();
                                    da5.Fill(dt5);
                                    comboBox6.DataSource = dt5;
                                    comboBox6.DisplayMember = "Nazvanie";
                                    comboBox6.ValueMember = "id_InvoiceTable";
                                    comboBox6.SelectedIndex = -1;
                                    comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                    comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                    comboBox12.DataSource = dt5;
                                    comboBox12.DisplayMember = "id_InvoiceTable";
                                    comboBox12.ValueMember = "id_InvoiceTable";

                                    comboBox2.DataSource = dt5;
                                    comboBox2.DisplayMember = "Amount";
                                    comboBox2.ValueMember = "id_InvoiceTable";
                                    comboBox2.SelectedIndex = -1;

                                    comboBox15.DataSource = dt5;
                                    comboBox15.DisplayMember = "PriceSale";
                                    comboBox15.ValueMember = "id_InvoiceTable";
                                    comboBox15.SelectedIndex = -1;
                                    textBox27.Text = comboBox15.Text;
                                    cmd5.Dispose();
                                }

                                string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                    "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
            "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По Цене], Summa || '  руб'  AS Сумма  FROM RealisationOnNomenclatures " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
            " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                DataTable dt3 = new DataTable();
                                da3.Fill(dt3);
                                dataGridView8.DataSource = dt3;
                                dataGridView8.Columns[0].Visible = false;
                                dataGridView8.Columns[1].Visible = false;
                                cmd3.Dispose();
                                dataGridView8.Select();
                                conn.Close();
                                checkBox11.Checked = false;

                                dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                yt_Button11.Enabled = true;
                                dataGridView8.Select();
                            }

                            else
                            {
                                string query12 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd12 = new SQLiteCommand(query12, conn);
                                cmd12.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                cmd12.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                Int64 count2 = (Int64)cmd12.ExecuteScalar();
                                cmd12.Dispose();

                                if (count2 > 0)
                                {
                                    string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                    cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double amount = (Double)cmd10.ExecuteScalar();
                                    textBox10.Text = Convert.ToString(amount);
                                    cmd10.Dispose();

                                    textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + 1.5);

                                    //пересчет общей суммы
                                    string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                    cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double summa = (Double)cmd11.ExecuteScalar();
                                    textBox12.Text = Convert.ToString(summa);
                                    cmd11.Dispose();

                                    textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + CenaNapit * 1.5);

                                    string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                    cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 1.5);
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    //уменьшение количества тар
                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();
                                    //MessageBox.Show("Добавлено " + Convert.ToDouble(kolichestvo * 1.5) + "л " + comboBox6.Text + " и " + kolichestvo + " тары по 1.5л");
                                    conn.Close();
                                    checkBox11.Checked = false;
                                }
                                else
                                {

                                    //добавление напитка
                                    string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                    cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.SelectedValue));
                                    cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd14.Parameters.AddWithValue("@amount", 1.5);
                                    cmd14.Parameters.AddWithValue("@price", CenaNapit);
                                    cmd14.Parameters.AddWithValue("@summa", CenaNapit * 1.5);
                                    cmd14.ExecuteNonQuery();
                                    cmd14.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 1.5);
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    //уменьшение количества тар
                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();
                                    //MessageBox.Show("Добавлено " + Convert.ToDouble(kolichestvo * 1.5) + "л " + comboBox6.Text + " и " + kolichestvo + " тары по 1.5л");
                                    conn.Close();
                                    checkBox11.Checked = false;

                                }

                                string query15 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                conn.Open();
                                SQLiteCommand cmd15 = new SQLiteCommand(query15, conn);
                                cmd15.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd15.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                Int64 count = (Int64)cmd15.ExecuteScalar();
                                cmd15.Dispose();

                                if (count > 0)
                                {
                                    string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                    cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double amount = (Double)cmd10.ExecuteScalar();
                                    textBox10.Text = Convert.ToString(amount);
                                    cmd10.Dispose();

                                    textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + 1);

                                    //пересчет общей суммы
                                    string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                    cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double summa = (Double)cmd11.ExecuteScalar();
                                    textBox12.Text = Convert.ToString(summa);
                                    cmd11.Dispose();

                                    textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + Convert.ToDouble(comboBox22.Text));

                                    string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                    cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 1.5);
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    //уменьшение количества тар
                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();

                                    if (checkBox10.Checked)
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                        " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);


                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    else
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                        " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);


                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
                "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По Цене], Summa || '  руб'  AS Сумма  FROM RealisationOnNomenclatures " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
                " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                    DataTable dt3 = new DataTable();
                                    da3.Fill(dt3);
                                    dataGridView8.DataSource = dt3;
                                    dataGridView8.Columns[0].Visible = false;
                                    dataGridView8.Columns[1].Visible = false;
                                    cmd3.Dispose();
                                    dataGridView8.Select();
                                    conn.Close();
                                    checkBox11.Checked = false;

                                    dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    yt_Button11.Enabled = true;
                                    dataGridView8.Select();
                                }
                                else
                                {

                                    //добавление тар
                                    string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                    cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd14.Parameters.AddWithValue("@amount", 1);
                                    cmd14.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                    cmd14.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text));
                                    cmd14.ExecuteNonQuery();
                                    cmd14.Dispose();

                                    //скидка клиента
                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    //уменьшение количества напитка
                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 1.5);
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    //уменьшение количества тар
                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();

                                    if (checkBox10.Checked)
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                        " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);

                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    else
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                        " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                        " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                        " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);

                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
                "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По Цене], Summa || '  руб' AS Сумма  FROM RealisationOnNomenclatures " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
                " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                    DataTable dt3 = new DataTable();
                                    da3.Fill(dt3);
                                    dataGridView8.DataSource = dt3;
                                    dataGridView8.Columns[0].Visible = false;
                                    dataGridView8.Columns[1].Visible = false;
                                    cmd3.Dispose();
                                    dataGridView8.Select();
                                    conn.Close();
                                    checkBox11.Checked = false;

                                    dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    yt_Button11.Enabled = true;
                                    dataGridView8.Select();
                                }
                            }
                        }
                    }
                }
            }
        }


        private void yt_Button7_Click(object sender, EventArgs e)
        {
            kolichestvo = 0;
            if (dat == 0)
                dateTimePicker5.Value = DateTime.Now;
            string query16 = "SELECT COUNT(id_InvoiceTable) AS count FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" WHERE Nomenclatures.Name LIKE 'Тара 2%' ";
            SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn2.Open();
            SQLiteCommand cmd16 = new SQLiteCommand(query16, conn2);
            Int64 count3 = (Int64)cmd16.ExecuteScalar();
            cmd16.Dispose();
            conn2.Close();
            if (count3 == 0)
            {
                MessageBox.Show("Такой тары нет в номенклатуре!");
            }
            else
            {
                if (textBox27.Text.Trim() == "")
                    MessageBox.Show("Введите цену!");
                else
                {
                    if (checkBox5.Checked)
                    {
                        kolichestvo = Convert.ToDouble(numericUpDown4.Value / 2);
                        kolichestvo = Math.Truncate(kolichestvo);

                        if (kolichestvo == 0)
                            return;
                        else
                        {
                            string query7 = "SELECT id_InvoiceTable, Nomenclatures.id_Nomenclature AS id_Nomenclature, Amount, PriceSale FROM InvoiceTables " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            "WHERE Nomenclatures.Name LIKE 'Тара 2%'";
                            SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                            conn1.Open();
                            SQLiteCommand cmd7 = new SQLiteCommand(query7, conn1);
                            SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                            DataTable dt7 = new DataTable();
                            da7.Fill(dt7);
                            comboBox23.DataSource = dt7;
                            comboBox23.DisplayMember = "id_InvoiceTable";
                            comboBox23.ValueMember = "id_InvoiceTable";

                            comboBox20.DataSource = dt7;
                            comboBox20.DisplayMember = "id_Nomenclature";
                            comboBox20.ValueMember = "id_InvoiceTable";

                            comboBox21.DataSource = dt7;
                            comboBox21.DisplayMember = "Amount";
                            comboBox21.ValueMember = "id_InvoiceTable";

                            comboBox22.DataSource = dt7;
                            comboBox22.DisplayMember = "PriceSale";
                            comboBox22.ValueMember = "id_InvoiceTable";
                            conn1.Close();

                            if (textBox3.Text == "")
                            {
                                string query = "INSERT INTO Realisations (id_Client, id_Employee, Date) VALUES (@idclient, @idemployee, @date)";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                                cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                                cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(comboBox11.SelectedValue));
                                cmd.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                string query1 = "SELECT MAX(id_Realisation) FROM Realisations";
                                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                                Int64 id = (Int64)cmd1.ExecuteScalar();
                                textBox3.Text = Convert.ToString(id);
                                cmd1.Dispose();

                                string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                                cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(kolichestvo * 2));
                                cmd2.Parameters.AddWithValue("@price", CenaNapit);
                                cmd2.Parameters.AddWithValue("@summa", CenaNapit * Convert.ToDouble(kolichestvo * 2));
                                cmd2.ExecuteNonQuery();
                                cmd2.Dispose();


                                query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd6 = new SQLiteCommand(query2, conn);
                                cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd6.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd6.Parameters.AddWithValue("@amount", kolichestvo);
                                cmd6.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                cmd6.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text) * kolichestvo);
                                cmd6.ExecuteNonQuery();
                                cmd6.Dispose();


                                string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                summ = (Double)cmd4.ExecuteScalar();
                                textBox11.Text = Convert.ToString(summ);
                                if (comboBox8.SelectedIndex != 0)
                                {
                                    textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                }
                                cmd4.Dispose();

                                //Уменьшение остатков напитка
                                string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd8 = new SQLiteCommand(query5, conn);
                                cmd8.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd8.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(kolichestvo * 2));
                                cmd8.ExecuteNonQuery();
                                cmd8.Dispose();

                                ostatok = ostatok - Convert.ToDouble(kolichestvo * 2);

                                //Уменьшение остатков тары
                                string query9 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - kolichestvo);
                                cmd9.ExecuteNonQuery();
                                cmd9.Dispose();
                                // MessageBox.Show("Добавлено " + Convert.ToDouble(textBox16.Text) + "л "+comboBox6.Text);

                            }

                            else
                            {
                                string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                                cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(kolichestvo * 2));
                                cmd2.Parameters.AddWithValue("@price", CenaNapit);
                                cmd2.Parameters.AddWithValue("@summa", CenaNapit * Convert.ToDouble(kolichestvo * 2));
                                cmd2.ExecuteNonQuery();
                                cmd2.Dispose();

                                string query12 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                SQLiteCommand cmd12 = new SQLiteCommand(query12, conn);
                                cmd12.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd12.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                Int64 count = (Int64)cmd12.ExecuteScalar();
                                cmd12.Dispose();

                                if (count > 0)
                                {
                                    string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                    cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double amount = (Double)cmd10.ExecuteScalar();
                                    textBox10.Text = Convert.ToString(amount);
                                    cmd10.Dispose();

                                    textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + kolichestvo);

                                    string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                    cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double summa = (Double)cmd11.ExecuteScalar();
                                    textBox12.Text = Convert.ToString(summa);
                                    cmd11.Dispose();

                                    textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + (Convert.ToDouble(comboBox22.Text) * Convert.ToDouble(kolichestvo)));

                                    string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                    cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(kolichestvo * 2));
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    ostatok = ostatok - Convert.ToDouble(kolichestvo * 2);

                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - kolichestvo);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();
                                    //MessageBox.Show("Добавлено " + Convert.ToDouble(textBox16.Text) + "л " + comboBox6.Text);
                                }
                                else
                                {


                                    query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                    cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd14.Parameters.AddWithValue("@amount", kolichestvo);
                                    cmd14.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                    cmd14.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text) * kolichestvo);
                                    cmd14.ExecuteNonQuery();
                                    cmd14.Dispose();

                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", ostatok - Convert.ToDouble(kolichestvo * 2));
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    ostatok = ostatok - Convert.ToDouble(kolichestvo * 2);

                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - kolichestvo);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();
                                    //MessageBox.Show("Добавлено " + Convert.ToDouble(textBox16.Text) + "л " + comboBox6.Text);
                                }
                            }

                        }
                    }
                    else
                    {
                        if (comboBox6.Text.Trim() == "" | comboBox2.Text.Trim() == "" | comboBox15.Text.Trim() == "" | comboBox6.Text.Trim() == "-")
                            MessageBox.Show("Выберите напиток!");
                        else
                        {
                            CenaNapit = Convert.ToDouble(textBox27.Text);
                            string query7 = "SELECT id_InvoiceTable, Nomenclatures.id_Nomenclature AS id_Nomenclature, Amount, PriceSale FROM InvoiceTables " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            "WHERE Nomenclatures.Name LIKE 'Тара 2%'";
                            SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                            conn1.Open();
                            SQLiteCommand cmd7 = new SQLiteCommand(query7, conn1);
                            SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                            DataTable dt7 = new DataTable();
                            da7.Fill(dt7);
                            comboBox23.DataSource = dt7;
                            comboBox23.DisplayMember = "id_InvoiceTable";
                            comboBox23.ValueMember = "id_InvoiceTable";

                            comboBox20.DataSource = dt7;
                            comboBox20.DisplayMember = "id_Nomenclature";
                            comboBox20.ValueMember = "id_InvoiceTable";

                            comboBox21.DataSource = dt7;
                            comboBox21.DisplayMember = "Amount";
                            comboBox21.ValueMember = "id_InvoiceTable";

                            comboBox22.DataSource = dt7;
                            comboBox22.DisplayMember = "PriceSale";
                            comboBox22.ValueMember = "id_InvoiceTable";
                            conn1.Close();

                            if (textBox3.Text == "")
                            {
                                string query = "INSERT INTO Realisations (id_Client, id_Employee, Date) VALUES (@idclient, @idemployee, @date)";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                                cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                                cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(comboBox11.SelectedValue));
                                cmd.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                string query1 = "SELECT MAX(id_Realisation) FROM Realisations";
                                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                                Int64 id = (Int64)cmd1.ExecuteScalar();
                                textBox3.Text = Convert.ToString(id);
                                cmd1.Dispose();

                                string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                                cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(2));
                                cmd2.Parameters.AddWithValue("@price", CenaNapit);
                                cmd2.Parameters.AddWithValue("@summa", CenaNapit * 2);
                                cmd2.ExecuteNonQuery();
                                cmd2.Dispose();


                                query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteCommand cmd6 = new SQLiteCommand(query2, conn);
                                cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd6.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd6.Parameters.AddWithValue("@amount", 1);
                                cmd6.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                cmd6.Parameters.AddWithValue("@summa", Convert.ToInt32(comboBox22.Text));
                                cmd6.ExecuteNonQuery();
                                cmd6.Dispose();


                                string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                summ = (Double)cmd4.ExecuteScalar();
                                textBox11.Text = Convert.ToString(summ);
                                if (comboBox8.SelectedIndex != 0)
                                {
                                    textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                }
                                cmd4.Dispose();

                                //Уменьшение остатков напитка
                                string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd8 = new SQLiteCommand(query5, conn);
                                cmd8.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd8.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 2);
                                cmd8.ExecuteNonQuery();
                                cmd8.Dispose();

                                //Уменьшение остатков тары
                                string query9 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                cmd9.ExecuteNonQuery();
                                cmd9.Dispose();

                                if (checkBox10.Checked)
                                {
                                    query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                    SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                    DataTable dt5 = new DataTable();
                                    da5.Fill(dt5);
                                    comboBox6.DataSource = dt5;
                                    comboBox6.DisplayMember = "Nazvanie";
                                    comboBox6.ValueMember = "id_InvoiceTable";
                                    comboBox6.SelectedIndex = -1;
                                    comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                    comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                    comboBox12.DataSource = dt5;
                                    comboBox12.DisplayMember = "id_InvoiceTable";
                                    comboBox12.ValueMember = "id_InvoiceTable";

                                    comboBox2.DataSource = dt5;
                                    comboBox2.DisplayMember = "Amount";
                                    comboBox2.ValueMember = "id_InvoiceTable";
                                    comboBox2.SelectedIndex = -1;

                                    comboBox15.DataSource = dt5;
                                    comboBox15.DisplayMember = "PriceSale";
                                    comboBox15.ValueMember = "id_InvoiceTable";
                                    comboBox15.SelectedIndex = -1;
                                    textBox27.Text = comboBox15.Text;
                                    cmd5.Dispose();
                                }
                                else
                                {
                                    query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                    SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                    DataTable dt5 = new DataTable();
                                    da5.Fill(dt5);
                                    comboBox6.DataSource = dt5;
                                    comboBox6.DisplayMember = "Nazvanie";
                                    comboBox6.ValueMember = "id_InvoiceTable";
                                    comboBox6.SelectedIndex = -1;
                                    comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                    comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                    comboBox12.DataSource = dt5;
                                    comboBox12.DisplayMember = "id_InvoiceTable";
                                    comboBox12.ValueMember = "id_InvoiceTable";

                                    comboBox2.DataSource = dt5;
                                    comboBox2.DisplayMember = "Amount";
                                    comboBox2.ValueMember = "id_InvoiceTable";
                                    comboBox2.SelectedIndex = -1;

                                    comboBox15.DataSource = dt5;
                                    comboBox15.DisplayMember = "PriceSale";
                                    comboBox15.ValueMember = "id_InvoiceTable";
                                    comboBox15.SelectedIndex = -1;
                                    textBox27.Text = comboBox15.Text;
                                    cmd5.Dispose();
                                }
                                string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                    "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
            "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По Цене], Summa || '  руб' AS Сумма  FROM RealisationOnNomenclatures " +
            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
            " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                DataTable dt3 = new DataTable();
                                da3.Fill(dt3);
                                dataGridView8.DataSource = dt3;
                                dataGridView8.Columns[0].Visible = false;
                                dataGridView8.Columns[1].Visible = false;
                                cmd3.Dispose();
                                dataGridView8.Select();
                                conn.Close();
                                checkBox11.Checked = false;

                                dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                yt_Button11.Enabled = true;
                                dataGridView8.Select();
                            }

                            else
                            {
                                string query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                                conn.Open();
                                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn);
                                cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(2));
                                cmd2.Parameters.AddWithValue("@price", CenaNapit);
                                cmd2.Parameters.AddWithValue("@summa", CenaNapit * 2);
                                cmd2.ExecuteNonQuery();
                                cmd2.Dispose();

                                string query12 = "SELECT COUNT(id_RealisationOnNomenclature) AS count FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                SQLiteCommand cmd12 = new SQLiteCommand(query12, conn);
                                cmd12.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                cmd12.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                Int64 count = (Int64)cmd12.ExecuteScalar();
                                cmd12.Dispose();

                                if (count > 0)
                                {
                                    string query10 = "SELECT Amount AS Amount FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd10 = new SQLiteCommand(query10, conn);
                                    cmd10.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd10.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double amount = (Double)cmd10.ExecuteScalar();
                                    textBox10.Text = Convert.ToString(amount);
                                    cmd10.Dispose();

                                    textBox10.Text = Convert.ToString(Convert.ToDouble(textBox10.Text) + 1);

                                    string query11 = "SELECT Summa AS Summa FROM RealisationOnNomenclatures WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd11 = new SQLiteCommand(query11, conn);
                                    cmd11.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd11.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    Double summa = (Double)cmd11.ExecuteScalar();
                                    textBox12.Text = Convert.ToString(summa);
                                    cmd11.Dispose();

                                    textBox12.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + Convert.ToDouble(comboBox22.Text));

                                    string query9 = "UPDATE RealisationOnNomenclatures SET Amount = @amount, Summa = @summa WHERE id_InvoiceTable = @idinvoicetable AND id_Realisation=@idrealisation";
                                    SQLiteCommand cmd9 = new SQLiteCommand(query9, conn);
                                    cmd9.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd9.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd9.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox10.Text));
                                    cmd9.Parameters.AddWithValue("@summa", Convert.ToDouble(textBox12.Text));
                                    cmd9.ExecuteNonQuery();
                                    cmd9.Dispose();

                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 2);
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();

                                    if (checkBox10.Checked)
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                    " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                    " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                    " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);


                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;
                                        cmd5.Dispose();
                                    }
                                    else
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                    " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                    " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                    " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);


                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
                "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По Цене], Summa || '  руб' AS Сумма  FROM RealisationOnNomenclatures " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
                " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                    DataTable dt3 = new DataTable();
                                    da3.Fill(dt3);
                                    dataGridView8.DataSource = dt3;
                                    dataGridView8.Columns[0].Visible = false;
                                    dataGridView8.Columns[1].Visible = false;
                                    cmd3.Dispose();
                                    dataGridView8.Select();
                                    conn.Close();
                                    checkBox11.Checked = false;

                                    dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    yt_Button11.Enabled = true;
                                    dataGridView8.Select();
                                }
                                else
                                {


                                    query2 = "INSERT INTO RealisationOnNomenclatures (id_InvoiceTable, id_Realisation, Amount, Price, Summa) " +
                    "VALUES (@idinvoicetable, @idrealisation, @amount, @price, @summa)";
                                    SQLiteCommand cmd14 = new SQLiteCommand(query2, conn);
                                    cmd14.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd14.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    cmd14.Parameters.AddWithValue("@amount", 1);
                                    cmd14.Parameters.AddWithValue("@price", Convert.ToDouble(comboBox22.Text));
                                    cmd14.Parameters.AddWithValue("@summa", Convert.ToDouble(comboBox22.Text));
                                    cmd14.ExecuteNonQuery();
                                    cmd14.Dispose();

                                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    summ = (Double)cmd4.ExecuteScalar();
                                    textBox11.Text = Convert.ToString(summ);
                                    if (comboBox8.SelectedIndex != 0)
                                    {
                                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                                    }
                                    cmd4.Dispose();

                                    string query5 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd6 = new SQLiteCommand(query5, conn);
                                    cmd6.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox12.Text));
                                    cmd6.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox2.Text) - 2);
                                    cmd6.ExecuteNonQuery();
                                    cmd6.Dispose();

                                    string query13 = "UPDATE InvoiceTables SET Amount = @amount WHERE id_InvoiceTable = @idinvoicetable";
                                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn);
                                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(comboBox23.SelectedValue));
                                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(comboBox21.Text) - 1);
                                    cmd13.ExecuteNonQuery();
                                    cmd13.Dispose();

                                    if (checkBox10.Checked)
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                    " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                    " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                    " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
            " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);


                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    else
                                    {
                                        string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                    " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                    " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                    " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                                        SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                        DataTable dt5 = new DataTable();
                                        da5.Fill(dt5);


                                        comboBox6.DataSource = dt5;
                                        comboBox6.DisplayMember = "Nazvanie";
                                        comboBox6.ValueMember = "id_InvoiceTable";
                                        comboBox6.SelectedIndex = -1;
                                        comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                        comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                                        comboBox12.DataSource = dt5;
                                        comboBox12.DisplayMember = "id_InvoiceTable";
                                        comboBox12.ValueMember = "id_InvoiceTable";

                                        comboBox2.DataSource = dt5;
                                        comboBox2.DisplayMember = "Amount";
                                        comboBox2.ValueMember = "id_InvoiceTable";
                                        comboBox2.SelectedIndex = -1;

                                        comboBox15.DataSource = dt5;
                                        comboBox15.DisplayMember = "PriceSale";
                                        comboBox15.ValueMember = "id_InvoiceTable";
                                        comboBox15.SelectedIndex = -1;
                                        textBox27.Text = comboBox15.Text;

                                        cmd5.Dispose();
                                    }
                                    string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
                "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По Цене], Summa || '  руб' AS Сумма  FROM RealisationOnNomenclatures " +
                " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
                " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                                    DataTable dt3 = new DataTable();
                                    da3.Fill(dt3);
                                    dataGridView8.DataSource = dt3;
                                    dataGridView8.Columns[0].Visible = false;
                                    dataGridView8.Columns[1].Visible = false;
                                    cmd3.Dispose();
                                    dataGridView8.Select();
                                    conn.Close();
                                    checkBox11.Checked = false;

                                    dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    yt_Button11.Enabled = true;
                                    dataGridView8.Select();
                                }
                            }
                        }
                    }
                }
            }
            }

        private void text_press(object sender, KeyPressEventArgs e)
        {
            TextBox TBox = (TextBox)sender;
            if (!(Char.IsDigit(e.KeyChar)) && !((e.KeyChar == ',') && (TBox.Text.IndexOf(",") == -1) && (TBox.Text.Length != 0)))
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        private void yt_Button12_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                DialogResult result = MessageBox.Show("Отменить продажу?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int rows = dataGridView8.Rows.Count;
                    for (int i = 0; i <= rows; i++)
                    {
                        yt_Button15_Click(sender, e);
                    }

                    textBox3.Clear();
                    comboBox9.SelectedIndex = -1;
                    yt_Button11.Enabled = false;
                    checkBox7.Checked = false;
                    checkBox8.Checked = false;
                    checkBox4.Checked = false;
                    summ = 0;
                }
                else if (result == DialogResult.No)
                {
                    tabControl1.SelectedIndex = 0;
                    return;
                }
            }
        }

        private void dataGridView8_SelectionChanged(object sender, EventArgs e)
        {
            ind = dataGridView8.CurrentRow.Index;
            textBox13.Text = dataGridView8.Rows[ind].Cells[0].Value.ToString();
            textBox14.Text = dataGridView8.Rows[ind].Cells[1].Value.ToString();
            textBox6.Text = dataGridView8.Rows[ind].Cells[4].Value.ToString();
        }

        private void yt_Button10_Click(object sender, EventArgs e)
        {
            if (dataGridView8.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Удалить позицию?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (dataGridView8.Rows.Count > 1)
                    {
                        string query13 = "UPDATE InvoiceTables SET Amount = Amount + @amount " +
                        "WHERE id_InvoiceTable = @idinvoicetable";
                        SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                        conn2.Open();
                        SQLiteCommand cmd13 = new SQLiteCommand(query13, conn2);
                        cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(textBox13.Text));
                        cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox6.Text));
                        cmd13.ExecuteNonQuery();
                        cmd13.Dispose();

                        string query = "DELETE FROM RealisationOnNomenclatures WHERE id_RealisationOnNomenclature = @idrealisationonnomen";
                        SQLiteCommand cmd = new SQLiteCommand(query, conn2);
                        cmd.Parameters.AddWithValue("@idrealisationonnomen", Convert.ToInt32(textBox14.Text));
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (checkBox10.Checked)
                        {
                            query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                            SQLiteCommand cmd5 = new SQLiteCommand(query, conn2);
                            SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                            DataTable dt5 = new DataTable();
                            da5.Fill(dt5);
                            comboBox6.DataSource = dt5;
                            comboBox6.DisplayMember = "Nazvanie";
                            comboBox6.ValueMember = "id_InvoiceTable";
                            comboBox6.SelectedIndex = -1;
                            comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                            comboBox12.DataSource = dt5;
                            comboBox12.DisplayMember = "id_InvoiceTable";
                            comboBox12.ValueMember = "id_InvoiceTable";

                            comboBox2.DataSource = dt5;
                            comboBox2.DisplayMember = "Amount";
                            comboBox2.ValueMember = "id_InvoiceTable";
                            comboBox2.SelectedIndex = -1;

                            comboBox15.DataSource = dt5;
                            comboBox15.DisplayMember = "PriceSale";
                            comboBox15.ValueMember = "id_InvoiceTable";
                            comboBox15.SelectedIndex = -1;
                            textBox27.Text = comboBox15.Text;

                            cmd5.Dispose();
                        }
                        else
                        {
                            query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                            SQLiteCommand cmd5 = new SQLiteCommand(query, conn2);
                            SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                            DataTable dt5 = new DataTable();
                            da5.Fill(dt5);
                            comboBox6.DataSource = dt5;
                            comboBox6.DisplayMember = "Nazvanie";
                            comboBox6.ValueMember = "id_InvoiceTable";
                            comboBox6.SelectedIndex = -1;
                            comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                            comboBox12.DataSource = dt5;
                            comboBox12.DisplayMember = "id_InvoiceTable";
                            comboBox12.ValueMember = "id_InvoiceTable";

                            comboBox2.DataSource = dt5;
                            comboBox2.DisplayMember = "Amount";
                            comboBox2.ValueMember = "id_InvoiceTable";
                            comboBox2.SelectedIndex = -1;

                            comboBox15.DataSource = dt5;
                            comboBox15.DisplayMember = "PriceSale";
                            comboBox15.ValueMember = "id_InvoiceTable";
                            comboBox15.SelectedIndex = -1;
                            textBox27.Text = comboBox15.Text;

                            cmd5.Dispose();
                        }
                        if (checkBox10.Checked)
                        {
                            query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' AND Amount > 0 OR id_InvoiceTable=1 ORDER BY Nazvanie";
                            SQLiteCommand cmd6 = new SQLiteCommand(query, conn2);
                            SQLiteDataAdapter da6 = new SQLiteDataAdapter(cmd6);
                            DataTable dt6 = new DataTable();
                            da6.Fill(dt6);
                            comboBox7.DataSource = dt6;
                            comboBox7.DisplayMember = "Nazvanie";
                            comboBox7.ValueMember = "id_InvoiceTable";
                            comboBox7.SelectedIndex = -1;
                            comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                            comboBox13.DataSource = dt6;
                            comboBox13.DisplayMember = "id_InvoiceTable";
                            comboBox13.ValueMember = "id_InvoiceTable";

                            comboBox17.DataSource = dt6;
                            comboBox17.DisplayMember = "Amount";
                            comboBox17.ValueMember = "id_InvoiceTable";
                            comboBox17.SelectedIndex = -1;

                            comboBox19.DataSource = dt6;
                            comboBox19.DisplayMember = "EdIzm";
                            comboBox19.ValueMember = "id_InvoiceTable";
                            comboBox19.SelectedIndex = -1;

                            comboBox16.DataSource = dt6;
                            comboBox16.DisplayMember = "PriceSale";
                            comboBox16.ValueMember = "id_InvoiceTable";
                            comboBox16.SelectedIndex = -1;
                            textBox16.Text = comboBox16.Text;

                            cmd6.Dispose();
                        }
                        else
                        {
                            query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' ORDER BY Nazvanie";
                            SQLiteCommand cmd6 = new SQLiteCommand(query, conn2);
                            SQLiteDataAdapter da6 = new SQLiteDataAdapter(cmd6);
                            DataTable dt6 = new DataTable();
                            da6.Fill(dt6);
                            comboBox7.DataSource = dt6;
                            comboBox7.DisplayMember = "Nazvanie";
                            comboBox7.ValueMember = "id_InvoiceTable";
                            comboBox7.SelectedIndex = -1;
                            comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                            comboBox13.DataSource = dt6;
                            comboBox13.DisplayMember = "id_InvoiceTable";
                            comboBox13.ValueMember = "id_InvoiceTable";

                            comboBox17.DataSource = dt6;
                            comboBox17.DisplayMember = "Amount";
                            comboBox17.ValueMember = "id_InvoiceTable";
                            comboBox17.SelectedIndex = -1;

                            comboBox19.DataSource = dt6;
                            comboBox19.DisplayMember = "EdIzm";
                            comboBox19.ValueMember = "id_InvoiceTable";
                            comboBox19.SelectedIndex = -1;

                            comboBox16.DataSource = dt6;
                            comboBox16.DisplayMember = "PriceSale";
                            comboBox16.ValueMember = "id_InvoiceTable";
                            comboBox16.SelectedIndex = -1;
                            textBox16.Text = comboBox16.Text;

                            cmd6.Dispose();
                        }

                        string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                            "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
    "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По Цене], Summa || '  руб' AS Сумма FROM RealisationOnNomenclatures " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
    " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                        SQLiteCommand cmd3 = new SQLiteCommand(query3, conn2);
                        cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                        SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                        DataTable dt3 = new DataTable();
                        da3.Fill(dt3);
                        dataGridView8.DataSource = dt3;
                        dataGridView8.Columns[0].Visible = false;
                        dataGridView8.Columns[1].Visible = false;
                        cmd3.Dispose();
                        dataGridView8.Select();


                        string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                        SQLiteCommand cmd4 = new SQLiteCommand(query4, conn2);
                        cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                        summ = (Double)cmd4.ExecuteScalar();
                        textBox11.Text = Convert.ToString(summ);
                        if (comboBox8.SelectedIndex != 0)
                        {
                            textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                        }
                        cmd4.Dispose();
                        conn2.Close();
                        checkBox11.Checked = false;

                        dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    }
                    else if (dataGridView8.Rows.Count == 1)
                    {
                        string query13 = "UPDATE InvoiceTables SET Amount = Amount + @amount " +
    "WHERE id_InvoiceTable = @idinvoicetable";
                        SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                        conn2.Open();
                        SQLiteCommand cmd13 = new SQLiteCommand(query13, conn2);
                        cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(textBox13.Text));
                        cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox6.Text));
                        cmd13.ExecuteNonQuery();
                        cmd13.Dispose();

                        string query = "DELETE FROM RealisationOnNomenclatures WHERE id_RealisationOnNomenclature = @idrealisationonnomen";
                        SQLiteCommand cmd = new SQLiteCommand(query, conn2);
                        cmd.Parameters.AddWithValue("@idrealisationonnomen", Convert.ToInt32(textBox14.Text));
                        cmd.ExecuteNonQuery();
                        SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        cmd.Dispose();

                        string query1 = "DELETE FROM Realisations WHERE id_Realisation = @idrealisation";

                        SQLiteCommand cmd1 = new SQLiteCommand(query1, conn2);
                        cmd1.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                        cmd1.ExecuteNonQuery();
                        SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);
                        cmd.Dispose();

                        if (checkBox10.Checked)
                        {
                            query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                            SQLiteCommand cmd5 = new SQLiteCommand(query, conn2);
                            SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                            DataTable dt5 = new DataTable();
                            da5.Fill(dt5);
                            comboBox6.DataSource = dt5;
                            comboBox6.DisplayMember = "Nazvanie";
                            comboBox6.ValueMember = "id_InvoiceTable";
                            comboBox6.SelectedIndex = -1;
                            comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                            comboBox12.DataSource = dt5;
                            comboBox12.DisplayMember = "id_InvoiceTable";
                            comboBox12.ValueMember = "id_InvoiceTable";

                            comboBox2.DataSource = dt5;
                            comboBox2.DisplayMember = "Amount";
                            comboBox2.ValueMember = "id_InvoiceTable";
                            comboBox2.SelectedIndex = -1;

                            comboBox15.DataSource = dt5;
                            comboBox15.DisplayMember = "PriceSale";
                            comboBox15.ValueMember = "id_InvoiceTable";
                            comboBox15.SelectedIndex = -1;
                            textBox27.Text = comboBox15.Text;

                            cmd5.Dispose();
                        }
                        else
                        {
                            query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                            SQLiteCommand cmd5 = new SQLiteCommand(query, conn2);
                            SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                            DataTable dt5 = new DataTable();
                            da5.Fill(dt5);
                            comboBox6.DataSource = dt5;
                            comboBox6.DisplayMember = "Nazvanie";
                            comboBox6.ValueMember = "id_InvoiceTable";
                            comboBox6.SelectedIndex = -1;
                            comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                            comboBox12.DataSource = dt5;
                            comboBox12.DisplayMember = "id_InvoiceTable";
                            comboBox12.ValueMember = "id_InvoiceTable";

                            comboBox2.DataSource = dt5;
                            comboBox2.DisplayMember = "Amount";
                            comboBox2.ValueMember = "id_InvoiceTable";
                            comboBox2.SelectedIndex = -1;

                            comboBox15.DataSource = dt5;
                            comboBox15.DisplayMember = "PriceSale";
                            comboBox15.ValueMember = "id_InvoiceTable";
                            comboBox15.SelectedIndex = -1;
                            textBox27.Text = comboBox15.Text;

                            cmd5.Dispose();
                        }
                        if (checkBox10.Checked)
                        {
                            query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' AND Amount > 0 OR id_InvoiceTable=1 ORDER BY Nazvanie";
                            SQLiteCommand cmd6 = new SQLiteCommand(query, conn2);
                            SQLiteDataAdapter da6 = new SQLiteDataAdapter(cmd6);
                            DataTable dt6 = new DataTable();
                            da6.Fill(dt6);
                            comboBox7.DataSource = dt6;
                            comboBox7.DisplayMember = "Nazvanie";
                            comboBox7.ValueMember = "id_InvoiceTable";
                            comboBox7.SelectedIndex = -1;
                            comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                            comboBox13.DataSource = dt6;
                            comboBox13.DisplayMember = "id_InvoiceTable";
                            comboBox13.ValueMember = "id_InvoiceTable";

                            comboBox17.DataSource = dt6;
                            comboBox17.DisplayMember = "Amount";
                            comboBox17.ValueMember = "id_InvoiceTable";
                            comboBox17.SelectedIndex = -1;

                            comboBox19.DataSource = dt6;
                            comboBox19.DisplayMember = "EdIzm";
                            comboBox19.ValueMember = "id_InvoiceTable";
                            comboBox19.SelectedIndex = -1;

                            comboBox16.DataSource = dt6;
                            comboBox16.DisplayMember = "PriceSale";
                            comboBox16.ValueMember = "id_InvoiceTable";
                            comboBox16.SelectedIndex = -1;
                            textBox16.Text = comboBox16.Text;

                            cmd6.Dispose();
                        }
                        else
                        {
                            query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND ProductGroups.Name <> 'Напитки' ORDER BY Nazvanie";
                            SQLiteCommand cmd6 = new SQLiteCommand(query, conn2);
                            SQLiteDataAdapter da6 = new SQLiteDataAdapter(cmd6);
                            DataTable dt6 = new DataTable();
                            da6.Fill(dt6);
                            comboBox7.DataSource = dt6;
                            comboBox7.DisplayMember = "Nazvanie";
                            comboBox7.ValueMember = "id_InvoiceTable";
                            comboBox7.SelectedIndex = -1;
                            comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

                            comboBox13.DataSource = dt6;
                            comboBox13.DisplayMember = "id_InvoiceTable";
                            comboBox13.ValueMember = "id_InvoiceTable";

                            comboBox17.DataSource = dt6;
                            comboBox17.DisplayMember = "Amount";
                            comboBox17.ValueMember = "id_InvoiceTable";
                            comboBox17.SelectedIndex = -1;

                            comboBox19.DataSource = dt6;
                            comboBox19.DisplayMember = "EdIzm";
                            comboBox19.ValueMember = "id_InvoiceTable";
                            comboBox19.SelectedIndex = -1;

                            comboBox16.DataSource = dt6;
                            comboBox16.DisplayMember = "PriceSale";
                            comboBox16.ValueMember = "id_InvoiceTable";
                            comboBox16.SelectedIndex = -1;
                            textBox16.Text = comboBox16.Text;

                            cmd6.Dispose();
                        }

                        string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                            "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
    "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По Цене], Summa || '  руб' AS Сумма FROM RealisationOnNomenclatures " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
    " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                        SQLiteCommand cmd3 = new SQLiteCommand(query3, conn2);
                        cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                        SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                        DataTable dt3 = new DataTable();
                        da3.Fill(dt3);
                        dataGridView8.DataSource = dt3;
                        dataGridView8.Columns[0].Visible = false;
                        dataGridView8.Columns[1].Visible = false;
                        cmd3.Dispose();
                        dataGridView8.Select();


                        textBox11.Text = Convert.ToString(0);
                        conn2.Close();
                        checkBox11.Checked = false;

                        dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        textBox3.Clear();
                        yt_Button11.Enabled = false;
                    }
                }


                else if (result == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void yt_Button9_Click(object sender, EventArgs e)
        {
            if (dataGridView8.Rows.Count > 0)
            {
                yt_Button12_Click(sender, e);
            }
            else
            {
                FormSales formsales = new FormSales(k);
                formsales.Owner = this;
                formsales.Show();
            }
            checkBox7.Checked = false;
            checkBox8.Checked = false;
        }

        private void dataGridView8_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dataGridView8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                yt_Button10_Click(sender, e);
            }
        }

        private void yt_Button4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button4_Click_1(sender, e);
            }
        }

        private void yt_Button5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button5_Click_1(sender, e);
            }
        }

        private void yt_Button6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button6_Click_1(sender, e);
            }
        }

        private void yt_Button7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button7_Click(sender, e);
            }
        }

        private void yt_Button8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button8_Click(sender, e);
            }
        }

        private void yt_Button11_Click(object sender, EventArgs e)
        {
            if (dat == 0)
                dateTimePicker5.Value = DateTime.Now;
            if (dataGridView8.Rows.Count > 0)
                {
                    if ((comboBox8.Text == "" || comboBox8.SelectedIndex < 1) && checkBox4.Checked == true)
                        MessageBox.Show("Выберите клиента со скидкой!");
                    else if (textBox26.Enabled)
                        MessageBox.Show("Подтвердите ввод скидки!");
                    else if (checkBox7.Checked == false && checkBox8.Checked == true)
                    {
                        string query = "INSERT INTO Payments (id_Realisation, Date, Type, Sum) " +
            "VALUES (@idrealisation, @date, @type, @sum)";
                        SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                        conn.Open();
                        SQLiteCommand cmd2 = new SQLiteCommand(query, conn);
                        cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                        cmd2.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                        cmd2.Parameters.AddWithValue("@type", "Безналичные");
                        cmd2.Parameters.AddWithValue("@sum", Convert.ToDouble(textBox11.Text));
                        SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                        DataTable dt2 = new DataTable();
                        da2.Fill(dt2);
                        cmd2.Dispose();

                    query = "UPDATE Realisations SET id_Client = @idclient " +
                    "WHERE id_Realisation = @idrealisation";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                    cmd.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    conn.Close();

                    dataGridView8.DataSource = null;

                    MessageBox.Show("Продажа успешно сохранена!");
                    checkBox4.Checked = false;
                    checkBox7.Checked = false;
                    checkBox8.Checked = false;
                    textBox11.Text = Convert.ToString(0);
                    textBox3.Clear();
                    summ = 0;
                }
                else if (checkBox7.Checked == true && checkBox8.Checked == false)
                {
                    string query = "INSERT INTO Payments (id_Realisation, Date, Type, Sum) " +
        "VALUES (@idrealisation, @date, @type, @sum)";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd2 = new SQLiteCommand(query, conn);
                    cmd2.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    cmd2.Parameters.AddWithValue("@date", dateTimePicker5.Value);
                    cmd2.Parameters.AddWithValue("@type", "Наличные");
                    cmd2.Parameters.AddWithValue("@sum", Convert.ToDouble(textBox11.Text));
                    SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    cmd2.Dispose();

                    query = "UPDATE Realisations SET id_Client = @idclient " +
                    "WHERE id_Realisation = @idrealisation";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idclient", Convert.ToInt32(comboBox10.SelectedValue));
                    cmd.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    conn.Close();

                    dataGridView8.DataSource = null;

                    MessageBox.Show("Продажа успешно сохранена!");
                    checkBox4.Checked = false;
                    checkBox7.Checked = false;
                    checkBox8.Checked = false;
                    textBox11.Text = Convert.ToString(0);
                    textBox3.Clear();
                    summ = 0;
                }
                else
                        MessageBox.Show("Выберите тип оплаты!");
                }
            dat = 0;
            dateTimePicker5.Value = DateTime.Now;
            }

        private void yt_Button9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button9_Click(sender, e);
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                yt_Button3_Click(sender, e);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteClient_Click(sender, e);
            }
        }

        private void dataGridView6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteNomenclature_Click(sender, e);
            }
        }

        private void dataGridView5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeletePostav_Click(sender, e);
            }
        }

        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSotrud_Click(sender, e);
            }
        }

        private void yt_Button15_Click(object sender, EventArgs e)
        {
                if (dataGridView8.Rows.Count > 1)
                {
                    string query13 = "UPDATE InvoiceTables SET Amount = Amount + @amount " +
                    "WHERE id_InvoiceTable = @idinvoicetable";
                    SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn2.Open();
                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn2);
                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(textBox13.Text));
                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox6.Text));
                    cmd13.ExecuteNonQuery();
                    cmd13.Dispose();

                    string query = "DELETE FROM RealisationOnNomenclatures WHERE id_RealisationOnNomenclature = @idrealisationonnomen";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn2);
                    cmd.Parameters.AddWithValue("@idrealisationonnomen", Convert.ToInt32(textBox14.Text));
                    cmd.ExecuteNonQuery();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmd.Dispose();

                    string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
"  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По Цене], Summa || '  руб' AS Сумма FROM RealisationOnNomenclatures " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
" WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn2);
                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView8.DataSource = dt3;
                    dataGridView8.Columns[0].Visible = false;
                    dataGridView8.Columns[1].Visible = false;
                    cmd3.Dispose();
                    dataGridView8.Select();


                    string query4 = "SELECT SUM(Summa) AS Summa FROM RealisationOnNomenclatures WHERE id_Realisation = @idrealisation";
                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn2);
                    cmd4.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    summ = (Double)cmd4.ExecuteScalar();
                    textBox11.Text = Convert.ToString(summ);
                    if (comboBox8.SelectedIndex != 0)
                    {
                        textBox11.Text = Convert.ToString(Convert.ToDouble(textBox11.Text) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
                    }
                    cmd4.Dispose();
                    conn2.Close();
                checkBox11.Checked = false;

                dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            }
                else if (dataGridView8.Rows.Count == 1)
                {
                    string query13 = "UPDATE InvoiceTables SET Amount = Amount + @amount " +
"WHERE id_InvoiceTable = @idinvoicetable";
                    SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn2.Open();
                    SQLiteCommand cmd13 = new SQLiteCommand(query13, conn2);
                    cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(textBox13.Text));
                    cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox6.Text));
                    cmd13.ExecuteNonQuery();
                    cmd13.Dispose();

                    string query = "DELETE FROM RealisationOnNomenclatures WHERE id_RealisationOnNomenclature = @idrealisationonnomen";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn2);
                    cmd.Parameters.AddWithValue("@idrealisationonnomen", Convert.ToInt32(textBox14.Text));
                    cmd.ExecuteNonQuery();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmd.Dispose();

                    string query1 = "DELETE FROM Realisations WHERE id_Realisation = @idrealisation";

                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn2);
                    cmd1.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    cmd1.ExecuteNonQuery();
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    cmd.Dispose();

                    string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
"  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб'  AS [По Цене], Summa || '  руб' AS Сумма FROM RealisationOnNomenclatures " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
" WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn2);
                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView8.DataSource = dt3;
                    dataGridView8.Columns[0].Visible = false;
                    dataGridView8.Columns[1].Visible = false;
                    cmd3.Dispose();
                    dataGridView8.Select();


                    textBox11.Text = Convert.ToString(0);
                    conn2.Close();
                checkBox11.Checked = false;

                dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                textBox3.Clear();
                }
        }

        private void yt_Button14_Click(object sender, EventArgs e)
        {
            FormSalesSotrud fomrsalessotrud = new FormSalesSotrud();
            fomrsalessotrud.Show();
        }

        private void yt_Button11_KeyDown(object sender, KeyEventArgs e)
        {
            yt_Button11_Click(sender, e);
        }

        private void yt_Button12_KeyDown(object sender, KeyEventArgs e)
        {
            yt_Button12_Click(sender, e);
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (dataGridView8.Rows.Count > 0)
            {
                yt_Button12_Click(sender, e);
            }
        }

        private void yt_Button13_Click(object sender, EventArgs e)
        {
            FormSalesOnMonths formsalesonmoths = new FormSalesOnMonths();
            formsalesonmoths.Show();
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (textBox4.Text == "0")
                textBox4.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblNewUserLogin.Visible = true;
            lblNewUserPass.Visible = true;
            tbNewUserLogin.Visible = true;
            tbNewUserPass.Visible = true;
            ShowPass3.Visible = true;
            ShowPass3.Checked = false;
            OkNewUser.Visible = true;
            CancelNewUser.Visible = true;
        }

        private void CancelNewUser_Click(object sender, EventArgs e)
        {
            lblNewUserLogin.Visible = false;
            lblNewUserPass.Visible = false;
            tbNewUserLogin.Visible = false;
            tbNewUserPass.Visible = false;
            ShowPass3.Visible = false;
            OkNewUser.Visible = false;
            CancelNewUser.Visible = false;
        }

        private void ShowPass3_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPass3.Checked)
            {
                tbNewUserPass.PasswordChar = (char)0;
            }
            else
            {
                tbNewUserPass.PasswordChar = (char)42;
            }
        }





        private void tbNewUserLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
                e.Handled = true;
            else return;
        }

        private void tbNewUserPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
                e.Handled = true;
            else return;
        }

        private void OkNewPass_Click(object sender, EventArgs e)
        {
            if (tbLogin1.Text.Trim() == "" | tbPass1.Text.Trim() == "" | tbLogin2.Text.Trim() == "" | tbPass2.Text.Trim() == "")
            {
                MessageBox.Show("Пустые поля!", "Ошибка");
            }
            else
            {
                string query = "SELECT * FROM Users WHERE Login=@log AND Password=@pass AND id_User=1";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@log", tbLogin1.Text);
                cmd.Parameters.AddWithValue("@pass", tbPass1.Text);
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string query1 = "UPDATE Users SET Login=@login, Password=@password WHERE id_User=1";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("@login", tbLogin2.Text);
                    cmd1.Parameters.AddWithValue("@password", tbPass2.Text);
                    cmd1.ExecuteNonQuery();
                    cmd1.Dispose();
                    cmd.Dispose();
                    reader.Close();
                    conn.Close();
                    MessageBox.Show("Пароль администратора изменен!");
                    tbLogin1.Clear();
                    tbPass1.Clear();
                    tbLogin2.Clear();
                    tbPass2.Clear();
                }
                else
                {
                    MessageBox.Show("Неверный текущий логин/пароль!", "Ошибка");
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D1)
                tabControl1.SelectedIndex = 0;
            if (e.Control && e.KeyCode == Keys.D2)
                tabControl1.SelectedIndex = 1;
            if (e.Control && e.KeyCode == Keys.D3)
                tabControl1.SelectedIndex = 2;
            if (e.Control && e.KeyCode == Keys.D4)
                tabControl1.SelectedIndex = 3;
            if (e.Control && e.KeyCode == Keys.D5)
                tabControl1.SelectedIndex = 4;
            if (e.Control && e.KeyCode == Keys.D6)
                tabControl1.SelectedIndex = 5;
            if (e.Control && e.KeyCode == Keys.D7)
                tabControl1.SelectedIndex = 6;
            if (e.Control && e.KeyCode == Keys.D8)
                tabControl1.SelectedIndex = 7;
            if (e.Control && e.KeyCode == Keys.D9)
                tabControl1.SelectedIndex = 8;
            if (e.Control && e.KeyCode == Keys.D0)
                tabControl1.SelectedIndex = 9;
        }

        private void dateTimePicker5_CloseUp(object sender, EventArgs e)
        {
            dat = 1;
            dateTimePicker5.Value = dateTimePicker5.Value.Date;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
                textBox2.Clear();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
                textBox1.Clear();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            textBox17.Text = Convert.ToString(numericUpDown1.Value + numericUpDown2.Value + numericUpDown3.Value + numericUpDown4.Value);
            if (Convert.ToDouble(numericUpDown1.Value) == 0)
                textBox18.Text = "0";
            else if (Convert.ToDouble(numericUpDown1.Value) > 0 && Convert.ToDouble(numericUpDown1.Value) <= 0.5)
                textBox18.Text = "1";
            else if (Convert.ToDouble(numericUpDown1.Value) > 0.5 && Convert.ToDouble(numericUpDown1.Value) <= 1)
                textBox18.Text = "2";
            else if (Convert.ToDouble(numericUpDown1.Value) > 1 && Convert.ToDouble(numericUpDown1.Value) <= 1.5)
                textBox18.Text = "3";
            else if (Convert.ToDouble(numericUpDown1.Value) > 1.5 && Convert.ToDouble(numericUpDown1.Value) <= 2)
                textBox18.Text = "4";
            else if (Convert.ToDouble(numericUpDown1.Value) > 2 && Convert.ToDouble(numericUpDown1.Value) <= 2.5)
                textBox18.Text = "5";
            else if (Convert.ToDouble(numericUpDown1.Value) > 2.5 && Convert.ToDouble(numericUpDown1.Value) <= 3)
                textBox18.Text = "6";
            else if (Convert.ToDouble(numericUpDown1.Value) > 3 && Convert.ToDouble(numericUpDown1.Value) <= 3.5)
                textBox18.Text = "7";
            else if (Convert.ToDouble(numericUpDown1.Value) > 3.5 && Convert.ToDouble(numericUpDown1.Value) <= 4)
                textBox18.Text = "8";
            else if (Convert.ToDouble(numericUpDown1.Value) > 4 && Convert.ToDouble(numericUpDown1.Value) <= 4.5)
                textBox18.Text = "9";
            else if (Convert.ToDouble(numericUpDown1.Value) > 4.5 && Convert.ToDouble(numericUpDown1.Value) <= 5)
                textBox18.Text = "10";
            else if (Convert.ToDouble(numericUpDown1.Value) > 5 && Convert.ToDouble(numericUpDown1.Value) <= 5.5)
                textBox18.Text = "11";
            else if (Convert.ToDouble(numericUpDown1.Value) > 5.5 && Convert.ToDouble(numericUpDown1.Value) <= 6)
                textBox18.Text = "12";
            else if (Convert.ToDouble(numericUpDown1.Value) > 6 && Convert.ToDouble(numericUpDown1.Value) <= 6.5)
                textBox18.Text = "13";
            else if (Convert.ToDouble(numericUpDown1.Value) > 6.5 && Convert.ToDouble(numericUpDown1.Value) <= 7)
                textBox18.Text = "14";
            else if (Convert.ToDouble(numericUpDown1.Value) > 7 && Convert.ToDouble(numericUpDown1.Value) <= 7.5)
                textBox18.Text = "15";
            else if (Convert.ToDouble(numericUpDown1.Value) > 7.5)
                textBox18.Text = Convert.ToString(Convert.ToDouble(numericUpDown1.Value)*2);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            textBox17.Text = Convert.ToString(numericUpDown1.Value + numericUpDown2.Value + numericUpDown3.Value + numericUpDown4.Value);
            textBox19.Text = Convert.ToString(Convert.ToDouble(numericUpDown2.Value));
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            textBox17.Text = Convert.ToString(numericUpDown1.Value + numericUpDown2.Value + numericUpDown3.Value + numericUpDown4.Value);
            textBox20.Text = Convert.ToString(Convert.ToDouble(numericUpDown3.Value) / 1.5);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            textBox17.Text = Convert.ToString(numericUpDown1.Value + numericUpDown2.Value + numericUpDown3.Value + numericUpDown4.Value);
            textBox21.Text = Convert.ToString(Convert.ToDouble(numericUpDown4.Value) / 2);
        }

        private void yt_Button16_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text.Trim() != "")
                ostatok = Convert.ToDouble(comboBox2.Text);
            if (comboBox6.Text.Trim() == "" | comboBox2.Text.Trim() == "" | comboBox15.Text.Trim() == "" | comboBox6.Text.Trim() == "-")
                MessageBox.Show("Выберите напиток!");
            else if (textBox27.Text.Trim() == "")
                MessageBox.Show("Введите цену!");
            else if (textBox17.Text.Trim() == "" | textBox17.Text == "0" | textBox17.Text == "0,0")
                MessageBox.Show("Введите количество напитка!");
            else if (numericUpDown1.Value > 0 | numericUpDown2.Value > 0 | numericUpDown3.Value > 0 | numericUpDown4.Value > 0)
            {
                CenaNapit = Convert.ToDouble(textBox27.Text);
                if (numericUpDown2.Value > 0 | numericUpDown3.Value > 0 | numericUpDown4.Value > 0)
                {
                    if (numericUpDown3.Value > 0 | numericUpDown4.Value > 0)
                    {
                        if (numericUpDown4.Value > 0)
                        {
                            string query16 = "SELECT COUNT(id_InvoiceTable) AS count FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" WHERE Nomenclatures.Name LIKE 'Тара 2%' ";
                            SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                            conn2.Open();
                            SQLiteCommand cmd16 = new SQLiteCommand(query16, conn2);
                            Int64 count3 = (Int64)cmd16.ExecuteScalar();
                            cmd16.Dispose();
                            conn2.Close();

                            if (count3 == 0)
                            {
                                MessageBox.Show("Тары на 2 л нет в номенклатуре!");
                                return;
                            }
                            yt_Button7_Click(sender, e);
                        }
                        yt_Button6_Click_1(sender, e);
                    }
                    yt_Button5_Click_1(sender, e);
                }
                yt_Button4_Click_1(sender, e);

                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                if (checkBox10.Checked)
                {
                    string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                            " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                            " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                            " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
        " WHERE Type = 3 AND ProductGroups.Name = 'Напитки' AND Amount > 0 OR id_InvoiceTable=1  ORDER BY Nazvanie";
                    SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                    DataTable dt5 = new DataTable();
                    da5.Fill(dt5);


                    comboBox6.DataSource = dt5;
                    comboBox6.DisplayMember = "Nazvanie";
                    comboBox6.ValueMember = "id_InvoiceTable";
                    comboBox6.SelectedIndex = -1;
                    comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                    comboBox12.DataSource = dt5;
                    comboBox12.DisplayMember = "id_InvoiceTable";
                    comboBox12.ValueMember = "id_InvoiceTable";

                    comboBox2.DataSource = dt5;
                    comboBox2.DisplayMember = "Amount";
                    comboBox2.ValueMember = "id_InvoiceTable";
                    comboBox2.SelectedIndex = -1;

                    comboBox15.DataSource = dt5;
                    comboBox15.DisplayMember = "PriceSale";
                    comboBox15.ValueMember = "id_InvoiceTable";
                    comboBox15.SelectedIndex = -1;
                    textBox27.Text = comboBox15.Text;

                    cmd5.Dispose();
                }
                else
                {
                    string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, " +
                            " IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                            "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
                            " PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
                            " strftime('%d.%m.%Y',SrokGodnosti) FROM InvoiceTables " +
                            " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                            " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                " WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                    SQLiteCommand cmd5 = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                    DataTable dt5 = new DataTable();
                    da5.Fill(dt5);


                    comboBox6.DataSource = dt5;
                    comboBox6.DisplayMember = "Nazvanie";
                    comboBox6.ValueMember = "id_InvoiceTable";
                    comboBox6.SelectedIndex = -1;
                    comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

                    comboBox12.DataSource = dt5;
                    comboBox12.DisplayMember = "id_InvoiceTable";
                    comboBox12.ValueMember = "id_InvoiceTable";

                    comboBox2.DataSource = dt5;
                    comboBox2.DisplayMember = "Amount";
                    comboBox2.ValueMember = "id_InvoiceTable";
                    comboBox2.SelectedIndex = -1;

                    comboBox15.DataSource = dt5;
                    comboBox15.DisplayMember = "PriceSale";
                    comboBox15.ValueMember = "id_InvoiceTable";
                    comboBox15.SelectedIndex = -1;
                    textBox27.Text = comboBox15.Text;

                    cmd5.Dispose();
                }

                string query3 = "SELECT RealisationOnNomenclatures.id_InvoiceTable, id_RealisationOnNomenclature, ProductGroups.Name AS Группа, " +
        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование, " +
    "  RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Единица измерения], RealisationOnNomenclatures.Price || '  руб' AS [По Цене], Summa || '  руб' AS Сумма  FROM RealisationOnNomenclatures " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " JOIN InvoiceTables ON InvoiceTables.id_InvoiceTable = RealisationOnNomenclatures.id_InvoiceTable" +
    " WHERE id_Realisation = @idrealisation ORDER BY Группа DESC";
                    SQLiteCommand cmd3 = new SQLiteCommand(query3, conn);
                    cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox3.Text));
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView8.DataSource = dt3;
                    dataGridView8.Columns[0].Visible = false;
                    dataGridView8.Columns[1].Visible = false;
                    cmd3.Dispose();
                    dataGridView8.Select();
                    conn.Close();
                checkBox11.Checked = false;

                dataGridView8.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView8.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView8.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView8.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView8.Select();

                numericUpDown1.Value = 0;
                    numericUpDown2.Value = 0;
                    numericUpDown3.Value = 0;
                    numericUpDown4.Value = 0;
                    yt_Button11.Enabled = true;

                    checkBox5.Checked = false;
              //  }
            }
        }

        private void numericUpDown2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar)) || (e.KeyChar == 8)) return;
            else
                e.Handled = true;
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 0)
                numericUpDown1.Value = 0;
        }

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                yt_Button4.Enabled = false;
                yt_Button5.Enabled = false;
                yt_Button6.Enabled = false;
                yt_Button7.Enabled = false;
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
                numericUpDown3.Enabled = true;
                numericUpDown4.Enabled = true;
                textBox17.Enabled = true;
                textBox18.Enabled = true;
                textBox19.Enabled = true;
                textBox20.Enabled = true;
                textBox21.Enabled = true;
                yt_Button16.Enabled = true;
                textBox17.Text = "0";
                textBox18.Text = "0";
                textBox19.Text = "0";
                textBox20.Text = "0";
                textBox21.Text = "0";
            }
            else
            {
                yt_Button4.Enabled = true;
                yt_Button5.Enabled = true;
                yt_Button6.Enabled = true;
                yt_Button7.Enabled = true;
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = false;
                textBox17.Enabled = false;
                textBox18.Enabled = false;
                textBox19.Enabled = false;
                textBox20.Enabled = false;
                textBox21.Enabled = false;
                yt_Button16.Enabled = false;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                numericUpDown3.Value = 0;
                numericUpDown4.Value = 0;
                textBox17.Text = "0";
                textBox18.Text = "0";
                textBox19.Text = "0";
                textBox20.Text = "0";
                textBox21.Text = "0";
            }
        }

        private void OkNewUser_Click(object sender, EventArgs e)
        {
            if (tbNewUserLogin.Text.Trim() == "" | tbNewUserPass.Text.Trim() == "")
            {
                MessageBox.Show("Заполните логин и пароль!");
            }
            else
            {
                string query2 = "INSERT INTO Users (Login, Password) VALUES (@login, @password)";
                SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn2.Open();
                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn2);
                cmd2.Parameters.AddWithValue("@login", tbNewUserLogin.Text);
                cmd2.Parameters.AddWithValue("@password", tbNewUserPass.Text);
                cmd2.ExecuteNonQuery();
                cmd2.Dispose();
                conn2.Close();

                MessageBox.Show("Пользователь успешно добавлен!");
                tbNewUserLogin.Clear();
                tbNewUserPass.Clear();
            }
        }

        private void bAddOstat_Click(object sender, EventArgs e)
        {
            dataGridView7.Enabled = false;
            string query = "SELECT id_Nomenclature, ProductGroups.Name||' '||IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Articul, '' ) AS Nazvanie, EdIzm FROM Nomenclatures " +
                    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox3.DataSource = dt;
            comboBox3.DisplayMember = "Nazvanie";
            comboBox3.ValueMember = "id_Nomenclature";
            comboBox3.SelectedIndex = -1;
            comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;

            comboBox5.DataSource = dt;
            comboBox5.DisplayMember = "id_Nomenclature";
            comboBox5.ValueMember = "id_Nomenclature";
            cmd.Dispose();
            conn.Close();

            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            bSaveOstat.Visible = true;
            bCancelOstat.Visible = true;
            bAddOstat.Enabled = false;
            bEditOstat.Enabled = false;
            bDeleteOstat.Enabled = false;
            tbOstatSearch.ReadOnly = true;

            comboBox4.SelectedIndex = -1;

            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox7.Text = "0";
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker4.Value = DateTime.Now;


            ost = 1;  
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            ind = dataGridView3.CurrentRow.Index;
            tbidsotrud.Text = dataGridView3.Rows[ind].Cells[0].Value.ToString();
            tbSotrudFirstName.Text = dataGridView3.Rows[ind].Cells[1].Value.ToString();
            tbSotrudLastName.Text = dataGridView3.Rows[ind].Cells[2].Value.ToString();
            tbSotrudMiddleName.Text = dataGridView3.Rows[ind].Cells[3].Value.ToString();
            tbSotrudPhone.Text = dataGridView3.Rows[ind].Cells[4].Value.ToString();
        }

        private void dataGridView5_SelectionChanged(object sender, EventArgs e)
        {
            ind = dataGridView5.CurrentRow.Index;
            tbidprovider.Text = dataGridView5.Rows[ind].Cells[0].Value.ToString();
            cbProviderType.Text = dataGridView5.Rows[ind].Cells[1].Value.ToString();
            tbPostavName.Text = dataGridView5.Rows[ind].Cells[2].Value.ToString();
            tbPostavAddress.Text = dataGridView5.Rows[ind].Cells[3].Value.ToString();
            tbPostavPhone.Text = dataGridView5.Rows[ind].Cells[4].Value.ToString();
            tbPostavDopPhone.Text = dataGridView5.Rows[ind].Cells[5].Value.ToString();
            tbPostavEmail.Text = dataGridView5.Rows[ind].Cells[6].Value.ToString();
        }

        private void dataGridView6_SelectionChanged(object sender, EventArgs e)
        {
            ind = dataGridView6.CurrentRow.Index;
            tbidnomenclature.Text = dataGridView6.Rows[ind].Cells[0].Value.ToString();
            comboBox1.Text = dataGridView6.Rows[ind].Cells[1].Value.ToString();
            tbNomenclatureName.Text = dataGridView6.Rows[ind].Cells[3].Value.ToString();
            tbNomenclatureArticul.Text = dataGridView6.Rows[ind].Cells[4].Value.ToString();
            tbNomenclatureMassa.Text = dataGridView6.Rows[ind].Cells[5].Value.ToString();
            cbEdIzm.Text = dataGridView6.Rows[ind].Cells[6].Value.ToString();
        }

        private void yt_Button17_Click(object sender, EventArgs e)
        {
            FormProductGroups formproductgroups = new FormProductGroups();
            formproductgroups.Owner = this;
            formproductgroups.ShowDialog();
        }

        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox16.Text = comboBox16.Text;
        }

        private void textBox16_Enter(object sender, EventArgs e)
        {
            if (textBox16.Text == "0")
                textBox16.Clear();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                textBox16.Enabled = true;
                textBox16.Focus();
            }
            else
            {
                textBox16.Enabled = false;
                textBox16.Text = comboBox16.Text;
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox6.Checked = false;
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            double summa;
            if (textBox16.Text != "" && textBox4.Text != "" && textBox16.Text != "System.Data.DataRowView")
            {
                double n = Convert.ToDouble(textBox4.Text);
                double cena = Convert.ToDouble(textBox16.Text);
                summa = n * cena;
                textBox5.Text = summa.ToString();
            }
            else textBox5.Text = "0";
        }

        private void textBox23_Enter(object sender, EventArgs e)
        {
            if (textBox23.Text == "0")
                textBox23.Clear();
        }

        private void textBox23_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            if (textBox23.Text != "" && textBox4.Text != "" && textBox23.Text != "System.Data.DataRowView" && textBox16.Text != "" && textBox16.Text != "System.Data.DataRowView")
            {
                textBox4.Text = Convert.ToString(Convert.ToDouble(textBox23.Text) / 1000);
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text.Trim() == "")
                textBox4.Text = "0";
        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            if (textBox16.Text.Trim() == "")
                textBox16.Text = "0";
        }

        private void textBox23_Leave(object sender, EventArgs e)
        {
            if (textBox23.Text.Trim() == "")
                textBox23.Text = "0";
        }

        private void label51_TextChanged(object sender, EventArgs e)
        {
            if (label51.Text.Trim() == "КГ" | label51.Text.Trim() == "Кг" | label51.Text.Trim() == "кг")
            {
                label66.Visible = true;
                label67.Visible = true;
                textBox23.Visible = true;
            }
            else
            {
                label66.Visible = false;
                label67.Visible = false;
                textBox23.Visible = false;
            }
        }

        private void dataGridView8_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dataGridView8.Rows.Count < 1)
                yt_Button10.Enabled = false;
            else yt_Button10.Enabled = true;
        }

        private void dataGridView8_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dataGridView8.Rows.Count > 0)
                yt_Button10.Enabled = true;
            else yt_Button10.Enabled = false;
        }

        private void yt_Button18_Click(object sender, EventArgs e)
        {
            if (textBox24.Text.Trim() == "Приход")
            {
                dataGridView4.Select();
                if (dataGridView4.Rows.Count > 1)
                {
                                string query6 = "UPDATE InvoiceTables SET id_InvoiceHeader = @idinvoiceheader, Amount = Amount - @amount, SrokGodnosti = @srokgodnosti, EdIzm=@edizm, PriceSale = @pricesale " +
            "WHERE id_Nomenclature = @idnomenclature AND Type = 3";
                    SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn1.Open();
                    SQLiteCommand cmd5 = new SQLiteCommand(query6, conn1);
                                cmd5.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(0));
                                cmd5.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(textBox9.Text.Trim()));
                                cmd5.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox15.Text.Trim()));
                                cmd5.Parameters.AddWithValue("@edizm", Convert.ToString(textBox25.Text.Trim()));
                                cmd5.Parameters.AddWithValue("@srokgodnosti", "-");
                                cmd5.Parameters.AddWithValue("@pricesale", Convert.ToDouble(0));
                                SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                                DataTable dt5 = new DataTable();
                                da5.Fill(dt5);
                                cmd5.Dispose();

                                string query2 = "DELETE FROM InvoiceTables WHERE id_InvoiceTable = @idinvoicetable";
                                SQLiteCommand cmd2 = new SQLiteCommand(query2, conn1);
                                cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(textBox8.Text));
                                SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                                DataTable dt2 = new DataTable();
                                da2.Fill(dt2);
                                cmd2.Dispose();
                            

                    string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature, ProductGroups.Name AS Группа, " +
    "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
    "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения], Rasfasovka AS [Расфасовка (уп)]," +
    " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до], Price|| ' руб' AS Цена, Summ || ' руб' AS Сумма,  PriceSale|| ' руб' AS [Цена на продажу]  FROM InvoiceTables " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " WHERE Type = 1 AND InvoiceTables.id_InvoiceHeader = @idinvoiceheader";
                    SQLiteCommand cmd3 = new SQLiteCommand(query1, conn1);
                    cmd3.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(tbidInvoice.Text));
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView4.DataSource = dt3;
                    dataGridView4.Columns[0].Visible = false;
                    dataGridView4.Columns[1].Visible = false;
                    cmd3.Dispose();
                    conn1.Close();
                    dataGridView4.Focus();
                }
                else if (dataGridView4.Rows.Count == 1)
                {
                    string query6 = "UPDATE InvoiceTables SET id_InvoiceHeader = @idinvoiceheader, Amount = Amount - @amount, SrokGodnosti = @srokgodnosti, EdIzm=@edizm, PriceSale = @pricesale " +
"WHERE id_Nomenclature = @idnomenclature AND Type = 3";
                    SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn1.Open();
                    SQLiteCommand cmd5 = new SQLiteCommand(query6, conn1);
                    cmd5.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(0));
                    cmd5.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(textBox9.Text.Trim()));
                    cmd5.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox15.Text.Trim()));
                    cmd5.Parameters.AddWithValue("@edizm", Convert.ToString(textBox25.Text.Trim()));
                    cmd5.Parameters.AddWithValue("@srokgodnosti", "-");
                    cmd5.Parameters.AddWithValue("@pricesale", Convert.ToDouble(0));
                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                    DataTable dt5 = new DataTable();
                    da5.Fill(dt5);
                    cmd5.Dispose();

                    string query2 = "DELETE FROM InvoiceTables WHERE id_InvoiceTable = @idinvoicetable";
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, conn1);
                    cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(textBox8.Text));
                    SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    cmd2.Dispose();
                    dataGridView4.Focus();

                    string query7 = "DELETE FROM InvoiceHeaders WHERE id_InvoiceHeader = @idinvoiceheader";
                    SQLiteCommand cmd7 = new SQLiteCommand(query7, conn1);
                    cmd7.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(tbidInvoice.Text));
                    SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                    DataTable dt7 = new DataTable();
                    da7.Fill(dt7);
                    cmd7.Dispose();

                    string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature, ProductGroups.Name AS Группа, " +
    "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
    "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения], Rasfasovka AS [Расфасовка (уп)]," +
    " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до], Price|| ' руб' AS Цена, Summ || ' руб' AS Сумма,  PriceSale|| ' руб' AS [Цена на продажу]  FROM InvoiceTables " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " WHERE Type = 1 AND InvoiceTables.id_InvoiceHeader = @idinvoiceheader";
                    SQLiteCommand cmd3 = new SQLiteCommand(query1, conn1);
                    cmd3.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(tbidInvoice.Text));
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView4.DataSource = dt3;
                    dataGridView4.Columns[0].Visible = false;
                    dataGridView4.Columns[1].Visible = false;
                    cmd3.Dispose();
                    conn1.Close();
                }
            }
            else if (textBox24.Text.Trim() == "Расход")
            {
                dataGridView4.Select();
                if (dataGridView4.Rows.Count > 1)
                {
                    string query6 = "UPDATE InvoiceTables SET id_InvoiceHeader = @idinvoiceheader, Amount = Amount + @amount,  EdIzm=@edizm " +
"WHERE id_Nomenclature = @idnomenclature AND Type = 3";
                    SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn1.Open();
                    SQLiteCommand cmd5 = new SQLiteCommand(query6, conn1);
                    cmd5.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(0));
                    cmd5.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(textBox9.Text.Trim()));
                    cmd5.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox15.Text.Trim()));
                    cmd5.Parameters.AddWithValue("@edizm", Convert.ToString(textBox25.Text.Trim()));
                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                    DataTable dt5 = new DataTable();
                    da5.Fill(dt5);
                    cmd5.Dispose();

                    string query2 = "DELETE FROM InvoiceTables WHERE id_InvoiceTable = @idinvoicetable";
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, conn1);
                    cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(textBox8.Text));
                    SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    cmd2.Dispose();


                    string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature, ProductGroups.Name AS Группа, " +
    "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
    "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения], Rasfasovka AS [Расфасовка (уп)]," +
    " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до], Price|| ' руб' AS Цена, Summ || ' руб' AS Сумма,  PriceSale|| ' руб' AS [Цена на продажу]  FROM InvoiceTables " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " WHERE Type = 2 AND InvoiceTables.id_InvoiceHeader = @idinvoiceheader";
                    SQLiteCommand cmd3 = new SQLiteCommand(query1, conn1);
                    cmd3.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(tbidInvoice.Text));
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView4.DataSource = dt3;
                    dataGridView4.Columns[0].Visible = false;
                    dataGridView4.Columns[1].Visible = false;
                    cmd3.Dispose();
                    conn1.Close();
                    dataGridView4.Focus();
                }
                else if (dataGridView4.Rows.Count == 1)
                {
                    string query6 = "UPDATE InvoiceTables SET id_InvoiceHeader = @idinvoiceheader, Amount = Amount + @amount, EdIzm=@edizm " +
"WHERE id_Nomenclature = @idnomenclature AND Type = 3";
                    SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn1.Open();
                    SQLiteCommand cmd5 = new SQLiteCommand(query6, conn1);
                    cmd5.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(0));
                    cmd5.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(textBox9.Text.Trim()));
                    cmd5.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox15.Text.Trim()));
                    cmd5.Parameters.AddWithValue("@edizm", Convert.ToString(textBox25.Text.Trim()));
                    SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                    DataTable dt5 = new DataTable();
                    da5.Fill(dt5);
                    cmd5.Dispose();

                    string query2 = "DELETE FROM InvoiceTables WHERE id_InvoiceTable = @idinvoicetable";
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, conn1);
                    cmd2.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(textBox8.Text));
                    SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    cmd2.Dispose();
                    dataGridView4.Focus();

                    string query7 = "DELETE FROM InvoiceHeaders WHERE id_InvoiceHeader = @idinvoiceheader";
                    SQLiteCommand cmd7 = new SQLiteCommand(query7, conn1);
                    cmd7.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(tbidInvoice.Text));
                    SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                    DataTable dt7 = new DataTable();
                    da7.Fill(dt7);
                    cmd7.Dispose();

                    string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature, ProductGroups.Name AS Группа, " +
    "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
    "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения], Rasfasovka AS [Расфасовка (уп)]," +
    " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до], Price|| ' руб' AS Цена, Summ || ' руб' AS Сумма,  PriceSale|| ' руб' AS [Цена на продажу]  FROM InvoiceTables " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " WHERE Type = 2 AND InvoiceTables.id_InvoiceHeader = @idinvoiceheader";
                    SQLiteCommand cmd3 = new SQLiteCommand(query1, conn1);
                    cmd3.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(tbidInvoice.Text));
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView4.DataSource = dt3;
                    dataGridView4.Columns[0].Visible = false;
                    dataGridView4.Columns[1].Visible = false;
                    cmd3.Dispose();
                    conn1.Close();
                }
            }
        }

        private void dataGridView4_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView4.Rows.Count > 0)
            {
                int ind2 = dataGridView4.CurrentRow.Index;
                textBox8.Text = dataGridView4.Rows[ind2].Cells[0].Value.ToString();
                if (dataGridView4.Rows[ind2].Cells[6].Value.ToString() != "" && dataGridView4.Rows[ind2].Cells[6].Value.ToString() != "0")
                    textBox15.Text = dataGridView4.Rows[ind2].Cells[6].Value.ToString();
                else
                    textBox15.Text = dataGridView4.Rows[ind2].Cells[4].Value.ToString();
                textBox9.Text = dataGridView4.Rows[ind2].Cells[1].Value.ToString();
                if (dataGridView4.Rows[ind2].Cells[6].Value.ToString() != "" && dataGridView4.Rows[ind2].Cells[6].Value.ToString() != "0")
                    textBox25.Text = "Уп";
                else
                    textBox25.Text = dataGridView4.Rows[ind2].Cells[5].Value.ToString(); ;
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var g = e.Graphics;

            int leftMargin = e.MarginBounds.Left - 55; // отступы слева в документе
            int topMargin = e.MarginBounds.Top; // отступы сверху в документе
            int yPos = 0; // текущая позиция Y для вывода строки

            Font myFont = new Font("Arial", 17, FontStyle.Regular, GraphicsUnit.Pixel);

            for (; printed < listBox3.Items.Count; ++printed)
            {
                yPos = (int)(topMargin + printed * (myFont.GetHeight(e.Graphics)+10))-55;

                g.DrawString( 
                    (string)listBox3.Items[printed],
                    myFont,
                    Brushes.Black,
                    leftMargin, yPos, new StringFormat());
            }

            e.HasMorePages = printed != listBox3.Items.Count;
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (printed != 0)
                e.Cancel = true;
        }

        private void printDocument1_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            printed = 0;
        }

        private void yt_Button20_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void yt_Button19_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
                printDocument1.Print();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                checkBox8.Checked = false;
                checkBox7.BackColor = System.Drawing.Color.FromArgb(255, 204, 0);
            }
            else
            {
                checkBox7.BackColor = System.Drawing.Color.White;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                checkBox7.Checked = false;
                checkBox8.BackColor = System.Drawing.Color.FromArgb(255, 204, 0);
            }
            else
            {
                checkBox8.BackColor = System.Drawing.Color.White;
            }
        }

        private void checkBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (checkBox5.Checked)
                    checkBox5.Checked = false;
                else
                    checkBox5.Checked = true;
            }
        }

        private void checkBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (checkBox6.Checked)
                    checkBox6.Checked = false;
                else
                    checkBox6.Checked = true;
            }
        }

        private void yt_Button16_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button16_Click(sender, e);
            }
        }

        private void yt_Button10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button10_Click(sender, e);
            }
        }

        private void comboBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comboBox9.DroppedDown = true;
            }
        }

        private void checkBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                checkBox7_CheckedChanged(sender, e);
            }
        }

        private void checkBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                checkBox8_CheckedChanged(sender, e);
            }
        }

        private void comboBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') || 
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я') || (e.KeyChar >= 'а' && e.KeyChar <= 'я') || (e.KeyChar == 8) || (e.KeyChar == 45)) return;
            else
                e.Handled = true;
        }

        private void comboBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox18.Text == "System.Data.DataRowView")
                textBox26.Text = "0";
            else textBox26.Text = comboBox18.Text;
        }

        private void textBox26_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar)) || (e.KeyChar == 8)) return;
            else
                e.Handled = true;
        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {
            if (textBox26.Text != "0" && textBox26.Text != "")
            {
                textBox11.Text = Convert.ToString(Convert.ToDouble(summ) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
            }
            else if (textBox26.Text == "0" || textBox26.Text == "")
                textBox11.Text = Convert.ToString(summ);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox26.Text != "0" && textBox26.Text != "")
            {
                textBox11.Text = Convert.ToString(Convert.ToDouble(summ) * (1 - (Convert.ToDouble(textBox26.Text) / 100)));
            }
            else if (textBox26.Text == "0" || textBox26.Text == "")
                textBox11.Text = Convert.ToString(summ);
            button1.Visible = false;
            button2.Enabled = true;
            textBox26.Enabled = false;
        }

        private void textBox26_EnabledChanged(object sender, EventArgs e)
        {
            if (textBox26.Enabled == true)
                return;
            else textBox26_TextChanged(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Enabled)
            {
                textBox26.Enabled = true;
                textBox26.Focus();
                button1.Visible = true;
                button2.Enabled = false;
            }
            else
                textBox26.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button2.Enabled)
            {
                textBox16.Enabled = true;
                textBox16.Focus();
                button3.Enabled = false;
            }
            else
                textBox16.Enabled = false;
        }

        private void button3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (button3.Enabled == true)
                    button3.Enabled = false;
                else
                    button3.Enabled = true;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
            {
                textBox16.Enabled = true;
                textBox16.Focus();
            }
            else
            {
                textBox16.Enabled = false;
                textBox16.Text = comboBox16.Text;
            }
        }

        private void checkBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (checkBox9.Checked)
                    checkBox9.Checked = false;
                else
                    checkBox9.Checked = true;
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                Properties.Settings.Default.SavedSetting1 = 1;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.SavedSetting1 = 0;
                Properties.Settings.Default.Save();
            }
        }

        private void textBox27_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        private void textBox27_Leave(object sender, EventArgs e)
        {
            if (textBox27.Text.Trim() == "")
                checkBox11.Checked = false;
        }

        private void textBox27_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
            {
                textBox27.Enabled = true;
                textBox27.Focus();
            }
            else
            {
                textBox27.Enabled = false;
                textBox27.Text = comboBox15.Text;
            }
        }

        private void checkBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (checkBox11.Checked)
                    checkBox11.Checked = false;
                else
                    checkBox11.Checked = true;
            }
        }

        private void textBox28_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        private void textBox29_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox28.Enabled = true;
            button5.Visible = true;
            button7.Enabled = false;
            textBox28.Focus();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox29.Enabled = true;
            button6.Visible = true;
            button8.Enabled = false;
            textBox29.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox28.Text.Trim() == "")
                MessageBox.Show("Введите количество!");
            else
            {
                Properties.Settings.Default.UvedomNapit = Convert.ToDouble(textBox28.Text);
                Properties.Settings.Default.Save();
                textBox28.Enabled = false;
                button7.Enabled = true;
                button5.Visible = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox29.Text.Trim() == "")
                MessageBox.Show("Введите количество!");
            else
            {
                Properties.Settings.Default.UvedomSrok = Convert.ToInt32(textBox29.Text);
                Properties.Settings.Default.Save();
                textBox29.Enabled = false;
                button8.Enabled = true;
                button6.Visible = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox30.Enabled = true;
            button10.Visible = true;
            button9.Enabled = false;
            textBox30.Focus();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                email = email.Trim();
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!IsValidEmail(textBox30.Text))
                MessageBox.Show("Введите корректный Email!");
            else
            {
                Properties.Settings.Default.Email = textBox30.Text;
                Properties.Settings.Default.Save();
                textBox30.Enabled = false;
                button9.Enabled = true;
                button10.Visible = false;
                email = textBox30.Text;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
                FormClearBD formclearbd = new FormClearBD();
                formclearbd.ShowDialog();
        }

        private void textBox31_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar)) || (e.KeyChar == 8)) return;
            else
                e.Handled = true;
        }

        private void textBox32_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar)) || (e.KeyChar == 8)) return;
            else
                e.Handled = true;
        }

        private void bEditOstat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button4_Click(sender, e);
            }
        }

        private void bSaveOstat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button5_Click(sender, e);
            }
        }

        private void bCancelOstat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button6_Click(sender, e);
            }
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
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button2_Click(sender, e);
            }
        }

        private void yt_Button3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button3_Click(sender, e);
            }
        }

        private void bAddClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bAddClient_Click_1(sender, e);
            }
        }

        private void EditClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EditClient_Click_1(sender, e);
            }
        }

        private void DeleteClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DeleteClient_Click(sender, e);
            }
        }

        private void SaveClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SaveClient_Click(sender, e);
            }
        }

        private void CancelClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CancelClient_Click_1(sender, e);
            }
        }

        private void AddNomenclature_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddNomenclature_Click(sender, e);
            }
        }

        private void EditNomenclature_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EditNomenclature_Click(sender, e);
            }
        }

        private void DeleteNomenclature_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DeleteNomenclature_Click(sender, e);
            }
        }

        private void SaveNomenclature_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SaveNomenclature_Click(sender, e);
            }
        }

        private void CancelNomenclature_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CancelNomenclature_Click(sender, e);
            }
        }

        private void yt_Button17_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button17_Click(sender, e);
            }
        }

        private void AddPostav_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddPostav_Click(sender, e);
            }
        }

        private void EditPostav_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EditPostav_Click(sender, e);
            }
        }

        private void DeletePostav_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DeletePostav_Click(sender, e);
            }
        }

        private void SavePostav_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SavePostav_Click(sender, e);
            }
        }

        private void CancelPostav_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CancelPostav_Click(sender, e);
            }
        }

        private void AddSotrud_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddSotrud_Click(sender, e);
            }
        }

        private void EditSotrud_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EditSotrud_Click(sender, e);
            }
        }

        private void SaveSotrud_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SaveSotrud_Click(sender, e);
            }
        }

        private void CancelSotrud_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CancelSotrud_Click(sender, e);
            }
        }

        private void yt_Button13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button13_Click(sender, e);
            }
        }

        private void yt_Button14_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button14_Click(sender, e);
            }
        }

        private void ChangeAdminPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangeAdminPass_Click(sender, e);
            }
        }

        private void AddUsers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button1_Click(sender, e);
            }
        }

        private void yt_Button20_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button20_Click(sender, e);
            }
        }

        private void yt_Button19_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                yt_Button19_Click(sender, e);
            }
        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox27.Text = comboBox15.Text;
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                    ind = dataGridView2.CurrentRow.Index;
                    tbidInvoice.Text = dataGridView2.Rows[ind].Cells[0].Value.ToString();
                    textBox24.Text = dataGridView2.Rows[ind].Cells[1].Value.ToString();
                if (textBox24.Text == "Приход")
                {
                    string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature,  ProductGroups.Name AS Группа, " +
                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                        "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения], Rasfasovka AS [Расфасовка (Уп)], " +
                        " strftime('%d.%m.%Y', SrokGodnosti) AS [Срок годности до], Price || ' руб'  AS Цена, Summ || ' руб'  AS Сумма,  PriceSale || ' руб'  AS [Цена на продажу]  FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                        " WHERE(id_InvoiceHeader = @idinvoiceheader) AND (Type = 1 OR Type = 2)";
                    SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    SQLiteCommand cmd3 = new SQLiteCommand(query1, conn2);
                    cmd3.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(tbidInvoice.Text));
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView4.DataSource = dt3;
                    dataGridView4.Columns[0].Visible = false;
                    dataGridView4.Columns[1].Visible = false;
                    cmd3.Dispose();
                    conn2.Close();
                    dataGridView4.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //dataGridView4.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView4.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView4.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView4.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (textBox24.Text == "Расход")
                {
                    string query1 = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature,  ProductGroups.Name AS Группа, " +
                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                        "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
                        " Price || ' руб'  AS Цена, Summ || ' руб'  AS Сумма,  PriceSale || ' руб'  AS [Цена на продажу]  FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                        " WHERE(id_InvoiceHeader = @idinvoiceheader) AND (Type = 1 OR Type = 2)";
                    SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    SQLiteCommand cmd3 = new SQLiteCommand(query1, conn2);
                    cmd3.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(tbidInvoice.Text));
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    dataGridView4.DataSource = dt3;
                    dataGridView4.Columns[0].Visible = false;
                    dataGridView4.Columns[1].Visible = false;
                    cmd3.Dispose();
                    conn2.Close();
                    dataGridView4.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView4.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView4.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView4.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView4.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }
    }
}
