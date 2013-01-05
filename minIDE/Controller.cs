using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ideone.Client;
using ideone;

namespace minIDE
{
    /// <summary>
    /// In charge of the transactions between the view and the backend processing
    /// </summary>
    class Controller
    {
        Client ideoneClient = new Client();

        /// <summary>
        /// Send submission to ideone servers and return information
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="input"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public String sendSubmission(String sourceCode, String input, int language)
        {
            // TODO: Make the submission process async / threaded
            Submission submission = ideoneClient.createSubmission(sourceCode, language, input);

            while (!ideoneClient.isSubmissionCompiled(submission))
            {
                // HACK: Should rather be done using a Timer or other mechanism
                // TODO: Progress bar with changing states (See ideone API doc)
                System.Threading.Thread.Sleep(1500);
            }

            ideoneClient.getSubmissionDetails(submission);

            return submission.ToString() ;
        }

    }
}
