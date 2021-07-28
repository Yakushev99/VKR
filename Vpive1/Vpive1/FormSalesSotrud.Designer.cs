
namespace Vpive1
{
    partial class FormSalesSotrud
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSalesSotrud));
            this.Employees1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dbDataSet = new Vpive1.dbDataSet();
            this.EmployeesFirstDateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.EmployeesLastDateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.yt_Button1 = new Vpive1.yt_Button();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Employees1TableAdapter = new Vpive1.dbDataSetTableAdapters.Employees1TableAdapter();
            this.EmployeesFirstDateTableAdapter = new Vpive1.dbDataSetTableAdapters.EmployeesFirstDateTableAdapter();
            this.EmployeesLastDateTableAdapter = new Vpive1.dbDataSetTableAdapters.EmployeesLastDateTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.Employees1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmployeesFirstDateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmployeesLastDateBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // Employees1BindingSource
            // 
            this.Employees1BindingSource.DataMember = "Employees1";
            this.Employees1BindingSource.DataSource = this.dbDataSet;
            // 
            // dbDataSet
            // 
            this.dbDataSet.DataSetName = "dbDataSet";
            this.dbDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // EmployeesFirstDateBindingSource
            // 
            this.EmployeesFirstDateBindingSource.DataMember = "EmployeesFirstDate";
            this.EmployeesFirstDateBindingSource.DataSource = this.dbDataSet;
            // 
            // EmployeesLastDateBindingSource
            // 
            this.EmployeesLastDateBindingSource.DataMember = "EmployeesLastDate";
            this.EmployeesLastDateBindingSource.DataSource = this.dbDataSet;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.Employees1BindingSource;
            reportDataSource2.Name = "DataSet2";
            reportDataSource2.Value = this.EmployeesFirstDateBindingSource;
            reportDataSource3.Name = "DataSet3";
            reportDataSource3.Value = this.EmployeesLastDateBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Vpive1.ReportSotrud.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(-3, 65);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(890, 502);
            this.reportViewer1.TabIndex = 0;
            // 
            // yt_Button1
            // 
            this.yt_Button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(54)))));
            this.yt_Button1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yt_Button1.ForeColor = System.Drawing.Color.Black;
            this.yt_Button1.Location = new System.Drawing.Point(358, 17);
            this.yt_Button1.Name = "yt_Button1";
            this.yt_Button1.Size = new System.Drawing.Size(140, 38);
            this.yt_Button1.TabIndex = 10;
            this.yt_Button1.Text = "Сформировать";
            this.yt_Button1.Click += new System.EventHandler(this.yt_Button1_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(233, 26);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(107, 23);
            this.dateTimePicker2.TabIndex = 9;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(89, 26);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(108, 23);
            this.dateTimePicker1.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(202, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "по";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Период с";
            // 
            // Employees1TableAdapter
            // 
            this.Employees1TableAdapter.ClearBeforeFill = true;
            // 
            // EmployeesFirstDateTableAdapter
            // 
            this.EmployeesFirstDateTableAdapter.ClearBeforeFill = true;
            // 
            // EmployeesLastDateTableAdapter
            // 
            this.EmployeesLastDateTableAdapter.ClearBeforeFill = true;
            // 
            // FormSalesSotrud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.yt_Button1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reportViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSalesSotrud";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выручка по сотрудникам";
            this.Load += new System.EventHandler(this.FormSalesSotrud_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Employees1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmployeesFirstDateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmployeesLastDateBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource Employees1BindingSource;
        private dbDataSet dbDataSet;
        private dbDataSetTableAdapters.Employees1TableAdapter Employees1TableAdapter;
        private yt_Button yt_Button1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource EmployeesFirstDateBindingSource;
        private System.Windows.Forms.BindingSource EmployeesLastDateBindingSource;
        private dbDataSetTableAdapters.EmployeesFirstDateTableAdapter EmployeesFirstDateTableAdapter;
        private dbDataSetTableAdapters.EmployeesLastDateTableAdapter EmployeesLastDateTableAdapter;
    }
}