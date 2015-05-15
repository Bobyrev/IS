using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MIS
{
    public partial class Form2 : Form
    {
        Form1 form1;
        SqlConnection connect;

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (form1.Visible == false)
            { 
                form1.Close(); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.Length == 0) 
            {
                label3.Text = "Введите логин.";
                return;
            }
            if (textBox2.Text.Length == 0)
            {
                label3.Text = "Введите пароль.";
                return;
            }

            string str = @"Data Source=CEPEGGA-ПК\SQLEXPRESS;
                           Initial Catalog=Med;
                           Integrated Security=True";
            connect = new SqlConnection(str);
            connect.Open();

            SqlCommand cmd = new SqlCommand(String.Format("SELECT passwd FROM Login WHERE login = '{0}'", textBox1.Text.Trim()), connect);
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                if (sdr.Read())
                {
                    if (sdr[0].ToString() == textBox2.Text.Trim())
                    {
                        sdr.Close();
                        form1.connect = this.connect;
                        form1.Load_Spetialization_Doctors();
                        form1.Visible = true;
                        this.Close();
                    }
                    else
                    {
                        label3.Text = "Неверный логин или пароль.";
                    }
                }
                else
                {
                    label3.Text = "Неверный логин или пароль.";
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
