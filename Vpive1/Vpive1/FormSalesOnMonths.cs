using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vpive1
{
    public partial class FormSalesOnMonths : Form
    {
        public FormSalesOnMonths()
        {
            InitializeComponent();
        }

        private void FormSalesOnMonths_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSet.SalesOnMonths". При необходимости она может быть перемещена или удалена.

        }

        private void yt_Button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                this.SalesOnMonthsTableAdapter.Fill(this.dbDataSet.SalesOnMonths, Convert.ToDateTime("2021-01-01"), Convert.ToDateTime("2022-01-01"));
                this.reportViewer1.RefreshReport();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                this.SalesOnMonthsTableAdapter.Fill(this.dbDataSet.SalesOnMonths, Convert.ToDateTime("2022-01-01"), Convert.ToDateTime("2023-01-01"));
                this.reportViewer1.RefreshReport();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                this.SalesOnMonthsTableAdapter.Fill(this.dbDataSet.SalesOnMonths, Convert.ToDateTime("2023-01-01"), Convert.ToDateTime("2024-01-01"));
                this.reportViewer1.RefreshReport();
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                this.SalesOnMonthsTableAdapter.Fill(this.dbDataSet.SalesOnMonths, Convert.ToDateTime("2024-01-01"), Convert.ToDateTime("2025-01-01"));
                this.reportViewer1.RefreshReport();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                this.SalesOnMonthsTableAdapter.Fill(this.dbDataSet.SalesOnMonths, Convert.ToDateTime("2025-01-01"), Convert.ToDateTime("2026-01-01"));
                this.reportViewer1.RefreshReport();
            }
        }
    }
}
