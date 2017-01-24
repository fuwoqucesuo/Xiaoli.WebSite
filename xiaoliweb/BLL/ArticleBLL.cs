using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xiaoliweb.Models;
namespace xiaoliweb.BLL
{
    public static class ArticleBLL
    {
        public static int InsertArticleList(Article_listModel am) {
            if (am.CreateDate == null)
                am.CreateDate = DateTime.Now;
            return DAL.ArticleDAL.InsertArticleList(am); 
        }
    }
}