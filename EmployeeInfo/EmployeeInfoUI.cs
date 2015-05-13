using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeInfo
{
    public partial class EmployeeInfoUI : Form
    {

        SqlConnection aConnection = new SqlConnection();
        string connectionString = ConfigurationManager.ConnectionStrings["EmployeeInfoDBConnectionString"].ConnectionString;

        private bool isUpdateMode = false;
        private int employeeId = 0;
        private int employeeIdforDelete = 0;

        public EmployeeInfoUI()
        {
            InitializeComponent();
            ShowEmployeeinListView();
        }

        private void ShowEmployeeinListView()
        {
            showEmployeeListView.Items.Clear();
            List<Employee> aEmployees = GetAllEmployeeDetails();
            foreach (var employees in aEmployees)
            {
                ListViewItem item = new ListViewItem(employees.Id.ToString());
                item.SubItems.Add(employees.Name);
                item.SubItems.Add(employees.Address);
                item.SubItems.Add(employees.Email);
                item.SubItems.Add(employees.Salary.ToString());

                showEmployeeListView.Items.Add(item);

            }
        }

        private List<Employee> GetAllEmployeeDetails()
        {
            aConnection.ConnectionString = connectionString;
            aConnection.Open();
            string query = string.Format("SELECT * FROM Employee");

            SqlCommand aCommand = new SqlCommand(query, aConnection);
            SqlDataReader aReader = aCommand.ExecuteReader();

            List<Employee> employees = new List<Employee>();
            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    employee = new Employee();
                    employee.Id = (int)aReader[0];
                    employee.Name = aReader[1].ToString();
                    employee.Address = aReader[2].ToString();
                    employee.Email = aReader[3].ToString();
                    employee.Salary = Convert.ToDouble(aReader[4]);
                   
                    employees.Add(employee);
                }
            }
            aConnection.Close();
            return employees;
        }

        Employee aEmployee = new Employee();
        private Employee employee;

        private void saveButton_Click(object sender, EventArgs e)
        {
            string messageInfo = "Please fillup\n";
            if (nameTextBox.Text == "" || addressTextBox.Text == "" || emailTextBox.Text == "" || salaryTextBox.Text == "")
            {
                if (nameTextBox.Text == "")
                {
                    messageInfo += "Name Textbox\n";
                }
                if (addressTextBox.Text == "")
                {
                    messageInfo += "Address Textbox\n";
                }
                if (emailTextBox.Text == "")
                {
                    messageInfo += "Email Textbox\n";
                }
                if (salaryTextBox.Text == "")
                {
                    messageInfo += "Salary Textbox\n";
                }
                MessageBox.Show(messageInfo);
            }
            else
            {
                aEmployee.Name = nameTextBox.Text;
                aEmployee.Address = addressTextBox.Text;
                aEmployee.Email = emailTextBox.Text;
                aEmployee.Salary = Convert.ToDouble(salaryTextBox.Text);

                if (isUpdateMode)
                {
                    string message = UpdateEmployee(aEmployee);
                    MessageBox.Show(message);
                    ShowEmployeeinListView();
                    saveButton.Text = "Save";
                    employeeId = 0;
                    isUpdateMode = false;
                    emailTextBox.Enabled = true;
                }
                else
                {
                    string message = SaveEmployee(aEmployee);
                    MessageBox.Show(message);
                    ShowEmployeeinListView();
                    nameTextBox.Clear();
                    addressTextBox.Clear();
                    emailTextBox.Clear();
                    salaryTextBox.Clear();
                } 
            }

           
        }

        private string UpdateEmployee(Employee employee)
        {
           
                aConnection.ConnectionString = connectionString;
                aConnection.Open();
                string query = string.Format("UPDATE Employee SET Name='{0}',Address='{1}',Salary={2} WHERE ID={3}", employee.Name, employee.Address, employee.Salary, employeeId);

                SqlCommand aCommand = new SqlCommand(query, aConnection);
                int rowAffected = aCommand.ExecuteNonQuery();
                aConnection.Close();
                if (rowAffected > 0)
                {
                    return "Update Successful";
                    
                }
                else
                {
                    return "Update Failed";
                }
                
            
            
        }

        private string SaveEmployee(Employee employee)
        {
            
            if (IsThisEmailAlreadyused(employee.Email))
            {
                return "This Email Already in System";
            }
            else
            {
                aConnection.ConnectionString = connectionString;
                aConnection.Open();
                string query = string.Format("INSERT INTO Employee VALUES('{0}','{1}','{2}',{3})", employee.Name, employee.Address, employee.Email, employee.Salary);

                SqlCommand aCommand = new SqlCommand(query, aConnection);
                int rowAffected = aCommand.ExecuteNonQuery();
                aConnection.Close();
                if (rowAffected > 0)
                {
                    return "Save Successful";
                }
                else
                {
                    return "Failed";
                }
            }
        }

        private bool IsThisEmailAlreadyused(string email)
        {
            aConnection.ConnectionString = connectionString;
            aConnection.Open();
            string query = string.Format("SELECT * FROM Employee WHERE email='{0}'", email);

            SqlCommand aCommand = new SqlCommand(query, aConnection);
            SqlDataReader aReader = aCommand.ExecuteReader();
            bool message = aReader.HasRows;
            aConnection.Close();
            return message;
        }

        private void showEmployeeListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = showEmployeeListView.SelectedItems[0];

            int id = int.Parse(item.Text.ToString());

            Employee employee = GetEmployeeById(id);

            if (employee!=null)
            {
                isUpdateMode = true;
                saveButton.Text = "Update";
                employeeId = employee.Id;
                employeeIdforDelete = employeeId;

                emailTextBox.Enabled = false;
                nameTextBox.Text = employee.Name;
                addressTextBox.Text = employee.Address;
                emailTextBox.Text = employee.Email;
                salaryTextBox.Text = employee.Salary.ToString();
            }

            
        }

        private Employee GetEmployeeById(int id)
        {
            aConnection.ConnectionString = connectionString;
            aConnection.Open();
            string query = string.Format("SELECT * FROM Employee WHERE Id='{0}'", id);

            SqlCommand aCommand = new SqlCommand(query, aConnection);
            SqlDataReader aReader = aCommand.ExecuteReader();
          
            while (aReader.Read())
            {

                employee = new Employee();
                employee.Id = (int)aReader[0];
                employee.Name = aReader[1].ToString();
                employee.Address = aReader[2].ToString();
                employee.Email = aReader[3].ToString();
                employee.Salary = Convert.ToDouble(aReader[4]);

               
            }
            aReader.Close();
            aConnection.Close();

            return employee;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

            DialogResult aDialogResult = MessageBox.Show("Are you Want to delate!!!!", "Some Title", MessageBoxButtons.YesNo);

            if (aDialogResult==DialogResult.Yes)
            {
                aConnection.ConnectionString = connectionString;
                aConnection.Open();
                string query = string.Format("DELETE FROM Employee WHERE Id='{0}'", employeeIdforDelete);

                SqlCommand aCommand = new SqlCommand(query, aConnection);
                int rowAffected = aCommand.ExecuteNonQuery();


                aConnection.Close();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Successful");
                }
                else
                {
                    MessageBox.Show("Failed");
                }

                ShowEmployeeinListView();

                nameTextBox.Clear();
                addressTextBox.Clear();
                emailTextBox.Clear();
                salaryTextBox.Clear();
            }

            else if (aDialogResult == DialogResult.No)
            {
                MessageBox.Show("Its Ok!!!");
            }
            saveButton.Text = "Save";
            emailTextBox.Enabled = true;


        }
    }
}
