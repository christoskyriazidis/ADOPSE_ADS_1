using ApiOne.Helpers;
using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models.Admin;
using ApiOne.Models.Admin.Report;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        public IEnumerable<GetReports> GetReports()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select distinct(adid),a.title,(select count(*) from report where adid=r.AdId ) as reportCount from report r join ad a on (r.AdId=a.id)";
                var getReports = conn.Query<GetReports>(sql).ToList();
                return getReports;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<GetReportsByAd> GetReportsByAd(int AdId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select r.Timestamp,r.ReportText,r.CustomerId,a.title,c.username from report r join ad a on (r.AdId=a.id) join customer c on (c.id=r.CustomerId) where r.AdId=@AdId order by r.Timestamp desc";
                var getReports = conn.Query<GetReportsByAd>(sql,new { AdId }).ToList();
                return getReports;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public bool InsertCategory(InsertCategory insertCategory)
        {
            try
            {
                var imageName = insertCategory.Title.Replace(" ", "");
                var ImageUrl = $"https://localhost:44374/images/CategorySubcategory/{imageName}.png";
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "insert into Category (title,imageUrl) values (@Title,@ImageUrl)";
                var chatMessages = conn.Query(sql, new { insertCategory.Title, ImageUrl });
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool InsertSubCategory(InsertSubCategory insertSubCategory)
        {
            try
            {
                var imageName= insertSubCategory.Title.Replace(" ", "");
                var ImageUrl = $"https://localhost:44374/images/CategorySubcategory/{imageName}.png";
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "insert into SubCategory (title,imageUrl,categoryId) values (@Title,@ImageUrl,@CategoryId)";
                var chatMessages = conn.Query(sql, new { insertSubCategory.Title, ImageUrl, insertSubCategory.CategoryId });
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }


    }
}
