using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
namespace xiaoliweb.DAL
{
    public class HomeDAL
    {
        public static DataTable FetchTags() {
            string sql = "SELECT * FROM bds26091016_db.dbo.Tags WITH(NOLOCK)";
            return DBhelper.DBhelp.GetDataTableNotPara(sql);
        }
    }
}