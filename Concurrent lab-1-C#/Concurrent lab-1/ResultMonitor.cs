using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrent_lab_1
{
    using System;
    using System.Threading;

    public class ResultMonitor
    {
        private static readonly object resultMonitorLock = new object();
        private Employee[] resultArray;
        private int currentIndex;
        const int arrSize = 25;
        private int n;
        public int Count() { return n; }
        public ResultMonitor()
        {
            resultArray = new Employee[arrSize];
            currentIndex = 0;
        }



        public void AddEmployee(Employee employee)
        {
            lock (resultMonitorLock)
            {
               
                int insertIndex = currentIndex;
                while (insertIndex > 0 && resultArray[insertIndex - 1].ID > employee.ID)
                {
                    resultArray[insertIndex] = resultArray[insertIndex - 1];
                    insertIndex--;
                }

                if (insertIndex >= 0 && insertIndex < resultArray.Length)
                {
                    resultArray[insertIndex] = employee;
                    currentIndex++;
                    n++;
                }
            }
        }


        public Employee[] GetEmployees()
        {
            
            {
                Employee[] employees = new Employee[currentIndex];
                Array.Copy(resultArray, employees, currentIndex);
                return employees;
                
            }
        }
    }

}
