using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Med
{
    class Doctors
    {
        public Doctors() { }

        public List<string[]> Select(SqlConnection connect)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Doctors", connect);
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

        public void Add(SqlConnection connect, string last_name, string first_name, string patronymic, string specialization)
        {
            SqlCommand cmd = new SqlCommand("SELECT Max(ID) FROM Doctors", connect);
            int maxID;
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                sdr.Read();
                maxID = (int)sdr[0] + 1;
            }
            cmd = new SqlCommand(string.Format("INSERT INTO Doctors VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", maxID, last_name, first_name, patronymic, specialization), connect);
            cmd.ExecuteNonQuery();
        }

        public void Edit(SqlConnection connect, int ID, string last_name, string first_name, string patronymic, string specialization)
        {
            SqlCommand cmd = new SqlCommand(string.Format("UPDATE Doctors SET last_name = '{0}', first_name = '{1}', patronymic = '{2}', specialization = '{3}' WHERE ID = '{4}'", last_name, first_name, patronymic, specialization, ID), connect);
            cmd.ExecuteNonQuery();
        }

        public void Delete(SqlConnection connect, int ID)
        {
            SqlCommand cmd = new SqlCommand(string.Format("DELETE FROM Doctors WHERE ID = '{0}'", ID), connect);
            cmd.ExecuteNonQuery();
        }
    }
}
