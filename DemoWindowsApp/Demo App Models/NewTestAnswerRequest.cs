using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWindowsApp.Demo_App_Models
{
    public class NewTestAnswerRequest
    {
        public string testuuId { get; set; }
        public String userTestuuId { get; set; }
        public List<NewAnswers> answers { get; set; } = new List<NewAnswers>();
    }
    public class NewAnswers
    {
        public string questionuuId { get; set; }
        public List<short> optionId { get; set; }

    }
}
