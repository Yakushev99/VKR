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
    public partial class FormSales : Form
    {
        private int k;
        public FormSales(int k)
        {
            this.k = k;
            InitializeComponent();
        }

        private void FormSales_Load(object sender, EventArgs e)
        {
            if (k == 2)
            {
                yt_Button2.Visible = false;
                string query = "SELECT Realisations.id_Realisation,  Clients.FirstName ||' '|| Clients.LastName AS Клиент, " +
        "Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y %H:%M:%S', Payments.Date) AS Дата, Payments.Type AS [Тип оплаты], Payments.Sum || ' руб' AS Сумма FROM Realisations " +
        "JOIN Clients ON Clients.id_Client = Realisations.id_Client " +
        "JOIN Payments ON Realisations.id_Realisation = Payments.id_Realisation " +
        "JOIN Employees ON Realisations.id_Employee = Employees.id_Employee " +
        " WHERE Payments.Date BETWEEN @date1 AND  @date2 ORDER BY Payments.Date";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@date1", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@date2", dateTimePicker2.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
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
                dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Select();
                label4.Text = dataGridView1.Rows.Count.ToString();
            }
            else
            {
                string query = "SELECT Realisations.id_Realisation,  Clients.FirstName ||' '|| Clients.LastName AS Клиент, " +
                    "Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y %H:%M:%S', Payments.Date) AS Дата, Payments.Type AS [Тип оплаты], Payments.Sum || ' руб' AS Сумма FROM Realisations " +
                    "JOIN Clients ON Clients.id_Client = Realisations.id_Client " +
                    "JOIN Payments ON Realisations.id_Realisation = Payments.id_Realisation " +
                    "JOIN Employees ON Realisations.id_Employee = Employees.id_Employee " +
                    " WHERE Payments.Date BETWEEN @date1 AND  @date2 ORDER BY Payments.Date";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@date1", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@date2", dateTimePicker2.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
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
                dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Select();
                if (dataGridView1.Rows.Count < 1)
                    yt_Button2.Enabled = false;
                else yt_Button2.Enabled = true;
                label4.Text = dataGridView1.Rows.Count.ToString();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int ind = dataGridView1.CurrentRow.Index;
            textBox1.Text = dataGridView1.Rows[ind].Cells[0].Value.ToString();

            string query1 = "SELECT RealisationOnNomenclatures.id_RealisationOnNomenclature, RealisationOnNomenclatures.id_InvoiceTable, RealisationOnNomenclatures.id_Realisation, ProductGroups.Name AS Группа, " +
"IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
" RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
" RealisationOnNomenclatures.Price || ' руб' AS [По цене], Summa || ' руб' AS Сумма FROM RealisationOnNomenclatures " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN InvoiceTables ON RealisationOnNomenclatures.id_InvoiceTable=InvoiceTables.id_InvoiceTable " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE id_Realisation = @idrealisation ORDER BY ProductGroups.Name DESC, Summa DESC";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd3 = new SQLiteCommand(query1, conn);
            cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox1.Text));
            SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView2.DataSource = dt3;
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
            dataGridView2.Columns[2].Visible = false;
            cmd3.Dispose();
            conn.Close();

            dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Select();
            conn.Close();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string query = "SELECT Realisations.id_Realisation,  Clients.FirstName ||' '|| Clients.LastName AS Клиент, " +
    "Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y %H:%M:%S', Payments.Date) AS Дата, Payments.Type AS [Тип оплаты], Payments.Sum || ' руб' AS Сумма FROM Realisations " +
    "JOIN Clients ON Clients.id_Client = Realisations.id_Client " +
    "JOIN Payments ON Realisations.id_Realisation = Payments.id_Realisation " +
    "JOIN Employees ON Realisations.id_Employee = Employees.id_Employee " +
    " WHERE Payments.Date BETWEEN @date1 AND  @date2 ORDER BY Payments.Date";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@date1", dateTimePicker1.Value.Date);
            cmd.Parameters.AddWithValue("@date2", dateTimePicker2.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
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
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Select();
            label4.Text = dataGridView1.Rows.Count.ToString();
            if (dataGridView1.Rows.Count < 1)
                yt_Button2.Enabled = false;
            else yt_Button2.Enabled = true;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            string query = "SELECT Realisations.id_Realisation,  Clients.FirstName ||' '|| Clients.LastName AS Клиент, " +
    "Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y %H:%M:%S', Payments.Date) AS Дата, Payments.Type AS [Тип оплаты], Payments.Sum || ' руб' AS Сумма FROM Realisations " +
    "JOIN Clients ON Clients.id_Client = Realisations.id_Client " +
    "JOIN Payments ON Realisations.id_Realisation = Payments.id_Realisation " +
    "JOIN Employees ON Realisations.id_Employee = Employees.id_Employee " +
   " WHERE Payments.Date BETWEEN @date1 AND  @date2 ORDER BY Payments.Date";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@date1", dateTimePicker1.Value.Date);
            cmd.Parameters.AddWithValue("@date2", dateTimePicker2.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
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
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Select();
            label4.Text = dataGridView1.Rows.Count.ToString();
            if (dataGridView1.Rows.Count < 1)
                yt_Button2.Enabled = false;
            else yt_Button2.Enabled = true;
        }

        private void yt_Button1_Click(object sender, EventArgs e)
        {
            FormSalesOnDay formsod = new FormSalesOnDay();
            formsod.Show();
        }

        private void yt_Button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Удалить продажу?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int rows = dataGridView2.Rows.Count;
                for (int i = 0; i <= rows; i++)
                {
                    yt_Button3_Click(sender, e);
                }
                string query4 = "SELECT Realisations.id_Realisation,  Clients.FirstName ||' '|| Clients.LastName AS Клиент, " +
"Employees.FirstName ||' '|| Employees.LastName AS Сотрудник, strftime('%d.%m.%Y %H:%M:%S', Payments.Date) AS Дата, Payments.Type AS [Тип оплаты], Payments.Sum || ' руб' AS Сумма FROM Realisations " +
"JOIN Clients ON Clients.id_Client = Realisations.id_Client " +
"JOIN Payments ON Realisations.id_Realisation = Payments.id_Realisation " +
"JOIN Employees ON Realisations.id_Employee = Employees.id_Employee " +
" WHERE Payments.Date BETWEEN @date1 AND  @date2 ORDER BY Payments.Date";
                SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn2.Open();
                SQLiteCommand cmd4 = new SQLiteCommand(query4, conn2);
                cmd4.Parameters.AddWithValue("@date1", dateTimePicker1.Value.Date);
                cmd4.Parameters.AddWithValue("@date2", dateTimePicker2.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
                SQLiteDataAdapter da4 = new SQLiteDataAdapter(cmd4);
                DataTable dt4 = new DataTable();
                da4.Fill(dt4);
                dataGridView1.DataSource = dt4;
                dataGridView1.Columns[0].Visible = false;
                cmd4.Dispose();
                conn2.Close();
                dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Select();
                if (dataGridView1.Rows.Count < 1)
                    yt_Button2.Enabled = false;
                else yt_Button2.Enabled = true;
            }
            else if (result == DialogResult.No)
                return;
        }

        private void yt_Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 1)
            {
                string query13 = "UPDATE InvoiceTables SET Amount = Amount + @amount " +
                "WHERE id_InvoiceTable = @idinvoicetable";
                SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn2.Open();
                SQLiteCommand cmd13 = new SQLiteCommand(query13, conn2);
                cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(textBox2.Text));
                cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox3.Text));
                cmd13.ExecuteNonQuery();
                cmd13.Dispose();

                string query = "DELETE FROM RealisationOnNomenclatures WHERE id_RealisationOnNomenclature = @idrealisationonnomen";
                SQLiteCommand cmd = new SQLiteCommand(query, conn2);
                cmd.Parameters.AddWithValue("@idrealisationonnomen", Convert.ToInt32(textBox4.Text));
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmd.Dispose();

                string query3 = "SELECT RealisationOnNomenclatures.id_RealisationOnNomenclature, RealisationOnNomenclatures.id_InvoiceTable, RealisationOnNomenclatures.id_Realisation, ProductGroups.Name AS Группа, " +
    "IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Наименование," +
    " RealisationOnNomenclatures.Amount AS Количество, InvoiceTables.EdIzm AS [Ед измерения]," +
    " RealisationOnNomenclatures.Price || ' руб' AS [По цене], Summa || ' руб' AS Сумма FROM RealisationOnNomenclatures " +
    " JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
    " JOIN InvoiceTables ON RealisationOnNomenclatures.id_InvoiceTable=InvoiceTables.id_InvoiceTable " +
    " JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
    " WHERE id_Realisation = @idrealisation ORDER BY ProductGroups.Name DESC, Summa DESC";
                SQLiteCommand cmd3 = new SQLiteCommand(query3, conn2);
                cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox1.Text));
                SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                DataTable dt3 = new DataTable();
                da3.Fill(dt3);
                dataGridView2.DataSource = dt3;
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
                cmd3.Dispose();
                dataGridView2.Select();

                conn2.Close();

            }
            else if (dataGridView2.Rows.Count == 1)
            {
                string query13 = "UPDATE InvoiceTables SET Amount = Amount + @amount " +
"WHERE id_InvoiceTable = @idinvoicetable";
                SQLiteConnection conn2 = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn2.Open();
                SQLiteCommand cmd13 = new SQLiteCommand(query13, conn2);
                cmd13.Parameters.AddWithValue("@idinvoicetable", Convert.ToInt32(textBox2.Text));
                cmd13.Parameters.AddWithValue("@amount", Convert.ToDouble(textBox3.Text));
                cmd13.ExecuteNonQuery();
                cmd13.Dispose();

                string query = "DELETE FROM RealisationOnNomenclatures WHERE id_RealisationOnNomenclature = @idrealisationonnomen";
                SQLiteCommand cmd = new SQLiteCommand(query, conn2);
                cmd.Parameters.AddWithValue("@idrealisationonnomen", Convert.ToInt32(textBox4.Text));
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmd.Dispose();

                string query1 = "DELETE FROM Realisations WHERE id_Realisation = @idrealisation";

                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn2);
                cmd1.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox1.Text));
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
                cmd3.Parameters.AddWithValue("@idrealisation", Convert.ToInt32(textBox1.Text));
                SQLiteDataAdapter da3 = new SQLiteDataAdapter(cmd3);
                DataTable dt3 = new DataTable();
                da3.Fill(dt3);
                dataGridView2.DataSource = dt3;
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
                cmd3.Dispose();
                dataGridView2.Select();

                dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView2.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView2.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                textBox1.Clear();
            }

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            int ind;
            ind = dataGridView2.CurrentRow.Index;
            textBox2.Text = dataGridView2.Rows[ind].Cells[1].Value.ToString();
            textBox3.Text = dataGridView2.Rows[ind].Cells[5].Value.ToString();
            textBox4.Text = dataGridView2.Rows[ind].Cells[0].Value.ToString();
        }

        private void FormSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form2 main = this.Owner as Form2;
            if (main != null)
            {
                main.dateTimePicker5.Value = DateTime.Now.Date;
                string query = "SELECT id_InvoiceTable, InvoiceTables.id_Nomenclature AS Nomen, ProductGroups.Name, " +
" IFNULL(Nomenclatures.Name, '')||' '||IFNULL(Nomenclatures.Articul, '')" +
"||' '||IFNULL(Nomenclatures.Weight, '')||' '||IFNULL(Nomenclatures.EdIzm, '') AS Nazvanie," +
" PriceSale, Amount, InvoiceTables.EdIzm AS EdIzm," +
" strftime('%d.%m.%Y', SrokGodnosti) FROM InvoiceTables " +
" JOIN Nomenclatures ON InvoiceTables.id_Nomenclature=Nomenclatures.id_Nomenclature " +
" JOIN ProductGroups ON Nomenclatures.id_ProductGroup=ProductGroups.id_ProductGroup " +
" WHERE Type = 3 AND (ProductGroups.Name = 'Напитки' OR id_InvoiceTable=1) ORDER BY Nazvanie";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                main.comboBox6.DataSource = dt;
                main.comboBox6.DisplayMember = "Nazvanie";
                main.comboBox6.ValueMember = "id_InvoiceTable";
                main.comboBox6.SelectedIndex = -1;

                main.comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                main.comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;


                main.comboBox12.DataSource = dt;
                main.comboBox12.DisplayMember = "id_InvoiceTable";
                main.comboBox12.ValueMember = "id_InvoiceTable";
                main.comboBox12.SelectedIndex = -1;

                main.comboBox2.DataSource = dt;
                main.comboBox2.DisplayMember = "Amount";
                main.comboBox2.ValueMember = "id_InvoiceTable";
                main.comboBox2.SelectedIndex = -1;


                main.comboBox15.DataSource = dt;
                main.comboBox15.DisplayMember = "PriceSale";
                main.comboBox15.ValueMember = "id_InvoiceTable";
                main.comboBox15.SelectedIndex = -1;

                main.comboBox6.Text = "";
                main.comboBox2.Text = "";
                main.comboBox15.Text = "";
                cmd.Dispose();

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
                main.comboBox7.DataSource = dt1;
                main.comboBox7.DisplayMember = "Nazvanie";
                main.comboBox7.ValueMember = "id_InvoiceTable";
                main.comboBox7.SelectedIndex = -1;
                main.comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                main.comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;


                main.comboBox13.DataSource = dt1;
                main.comboBox13.DisplayMember = "id_InvoiceTable";
                main.comboBox13.ValueMember = "id_InvoiceTable";
                main.comboBox13.SelectedIndex = -1;

                main.comboBox17.DataSource = dt1;
                main.comboBox17.DisplayMember = "Amount";
                main.comboBox17.ValueMember = "id_InvoiceTable";
                main.comboBox17.SelectedIndex = -1;

                main.comboBox16.DataSource = dt1;
                main.comboBox16.DisplayMember = "PriceSale";
                main.comboBox16.ValueMember = "id_InvoiceTable";
                main.comboBox16.SelectedIndex = -1;
                main.textBox16.Text = main.comboBox16.Text;

                main.comboBox19.DataSource = dt1;
                main.comboBox19.DisplayMember = "EdIzm";
                main.comboBox19.ValueMember = "id_InvoiceTable";
                main.comboBox19.SelectedIndex = -1;

                cmd1.Dispose();
                conn.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                dateTimePicker1.Value = DateTime.Now.AddDays(1);
                dateTimePicker2.Value = DateTime.Now.AddMonths(12);
            }
            else
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                dateTimePicker1.Value = DateTime.Now.Date;
                dateTimePicker2.Value = DateTime.Now.Date; ;
            }
        }
    }
}
