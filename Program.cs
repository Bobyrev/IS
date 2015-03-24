using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med
{
    class Program
    {
        static void Main(string[] args)
        {
			string str = @"Data Source=CEPEGGA-ПК\SQLEXPRESS;
                           Initial Catalog=Med;
                           Integrated Security=True";
            Clients cl = new Clients(str);
            foreach (string[] elem in cl.AllRecords()) 
            {
                Console.WriteLine(string.Join(" ",elem));
            }
        }
    }
}
