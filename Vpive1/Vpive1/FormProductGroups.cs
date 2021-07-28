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
    public partial class FormProductGroups : Form
    {
        private int k = 0, ind;
        public FormProductGroups()
        {
            InitializeComponent();
        }

        private void FormProductGroups_Load(object sender, EventArgs e)
        {
            string query = "SELECT id_ProductGroup, Name AS Наименование FROM ProductGroups WHERE id_ProductGroup>1";
            SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.Dispose();
            conn.Close();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

        }

        private void yt_Button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
            k = 1;
            yt_Button4.Visible = true;
            yt_Button5.Visible = true;
            yt_Button1.Enabled = false;
            yt_Button2.Enabled = false;
            yt_Button3.Enabled = false;
            textBox1.Enabled = true;
            textBox1.Clear();
        }

        private void yt_Button2_Click(object sender, EventArgs e)
        {
            k = 2;
            yt_Button4.Visible = true;
            yt_Button5.Visible = true;
            yt_Button1.Enabled = false;
            yt_Button2.Enabled = false;
            yt_Button3.Enabled = false;
            textBox1.Enabled = true;
        }

        private void yt_Button5_Click(object sender, EventArgs e)
        {
            yt_Button4.Visible = false;
            yt_Button5.Visible = false;
            yt_Button1.Enabled = true;
            yt_Button2.Enabled = true;
            yt_Button3.Enabled = true;
            textBox1.Enabled = false;
        }

        private void yt_Button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void yt_Button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("Не заполнено наименование!", "Ошибка");
            else if (k == 1)
            {
                string query = "INSERT INTO ProductGroups (Name) VALUES (@name)";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                cmd.Dispose();

                string query1 = "SELECT id_ProductGroup, Name AS Наименование FROM ProductGroups WHERE id_ProductGroup>1";
                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                dataGridView1.DataSource = dt1;
                cmd1.Dispose();
                conn.Close();
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                dataGridView1.Focus();

                yt_Button4.Visible = false;
                yt_Button5.Visible = false;
                yt_Button1.Enabled = true;
                yt_Button2.Enabled = true;
                yt_Button3.Enabled = true;
                textBox1.Enabled = false;
            }
            else if (k == 2)
            {
                string query = "UPDATE ProductGroups SET Name = @name WHERE id_ProductGroup = @idproductgroup";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                cmd.Parameters.AddWithValue("@idproductgroup", Convert.ToInt32(textBox2.Text));
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                cmd.Dispose();
                conn.Close();

                string query1 = "SELECT id_ProductGroup, Name AS Наименование FROM ProductGroups WHERE id_ProductGroup>1";
                SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                dataGridView1.DataSource = dt1;
                cmd1.Dispose();
                conn.Close();
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                dataGridView1.Focus();

                yt_Button4.Visible = false;
                yt_Button5.Visible = false;
                yt_Button1.Enabled = true;
                yt_Button2.Enabled = true;
                yt_Button3.Enabled = true;
                textBox1.Enabled = false;
            }
        }

        private void yt_Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Focused == false)
                MessageBox.Show("Не выбрана запись для удаления!", "Ошибка");
            else if (dataGridView1.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Удалить запись?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM ProductGroups WHERE id_ProductGroup = @idproductgroup";
                    SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idproductgroup", Convert.ToInt32(textBox2.Text));
                    cmd.ExecuteNonQuery();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    cmd.Dispose();
                    conn.Close();

                    string query1 = "SELECT id_ProductGroup, Name AS Наименование FROM ProductGroups WHERE id_ProductGroup>1";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
                    SQLiteDataAdapter da1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    dataGridView1.DataSource = dt1;
                    cmd1.Dispose();
                    conn.Close();
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                    MessageBox.Show("Удаление прошло успешно!");
                    dataGridView1.Select();
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void FormProductGroups_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form2 main = this.Owner as Form2;
            if (main != null)
            {
                string query = "SELECT id_ProductGroup, Name FROM ProductGroups";
                SQLiteConnection conn = new SQLiteConnection("Data source=Vpive.db;Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                main.cbGroup.DataSource = dt;
                main.cbGroup.DisplayMember = "Name";
                main.cbGroup.ValueMember = "id_ProductGroup";
                main.cbGroup.SelectedIndex = 0;
                main.comboBox1.DataSource = dt;
                main.comboBox1.DisplayMember = "id_ProductGroup";
                main.comboBox1.ValueMember = "id_ProductGroup";
                cmd.Dispose();
                conn.Close();
            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            ind = dataGridView1.CurrentRow.Index;
            textBox2.Text = dataGridView1.Rows[ind].Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.Rows[ind].Cells[1].Value.ToString();

        }
    }
}
