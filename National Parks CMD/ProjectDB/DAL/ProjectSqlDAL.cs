using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class ProjectSqlDAL
    {

        private const string SQL_AllProjects = @"SELECT project_id, name, from_date, to_date FROM project";
        private string connectionString;
        private const string SQL_AddProject = @"INSERT INTO project (name, from_date, to_date) values (@name, @startDate, @endDate)";
        private const string SQL_AssignEmployeeToProject = @"INSERT INTO project_employee (project_id, employee_id) values (@projectID, @employeeID)";
        private const string SQL_RemoveEmployeeFromProject = @"DELETE FROM project_employee where project_id = @projectID and employee_id = @employeeID";

        // Single Parameter Constructor
        public ProjectSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Project> GetAllProjects()
        {
            List<Project> output = new List<Project>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AllProjects, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Project item = GetProjectsFromReader(reader);
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

        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            bool isCreated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AssignEmployeeToProject, conn);
                    cmd.Parameters.AddWithValue("@projectID", projectId);
                    cmd.Parameters.AddWithValue("@employeeID", employeeId);

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

        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            bool isCreated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_RemoveEmployeeFromProject, conn);
                    cmd.Parameters.AddWithValue("@projectID", projectId);
                    cmd.Parameters.AddWithValue("@employeeID", employeeId);

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

        public bool CreateProject(Project newProject)
        {
            bool isCreated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AddProject, conn);
                    cmd.Parameters.AddWithValue("@name", newProject.Name);
                    cmd.Parameters.AddWithValue("@startDate", newProject.StartDate);
                    cmd.Parameters.AddWithValue("@endDate", newProject.EndDate);

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

        private Project GetProjectsFromReader(SqlDataReader reader)
        {
            Project item = new Project();

            item.ProjectId = Convert.ToInt32(reader["project_id"]);
            item.Name = Convert.ToString(reader["name"]);
            item.StartDate = Convert.ToDateTime(reader["from_date"]);
            item.EndDate = Convert.ToDateTime(reader["to_date"]);
            return item;
        }

    }
}
