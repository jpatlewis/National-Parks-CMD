using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{

    public class DepartmentSqlDAL
    {

        private const string SQL_AllDepartments = "SELECT department.department_id, department.name FROM department order by department.department_id asc";
        private const string SQL_AddDepartment = "INSERT INTO department (name) values (@name)";
        private const string SQL_UpdateDepartment = "UPDATE department set name = @name where department_id = @ID";

        private string connectionString;

        // Single Parameter Constructor
        public DepartmentSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Department> GetDepartments()
        {
            List<Department> output = new List<Department>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AllDepartments, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Department item = GetDepartmentFromReader(reader);
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

        public bool CreateDepartment(Department newDepartment)
        {
            bool isCreated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AddDepartment, conn);
                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);
 
                    int checking = cmd.ExecuteNonQuery();
                    if (checking > 0)
                    {
                        isCreated = true;
                    }
                    

                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return isCreated;
        }

        public bool UpdateDepartment(Department updatedDepartment)
        {
            bool isCreated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_UpdateDepartment, conn);
                    cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);
                    cmd.Parameters.AddWithValue("@ID", updatedDepartment.Id);


                    int checking = cmd.ExecuteNonQuery();
                    if (checking > 0)
                    {
                        isCreated = true;
                    }


                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return isCreated;
        }


        private Department GetDepartmentFromReader(SqlDataReader reader)
        {
            Department item = new Department();

            item.Id = Convert.ToInt32(reader["department_id"]);
            item.Name = Convert.ToString(reader["name"]);

            return item;
        }
    }
}
