using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmployeeInfo.BLL;

namespace EmployeeInfo
{
    public partial class EmployeeInfoUI : Form
    {


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
            List<Employee> aEmployees = new EmployeeBll().GetAllEmployeeList();
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
                aEmployee.Id = employeeId;
                aEmployee.Name = nameTextBox.Text;
                aEmployee.Address = addressTextBox.Text;
                aEmployee.Email = emailTextBox.Text;
                aEmployee.Salary = Convert.ToDouble(salaryTextBox.Text);

                if (isUpdateMode)
                {
                    string message = new EmployeeBll().UpdatePatientDetails(aEmployee);
                    MessageBox.Show(message);
                    ShowEmployeeinListView();
                    saveButton.Text = @"Save";
                    employeeId = 0;
                    isUpdateMode = false;
                    emailTextBox.Enabled = true;
                }
                else
                {
                    string message = new EmployeeBll().SavePatientDetails(aEmployee);
                    MessageBox.Show(message);
                    ShowEmployeeinListView();
                    nameTextBox.Clear();
                    addressTextBox.Clear();
                    emailTextBox.Clear();
                    salaryTextBox.Clear();
                }
            }


        }

        private void showEmployeeListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = showEmployeeListView.SelectedItems[0];

            int id = int.Parse(item.Text.ToString());

            Employee employee = new EmployeeBll().GetEmployeeById(id);

            if (employee != null)
            {
                isUpdateMode = true;
                saveButton.Text = @"Update";
                employeeId = employee.Id;
                employeeIdforDelete = employeeId;

                emailTextBox.Enabled = false;
                nameTextBox.Text = employee.Name;
                addressTextBox.Text = employee.Address;
                emailTextBox.Text = employee.Email;
                salaryTextBox.Text = employee.Salary.ToString(CultureInfo.InvariantCulture);
            }
        }


        private void deleteButton_Click(object sender, EventArgs e)
        {

            DialogResult aDialogResult = MessageBox.Show(@"Are you Want to delate!!!!", @"Confirmation", MessageBoxButtons.YesNo);

            if (aDialogResult == DialogResult.Yes)
            {
                if (new EmployeeBll().DeleteEmployeeById(employeeIdforDelete))
                {
                    MessageBox.Show(@"Successful");
                }
                else
                {
                    MessageBox.Show(@"Failed");
                }

                ShowEmployeeinListView();

                nameTextBox.Clear();
                addressTextBox.Clear();
                emailTextBox.Clear();
                salaryTextBox.Clear();
            }

            else if (aDialogResult == DialogResult.No)
            {
                MessageBox.Show(@"Its Ok!!!");
            }
            saveButton.Text = @"Save";
            emailTextBox.Enabled = true;
        }
    }
}
