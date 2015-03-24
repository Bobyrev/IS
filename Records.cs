using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Med
{
    class Records
    {
        private SqlConnection connect;
        private SqlCommand cmd;
        public Records(string str) 
        {
            connect = new SqlConnection(str);
            connect.Open();
        }

        public List<string[]> AllRecords()
        {//Выводит все записи
            cmd = new SqlCommand("SELECT Records.ID, Doctors.Surname, Clients.Surname, Records.Time FROM Records "+
                                 "INNER JOIN Clients ON Clients.ID = Records.ID_Client "+
                                 "INNER JOIN Doctors ON Doctors.ID = Records.ID_Doctor "+
                                 "WHERE Records.Del IS NULL", connect);
            List<string[]> result = new List<string[]>();
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    result.Add(new string[] { sdr[0].ToString(), sdr[1].ToString(), sdr[2].ToString(), sdr[3].ToString() });
                }
            }
            return result;
        }

        public void Add(int ID_Doctor, int ID_Client, DateTime time) 
        {
            cmd = new SqlCommand(string.Format("SELECT COUNT(*) FROM Records WHERE ID_Doctor = '{0}' AND ID_Client = '{1}' AND Time = '{2}'", ID_Doctor, ID_Client, time), connect);
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                int count = 0;
                while (sdr.Read())
                {
                    count = int.Parse(sdr[0].ToString());
                }
                if (count != 0)
                {
                    return;
                }
            }
            cmd = new SqlCommand("SELECT Max(ID) FROM Records", connect);
            int ID = 1;
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {//Получаем ID
                while (sdr.Read())
                {
                    ID = int.Parse(sdr[0].ToString()) + 1;
                }
            }
            cmd = new SqlCommand(string.Format("INSERT INTO Records (ID, ID_Doctor, ID_Client, Time) VALUES ('{0}', '{1}', '{2}', '{3}')", ID, ID_Doctor, ID_Client, time), connect);
            cmd.ExecuteNonQuery();
        }

        public void Edit(int ID, int ID_Doctor, int ID_Client, DateTime time) 
        {
            cmd = new SqlCommand(string.Format("UPDATE Records SET ID_Doctor = '{0}', ID_Client = '{1}', Time = '{2}' WHERE ID = '{3}'", ID_Doctor, ID_Client, time, ID), connect);
            cmd.ExecuteNonQuery();
        }

        public void Del(int ID)
        {
            cmd = new SqlCommand(string.Format("UPDATE Records SET Del = '{1}' WHERE ID = '{0}'", ID, DateTime.Now.Date), connect);
            cmd.ExecuteNonQuery();
        }
    }
}
