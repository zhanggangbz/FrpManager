using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrpClient
{
    public class FrpSettingModel
    {
        public String ID { get; set; }

        public String Name { get; set; }

        public String ConfigFile { get; set; }

        public String LogFile { get; set; }

        public bool AutoRun { get; set; }

        public FrpSettingModel()
        {
            ID = Guid.NewGuid().ToString("N");

            Name = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            ConfigFile = "";

            LogFile = "";

            AutoRun = false;
        }

        public FrpSettingModel(string name,string cfile,string lfile,bool autorun)
        {
            ID = Guid.NewGuid().ToString("N");

            Name = name;

            ConfigFile = cfile;

            LogFile = lfile;

            AutoRun = autorun;
        }
    }
}
