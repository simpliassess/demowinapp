using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWindowsApp.Demo_App_Models
{
    public class TestItemDetailsResponseProjection
    {
        public long itemId { get; set; }
        public short displayOrder { get; set; }
        public string itemBankName { get; set; }
        public long totalQuestions { get; set; }
        public string itemuuId { get; set; }
    }
}
