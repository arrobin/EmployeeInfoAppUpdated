using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInfo.DAL.Gateway
{
    class EmployeeGateway:BaseGateway
    {
        public List<Employee> GetAllEmployeeDetails()
        {
           string query = "SELECT * FROM Employee";

            Connection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, Connection);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            List<Employee> employees = new List<Employee>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.Id = (int)reader["id"];
                    employee.Name = reader["name"].ToString();
                    employee.Address = reader["address"].ToString();
                    employee.Email = reader["email"].ToString();
                    employee.Salary = Convert.ToDouble(reader["salary"]);

                    employees.Add(employee);
                }
            }
            Connection.Close();
            return employees;
        }

        public Employee GetEmployeeById(int id)
        {
            string query = "SELECT * FROM Employee WHERE id='"+id+"'";

            Connection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, Connection);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            Employee employee = new Employee();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    employee.Id = (int)reader["id"];
                    employee.Name = reader["name"].ToString();
                    employee.Address = reader["address"].ToString();
                    employee.Email = reader["email"].ToString();
                    employee.Salary = Convert.ToDouble(reader["salary"]);
                }
            }
            Connection.Close();
            return employee;
        }

        public bool SaveEmployee(Employee aEmployee)
        {
            string query = "INSERT INTO Employee (name, address, email,salary) VALUES (@name, @address, @email, @salary);";
            Connection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, Connection);
            sqlCommand.Parameters.AddWithValue("@name", aEmployee.Name);
            sqlCommand.Parameters.AddWithValue("@address", aEmployee.Address);
            sqlCommand.Parameters.AddWithValue("@email", aEmployee.Email);
            sqlCommand.Parameters.AddWithValue("@salary", aEmployee.Salary);
            try
            {
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception exception)
            {
                Helper.Helper.WriteError(exception);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
        public bool UpdateEmployee(Employee aEmployee)
        {
            string query = "UPDATE Employee SET name=@name, address=@address, email=@email, salary=@salary WHERE id=@id;";
            Connection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, Connection);
            sqlCommand.Parameters.AddWithValue("@name", aEmployee.Name);
            sqlCommand.Parameters.AddWithValue("@address", aEmployee.Address);
            sqlCommand.Parameters.AddWithValue("@email", aEmployee.Email);
            sqlCommand.Parameters.AddWithValue("@salary", aEmployee.Salary);
            sqlCommand.Parameters.AddWithValue("@id", aEmployee.Id);
            try
            {
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception exception)
            {
                Helper.Helper.WriteError(exception);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
        public bool IsThisEmailAlreadyused(string email)
        {
            string query = "SELECT * FROM Employee WHERE email='"+email+"'";
            Connection.Open();
            bool flag = false;
            SqlDataReader aSqlDataReader = new SqlCommand(query, Connection).ExecuteReader();
            if (aSqlDataReader.HasRows)
            {
                flag = true;
            }
            aSqlDataReader.Close();
            Connection.Close();
            return flag;
        }
        public bool DeleteEmployeeById(int employeeId)
        {
            string query = "DELETE FROM Employee WHERE Id='" + employeeId+"'";
            Connection.Open();
            SqlCommand comd = new SqlCommand(query, Connection);
            try
            {
                var a = comd.ExecuteNonQuery();
                if (a > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                Helper.Helper.WriteError(exception);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
    }
}
