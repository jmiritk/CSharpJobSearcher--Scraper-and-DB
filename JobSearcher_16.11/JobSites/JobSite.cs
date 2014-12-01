using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearcher_16._11
{
    abstract public class JobSite
    {
        string Url1 = "";
        string Url2 = "";
        public string CompleteSiteURL { get; set; }
        public string SiteName { get; set; }
        public int pageId { get; set; }
        public bool isFirstClick { get; set; }
        public String SpecificJobURL { get; set; }

        abstract public List<Job> readJobs(string i_url);
        abstract public string createSiteURL();
        virtual public string getJobID(string i_stringWithId)
        {
            return null;
        }

       
    }
}
