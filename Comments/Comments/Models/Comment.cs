using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Comments.Models
{
    public class Comment : User
    {
        public int Id { get; set; }
        public DateTime? DateComment { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int? Parent_Id { get; set; }
        public string LVL { get; set; }

    }
}