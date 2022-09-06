using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWindowsApp.Demo_App_Models
{
    public class TestDeliveryResponseEntity
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object Data { get; set; }
        public object error { get; set; }
    }
}
