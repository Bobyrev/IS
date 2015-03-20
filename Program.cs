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
            Clients cl = new Clients();
            foreach (string[] elem in cl.AllRecords()) 
            {
                Console.WriteLine(string.Join(" ",elem));
            }
        }
    }
}
