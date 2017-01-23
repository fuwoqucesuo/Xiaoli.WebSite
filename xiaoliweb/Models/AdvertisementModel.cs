using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace xiaoliweb.Models
{
    public class AdvertisementModel: BaseModel
    {
        public AdvertisementModel()
        {
        }
        public AdvertisementModel(DataRow row):base(row) {
            base.Parse(row);
        }
        public int PKID { get; set; }
        public string Describe { get; set; }
        public string Url { get; set; }
        public int SortType { get; set; }
        public bool AdvertisementStatus { get; set; }
    }
}