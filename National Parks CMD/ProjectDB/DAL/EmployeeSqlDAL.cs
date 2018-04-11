using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class EmployeeSqlDAL
    {
        private const string SQL_AllEmployees = @"SELECT employee_id, department_id, first_name, last_name, job_title, birth_date, gender, hire_date FROM employee";
        private const string SQL_EmployeeByName = @"SELECT employee_id, department_id, first_name, last_name, job_title, birth_date, gender, hire_date FROM employee where employee.first_name = @first_name and employee.last_name = @last_name";
        private const string SQL_EmployeesNoProject = @"SELECT employee.employee_id, employee.department_id, employee.first_name, employee.last_name, employee.job_title, employee.birth_date, employee.gender, employee.hire_date from project JOIN project_employee on project.project_id = project_employee.project_id RIGHT JOIN employee on employee.employee_id = project_employee.employee_id where project_employee.project_id IS NULL";



        private string connectionString;

        // Single Parameter Constructor
        public EmployeeSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Employee> GetAllEmployees()
        {
                List<Employee> output = new List<Employee>();

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(SQL_AllEmployees, conn);

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Employee item = GetEmployeesFromReader(reader);
                            output.Add(item);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw;
                }

                return output;
        }

        public List<Employee> Search(string firstname, string lastname)
        {
            List<Employee> output = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_EmployeeByName, conn);
                    cmd.Parameters.AddWithValue("@first_name", firstname);
                    cmd.Parameters.AddWithValue("@last_name", lastname);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee item = GetEmployeesFromReader(reader);
                        output.Add(item);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return output;
        }

        public List<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> output = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_EmployeesNoProject, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee item = GetEmployeesFromReader(reader);
                        output.Add(item);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return output;
        }


        private Employee GetEmployeesFromReader(SqlDataReader reader)
        {
            Employee item = new Employee();

            item.EmployeeId = Convert.ToInt32(reader["employee_id"]);
            item.DepartmentId = Convert.ToInt32(reader["department_id"]);
            item.FirstName = Convert.ToString(reader["first_name"]);
            item.LastName = Convert.ToString(reader["last_name"]);
            item.JobTitle = Convert.ToString(reader["job_title"]);
            item.BirthDate = Convert.ToDateTime(reader["birth_date"]);
            item.Gender = Convert.ToString(reader["gender"]);
            item.HireDate = Convert.ToDateTime(reader["hire_date"]);


            return item;
        }


    }
}
