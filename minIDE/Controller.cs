using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace minIDE
{
    class Controller
    {
        // Should be placed in configuration file
        String ideOneUser     = "";
        String ideOnePassword = "";

        public String sendCodeToServer(String sourceCode, int language)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Ideone_ServiceService client = new Ideone_ServiceService();
            Object[] ret = client.createSubmission(ideOneUser, ideOnePassword, sourceCode, language, null, true, true);
            
            foreach (object o in ret)
            {
                if (o is XmlElement)
                {
                    XmlNodeList x = ((XmlElement)o).ChildNodes;
                    result.Add(x.Item(0).InnerText, x.Item(1).InnerText);
                }
            }

            Object[] retDet = client.getSubmissionDetails(ideOneUser, ideOnePassword, result["link"],
                false, true, true, true, true);

            foreach (object o in retDet)
            {
                if (o is XmlElement)
                {
                    XmlNodeList x = ((XmlElement)o).ChildNodes;
                    if(!result.ContainsKey(x.Item(0).InnerText))
                        result.Add(x.Item(0).InnerText, x.Item(1).InnerText);
                }
            }

            return buildOutputMessage(result);
        }

        // Should be moved out of this class
        private String buildOutputMessage(Dictionary<string, string> result)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in result)
            {
                builder.Append(kvp.Key + " : " + kvp.Value + "\n");
            }

            return builder.ToString();
        }
    }
}
