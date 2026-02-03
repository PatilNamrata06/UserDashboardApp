using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserDashboardApp.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}