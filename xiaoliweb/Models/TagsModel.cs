using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace xiaoliweb.Models
{
    public class TagsModel : BaseModel
    {
        public TagsModel()
        {
        }
        public TagsModel(DataRow row):base(row) {
            base.Parse(row);
        }
        public int PKID { get; set; }
        public int TagType { get; set; }
        public bool TagStatus { get; set; }
        public string TagUrl { get; set; }
    }
}