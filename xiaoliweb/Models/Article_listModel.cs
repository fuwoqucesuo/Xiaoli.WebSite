using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xiaoliweb.Models
{
    public class Article_listModel
    {
        public int PKID { get; set; }
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Content { get; set; }
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public int Sort { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}