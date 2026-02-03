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
    public class UserController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public PartialViewResult Create()
        {
            return PartialView("_CreateUser", new UserModel());
        }

        [HttpPost]
        public JsonResult Create(UserModel model, HttpPostedFileBase image)
        {
            string img = null;
            if (image != null)
            {
                string path = Server.MapPath("~/Uploads");
                Directory.CreateDirectory(path);
                string file = Guid.NewGuid() + Path.GetExtension(image.FileName);
                image.SaveAs(Path.Combine(path, file));
                img = "/Uploads/" + file;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Users VALUES(@U,@E,@M,@C,@I,GETDATE())", con);
                cmd.Parameters.AddWithValue("@U", model.Username);
                cmd.Parameters.AddWithValue("@E", model.Email);
                cmd.Parameters.AddWithValue("@M", model.Mobile);
                cmd.Parameters.AddWithValue("@C", model.Country);
                cmd.Parameters.AddWithValue("@I", (object)img ?? DBNull.Value);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Json(new { success = true });
        }

        public PartialViewResult Edit(int id)
        {
            UserModel u = new UserModel();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    u.Id = id;
                    u.Username = r["Username"].ToString();
                    u.Email = r["Email"].ToString();
                    u.Mobile = r["Mobile"].ToString();
                    u.Country = r["Country"].ToString();
                    u.ImagePath = r["ImagePath"]?.ToString();
                }
            }
            return PartialView("_EditUser", u);
        }

        [HttpPost]
        public JsonResult Edit(UserModel model, HttpPostedFileBase image)
        {
            string img = model.ImagePath;
            if (image != null)
            {
                string path = Server.MapPath("~/Uploads");
                Directory.CreateDirectory(path);
                string file = Guid.NewGuid() + Path.GetExtension(image.FileName);
                image.SaveAs(Path.Combine(path, file));
                img = "/Uploads/" + file;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Users SET Username=@U,Email=@E,Mobile=@M,Country=@C,ImagePath=@I WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@U", model.Username);
                cmd.Parameters.AddWithValue("@E", model.Email);
                cmd.Parameters.AddWithValue("@M", model.Mobile);
                cmd.Parameters.AddWithValue("@C", model.Country);
                cmd.Parameters.AddWithValue("@I", (object)img ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Json(new { success = true });
        }
    }

}
