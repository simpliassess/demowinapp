using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWindowsApp.Demo_App_Models
{
    public class ErrorClassModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<BaseException> error { get; set; }
    }
    public class BaseException
    {
        public string field { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }
}
