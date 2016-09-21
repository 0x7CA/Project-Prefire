using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ProjectPrefire
{
    public class Logger
    {
        public Logger() { }

        public void writeLog(string l)
        {
            ListBox log = Application.OpenForms[0].Controls.Find("log", true).FirstOrDefault() as ListBox;
            log.Items.Add(l);
        }
    }
}
