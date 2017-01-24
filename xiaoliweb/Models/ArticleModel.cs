using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xiaoliweb.Models
{
    public  class ArticleModel
    {

        public int PKID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}