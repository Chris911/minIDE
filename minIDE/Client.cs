using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml;

namespace minIDE
{
    public class Client
    {
        // Set/get are here for configuration at runtime
        public String Username { get; set; }
        public String Password { get; set; }

        //Actual ideone client
        private Ideone_ServiceService _ideoneClient = new Ideone_ServiceService();

        public Client()
        { 
            //Load username and password from config
            Username = "";
            Password = "";

        }

        /// <summary>
        /// Create an ideone.com submission.
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="language"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public Submission CreateSubmission(String sourceCode, int language, String input)
        {
            Object[] ret = _ideoneClient.createSubmission(
                this.Username,
                this.Password, 
                sourceCode, 
                language, 
                input, 
                true, 
                true);

            Dictionary<string, string> result = ResultToDict(ret);
            Submission submission = new Submission {ErrorCode = result["error"]};
            if (submission.ErrorCode == "OK")
            {
                submission.Link = result["link"];
            }
            else
            {
                ThrowClientError(result["error"]);
            }

            return submission;
        }

        /// <summary>
        /// Get details about a ideone.com submission.
        /// </summary>
        /// <param name="submission"></param>
        /// <returns></returns>
        public void GetSubmissionDetails(Submission submission)
        {
            Object[] ret = _ideoneClient.getSubmissionDetails(
                this.Username,
                this.Password,
                submission.Link,
                true, true, true, true, true);

            Dictionary<string, string> result = ResultToDict(ret);
            submission.AddDetails(result);
        }

        /// <summary>
        /// Returns true only if the submission has been compiled and finished running
        /// </summary>
        /// <param name="submission"></param>
        /// <returns></returns>
        public Boolean IsSubmissionCompiled(Submission submission)
        {
            Object[] ret = _ideoneClient.getSubmissionDetails(
                this.Username,
                this.Password,
                submission.Link,
                false, false, false, false, false);

            Dictionary<string, string> result = ResultToDict(ret);

            return (true && result["status"] == "0");
        }


        /// <summary>
        /// Generate a Dictionary from the results in XML
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private static Dictionary<string, string> ResultToDict(IEnumerable<object> results)
        {
            // TODO: At the moment we return null if there's an error. Might be better to throw exception
            if (results == null) return null;

            var dictResult = new Dictionary<string, string>();
            try
            {
                foreach (XmlNodeList x in results
                    .OfType<XmlElement>()
                    .Select(o => (o).ChildNodes)
                    .Where(x => !dictResult.ContainsKey(x.Item(0).InnerText)))
                {
                    dictResult.Add(x.Item(0).InnerText, x.Item(1).InnerText);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return dictResult;
        }

        public Dictionary<string, int> LoadLanguages()
        {
            var languages = _ideoneClient.getLanguages(this.Username, this.Password);
            if (languages == null)
            {
                ThrowClientError("Error loading languages list");
                return null;
            }
            if (languages[2] is XmlElement)
            {
                var langDict = new Dictionary<string, int>();
                try
                {
                    XmlNodeList langNodeList = ((XmlElement) languages[2]).ChildNodes.Item(1).ChildNodes;
                    foreach (XmlElement x in langNodeList)
                    {
                        string langName = x.ChildNodes.Item(1).InnerText;
                        int langId = Convert.ToInt32(x.ChildNodes.Item(0).InnerText);
                        langDict.Add(langName, langId);
                    }
                }
                catch (Exception e)
                {
                    ThrowClientError("Error loading languages list");
                }

                return langDict;
            }
            return null;
        }

        /// <summary>
        /// Throw error message related to the ideone client
        /// </summary>
        /// <param name="error"></param>
        private static void ThrowClientError(String error)
        {
            // TODO: At some point this will have to be moved into a resource file
            if (error == "AUTH_ERROR")
                error = "There was an error logging in to the ideone.com API. Make sure your credentials are valid.";
            else if (error == "PASTE_NOT_FOUND")
                error = "The paste ID submitted is invalid.";
            else if (error == "WRONG_LANG_ID")
                error = "Unknown language selected.";
            else if (error == "ACCESS_DENIED")
                error = "You do not have access to this paste.";
            else if (error == "CANNOT_SUBMIT_THIS_MONTH_ANYMORE")
                error = "You reached your monthly limit. See http://ideone.com/offer/users for details)";

            MessageBox.Show(error, "Ideone Client Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
