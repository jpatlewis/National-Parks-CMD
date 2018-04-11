using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using ProjectDB.DAL;
using ProjectDB.Models;
using System.Collections.Generic;

namespace ProjectDBTest
{
    [TestClass]
    public class DepartmentSqlDALTest
    {

        private int departmentID = 0;
        private TransactionScope tran;
        private static string connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = Company; Integrated Security = True";
        private DepartmentSqlDAL test = new DepartmentSqlDAL(connectionString);

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

                using (SqlConnection conn = new SqlConnection(connectionString)) 
                {
                    SqlCommand cmd;

                    conn.Open();

                    cmd = new SqlCommand("Insert into department values ('name')", conn);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("SELECT CAST(SCOPE_IDENTITY() as int)", conn);
                    departmentID = (int)cmd.ExecuteScalar();
                }           
        }

       
        [TestMethod()]
        public void GetDepartmentsTest()
        {

            List<Department> departments = test.GetDepartments();

            Assert.AreEqual(8, departments.Count);
            Assert.AreEqual(departmentID, departments[departments.Count-1].Id);
         
        }

        [TestMethod()]
        public void CreateDepartmentTest()
        {
            Department testdept = new Department();

            testdept.Id = departmentID + 1;

            testdept.Name = "steve";

            Assert.IsTrue(test.CreateDepartment(testdept));

        }

        [TestMethod()]
        public void UpdateDepartmentTest()
        {
            Department testdept = new Department();

            testdept.Id = 3;

            testdept.Name = "steve";

            Assert.IsTrue(test.UpdateDepartment(testdept));

        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }
    }
}
