using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWindowsApp.Demo_App_Models
{
    public class TestItemDetailsResponse
    {
        public long totalQuestionCount { get; set; }
        public List<TestItemDetailsResponseProjection> items { get; set; } = new List<TestItemDetailsResponseProjection>();
    }
}
