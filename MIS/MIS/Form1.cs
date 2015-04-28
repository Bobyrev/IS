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
            SqlCommand cmd = new SqlCommand(String.Format("SELECT Surname, Name, Twoname FROM Doctors WHERE specialization = '{0}' AND Del IS NULL", comboBox1.Text), connect);
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
                    SqlCommand cmd = new SqlCommand(String.Format("SELECT Surname, Name, Twoname FROM Clients WHERE Polis = '{0}'", textBox1.Text), connect);
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

        //--------------------Моя часть--------------------------------------

        List<string[]> work_data = new List<string[]>();
        private void ShowAll(DateTime day) 
        {
            work_data.Clear();
            SqlCommand cmd = new SqlCommand(string.Format("SELECT Doctors.ID, Work_days.ID, Doctors.Name + ' ' + Doctors.Surname, Work_days.Start_work, Work_days.End_work, Work_days.Cabinet "+
                                            "FROM Work_days "+
                                            "INNER JOIN Doctors ON Doctors.ID = Work_days.Doctor_ID "+
                                            "WHERE Work_days.Del IS NULL AND Work_days.Day = '{0}'",day.ToString("dd-MM-yyyy")),connect);
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    work_data.Add(new string[] { sdr[0].ToString(), sdr[1].ToString(), sdr[2].ToString(), sdr[3].ToString(), sdr[4].ToString(), sdr[5].ToString() });
                }
            }
            dataGridView1.Rows.Clear();
            foreach (string[] elem in work_data) 
            {
                dataGridView1.Rows.Add(elem[2], elem[3], elem[4], elem[5]);
            }
            del = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {//Отображение
            ShowAll(dateTimePicker2.Value);
        }


        List<string[]> doctors = new List<string[]>();
        bool del = false;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {//При изменении строки
            if (del) return; 
            SqlCommand cmd = new SqlCommand("SELECT Doctors.ID, Doctors.Surname FROM Doctors",connect);
            doctors.Clear();
            comboBox3.Items.Clear();
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    doctors.Add(new string[] { sdr[0].ToString(), sdr[1].ToString() });
                    comboBox3.Items.Add(sdr[1].ToString());
                }
            }
            int ind = dataGridView1.CurrentRow.Index;
            if (work_data.Count != 0)
            {
                for (int i = 0; i < doctors.Count; i++)
                {
                    if (doctors[i][0] == work_data[ind][0])
                    {
                        comboBox3.Text = doctors[i][1];
                        break;
                    }
                }
                textBox5.Text = work_data[ind][5];
                string[] work_start = work_data[ind][3].Split(':');
                numericUpDown1.Value = int.Parse(work_start[0]);
                numericUpDown2.Value = int.Parse(work_start[1]);
                string[] work_end = work_data[ind][4].Split(':');
                numericUpDown4.Value = int.Parse(work_end[0]);
                numericUpDown3.Value = int.Parse(work_end[1]);
            }
            else
            {
                comboBox3.Text = doctors[0][1];
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            ShowAll(dateTimePicker2.Value);
        }

        private void button6_Click(object sender, EventArgs e)
        {//Добавление
            if (comboBox3.Text == "" && textBox5.Text == "" && numericUpDown1.Value >= numericUpDown4.Value)
                return;
            string doc = "";
            for (int i = 0; i < doctors.Count; i++) 
            {
                if (doctors[i][1] == comboBox3.Text) 
                {
                    doc = doctors[i][0];
                }
            }
            string start = numericUpDown1.Value.ToString("00") + ':' + numericUpDown2.Value.ToString("00");
            string end = numericUpDown4.Value.ToString("00") + ':' + numericUpDown3.Value.ToString("00");
            string cab = textBox5.Text;
            SqlCommand cmd = new SqlCommand(string.Format("INSERT INTO Work_days VALUES('{0}','{1}','{2}','{3}','{4}', NULL)",
                                            doc,
                                            dateTimePicker2.Value.ToString("dd-MM-yyyy"),
                                            start,
                                            end,
                                            cab), connect);
            cmd.ExecuteNonQuery();
            ShowAll(dateTimePicker2.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {//Изменение
            if (work_data.Count == 0 || (comboBox3.Text == "" && textBox5.Text == "" && numericUpDown1.Value >= numericUpDown4.Value))
                return;
            string doc = "";
            for (int i = 0; i < doctors.Count; i++)
            {
                if (doctors[i][1] == comboBox3.Text)
                {
                    doc = doctors[i][0];
                }
            }
            string id = work_data[dataGridView1.CurrentRow.Index][1];
            string start = numericUpDown1.Value.ToString("00") + ':' + numericUpDown2.Value.ToString("00");
            string end = numericUpDown4.Value.ToString("00") + ':' + numericUpDown3.Value.ToString("00");
            string cab = textBox5.Text;
            SqlCommand cmd = new SqlCommand(string.Format("UPDATE Work_days SET Doctor_ID = '{0}', Day = '{1}',Start_work ='{2}',End_work = '{3}',Cabinet ='{4}' WHERE ID = '{5}'",
                                            doc,
                                            dateTimePicker2.Value.ToString("dd-MM-yyyy"),
                                            start,
                                            end,
                                            cab,
                                            id), connect);
            cmd.ExecuteNonQuery();
            ShowAll(dateTimePicker2.Value);

        }

        private void button4_Click(object sender, EventArgs e)
        {//Удаление
            del = true;
            if (work_data.Count == 0)
                return;

            string id = work_data[dataGridView1.CurrentRow.Index][1];
            SqlCommand cmd = new SqlCommand(string.Format("UPDATE Work_days SET Del = '{0}' WHERE ID = '{1}'",DateTime.Today,id), connect);
            cmd.ExecuteNonQuery();
            ShowAll(dateTimePicker2.Value);
        }
    }
}
