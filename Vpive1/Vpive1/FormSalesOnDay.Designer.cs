
namespace Vpive1
{
    partial class FormSalesOnDay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSalesOnDay));
            this.DataTable1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dbDataSet = new Vpive1.dbDataSet();
            this.Payments1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PaymentsFirstDateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PaymentsLastDateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.yt_Button1 = new Vpive1.yt_Button();
            this.DataTable1TableAdapter = new Vpive1.dbDataSetTableAdapters.DataTable1TableAdapter();
            this.Payments1TableAdapter = new Vpive1.dbDataSetTableAdapters.Payments1TableAdapter();
            this.PaymentsFirstDateTableAdapter = new Vpive1.dbDataSetTableAdapters.PaymentsFirstDateTableAdapter();
            this.PaymentsLastDateTableAdapter = new Vpive1.dbDataSetTableAdapters.PaymentsLastDateTableAdapter();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.yt_Button2 = new Vpive1.yt_Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataTable1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Payments1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsFirstDateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsLastDateBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // DataTable1BindingSource
            // 
            this.DataTable1BindingSource.DataMember = "DataTable1";
            this.DataTable1BindingSource.DataSource = this.dbDataSet;
            // 
            // dbDataSet
            // 
            this.dbDataSet.DataSetName = "dbDataSet";
            this.dbDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Payments1BindingSource
            // 
            this.Payments1BindingSource.DataMember = "Payments1";
            this.Payments1BindingSource.DataSource = this.dbDataSet;
            // 
            // PaymentsFirstDateBindingSource
            // 
            this.PaymentsFirstDateBindingSource.DataMember = "PaymentsFirstDate";
            this.PaymentsFirstDateBindingSource.DataSource = this.dbDataSet;
            // 
            // PaymentsLastDateBindingSource
            // 
            this.PaymentsLastDateBindingSource.DataMember = "PaymentsLastDate";
            this.PaymentsLastDateBindingSource.DataSource = this.dbDataSet;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(89, 26);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(107, 23);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Период с";
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.DataTable1BindingSource;
            reportDataSource2.Name = "DataSet2";
            reportDataSource2.Value = this.Payments1BindingSource;
            reportDataSource3.Name = "DataSet3";
            reportDataSource3.Value = this.PaymentsFirstDateBindingSource;
            reportDataSource4.Name = "DataSet4";
            reportDataSource4.Value = this.PaymentsLastDateBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource4);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Vpive1.ReportSalesOnDay.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(-1, 69);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(889, 497);
            this.reportViewer1.TabIndex = 4;
            // 
            // yt_Button1
            // 
            this.yt_Button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(54)))));
            this.yt_Button1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yt_Button1.ForeColor = System.Drawing.Color.Black;
            this.yt_Button1.Location = new System.Drawing.Point(358, 17);
            this.yt_Button1.Name = "yt_Button1";
            this.yt_Button1.Size = new System.Drawing.Size(140, 38);
            this.yt_Button1.TabIndex = 2;
            this.yt_Button1.Text = "Сформировать";
            this.yt_Button1.Click += new System.EventHandler(this.yt_Button1_Click);
            // 
            // DataTable1TableAdapter
            // 
            this.DataTable1TableAdapter.ClearBeforeFill = true;
            // 
            // Payments1TableAdapter
            // 
            this.Payments1TableAdapter.ClearBeforeFill = true;
            // 
            // PaymentsFirstDateTableAdapter
            // 
            this.PaymentsFirstDateTableAdapter.ClearBeforeFill = true;
            // 
            // PaymentsLastDateTableAdapter
            // 
            this.PaymentsLastDateTableAdapter.ClearBeforeFill = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(202, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "по";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(233, 26);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(107, 23);
            this.dateTimePicker2.TabIndex = 6;
            // 
            // yt_Button2
            // 
            this.yt_Button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(54)))));
            this.yt_Button2.Enabled = false;
            this.yt_Button2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yt_Button2.ForeColor = System.Drawing.Color.Black;
            this.yt_Button2.Location = new System.Drawing.Point(641, 17);
            this.yt_Button2.Name = "yt_Button2";
            this.yt_Button2.Size = new System.Drawing.Size(140, 38);
            this.yt_Button2.TabIndex = 7;
            this.yt_Button2.Text = "Отправить отчет по Email";
            this.yt_Button2.Click += new System.EventHandler(this.yt_Button2_Click);
            // 
            // FormSalesOnDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.yt_Button2);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.yt_Button1);
            this.Controls.Add(this.dateTimePicker1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSalesOnDay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Продажи за день";
            this.Load += new System.EventHandler(this.FormSalesOnDay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataTable1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Payments1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsFirstDateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsLastDateBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private yt_Button yt_Button1;
        private System.Windows.Forms.Label label1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource DataTable1BindingSource;
        private dbDataSet dbDataSet;
        private System.Windows.Forms.BindingSource Payments1BindingSource;
        private System.Windows.Forms.BindingSource PaymentsFirstDateBindingSource;
        private System.Windows.Forms.BindingSource PaymentsLastDateBindingSource;
        private dbDataSetTableAdapters.DataTable1TableAdapter DataTable1TableAdapter;
        private dbDataSetTableAdapters.Payments1TableAdapter Payments1TableAdapter;
        private dbDataSetTableAdapters.PaymentsFirstDateTableAdapter PaymentsFirstDateTableAdapter;
        private dbDataSetTableAdapters.PaymentsLastDateTableAdapter PaymentsLastDateTableAdapter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private yt_Button yt_Button2;
    }
}