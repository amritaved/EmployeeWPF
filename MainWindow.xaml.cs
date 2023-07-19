using EmployeeWPF.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using EmployeeWPF.Common;
using System.Drawing;
using EmployeeWPF.Helper;

namespace EmployeeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetEmployeeDetails();
        }

        private async void GetEmployeeDetails()
        {
            Employee employeeData = (Employee)this.DataGridEmployee.SelectedItem;
            string userId = null;
            var response = await RestAPI.GetUsers(userId);
            if (response.IsSuccessStatusCode)
            {
                var employees = await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>();
                DataGridEmployee.ItemsSource = employees;
                ClearTextBoxes();
            }
            else
            {
                MessageBox.Show(HelperClass.ErrorHandling(response).ToString());
            }
        }

        private void ButtonGet_Click(object sender, RoutedEventArgs e)
        {
            GetEmployeeDetails();
        }

        private async void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Employee newEmployee = new()
            {
                name = TextBoxName.Text,
                gender = TextBoxGender.Text,
                email = TextBoxUserEmail.Text,
                status = TextBoxStatus.Text
            };
            if (String.IsNullOrEmpty(newEmployee.name) || String.IsNullOrEmpty(newEmployee.gender) || String.IsNullOrEmpty(newEmployee.email) || String.IsNullOrEmpty(newEmployee.status))
            {
                MessageBox.Show("Please fill the all the employee details");
            }
            else
            {
                var employeeDetails = await RestAPI.CreateUser(newEmployee);

                if (employeeDetails.IsSuccessStatusCode)
                {
                    MessageBox.Show(newEmployee.name + "'s details has successfully been added!");
                    GetEmployeeDetails();
                }
                else
                {
                    MessageBox.Show(HelperClass.ErrorHandling(employeeDetails).ToString());
                }
            }
        }

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Employee employeeData = (Employee)this.DataGridEmployee.SelectedItem;
            if (employeeData != null)
            {
                var deleteEmployee = await RestAPI.DeleteUser(employeeData);
                if (deleteEmployee.IsSuccessStatusCode)
                {
                    MessageBox.Show(employeeData.name + "'s details has successfully been deleted!");
                    GetEmployeeDetails();
                }
                else
                {
                    MessageBox.Show(HelperClass.ErrorHandling(deleteEmployee).ToString());
                }
            }
            else
            {
                MessageBox.Show("Please select a record to delete");
            }
        }

        private async void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Employee employeeData = (Employee)this.DataGridEmployee.SelectedItem;
            if (employeeData != null)
            {
                var updateEmployee = await RestAPI.UpdateUser(employeeData);
                if (updateEmployee.IsSuccessStatusCode)
                {
                    MessageBox.Show(employeeData.name + "'s details has successfully been updated!");
                    GetEmployeeDetails();
                }
                else
                {
                    MessageBox.Show(HelperClass.ErrorHandling(updateEmployee).ToString());
                }
            }
            else
            {
                MessageBox.Show("Please select a record to edit");
            }
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            DataGridEmployee.SelectAllCells();
            DataGridEmployee.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, DataGridEmployee);
            DataGridEmployee.UnselectAllCells();
            String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            File.AppendAllText("C:\\Employees.csv", result, UnicodeEncoding.UTF8);
            MessageBox.Show("CSV Exported Successfully on path C:\\");
        }

        private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TextBoxSearch.Text))
            {
                var url = $"{ConstantStrings.apiUrl}?name={TextBoxSearch.Text}";
                var response = await RestAPI.GetUsers(url);
                if (response.IsSuccessStatusCode)
                {
                    var employees = await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>();
                    DataGridEmployee.ItemsSource = employees;
                }
                else
                {
                    MessageBox.Show(HelperClass.ErrorHandling(response).ToString());
                }
                TextBoxSearch.Clear();
            }
            else
            {
                MessageBox.Show("Please enter text to search");
            }
        }


      


        private void ClearTextBoxes()
        {
            TextBoxName.Text = string.Empty;
            TextBoxGender.Text = string.Empty;
            TextBoxUserEmail.Text = string.Empty;
            TextBoxStatus.Text = string.Empty;
        }


    }
}

