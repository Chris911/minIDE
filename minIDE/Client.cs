using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ideone;

namespace ideone.Client
{
    public class Client
    {
        String username;
        String password;

        //Actual ideone client
        Ideone_ServiceService ideoneClient = new Ideone_ServiceService();

        public Client()
        { 
            //Load username and password from config
            username = "";
            password = "";
        }

        /// <summary>
        /// Create an ideone.com submission.
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="language"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public Submission createSubmission(String sourceCode, int language, String input)
        {
            Object[] ret = ideoneClient.createSubmission(
                this.username, 
                this.password, 
                sourceCode, 
                language, 
                input, 
                true, 
                true);

            Dictionary<string, string> result = this.resultToDict(ret);
            Submission submission = new Submission(result["link"], result["error"]);

            return submission;
        }

        /// <summary>
        /// Get details about a ideone.com submission.
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public void getSubmissionDetails(Submission submission)
        {
            Object[] ret = ideoneClient.getSubmissionDetails(
                this.username, 
                this.password,
                submission.link,
                true, true, true, true, true);

            Dictionary<string, string> result = this.resultToDict(ret);
            submission.addDetails(result);
        }

        /// <summary>
        /// Returns true only if the submission has been compiled and finished running
        /// </summary>
        /// <param name="submission"></param>
        /// <returns></returns>
        public Boolean isSubmissionCompiled(Submission submission)
        {
            Object[] ret = ideoneClient.getSubmissionDetails(
                this.username,
                this.password,
                submission.link,
                false, false, false, false, false);

            Dictionary<string, string> result = this.resultToDict(ret);

            return (true ? result["status"] == "0" : false);
        }
        /// <summary>
        /// Generate a Dictionary from the results in XML
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private Dictionary<string, string> resultToDict(Object[] results)
        {
            Dictionary<string, string> dictResult = new Dictionary<string, string>();
            foreach (object o in results)
            {
                if (o is XmlElement)
                {
                    XmlNodeList x = ((XmlElement)o).ChildNodes;
                    if (!dictResult.ContainsKey(x.Item(0).InnerText))
                        dictResult.Add(x.Item(0).InnerText, x.Item(1).InnerText);
                }
            }

            return dictResult;
        }
    }
}
