using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minIDE
{
    /// <summary>
    /// Container for a ideone submission
    /// </summary>
    public class Submission
    {
        public String Link { get; set; }
        public String ErrorCode { get; set; }
        public String LanguageId { get; set; }
        public String LanguageName { get; set; } 
        public String LanguageVersion { get; set; }
        public String ExecutionTime { get; set; }
        public String Date { get; set; }
        public String Status { get; set; }
        public String Result { get; set; }
        public String Memory { get; set; }
        public String Signal { get; set; }
        public Boolean IsPublic { get; set; }
        public String SourceCode { get; set; }
        public String Input { get; set; }
        public String Output { get; set; }
        public String StdError { get; set; }
        public String CompilationInfo { get; set; }

        private String _subText;

        public Submission()
        { }

        public Submission(String link, String errorCode)
        {
            this.ErrorCode = errorCode;
            this.Link = link;
        }

        public void AddDetails(Dictionary<string, string> result)
        {
            this.LanguageId = result["langId"];
            this.LanguageName = result["langName"];
            this.LanguageVersion = result["langVersion"];
            this.ExecutionTime = result["time"];
            this.Date = result["date"];
            this.Status = result["status"];
            this.Result = result["result"];
            this.Memory = result["memory"];
            this.Signal = result["signal"];
            this.IsPublic = (true && result["public"] == "true");
            this.SourceCode = result["source"];
            this.Input = result["input"];
            this.Output = result["output"];
            this.StdError = result["stderr"];
            this.CompilationInfo = result["cmpinfo"];

            // HACK: Shouldn't be called in addDetails but always after.. 
            this.BuildSubmissionText(result); 
        }

        private void BuildSubmissionText(Dictionary<string, string> result)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Link: http://ideone.com/" + this.Link + "\n");
            foreach (KeyValuePair<string, string> kvp in result)
            {
                builder.Append(kvp.Key + " : " + kvp.Value + "\n");
            }
            this._subText = builder.ToString();
        }
    
        public override string ToString()
        {
            return _subText;
        }

        public Boolean IsValid()
        {
            return (true && this.ErrorCode == "OK");
        }
            

    }
}
