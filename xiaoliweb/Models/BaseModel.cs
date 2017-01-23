using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace xiaoliweb.Models
{
    public class BaseModel
    {
        protected BaseModel() { }
        protected BaseModel(DataRow row)
        {
            Parse(row);
        }

         protected virtual void Parse(DataRow row)
        {
            if (row == null)
                return;
        }


    }
}