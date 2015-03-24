using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Med
{
    class Clients
    {
        private SqlConnection connect;
        private SqlCommand cmd;
        private Random rnd = new Random();
        public Clients(string str)
        {
            
            connect = new SqlConnection(str);
            connect.Open();
        }

        public List<string[]> AllRecords()
        {//Выводит все записи
            cmd = new SqlCommand("SELECT * FROM Clients", connect);
            List<string[]> result = new List<string[]>();
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    result.Add(new string[] { sdr[0].ToString(), sdr[1].ToString(), sdr[2].ToString(), sdr[3].ToString(), sdr[4].ToString() });
                }
            }
            return result;
        }

        public void Add(string Surname, string Name, string Twoname, int Polis)
        {
            cmd = new SqlCommand(string.Format("SELECT COUNT(*) FROM Clients WHERE Name = '{0}' AND Surname = '{1}' AND Twoname = '{2}' AND Polis = '{3}'", Name, Surname, Twoname, Polis), connect);
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
            cmd = new SqlCommand("SELECT ID FROM Clients", connect);
            List<int> IDs = new List<int>();
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {//Получаем ID
                while (sdr.Read())
                {
                    IDs.Add(int.Parse(sdr[0].ToString()));
                }
            }
            int ID = rnd.Next(1, 9999999);
            bool flag = true;
            while (flag)
            {//Проверяем наш ID
                if (IDs.Contains(ID))
                {
                    ID = rnd.Next(1, 9999999);
                }
                else
                {
                    flag = false;
                }
            }
            cmd = new SqlCommand(string.Format("INSERT INTO Clients VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", ID, Name, Surname, Twoname, Polis), connect);
            cmd.ExecuteNonQuery();
        }

        public void Edit(int ID, string Name, string Surname, string Twoname, int Polis)
        {
            cmd = new SqlCommand(string.Format("UPDATE Clients SET Name = '{0}', Surname = '{1}', Twoname = '{2}', Polis = '{3}' WHERE ID = '{4}'", Name, Surname, Twoname, Polis, ID), connect);
            cmd.ExecuteNonQuery();
        }

        public void Del(int ID) 
        {
            cmd = new SqlCommand(string.Format("DELETE FROM Clients WHERE ID = '{0}'",ID),connect);
            cmd.ExecuteNonQuery();
        }
    }
}
