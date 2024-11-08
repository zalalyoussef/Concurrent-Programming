using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrent_lab_1
{
    public class Employee
    {
            public int ID { get; set; }
            public double Salary { get; set; }
            public string Name { get; set; }
            public string Hash { get; set; }

        private static readonly Random random = new Random();

        public Employee()
        {
                ID = 0;
                Salary = 0.0;
                Name = "Default";
            Hash = RandomString(30);
        }

            
            public Employee(int ID, double Salary, string Name)
            {
                this.ID= ID;
                this.Salary = Salary;
                this.Name = Name;
              Hash = RandomString(30);
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }


    }
}
