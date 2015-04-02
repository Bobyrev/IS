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
    public partial class Form1 : Form
    {

        public SqlConnection connect;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label8.Text = "";
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            this.Visible = false;
            form2.Show();
        }

        public void Load_Spetialization_Doctors()
        {
            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand(String.Format("SELECT DISTINCT Specialization FROM Doctors ORDER BY Specialization ASC"), connect);
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    comboBox1.Items.Add(sdr[0]);
                }
            }
        }

        private void Load_Name_Doctors()
        {
            comboBox2.Items.Clear();
            SqlCommand cmd = new SqlCommand(String.Format("SELECT last_name, first_name, patronymic FROM Doctors WHERE specialization = '{0}' AND Deleted IS NULL ORDER BY last_name ASC", comboBox1.Text), connect);
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    comboBox2.Items.Add(String.Format("{0} {1}.{2}.", sdr[0], sdr[1].ToString()[0], sdr[2].ToString()[0]));
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Name_Doctors();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            connect.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label8.ForeColor = Color.Black;
            if (textBox1.Text.Length == 0)
            {
                label8.Text = "";
            }
            else
            {
                if (textBox1.Text.Length == 24)
                {
                    label8.Text = "✓";
                    label8.ForeColor = Color.Green;
                    SqlCommand cmd = new SqlCommand(String.Format("SELECT last_name, first_name, patronymic FROM Clients WHERE polis = '{0}'", textBox1.Text), connect);
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.Read())
                        {
                            textBox2.Text = sdr[0].ToString();
                            textBox3.Text = sdr[1].ToString();
                            textBox4.Text = sdr[2].ToString();
                        }
                        else
                        {
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox4.Text = "";
                        }
                    }
                }
                else
                {
                    label8.Text = textBox1.Text.Length.ToString();
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void label10_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }

        private void label11_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length != 0)
            {
                label9.Visible = true;
            }
            else
            {
                label9.Visible = false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length != 0)
            {
                label10.Visible = true;
            }
            else
            {
                label10.Visible = false;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length != 0)
            {
                label11.Visible = true;
            }
            else
            {
                label11.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }
    }
}
