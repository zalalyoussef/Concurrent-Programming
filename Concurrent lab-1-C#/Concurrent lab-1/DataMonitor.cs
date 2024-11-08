using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrent_lab_1
{

    public class DataMonitor
    {
        private Employee[] employeeArray;
        private int currentIndex;
        private static readonly object dataMonitorLock = new object();
        const int arrSize = 15;
        private int n;


        public DataMonitor()
        {
            employeeArray = new Employee[arrSize];
            currentIndex = 0;
            
        }

        
        public void AddEmployee(Employee employee)
        {
            lock (dataMonitorLock)
            {
                while (currentIndex >= arrSize)
                {
                    Monitor.Wait(dataMonitorLock);
                }
                employeeArray[currentIndex++] = employee;
                n++;
                Monitor.PulseAll(dataMonitorLock);
                
            }

        }
        public int Count() { return n; }

        public Employee RemoveEmployee()
        {
            lock (dataMonitorLock)
            {
                while (currentIndex == 0 && n < 25)
                {
                    Monitor.Wait(dataMonitorLock);
                }
                if (currentIndex == 0 && n >= 25)
                {
                    
                    return null;
                }
                Employee item = employeeArray[--currentIndex];
                Monitor.PulseAll(dataMonitorLock);
                return item;
            }
        }
    }

}
