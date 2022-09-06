using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWindowsApp.Demo_App_Models
{
    public class TDTestLandingProjection
    {

        public long ExamineeId { get; set; }
        public string TestTitle { get; set; }
        public string TestDescription { get; set; }
        public string DurationDisplay { get; set; }
        public short Duration { get; set; }
        public long TestBankId { get; set; }
        public string Pretestmessage { get; set; }
        public long UserTestId { get; set; }
        public bool Isexamcomplete { get; set; }
        public bool Isdurationapplicable { get; set; }
        public string ScheduleStatus_cal { get; set; }
        public string ScheduleName { get; set; }
        public string TestScheduleuuId { get; set; }
        public string Scheduledescription { get; set; }
        public string Schedulestartdate { get; set; }
        public string Scheduleenddate { get; set; }
        public string Testbankuuid { get; set; }
        public string Usertestuuid { get; set; }
        public bool? IsScheduleStarted { get; set; }
        public bool? IsScheduleExpired { get; set; }

    }
}
