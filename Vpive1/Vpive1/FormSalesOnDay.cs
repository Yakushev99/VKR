using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Microsoft.Reporting.Map.WebForms;
using System.IO;

namespace Vpive1
{
    public partial class FormSalesOnDay : Form
    {
        public FormSalesOnDay()
        {
            InitializeComponent();
        }
        protected void Export(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = reportViewer1.LocalReport.Render(
               "EXCELOPENXML", null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            FileStream fs = new FileStream(@"Report.xlsx",
               FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        private void FormSalesOnDay_Load(object sender, EventArgs e)
        {


        }

        private void yt_Button1_Click(object sender, EventArgs e)
        {
            reportViewer1.Name = "Report";
            this.DataTable1TableAdapter.Fill(this.dbDataSet.DataTable1, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date.AddDays(1));
            this.Payments1TableAdapter.Fill(this.dbDataSet.Payments1, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date.AddDays(1));
            this.PaymentsFirstDateTableAdapter.Fill(this.dbDataSet.PaymentsFirstDate, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date.AddDays(1));
            this.PaymentsLastDateTableAdapter.Fill(this.dbDataSet.PaymentsLastDate, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date.AddDays(1));
            this.reportViewer1.RefreshReport();
            //reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            yt_Button2.Enabled = true;
        }

        private void yt_Button2_Click(object sender, EventArgs e)
        {
                Export(sender, e);
                try
                {
                SendMail("smtp.mail.ru", "vpive.magazin@mail.ru", "D6FLEQ58PNM3gvFRfHPe", Properties.Settings.Default.Email, "Отчет по продажам в магазине", "Отчет по продажам");
                MessageBox.Show("Отчет успешно отправлен!");
                }
                catch
                {
                    MessageBox.Show("Невозможно отправить отчет! Проверьте правильность введенного Email в настройках!", "Ошибка!");
                }
                yt_Button2.Enabled = false; 
        }

        public static void SendMail(string smtpServer, string from, string password,
string mailto, string caption, string message, string attachFile = "Report.xlsx")
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("vpive.magazin@mail.ru", password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                // client.Dispose();
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }
    }
}
