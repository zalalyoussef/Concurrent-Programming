using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.ConstrainedExecution;

namespace Concurrent_lab_1
{
    class Program
    {
        static void Main(string[] args)
        {

            DataMonitor initialdata = new DataMonitor();
            ResultMonitor computeddata=new ResultMonitor();

            if (File.Exists("Results.txt"))
                File.Delete("Results.txt");

            Thread[] workerThreads = new Thread[4];

            for (int i = 0; i < workerThreads.Length; i++)
            {
                workerThreads[i] = new Thread(() => Calculation(initialdata, computeddata));
                workerThreads[i].Start();
            }

            ReadData(initialdata);
            
            foreach (var item in workerThreads) { item.Join(); }
            Print(computeddata);
            
        }
        static void ReadData(DataMonitor dataMonitor)
        {
            
            
            {
                
                string filePath = "Employees.txt";

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            int id = int.Parse(parts[0]);
                            double salary = double.Parse(parts[1]);
                            string name = parts[2];

                            
                            Employee employee = new Employee(id, salary, name);
                           
                            
                            dataMonitor.AddEmployee(employee);
                        }
                    }
                }
            }
            
        }
        static void Print(ResultMonitor dataMonitor)
        {
            try
            {
                
                string filePath = "D:\\semester5\\concurrent lab1\\Concurrent lab-1\\Concurrent lab-1\\Results.txt";

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("ID    Salary     Name     Computed result");
                    writer.WriteLine("______________________________________________________________________");
                    
                    foreach (Employee employee in dataMonitor.GetEmployees())
                    {
                        writer.WriteLine("{0,-5} {1,-8} {2,-15} {3,10}",employee.ID,employee.Salary,employee.Name,employee.Hash);

                    }
                    writer.WriteLine("______________________________________________________________________");
                }

                Console.WriteLine("Employees printed to Results.txt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing data: " + ex.Message);
            }
        }
        static void Calculation(DataMonitor dataMonitor, ResultMonitor resultMonitor)
        {
                while (true)
                {

                    Employee employee = dataMonitor.RemoveEmployee();


                    if (employee == null)
                    {
                        break;
                    }

                    if (!char.IsDigit(employee.Hash[0]))
                    {
                        resultMonitor.AddEmployee(employee);
                    }

                }            

        }
    }
}

