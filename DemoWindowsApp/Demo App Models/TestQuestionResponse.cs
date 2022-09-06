using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static DemoWindowsApp.Demo_App_Models.LoginDataModel;

namespace DemoWindowsApp.Demo_App_Models
{
    public class TestQuestionResponse
    {
        public string questionuuId { get; set; }
        public bool? shuffleOption { get; set; }
        public short ansRenderCount { get; set; }
        public short displayOrder { get; set; }
        public DescriptionMasterModel questiondescription { get; set; }
        public string questionType { get; set; }
        public string questionSubType { get; set; }
        [JsonPropertyName("attempted")]
        public bool attempted { get; set; }
        public List<OptionResponse> options { get; set; } = new List<OptionResponse>();

    }
    public class OptionResponse
    {
        public short optiId { get; set; }
        public short arrangeOrder { get; set; }
        public DescriptionMasterModel answerDescription { get; set; }
        //[JsonPropertyName("selected")]
        public bool selected { get; set; }
        public long existingId { get; set; }

    }

}
