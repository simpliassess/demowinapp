using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static DemoWindowsApp.Demo_App_Models.LoginDataModel;

namespace DemoWindowsApp.Demo_App_Models
{
    public class ScoreDto
    {
        public short totalCorrectAnswer { get; set; }
        public short totalIncorrect { get; set; }
        public decimal totalScore { get; set; }
        public short totalAttempted { get; set; }
        public short totalTestQuestions { get; set; }
        public short unAttemptedQuestions { get; set; }
        public decimal totalPositiveScore { get; set; }
        public decimal totalNegativeScore { get; set; }
        public DescriptionMasterModel postTestMessage { get; set; }

        public bool? submitStatus { get; set; }
        [JsonIgnore]
        private HashSet<long> correctAns { get; set; } = new HashSet<long>();
        [JsonIgnore]
        private HashSet<long> inCorrectAns { get; set; } = new HashSet<long>();

        public short getTotalAttempted()
        {

            return totalAttempted;
        }

        public void setTotalAttempted(short totalAttempted)
        {
            this.totalAttempted = totalAttempted;
        }

        public short getTotalCorrectAnswer()
        {
            return (short)correctAns.Count();
        }

        public short getTotalIncorrect()
        {
            return (short)inCorrectAns.Count;
        }

        public decimal getTotalScore()
        {
            return totalScore;
        }

        public void setTotalScore(decimal totalScore)
        {
            this.totalScore = totalScore;
        }

        public HashSet<long> getCorrectAns()
        {
            return correctAns;
        }

        public void setCorrectAns(HashSet<long> correctAns)
        {
            this.correctAns = correctAns;
        }

        public HashSet<long> getInCorrectAns()
        {
            return inCorrectAns;
        }

        public void setInCorrectAns(HashSet<long> inCorrectAns)
        {
            this.inCorrectAns = inCorrectAns;
        }

        public short getTotalTestQuestions()
        {
            return totalTestQuestions;
        }

        public void setTotalTestQuestions(short totalTestQuestions)
        {
            this.totalTestQuestions = totalTestQuestions;
        }

        public short getUnAttemptedQuestions()
        {
            return unAttemptedQuestions;
        }

        public void setUnAttemptedQuestions(short unAttemptedQuestions)
        {
            this.unAttemptedQuestions = unAttemptedQuestions;
        }



        public void setTotalCorrectAnswer(short totalCorrectAnswer)
        {
            this.totalCorrectAnswer = totalCorrectAnswer;
        }

        public void setTotalIncorrect(short totalIncorrect)
        {
            this.totalIncorrect = totalIncorrect;
        }

        public decimal getTotalPositiveScore()
        {
            return totalPositiveScore;
        }

        public void setTotalPositiveScore(decimal totalPositiveScore)
        {
            this.totalPositiveScore = totalPositiveScore;
        }

        public decimal getTotalNegativeScore()
        {
            return totalNegativeScore;
        }

        public void setTotalNegativeScore(decimal totalNegativeScore)
        {
            this.totalNegativeScore = totalNegativeScore;
        }

        public DescriptionMasterModel getPostTestMessage()
        {
            return postTestMessage;
        }

        public void setPostTestMessage(DescriptionMasterModel postTestMessage)
        {
            this.postTestMessage = postTestMessage;
        }



    }
}
