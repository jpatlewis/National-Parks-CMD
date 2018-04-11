using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectDB.DAL;
using ProjectDB.Models;

namespace ProjectDBTest
{
    [TestClass]
    public class ProjectSqlDALTest
    {
        private int ProjectID = 0;
        private TransactionScope tran;
        private static string connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = Company; Integrated Security = True";
        private ProjectSqlDAL test = new ProjectSqlDAL(connectionString);

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                cmd = new SqlCommand("Insert into project values ('steve', '1980-12-25', '2018-01-1')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT CAST(SCOPE_IDENTITY() as int)", conn);
                ProjectID = (int)cmd.ExecuteScalar();
            }
        }

        [TestMethod()]
        public void GetProjectTest()
        {
            List<Project> GetProject = test.GetAllProjects();

            Assert.AreEqual(8, GetProject.Count);
        }

        [TestMethod()]
        public void AssignEmployeeToProjectTest()
        {
            Assert.IsTrue(test.AssignEmployeeToProject(2, 7));
        }

        [TestMethod()]
        public void UnassignEmployeeFromProjectTest()
        {
            Assert.IsTrue(test.RemoveEmployeeFromProject(4, 6));
        }

        [TestMethod()]
        public void CreateProjectTest()
        {
            Project testProject = new Project();

            testProject.Name = "Gary";

            testProject.StartDate = new DateTime(1900, 01, 01);

            testProject.EndDate = new DateTime(2000, 01, 01);

            Assert.IsTrue(test.CreateProject(testProject));
        }



        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

    }
}
