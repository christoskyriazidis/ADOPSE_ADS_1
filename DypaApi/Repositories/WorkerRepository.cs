using Dapper;
using DypaApi.Helpers;
using DypaApi.Interfaces;
using DypaApi.Models.Users;
using DypaApi.Models.Worker;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Repositories
{
    public class WorkerRepository : IWorker
    {
        public IEnumerable<Worker> GetWorkers()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select * from worker w join users u on (w.id=u.id)";
                var sensors = conn.Query<Worker>(sql).ToList();
                return sensors;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }
        public int GetCustomerIdFromSub(string SubId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT TOP 1 id FROM Users WHERE SubId=@SubId";
                var customers = conn.Query<int>(sql, new { SubId }).FirstOrDefault();
                return customers;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return -1;
            }
        }

        public Owner GetMyProfile(int Uid)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select u.*,ur.title as RoleTitle from users u join UserRole ur on(ur.id=u.Role) where u.id=@Uid";
                var profile = conn.Query<Owner>(sql,new { Uid }).FirstOrDefault();
                return profile;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }
    }
}
