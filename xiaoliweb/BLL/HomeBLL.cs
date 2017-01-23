using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xiaoliweb.DAL;
using xiaoliweb.Models;
namespace xiaoliweb.BLL
{
    public class HomeBLL
    {
        public  static List<TagsModel> FetchTags() {
            var dt = HomeDAL.FetchTags();
            if (dt != null && dt.Rows.Count > 0) 
            {
                return DBhelper.DataHelper.ConvertTo<TagsModel>(dt).ToList();
            }
            else {
                return null;
            }
        }
    }
}