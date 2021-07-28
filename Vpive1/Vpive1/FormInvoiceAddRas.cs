using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vpive1
{
    public partial class FormInvoiceAddRas : Form
    {
        int t;
        public FormInvoiceAddRas()
        {
            InitializeComponent();
        }

        private void FormInvoiceAddRas_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.Enabled = false;
            comboBox1.SelectedIndex = 1;

            string query = "SELECT id_Provider, Name FROM Providers WHERE Type='Покупатель'";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "id_Provider";
            comboBox2.SelectedIndex = -1;
            comboBox7.DataSource = dt;
            comboBox7.DisplayMember = "id_Provider";
            comboBox7.ValueMember = "id_Provider";
            cmd.Dispose();
            conn.Close();

            query = "SELECT id_Employee, (LastName||' ' ||FirstName||' '||MiddleName) AS FIO FROM Employees";
            conn.Open();
            SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            comboBox3.DataSource = dt1;
            comboBox3.DisplayMember = "FIO";
            comboBox3.ValueMember = "id_Employee";
            comboBox3.SelectedIndex = -1;
            comboBox8.DataSource = dt1;
            comboBox8.DisplayMember = "id_Employee";
            comboBox8.ValueMember = "id_Employee";
            cmd.Dispose();
            conn.Close();

            comboBox4.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox4.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void yt_Button4_Click(object sender, EventArgs e)
        {
            if ((comboBox1.SelectedIndex == -1) | (comboBox2.SelectedIndex == -1) | (comboBox3.SelectedIndex == -1))
                MessageBox.Show("Введите всю необходимую информацию выше!", "Ошибка");
            else
            {
                string query = "SELECT id_InvoiceTable, InvoiceTables.id_InvoiceHeader, Nomenclatures.id_Nomenclature, IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Articul, '' )" +
                    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie, " +
                    "Amount, InvoiceTables.EdIzm FROM InvoiceTables " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature = Nomenclatures.id_Nomenclature " +
    " WHERE InvoiceTables.Type = 3 OR id_InvoiceTable=1 ORDER BY Nazvanie";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox4.DataSource = dt;
                comboBox4.DisplayMember = "Nazvanie";
                comboBox4.ValueMember = "id_InvoiceTable";
                comboBox4.SelectedIndex = -1;
                comboBox4.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox4.AutoCompleteSource = AutoCompleteSource.ListItems;


                comboBox9.DataSource = dt;
                comboBox9.DisplayMember = "Amount";
                comboBox9.ValueMember = "id_InvoiceTable";
                comboBox9.SelectedIndex = -1;

                comboBox5.DataSource = dt;
                comboBox5.DisplayMember = "EdIzm";
                comboBox5.ValueMember = "id_InvoiceTable";
                comboBox5.SelectedIndex = -1;

                cbidNomen.DataSource = dt;
                cbidNomen.DisplayMember = "id_Nomenclature";
                cbidNomen.ValueMember = "id_InvoiceTable";
                cbidNomen.SelectedIndex = -1;

                cbInvoiceTable.DataSource = dt;
                cbInvoiceTable.DisplayMember = "id_InvoiceTable";
                cbInvoiceTable.ValueMember = "id_InvoiceTable"; 


                cmd.Dispose();
                conn.Close();

                comboBox4.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                yt_Button7.Enabled = true;
                yt_Button8.Enabled = true;
                textBox6.Enabled = true;
                yt_Button4.Enabled = false;
                yt_Button5.Enabled = false;

                comboBox5.SelectedIndex = -1;
                textBox4.Text = "0";
                textBox5.Text = "0";
                textBox6.Text = "0";
                yt_Button7.Visible = true;
                yt_Button8.Visible = true;

                t = 1;

            }
        }

        private void yt_Button8_Click(object sender, EventArgs e)
        {
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            yt_Button7.Enabled = false;
            yt_Button8.Enabled = false;
            textBox6.Enabled = false;
            yt_Button4.Enabled = true;
            yt_Button5.Enabled = true;

            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            yt_Button7.Visible = false;
            yt_Button8.Visible = false;
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

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            text_press(sender, e);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            double n = 0;
            double cena = 0;
            double summa;
            if (textBox5.Text != "" && textBox4.Text != "")
            {
                n = Convert.ToDouble(textBox5.Text);
                cena = Convert.ToDouble(textBox4.Text);
                summa = n * cena;
                textBox6.Text = summa.ToString();
            }
            else textBox6.Text = "0";
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            double n = 0;
            double cena = 0;
            double summa;
            if (textBox5.Text != "" && textBox4.Text != "")
            {
                n = Convert.ToDouble(textBox5.Text);
                cena = Convert.ToDouble(textBox4.Text);
                summa = n * cena;
                textBox6.Text = summa.ToString();
            }
            else textBox6.Text = "0";
        }

        private void yt_Button7_Click(object sender, EventArgs e)
        {
            if ((cbidNomen.SelectedIndex == -1) | (comboBox5.SelectedIndex == -1) | (textBox5.Text == "") | (textBox6.Text == "") | (comboBox4.Text == ""))
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка");
            }
            else
            {
                textBox7.Text = Convert.ToString(Convert.ToDouble(comboBox9.Text) - Convert.ToDouble(textBox5.Text));
                if (textBox15.Text == "")
                {
                    string query2 = "INSERT INTO InvoiceHeaders (id_Provider, id_InvoiceType, id_Employee, Date, Sum) VALUES (@idprovider, @idinvoicetype, @idemployee, @date, @sum)";
                    SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn2.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query2, conn2);
                    cmd.Parameters.AddWithValue("@idprovider", Convert.ToInt32(comboBox7.SelectedValue));
                    cmd.Parameters.AddWithValue("@idinvoicetype", Convert.ToInt32(textBox2.Text));
                    cmd.Parameters.AddWithValue("@idemployee", Convert.ToInt32(comboBox8.SelectedValue));
                    cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@sum", Convert.ToDouble(textBox1.Text));
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmd.Dispose();

                    string query3 = "SELECT MAX(id_InvoiceHeader) FROM InvoiceHeaders";
                    SQLiteCommand cmd1 = new SQLiteCommand(query3, conn2);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    Int64 id = (Int64)cmd1.ExecuteScalar();
                    textBox15.Text = Convert.ToString(id);
                    cmd1.Dispose();

                    string query = "INSERT INTO InvoiceTables (id_InvoiceHeader, id_Nomenclature, Type, Amount, SrokGodnosti, Price, EdIzm, Summ) " +
                        "VALUES (@idinvoiceheader, @idnomenclature, @type, @amount, @srokgodnosti, @price, @edizm, @summ)";
                    SQLiteCommand cmd2 = new SQLiteCommand(query, conn2);
                    cmd2.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
                    cmd2.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(cbidNomen.Text));
                    cmd2.Parameters.AddWithValue("@type", 2);
                    cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox5.Text));
                    cmd2.Parameters.AddWithValue("@srokgodnosti", "-");
                    cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(textBox4.Text));
                    cmd2.Parameters.AddWithValue("@edizm", comboBox5.Text);
                    cmd2.Parameters.AddWithValue("@summ", Convert.ToDouble(textBox6.Text));
                    SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    cmd2.Dispose();

                    Load_Data();

                }
                else
                {
                    string query = "INSERT INTO InvoiceTables (id_InvoiceHeader, id_Nomenclature, Type, Amount, SrokGodnosti, Price, EdIzm, Summ) " +
                        "VALUES (@idinvoiceheader, @idnomenclature, @type, @amount, @srokgodnosti, @price, @edizm, @summ)";
                    SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn2.Open();
                    SQLiteCommand cmd2 = new SQLiteCommand(query, conn2);
                    cmd2.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
                    cmd2.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(cbidNomen.Text));
                    cmd2.Parameters.AddWithValue("@type", 2);
                    cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox5.Text));
                    cmd2.Parameters.AddWithValue("@srokgodnosti", "-");
                    cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(textBox4.Text));
                    cmd2.Parameters.AddWithValue("@edizm", comboBox5.Text);
                    cmd2.Parameters.AddWithValue("@summ", Convert.ToDouble(textBox6.Text));
                    SQLiteDataAdapter da2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    cmd2.Dispose();

                    Load_Data();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox2.Text = "1";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                textBox2.Text = "2";
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int ind = dataGridView1.CurrentRow.Index;
            tbidtovar.Text = dataGridView1.Rows[ind].Cells[0].Value.ToString();
        }

        private void yt_Button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Select();
            if (dataGridView1.Rows.Count > 0)
            {
                int count = dataGridView1.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    string query = "SELECT * FROM InvoiceTables WHERE id_InvoiceHeader=@idinvoiceheader AND Type = 2 LIMIT 1 OFFSET @offset";
                    SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn1.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn1);
                    cmd.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
                    cmd.Parameters.AddWithValue("@offset", Convert.ToInt32(i));
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    cmd.Dispose();
                    

                    if (dataGridView1.Rows[0].Cells[5].Value.ToString() == "")
                    {
                        string query6 = "UPDATE InvoiceTables SET id_InvoiceHeader = @idinvoiceheader, Amount = Amount - @amount " +
    "WHERE id_Nomenclature = @idnomenclature AND Type = 3";
                        SQLiteCommand cmd5 = new SQLiteCommand(query6, conn1);
                        cmd5.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
                        cmd5.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(dataGridView1.Rows[0].Cells[2].Value));
                        cmd5.Parameters.AddWithValue("@amount", Convert.ToDouble(dataGridView1.Rows[0].Cells[4].Value));
                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                        DataTable dt5 = new DataTable();
                        da5.Fill(dt5);
                        cmd5.Dispose();
                        dataGridView1.Focus();
                        conn1.Close();
                    }
                    else
                    {
                        string query6 = "UPDATE InvoiceTables SET id_InvoiceHeader = @idinvoiceheader, Amount = Amount - @amount " +
        "WHERE id_Nomenclature = @idnomenclature AND Type = 3";
                        SQLiteCommand cmd5 = new SQLiteCommand(query6, conn1);
                        cmd5.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
                        cmd5.Parameters.AddWithValue("@idnomenclature", Convert.ToInt32(dataGridView1.Rows[0].Cells[2].Value));
                        cmd5.Parameters.AddWithValue("@amount", Convert.ToDouble(dataGridView1.Rows[0].Cells[4].Value));
                        SQLiteDataAdapter da5 = new SQLiteDataAdapter(cmd5);
                        DataTable dt5 = new DataTable();
                        da5.Fill(dt5);
                        cmd5.Dispose();
                        dataGridView1.Focus();
                        conn1.Close();
                    }
                }

                string query7 = "UPDATE InvoiceHeaders SET Sum = @sum  WHERE id_InvoiceHeader = @idinvoiceheader";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd7 = new SQLiteCommand(query7, conn);
                cmd7.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
                cmd7.Parameters.AddWithValue("@sum", Convert.ToDouble(textBox1.Text));
                SQLiteDataAdapter da7 = new SQLiteDataAdapter(cmd7);
                DataTable dt7 = new DataTable();
                da7.Fill(dt7);
                cmd7.Dispose();
                conn.Close();

                textBox7.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox15.Clear();
                yt_Button7.Enabled = false;
                yt_Button8.Enabled = false;
                this.Hide();
            }
            else
            {
                MessageBox.Show("Добавьте товары!", "Ошибка!");
            }
        }

        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            EdIzm.Text = comboBox5.Text;
        }

        private void yt_Button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Удалить запись?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM InvoiceTables WHERE id_InvoiceTable = @idinvoicetable";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(tbidtovar.Text));
                    cmd.ExecuteNonQuery();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    cmd.Dispose();
                    conn.Close();

                    query = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                        "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
                        " SrokGodnosti AS [Срок годности до], Price || ' руб' AS Цена, Summ || ' руб' AS Сумма  FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                        " WHERE id_InvoiceHeader = @idinvoiceheader";
                    conn.Open();
                    SQLiteCommand cmd1 = new SQLiteCommand(query, conn);
                    cmd1.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    dataGridView1.DataSource = dt1;
                    dataGridView1.Columns[0].Visible = false;
                    cmd.Dispose();
                    conn.Close();

                    string query4 = "SELECT SUM(Summ) AS Summa FROM InvoiceTables WHERE id_InvoiceHeader = @idinvoiceheader";
                    SQLiteConnection conn4 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn4.Open();
                    SQLiteCommand cmd4 = new SQLiteCommand(query4, conn4);
                    cmd4.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
                    SQLiteDataAdapter da4 = new SQLiteDataAdapter(cmd4);
                    Double id2 = (Double)cmd4.ExecuteScalar();
                    textBox1.Text = Convert.ToString(id2);
                    cmd4.Dispose();
                    conn4.Close();

                    Edit_Columns();

                    MessageBox.Show("Удаление прошло успешно!");
                    dataGridView1.Focus();
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
            int ind = dataGridView1.CurrentRow.Index;
            tbidtovar.Text = dataGridView1.Rows[ind].Cells[0].Value.ToString();
        }

        private void yt_Button3_Click(object sender, EventArgs e)
        {
            if (textBox15.Text != "")
            {
                DialogResult result = MessageBox.Show("Отменить добавление накладной?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM InvoiceTables WHERE id_InvoiceHeader = @idinvoiceheader AND Type = 2";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
                    cmd.ExecuteNonQuery();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmd.Dispose();
                    conn.Close();

                    string query1 = "DELETE FROM InvoiceHeaders WHERE id_InvoiceHeader = @idinvoiceheader";
                    SQLiteConnection conn1 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn1.Open();
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn1);
                    cmd1.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
                    cmd1.ExecuteNonQuery();
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    cmd.Dispose();
                    conn1.Close();

                    textBox7.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    textBox4.Enabled = false;
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    textBox15.Clear();
                    yt_Button7.Enabled = false;
                    yt_Button8.Enabled = false;
                    this.Hide();
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                textBox7.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox15.Clear();
                yt_Button7.Enabled = false;
                yt_Button8.Enabled = false;
                this.Hide();
            }
            
        }

        private void yt_Button1_Click(object sender, EventArgs e)
        {
            string query = "UPDATE InvoiceHeaders SET Sum = @sum WHERE id_InvoiceHeader = @idinvoiceheader";
            SQLiteConnection conn6 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn6.Open();
            SQLiteCommand cmd6 = new SQLiteCommand(query, conn6);
            cmd6.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
            cmd6.Parameters.AddWithValue("@sum", Convert.ToDouble(textBox1.Text));
            cmd6.ExecuteNonQuery();
            cmd6.Dispose();
            conn6.Close();
        }

        private void FormInvoiceAddRas_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form2 main = this.Owner as Form2;
            if (main != null)
            {
                main.checkBox1.Checked = false;
                main.dateTimePicker1.Enabled = false;
                main.dateTimePicker2.Enabled = false;
                string query = "SELECT InvoiceHeaders.id_InvoiceHeader, InvoiceTypes.Name AS Тип, Providers.Name AS Контрагент, " +
                    "Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y', Date) AS Дата, Sum || ' руб'  AS Сумма FROM InvoiceHeaders " +
                    "JOIN Providers ON InvoiceHeaders.id_Provider = Providers.id_Provider " +
                    "JOIN InvoiceTypes ON InvoiceHeaders.id_InvoiceType = InvoiceTypes.id_InvoiceTypes " +
                    "JOIN Employees ON InvoiceHeaders.id_Employee = Employees.id_Employee";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                main.dataGridView2.DataSource = dt;
                main.dataGridView2.Columns[0].Visible = false;
                cmd.Dispose();
                conn.Close();
                main.dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                main.dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                main.dataGridView2.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                main.dataGridView2.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                main.dataGridView2.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                main.dataGridView2.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                main.dataGridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                main.dataGridView2.Select();
                main.label71.Text = main.dataGridView2.Rows.Count.ToString();

                if (main.dataGridView2.Rows.Count < 2)
                {
                    string query1 = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
                        "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
                        "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
                        "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
                        " strftime('%d.%m.%Y',SrokGodnosti) AS [Срок годности до], Price|| ' руб' AS Цена, Summ|| ' руб' AS Сумма FROM InvoiceTables " +
                        " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
                        " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
                        " WHERE Type = 1 OR Type = 2";
                    SQLiteCommand cmd3 = new SQLiteCommand(query1, conn);
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    main.dataGridView4.DataSource = dt3;
                    main.dataGridView4.Columns[0].Visible = false;
                    cmd3.Dispose();
                    conn.Close();

                    main.dataGridView4.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    main.dataGridView4.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    main.dataGridView4.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    main.dataGridView4.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    main.dataGridView4.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    main.dataGridView4.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    main.dataGridView4.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    main.dataGridView4.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                    main.dataGridView4.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    main.dataGridView4.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    main.dataGridView4.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    main.dataGridView4.Select();
                }
            }
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Clear();
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (textBox4.Text == "0")
                textBox4.Clear();
        }

        private void Load_Data()
        {
            string query1 = "SELECT id_InvoiceTable, ProductGroups.Name AS Группа, " +
    "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
    "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
    "  Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
    " SrokGodnosti AS [Срок годности до], Price || ' руб' AS Цена, Summ || ' руб' AS Сумма  FROM InvoiceTables " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " WHERE id_InvoiceHeader = @idinvoiceheader";
            SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn2.Open();
            SQLiteCommand cmd3 = new SQLiteCommand(query1, conn2);
            cmd3.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
            SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;
            dataGridView1.Columns[0].Visible = false;
            cmd3.Dispose();

            string query4 = "SELECT SUM(Summ) AS Summa FROM InvoiceTables WHERE id_InvoiceHeader = @idinvoiceheader";
            SQLiteCommand cmd4 = new SQLiteCommand(query4, conn2);
            cmd4.Parameters.AddWithValue("@idinvoiceheader", Convert.ToInt32(textBox15.Text));
            SQLiteDataAdapter da4 = new SQLiteDataAdapter(cmd4);
            Double id2 = (Double)cmd4.ExecuteScalar();
            textBox1.Text = Convert.ToString(id2);
            cmd4.Dispose();

            string query9 = "SELECT id_InvoiceTable, InvoiceTables.id_InvoiceHeader, Nomenclatures.id_Nomenclature, IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Articul, '' )" +
                "||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie, " +
"Amount, InvoiceTables.EdIzm FROM InvoiceTables " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup = ProductGroups.id_ProductGroup " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature = Nomenclatures.id_Nomenclature " +
" WHERE Amount>0 AND InvoiceTables.Type = 3";
            SQLiteCommand cmd9 = new SQLiteCommand(query9, conn2);
            SQLiteDataAdapter da9 = new SQLiteDataAdapter(cmd9);
            DataTable dt9 = new DataTable();
            da9.Fill(dt9);

            comboBox4.DataSource = dt9;
            comboBox4.DisplayMember = "Nazvanie";
            comboBox4.ValueMember = "id_InvoiceTable";
            comboBox4.SelectedIndex = -1;
            comboBox4.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox4.AutoCompleteSource = AutoCompleteSource.ListItems;


            comboBox9.DataSource = dt9;
            comboBox9.DisplayMember = "Amount";
            comboBox9.ValueMember = "id_InvoiceTable";
            comboBox9.SelectedIndex = -1;

            comboBox5.DataSource = dt9;
            comboBox5.DisplayMember = "EdIzm";
            comboBox5.ValueMember = "id_InvoiceTable";
            comboBox5.SelectedIndex = -1;

            cbidNomen.DataSource = dt9;
            cbidNomen.DisplayMember = "id_Nomenclature";
            cbidNomen.ValueMember = "id_InvoiceTable";
            cbidNomen.SelectedIndex = -1;

            cbInvoiceTable.DataSource = dt9;
            cbInvoiceTable.DisplayMember = "id_InvoiceTable";
            cbInvoiceTable.ValueMember = "id_InvoiceTable";
            dataGridView1.Select();
            conn2.Close();

            Edit_Columns();

            MessageBox.Show("Товар успешно добавлен!");
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            yt_Button7.Enabled = false;
            yt_Button8.Enabled = false;
            textBox6.Enabled = false;

            yt_Button4.Enabled = true;
            yt_Button5.Enabled = true;
            yt_Button2.Enabled = true;
            yt_Button3.Enabled = true;
            yt_Button7.Visible = false;
            yt_Button8.Visible = false;

            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
        }

        private void Edit_Columns()
        {
            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }
}
