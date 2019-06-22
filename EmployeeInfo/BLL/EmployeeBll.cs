using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeInfo.DAL.Gateway;

namespace EmployeeInfo.BLL
{
    class EmployeeBll
    {
        readonly EmployeeGateway _aGateway= new EmployeeGateway();
        public bool HasThisEmployeeAlreadyInSystem(string givenEmployeeEmail)
        {
            return _aGateway.IsThisEmailAlreadyused(givenEmployeeEmail);
        }
        public string SavePatientDetails(Employee aEmployee)
        {
            if (HasThisEmployeeAlreadyInSystem(aEmployee.Email))
            {
                return "This Email Already In System!!!";
            }
            return _aGateway.SaveEmployee(aEmployee) ? "ok" : "Employee Information Not Saved";
        }
        public string UpdatePatientDetails(Employee aEmployee)
        {
            return _aGateway.UpdateEmployee(aEmployee) ? "ok" : "Employee Information Not Updated";
        }

        public bool DeleteEmployeeById(int employeeId)
        {
            return _aGateway.DeleteEmployeeById(employeeId);
        }

        public Employee GetEmployeeById(int id)
        {
            return _aGateway.GetEmployeeById(id);
        }
        public List<Employee> GetAllEmployeeList()
        {
            return _aGateway.GetAllEmployeeDetails();
        }
    }
}
