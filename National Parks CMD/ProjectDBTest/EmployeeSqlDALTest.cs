using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectDB.DAL;
using ProjectDB.Models;
using System.Linq;

namespace ProjectDBTest
{
    [TestClass]
    public class EmployeeSqlDALTest
    {

        private int employeeID = 0;
        private TransactionScope tran;
        private static string connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = Company; Integrated Security = True";
        private EmployeeSqlDAL test = new EmployeeSqlDAL(connectionString);

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                cmd = new SqlCommand("Insert into employee (department_id, first_name, last_name, job_title, birth_date, gender, hire_date) values (1, 'test_first', 'test_last', 'cool guy', '1990-01-1', 'M', '2018-01-1')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT CAST(SCOPE_IDENTITY() as int)", conn);
                employeeID = (int)cmd.ExecuteScalar();
            }
        }

        [TestMethod()]
        public void GetEmployeesTest()
        {

            List<Employee> GetEmployees = test.GetAllEmployees();

            Assert.AreEqual(13, GetEmployees.Count);
            Assert.AreEqual(employeeID, GetEmployees[GetEmployees.Count - 1].EmployeeId);

        }

        [TestMethod]
        public void SearchEmployeeTest()
        {
            List<Employee> SearchTest = test.Search("Sid", "Goodman");

            Assert.AreEqual("Sid", SearchTest.First().FirstName);
            Assert.AreEqual("Goodman", SearchTest.First().LastName);
        }


        [TestMethod]
        public void GetEmployeesWithoutProjects()
        {
            List<Employee> NoProjectTest = test.GetEmployeesWithoutProjects();

            Assert.AreEqual(3, NoProjectTest.Count); //because we created someone and didnt assign them a project
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

    }
}
