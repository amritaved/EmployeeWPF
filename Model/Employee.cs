using System.Collections.Generic;


namespace EmployeeWPF.Model
{
    public class Employee
    {

        public int id { get; set; }
        
        public string name { get; set; }

        public string email { get; set; }

        public string gender { get; set; }

        public string status { get; set; }
    }

    public class EmployeeResponse
    {
        public List<Employee> Data { get; set; }
    }
}
