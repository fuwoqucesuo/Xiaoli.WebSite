using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xiaoliweb.DBhelper;
using System.Data;
using System.Data.SqlClient;
using xiaoliweb.Models;

namespace xiaoliweb.DAL
{
    public static class ArticleDAL
    {
        public static int InsertArticleList(Article_listModel am)
        {
            using (var cmd = new SqlCommand(@"INSERT  INTO Article_list
            ( ArticleID ,
              Title ,
              [Desc] ,
              Content ,
              Code ,
              ImageUrl ,
              Sort ,
              CreateDate ,
              UpdateDate
            )
    VALUES  ( @ArticleID ,
              @Title ,
              @Desc ,
              @Content ,
              @Code ,
              @ImageUrl ,
              @Sort ,
              @CreateDate ,
              @UpdateDate
            );"))
            {
                cmd.Parameters.AddWithValue("@ArticleID", am.ArticleID);
                cmd.Parameters.AddWithValue("@Title", am.Title);
                cmd.Parameters.AddWithValue("@Desc", am.Desc);
                cmd.Parameters.AddWithValue("@Content", am.Content);
                cmd.Parameters.AddWithValue("@Code", am.Code);
                cmd.Parameters.AddWithValue("@ImageUrl", am.ImageUrl);
                cmd.Parameters.AddWithValue("@Sort", am.Sort);
                cmd.Parameters.AddWithValue("@CreateDate", am.CreateDate);
                cmd.Parameters.AddWithValue("@UpdateDate", am.UpdateDate);
                return DbHelper.ExecuteNonQuery(cmd);
            }
        }

        public static int InsertArticle(ArticleModel article)
        {
            using (var cmd = new SqlCommand(@"
                INSERT  INTO [dbo].[Article]
                        ( Title ,
                          [Desc] ,
                          CreateDate ,
                          UpdateDate ,
                          [Status]
	                    )
                VALUES  ( @Title ,
                          @Desc ,
                          @CreateDate ,
                          @UpdateDate ,
                          @Status
	                    );
	            SELECT @@IDENTITY
            "))
            {
                cmd.Parameters.AddWithValue("@Title", article.Title);
                cmd.Parameters.AddWithValue("@Desc", article.Desc);
                cmd.Parameters.AddWithValue("@CreateDate", article.CreateDate);
                cmd.Parameters.AddWithValue("@UpdateDate", article.UpdateDate);
                cmd.Parameters.AddWithValue("@Status", article.Status);
                return DbHelper.ExecuteNonQuery(cmd);
            }
        }
    }
}