using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace minIDE
{
    /// <summary>
    /// In charge of the transactions between the view and the backend processing
    /// </summary>
    class Controller
    {
        Client _ideoneClient = new Client();

        // HACK: Not sure if this should be here
        public Dictionary<string, int> LanguageToIdMap;

        public Controller()
        {;
            LanguageToIdMap = _ideoneClient.LoadLanguages();
        }

        /// <summary>
        /// Send submission to ideone servers and return information
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="input"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public String SendSubmission(String sourceCode, String input, int language)
        {
            // TODO: Make the submission process async / threaded
            Submission submission = _ideoneClient.CreateSubmission(sourceCode, language, input);
            
            if (! submission.IsValid())
                return submission.ErrorCode;

            while (!_ideoneClient.IsSubmissionCompiled(submission))
            {
                // HACK: Should rather be done using a Timer or other mechanism
                // TODO: Progress bar with changing states (See ideone API doc)
                System.Threading.Thread.Sleep(1500);
            }

            _ideoneClient.GetSubmissionDetails(submission);

            return submission.ToString() ;
        }

        public List<String> GetLanguagesList()
        {
            return LanguageToIdMap.Keys.ToList();
        } 

    }
}
