
namespace Vpive1
{
    partial class FormProductGroups
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProductGroups));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.yt_Button6 = new Vpive1.yt_Button();
            this.yt_Button5 = new Vpive1.yt_Button();
            this.yt_Button4 = new Vpive1.yt_Button();
            this.yt_Button3 = new Vpive1.yt_Button();
            this.yt_Button2 = new Vpive1.yt_Button();
            this.yt_Button1 = new Vpive1.yt_Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Size = new System.Drawing.Size(311, 193);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(420, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 19);
            this.label1.TabIndex = 4;
            this.label1.Text = "Группа";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(329, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Наименование";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(444, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(113, 23);
            this.textBox1.TabIndex = 6;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(519, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(38, 20);
            this.textBox2.TabIndex = 9;
            this.textBox2.Visible = false;
            // 
            // yt_Button6
            // 
            this.yt_Button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(54)))));
            this.yt_Button6.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yt_Button6.ForeColor = System.Drawing.Color.Black;
            this.yt_Button6.Location = new System.Drawing.Point(468, 221);
            this.yt_Button6.Name = "yt_Button6";
            this.yt_Button6.Size = new System.Drawing.Size(93, 36);
            this.yt_Button6.TabIndex = 10;
            this.yt_Button6.Text = "Выход";
            this.yt_Button6.Click += new System.EventHandler(this.yt_Button6_Click);
            // 
            // yt_Button5
            // 
            this.yt_Button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(54)))));
            this.yt_Button5.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yt_Button5.ForeColor = System.Drawing.Color.Black;
            this.yt_Button5.Location = new System.Drawing.Point(468, 166);
            this.yt_Button5.Name = "yt_Button5";
            this.yt_Button5.Size = new System.Drawing.Size(93, 36);
            this.yt_Button5.TabIndex = 8;
            this.yt_Button5.Text = "Отмена";
            this.yt_Button5.Visible = false;
            this.yt_Button5.Click += new System.EventHandler(this.yt_Button5_Click);
            // 
            // yt_Button4
            // 
            this.yt_Button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(54)))));
            this.yt_Button4.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yt_Button4.ForeColor = System.Drawing.Color.Black;
            this.yt_Button4.Location = new System.Drawing.Point(332, 166);
            this.yt_Button4.Name = "yt_Button4";
            this.yt_Button4.Size = new System.Drawing.Size(93, 36);
            this.yt_Button4.TabIndex = 7;
            this.yt_Button4.Text = "Сохранить";
            this.yt_Button4.Visible = false;
            this.yt_Button4.Click += new System.EventHandler(this.yt_Button4_Click);
            // 
            // yt_Button3
            // 
            this.yt_Button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(54)))));
            this.yt_Button3.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yt_Button3.ForeColor = System.Drawing.Color.Black;
            this.yt_Button3.Location = new System.Drawing.Point(230, 221);
            this.yt_Button3.Name = "yt_Button3";
            this.yt_Button3.Size = new System.Drawing.Size(93, 36);
            this.yt_Button3.TabIndex = 3;
            this.yt_Button3.Text = "Удалить";
            this.yt_Button3.Click += new System.EventHandler(this.yt_Button3_Click);
            // 
            // yt_Button2
            // 
            this.yt_Button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(54)))));
            this.yt_Button2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yt_Button2.ForeColor = System.Drawing.Color.Black;
            this.yt_Button2.Location = new System.Drawing.Point(121, 221);
            this.yt_Button2.Name = "yt_Button2";
            this.yt_Button2.Size = new System.Drawing.Size(93, 36);
            this.yt_Button2.TabIndex = 2;
            this.yt_Button2.Text = "Изменить";
            this.yt_Button2.Click += new System.EventHandler(this.yt_Button2_Click);
            // 
            // yt_Button1
            // 
            this.yt_Button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(54)))));
            this.yt_Button1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yt_Button1.ForeColor = System.Drawing.Color.Black;
            this.yt_Button1.Location = new System.Drawing.Point(12, 221);
            this.yt_Button1.Name = "yt_Button1";
            this.yt_Button1.Size = new System.Drawing.Size(93, 36);
            this.yt_Button1.TabIndex = 1;
            this.yt_Button1.Text = "Добавить";
            this.yt_Button1.Click += new System.EventHandler(this.yt_Button1_Click);
            // 
            // FormProductGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(569, 268);
            this.Controls.Add(this.yt_Button6);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.yt_Button5);
            this.Controls.Add(this.yt_Button4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.yt_Button3);
            this.Controls.Add(this.yt_Button2);
            this.Controls.Add(this.yt_Button1);
            this.Controls.Add(this.dataGridView1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormProductGroups";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Группы товаров";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProductGroups_FormClosing);
            this.Load += new System.EventHandler(this.FormProductGroups_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private yt_Button yt_Button1;
        private yt_Button yt_Button2;
        private yt_Button yt_Button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private yt_Button yt_Button4;
        private yt_Button yt_Button5;
        private System.Windows.Forms.TextBox textBox2;
        private yt_Button yt_Button6;
    }
}