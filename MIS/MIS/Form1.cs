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
            label5.Text = "";
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
        }

        #region Доп.методы

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

        #endregion

        private void Form1_Shown(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            this.Visible = false;
            form2.Show();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!Char.IsDigit(e.KeyChar) || textBox1.Text.Length == 24) && e.KeyChar != 8) 
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 24)
            {
                label5.Text = "✓";
                label5.ForeColor = Color.Green;

                SqlCommand cmd = new SqlCommand(string.Format("SELECT Name, Surname, Twoname FROM Clients WHERE Polis = '{0}'", textBox1.Text), connect);

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    if (sdr.Read())
                    {
                        textBox2.Text = sdr[0].ToString();
                        textBox3.Text = sdr[1].ToString();
                        textBox4.Text = sdr[2].ToString();
                    }
                }
            }
            else
            {
                label5.Text = textBox1.Text.Length.ToString();
                label5.ForeColor = Color.Black;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length != 0)
            {
                label6.Visible = true;
            }
            else
            {
                label6.Visible = false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length != 0)
            {
                label7.Visible = true;
            }
            else
            {
                label7.Visible = false;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length != 0)
            {
                label8.Visible = true;
            }
            else
            {
                label8.Visible = false;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
        }

        private void Load_Name_Doctors()
        {
            comboBox2.Items.Clear();
            SqlCommand cmd = new SqlCommand(String.Format("SELECT Name, Surname, Twoname FROM Doctors WHERE Specialization = '{0}' AND Del IS NULL", comboBox1.Text), connect);
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
            comboBox4.Items.Clear();
        }

        private void new_rasp()
        {
            comboBox4.Items.Clear();
            string[] str = comboBox2.Text.Split(' ');
            int id = 0;
            SqlCommand cmd = new SqlCommand(string.Format("SELECT ID FROM Doctors WHERE Name = '{0}' AND Specialization = '{1}'", str[0], comboBox1.Text), connect);
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                sdr.Read();
                id = (int)sdr[0];
            }
            int start_hour = 0;
            int start_minutes = 0;
            int stop_hour = 0;
            int stop_minutes = 0;
            cmd = new SqlCommand(string.Format("SELECT Start_work, End_work, Cabinet FROM Work_days WHERE Day = '{0}' AND Doctor_ID = '{1}'", dateTimePicker1.Value, id), connect);
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                if (sdr.Read())
                {
                    int[] t = sdr[0].ToString().Split(':').Select(int.Parse).ToArray();
                    start_hour = t[0];
                    start_minutes = t[1];
                    t = sdr[1].ToString().Split(':').Select(int.Parse).ToArray();
                    stop_hour = t[0];
                    stop_minutes = t[1];
                }
            }
            if (start_hour != 0 && stop_hour != 0)
            {
                string time = start_hour.ToString() + ":" + ((start_minutes == 0) ? "00" : start_minutes.ToString());
                cmd = new SqlCommand(string.Format("SELECT * FROM Records WHERE ID_Doctor = '{0}' AND Time = '{1}' AND Day = '{2}'", id, time, dateTimePicker1.Value), connect);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    if (!sdr.Read())
                    {
                        comboBox4.Items.Add(start_hour.ToString() + ":" + ((start_minutes == 0) ? "00" : start_minutes.ToString()));
                    }
                }
            }
            else
            {
                MessageBox.Show(string.Format("На дату: {0} у врача {1} нет свободного времени.", dateTimePicker1.Value.Date, comboBox2.Text));
            }
            while (start_hour != stop_hour || start_minutes != stop_minutes)
            {
                if (start_minutes == 45)
                {
                    start_minutes = 0;
                    start_hour++;
                }
                else
                {
                    start_minutes += 15;
                }
                string time = start_hour.ToString() + ":" + start_minutes.ToString();
                cmd = new SqlCommand(string.Format("SELECT * FROM Records WHERE ID_Doctor = '{0}' AND Time = '{1}' AND Day = '{2}'", id, time, dateTimePicker1.Value), connect);
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    if (!sdr.Read())
                    {
                        comboBox4.Items.Add(start_hour.ToString() + ":" + ((start_minutes == 0) ? "00" : start_minutes.ToString()));
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            new_rasp();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            if (comboBox2.Text.Length != 0)
            {
                new_rasp();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 24)
            {
                textBox1.BackColor = Color.Red;
            }
            else
            {
                textBox1.BackColor = Color.White;
                if (textBox2.Text.Length == 0)
                {
                    textBox2.BackColor = Color.Red;
                }
                else
                {
                    textBox2.BackColor = Color.White;
                    if (textBox3.Text.Length == 0)
                    {
                        textBox3.BackColor = Color.Red;
                    }
                    else
                    {
                        textBox3.BackColor = Color.White;
                        if (textBox4.Text.Length == 0)
                        {
                            textBox4.BackColor = Color.Red;
                        }
                        else
                        {
                            textBox4.BackColor = Color.White;
                            if (comboBox1.Text.Length == 0) 
                            {
                                MessageBox.Show("Выберите специализацию врача.");
                            }
                            else
                            {
                                if (comboBox2.Text.Length == 0)
                                {
                                    MessageBox.Show("Выберите врача.");
                                }
                                else
                                {
                                    if (comboBox4.Text.Length == 0)
                                    {
                                        MessageBox.Show("Выберите время на которое записать клиента.");
                                    }
                                    else
                                    {
                                        int id_d = 0;
                                        string[] str = comboBox2.Text.Split(' ');
                                        SqlCommand cmd = new SqlCommand(string.Format("SELECT ID FROM Doctors WHERE Name = '{0}' AND Specialization = '{1}'", str[0], comboBox1.Text), connect);
                                        using (SqlDataReader sdr = cmd.ExecuteReader())
                                        {
                                            sdr.Read();
                                            id_d = (int)sdr[0];
                                        }
                                        int id = 0;
                                        cmd = new SqlCommand(string.Format("SELECT MAX(ID) FROM Records"), connect);
                                        using (SqlDataReader sdr = cmd.ExecuteReader())
                                        {
                                            if (sdr.Read())
                                            {
                                                try
                                                {
                                                    id = (int)sdr[0];
                                                }
                                                catch { }
                                            }
                                        }
                                        id++;
                                        cmd = new SqlCommand(string.Format("INSERT INTO Records VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', NULL)", id, id_d, textBox1.Text, dateTimePicker1.Value, comboBox4.Text), connect);
                                        if (cmd.ExecuteNonQuery() == 1)
                                        {
                                            MessageBox.Show("Запись успешно добавлена.");
                                            textBox1.Text = "";
                                            textBox2.Text = "";
                                            textBox3.Text = "";
                                            textBox4.Text = "";
                                            comboBox1.Text = "";
                                            comboBox2.Items.Clear();
                                            comboBox4.Items.Clear();
                                        }
                                        else
                                        {
                                            MessageBox.Show("При добавлении произошла ошибка.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
