using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace xiaoliweb.Models
{
    public class UserObjectModel: BaseModel
    {
        public UserObjectModel()
        {
        }
        public UserObjectModel(DataRow row):base(row) {
            base.Parse(row);
        }
        public int PKID { get; set; }
        public string UserName { get; set; }
        public string UserPassWord { get; set; }
        public string Sex { get; set; }
        public string UserEmail { get; set; }
        public DateTime UserBirthday { get; set; }
        public string UserMobile { get; set; }
        public string HeadImage { get; set; }
        public int UserLevel { get; set; }
    }
}