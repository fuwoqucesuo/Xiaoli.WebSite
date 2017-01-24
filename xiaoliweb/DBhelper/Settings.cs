using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace xiaoliweb.DBhelper
{
    public class Settings
    {
        protected static string sm;
        static Settings()
        {
            var sf = AppDomain.CurrentDomain.BaseDirectory + "schema.config";
            if (File.Exists(sf))
            {
                sm = File.ReadAllText(sf);
            }
            else
            {
                sm = "release";
            }
        }
        public static T Load<T>(string name = null) where T : class
        {
            const string ext = ".cfg";
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = typeof(T).Name;
                }
                if (!name.EndsWith(ext))
                {
                    name += ext;
                }

                var f = AppDomain.CurrentDomain.BaseDirectory + "configs\\" + sm + "\\" + name;
                if (File.Exists(f))
                {
                    var s = File.ReadAllText(f);
                    var jss = new JavaScriptSerializer();
                    var r = jss.Deserialize<T>(s);
                    return r;
                }
                return default(T);
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine(ex.ToString());
                }
                return default(T);
            }
        }
    }
}