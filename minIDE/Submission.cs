using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ideone
{
    /// <summary>
    /// Container for a ideone submission
    /// </summary>
    public class Submission
    {
        public String link { get; set; }
        public String errorCode { get; set; }
        public String languageId { get; set; }
        public String languageName { get; set; } 
        public String languageVersion { get; set; }
        public String executionTime { get; set; }
        public String date { get; set; }
        public String status { get; set; }
        public String result { get; set; }
        public String memory { get; set; }
        public String signal { get; set; }
        public Boolean isPublic { get; set; }
        public String sourceCode { get; set; }
        public String input { get; set; }
        public String output { get; set; }
        public String stdError { get; set; }
        public String compilationInfo { get; set; }

        private String subText;

        public Submission()
        { }

        public Submission(String link, String errorCode)
        {
            this.errorCode = errorCode;
            this.link = link;
        }

        public void addDetails(Dictionary<string, string> result)
        {
            this.languageId = result["langId"];
            this.languageName = result["langName"];
            this.languageVersion = result["langVersion"];
            this.executionTime = result["time"];
            this.date = result["date"];
            this.status = result["status"];
            this.result = result["result"];
            this.memory = result["memory"];
            this.signal = result["signal"];
            this.isPublic = (true ? result["public"] == "true" : false);
            this.sourceCode = result["source"];
            this.input = result["input"];
            this.output = result["output"];
            this.stdError = result["stderr"];
            this.compilationInfo = result["cmpinfo"];

            // HACK: Shouldn't be called in addDetails but always after.. 
            this.buildSubmissionText(result); 
        }

        private void buildSubmissionText(Dictionary<string, string> result)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Link: http://ideone.com/" + this.link + "\n");
            foreach (KeyValuePair<string, string> kvp in result)
            {
                builder.Append(kvp.Key + " : " + kvp.Value + "\n");
            }
            this.subText = builder.ToString();
        }
    
        public override string ToString()
        {
            return subText;
        }
    }
}
