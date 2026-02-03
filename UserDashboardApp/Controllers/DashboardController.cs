using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using UserDashboardApp.Models;

namespace UserDashboardApp.Controllers
{
    public class DashboardController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ActionResult Index()
        {
            List<UserModel> list = new List<UserModel>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users ORDER BY CreatedAt DESC", con);
                con.Open();
                var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    list.Add(new UserModel
                    {
                        Id = (int)r["Id"],
                        Username = r["Username"].ToString(),
                        Email = r["Email"].ToString(),
                        Mobile = r["Mobile"].ToString(),
                        Country = r["Country"].ToString(),
                        ImagePath = r["ImagePath"]?.ToString()
                    });
                }
            }
            return View(list);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Json(new { success = true });
        }
    }

}
