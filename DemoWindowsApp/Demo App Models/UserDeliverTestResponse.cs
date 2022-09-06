using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DemoWindowsApp.Demo_App_Models.LoginDataModel;

namespace DemoWindowsApp.Demo_App_Models
{
    public class UserDeliverTestResponse
    {
        public long testId { get; set; }
        public string testTitle { get; set; }
        public string testDescription { get; set; }
        public string durationDisplay { get; set; }
        public short duration { get; set; }
        public long userTestId { get; set; }
        public bool durationapplicable { get; set; }
        public bool examCompleted { get; set; }
        public DescriptionMasterModel preTestMessage { get; set; }
        public string scheduleStatus { get; set; }
        public string scheduleName { get; set; }
        public string scheduleuuId { get; set; }
        public string scheduleDescription { get; set; }
        public string scheduleStartDate { get; set; }
        public string scheduleEndDate { get; set; }
        public string usertestuuId { get; set; }
        public string testbankuuId { get; set; }
        public bool? IsScheduleStarted { get; set; }
        public bool? IsScheduleExpired { get; set; }
        public bool? IsAutoSubmitted { get; set; }
        public DateTime? testStartDate { get; set; }
    }
}
