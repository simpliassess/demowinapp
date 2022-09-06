using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWindowsApp.Demo_App_Models
{
    public class LoginDataModel
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Data
        {
            public string accessToken { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string emailId { get; set; }
            public int clientId { get; set; }
            public int examineeId { get; set; }
        }

        public class Login
        {
            public string message { get; set; }
            public string status { get; set; }
            public Data data { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Error
        {
            public string field { get; set; }
            public string code { get; set; }
            public string message { get; set; }
        }

        public class LoginErrorModel
        {
            public bool success { get; set; }
            public string message { get; set; }
            public Data data { get; set; }
            public List<Error> error { get; set; }
        }

        public class DescriptionMasterModel
        {
            public List<string> mediaURL { get; set; }
            public string description { get; set; }
        }
    }
}
