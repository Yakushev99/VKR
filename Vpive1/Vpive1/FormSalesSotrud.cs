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
    public partial class FormSalesSotrud : Form
    {
        public FormSalesSotrud()
        {
            InitializeComponent();
        }

        private void FormSalesSotrud_Load(object sender, EventArgs e)
        {


        }

        private void yt_Button1_Click(object sender, EventArgs e)
        {
            this.Employees1TableAdapter.Fill(this.dbDataSet.Employees1, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date.AddDays(1));
            this.EmployeesFirstDateTableAdapter.Fill(this.dbDataSet.EmployeesFirstDate, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date.AddDays(1));
            this.EmployeesLastDateTableAdapter.Fill(this.dbDataSet.EmployeesLastDate, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date.AddDays(1));

            this.reportViewer1.RefreshReport();
        }
    }
}
