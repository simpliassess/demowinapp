using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWindowsApp.Demo_App_Models
{
    public class UserTestDetailsModel
    {
        public long Usertestid { get; set; }
        public long Testbankid { get; set; }
        public int? Collectionid { get; set; }
        public int? Clientid { get; set; }
        public decimal? Totalscore { get; set; }
        public short? Totalcorrectanswers { get; set; }
        public short? Totalincorrectanswers { get; set; }
        public bool Isexamcomplete { get; set; }
        public int? Finshtesttime { get; set; }
        public DateTime? Finshtestdatetime { get; set; }
        public bool Isstartedtest { get; set; }
        public DateTime? Starttestdatetime { get; set; }
        public string Userfeedback { get; set; }
        public short? Reattemptcount { get; set; }
        public bool Isexaminervisited { get; set; }
        public string Examinercomments { get; set; }
        public long? Examinerid { get; set; }
        public int? Consumedtesttime { get; set; }
        public short? Totalattemptedques { get; set; }
        public short? Totalunattemptedques { get; set; }
        public short? Totalquesontest { get; set; }
        public decimal? Totalpositivescore { get; set; }
        public decimal? Totalnegativescore { get; set; }
        public string Testresult { get; set; }
        public string Usertestuuid { get; set; }
        public long? Testscheduleid { get; set; }
        public long? Examineeid { get; set; }
        public long? Createduserid { get; set; }
        public DateTime? Createddatetime { get; set; }
        public long? Modifieduserid { get; set; }
        public DateTime? Modifieddatetime { get; set; }
    }
}
